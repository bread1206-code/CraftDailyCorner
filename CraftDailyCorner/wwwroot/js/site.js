/* ==================================================
 * 1) DOM Ready：初始化入口
 * ================================================== */

document.addEventListener("DOMContentLoaded", function () {
    initFavoriteButtons();      // 收藏
    
});

/* ==================================================
 * 2) 收藏 Favorite（核心：按鈕 click + UI 更新 + 動畫）
 * ================================================== */

/**
 * 綁定收藏按鈕事件
 * 目前策略：載入時 querySelectorAll 綁 click
 * 注意：若你未來有 AJAX 動態載入商品卡片，新的 .favorite-btn 不會自動被綁到
 * （那時候再改事件委派或再次呼叫 initFavoriteButtons）
 */
function initFavoriteButtons() {
    document.querySelectorAll(".favorite-btn").forEach(btn => {

        btn.addEventListener("click", async function (e) {
            e.preventDefault();
            e.stopPropagation();

            // 如果正在 loading，直接擋掉
            if (btn.classList.contains("loading")) return;

            const productId = btn.dataset.productId;

            // 先進入 loading（防連點）
            btn.classList.add("loading");

            try {
                const response = await fetch("/api/favorite/toggle", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded"
                    },
                    body: `productId=${encodeURIComponent(productId)}`
                });

                // 未登入：打開登入 Modal
                if (response.status === 401) {
                    btn.classList.remove("loading");
                    openLoginModal();
                    return;
                }

                if (!response.ok) {
                    alert("操作失敗，請稍後再試135");
                    return;
                }

                const result = await response.json();

                // 只在「收藏成功」時，加蓋章動畫
                if (result.isFavorite) {
                    btn.classList.add("stamping");
                    showFavoriteStamp();
                }

                // 取消收藏：只有在「我的收藏頁」才移除商品卡片
                if (!result.isFavorite) {
                    const isFavoritePage = document.getElementById("favorite-page") !== null;
                    if (isFavoritePage) {
                        const card = btn.closest(".col-12");
                        if (card) card.remove();
                    }
                }

                // 更新 icon / 文字
                updateFavoriteButton(btn, result.isFavorite);

            } catch (err) {
                console.error("fetch error:", err);
                alert("系統錯誤，請稍後再試246");
            } finally {
                // 動畫結束後解除狀態
                setTimeout(() => {
                    btn.classList.remove("loading", "stamping");
                }, 1500);
            }
        });

    });
}

// 更新收藏按鈕的外觀
function updateFavoriteButton(btn, isFavorite) {
    const icon = btn.querySelector("i");
    const text = btn.querySelector("span"); // 詳情頁才有

    if (isFavorite) {
        icon?.classList.remove("bi-heart", "text-muted");
        icon?.classList.add("bi-heart-fill", "text-danger");

        if (text) text.textContent = "已收藏";

        btn.dataset.isFavorite = "true";
    } else {
        icon?.classList.remove("bi-heart-fill", "text-danger");
        icon?.classList.add("bi-heart", "text-muted");

        if (text) text.textContent = "收藏";

        btn.dataset.isFavorite = "false";
    }
}

// 收藏蓋章動畫
function showFavoriteStamp() {
    const overlay = document.getElementById("favorite-stamp-overlay");
    if (!overlay) return;

    overlay.classList.add("show");
    overlay.style.display = "block";

    setTimeout(() => {
        overlay.classList.remove("show");
        overlay.style.display = "none";
    }, 800);
}

/* ==================================================
 * 3) 圖片預覽 Preview
 * ================================================== */

window.previewImage = function (options) {
    const { input, previewSelector, fallbackSrc = null } = options;

    if (!input.files || input.files.length === 0) return;

    const file = input.files[0];

    // 前端 UX 防呆（後端仍需驗）
    if (!file.type.startsWith("image/")) {
        alert("請選擇圖片檔案");
        input.value = "";
        return;
    }

    const reader = new FileReader();

    reader.onload = function (e) {
        const img = document.querySelector(previewSelector);
        if (!img) return;

        img.src = e.target.result;
        img.classList.remove("d-none");
    };

    reader.readAsDataURL(file);
};

/* ==================================================
 * 4) 登入 / 註冊 Modal + AJAX（Login / Register）
 * ================================================== */

// 開啟登入 Modal（並填 returnUrl）
function openLoginModal() {
    const modalEl = document.getElementById('loginModal');

    // 防呆：Modal 不存在就停
    if (!modalEl) {
        console.error('loginModal 不存在於 DOM');
        return;
    }

    // 填 returnUrl
    const returnUrl = window.location.pathname + window.location.search;
    const input = modalEl.querySelector('input[name="returnUrl"]');
    if (input) input.value = returnUrl;

    // Bootstrap 5：不重複初始化
    let modal = bootstrap.Modal.getInstance(modalEl);
    if (!modal) modal = new bootstrap.Modal(modalEl);

    modal.show();
}

