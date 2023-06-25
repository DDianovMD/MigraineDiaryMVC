// Global scope variables

let auraContainer = document.querySelector('.aura-description');
let hasAuraYesElement = document.querySelector('.aura-yes');
let hasAuraNoElement = document.querySelector('.aura-no');

let medicationsContainer = document.getElementById('used-medications-container');
let medicationUsageYesElement = document.getElementById('medication-usage-yes');
let medicationUsageNoElement = document.getElementById('medication-usage-no');

let addButtonContainer = document.getElementById('add-medication-button');
let indexCounter = document.getElementById('used-medications-container').children.length;

function addAuraDescription() {

    if (hasAuraYesElement.value == 'true') {

        // Select DOM elements
        let divElement = document.querySelector('.aura-description');

        // Create new DOM elemenets
        let labelElement = document.createElement('label');
        let brElement = document.createElement('br');
        let textAreaElement = document.createElement('textarea');

        // Set labes's attributes
        labelElement.setAttribute('for', 'AuraDescriptionNotes');
        labelElement.setAttribute('id', 'aura-description-label');
        labelElement.innerText = "Oпишете аурата си:";

        // Set textarea's attributes
        textAreaElement.setAttribute('name', 'AuraDescriptionNotes');
        textAreaElement.setAttribute('id', 'AuraDescriptionNotes');
        textAreaElement.setAttribute('rows', '5');
        textAreaElement.setAttribute('cols', '80');

        // Toggle onclick attribute
        hasAuraYesElement.removeAttribute('onclick');
        hasAuraNoElement.setAttribute('onclick', 'removeAuraDescription();');

        labelElement.appendChild(brElement);
        labelElement.appendChild(textAreaElement);
        divElement.appendChild(labelElement);
    }
}
function removeAuraDescription() {

    if (auraContainer.hasChildNodes) {

        // Remove DOM element
        auraContainer.innerHTML = '';

        // Toggle onclick attribute
        hasAuraNoElement.removeAttribute('onclick');
        hasAuraYesElement.setAttribute('onclick', 'addAuraDescription();');
    }
}

function createAddButton() {

    if (medicationUsageYesElement.value == 'yes') {

        // Create button and set it's attributes
        let buttonElement = document.createElement('button');
        buttonElement.setAttribute('type', 'button');
        buttonElement.setAttribute('id', 'addMedication');
        buttonElement.setAttribute('class', 'btn btn-success');
        buttonElement.setAttribute('onclick', 'аddMedication();');
        buttonElement.innerText = 'Добави медикамент';

        // Toggle onclick attributes
        medicationUsageYesElement.removeAttribute('onclick');
        medicationUsageNoElement.setAttribute('onclick', 'removeAddButton();');
        addButtonContainer.appendChild(buttonElement);
    }
};

function removeAddButton() {

    // Select button for adding new medications
    let buttonElement = document.getElementById('addMedication');

    // Remove button if it's available
    if (buttonElement != undefined) {
        buttonElement.remove();

        // Toggle onclick attributes
        medicationUsageNoElement.removeAttribute('onclick');
        medicationUsageYesElement.setAttribute('onclick', 'createAddButton();');
    }

    // Remove all input fields along with the button
    removeMedicationInputs(medicationsContainer);
};

function removeMedicationInputs(parentElement) {

    while (parentElement.firstChild) {
        parentElement.removeChild(parentElement.firstChild);
    }
}

function addNameInput() {

    let divElement = document.createElement('div');
    let medicationNameElement = document.createElement('input');
    //let labelElement = document.createElement('label');
    //let brElement = document.createElement('br');

    // Set div element's id attribute
    divElement.setAttribute('id', `medication-${indexCounter}`);

    // Set label element
    //labelElement.innerText = 'Въведете използвания медикамент';
    //labelElement.appendChild(brElement);

    // Set attributes for "Name" input
    medicationNameElement.style.marginRight = "20px";
    medicationNameElement.setAttribute('type', 'text');
    medicationNameElement.setAttribute('name', `MedicationsTaken[${indexCounter}].Name`);
    medicationNameElement.setAttribute('class', 'text-center');
    medicationNameElement.setAttribute('placeholder', 'Име на медикамента');

    // Add "Name" input in the HTML
    //labelElement.appendChild(medicationNameElement);
    divElement.appendChild(medicationNameElement);
    medicationsContainer.appendChild(divElement);
};

