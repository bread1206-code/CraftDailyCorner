// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// 開啟登入視窗
let pendingAddToCartProductId = null;

function openLoginModal(productId) {
    pendingAddToCartProductId = productId;

    const modal = new bootstrap.Modal(
        document.getElementById('loginModal')
    );
    modal.show();
}

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
    const form = document.getElementById('loginForm');
    const formData = new FormData(form);

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