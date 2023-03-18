PrintCopyRight();

function PrintCopyRight() {
    console.log("%cHello! \ud83d\ude4b", "color: #29c4a9;font-size: 16px;font-weight: 600;"),
        console.log("%cFInvoice front-end was built with HTML, CSS, and lots of love. \n\nFInvoice back-end was built with SQL Server, razor pages and lots of love. \n\n\ud83d\udc49 Want to learn with us? Check out ".concat(window.location.origin, "/Index"), "color: #29c4a9;font-size: 14px;");
}

$(document).ready(function () {
    $(window).bind("scroll", function () {
        var gap = 50;
        if ($(window).scrollTop() > gap) {
            $(".header-top").addClass("active");
        } else {
            $(".header-top").removeClass("active");
        }
    });
});

function menuToggle() {
    const toggleMenu = document.querySelector(".user-menu");
    toggleMenu.classList.toggle("active");
}