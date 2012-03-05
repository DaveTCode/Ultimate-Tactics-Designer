<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Donate
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <div id="LeftPane">
    <h1 class="Title Green">
      <span class="Head">Donate</span>
      <img src="/Content/images/Title-BG-Green.png" class="TitleBar" alt="" />
    </h1>

    <p>
    Please don't send me any money for using this software/website. It is highly unlikely that I will get a 
    sufficient amount from it to pay for the accountant that I would need, let alone to live off. 
    I'd much rather any money was sent directly to charity.
    </p>

    <p>
    If you aren't sure how much to give then I've given these guidelines:
    </p>
    <ul>
      <li>Casual User - Just let me know whether you liked the software/what you would improve</li>
      <li>College/University Coach - £10/$15</li>
      <li>Club coach - £20/$30</li>
    </ul>

    <p>
    I'd hugely prefer if the money was given to the tearfund "Where it's most needed" account (link below). If giving to Christian charities
    bothers you then feel free to give wherever you prefer!
    </p>

    <a target="_blank" href="https://www.tearfund.org/get_involved/give/give_main/one-off_donations/general_fund/?amount=10"><img src="/Content/images/Tearfund.png" alt=""/></a>
  </div>
  <div id="RightPane">
      <% Html.RenderPartial("RightMenu"); %>
  </div>
</asp:Content>
