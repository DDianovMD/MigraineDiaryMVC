function getNotification() {
    // Fetch messages count.
    fetch('/messageNotifications')
        .then(function (response) {
            return response.json();
        })
        .catch(function (error) {
            return console.error(error.toString());
        })
        .then(function (jsonResult) {
            if (jsonResult.count != 0) {
                let span = document.getElementById('notification-icon');
                span.innerText = jsonResult.count.toString();
                span.style.left = '-101px';
                span.style.top = '-12px';
                span.style.display = 'inline'
            }
        })
        .catch(function (error) {
            return console.error(error.toString());
        });

};

// Update notifications every minute.
setInterval(getNotification, 60000);
