let indexCounter = 1;

function addPracticioner() {

    // Get practicioners div element where content is gonna be dynamically generated.
    let practicionersDiv = document.getElementById('practicioners');

    // Create div container.
    let divContainer = document.createElement('div');
    divContainer.setAttribute('class', 'practicioners-inner-container');

    // Create div's content.
    let ranks = createRanksDropdown();
    let names = createNamesInput();
    let doctoralDegrees = createDoctoralDegreeDropdown();

    // Create "Remove" button
    let button = document.createElement('button');
    button.setAttribute('type', 'button');
    button.setAttribute('class', 'remove-button btn btn-danger')
    button.setAttribute('onclick', `removePracticioner(${indexCounter});`);
    button.style = 'margin-left: 10px';
    button.innerText = 'Премахни';

    // Nest content into page.
    divContainer.appendChild(ranks);
    divContainer.appendChild(names);
    divContainer.appendChild(doctoralDegrees);
    divContainer.appendChild(button);

    practicionersDiv.appendChild(divContainer);

    indexCounter++;
}

function createRanksDropdown() {

    // Create <div> wrapper.
    let rankDivElement = document.createElement('div');
    rankDivElement.setAttribute('class', 'select-rank');

    // Create <select> element with ranks.
    let ranksDropdown = document.createElement('select');
    ranksDropdown.setAttribute('name', `Practicioners[${indexCounter}].Rank`)

    // Create assistant <option>.
    let assistantOptionElemenet = document.createElement('option')
    assistantOptionElemenet.value = 'assistant'
    assistantOptionElemenet.innerText = 'Ас.';

    // Create chief assistant <option>.
    let chiefAssistantOptionElemenet = document.createElement('option')
    chiefAssistantOptionElemenet.value = 'chiefAssistant'
    chiefAssistantOptionElemenet.innerText = 'Гл. ас.';

    // Create doctor <option>.
    let doctorOptionElemenet = document.createElement('option')
    doctorOptionElemenet.value = 'doctor'
    doctorOptionElemenet.innerText = 'Д-р';

    // Create docent <option>.
    let docentOptionElemenet = document.createElement('option')
    docentOptionElemenet.value = 'docent'
    docentOptionElemenet.innerText = 'Доц.';

    // Create professor <option>.
    let professorOptionElemenet = document.createElement('option')
    professorOptionElemenet.value = 'professor'
    professorOptionElemenet.innerText = 'Проф.';

    // Create academician <option>.
    let academicianOptionElemenet = document.createElement('option')
    academicianOptionElemenet.value = 'academician'
    academicianOptionElemenet.innerText = 'Акад.';

    // Nest <option> elements into ranks <select> element.
    ranksDropdown.appendChild(doctorOptionElemenet);
    ranksDropdown.appendChild(assistantOptionElemenet);
    ranksDropdown.appendChild(chiefAssistantOptionElemenet);
    ranksDropdown.appendChild(docentOptionElemenet);
    ranksDropdown.appendChild(professorOptionElemenet);
    ranksDropdown.appendChild(academicianOptionElemenet);

    // Nest <dropdown> into <div> wrapper.
    rankDivElement.appendChild(ranksDropdown)

    return rankDivElement;
}

function createNamesInput() {

    // Create <div> wrapper.
    let namesDiv = document.createElement('div');
    namesDiv.setAttribute('class', 'practicioner-names')

    // Create first name <input> field.
    let firstNameInputElemenet = document.createElement('input');
    firstNameInputElemenet.setAttribute('type', 'text');
    firstNameInputElemenet.setAttribute('name', `Practicioners[${indexCounter}].FirstName`);
    firstNameInputElemenet.setAttribute('placeholder', 'Име');

    // Create last name <input> field.
    let secondNameInputElemenet = document.createElement('input');
    secondNameInputElemenet.setAttribute('type', 'text');
    secondNameInputElemenet.setAttribute('name', `Practicioners[${indexCounter}].LastName`);
    secondNameInputElemenet.setAttribute('placeholder', 'Фамилия');

    // Nest <input> fields into <div> wrapper.
    namesDiv.appendChild(firstNameInputElemenet);
    namesDiv.appendChild(secondNameInputElemenet);

    return namesDiv;
}

