// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

class SelectorReset {
    static attachResetHandler(buttonId, fieldsToReset) {
        const resetButton = document.getElementById(buttonId);

        if (!resetButton) {
            return;
        }

        resetButton.addEventListener("click", function () {
            fieldsToReset.forEach((field) => {
                const input = document.querySelector(field.selector);

                if (input) {
                    input.value = "";
                }
            });

            const searchForm = resetButton.closest("form");

            if (searchForm) {
                searchForm.submit();
            }

        });
    }
}

function scrollToUp(elementId, windowScrollY = 100) {
    let floatingElement = document.getElementById(elementId);

    floatingElement.addEventListener("click", function () {
        window.scrollTo({
            top: 0,
            behavior: "smooth"
        });
    });

    let isButtonHidden = true;

    window.addEventListener("scroll", function () {
        if (window.scrollY > windowScrollY) {
            if (isButtonHidden) {
                floatingElement.style.bottom = "20px";
                isButtonHidden = false;
            }
        } else {
            if (!isButtonHidden) {
                floatingElement.style.bottom = "-60px";
                isButtonHidden = true;
            }
        }
    });
}