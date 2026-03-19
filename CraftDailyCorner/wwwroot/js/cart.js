// 登入前暫存「使用者想做的加購行為」
let pendingAddToCart = null;

// 初始化 Badge
function initCartBadge() {
    fetch('/Cart/GetCartCount')
        .then(r => r.json())
        .then(count => updateBadge(count))
        .catch(() => updateBadge(0));
}

function updateBadge(count) {
    const badge = document.querySelector('.cart-badge');
    if (!badge) return;

    badge.textContent = count;
    badge.style.display = count > 0 ? 'inline-block' : 'none';
}

// 加入購物車
function addToCart(productId, btn) {

    const qtyInput = document.querySelector('#qty');
    const quantity = qtyInput ? parseInt(qtyInput.value) : 1;
    // 未登入：記住「使用者原本要做的事」
    if (!window.isAuthenticated) {
        pendingAddToCart = {
            productId: productId,
            quantity: quantity
        };

        prepareLoginModal(); // 填 returnUrl
        openLoginModal();    // 開登入 Modal
        return;
    }

    // 已登入：正常加入購物車
    fetch('/Cart/AddItem', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': getCsrfToken()
        },
        body: JSON.stringify({
            productId,
            quantity
        })
    })
        .then(r => r.json())
        .then(res => {
            if (!res.success) {
                alert(res.message);
                return;
            }

            updateBadge(res.summary.totalQuantity);
            if (btn) {
                playFlyAnimation(btn);
            }
        });
}

// 更新數量
function updateQuantity(productId, quantity) {
    quantity = parseInt(quantity, 10);
    if (isNaN(quantity) || quantity < 1) {
        alert('數量必須是大於 0 的整數');
        return;
    }

    fetch('/Cart/UpdateQuantity', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': getCsrfToken()
        },
        body: JSON.stringify({
            productId,
            quantity
        })
    })
        .then(r => r.json())
        .then(res => {
            if (!res.success) {
                alert(res.message);
                return;
            }

            updateBadge(res.summary.totalQuantity);
            reloadCartModal();
        });
}

//移除商品
function removeFromCart(productId) {
    fetch('/Cart/RemoveItem', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': getCsrfToken()
        },
        body: JSON.stringify({ productId })
    })
        .then(r => r.json())
        .then(res => {
            updateBadge(res.summary.totalQuantity);
            reloadCartModal();

        });
}



// Modal 操作
function reloadCartModal() {
    fetch('/Cart/GetCartItems')
        .then(r => r.text())
        .then(html => {
            document.querySelector('#cartModalBody').innerHTML = html;
        });
}

// CSRF Token

function getCsrfToken() {
    return document.querySelector('input[name="__RequestVerificationToken"]')?.value;
}

// 飛入動畫
function playFlyAnimation(btn) {

    const row = btn.closest('.row');
    if (!row) return;

    const img = document.querySelector('#productCarousel .carousel-item.active img');

    if (!img) return;

    const cartIcon = document.getElementById('cartIcon');
    if (!cartIcon) return;

    const imgClone = img.cloneNode(true);

    const imgRect = img.getBoundingClientRect();
    const cartRect = cartIcon.getBoundingClientRect();

    imgClone.classList.remove("w-100");
    imgClone.style.margin = "0";
    imgClone.style.maxWidth = "none";
    imgClone.style.objectFit = "cover";

    imgClone.style.position = "fixed";
    imgClone.style.left = imgRect.left + "px";
    imgClone.style.top = imgRect.top + "px";
    imgClone.style.width = imgRect.width + "px";
    imgClone.style.height = imgRect.height + "px";
    imgClone.style.transition = "all 0.8s ease-in-out";
    imgClone.style.zIndex = 9999;
    imgClone.style.borderRadius = "8px";

    document.body.appendChild(imgClone);

    const targetX = cartRect.left + cartRect.width / 2;
    const targetY = cartRect.top + cartRect.height / 2;

    const finalSize = 30;

    requestAnimationFrame(() => {
        imgClone.style.left = (targetX - finalSize / 2) + "px";
        imgClone.style.top = (targetY - finalSize / 2) + "px";
        imgClone.style.width = finalSize + "px";
        imgClone.style.height = finalSize + "px";
        imgClone.style.opacity = "0.4";
    }, 50);

    setTimeout(() => {
        imgClone.remove();
    }, 900);
}

