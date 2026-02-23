document.addEventListener("DOMContentLoaded", function () {

    const btn = document.getElementById("btn-comment");

    if (!btn) return;

    btn.addEventListener("click", async function () {

        const content = document
            .getElementById("comment-content")
            .value
            .trim();

        if (!content) {
            alert("請輸入留言內容");
            return;
        }

        const postId = this.dataset.postid;

        const response = await fetch("/PostComment/Create", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                postId: postId,
                content: content
            })
        });

        if (!response.ok) {
            alert("留言失敗");
            return;
        }

        const html = await response.text();

        document
            .querySelector("#comment-list")
            .insertAdjacentHTML("afterbegin", html);

        document.getElementById("comment-content").value = "";

        const modal = bootstrap.Modal.getInstance(
            document.getElementById("commentModal")
        );

        modal.hide();
    });

});