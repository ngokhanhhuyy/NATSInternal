/**
 * @typedef {object} Config
 * @property {boolean} interceptAnchor
 * @property {"getAndPost" | "getOnly" | "postOnly" | false} interceptForm
 * @property {number} hubStartingTimeout
 * @property {number} requestTimeout
 */

let /** @type {HTMLButtonElement} */ optionsButton;
let /** @type {HTMLFormElement} */ configForm, /** @type {HTMLButtonElement} */ configSubmitButton;
let /** @type {string} */ currentRoute;
let /** @type {HTMLInputElement} */ addressInput, /** @type {HTMLIFrameElement} */ iframe;
let /** @type {HTMLButtonElement} */ refreshButton, /** @type {HTMLButtonElement} */ resetWSButton;
let /** @type {signalR.HubConnectionBuilder} */ hubConnection;

const /** @type {Config} */ config = {
    interceptAnchor: true,
    interceptForm: "getAndPost",
    hubStartingTimeout: 5,
    requestTimeout: 10
};

window.addEventListener("DOMContentLoaded", async () => {
    hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/wrapper")
        .withHubProtocol(new signalR.protocols.msgpack.MessagePackHubProtocol())
        .build();

    optionsButton = document.getElementById("optionsButton");

    configForm = document.getElementById("options");
    configForm.addEventListener("submit", changeServiceWorkerConfig);

    configSubmitButton = document.getElementById("configSubmitButton");

    addressInput = document.getElementById("addressInput");
    addressInput.addEventListener("focus", () => addressInput.select());
    addressInput.addEventListener("focusout", () =>  addressInput.blur());
    addressInput.addEventListener("keydown", (event) => {
        if (event.keyCode === 13) {
            iframe.src = window.location.origin + currentRoute;
        }
    });
    
    iframe = document.getElementById("iframe");
    iframe.addEventListener("load", () => {
        optionsButton.textContent = window.location.origin + "/";
    });

    refreshButton = document.getElementById("refreshButton");
    refreshButton.addEventListener("click", () => iframe.src = iframe.src);

    loadConfigButton = document.getElementById("loadConfigButton");
    loadConfigButton.addEventListener("click", getServiceWorkerConfig);

    resetWSButton = document.getElementById("resetWsButton");
    resetWSButton.addEventListener("click", resetHubConnnection);
    
    try {
        await connection.start();
        console.log("Wrapper hub connected.");
    } catch (error) {
        console.log(error);
    }
});

window.addEventListener("message", (event) => {
    if (typeof event.data === "string") {
        console.log(event.data);
        return;
    }

    /** @type {{ type: string, newRoute: string }} */
    const { type, newRoute } = event.data;
    if (type === "routeChanged") {
        currentRoute = newRoute;
        addressInput.value = newRoute.substring(1, newRoute.length);
    }
});

function changeServiceWorkerConfig(/** @type {SubmitEvent} */ event) {
    event.preventDefault();
    const formData = Object.fromEntries(new FormData(event.target));
    const config = {};
    Object.keys(formData).forEach(key => {
        console.log(JSON.stringify(formData));
        config[key] = !isNaN(formData[key]) ? parseInt(formData[key])* 1000 : (formData[key] || false);
    });
    serviceWorkerRegistration.active.postMessage({ type: "changeConfig", config });
}

function resetHubConnnection() {
    hubConnection.start();
}