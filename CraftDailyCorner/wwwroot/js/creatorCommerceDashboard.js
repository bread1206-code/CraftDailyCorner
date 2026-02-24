document.addEventListener("DOMContentLoaded", function () {

    const dataElement = document.getElementById("commerceChartData");
    if (!dataElement) return;

    const revenueTrendData = JSON.parse(dataElement.dataset.revenue || "[]");

    const labels = revenueTrendData.map(x => x.monthLabel || x.MonthLabel);
    const revenueValues = revenueTrendData.map(x => x.revenue || x.Revenue);

    const ctx = document.getElementById("revenueTrendChart");

    if (ctx) {
        new Chart(ctx, {
            type: "line",
            data: {
                labels: labels,
                datasets: [{
                    label: "月營收",
                    data: revenueValues,
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
                                return " $" + context.raw.toLocaleString();
                            }
                        }
                    }
                }
            }
        });
    }

});