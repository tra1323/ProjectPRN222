function updateTotal() {
    let total = 0;

    document.querySelectorAll('tr').forEach(row => {
        const checkbox = row.querySelector('input[type="checkbox"]');
        const quantityInput = row.querySelector('input[type="number"]');
        const totalCell = row.querySelector('td[id^="total-"]');

        if (checkbox && checkbox.checked) {
            const price = parseFloat(row.querySelector('td:nth-child(3)').innerText.replace(/[^\d.]/g, ''));
            const quantity = parseInt(quantityInput.value);
            const productTotal = price * quantity;

            total += productTotal;

            totalCell.innerText = productTotal.toLocaleString('vi-VN') + ' đ';
        }
    });

    document.getElementById('grandTotal').innerText = total.toLocaleString('vi-VN') + ' đ';
}

window.onload = updateTotal;
