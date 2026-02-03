function showCartToast() {
    const toastEl = document.getElementById('cartToast');
    if (!toastEl) return;

    const toast = new bootstrap.Toast(toastEl, { delay: 1500 });
    toast.show();
}

function addToCart(btn) {

    const productId = btn.dataset.id;

    fetch('/Cart/AddToCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken':
                document.querySelector('input[name="__RequestVerificationToken"]')?.value
        },
        body: JSON.stringify({ productId: productId })
    })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                playFlyAnimation(btn);     // 飛入動畫
                showCartToast();           // Toast 提示
                refreshCartModal();        // 更新購物車 Modal
                refreshCartCount();        // 更新 Badge
            }
        })
        .catch(err => console.error("AddToCart error:", err));
}

// 飛入動畫（圖片飛向購物車）
function playFlyAnimation(btn) {

    const row = btn.closest('.row');
    if (!row) return;

    const img = row.querySelector('.carousel-item.active img');
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

    setTimeout(() => {
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

// 更新購物車 Modal
function refreshCartModal() {
    fetch('/Cart/GetCartModal')
        .then(res => res.text())
        .then(html => {
            const modalBody = document.getElementById('cartModalBody');
            if (!modalBody) return;

            modalBody.innerHTML = html;
        });
}
// 更新 Badge 數量
function refreshCartCount() {
    fetch('/Cart/GetCartCount')
        .then(res => res.json())
        .then(data => {

            const badge = document.getElementById('cartCountBadge');
            if (!badge) return;

            badge.textContent = data.count;

            // 0 則隱藏
            if (data.count === 0) {
                badge.style.display = "none";
            } else {
                badge.style.display = "inline-block";
            }
        })
        .catch(err => console.error("refreshCartCount error:", err));
}
// 頁面載入時初始化 Badge
document.addEventListener("DOMContentLoaded", () => {
    refreshCartCount();
});

function removeFromCart(productId) {

    fetch('/Cart/RemoveFromModal', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken':
                document.querySelector('input[name="__RequestVerificationToken"]')?.value
        },
        body: JSON.stringify({ productId: productId })
    })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                refreshCartModal();   // 重新載入 Modal
                refreshCartCount();   // 更新 Badge
            }
        })
        .catch(err => console.error("removeFromCart error:", err));
}
