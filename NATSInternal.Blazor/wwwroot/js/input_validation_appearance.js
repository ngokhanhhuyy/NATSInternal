window.addEventListener("DOMContentLoaded", () => {
    // Change color for the invalid validation message elements and their associated input
    // elements.
    /**
     * @type {NodeListOf<HTMLSpanElement>}
     */
    const invalidValidationMessages = document.querySelectorAll(".field-validation-error");
    invalidValidationMessages.forEach(message => {
        message.classList.add("invalid-feedback");
        message.classList.add("d-inline");
        /**
         * @type {HTMLInputElement | HTMLTextAreaElement}
         */
        let input = document.getElementById(message.getAttribute("data-valmsg-for"));
        if (!input) {
            input = document.getElementById(message.getAttribute("input-id"));
        }
        input.classList.add("is-invalid");
    });

    // Change color for the valid validation message elements and their associated input
    // elements.
    /**
     * @type {NodeListOf<HTMLSpanElement>}
     */
    const validValidationMessages = document.querySelectorAll(".field-validation-valid");
    validValidationMessages.forEach(message => {
        /**
         * @type {HTMLInputElement | HTMLTextAreaElement}
         */
        let input = document.getElementById(message.getAttribute("data-valmsg-for"));
        if (!input) {
            input = document.getElementById(message.getAttribute("input-id"));
        }
        /**
         * @type {HTMLFormElement}
         */
        let form = input.form;
        /**
         * @type {string}
         */
        let wasValidated = form.getAttribute("was-validated");
        if (wasValidated === "true") {
            message.classList.add("valid-feedback");
            message.classList.add("d-inline");
            input.classList.add("is-valid");
        }
    });
});