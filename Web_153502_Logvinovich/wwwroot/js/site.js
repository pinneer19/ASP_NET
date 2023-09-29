function setupLinkElements() {
    document.querySelectorAll('a.page-link').forEach(function (element) {
        element.addEventListener('click', function () {
            if (element.hasAttribute('href')) { return; }
            var url = element.getAttribute('ajax');

            $.ajax({
                type: 'GET',
                url: url,
                success: function (data) {
                    $('#partialData').html(data);
                    setupLinkElements();
                },
                error: function (req, status, error) {
                    console.log(status)
                }
            });
        });
    });
}

setupLinkElements();