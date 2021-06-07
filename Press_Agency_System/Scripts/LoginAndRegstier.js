var modal = document.getElementById("modal_signin");
var btn = document.getElementById("btn-login");
var modall = document.getElementById("modal_signup");
var btnn = document.getElementById("btn-Reg");



btn.onclick = function () 
{
    if (modall.style.display == "block")
    {
        modall.style.display = "none";
    }
    modal.style.display = "block";

    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
}
    $('#log_form').submit(function () {
        if ($('#UserName').val() != '' && $('#Password').val() != '') {
            var form = $('#log_form');
            var lData = $('#log_form').serialize();
            $.ajax({
                url: '/Account/SmartLogin/',
                type: 'POST',
                data: JSON.stringify({ model: lData }),
                success: function (result) {

                    if (result == 'true') {
                        window.location.href = '/Home/Index';
                        modal.style.display = "none";
                    }
                    else {
                        modal.style.display = "block";
                        document.getElementById('error-field').innerHTML = 'Invalid Username or Password';
                    }
                },
                error: function (errorResult) {
                    modal.style.display = "block";
                    document.getElementById('error-field').innerHTML = 'Invalid Username or Password';
                }
            });
            return false;
        }
    });




        
btnn.onclick = function () 
{
    if (modal.style.display == "block") {
        modal.style.display = "none";
    }
    modall.style.display = "block";
            
    window.onclick = function (event) {
        if (event.target == modall) {
            modall.style.display = "none";
        }
    }
}
    //$('#Reg_form').submit(function () {
    //    if ($('#UserName').val() != '' && $('#Password').val() != '' && $('#ConfirmPassword').val() != '') {
    //        var form = $('#Reg_form');
    //        var lData = $('#Reg_form').serialize();
    //        var token = $('input[name="__RequestVerificationToken"]', form).val();
    //        $.ajax({
    //            url: '/Account/SmartRegister/',
    //            type: 'POST',
    //            data: JSON.stringify({ model: lData }),
    //            success: function (result) {

    //                if (result == 'true') {
    //                    window.location.href = '/Home/Index';
    //                    modall.style.display = "none";
    //                }
    //                else if (result == 'false') {
    //                    modall.style.display = "block";
    //                    document.getElementById('error-field').innerHTML = 'Username already exists';
    //                }
    //                else {
    //                    modall.style.display = "block";
    //                    document.getElementById('error-field').innerHTML = 'please vaild data ';
    //                }
    //            },
    //            error: function (errorResult) {
    //                modall.style.display = "block";
    //                document.getElementById('error-field').innerHTML = 'data not vaild';
    //            }
    //        });
    //        return false;
    //    }
    //});








const inputs = document.querySelectorAll(".input");


function addcl(){
    let parent = this.parentNode.parentNode;
    parent.classList.add("focus");
}

function remcl(){
    let parent = this.parentNode.parentNode;
    if(this.value == ""){
        parent.classList.remove("focus");
    }
}


inputs.forEach(input => {
    input.addEventListener("focus", addcl);
input.addEventListener("blur", remcl);
});


function gotolog(){
    modall.style.display = "none";
    modal.style.display="block";
}

function gotolog(){
    modal.style.display = "none";
    modall.style.display="block";
}
