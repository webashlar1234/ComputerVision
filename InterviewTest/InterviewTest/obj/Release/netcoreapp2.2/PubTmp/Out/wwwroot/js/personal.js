// ****** Scroll to Top *****

$(document).ready(function () {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 50) {
            $('.page-scroll').fadeIn();
        } else {
            $('.page-scroll').fadeOut();
        }
    });
    // scroll body to 0px on click
    $('.page-scroll').click(function () {
        $('.page-scroll').tooltip('hide');
        $('body,html').animate({
            scrollTop: 0
        }, 800);
        return false;
    });

    
});


$(function () {
    $(".expand").on("click", function () {
        $expand = $(this).find(">:first-child");

        if ($expand.text() == "+") {
            $expand.text("-");
        } else {
            $expand.text("+");
        }
    });
});


$(function () {
    $('#fullscreen').on('click', function (event) {
        event.preventDefault();
        window.parent.location = $('#fullscreen').attr('href');
    });
    $('#fullscreen').tooltip();
    /* END DEMO OF JS */

    $('.navbar-toggler').on('click', function (event) {
        event.preventDefault();
        $(this).closest('.navbar-minimal').toggleClass('open');
    })
});