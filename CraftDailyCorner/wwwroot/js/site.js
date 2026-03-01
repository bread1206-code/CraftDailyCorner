
// 開啟登入視窗
let pendingAddToCartProductId = null;

// DOMContentLoaded 是為了確保 HTML 元素都載入後才綁定事件
document.addEventListener("DOMContentLoaded", function () {

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

                if (response.status === 401) {
                    btn.classList.remove("loading");
                    openLoginModal();
                    return;
                }

                if (!response.ok) {
                    alert("操作失敗，請稍後再試1");
                    return;
                }

                const result = await response.json();

                // 只在「收藏成功」時，加蓋章動畫
                if (result.isFavorite) {
                    btn.classList.add("stamping");
                    showFavoriteStamp();
                }
                if (!result.isFavorite) {
                    // ⭐ 只有在「我的收藏頁」才移除商品卡片
                    const isFavoritePage = document.getElementById("favorite-page") !== null;

                    if (isFavoritePage) {
                        const card = btn.closest(".col-12");
                        if (card) {
                            card.remove();
                        }
                    }
                }

                // 更新 icon / 文字
                updateFavoriteButton(btn, result.isFavorite);

            } catch (err) {
                console.error("fetch error:", err);
                alert("系統錯誤，請稍後再試2");

            } finally {
                // ⭐ 動畫結束後解除狀態
                setTimeout(() => {
                    btn.classList.remove("loading", "stamping");
                }, 1500); // 要跟 CSS animation 時間一致
            }
        });

    });
});



