$(document).ready(function () {
    $('#dataTable').DataTable({
        "aaSorting": [],
        "bDestroy": true
    });
    //function OnSuccess(response) {
    //    $("#result").empty().append(response);
    //    setTimeout(function () {
    //        $(".alert").alert('close');
    //    }, 6000);
    //}
    //function OnFailure(response) {
    //    $("#result").empty().append("<div class=\"alert alert-danger\" role=\"alert\"><strong>Oops!</strong> Some thing goes wrong please try again later.</div>");
    //                setTimeout(function () {
    //                    $(".alert").alert('close');
    //              }, 6000);
    //}
    //var form = $("#sendForm");
    //form.validate();
    //$("#sendForm").submit(function (event) {
    //    event.preventDefault();
    //    if (form.valid()) {
    //        $("#result").empty().append("<div class=\"alert alert-info\" role=\"alert\"><strong>Wait!</strong> We are processing you request now.</div>");
    //        var name = $("#Name").val();
    //        var email = $("#Email").val();
    //        var message = $("#Message").val();
    //        $.ajax({
    //            method: "POST",
    //            url: "/home/send",
    //            dataType: "html",
    //            data: {
    //                name: name,
    //                email: email,
    //                message: message
    //            },
    //            success: function (data) {
    //                $("#result").empty().append(data);
    //                setTimeout(function () {
    //                    $(".alert").alert('close');
    //                }, 6000);
    //            },
    //            error: function (xhr, status, error) {
    //                $("#result").empty().append("<div class=\"alert alert-danger\" role=\"alert\"><strong>Oops!</strong> Some thing goes wrong please try again later.</div>");
    //                setTimeout(function () {
    //                    $(".alert").alert('close');
    //                }, 6000);
    //            }
    //        });
    //    }

    //});
    /**
     * This object controls the nav bar. Implement the add and remove
     * action over the elements of the nav bar that we want to change.
     *
     * @type {{flagAdd: boolean, elements: string[], add: Function, remove: Function}}
     */
    var myNavBar = {

        flagAdd: true,

        elements: [],

        init: function (elements) {
            this.elements = elements;
        },

        add: function () {
            if (this.flagAdd) {
                for (var i = 0; i < this.elements.length; i++) {
                    document.getElementById(this.elements[i]).className += " fixed-theme";
                }
                this.flagAdd = false;
            }
        },

        remove: function () {
            for (var i = 0; i < this.elements.length; i++) {
                document.getElementById(this.elements[i]).className =
                    document.getElementById(this.elements[i]).className.replace(/(?:^|\s)fixed-theme(?!\S)/g, '');
            }
            this.flagAdd = true;
        }

    };

    /**
     * Init the object. Pass the object the array of elements
     * that we want to change when the scroll goes down
     */
    myNavBar.init([
        "header",
        "header-container",
        "brand"
    ]);

    /**
     * Function that manage the direction
     * of the scroll
     */
    function offSetManager() {

        var yOffset = 0;
        var currYOffSet = window.pageYOffset;

        if (yOffset < currYOffSet) {
            myNavBar.add();
        }
        else if (currYOffSet === yOffset) {
            myNavBar.remove();
        }

    }

    /**
     * bind to the document scroll detection
     */
    window.onscroll = function () {
        offSetManager();
    };

    /**
     * We have to do a first detectation of offset because the page
     * could be load with scroll down set.
     */
    offSetManager();
    /*********************/
    $(".scroll").click(function () {
        $('html,body').animate({
            scrollTop: $(".learn").offset().top - 50
        },
            'slow');
    });

    $('body').on('mouseenter mouseleave', '.dropdown', function (e) {
        var _d = $(e.target).closest('.dropdown'); _d.addClass('show');
        setTimeout(function () {
            _d[_d.is(':hover') ? 'addClass' : 'removeClass']('show');
        }, 300);
    });


    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $(".scrollToTop").fadeIn();
        } else {
            $(".scrollToTop").fadeOut();
        }
    });
    $(".scrollToTop").click(function () {
        $("html,body").animate({ scrollTop: 0 }, 800);
        return false;
    });
});
