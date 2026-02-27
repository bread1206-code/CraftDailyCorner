// ===============================
// Admin Dashboard JS
// ===============================

let dashboardChart = null;
let isLoading = false;

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

    const ctx = document.getElementById("dashboardChart");

    dashboardChart = new Chart(ctx, {
        data: {
            labels: [],
            datasets: [
                {
                    type: 'line',
                    label: '訂單數',
                    data: [],
                    borderWidth: 2,
                    tension: 0.3,
                    yAxisID: 'y'
                },
                {
                    type: 'line',
                    label: '營收',
                    data: [],
                    borderWidth: 2,
                    tension: 0.3,
                    yAxisID: 'y1'
                },
                {
                    type: 'bar',
                    label: '新增會員',
                    data: [],
                    yAxisID: 'y'
                }
            ]
        },
        options: {
            responsive: true,
            interaction: {
                mode: 'index',
                intersect: false
            },
            scales: {
                y: {
                    type: 'linear',
                    position: 'left'
                },
                y1: {
                    type: 'linear',
                    position: 'right',
                    grid: {
                        drawOnChartArea: false
                    }
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

    dashboardChart.data.labels = data.labels;

    dashboardChart.data.datasets[0].data = data.orderData || [];
    dashboardChart.data.datasets[1].data = data.revenueData || [];
    dashboardChart.data.datasets[2].data = data.memberData || [];

    dashboardChart.update();
}

//Loading 控制

function setLoading(state) {

    isLoading = state;

    const canvas = document.getElementById("dashboardChart");

    if (state) {
        canvas.style.opacity = "0.5";
    } else {
        canvas.style.opacity = "1";
    }
}