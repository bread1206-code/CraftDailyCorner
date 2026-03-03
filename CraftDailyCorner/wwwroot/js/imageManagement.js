// wwwroot/js/imageManagement.js
function initImageManagement() {

    const container = document.getElementById("imageManagement");
    if (!container) return;

    const entityId = container.dataset.entityId;
    const entityType = container.dataset.entityType;

    // 讀取上限（由後端 VM 帶入 data-max-count；若沒值 = 不限制）
    const maxCountAttr = container.dataset.maxCount;
    const maxCount = maxCountAttr ? parseInt(maxCountAttr, 10) : null;

    const imageList = container.querySelector("#imageList");
    if (!imageList) return;

    const tokenInput = container.querySelector('input[name="__RequestVerificationToken"]');
    if (!tokenInput) return;

    const antiForgeryToken = tokenInput.value;

    // ==================================================
    // 快速刪除模式
    // ==================================================

    const quickToggle = container.querySelector("#quickDeleteToggle");
    const quickAlert = container.querySelector("#quickDeleteAlert");

    if (quickToggle && quickAlert) {
        quickToggle.addEventListener("change", function () {
            quickAlert.classList.toggle("d-none", !this.checked);
        });
    }

    // ==================================================
    // 拖曳排序
    // ==================================================
    new Sortable(imageList, {
        animation: 150,

        onEnd: async function () {

            const items = [...imageList.querySelectorAll(".imageItem")];
            const orderedIds = [];

            items.forEach((item, index) => {

                const imageId = parseInt(item.dataset.imageId, 10);
                orderedIds.push(imageId);

                // ===== 即時更新 UI =====
                const infoArea = item.querySelector(".card-body");
                if (!infoArea) return;

                const badge = infoArea.querySelector(".badge");
                const sortLabel = infoArea.querySelector(".sortLabel");

                if (index === 0) {
                    // 第一張 → 封面
                    if (sortLabel) sortLabel.remove();

                    if (badge) {
                        badge.className = "badge bg-success mb-2";
                        badge.innerText = "封面";
                    } else {
                        infoArea.insertAdjacentHTML(
                            "afterbegin",
                            `<span class="badge bg-success mb-2">封面</span>`
                        );
                    }
                } else {
                    if (badge) badge.remove();

                    if (sortLabel) {
                        sortLabel.innerText = `排序：${index + 1}`;
                    } else {
                        infoArea.insertAdjacentHTML(
                            "afterbegin",
                            `<span class="text-muted small mb-2 sortLabel">排序：${index + 1}</span>`
                        );
                    }
                }
            });

            // ===== 使用 FormData 傳送 =====
            const formData = new FormData();
            formData.append("entityId", entityId);
            formData.append("entityType", entityType);

            orderedIds.forEach(id => formData.append("orderedIds", id));

            formData.append("__RequestVerificationToken", antiForgeryToken);

            try {
                const response = await fetch("/ImageManagement/UpdateSort", {
                    method: "POST",
                    body: formData
                });

                if (!response.ok) {
                    alert("排序儲存失敗，請重新整理頁面");
                }
            } catch (error) {
                console.error(error);
                alert("排序發生錯誤");
            }
        }
    });

    // ==================================================
    // 刪除圖片（不重載 container）
    // ==================================================

    imageList.addEventListener("click", async function (e) {

        const button = e.target.closest(".deleteBtn");
        if (!button) return;

        const items = imageList.querySelectorAll(".imageItem");

        // 至少保留一張（通用文案，不寫死商品）
        if (items.length <= 1) {
            alert("至少需要一張圖片");
            return;
        }

        const imageId = button.dataset.imageId;
        const isQuickMode = quickToggle?.checked;

        if (!isQuickMode) {
            if (!confirm("確定要刪除此圖片？")) return;
        }

        button.disabled = true;

        const formData = new FormData();
        formData.append("entityId", entityId);
        formData.append("entityType", entityType);
        formData.append("imageId", imageId);
        formData.append("__RequestVerificationToken", antiForgeryToken);

        try {

            const response = await fetch("/ImageManagement/Delete", {
                method: "POST",
                body: formData
            });

            if (!response.ok) {
                const msg = await response.text();
                alert(msg || "刪除失敗");
                button.disabled = false;
                return;
            }

            const html = await response.text();

            // 只更新圖片列表
            const tempDiv = document.createElement("div");
            tempDiv.innerHTML = html;

            const newImageList = tempDiv.querySelector("#imageList");

            if (newImageList) {
                imageList.innerHTML = newImageList.innerHTML;
            }

            // 刪除後立即重新排序 UI
            refreshSortUI();

        } catch (error) {
            console.error(error);
            alert("刪除發生錯誤");
            button.disabled = false;
        }
    });

    // ==================================================
    // 上傳圖片（只更新 imageList）
    // ==================================================
    const uploadBtn = container.querySelector("#uploadImagesBtn");
    const fileInput = container.querySelector("#imageUploadInput");

    if (uploadBtn && fileInput) {

        // 初始禁用
        uploadBtn.disabled = true;

        // 偵測檔案選擇
        fileInput.addEventListener("change", function () {
            uploadBtn.disabled = this.files.length === 0;
        });

        uploadBtn.addEventListener("click", async function () {

            const currentImages = imageList.querySelectorAll(".imageItem").length;
            const selectedFiles = fileInput.files.length;

            if (!selectedFiles) {
                alert("請選擇圖片");
                return;
            }

            // 由 data-max-count 決定（Portfolio=25 / Product=10）
            if (maxCount && (currentImages + selectedFiles > maxCount)) {
                alert(`圖片最多 ${maxCount} 張`);
                return;
            }

            uploadBtn.disabled = true;

            // 顯示 Loading
            if (window.LoadingOverlay) {
                LoadingOverlay.show("圖片上傳中，請稍候...");
            }

            const formData = new FormData();
            formData.append("entityId", entityId);
            formData.append("entityType", entityType);

            // 加入檔案
            for (let i = 0; i < selectedFiles; i++) {
                formData.append("files", fileInput.files[i]);
            }

            formData.append("__RequestVerificationToken", antiForgeryToken);

            try {

                const response = await fetch("/ImageManagement/Upload", {
                    method: "POST",
                    body: formData
                });

                if (!response.ok) {
                    const msg = await response.text();
                    alert(msg || "上傳失敗");
                    return;
                }

                const html = await response.text();

                const tempDiv = document.createElement("div");
                tempDiv.innerHTML = html;

                const newImageList = tempDiv.querySelector("#imageList");

                if (newImageList) {
                    imageList.innerHTML = newImageList.innerHTML;
                }

                refreshSortUI();

                // 清空欄位
                fileInput.value = "";
                uploadBtn.disabled = true;

            } catch (error) {
                console.error(error);
                alert("上傳發生錯誤");
            }
            // 隱藏 Loading
            finally {
                if (window.LoadingOverlay) {
                    LoadingOverlay.hide();
                }
                uploadBtn.disabled = true;
            }
        });
    }

    // ==================================================
    // UI 更新工具函式
    // ==================================================

    function refreshSortUI() {
        const items = [...imageList.querySelectorAll(".imageItem")];
        items.forEach((item, index) => updateSortUI(item, index));
    }

    function updateSortUI(item, index) {

        const infoArea = item.querySelector(".card-body");
        if (!infoArea) return;

        const badge = infoArea.querySelector(".badge");
        const sortLabel = infoArea.querySelector(".sortLabel");

        if (index === 0) {

            if (sortLabel) sortLabel.remove();

            if (badge) {
                badge.className = "badge bg-success mb-2";
                badge.innerText = "封面";
            } else {
                infoArea.insertAdjacentHTML(
                    "afterbegin",
                    `<span class="badge bg-success mb-2">封面</span>`
                );
            }

        } else {

            if (badge) badge.remove();

            if (sortLabel) {
                sortLabel.innerText = `排序：${index + 1}`;
            } else {
                infoArea.insertAdjacentHTML(
                    "afterbegin",
                    `<span class="text-muted small mb-2 sortLabel">排序：${index + 1}</span>`
                );
            }
        }
    }
}

window.initImageManagement = initImageManagement;