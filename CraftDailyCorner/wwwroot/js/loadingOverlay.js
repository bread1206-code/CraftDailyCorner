window.LoadingOverlay = (function () {

    const overlay = () => document.getElementById("global-loading-overlay");
    const text = () => document.getElementById("loading-text");

    function show(message) {
        if (!overlay()) return;

        if (message) {
            text().innerText = message;
        }

        overlay().classList.remove("d-none");
    }

    function hide() {
        if (!overlay()) return;

        overlay().classList.add("d-none");
    }

    return {
        show,
        hide
    };

})();