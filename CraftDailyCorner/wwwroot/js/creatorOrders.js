document.addEventListener('DOMContentLoaded', function () {
    const checkAll = document.getElementById('checkAll');
    const batchPrintBtn = document.getElementById('batchPrintBtn');
    const orderCheckboxes = document.querySelectorAll('.order-checkbox');

    if (!orderCheckboxes.length) return;

    function updateBatchPrintButton() {
        if (!batchPrintBtn) return;

        const checkedCount = document.querySelectorAll('.order-checkbox:checked').length;
        batchPrintBtn.disabled = checkedCount === 0;
    }

    function updateCheckAllState() {
        if (!checkAll) return;

        const checkedCount = document.querySelectorAll('.order-checkbox:checked').length;
        const totalCount = orderCheckboxes.length;

        checkAll.checked = totalCount > 0 && checkedCount === totalCount;
        checkAll.indeterminate = checkedCount > 0 && checkedCount < totalCount;
    }

    if (checkAll) {
        checkAll.addEventListener('change', function () {
            orderCheckboxes.forEach(cb => {
                cb.checked = this.checked;
            });

            checkAll.indeterminate = false;
            updateBatchPrintButton();
        });
    }

    orderCheckboxes.forEach(cb => {
        cb.addEventListener('change', function () {
            updateCheckAllState();
            updateBatchPrintButton();
        });
    });

    updateCheckAllState();
    updateBatchPrintButton();
});