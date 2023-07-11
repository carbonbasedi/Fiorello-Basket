$(function () {
    var skipRow = 1
    $('#loadMore').on('click', function () {
        $.ajax({
            url: "home/loadmore",
            type: "GET",
            data: {
                skipRow: skipRow
            },
            contentType: "application/json",
            success: function (response) {
                $('#recentWorks').append(response)
                skipRow++;
            }
        })
    })
})