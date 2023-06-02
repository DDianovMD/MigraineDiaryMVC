﻿async function searchDoctor(entityCounterValue) {
    // Get user input.
    let searchCriteria = document.getElementById(`searchBar-${entityCounterValue}`).value;

    if (searchCriteria != '') {
        // Fetch doctors.
        await fetch(`https://localhost:7021/Headache/SearchDoctor?name=${searchCriteria}`)
            .then(function (response) {
                return response.json();
            })
            .catch(function (error) {
                return console.error(error.toString());
            })
            .then(function (jsonResult) {
                // Get div element in which result is going to be attached to.
                let searchResultsDiv = document.getElementById(`searchResults-${entityCounterValue}`);

                if (jsonResult.length > 0) {
                    // Clear old results while typing.
                    searchResultsDiv.innerHTML = '';

                    for (let i = 0; i < jsonResult.length; i++) {
                        // Create paragraph element.
                        let pElement = document.createElement('p');
                        
                        // Set element's attributes.
                        pElement.setAttribute('id', `searchResult-${i}`);
                        pElement.setAttribute('class', 'searchResult');
                        pElement.setAttribute('doctorID', `${jsonResult[i].id}`);
                        pElement.setAttribute('onclick', `removeSearchResults(${entityCounterValue}, ${i});`);
                        pElement.innerText = `Д-р ${jsonResult[i].firstName} ${jsonResult[i].lastName}`;

                        // Append element to DOM tree.
                        searchResultsDiv.appendChild(pElement);
                    }
                }
                else {
                    // Clear old results while typing.
                    searchResultsDiv.innerHTML = '';

                    // Create paragraph element.
                    let pElement = document.createElement('p');

                    // Set element's attributes.
                    pElement.setAttribute('class', 'not-found-result');
                    pElement.style.textDecorationColor = 'red';
                    pElement.style.marginLeft = '83px';
                    pElement.innerText = `Не са открити резултати.`;

                    // Append element to DOM tree.
                    searchResultsDiv.appendChild(pElement);
                }
            })
            .catch(function (error) {
                return console.error(error.toString());
            });
    }
    else {
        removeSearchResults(entityCounterValue);
    }
}


function removeSearchResults(entityCounterValue, resultCounter) {
    let resultValue;

    // Get div element in which results are nested.
    let searchResultsDiv = document.getElementById(`searchResults-${entityCounterValue}`);

    // Get "Share" button element.
    let shareButton = document.getElementById(`share-entity-btn-${entityCounterValue}`);

    // Check if resultCounter has value.
    // Function is called with not null parameter only if clicked over presented on screen search result.
    if (resultCounter != null) {
        // Get clicked result innerText
        resultValue = document.getElementById(`searchResult-${resultCounter}`).innerText;

        // Set searchBar innerText as result's innerText.
        document.getElementById(`searchBar-${entityCounterValue}`).value = resultValue;

        // Get clicked result doctorid attribute's value.
        let doctorID = document.getElementById(`searchResult-${resultCounter}`).getAttribute('doctorid');

        // Get entity's ID.
        let entityID = document.getElementById(`entityID-${entityCounterValue}`).value;

        // Get entity's type.
        let entityType = document.getElementById(`entityID-${entityCounterValue}`).getAttribute('entitytype');

        // Add onclick event listener to "Share" button for sharing with chosen doctor.
        shareButton.setAttribute('onclick', `share${entityType}(${entityCounterValue}, '${entityID}', '${doctorID}', '${resultValue}')`);

        // Make "Share" button visible.
        shareButton.style.visibility = 'visible';
    } else {
        // Clear searchBar text.
        document.getElementById(`searchBar-${entityCounterValue}`).value = '';

        // Make "Share" button invisible.
        shareButton.style.visibility = 'hidden';

        // Remove onclick event listener from "Share" button.
        shareButton.removeAttribute('onclick');
    }

    // Clear search results.
    searchResultsDiv.innerHTML = '';
}