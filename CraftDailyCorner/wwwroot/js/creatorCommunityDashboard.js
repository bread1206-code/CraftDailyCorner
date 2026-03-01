document.addEventListener("DOMContentLoaded", function () {

    const dataElement = document.getElementById("chartData");
    if (!dataElement) return;

    const postTrendData = JSON.parse(dataElement.dataset.post || "[]");
    const portfolioTrendData = JSON.parse(dataElement.dataset.portfolio || "[]");
    const commentTrendData = JSON.parse(dataElement.dataset.comment || "[]");
    const reactionTrendData = JSON.parse(dataElement.dataset.reaction || "[]");

    // ===== 發文趨勢 =====
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
            options: { responsive: true, maintainAspectRatio: false }
        });
    }

    // ===== 作品集趨勢 =====
    const portfolioLabels = portfolioTrendData.map(x => x.monthLabel || x.MonthLabel);
    const portfolioCounts = portfolioTrendData.map(x => x.portfolioCount || x.PortfolioCount);

    const portfolioCtx = document.getElementById("portfolioTrendChart");
    if (portfolioCtx) {
        new Chart(portfolioCtx, {
            type: "line",
            data: {
                labels: portfolioLabels,
                datasets: [{
                    label: "作品集數",
                    data: portfolioCounts,
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                }]
            },
            options: { responsive: true, maintainAspectRatio: false }
        });
    }

    // ===== 留言趨勢 =====
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
            options: { responsive: true, maintainAspectRatio: false }
        });
    }

    // ===== Reaction 趨勢 =====
    const reactionLabels = reactionTrendData.map(x => x.monthLabel || x.MonthLabel);
    const reactionCounts = reactionTrendData.map(x => x.reactionCount || x.ReactionCount);

    const reactionCtx = document.getElementById("reactionTrendChart");
    if (reactionCtx) {
        new Chart(reactionCtx, {
            type: "line",
            data: {
                labels: reactionLabels,
                datasets: [{
                    label: "Reaction 數",
                    data: reactionCounts,
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                }]
            },
            options: { responsive: true, maintainAspectRatio: false }
        });
    }

});