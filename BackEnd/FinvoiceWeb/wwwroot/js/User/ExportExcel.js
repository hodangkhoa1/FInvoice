function CheckValueUser(userID, urlServlet, homeUrl) {
    const showErrorBox = document.querySelector("#error-box");
    const bodyTag = document.getElementsByTagName("BODY")[0];

    if (userID === "") {
        bodyTag.style.overflowY = "hidden";
        bodyTag.style.height = "100%";
        showErrorBox.innerHTML = `
          <div class="error-popup">
              <div class="wrapper">
                  <div class="content">
                      <div class="warn-icon">
                          <span><i class="uil uil-exclamation"></i></span>
                      </div>
                      <h2>No Login Detected!</h2>
                      <p>You are not logged in. Please login to use all website functions.</p>
                      <div class="buttons">
                          <button onclick="window.location.href='${urlServlet}';" id="login-btn">Login</button>
                          <button onclick="window.location.href='${homeUrl}';" id="cancel-btn">Cancel</button>
                      </div>
                  </div>
              </div>
          </div>
      `;
    }
}