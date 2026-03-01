document.addEventListener("DOMContentLoaded", function () {

    const dataElement = document.getElementById("commerceChartData");
    if (!dataElement) return;

    const revenueTrendData = JSON.parse(dataElement.dataset.revenue || "[]");
    const orderTrendData = JSON.parse(dataElement.dataset.orders || "[]");

    // ===== Revenue =====
    const rLabels = revenueTrendData.map(x => x.monthLabel || x.MonthLabel);
    const rValues = revenueTrendData.map(x => x.revenue || x.Revenue);

    const revenueCtx = document.getElementById("revenueTrendChart");
    if (revenueCtx) {
        new Chart(revenueCtx, {
            type: "line",
            data: {
                labels: rLabels,
                datasets: [{
                    label: "月營收",
                    data: rValues,
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                return " $" + Number(context.raw).toLocaleString();
                            }
                        }
                    }
                }
            }
        });
    }

    // ===== Orders =====
    const oLabels = orderTrendData.map(x => x.monthLabel || x.MonthLabel);
    const oValues = orderTrendData.map(x => x.orderCount || x.OrderCount);

    const orderCtx = document.getElementById("orderTrendChart");
    if (orderCtx) {
        new Chart(orderCtx, {
            type: "line",
            data: {
                labels: oLabels,
                datasets: [{
                    label: "月訂單數",
                    data: oValues,
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false
            }
        });
    }

});