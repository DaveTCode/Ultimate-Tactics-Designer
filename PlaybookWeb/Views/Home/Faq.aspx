<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Playbook F.A.Q.
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <script type="text/javascript" src="/Scripts/jquery.scrollTo-min.js"></script>
  <script type="text/javascript">
    $(document).ready(function () {
      for (var ii = 0; ii < $(".FaqQuestion").length; ii++) {
        $('#question_' + ii).click(function () {
          $.scrollTo('#answer_' + $(this).attr('id').replace('question_', ''), {
            duration: 500
          });
        });
      }

      // Go To TOP
      $('.go_to_top').click(function () {
        $.scrollTo('#top_zone', { duration: 500 });
      });
    });
  </script>

  <div id="LeftPane">
    <a id="top_zone"></a>

    <h1 class="Title Green">
      <span class="Head">F.A.Q.</span>
      <img src="/Content/images/Title-BG-Green.png" class="TitleBar" alt="" />
    </h1>

    <table>
      <tr>
        <td>
          <h3>About Me</h3>
          <img class="FaqQuestion" src="/Content/images/question.png" alt="" border="0" />&nbsp;<a id="question_1" onclick="return false;" href="#">Who are you?</a> <br />
          <img class="FaqQuestion" src="/Content/images/question.png" alt="" border="0" />&nbsp;<a id="question_2" onclick="return false;" href="#">Who do you play ultimate for?</a> <br />
        </td>
        <td>
          <h3>Website</h3>
          <img class="FaqQuestion" src="/Content/images/question.png" alt="" border="0" />&nbsp;<a id="question_3" onclick="return false;" href="#">Can I use your player on our team site?</a> <br />
          <img class="FaqQuestion" src="/Content/images/question.png" alt="" border="0" />&nbsp;<a id="question_4" onclick="return false;" href="#">How do I move plays between groups?</a> <br />
          <img class="FaqQuestion" src="/Content/images/question.png" alt="" border="0" />&nbsp;<a id="question_5" onclick="return false;" href="#">Can I set up multiple accounts?</a> <br />
          <img class="FaqQuestion" src="/Content/images/question.png" alt="" border="0" />&nbsp;<a id="question_6" onclick="return false;" href="#">How do I delete my account?</a> <br />
        </td>
      </tr>
      <tr>
        <td>
          <h3>Client</h3>
          <img class="FaqQuestion" src="/Content/images/question.png" alt="" border="0" />&nbsp;<a id="question_7" onclick="return false;" href="#">Where can I download the client?</a> <br />
          <img class="FaqQuestion" src="/Content/images/question.png" alt="" border="0" />&nbsp;<a id="question_8" onclick="return false;" href="#">What are the prerequisites?</a> <br />
          <img class="FaqQuestion" src="/Content/images/question.png" alt="" border="0" />&nbsp;<a id="question_9" onclick="return false;" href="#">Is there a Mac version of the client?</a> <br />
        </td>
        <td>
          <h3>Coding</h3>
          <img class="FaqQuestion" src="/Content/images/question.png" alt="" border="0" />&nbsp;<a id="question_10" onclick="return false;" href="#">Why isn't this open source?</a> <br />
          <img class="FaqQuestion" src="/Content/images/question.png" alt="" border="0" />&nbsp;<a id="question_11" onclick="return false;" href="#">Can I help with coding?</a> <br />
          <img class="FaqQuestion" src="/Content/images/question.png" alt="" border="0" />&nbsp;<a id="question_12" onclick="return false;" href="#">Will you help me with my project?</a> <br />
        </td>
      </tr>
    </table>

    <h2>About Me</h2>
    <h3 id="answer_1">Who are you?</h3>
    <div id="answer_1_text">
      I'm a software engineer working for Metaswitch Networks based in Enfield, London.
    </div>

    <h3 id="answer_2">Who do you play ultimate for?</h3>
    <div id="answer_2_text">
      I currently coach a mixed team called Bear Cavalry who won all the major mixed events in the UK in 2011.

      When I play open I play for Clapham Ultimate and went to the WUCC Prague with them in 2010.
    </div>

    <h2>Website</h2>
    <h3 id="answer_3">Can I use your player on our team site?</h3>
    <div id="answer_3_text">
      <p>Yes! And I highly recommend doing so you'll be able to set up your own permission structure.</p>

      <p>You'll need to include <a href="/Scripts/canvas_player-1.0.js">the js player</a> and set it up so 