function createDoctoralDegreeDropdown() {

    // Create <div> wrapper.
    let doctoralDegreeDiv = document.createElement('div');
    doctoralDegreeDiv.setAttribute('class', 'doctoral-degree-dropdown');

    // Create <select> element.
    let doctoralDegreeDropdown = document.createElement('select');
    doctoralDegreeDropdown.setAttribute('name', `Practicioners[${indexCounter}].ScienceDegree`);

    // Create <option> elemenets.
    let defaultOption = document.createElement('option');
    defaultOption.setAttribute('value', 'default');
    defaultOption.setAttribute('hidden', 'hidden');
    defaultOption.setAttribute('selected', 'selected');
    defaultOption.setAttribute('disabled', 'disabled');
    defaultOption.innerText = "научна степен";

    let noDegree = document.createElement('option');
    noDegree.setAttribute('value', 'none');
    noDegree.innerText = "липсва";

    let medicalDoctor = document.createElement('option');
    medicalDoctor.setAttribute('value', 'medicalDoctor');
    medicalDoctor.innerText = "д.м.";

    let medicalScienceDoctor = document.createElement('option');
    medicalScienceDoctor.setAttribute('value', 'medicalScienceDoctor');
    medicalScienceDoctor.innerText = "д.м.н.";

    // Nest <option> elements into <select> list.
    doctoralDegreeDropdown.appendChild(defaultOption);
    doctoralDegreeDropdown.appendChild(noDegree);
    doctoralDegreeDropdown.appendChild(medicalDoctor);
    doctoralDegreeDropdown.appendChild(medicalScienceDoctor);

    // Nest <select> list into <div> wrapper.
    doctoralDegreeDiv.appendChild(doctoralDegreeDropdown);

    return doctoralDegreeDiv;

}

function removePracticioner(childNumber) {

    // Get node list
    let nodeList = document.querySelectorAll('div.practicioners-inner-container');

    // Remove node
    let node = nodeList[childNumber]
    node.remove();

    // Fix indexes
    resetIndexes();
}

function resetIndexes() {

    // Reset rank indexes.
    let selectRankNodeList = document.querySelectorAll('div.select-rank>select');

    for (var i = 0; i < selectRankNodeList.length; i++) {
        let currentNode = selectRankNodeList[i];

        currentNode.setAttribute('name', `Practicioners[${i}].Rank`);
    }

    // Reset names indexes.
    let namesInputsNodeList = document.querySelectorAll('div.practicioner-names>input');

    let value = 0;

    for (var i = 0; i < namesInputsNodeList.length; i += 2) {
        let firstNameNode = namesInputsNodeList[i];
        let lastNameNode = namesInputsNodeList[i + 1];

        firstNameNode.setAttribute('name', `Practicioners[${value}].FirstName`);
        lastNameNode.setAttribute('name', `Practicioners[${value}].LastName`);
        value++;
    }

    // Reset doctoral science degree indexes.
    let doctoralScienceDegreeNodeList = document.querySelectorAll('div.doctoral-degree-dropdown>select');

    for (var i = 0; i < doctoralScienceDegreeNodeList.length; i++) {
        let currentNode = doctoralScienceDegreeNodeList[i];

        currentNode.setAttribute('name', `Practicioners[${i}].ScienceDegree`);
    }

    // Reset remove button indexes.
    let buttonsNodeList = document.querySelectorAll('.remove-button');

    for (var i = 0; i < buttonsNodeList.length; i++) {
        let currentNode = buttonsNodeList[i];

        currentNode.setAttribute('onclick', `removePracticioner(${i + 1});`);
    }

    indexCounter--;
}