function addMedicationDosage() {

    let divElement = document.getElementById(`medication-${indexCounter}`);
    let medicationSinglePillDosageElement = document.createElement('input');
    //let labelElement = document.createElement('label');
    //let brElement = document.createElement('br');

    // Set label element
    //labelElement.innerText = 'Доза';
    //labelElement.appendChild(brElement);

    // Set attributes for Dosage input
    medicationSinglePillDosageElement.style.marginRight = "20px";
    medicationSinglePillDosageElement.setAttribute('type', 'number');
    medicationSinglePillDosageElement.setAttribute('name', `MedicationsTaken[${indexCounter}].SinglePillDosage`);
    medicationSinglePillDosageElement.setAttribute('placeholder', 'Доза');
    medicationSinglePillDosageElement.setAttribute('class', 'text-center');
    medicationSinglePillDosageElement.setAttribute('min', '0');
    medicationSinglePillDosageElement.setAttribute('max', '2000');
    medicationSinglePillDosageElement.setAttribute('step', '0.25');

    // Add "Dosage" input in the HTML
    //labelElement.appendChild(medicationSinglePillDosageElement);
    divElement.appendChild(medicationSinglePillDosageElement);
    medicationsContainer.appendChild(divElement);
};

function addUnitsDropdown() {

    let divElement = document.getElementById(`medication-${indexCounter}`);
    let selectElement = document.createElement('select');
    let firstOptionElement = document.createElement('option');
    let secondOptionElement = document.createElement('option');
    let thirdOptionElement = document.createElement('option');
    let fourthOptionElement = document.createElement('option');

    // Set first option element
    firstOptionElement.setAttribute('value', 'mcg');
    firstOptionElement.innerText = 'mcg';

    // Set second option element
    secondOptionElement.setAttribute('value', 'mg');
    secondOptionElement.setAttribute('selected', 'selected');
    secondOptionElement.innerText = 'mg';

    // Set third option element
    thirdOptionElement.setAttribute('value', 'g');
    thirdOptionElement.innerText = 'g';

    // Set third option element
    fourthOptionElement.setAttribute('value', 'ml');
    fourthOptionElement.innerText = 'ml';

    // Set select element
    selectElement.style.marginRight = "20px";
    selectElement.setAttribute('name', `MedicationsTaken[${indexCounter}].Units`);
    selectElement.setAttribute('class', 'mb-3');
    selectElement.appendChild(firstOptionElement);
    selectElement.appendChild(secondOptionElement);
    selectElement.appendChild(thirdOptionElement);
    selectElement.appendChild(fourthOptionElement);

    // Add select element in HTML
    divElement.appendChild(selectElement);  
};

function addNumberOfPillsInput() {

    let divElement = document.getElementById(`medication-${indexCounter}`);
    let numberOfPillsTakenElement = document.createElement('input');

    // Set attributes for Number of taken pills input
    numberOfPillsTakenElement.style.width = "200px";
    numberOfPillsTakenElement.setAttribute('type', 'number');
    numberOfPillsTakenElement.setAttribute('name', `MedicationsTaken[${indexCounter}].NumberOfTakenPills`);
    numberOfPillsTakenElement.setAttribute('placeholder', 'Брой приети таблетки');
    numberOfPillsTakenElement.setAttribute('class', 'text-center');
    numberOfPillsTakenElement.setAttribute('min', '0');
    numberOfPillsTakenElement.setAttribute('max', '10');
    numberOfPillsTakenElement.setAttribute('step', '0.25');

    // Add input element in HTML
    divElement.appendChild(numberOfPillsTakenElement);
}

function createRemoveMedicationButton() {
    // Get div element in which button is going to be nested.
    let divElement = document.getElementById(`medication-${indexCounter}`);

    // Create button element.
    let buttonElement = document.createElement('button');

    // Set button attributes.
    buttonElement.setAttribute('type', 'button');
    buttonElement.setAttribute('id', `remove-medication-${indexCounter}`);
    buttonElement.setAttribute('class', 'btn btn-danger');
    buttonElement.setAttribute('onclick', `removeMedication('medication-${indexCounter}')`);
    buttonElement.innerText = 'Премахни';

    // Set button's style.
    buttonElement.style.marginLeft = '20px';

    // Append button to divElement.
    divElement.appendChild(buttonElement);
}

function аddMedication() {

    addNameInput();
    addMedicationDosage();
    addUnitsDropdown();
    addNumberOfPillsInput();
    createRemoveMedicationButton();

    // Increment indexCounter
    indexCounter++;
};

function removeMedication(divId) {

    // Remove medication.
    document.getElementById(divId).remove();

    fixIndexes();
}

function fixIndexes() {

    let usedMedicationsContainerChilds = document.getElementById('used-medications-container').children;
    indexCounter = 0;

    for (let child of usedMedicationsContainerChilds) {
        child.id = `medication-${indexCounter}`;
        child.children[0].name = `MedicationsTaken[${indexCounter}].Name`;
        child.children[1].name = `MedicationsTaken[${indexCounter}].SinglePillDosage`;
        child.children[2].name = `MedicationsTaken[${indexCounter}].Units`;
        child.children[3].name = `MedicationsTaken[${indexCounter}].NumberOfTakenPills`;
        child.children[4].id = `remove-medication-${indexCounter}`;
        child.children[4].setAttribute('onclick', `removeMedication('medication-${indexCounter++}')`);
    }
}