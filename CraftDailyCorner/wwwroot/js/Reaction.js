const iconMap = {
    1: "bi-hand-thumbs-up-fill text-primary",
    2: "bi-heart-fill text-danger",
    3: "bi-emoji-laughing text-warning",
    4: "bi-emoji-surprise text-info",
    5: "bi-emoji-frown text-secondary",
    6: "bi-emoji-angry text-danger"
};

//  直接執行，不要 DOMContentLoaded

document.querySelectorAll(".reaction-container").forEach(container => {

    const mainBtn = container.querySelector(".reaction-main-btn");
    const picker = container.querySelector(".reaction-picker");
    const mainIcon = container.querySelector(".reaction-main-icon");

    let isProcessing = false;

    // 打開選單
    mainBtn.addEventListener("click", function (e) {

        e.stopPropagation();

        document.querySelectorAll(".reaction-picker")
            .forEach(p => p.classList.add("d-none"));

        picker.classList.toggle("d-none");
    });

    // 點外面關閉
    document.addEventListener("click", function (e) {
        if (!container.contains(e.target)) {
            picker.classList.add("d-none");
        }
    });


    picker.querySelectorAll(".reaction-item").forEach(item => {
        item.addEventListener("click", async function (e) {
            e.stopPropagation();

            if (isProcessing) return;
            isProcessing = true;

            const targetType = container.dataset.targetType;
            const targetId = container.dataset.targetId;
            const reactionType = this.dataset.type;


            // 立即關閉選單 (符合操作直覺)
            picker.classList.add("d-none");

            const formData = new FormData();
            formData.append("targetType", targetType);
            formData.append("targetId", targetId);
            formData.append("reactionType", reactionType);

            try {
                const response = await fetch("/reaction/toggle", {
                    method: "POST",
                    body: formData
                });

                //  1. 處理未登入狀況
                if (response.status === 401) {
                    // 喚起 Bootstrap Modal
                    const loginModal = new bootstrap.Modal(document.getElementById('loginModal'));
                    loginModal.show();
                    isProcessing = false;
                    return; // 中斷後續更新
                }

                //  2. 處理其他錯誤
                if (!response.ok) {
                    throw new Error("Network response was not ok");
                }

                const result = await response.json();
                updateUI(container, result);
                // 只有新增或變更才噴
                if (result.userReactionType) {
                    triggerReactionAnimation(
                        container,
                        "bi " + iconMap[result.userReactionType]
                    );
                }
            } catch (error) {
                console.error("Reaction Error:", error);
                alert("操作失敗，請稍後再試"); // 簡單提示
            } finally {
                isProcessing = false;
            }
        });
    });


    function updateUI(container, result) {

        const mainBtn = container.querySelector(".reaction-main-btn");
        const mainIcon = container.querySelector(".reaction-main-icon");

        const total = Object.values(result.reactions)
            .reduce((sum, value) => sum + value, 0);

        container.querySelector(".reaction-total").innerText = total;

        if (result.userReactionType) {
            mainIcon.className = "reaction-main-icon bi " +
                iconMap[result.userReactionType];
        }
        else {
            mainIcon.className = "reaction-main-icon bi bi-hand-thumbs-up-fill";
        }
    }

    function triggerReactionAnimation(container, iconClass) {

        const floatIcon = document.createElement("i");
        floatIcon.className = iconClass + " reaction-float";

        floatIcon.style.position = "absolute";
        floatIcon.style.left = "50%";
        floatIcon.style.top = "0";
        floatIcon.style.transform = "translateX(-50%)";

        container.appendChild(floatIcon);

        setTimeout(() => {
            floatIcon.remove();
        }, 1200);
    }
});