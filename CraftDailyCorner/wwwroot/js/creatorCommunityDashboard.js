document.addEventListener("DOMContentLoaded", function () {
    const cards = document.querySelectorAll(".js-community-chart-card");
    if (!cards.length) return;

    cards.forEach(initCommunityChartCard);
});

function initCommunityChartCard(card) {
    const apiUrl = card.dataset.apiUrl;
    const chartKey = card.dataset.chartKey;
    const valueType = card.dataset.valueType || "count";
    const defaultMode = card.dataset.defaultMode || "year";

    const canvas = card.querySelector("canvas");
    const yearFilter = card.querySelector(".js-filter-year");
    const rolling12Filter = card.querySelector(".js-filter-rolling12");
    const monthFilter = card.querySelector(".js-filter-month");

    const yearSelect = card.querySelector(".js-year-select");
    const rolling12Select = card.querySelector(".js-rolling12-select");
    const monthSelect = card.querySelector(".js-month-select");

    const modeButtons = card.querySelectorAll(".js-mode-btn");
    const rangeText = card.querySelector(".js-range-text");
    const growthBadge = card.querySelector(".js-growth-badge");
    const loadingText = card.querySelector(".js-loading-text");

    let chartInstance = null;
    let currentMode = defaultMode;

    function setActiveMode(mode) {
        currentMode = mode;

        modeButtons.forEach(btn => {
            btn.classList.toggle("active", btn.dataset.mode === mode);
        });

        yearFilter.classList.toggle("d-none", mode !== "year");
        rolling12Filter.classList.toggle("d-none", mode !== "rolling12");
        monthFilter.classList.toggle("d-none", mode !== "month");
    }

    function buildQueryString() {
        const params = new URLSearchParams();
        params.append("mode", currentMode);

        if (currentMode === "year") {
            params.append("year", yearSelect.value);
        } else if (currentMode === "rolling12") {
            const value = rolling12Select.value; // yyyy-MM
            const [year, month] = value.split("-");
            params.append("endYear", year);
            params.append("endMonth", month);
        } else if (currentMode === "month") {
            const value = monthSelect.value; // yyyy-MM
            const [year, month] = value.split("-");
            params.append("year", year);
            params.append("month", month);
        }

        return params.toString();
    }

    function setLoading(isLoading) {
        loadingText.classList.toggle("d-none", !isLoading);

        modeButtons.forEach(btn => btn.disabled = isLoading);
        if (yearSelect) yearSelect.disabled = isLoading;
        if (rolling12Select) rolling12Select.disabled = isLoading;
        if (monthSelect) monthSelect.disabled = isLoading;
    }

    function formatTooltipLabel(rawValue) {
        const value = Number(rawValue || 0);

        if (valueType === "currency") {
            return " $" + value.toLocaleString();
        }

        return " " + value.toLocaleString();
    }

    function updateGrowthBadge(growthRate) {
        if (!growthBadge) return;

        if (growthRate === null || growthRate === undefined) {
            growthBadge.classList.add("d-none");
            growthBadge.textContent = "";
            return;
        }

        growthBadge.classList.remove("d-none");
        growthBadge.textContent = `成長率 ${(Number(growthRate)).toLocaleString(undefined, {
            style: "percent",
            minimumFractionDigits: 0,
            maximumFractionDigits: 1
        })}`;
    }

    function renderChart(data) {
        const labels = data.labels || [];
        const values = data.values || [];

        if (rangeText) {
            rangeText.textContent = data.rangeText || "";
        }

        updateGrowthBadge(data.growthRate);

        if (chartInstance) {
            chartInstance.destroy();
        }

        chartInstance = new Chart(canvas, {
            type: "line",
            data: {
                labels: labels,
                datasets: [{
                    label: data.title || chartKey,
                    data: values,
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                interaction: {
                    mode: "index",
                    intersect: false
                },
                plugins: {
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                return formatTooltipLabel(context.raw);
                            }
                        }
                    },
                    legend: {
                        display: true
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }

    async function loadChart() {
        try {
            setLoading(true);

            const queryString = buildQueryString();
            const response = await fetch(`${apiUrl}?${queryString}`, {
                method: "GET",
                headers: {
                    "X-Requested-With": "XMLHttpRequest"
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }

            const data = await response.json();
            renderChart(data);
        } catch (error) {
            console.error(`[${chartKey}] load failed:`, error);

            if (chartInstance) {
                chartInstance.destroy();
                chartInstance = null;
            }

            if (rangeText) {
                rangeText.textContent = "載入失敗";
            }

            updateGrowthBadge(null);
        } finally {
            setLoading(false);
        }
    }

    modeButtons.forEach(btn => {
        btn.addEventListener("click", function () {
            const mode = btn.dataset.mode;
            setActiveMode(mode);
            loadChart();
        });
    });

    if (yearSelect) {
        yearSelect.addEventListener("change", loadChart);
    }

    if (rolling12Select) {
        rolling12Select.addEventListener("change", loadChart);
    }

    if (monthSelect) {
        monthSelect.addEventListener("change", loadChart);
    }

    setActiveMode(defaultMode);
    loadChart();
}