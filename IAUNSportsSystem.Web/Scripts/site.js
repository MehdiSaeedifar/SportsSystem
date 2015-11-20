$(function () {
    $('#carousel-example-generic').carousel({
        interval: 5000,
        pause: 'hover'
    });

    $('div.bgParallax').each(function () {
        var $obj = $(this);
        $(window).scroll(function () {
            var yPos = -($(window).scrollTop() / $obj.data('speed'));
            var bgpos = '50% ' + yPos + 'px';
            $obj.css('background-position', bgpos);
        });
    });

    $('.navbar-links a').each(function () {
        // and test its normalized href against the url pathname regexp
        if (this.href === window.location.href) {
            $(this).parent('li').addClass('active');
        }
    });


    $.reject({
        reject: {
            msie: 9
        },
        header: 'مرورگر شما قدیمی است.', // Header Text  
        paragraph1: 'مرورگر مورد استفاده شما در این سایت پشتیبانی نمی شود.', // Paragraph 1  
        paragraph2: 'لطفا یکی از مرورگرهای زیر را دریافت و نصب کرده و سپس اقدام به ورود به سایت کنید.',
        closeMessage: 'با مسئولیت خودتان این صفحه را ببندید!', // Message below close window link
        display: ['firefox', 'chrome'],
        closeLink: "بستن",
        browserInfo: {
            chrome: { // Specifies browser name and image name (browser_konqueror.gif)  
                text: 'chrome', // Text Link  
                url: 'http://www.google.com/chrome/browser/desktop/index.html' // URL To link to  
            }
        }

    }); // Customized Browsers  

});