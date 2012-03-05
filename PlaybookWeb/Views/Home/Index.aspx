<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Playbook - Home
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="LeftPane">
      <div id="about-box">
        <h1 class="Title Blue">
          <span class="Head">Getting Started</span>
          <img src="/Content/images/Title-BG-Green.png" class="TitleBar" alt="" />
        </h1>
        <p>
          The ultimate playbook is a free application that allows captains & coaches to create plays
          and upload them to their team page on this site for the whole team to view.
        </p>
        <p>
          It has a host of features not seen in any other ultimate playbook application:
        </p>
        
        <ul>
          <li>Triggered player start times</li>
          <li>Curved disc flight</li>
          <li>Online playback</li>
        </ul>

        <p>
          Please watch the video to get an idea of how powerful this application is.
        </p>
        <iframe width="425" height="349" src="http://www.youtube.com/embed/uq3pg0JcJSI" frameborder="0" allowfullscreen></iframe>
      </div>
    </div>
    <div id="RightPane">
      <% Html.RenderPartial("RightMenu"); %>
    </div>
    <div class="clear">&nbsp;</div>
</asp:Content>
