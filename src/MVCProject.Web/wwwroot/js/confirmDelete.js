function confirmDelete(event, scaleNumber) {
    if (confirm(`Сигурни ли сте, че искате да изтриете скала № ${scaleNumber}?`)) {
        return true;
    } else {
        event.preventDefault();
    }
}