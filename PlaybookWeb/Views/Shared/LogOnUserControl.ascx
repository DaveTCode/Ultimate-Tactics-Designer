<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
  <div id="Login">
<%
  if (Request.IsAuthenticated)
  {
%>
    <a class="LoginLink" href="/Account/LogOff">Logout</a>
    |
    <a title="Click Here To Go Your Team Page." class="SkinObject" href="/Team/ViewPlays"><%: Page.User.Identity.Name%></a>
    <div class="language-object"></div>
<%
  }
  else
  {
%>
    <a class="LoginLink" href="/Account/LogOn">Login</a>
    |
    <a class="SkinObject" href="/Account/Register">Register</a>
    <div class="language-object"></div>
<%
  }
%>
  </div>