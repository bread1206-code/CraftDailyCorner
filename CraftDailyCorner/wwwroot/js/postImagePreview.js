document.addEventListener("DOMContentLoaded", function () {

    const inputs = document.querySelectorAll(".image-preview-input");

    inputs.forEach(input => {

        input.addEventListener("change", function () {

            const previewId = this.dataset.previewTarget;
            const preview = document.getElementById(previewId);

            if (!preview) return;

            if (this.files && this.files[0]) {

                const reader = new FileReader();

                reader.onload = function (e) {
                    preview.src = e.target.result;
                    preview.classList.remove("d-none");
                };

                reader.readAsDataURL(this.files[0]);

            } else {
                preview.classList.add("d-none");
                preview.src = "";
            }
        });

    });

});