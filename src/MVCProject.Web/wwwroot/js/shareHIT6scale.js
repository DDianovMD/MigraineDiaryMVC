async function shareHIT6scale(entityCounterValue, scaleID, doctorID, doctorNames) {
    // Get "Share" button element.
    let shareButton = document.getElementById(`share-entity-btn-${entityCounterValue}`);

    // Make "Share" button invisible.
    shareButton.style.visibility = 'hidden';

    // Clear searchBar text.
    document.getElementById(`searchBar-${entityCounterValue}`).value = '';

    let data = {
        'doctorID': doctorID,
        'scaleID': scaleID,
    }

    await fetch('https://localhost:7021/Scales/ShareHIT6Scale', {
        method: 'POST',
        headers: {
            'Content-Type': "application/json; charset=utf-8",
        },
        body: JSON.stringify(data)
    })
        .then(function (response) {
            if (response.ok) {
                let addedDoctor;

                // Get paragraph where doctors with which headache is shared are displayed.
                let pElement = document.getElementById(`shared-paragraph-${entityCounterValue}`);

                // Create span element
                addedDoctor = document.createElement('span');

                // Set span element's inner text.
                addedDoctor.innerText = doctorNames;

                // Check if headache has been shared.
                if (pElement.lastElementChild.innerText != 'Споделено с: ') {
                    // Add comma separator if headache is already shared with someone.
                    pElement.lastElementChild.innerText += ', '
                }

                // Append span with added doctor's name.
                pElement.append(addedDoctor);

                // Make paragraph visible.
                pElement.style.display = 'block';

                // Alert with success message after DOM is updated.
                setTimeout(() => alert("Успешно споделихте скалата с избрания от Вас лекар."), 100);
            }
            else {
                return alert("Скалата вече е споделена с избрания от Вас лекар.")
            }
        })
        .catch(function (error) {
            return console.error(error.toString());
        });

    // Remove onclick event listener from "Share" button.
    shareButton.removeAttribute('onclick');
}