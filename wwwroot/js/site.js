// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function ClearInputCard() {
    document.getElementById("front-input").value = "";
    document.getElementById("back-input").value = "";
}

function SetColor() {
    var color = document.querySelector('input[name="CardColor"]:checked').value;
    document.getElementById("body").className = "card-body " + color;
    document.getElementById("answer").className = "card-body " + color;
}

function FlipCurrent() {
    document.getElementById("whole-card").classList.toggle("flip-counter");
}

function FlipCreated(cardID) {
    document.getElementById(cardID).classList.toggle("flip-counter");
}

function ConfirmPublish() {
    document.getElementById("publish").value = true;
}