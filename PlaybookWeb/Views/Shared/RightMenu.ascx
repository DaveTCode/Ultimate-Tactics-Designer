<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div class="RightMainMenu">
  <div class="TopL"><div class="TopR"><div class="Top"></div></div></div>
  <div class="Middle">
    <div class="Normal" style="padding:0 12px;">
      <ul id="RightLinks">
        <%if (Request.IsAuthenticated)
          {%>
        <li>
          <a href="/Team/ViewPlays">
            <img src="/Content/images/player-home-icon.png" alt="" />
            <span>Team Homepage</span>View/edit your teams plays
          </a>
        </li>
        <% } %>
        <li>
          <a href="/Home/Download">
            <img src="/Content/images/download-icon.png" alt="" />
            <span>Get Client</span>Download the client to start creating plays
          </a>
        </li>
        <li>
          <a href="/Home/Faq">
            <img src="/Content/images/faq-icon.png" alt="" />
            <span>FAQ</span>Check here for frequently asked questions
          </a>
        </li>
        <li>
          <a href="/Home/Donate">
            <img src="/Content/images/donate-icon.png" alt="" />
            <span>Donate</span>You can show your appreciation for the Playbook software by donating
          </a>
        </li>
        <li class="last">
          <a href="mailto:davet.code@googlemail.com">
            <img src="/Content/images/contact-icon.png" alt="" />
            <span>Contact</span>Inform the author of any issues/bugs or ideas
          </a>
        </li>
      </ul>
    </div>
  </div>
  <div class="BottomL"><div class="BottomR"><div class="Bottom"></div></div></div>
</div>