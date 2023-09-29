document.querySelectorAll('a.page-link').forEach(function (element) {
    element.addEventListener('click', function () {

        var url = element.getAttribute('href');

        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('#partialData').html(data);
            },
            error: function (req, status, error) {
                console.log(status)
            }
        });
    });
});