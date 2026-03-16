// ===============================
// Admin Dashboard JS
// ===============================

let isLoading = false;
let transactionChart = null;
let memberChart = null;

//初始化

document.addEventListener("DOMContentLoaded", function () {

    initChart();

    // 預設載入本月
    loadRangeData("month");

    // 區間按鈕
    document.querySelectorAll("[data-range]").forEach(btn => {
        btn.addEventListener("click", function () {

            if (isLoading) return;

            document.querySelectorAll("[data-range]").forEach(b => b.classList.remove("active"));
            this.classList.add("active");

            const range = this.getAttribute("data-range");

            // 清除歷史月份選單
            document.getElementById("historyMonthSelect").value = "";

            loadRangeData(range);
        });
    });

    // 歷史月份
    document.getElementById("historyMonthSelect")
        .addEventListener("change", function () {

            if (isLoading) return;

            const month = this.value;

            if (!month) return;

            document.querySelectorAll("[data-range]").forEach(b => b.classList.remove("active"));

            loadHistoryData(month);
        });
});

//初始化圖表

function initChart() {

    const ctx1 = document.getElementById("transactionChart");
    const ctx2 = document.getElementById("memberChart");

    const axisTextColor = "#cbd5e1";
    const gridColor = "rgba(148,163,184,0.15)";

    transactionChart = new Chart(ctx1, {
        type: 'line',
        data: {
            labels: [],
            datasets: [
                {
                    label: '訂單數',
                    data: [],
                    borderColor: '#3b82f6',
                    backgroundColor: 'rgba(59,130,246,0.1)',
                    pointBackgroundColor: '#3b82f6',
                    tension: 0.3
                },
                {
                    label: '營收',
                    data: [],
                    borderColor: '#22c55e',
                    backgroundColor: 'rgba(34,197,94,0.1)',
                    pointBackgroundColor: '#22c55e',
                    tension: 0.3,
                    yAxisID: 'y1'
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    labels: {
                        color: axisTextColor
                    }
                },
                tooltip: {
                    backgroundColor: '#1e293b',
                    titleColor: '#fff',
                    bodyColor: '#e2e8f0',
                    borderColor: '#334155',
                    borderWidth: 1
                }
            },
            scales: {
                x: {
                    ticks: { color: axisTextColor },
                    grid: { color: gridColor }
                },
                y: {
                    ticks: { color: axisTextColor },
                    grid: { color: gridColor },
                    beginAtZero: true
                },
                y1: {
                    ticks: { color: axisTextColor },
                    grid: { drawOnChartArea: false },
                    position: 'right',
                    beginAtZero: true
                }
            }
        }
    });

    memberChart = new Chart(ctx2, {
        type: 'bar',
        data: {
            labels: [],
            datasets: [
                {
                    label: '新增會員',
                    data: [],
                    backgroundColor: 'rgba(148,163,184,0.6)'
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    labels: {
                        color: axisTextColor
                    }
                },
                tooltip: {
                    backgroundColor: '#1e293b',
                    titleColor: '#fff',
                    bodyColor: '#e2e8f0',
                    borderColor: '#334155',
                    borderWidth: 1
                }
            },
            scales: {
                x: {
                    ticks: { color: axisTextColor },
                    grid: { color: gridColor }
                },
                y: {
                    ticks: { color: axisTextColor },
                    grid: { color: gridColor },
                    beginAtZero: true
                }
            }
        }
    });
}

//區間資料載入

async function loadRangeData(range) {

    try {

        setLoading(true);

        const response = await fetch(`/Admin/Dashboard/GetChartData?range=${range}`);

        const result = await response.json();

        if (!result.success) {
            alert("資料載入失敗");
            return;
        }

        updateChart(result.data);

    } catch (err) {
        console.error(err);
        alert("系統錯誤");
    }
    finally {
        setLoading(false);
    }
}

// 歷史月份資料

async function loadHistoryData(month) {

    try {

        setLoading(true);

        const response = await fetch(`/Admin/Dashboard/GetHistoryMonthData?month=${month}`);

        const result = await response.json();

        if (!result.success) {
            alert("資料載入失敗");
            return;
        }

        updateChart(result.data);

    } catch (err) {
        console.error(err);
        alert("系統錯誤");
    }
    finally {
        setLoading(false);
    }
}

//更新圖表

function updateChart(data) {
    console.log("chart data =", data);
    console.log("labels =", data.labels);
    console.log("orderData =", data.orderData);
    console.log("revenueData =", data.revenueData);
    console.log("memberData =", data.memberData);

    transactionChart.data.labels = data.labels;
    transactionChart.data.datasets[0].data = data.orderData || [];
    transactionChart.data.datasets[1].data = data.revenueData || [];
    transactionChart.update();

    memberChart.data.labels = data.labels;
    memberChart.data.datasets[0].data = data.memberData || [];
    memberChart.update();
}

//Loading 控制

function setLoading(state) {

    isLoading = state;

    const tCanvas = document.getElementById("transactionChart");
    const mCanvas = document.getElementById("memberChart");

    const opacity = state ? "0.5" : "1";

    if (tCanvas) tCanvas.style.opacity = opacity;
    if (mCanvas) mCanvas.style.opacity = opacity;
}

