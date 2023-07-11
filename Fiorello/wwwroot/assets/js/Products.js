$(function () {
    var skipRow = 1
    $('#loadMore').on('click', function () {
        $.ajax({
            url: "products/loadmore",
            type: "GET",
            data: {
                skipRow: skipRow
            },
            contentType: "application/json",
            success: function (response) {
                $('#products').append(response)
                skipRow++;
            }
        })
    })
})

