async function searchDoctor(counterValue) {
    // Get user input.
    let searchCriteria = document.getElementById(`searchBar-${counterValue}`).value;

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
                if (jsonResult.length > 0) {
                    // Get div element in which result is going to be attached to.
                    let searchResultDiv = document.getElementById(`searchResult-${counterValue}`);

                    // Clear old results while typing.
                    searchResultDiv.innerHTML = '';

                    for (let i = 0; i < jsonResult.length; i++) {
                        // Create paragraph element.
                        let pElement = document.createElement('p');

                        // Set element's attributes.
                        pElement.setAttribute('class', 'searchResult');
                        pElement.setAttribute('onclick', `removeSearchResults(${counterValue});`);
                        pElement.innerText = `Д-р ${jsonResult[i].firstName} ${jsonResult[i].lastName}`;

                        // Append element to DOM tree.
                        searchResultDiv.appendChild(pElement);
                    }
                }
                else {
                    // Get div element in which result is going to be attached to.
                    let searchResultDiv = document.getElementById(`searchResult-${counterValue}`);

                    // Clear old results while typing.
                    searchResultDiv.innerHTML = '';

                    // Create paragraph element.
                    let pElement = document.createElement('p');

                    // Set element's attributes.
                    pElement.setAttribute('class', 'not-found-result');
                    pElement.style.textDecorationColor = 'red';
                    pElement.innerText = `Не са открити резултати.`;

                    // Append element to DOM tree.
                    searchResultDiv.appendChild(pElement);
                }
            })
            .catch(function (error) {
                return console.error(error.toString());
            });
    }
    else {
        removeSearchResults(counterValue);
    }
}


function removeSearchResults(counterValue) {
    // Get div element in which results are nested.
    let searchResultDiv = document.getElementById(`searchResult-${counterValue}`);

    // Clear search results.
    searchResultDiv.innerHTML = '';

    // Clear searchBar text.
    document.getElementById(`searchBar-${counterValue}`).value = '';
}