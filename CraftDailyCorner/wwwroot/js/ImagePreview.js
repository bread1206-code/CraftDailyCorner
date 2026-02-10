// 圖片上傳預覽邏輯
const fileInput = document.getElementById('fileInput');
const previewBg = document.getElementById('preview-image-bg');
const uploadZone = document.getElementById('dropZone');

fileInput.addEventListener('change', function (e) {
    const file = e.target.files[0];
    if (file) {
        const reader = new FileReader();

        reader.onload = function (e) {
            previewBg.src = e.target.result;
            previewBg.style.display = 'block'; // 顯示圖片
            uploadZone.classList.add('has-image'); // 標記已有圖片
        }

        reader.readAsDataURL(file);
    } else {
        previewBg.style.display = 'none';
        uploadZone.classList.remove('has-image');
    }
});