// 初始化
document.addEventListener('DOMContentLoaded', () => {
    initCartBadge();

    const cartModal = document.getElementById('CartModal');
    if (cartModal) {
        cartModal.addEventListener('show.bs.modal', () => {
            reloadCartModal();
        });
    }
});

//
document.addEventListener('change', function (e) {
    // 1. 創作者主 Checkbox
    if (e.target.classList.contains('creator-check')) {
        const creatorId = e.target.dataset.creatorId;
        const isChecked = e.target.checked;

        if (isChecked) {
            // 強制只能選一個創作者：取消其他創作者的勾選
            document.querySelectorAll('.creator-check').forEach(cb => {
                if (cb !== e.target) cb.checked = false;
            });
            document.querySelectorAll('.product-check').forEach(cb => {
                cb.checked = (cb.dataset.creatorId === creatorId);
            });
        } else {
            document.querySelectorAll(`.product-check[data-creator-id="${creatorId}"]`)
                .forEach(cb => cb.checked = false);
        }
        updateCartSummary();
    }

    // 2. 單一商品 Checkbox
    if (e.target.classList.contains('product-check')) {
        const creatorId = e.target.dataset.creatorId;
        const isChecked = e.target.checked;

        if (isChecked) {
            // 如果勾選了不同創作者的商品，取消其他人的勾選
            document.querySelectorAll('.product-check').forEach(cb => {
                if (cb.dataset.creatorId !== creatorId) cb.checked = false;
            });
            document.querySelectorAll('.creator-check').forEach(cb => {
                cb.checked = (cb.dataset.creatorId === creatorId);
            });
        }

        // 檢查是否該創作者下的商品全取消了
        const creatorCheckbox = document.getElementById(`check-${creatorId}`);
        const siblings = document.querySelectorAll(`.product-check[data-creator-id="${creatorId}"]`);
        const anyChecked = Array.from(siblings).some(s => s.checked);

        creatorCheckbox.checked = anyChecked; // 只要有一個商品選中，標題就勾選

        updateCartSummary();
    }
});

// 更新底部金額與按鈕狀態
function updateCartSummary() {
    let total = 0;
    let count = 0;
    const checkedProducts = document.querySelectorAll('.product-check:checked');

    checkedProducts.forEach(cb => {
        // 從 DOM 取得小計金額 (簡單做法是把金額存進 data-price)
        const row = cb.closest('.row');
        const subtotal = parseInt(row.querySelector('strong').textContent.replace('$', ''));
        total += subtotal;
        count++;
    });

    document.getElementById('selected-count').textContent = count;
    document.getElementById('selected-amount').textContent = total.toLocaleString();
    document.getElementById('checkout-btn').disabled = (count === 0);
}

// 導向結帳頁 (需告知後端選了哪個創作者)
function goToCheckout() {
    // 新增：取得目前所有勾選的商品
    const checkedProducts = document.querySelectorAll('.product-check:checked');

    if (checkedProducts.length === 0) {
        alert('請先勾選要結帳的商品');
        return;
    }

    // 新增：因為前面已限制只能勾同一位創作者，直接取第一筆 creatorId
    const creatorId = checkedProducts[0].dataset.creatorId;

    // 新增：把勾選的商品 ProductId 一起帶到結帳頁
    const selectedProductIds = Array.from(checkedProducts)
        .map(cb => cb.dataset.productId)
        .filter(id => id);

    if (!creatorId || selectedProductIds.length === 0) {
        alert('結帳商品資料不完整，請重新勾選');
        return;
    }

    const query = new URLSearchParams({
        creatorId: creatorId,
        selectedProductIds: selectedProductIds.join(',')
    });

    // 導向結帳，並帶入 CreatorID 與本次勾選商品清單
    window.location.href = `/Orders/Checkout?${query.toString()}`;
}