// 設定登入後的返回 URL（可能是你某些地方會手動呼叫）
function prepareLoginModal() {
    const returnUrl = window.location.pathname + window.location.search;
    const input = document.querySelector('#loginModal input[name="returnUrl"]');
    if (input) input.value = returnUrl;
}

// AJAX 登入
function ajaxLogin() {
    const form = $('#loginForm');

    // 前端驗證
    if (!form.valid()) return;

    const formData = new FormData(form[0]);

    fetch('/Account/Login', {
        method: 'POST',
        body: formData,
        headers: { 'X-Requested-With': 'XMLHttpRequest' }
    })
        .then(async r => {
            if (r.ok) return r.json();
            throw new Error('登入失敗');
        })
        .then(res => {
            if (!res.success) {
                showLoginError(res.message);
                return;
            }

            onLoginSuccess();
            playLoginSuccessAnimation("envelope");
        })
        .catch(() => {
            showLoginError('系統錯誤，請稍後再試');
        });
}

function showLoginError(msg) {
    const el = document.getElementById('loginError');
    if (!el) return;
    el.textContent = msg;
    el.classList.remove('d-none');
}

// 註冊 Ajax
function ajaxRegister() {
    const form = $('#registerForm');

    // 前端驗證
    if (!form.valid()) return;

    const formData = new FormData(form[0]);

    fetch('/Account/Register', {
        method: 'POST',
        body: formData,
        headers: { 'X-Requested-With': 'XMLHttpRequest' }
    })
        .then(r => r.json())
        .then(res => {
            if (!res.success) {
                if (res.errors) showRegisterErrors(res.errors);
                if (res.message) showRegisterError(res.message);
                return;
            }

            // 註冊成功 = 已登入
            playLoginSuccessAnimation("balloon");
        })
        .catch(() => {
            showRegisterError('系統錯誤，請稍後再試');
        });
}

// 登入成功後的處理
function onLoginSuccess() {
    window.isAuthenticated = true;

    // 關閉登入 Modal
    const modalEl = document.getElementById('loginModal');
    bootstrap.Modal.getInstance(modalEl)?.hide();

    // 刷新 Navbar
    fetch('/Home/Navbar')
        .then(r => r.text())
        .then(html => {
            const container = document.getElementById('navbarContainer');
            if (container) container.innerHTML = html;

            // Navbar 重新渲染後，重新初始化 Badge
            if (typeof initCartBadge === "function") {
                initCartBadge();
            }
        });

    // 如果是為了加購而登入，自動完成（注意：你用的是 pendingAddToCart 變數）
    if (typeof pendingAddToCart !== "undefined" && pendingAddToCart) {
        addToCart(pendingAddToCart.productId);
        pendingAddToCart = null;
    }
}

function showRegisterError(msg) {
    const el = document.getElementById('registerError');
    if (!el) return;
    el.textContent = msg;
    el.classList.remove('d-none');
}

function showRegisterErrors(errors) {
    for (const key in errors) {
        const span = document.querySelector(`#registerForm span[data-valmsg-for="${key}"]`);
        if (span) span.textContent = errors[key];
    }
}


function switchToRegister() {
    const registerTabBtn = document.querySelector('[data-bs-target="#registerTab"]');
    if (!registerTabBtn) return;

    bootstrap.Tab.getOrCreateInstance(registerTabBtn).show();

    // 重新解析驗證規則
    $.validator.unobtrusive.parse('#registerForm');
}

//登入動畫
function playLoginSuccessAnimation(type = "envelope") {
    const overlay = document.getElementById("login-anim-overlay");
    if (!overlay) {
        location.reload();
        return;
    }

    const envelope = overlay.querySelector(".login-anim-envelope");
    const balloon = overlay.querySelector(".login-anim-balloon");

    // Reset
    overlay.classList.add("show");
    envelope.classList.add("d-none");
    balloon.classList.add("d-none");
    envelope.classList.remove("play", "stamp");
    balloon.classList.remove("play", "pop");

    if (type === "balloon") {
        balloon.classList.remove("d-none");

        requestAnimationFrame(() => {
            balloon.classList.add("play");
        });

        // 先等氣球飄上來（balloonUp）結束，再觸發 pop
        balloon.addEventListener("animationend", function onBalloonUpEnd(e) {
            if (e.animationName !== "balloonUp") return;

            balloon.removeEventListener("animationend", onBalloonUpEnd);

            // 觸發爆炸（此時 gifts 會開始動畫）
            balloon.classList.add("pop");
            //在爆炸瞬間產生 30 個禮物
            createGiftBurst(balloon, 30);
            // ✅ 等所有禮物動畫結束再 reload（不管你有 6、12、20 個都OK）
            const gifts = balloon.querySelectorAll(".gift");

            // 若沒有 gift（或你未來改成 JS 動態生成但生成失敗），就給保底時間
            if (!gifts.length) {
                setTimeout(() => location.reload(), 900);
                return;
            }

            let remaining = gifts.length;
            let reloaded = false;

            const done = () => {
                if (reloaded) return;
                reloaded = true;
                location.reload();
            };

            // 保底：避免某些情況 animationend 沒觸發（例如 CSS 被改掉）
            const fallbackTimer = setTimeout(done, 1500);

            gifts.forEach(g => {
                g.addEventListener("animationend", () => {
                    remaining--;
                    if (remaining <= 0) {
                        clearTimeout(fallbackTimer);
                        done();
                    }
                }, { once: true });
            });
        });

        return;
    }

    // 預設：信封動畫
    envelope.classList.remove("d-none");

    requestAnimationFrame(() => {
        envelope.classList.add("play");
    });

    // 等信封放大完成後蓋章
    envelope.addEventListener("animationend", function handler(e) {

        if (e.animationName === "envelopeZoom") {
            envelope.classList.add("stamp");
            return;
        }

        if (e.animationName === "stampShake") {
            envelope.removeEventListener("animationend", handler);
            location.reload();
        }
    });
}
// 氣球爆炸後的禮物動畫
function createGiftBurst(container, count = 20) {

    for (let i = 0; i < count; i++) {

        const gift = document.createElement("div");
        gift.className = "gift";

        const icon = document.createElement("i");
        icon.className = Math.random() > 0.5
            ? "bi bi-gift-fill"
            : "bi bi-gift";

        gift.appendChild(icon);

        // 隨機方向
        const angle = Math.random() * 2 * Math.PI;
        const distance = 150 + Math.random() * 200;

        const x = Math.cos(angle) * distance;
        const y = Math.sin(angle) * distance;

        gift.style.setProperty("--x", `${x}px`);
        gift.style.setProperty("--y", `${y}px`);
        gift.style.color = randomColor();

        container.appendChild(gift);

        requestAnimationFrame(() => {
            gift.classList.add("burst");
        });

        setTimeout(() => gift.remove(), 900);
    }
}

