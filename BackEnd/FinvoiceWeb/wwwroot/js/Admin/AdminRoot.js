let menuButton = document.querySelector(".menu-button");
let navigation = document.querySelector(".navigation");
let bodyMain = document.querySelector(".body-main");
let menuOpen = true;

menuButton.onclick = function () {
    navigation.classList.toggle("active");
    bodyMain.classList.toggle("active");
    if (menuOpen) {
        menuButton.classList.toggle("open");
        menuOpen = false;
    } else {
        menuButton.classList.toggle("open");
        menuOpen = true;
    }
};