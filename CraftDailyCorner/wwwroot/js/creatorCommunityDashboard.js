document.addEventListener("DOMContentLoaded", function () {

    const dataElement = document.getElementById("chartData");

    if (!dataElement) return;

    const postTrendData = JSON.parse(dataElement.dataset.post || "[]");
    const commentTrendData = JSON.parse(dataElement.dataset.comment || "[]");

    //發文趨勢

    const postLabels = postTrendData.map(x => x.monthLabel || x.MonthLabel);
    const postCounts = postTrendData.map(x => x.postCount || x.PostCount);

    const postCtx = document.getElementById("postTrendChart");

    if (postCtx) {
        new Chart(postCtx, {
            type: "line",
            data: {
                labels: postLabels,
                datasets: [{
                    label: "發文數",
                    data: postCounts,
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

    //留言趨勢

    const commentLabels = commentTrendData.map(x => x.monthLabel || x.MonthLabel);
    const commentCounts = commentTrendData.map(x => x.commentCount || x.CommentCount);

    const commentCtx = document.getElementById("commentTrendChart");

    if (commentCtx) {
        new Chart(commentCtx, {
            type: "line",
            data: {
                labels: commentLabels,
                datasets: [{
                    label: "留言數",
                    data: commentCounts,
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