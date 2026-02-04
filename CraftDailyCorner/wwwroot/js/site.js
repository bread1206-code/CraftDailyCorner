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
    const returnUrl = window.location.pathname + window.location.search;

    const input = document.querySelector('#loginModal input[name="returnUrl"]');
    if (input) {
        input.value = returnUrl;
    }

    new bootstrap.Modal(document.getElementById('loginModal')).show();
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