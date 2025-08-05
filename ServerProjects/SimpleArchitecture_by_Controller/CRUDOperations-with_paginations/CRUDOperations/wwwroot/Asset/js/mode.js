window.addEventListener("load", function () {
    // Change Swagger logo text
    const topBarText = document.querySelector('.topbar-wrapper .link span');
    if (topBarText) topBarText.textContent = "Web - API";

    // Add a build version label (example)
    const buildInfo = document.createElement("div");
    buildInfo.innerHTML = "<b>Build Version:</b> 1.0.0";
    buildInfo.style.padding = "5px";
    document.querySelector('.swagger-ui').prepend(buildInfo);
});
