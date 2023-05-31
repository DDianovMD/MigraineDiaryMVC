async function shareHeadache(entityCounterValue, headacheID, doctorID) {
    // Get "Share" button element.
    let shareButton = document.getElementById(`share-headache-btn-${entityCounterValue}`);

    // Make "Share" button invisible.
    shareButton.style.visibility = 'hidden';

    // Clear searchBar text.
    document.getElementById(`searchBar-${entityCounterValue}`).value = '';

    let data = {
        'doctorID': doctorID,
        'headacheID': headacheID,
    }

    await fetch('https://localhost:7021/Headache/ShareHeadache', {
        method: 'POST',
        headers: {
            'Content-Type': "application/json; charset=utf-8",
        },
        body: JSON.stringify(data)
    })
        .catch(function (error) {
            return console.error(error.toString());
        });

    // Remove onclick event listener from "Share" button.
    shareButton.removeAttribute('onclick');
}