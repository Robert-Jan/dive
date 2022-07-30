document.addEventListener("alpine:init", () => {
    Alpine.data("form", form);

    function form() {
        return {
            inputElements: [],
            init() {
                //Set up custom Iodine rules
                Iodine.rule('password', (value) => value == '' ? true : Iodine.assertRegexMatch(value,
                    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/
                ));
                Iodine.setErrorMessage('password', 'The password must match the given rules')

                Iodine.rule('alphanumeric', (value) => Iodine.assertRegexMatch(value, /^[a-zA-Z0-9-_]+$/));
                Iodine.setErrorMessage('alphanumeric', 'The [FIELD] must be alphanumeric')

                // Store an array of all the input elements with 'data-rules' attributes
                this.inputElements = [...this.$el.querySelectorAll("input[data-rules]")];
                this.inputElements.push(...this.$el.querySelectorAll("textarea[data-rules]"))
                this.initDomData();
                this.updateErrorMessages();
            },
            initDomData: function () {
                // Create an object attached to the component state for each input element to store its state
                this.inputElements.map((element) => {
                    this[element.name] = {
                        serverErrors: element.dataset.serverErrors ? JSON.parse(element.dataset.serverErrors) : [],
                        blurred: false
                    };
                });
            },
            updateErrorMessages: function () {
                // Map throught the input elements and set the 'errorMessage'
                this.inputElements.map((element) => {
                    this[element.name].errorMessage = this.getErrorMessage(element);
                });
            },
            getErrorMessage: function (element) {
                // Return any server errors if they're present
                if (this[element.name].serverErrors.length > 0) {
                    return input.serverErrors[0];
                }
                // Check using iodine and return the error message only if the element has not been blurred
                const result = Iodine.assert(element.value, JSON.parse(element.dataset.rules));
                if (result.valid !== true && this[element.name].blurred) {
                    return result.error
                }
                // Return empty string if there are no errors
                return "";
            },
            submit: function (event) {
                const invalidElements = this.inputElements.filter((input) => {
                    return Iodine.assert(input.value, JSON.parse(input.dataset.rules)).valid !== true;
                });
                if (invalidElements.length > 0) {
                    event.preventDefault();
                    document.getElementById(invalidElements[0].id).scrollIntoView();
                    // We set all the inputs as blurred if the form has been submitted
                    this.inputElements.map((input) => {
                        this[input.name].blurred = true;
                    });
                    // And update the error messages.
                    this.updateErrorMessages();
                }
            },
            change: function (event) {
                // Ignore all events that aren't coming from the inputs we're watching
                if (!this[event.target.name]) {
                    return false;
                }
                if (event.type === "input") {
                    this[event.target.name].serverErrors = [];
                }
                if (event.type === "focusout") {
                    this[event.target.name].blurred = true;
                }
                // Whether blurred or on input, we update the error messages
                this.updateErrorMessages();
            }
        };
    }
});