function randomColor() {
    const colors = [
        "#ff4d6d", "#ff9f1c", "#ffd60a",
        "#2ec4b6", "#4361ee", "#9b5de5"
    ];
    return colors[Math.floor(Math.random() * colors.length)];
}

/* ==================================================
 * 5) 檢舉 Report（表單互動 + AJAX 送出 + 動畫）
 * ================================================== */

// 檢舉原因切換：顯示/隱藏「其他原因」輸入框
document.addEventListener("change", function (e) {
    if (!e.target.classList.contains("report-reason")) return;

    const modal = e.target.closest(".modal");
    if (!modal) return;

    const textarea = modal.querySelector(".report-description");
    if (!textarea) return;

    const isOther = e.target.dataset.isOther === "true";

    if (isOther) {
        textarea.classList.remove("d-none");
        textarea.setAttribute("required", "required");
    } else {
        textarea.classList.add("d-none");
        textarea.removeAttribute("required");
        textarea.value = "";
    }
});

// AJAX 檢舉送出
document.addEventListener("submit", async function (e) {
    if (!e.target.classList.contains("report-form")) return;

    e.preventDefault();

    const form = e.target;
    const formData = new FormData(form);

    const tokenInput = form.querySelector("input[name='__RequestVerificationToken']");
    const token = tokenInput ? tokenInput.value : null;

    try {
        const response = await fetch(form.action, {
            method: "POST",
            headers: token ? { "RequestVerificationToken": token } : {},
            body: formData
        });

        if (!response.ok) {
            showReportStamp("發生錯誤");
            return;
        }

        const data = await response.json();

        if (data.result === "Success") {
            showReportStamp("檢舉成功");
        } else if (data.result === "AlreadyReported") {
            showReportStamp("您已檢舉過");
        }

        // 關閉 modal
        const modalEl = form.closest(".modal");
        const modal = bootstrap.Modal.getInstance(modalEl);
        if (modal) modal.hide();

    } catch (error) {
        console.error("report submit error:", error);
        notifyError("檢舉送出失敗，請稍後再試或重新登入。");
    }
});

// 檢舉蓋章動畫
function showReportStamp(text = "檢舉已送出") {
    const overlay = document.getElementById("report-stamp-overlay");
    if (!overlay) return;

    overlay.style.setProperty("--stamp-text", `"${text}"`);

    overlay.classList.add("show");
    overlay.style.display = "block";

    overlay.addEventListener("animationend", function handler() {
        overlay.classList.remove("show");
        overlay.style.display = "none";
        overlay.removeEventListener("animationend", handler);
    });
}

//共用通知
function notifyError(message) {
    // 1) 有你自己的 toast 函式就用它
    if (typeof showToast === "function") {
        showToast(message);
        return;
    }

    // 2) 沒有 toast：在頁面右上角塞一個 Bootstrap alert（自動消失）
    let box = document.getElementById("global-alert-box");
    if (!box) {
        box = document.createElement("div");
        box.id = "global-alert-box";
        box.style.position = "fixed";
        box.style.top = "16px";
        box.style.right = "16px";
        box.style.zIndex = "2000";
        document.body.appendChild(box);
    }

    const el = document.createElement("div");
    el.className = "alert alert-danger shadow-sm";
    el.textContent = message;

    box.appendChild(el);

    setTimeout(() => el.remove(), 3000);
}