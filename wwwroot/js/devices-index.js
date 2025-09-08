// wwwroot/js/devices-index.js
document.querySelectorAll('.js-delete').forEach(btn => {
    btn.addEventListener('click', async () => {
        const id = btn.getAttribute('data-id');
        if (confirm('Are you sure you want to delete this device?')) {
            const response = await fetch(`/Devices/Delete/${id}`, { method: 'POST' });
            if (response.ok) {
                location.reload();
            } else {
                alert('Failed to delete.');
            }
        }
    });
});
