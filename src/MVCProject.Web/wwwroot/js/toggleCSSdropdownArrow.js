function changeDirection(dropdownButtonCounter) {
    // Get button element.
    let dropdownButton = document.getElementById(`patient-dropdown-${dropdownButtonCounter}`);

    // Add or remove class based on aria-expanded value.
    if (dropdownButton.ariaExpanded == 'true') {
        dropdownButton.classList.add('dropup-toggle');
    }
    else {
        dropdownButton.classList.remove('dropup-toggle');
    }
}