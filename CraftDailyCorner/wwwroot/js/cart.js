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

//為了加購而登入成功後的處理
function onLoginSuccess() {
    window.isAuthenticated = true;

    // 關閉登入 Modal
    const modalEl = document.getElementById('loginModal');
    const modal = bootstrap.Modal.getInstance(modalEl);
    if (modal) modal.hide();

    // 如果是「為了加購而登入」，自動完成它
    if (pendingAddToCart) {
        addToCart(pendingAddToCart.productId);
        pendingAddToCart = null;
    }
}
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