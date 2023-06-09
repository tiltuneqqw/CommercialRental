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

function getCitiesByRegion() {
    const regionSelect = document.getElementById("region-select");
    const citySelect = document.getElementById("city-select");

    const region = regionSelect.value;

    const jsonPath = window.location.origin + `/json/cities.json`;
    fetch(jsonPath)
        .then(response => response.json())
        .then(data => {
            const regionCities = data.filter(city => city.region === region);

            citySelect.innerHTML = "";

            if (regionCities.length === 0) {
                const defaultOption = document.createElement("option");
                defaultOption.value = "";
                defaultOption.text = "Виберіть область";
                citySelect.add(defaultOption);
            }
            else {
                regionCities.forEach((city, index) => {
                    const option = document.createElement("option");
                    option.value = city.name;
                    option.text = city.name;
                    if (index == 0) {
                        option.selected = true;
                    }
                    citySelect.add(option);

                    const storedCity = localStorage.getItem("selectedCity");
                    if (storedCity === city.name) {
                        option.selected = true;
                    }
                });
            }
        })
        .catch(error => console.error("Error:", error));
}

function removeFields() {
    localStorage.removeItem("selectedCity");
}

document.addEventListener("DOMContentLoaded", function () {
    getCitiesByRegion();
});

document.getElementById("city-select").addEventListener("change", function () {
    const selectedCity = this.value;
    localStorage.setItem("selectedCity", selectedCity);
});