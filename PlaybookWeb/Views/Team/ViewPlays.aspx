<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  <%=Model.Name %> - Playbook
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <script type="text/javascript" src="/Scripts/canvas_player-1.0.js"></script>
  <script type="text/javascript" src="/Scripts/excanvas.compiled.js"></script>
  <script type="text/javascript" src="/Scripts/jquery.getURLParam.js"></script>
  <script type="text/javascript" src="/Scripts/jquery.watermark.min.js"></script>
  <script type="text/javascript" src="/Scripts/jquery.BlockUI.js"></script>

  <script type="text/javascript">
    var mCanvasPlayer = null;
    var mCanvas = null;

    $(document).ready(function () 
    {
      $("#PlayGroupAccordion").accordion({
        create: function(event, ui) {
          $("span.DeletePlayIcon").hide();
          $("span.EditPlayIcon").hide();
          $("span.DeletePlayIcon:first").show();
          $("span.EditPlayIcon:first").show();
        },
        changestart: function(event, ui) {
          ui.newHeader.find(".DeletePlayIcon").show();
          ui.newHeader.find(".EditPlayIcon").show();
          ui.oldHeader.find(".DeletePlayIcon").hide();
          ui.oldHeader.find(".EditPlayIcon").hide();
        }
      });

      mCanvas = jQuery("#PlayCanvas")[0];

      $("#NewPlayGroupNameInput").watermark("New group name");

      loadXmlPlay(null);
    });

    //------------------------------------------------------------------------
    // Loads the play and creates the new canvas player if a play was returned
    // from the server.
    //------------------------------------------------------------------------
    function loadXmlPlay(id) {
      if (id != null) {
        $.blockUI();
        
        $.ajax({
          type: "POST",
          url: "/Team/Play",
          dataType: "xml",
          data: "PlayId=" + id,
          success: function (data, textStatus, XMLHttpRequest) {
            xmlDoc = XMLHttpRequest.responseText;

            try {
              mCanvasPlayer = new CanvasPlayer(mCanvas, $.parseXML(xmlDoc));

              // Draws the first frame or the pitch if the play didn't exist.
              drawFirstFrame(mCanvasPlayer);
            } catch (e) {
              alert("Failed to load play");
            } finally {
              $.unblockUI();
            }
          },
          error: function(XMLHttpRequest, textStatus, errorThrown) {
            $.unblockUI();
          }
        });
      } else {
        xmlDoc = null;

        mCanvasPlayer = new CanvasPlayer(mCanvas, xmlDoc);

        // Draws the first frame or the pitch if the play didn't exist.
        drawFirstFrame(mCanvasPlayer);
      }
    }
        
    //------------------------------------------------------------------------
    // Called whenever the play button is clicked. Initiates playback of the
    // xml.                                                      
    //------------------------------------------------------------------------
    function playPauseClickHandler() {
      if ($("#PlayPauseButton").hasClass("PlayButton")) {
        $("#PlayPauseButton").removeClass("PlayButton").addClass("PauseButton");

        play(mCanvasPlayer, ResetPlayer);
      } else {
        $("#PlayPauseButton").removeClass("PauseButton").addClass("PlayButton");

        pause(mCanvasPlayer);
      }
    }

    function ResetPlayer() {
      $("#PlayPauseButton").removeClass("PauseButton").addClass("PlayButton");
      stop(mCanvasPlayer);
      drawFirstFrame(mCanvasPlayer);
    }

    function ToggleNewGroupForm() {
      $("#NewGroupForm").toggle();
    }

    $(function () {
      var tips = $(".validateTips");

      function updateTips(t) {
        tips.text(t).addClass("ui-state-highlight");
        tips.show();
        setTimeout(function () {
          tips.removeClass("ui-state-highlight", 1500);
        }, 500);
      }

      function checkLength(o, n, min, max) {
        if (o.val().length > max || o.val().length < min) {
          o.addClass("ui-state-error");
          updateTips("Length of " + n + " must be between " +
          min + " and " + max + ".");
          return false;
        } else {
          return true;
        }
      }

      function checkRegexp(o, regexp, n) {
        if (!(regexp.test(o.val()))) {
          o.addClass("ui-state-error");
          updateTips(n);
          return false;
        } else {
          return true;
        }
      }

      $("#AddPlayFormDialog").dialog({
        autoOpen: false,
        resizable: false,
        height: 300,
        width: 350,
        modal: true,
        buttons: {
          "Create Play": function () {
            var bValid = true;
            $("#AddPlayPlayName").removeClass("ui-state-error");

            bValid = bValid && checkLength($("#AddPlayPlayName"), "name", 3, 50);

            bValid = bValid && checkRegexp($("#AddPlayPlayName"), /^[a-z]([0-9a-z_])+$/i, "Play name may consist of a-z, 0-9, underscores, and must begin with a letter.");

            if (bValid) {
              $("form#AddPlayForm").submit();
              $(this).dialog("close");
            }
          },
          Cancel: function () {
            $(this).dialog("close");
          }
        },
        close: function () {
          $("#AddPlayPlayName").val("").removeClass("ui-state-error");
        },
        open: function () {
          tips.hide();
        }
      });

      $("#EditPlayFormDialog").dialog({
        autoOpen: false,
        resizable: false,
        height: 300,
        width: 350,
        modal: true,
        buttons: {
          "Edit Play": function () {
            var bValid = true;
            $("#EditPlayPlayName").removeClass("ui-state-error");

            bValid = bValid && checkLength($("#EditPlayPlayName"), "name", 3, 50);

            bValid = bValid && checkRegexp($("#EditPlayPlayName"), /^[a-z]([0-9a-z_])+$/i, "Play name may consist of a-z, 0-9, underscores, and must begin with a letter.");

            if (bValid) {
              $("form#EditPlayForm").submit();
              $(this).dialog("close");
            }
          },
          Cancel: function () {
            $(this).dialog("close");
          }
        },
        close: function () {
          $("#EditPlayPlayName").val("").removeClass("ui-state-error");
        },
        open: function () {
          tips.hide();
        }
      });

      $("#DeletePlayConfirmDialog").dialog({
        autoOpen: false,
        resizable: false,
        height: 180,
        modal: true,
        buttons: {
          "Delete Play": function() {
            window.location = "/Team/DeletePlay?PlayId=" + $("#DeletePlayId").val();
          },
          Cancel: function() {
            $(this).dialog("close");
          }
        }
      });

      $("#DeletePlayGroupConfirmDialog").dialog({
        autoOpen: false,
        resizable: false,
        height: 180,
        modal: true,
        buttons: {
          "Delete Group": function() {
            window.location = "/Team/DeletePlayGroup?PlayGroupId=" + $("#DeletePlayGroupId").val();
          },
          Cancel: function() {
            $(this).dialog("close");
          }
        }
      });
      
      $("#EditPlayGroupDialog").dialog({
        autoOpen: false,
        resizable: false,
        height: 220,
        modal: true,
        buttons: {
            "Save Changes" : function() {
                $("#EditPlayGroupForm").submit();
                $(this).dialog("close");
            },
            Cancel: function() {
                $(this).dialog("close");
            }
        }
      });
    });

    function MaximiseCanvas() {
      jQuery("#PlayCanvas")[0].width = window.innerWidth;
      jQuery("#PlayCanvas")[0].height = window.innerHeight;
    }

    function DeletePlay(id) {
      $("#DeletePlayId").val(id);
      $("#DeletePlayConfirmDialog").dialog("open");
    }

    function LoadPlay(id) {
      loadXmlPlay(id);
    }

    function DeletePlayGroup(id) {
      $("#DeletePlayGroupId").val(id);
      $("#DeletePlayGroupConfirmDialog").dialog("open");
    }

    function AddPlay(playGroupId) {
      $("#AddPlayPlayGroupId").val(playGroupId);
      $("#AddPlayFormDialog").dialog("open");
    }

    function EditPlay(playId, playGroupId, playName, playText) {
      $("#EditPlayPlayId").val(playId);
      $("#EditPlayPlayGroupId").val(playGroupId);
      $("#EditPlayPlayName").val(playName);
      $("#EditPlayPlayText").val(playText);

      $("#EditPlayFormDialog").dialog("open");
    }
    
    function EditPlayGroup(playGroupId, playGroupName) {
      $("#EditPlayGroupId").val(playGroupId);
      $("#EditPlayGroupName").val(playGroupName);
      
      $("#EditPlayGroupDialog").dialog("open");
    }
  </script>

  <div id="PlayGroupPanel">
    <div id="PlayGroupAccordion">
      <% foreach (var group in Model.PlayGroups) {%>
        <h3>
          <a href="#"><%= group.Name %>
            <span class="DeletePlayIcon" onclick="DeletePlayGroup(<%=group.Id%>);"></span>
            <span class="EditPlayIcon" onclick="EditPlayGroup(<%=group.Id%>, '<%=group.Name%>')"></span>
            <span class="Clear"></span>
          </a>
        </h3>
        <div>
          <ul class="PlayList">
            <% foreach (var play in group.Plays) { %>
              <li class="PlayListElement">
                <span class="PlayName" onclick="LoadPlay(<%=play.Id%>);"><%=play.Name%></span>
                <div class="DeletePlayIcon" onclick="DeletePlay(<%=play.Id%>);"></div>
                <div class="EditPlayIcon" onclick="EditPlay(<%=play.Id%>, <%=group.Id%>, '<%=play.Name%>', '<%=play.FormattedText%>');"></div>
                <div class="Clear"></div>
              </li>
            <% } %>
              <li class="PlayListElement last" onclick="AddPlay(<%=group.Id%>)">
                <span>New Play</span>
              </li>
          </ul>
        </div>
      <% }  %>
    </div>
    <div id="NewGroupSection">
      <h3 id="NewGroupHeader" onclick="ToggleNewGroupForm();"><span>New Group</span></h3>
      <div id="NewGroupForm" style="display:none">
        <form action="/Team/CreatePlayGroup" method="post">
          <input id="NewPlayGroupNameInput" class="PlayTextInput" type="text" name="playGroupName" value="" />
          <input class="PlayTextSubmit" type="submit" value="Add" name="Save" />
        </form>
      </div>
      <div class="Clear"></div>
    </div>
  </div>

  <div id="MainViewPanel">
    <canvas id="PlayCanvas" width="720px" height="250px"></canvas>
    <div id="ButtonBar">
      <button id="PlayPauseButton" class="PlayPauseSprites PlayButton" onclick="playPauseClickHandler();"></button>
      <button id="FullScreenButton" class="FullScreenButton" onclick="fullScreenHandler();"></button>
    </div>
  </div>

  <div id="AddPlayFormDialog" title="Add Play">
    <p class="validateTips"></p>
    <form id="AddPlayForm" action="/Team/CreatePlay" method="post" enctype="multipart/form-data">
      <fieldset>
        <input type="hidden" name="playGroupId" id="AddPlayPlayGroupId" />
        <label for="playName">Name</label>
        <input type="text" name="playName" id="AddPlayPlayName" class="text ui-widget-content ui-corner-all" />
        <label for="playData">Play file (saved from the client)</label>
        <input type="file" name="playData" id="AddPlayPlayData" class="file ui-widget-content ui-corner-all" />
        <input type="hidden" name="playText" value="" />
      </fieldset>
    </form>
  </div>

  <div id="EditPlayFormDialog" title="Edit Play">
    <p class="validateTips"></p>
    <form id="EditPlayForm" action="/Team/EditPlay" method="post" enctype="multipart/form-data">
      <fieldset>
        <input type="hidden" name="playId" id="EditPlayPlayId" />
        <input type="hidden" name="playGroupId" id="EditPlayPlayGroupId" />
        <label for="playName">Name</label>
        <input type="text" name="playName" id="EditPlayPlayName" class="text ui-widget-content ui-corner-all" />
        <label for="playData">Play file (saved from the client)</label>
        <input type="file" name="playData" id="EditPlayPlayData" class="file ui-widget-content ui-corner-all" />
        <input type="hidden" name="playText" id="EditPlayPlayText" value="" />
      </fieldset>
    </form>
  </div>
  
  <div id="EditPlayGroupDialog" title="Edit Play Group">
    <form id="EditPlayGroupForm" action="/Team/EditPlayGroup" method="post">
      <fieldset>
        <input type="hidden" name="id" id="EditPlayGroupId" />
        <label for="name">Name:</label>
        <input type="text" name="name" id="EditPlayGroupName" />
      </fieldset>
    </form>
  </div>

  <div id="DeletePlayConfirmDialog" title="Delete Play">
    <p>
      <span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>
      This play will be permanently deleted and cannot be recovered. Are you sure?
    </p>
    <input type="hidden" id="DeletePlayId" />
  </div>

  <div id="DeletePlayGroupConfirmDialog" title="Delete Play">
    <p>
      <span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>
      This group will be permanently deleted and cannot be recovered. Are you sure?
    </p>
    <input type="hidden" id="DeletePlayGroupId" />
  </div>

  <div class="Clear"></div>
</asp:Content>
