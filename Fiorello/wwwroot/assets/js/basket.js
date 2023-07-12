jQuery(function ($) {
    $(document).on('click', '#addToCart', function () {

        var id = $(this).data('id');
        $.ajax({
            method: "GET",
            url: "/basket/add",
            data: {
                id: id
            },
            success: function (res) {
                console.log(res)
                alert(res)
            },
            error: function (err) {
                alert(err.responseText)
            }
        })
    })

    $(document).on('click', '#increaseCount', function (e) {
        e.preventDefault();

        var increaseCount = $(this);

        var id = $(this).data('id');

        $.ajax({
            method: "GET",
            url: "/basket/increasecount",
            data: {
                id: id
            },
            success: function (res) {
                var countElement = $(increaseCount).parent().siblings().eq(3);
                var stockElement = $(increaseCount).parent().siblings().eq(4);
                console.log(stockElement);
                var count = parseInt(countElement.html());
                var stock = parseInt(stockElement.html());

                if (count < stock) {
                    count++;
                    countElement.html(count);
                }
            },
            error: function (err) {
                alert(err.responseText)
            }
        })
    })

    $(document).on('click', '#decreaseCount', function (e) {
        e.preventDefault();

        var decreaseCount = $(this);

        var id = $(this).data('id');

        $.ajax({
            method: "GET",
            url: "/basket/decreasecount",
            data: {
                id: id
            },
            success: function (res) {
                var countElement = $(decreaseCount).parent().siblings().eq(3);
                var count = parseInt(countElement.html());

                if (count > 0) {
                    count--;
                    countElement.html(count);
                }
            },
            error: function (err) {
                alert(err.responseText)
            }
        })
    })
})