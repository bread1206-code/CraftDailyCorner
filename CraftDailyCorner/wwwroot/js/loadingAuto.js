document.addEventListener("DOMContentLoaded", function () {

    document.addEventListener("submit", function (e) {

        const form = e.target;

        if (!(form instanceof HTMLFormElement))
            return;

        // 只處理有 multipart 的表單
        if (form.enctype !== "multipart/form-data")
            return;

        // 檢查是否真的有選檔
        const fileInputs = form.querySelectorAll('input[type="file"]');

        let hasFile = false;

        fileInputs.forEach(input => {
            if (input.files && input.files.length > 0) {
                hasFile = true;
            }
        });

        if (!hasFile)
            return;

        // 防止重複送出
        const submitBtn = form.querySelector('button[type="submit"]');
        if (submitBtn) {
            submitBtn.disabled = true;
        }

        LoadingOverlay.show("圖片上傳中，請稍候...");
    });

});