const iconMap = {
    1: "bi-hand-thumbs-up-fill text-primary",
    2: "bi-heart-fill text-danger",
    3: "bi-emoji-laughing text-success",
    4: "bi-emoji-surprise text-warning",
    5: "bi-emoji-frown text-info",
    6: "bi-emoji-angry text-danger"
};

// 使用事件委派或直接綁定皆可，此處維持原本結構
document.querySelectorAll(".reaction-container").forEach(container => {
    const mainBtn = container.querySelector(".reaction-main-btn");
    const picker = container.querySelector(".reaction-picker");
    const mainIcon = container.querySelector(".reaction-main-icon");
    let isProcessing = false;

    // 1. 切換選單顯示
    mainBtn.addEventListener("click", (e) => {
        e.stopPropagation();
        // 關閉其他所有打開的選單
        document.querySelectorAll(".reaction-picker").forEach(p => {
            if (p !== picker) p.classList.add("d-none");
        });
        picker.classList.toggle("d-none");
    });

    // 2. 點擊外部關閉選單
    document.addEventListener("click", (e) => {
        if (!container.contains(e.target)) {
            picker.classList.add("d-none");
        }
    });

    // 3. 點擊表情圖示
    picker.querySelectorAll(".reaction-item").forEach(item => {
        item.addEventListener("click", async function (e) {
            e.stopPropagation();
            if (isProcessing) return;
            isProcessing = true;

            const targetType = container.dataset.targetType;
            const targetId = container.dataset.targetId;
            const reactionType = this.dataset.type;

            picker.classList.add("d-none"); // 立即關閉選單

            const formData = new FormData();
            formData.append("targetType", targetType);
            formData.append("targetId", targetId);
            formData.append("reactionType", reactionType);

            try {
                const response = await fetch("/reaction/toggle", {
                    method: "POST",
                    body: formData
                });

                if (response.status === 401) {
                    const loginModal = new bootstrap.Modal(document.getElementById('loginModal'));
                    loginModal.show();
                    return;
                }

                if (!response.ok) throw new Error("Network error");

                const result = await response.json();
                updateUI(container, result);

                // ===== 關鍵修改：判斷要播哪種動畫 =====
                // 只有在「新增」或「切換」反應時才播放動畫 (UserReactionType 有值)
                if (result.userReactionType) {
                    const iconClass = iconMap[result.userReactionType];

                    if (container.closest(".big-effect")) {
                        // 播放 Big Effect (泡泡版)
                        triggerBigReactionAnimation(iconClass);
                    } else {
                        // 播放 普通版 (局部版)
                        triggerNormalReactionAnimation(container, "bi " + iconClass);
                    }
                }

            } catch (error) {
                console.error("Reaction Error:", error);
            } finally {
                isProcessing = false;
            }
        });
    });
});

function updateUI(container, result) {
    const mainIcon = container.querySelector(".reaction-main-icon");
    const total = Object.values(result.reactions).reduce((a, b) => a + b, 0);

    container.querySelector(".reaction-total").innerText = total;

    if (result.userReactionType) {
        mainIcon.className = "reaction-main-icon bi " + iconMap[result.userReactionType];
    } else {
        mainIcon.className = "reaction-main-icon bi bi-hand-thumbs-up-fill"; // 預設灰/黑
    }
}

// ==========================================
//  動畫函式庫
// ==========================================

// 1. 普通版動畫 (在按鈕上方飄一下)
function triggerNormalReactionAnimation(container, iconClass) {
    const floatIcon = document.createElement("i");
    floatIcon.className = iconClass + " reaction-normal-float";

    container.appendChild(floatIcon);

    // 動畫結束後移除
    floatIcon.addEventListener("animationend", () => {
        floatIcon.remove();
    });
}

// 2. Big Effect 動畫 (泡泡上升 -> 搖晃 -> 縮小爆破 -> 碎片)
function triggerBigReactionAnimation(iconClass) {

    // A. 建立全螢幕遮罩
    const overlay = document.createElement("div");
    overlay.className = "reaction-overlay";

    // B. 建立上升器 (Y軸動畫)
    const riser = document.createElement("div");
    riser.className = "reaction-big-riser";

    // C. 建立圖示 (X軸搖晃動畫)
    const icon = document.createElement("i");
    icon.className = "bi " + iconClass + " reaction-big-icon";

    riser.appendChild(icon);
    overlay.appendChild(riser);
    document.body.appendChild(overlay);

    // D. 時序控制
    // 上升動畫設為 1.5s，我們在 1.4s 處觸發爆破，讓接續更滑順
    setTimeout(() => {
        // 觸發爆破動畫
        icon.classList.add("pop");

        // 產生碎片 (傳入 riser 以取得目前螢幕位置)
        triggerFragments(iconClass, riser);
    }, 1400);

    // E. 整體清理
    setTimeout(() => {
        overlay.remove();
    }, 2500);
}

// 產生爆破碎片
function triggerFragments(iconClass, parentElement) {
    const fragmentCount = 12;

    // 取得 riser 此刻在螢幕的正中心座標
    const rect = parentElement.getBoundingClientRect();
    const centerX = rect.left + rect.width / 2;
    const centerY = rect.top + rect.height / 2;

    // 建立一個暫時層來放碎片，避免受父層 transform 影響
    const fragmentLayer = document.createElement("div");
    fragmentLayer.style.position = "fixed";
    fragmentLayer.style.left = centerX + "px";
    fragmentLayer.style.top = centerY + "px";
    fragmentLayer.style.width = "0";
    fragmentLayer.style.height = "0";
    fragmentLayer.style.zIndex = "10000";
    fragmentLayer.style.pointerEvents = "none";
    document.body.appendChild(fragmentLayer);

    for (let i = 0; i < fragmentCount; i++) {
        const fragment = document.createElement("i");
        fragment.className = "bi " + iconClass + " reaction-fragment";

        // 計算圓形噴射角度
        const angle = (Math.PI * 2 * i) / fragmentCount;
        const velocity = 100 + Math.random() * 150; // 噴射距離

        const tx = Math.cos(angle) * velocity + "px";
        const ty = Math.sin(angle) * velocity + "px";

        fragment.style.setProperty("--x", tx);
        fragment.style.setProperty("--y", ty);

        fragmentLayer.appendChild(fragment);
    }

    // 碎片動畫結束後移除層
    setTimeout(() => {
        fragmentLayer.remove();
    }, 1000);
}