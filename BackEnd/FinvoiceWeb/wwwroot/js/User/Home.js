function showSearchBox() {
    const toggleSearch = document.querySelector(".nav__bar-mobile-tool");
    toggleSearch.classList.toggle("active");
}

/* Mobile Menu */
function BlockScrollInUserMenu() {
    const bodyForUserMenu = document.getElementsByTagName("BODY")[0];
    const mobileMenuCheckbox = document.querySelector("#mobile-menu-checkbox");
    if (mobileMenuCheckbox.checked === true) {
        bodyForUserMenu.style.overflowY = "hidden";
    } else {
        bodyForUserMenu.style.overflowY = "scroll";
    }
}

var swiper = new Swiper(".mySwiper", {
    slidesPerView: 1,
    spaceBetween: 10,
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    keyboard: {
        enabled: true,
    },
    breakpoints: {
        640: {
            slidesPerView: 2,
            spaceBetween: 20,
        },
        768: {
            slidesPerView: 3,
            spaceBetween: 40,
        },
        1024: {
            slidesPerView: 4,
            spaceBetween: 50,
        },
    },
    autoplay: {
        delay: 3000,
        disableOnInteraction: false
    }
});

/* Search With MicroPhone */
function activeMicrophone() {
    const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition,
        microphoneHeader = document.querySelector("#microphone-header__prompt"),
        microphoneFooterLabel = document.querySelector("#microphone-footer-label"),
        microphoneBodyText = document.querySelector("#microphone-body-text");

    if (SpeechRecognition !== undefined) {
        let recognition = new SpeechRecognition();

        recognition.onstart = () => {
            microphoneHeader.innerHTML = "Listening...";
            microphoneFooterLabel.innerHTML = "";
            microphoneBodyText.innerHTML = "";
        };

        recognition.onspeechend = () => {
            microphoneHeader.innerHTML = "Microphone is off. Please speak again.";
            microphoneFooterLabel.innerHTML = "Tap the microphone to try again";
            recognition.stop();
        };

        recognition.onresult = (result) => {
            microphoneBodyText.innerHTML = `${result.results[0][0].transcript}`;
            console.log(`${result.results[0][0].transcript}`);
        };

        recognition.start();
    }
}

function showMicrophoneBox() {
    const bodyForMicrophoneBox = document.getElementsByTagName("BODY")[0];
    const microphoneWrapper = document.querySelector("#microphone-wrapper");

    microphoneWrapper.classList.add("active");
    bodyForMicrophoneBox.style.overflowY = "hidden";
    activeMicrophone();
}

function hideMicrophoneBox() {
    const bodyForMicrophoneBox = document.getElementsByTagName("BODY")[0];
    const microphoneWrapper = document.querySelector("#microphone-wrapper");

    microphoneWrapper.classList.remove("active");
    bodyForMicrophoneBox.style.overflowY = "scroll";
}

/* End Search With MicroPhone */

/* Search With MicroPhone In Mobile */
function activeMicrophoneMobile() {
    const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition,
        microphoneHeader = document.querySelector("#mobile-header__prompt"),
        microphoneFooterLabel = document.querySelector("#mobile__microphone-footer-label"),
        microphoneBodyText = document.querySelector("#mobile-microphone-body-text");

    if (SpeechRecognition !== undefined) {
        let recognition = new SpeechRecognition();

        recognition.onstart = () => {
            microphoneHeader.innerHTML = "Listening...";
            microphoneFooterLabel.innerHTML = "";
            microphoneBodyText.innerHTML = "";
        };

        recognition.onspeechend = () => {
            microphoneHeader.innerHTML = "Microphone is off. Please speak again.";
            microphoneFooterLabel.innerHTML = "Tap the microphone to try again";
            recognition.stop();
        };

        recognition.onresult = (result) => {
            microphoneBodyText.innerHTML = `${result.results[0][0].transcript}`;
            console.log(`${result.results[0][0].transcript}`);
        };

        recognition.start();
    }
}

function showMicrophoneBoxMobile() {
    const bodyForMicrophoneBox = document.getElementsByTagName("BODY")[0];
    const microphoneWrapper = document.querySelector("#mobile__microphone-wrapper");

    microphoneWrapper.classList.add("active");
    bodyForMicrophoneBox.style.overflowY = "hidden";
    activeMicrophoneMobile();
}

function hideMicrophoneBoxMobile() {
    const bodyForMicrophoneBox = document.getElementsByTagName("BODY")[0];
    const microphoneWrapper = document.querySelector("#mobile__microphone-wrapper");

    microphoneWrapper.classList.remove("active");
    bodyForMicrophoneBox.style.overflowY = "scroll";
}