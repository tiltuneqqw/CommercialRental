// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showDiv(divId) {
    var elements = document.querySelectorAll(".cancel-div");

    for (var i = 0; i < elements.length; i++) {
        elements[i].style.display = "none";
    }

    var div = document.getElementById(divId);
    div.style.display = 'flex'; // or any other desired display value
}

function openModal(divId) {
    var div = document.getElementById(divId);
    div.style.display = 'flex';
}

function closeModal(divId) {
    var div = document.getElementById(divId);
    div.style.display = 'none';
}

var phoneReg = document.getElementById("phone-register");
if (phoneReg) {
    phoneReg.addEventListener("input", function (event) {
        const phoneNumber = event.target.value.replace(/\D/g, "");
        const formattedPhoneNumber = phoneNumber.replace(/(\d{3})(\d{3})(\d{2})(\d{2})/, "$1-$2-$3-$4");
        event.target.value = formattedPhoneNumber;
    });
}