that you can make a call to create the CanvasPlayer object. This requires an xml document (on this site
it is retrieved via an ajax call) and a canvas element on the page.</p>

      <p>You're also welcome to make any improvements you like to the underlying script. If you do anything
particularly clever/interesting with it then let me know so I can include it back on the main site.</p>

      <p>Please <a href="mailto:davet.code@googlemail.com">email me</a> with any questions.</p>
    </div>

    <h3 id="answer_4">How do I move plays between groups?</h3>
    <div id="answer_4_text">Sorry, at the moment you can't. Feel free to let me know if you really want this
feature and I'll add it.</div>

    <h3 id="answer_5">Can I set up multiple accounts?</h3>
    <div id="answer_5_text">
      Fundamentally there is nothing stopping you. Please don't take too much advantage.
    </div>

    <h3 id="answer_6">How do I delete my account?</h3>
    <div id="answer_6_text">
      There is no external way of deleting accounts at the moment. <a href="mailto:davet.code@googlemail.com">Email me</a>
if you need an account deleted.
    </div>

    <h2>Client</h2>
    <h3 id="answer_7">Where can I download the client?</h3>
    <div id="answer_7_text">The installer can be downloaded <a href="/Home/Download">here</a></div>
    
    <h3 id="answer_8">What are the prerequisites?</h3>
    <div id="answer_8_text">The client is a .NET application and requires .NET framework 4. This should be installed when you install the application
but you can download it from <a href="http://www.microsoft.com/download/en/details.aspx?id=17851">here.</a></div>

    <h3 id="answer_9">Is there a Mac/Linux version of the client?</h3>
    <div id="answer_9_text">
      <p>Since the client application is a .NET app there isn't a direct linux/mac port planned. However, the
<a href="http://www.mono-project.com/Main_Page">mono project</a> enables .NET applications to be run on 
linux/mac based systems.</p>

      <p>I haven't gone through the same test plan (and unless someone sends me a mac I can't) but I have
heard that the client does work fine in mono.</p>

      <p>Whilst it's not officially supported, I am still interested in bugs in this version so please let me know.</p>

      <p>You'll need the core executable (minus the installer) which can be got <a href="Home/Download?installer=0">here</a></p>
    </div>

    <h2>Coding</h2>
    
    <h3 id="answer_10">Why isn't this open source?</h3>
    <div id="answer_10_text">
      <p>Whilst I'm still working on this project I'd like to keep control of it's direction and what features get
added/removed etc. This means that I can't open source it just yet.</p>

      <p>That said, if I ever decide I've had enough of it and will not be supporting it any more then I'll upload the
source code onto googlecode and pass it on to the community.</p>

      <p>Note that the web based player is already open source (since it's just a javascript file).</p>
    </div>
    
    <h3 id="answer_11">Can I help with coding?</h3>
    <div id="answer_11_text">
    <p>Maybe, drop me an <a href="mailto:davet.code@googlemail.com">email</a> with your ideas and how you'd like to help.</p>
    </div>

    <h3 id="answer_12">Will you help me with my project?</h3>
    <div id="answer_12_text">
    <p>Probably not. I've got a full time job as a software engineer and I write projects like this in my spare time. I don't
have a whole lot of time to try new things.</p>

    <p>You're welcome to <a href="mailto:davet.code@googlemail.com">email me</a> and ask though.</p>
    </div>
  </div>
  <div id="RightPane">
      <% Html.RenderPartial("RightMenu"); %>
  </div>
  <div class="Clear"></div>
</asp:Content>