//圖片預覽功能
window.previewImage = function (options) {
    const {
        input,
        previewSelector,
        fallbackSrc = null
    } = options;

    if (!input.files || input.files.length === 0) {
        return;
    }

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
// 登入成功後處理
function openLoginModal() {
    const modalEl = document.getElementById('loginModal');

    //  防呆：Modal 不存在就直接停
    if (!modalEl) {
        console.error('loginModal 不存在於 DOM');
        return;
    }

    // 填 returnUrl
    const returnUrl = window.location.pathname + window.location.search;
    const input = modalEl.querySelector('input[name="returnUrl"]');
    if (input) {
        input.value = returnUrl;
    }

    //  Bootstrap 5 正確打開方式（不重複初始化）
    let modal = bootstrap.Modal.getInstance(modalEl);
    if (!modal) {
        modal = new bootstrap.Modal(modalEl);
    }

    modal.show();
}

// 設定登入後的返回 URL
function prepareLoginModal() {
    const returnUrl = window.location.pathname + window.location.search;

    const input = document.querySelector(
        '#loginModal input[name="returnUrl"]'
    );

    if (input) {
        input.value = returnUrl;
    }
}
//AJAX 登入
function ajaxLogin() {
    const form = $('#loginForm');

    //前端驗證
    if (!form.valid()) {
        return; // 驗證不通過就不要送出
    }
    const formData = new FormData(form[0]);

    fetch('/Account/Login', {
        method: 'POST',
        body: formData,
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
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

            // 登入成功
            onLoginSuccess();
            location.reload();
        })
        .catch(() => {
            showLoginError('系統錯誤，請稍後再試');
        });
}

function showLoginError(msg) {
    const el = document.getElementById('loginError');
    el.textContent = msg;
    el.classList.remove('d-none');
}
//註冊Ajax版本
function ajaxRegister() {
    const form = $('#registerForm');

    //前端驗證
    if (!form.valid()) {
        return;
    }

    const formData = new FormData(form[0]);

    fetch('/Account/Register', {
        method: 'POST',
        body: formData,
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
        .then(r => r.json())
        .then(res => {

            if (!res.success) {
                //處理欄位錯誤
                if (res.errors) {
                    showRegisterErrors(res.errors);
                }
                //處理整體錯誤
                if (res.message) {
                    showRegisterError(res.message);
                }

                return;
            }
            // 註冊成功 = 已登入
            onLoginSuccess();
        })
        .catch(() => {
            showRegisterError('系統錯誤，請稍後再試');
        });
}

//登入成功後的處理
function onLoginSuccess() {
    window.isAuthenticated = true;

    // 關閉登入 Modal
    const modalEl = document.getElementById('loginModal');
    bootstrap.Modal.getInstance(modalEl)?.hide();

    // 刷新 Navbar（核心）
    fetch('/Home/Navbar')
        .then(r => r.text())
        .then(html => {
            document.getElementById('navbarContainer').innerHTML = html;

            // Navbar 重新渲染後，重新初始化 Badge
            initCartBadge();
        });

    // 如果是為了加購而登入，自動完成
    if (pendingAddToCart) {
        addToCart(pendingAddToCart.productId);
        pendingAddToCart = null;
    }
}

function showRegisterError(msg) {
    const el = document.getElementById('registerError');
    el.textContent = msg;
    el.classList.remove('d-none');
}
function showRegisterErrors(errors) {
    for (const key in errors) {
        const span = document.querySelector(
            `#registerForm span[data-valmsg-for="${key}"]`
        );
        if (span) {
            span.textContent = errors[key];
        }
    }
}

function clearRegisterErrors() {
    document
        .querySelectorAll('#registerForm span[data-valmsg-for]')
        .forEach(s => s.textContent = '');
}
function switchToRegister() {
    // Bootstrap Tab 切換
    const registerTabBtn = document.querySelector(
        '[data-bs-target="#registerTab"]'
    );

    if (!registerTabBtn) return;

    bootstrap.Tab.getOrCreateInstance(registerTabBtn).show();
    //重新解析驗證規則
    $.validator.unobtrusive.parse('#registerForm');
}

//收藏商品
function initFavoriteButton() {
    const btn = document.getElementById("favoriteBtn");
    if (!btn) return;

    btn.addEventListener("click", async function () {
        const productId = btn.dataset.productId;

        try {
            const response = await fetch("/api/favorite/toggle", {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded"
                },
                body: `productId=${encodeURIComponent(productId)}`
            });

            if (response.status === 401) {
                alert("請先登入會員");
                return;
            }

            if (!response.ok) {
                alert("操作失敗，請稍後再試");
                return;
            }

            const result = await response.json();
            updateFavoriteButton(btn, result.isFavorite);

        } catch (err) {
            console.error(err);
            alert("系統錯誤，請稍後再試");
        }
    });
}
// 更新收藏按鈕的外觀
function updateFavoriteButton(btn, isFavorite) {

    const icon = btn.querySelector("i");
    const text = btn.querySelector("span"); // 詳情頁才有

    if (isFavorite) {
        // icon
        icon.classList.remove("bi-heart", "text-muted");
        icon.classList.add("bi-heart-fill", "text-danger");

        // text（商品詳情頁）
        if (text) {
            text.textContent = "已收藏";
        }

        btn.dataset.isFavorite = "true";
    } else {
        icon.classList.remove("bi-heart-fill", "text-danger");
        icon.classList.add("bi-heart", "text-muted");

        if (text) {
            text.textContent = "收藏";
        }

        btn.dataset.isFavorite = "false";
    }
}
function showFavoriteStamp() {
    const overlay = document.getElementById("favorite-stamp-overlay");
    if (!overlay) return;

    overlay.classList.add("show");
    overlay.style.display = "block";

    setTimeout(() => {
        overlay.classList.remove("show");
        overlay.style.display = "none";
    }, 800); // 跟動畫時間一致
}

//檢舉時控制顯示檢舉原因欄位 
document.addEventListener("change", function (e) {

    if (!e.target.classList.contains("report-reason"))
        return;

    const modal = e.target.closest(".modal");
    if (!modal)
        return;

    const textarea = modal.querySelector(".report-description");
    if (!textarea)
        return;

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

/*檢舉 */
    // AJAX 檢舉送出
    document.addEventListener("submit", async function(e) {

        if (!e.target.classList.contains("report-form"))
            return;

        e.preventDefault();

        const form = e.target;
        const formData = new FormData(form);

        const token = form.querySelector("input[name='__RequestVerificationToken']").value;

        try {

            const response = await fetch(form.action, {
                method: "POST",
                headers: {
                    "RequestVerificationToken": token
                },
                body: formData
            });
            if (!response.ok) {
                showReportStamp("發生錯誤");
                return;
            }
            const data = await response.json();

            if (data.result === "Success") {
                    showReportStamp("檢舉成功");
                }
                else if (data.result === "AlreadyReported") {
                    showReportStamp("您已檢舉過");
                }

            // 關閉 modal
            const modalEl = form.closest(".modal");
            const modal = bootstrap.Modal.getInstance(modalEl);
            if (modal) modal.hide();

        } catch (error) {
            showToast("Error");
        }

    });
/*動畫 */
function showReportStamp(text = "檢舉已送出") {

    const overlay = document.getElementById("report-stamp-overlay");
    if (!overlay) return;

    // 動態改文字
    overlay.style.setProperty("--stamp-text", `"${text}"`);

    overlay.classList.add("show");
    overlay.style.display = "block";

    // 動畫結束後隱藏
    overlay.addEventListener("animationend", function handler() {
        overlay.classList.remove("show");
        overlay.style.display = "none";
        overlay.removeEventListener("animationend", handler);
    });
} 