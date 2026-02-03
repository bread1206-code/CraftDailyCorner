// Toast
function showCartToast(message = "已加入購物車") {
    const toastEl = document.getElementById('cartToast');
    if (!toastEl) return;

    toastEl.querySelector('.toast-body').innerText = message;
    const toast = bootstrap.Toast.getOrCreateInstance(toastEl, { delay: 1500 });
    toast.show();
}

// 加入購物車
function addToCart(btn) {

    const productId = btn.dataset.id;
    const qtyInput = document.getElementById("qty");

    if (!productId || !qtyInput) {
        alert("資料錯誤，請重新整理");
        return;
    }

    const qty = parseInt(qtyInput.value, 10);

    if (isNaN(qty) || qty <= 0) {
        alert("請輸入正確數量");
        return;
    }

    fetch('/Cart/Add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            productId: productId,
            qty: qty
        })
    })
        .then(async res => {
            const data = await res.json().catch(() => null);
            return { ok: res.ok, data };
        })
        .then(result => {

            if (!result.ok || !result.data) {
                alert("加入購物車失敗");
                return;
            }

            if (!result.data.success) {
                alert(result.data.message || "加入失敗");
                return;
            }

            // 成功
            refreshCartModal();
            refreshCartCount(result.data.cartQty);
            showCartToast(result.data.message);
            playFlyAnimation(btn);
        })
        .catch(err => {
            console.error("AddToCart error:", err);
            alert("系統錯誤，請稍後再試");
        });
}

// 更新購物車 Modal
function refreshCartModal() {
    fetch('/Cart/GetCartModal')
        .then(res => res.text())
        .then(html => {
            const modalBody = document.getElementById('cartModalBody');
            if (modalBody) modalBody.innerHTML = html;
        });
}


// 更新 Badge
function refreshCartCount(countFromServer) {

    const badge = document.getElementById('cartCountBadge');
    if (!badge) return;

    if (typeof countFromServer === "number") {
        badge.innerText = countFromServer;
    } else {
        // fallback
        fetch('/Cart/GetCartCount')
            .then(res => res.json())
            .then(data => badge.innerText = data.count);
    }

    badge.style.display =
        parseInt(badge.innerText, 10) > 0 ? "inline-block" : "none";
}


// 移除商品
function removeFromCart(productId) {

    fetch('/Cart/Remove', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ productId })
    })
        .then(res => res.json())
        .then(data => {

            if (!data.success) {
                alert(data.message || "移除失敗");
                return;
            }

            refreshCartModal();
            refreshCartCount(data.cartQty);
        })
        .catch(err => {
            console.error("removeFromCart error:", err);
            alert("系統錯誤");
        });
}


// 初始載入 Badge
document.addEventListener("DOMContentLoaded", () => {
    refreshCartCount();
});
