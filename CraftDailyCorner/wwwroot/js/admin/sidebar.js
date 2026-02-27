// ===============================
// Admin Sidebar Auto Refresh
// ===============================

document.addEventListener("DOMContentLoaded", function () {

    // 每 60 秒更新一次
    setInterval(refreshSidebarBadges, 60000);

});

async function refreshSidebarBadges() {

    try {

        const response = await fetch("/Admin/AdminSidebar/GetSidebarData");

        const result = await response.json();

        if (!result.success) return;

        updateBadge("badgeCreators", result.data.pendingCreators);
        updateBadge("badgeViolations", result.data.pendingViolations);

    } catch (err) {
        console.error("Sidebar refresh failed", err);
    }
}

function updateBadge(elementId, value) {

    const el = document.getElementById(elementId);

    if (!el) return;

    if (value > 0) {
        el.textContent = value;
        el.classList.remove("d-none");
    } else {
        el.classList.add("d-none");
    }
}