// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
function submitAction(formId) {
    'use strict'

    let form_Id = document.getElementById(formId);
    let formData = new FormData(form_Id);
    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    var forms = document.querySelectorAll('#form');
   
    // Loop over them and prevent submission
    Array.prototype.slice.call(forms)
        .forEach(function (form) {
            if (!form.checkValidity()) {
                event.preventDefault()
                event.stopPropagation()
                alert("Some fields need your input.");               
            }           
            form.classList.add('was-validated')
        })

    if (form.checkValidity()) {
        var r = confirm("Are you sure you want to submit?");
        if (r == false) {
            return false;
        }
        else if (r == true) {
            $('.spinner').css('display', 'block');
            document.getElementById("submitButton").value = "Submitting..";
            $.ajax({
                type: 'POST',
                url: $(form).attr("action"),
                data: formData,
                processData: false,
                contentType: false,
                success: function (data) {
                    window.location.replace(data.pageUrl);
                },
                error: function (data) {
                    $(".spinner").fadeOut('slow');
                    alert("Something went wrong. The request has not been submited!");
                }
            });
        }
    }
}