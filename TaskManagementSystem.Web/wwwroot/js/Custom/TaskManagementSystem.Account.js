TaskManagementSystem.Account = new function () {
  
    this.ToastrSuccess = function (msg) {
        toastr.success(msg);
    }

    this.ToastrError = function (msg) {
        toastr.error(msg);
    }

    this.ToastrRemove = function () {
        toastr.remove();
    }

    this.RefreshCaptcha = function () {

        $.ajax({
            url: UrlContent("Account/RefreshCaptcha"),
            type: 'GET',
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                $(".imgCaptcha").attr("src", data);
            }
        });
    }

    this.Search = function (SearchId) {
        debugger;
        $(".preloader").show();
        $.ajax({
            type: "GET",
            url: UrlContent("Account/SearchId"),
            data: { SearchId: SearchId },
            success: function (data) {
                window.location = UrlContent("Account/Register");
            }
        });
    }

    this.Login = function () {
        if ($("#loginformdata").valid()) {
            $(".loading-screen").show();
            encryptPwdWithoutUsername();
            var formData = $("#loginformdata").serialize();
            $.ajax({
                type: "POST",
                data: formData,
                url: UrlContent("Account/CheckLogin"),
                success: function (data) {
                    $(".loading-screen").hide();

                    if (data.isSuccess) {
                        window.location.href = UrlContent("Dashboard/Index");
                    } else {
                        $("#password").val("");
                        $("#txtCaptcha").val("");
                        TaskManagementSystem.Account.RefreshCaptcha();


                        
                        TaskManagementSystem.Account.ToastrError("ERROR !!");
                    }
                }, error: function (xhr, status, error) {
                    $(".loading-screen").hide();
                }
            });


        }
        //else {
        //    TaskManagementSystem.Account.ToastrError("Please Enter Valid Data !!");
        //}
    }

    this.CngPassword = function (id) {
        $(".preloader").show();
        $.ajax({
            type: 'GET',
            data: { id: id },
            url: "/Account/_ChangePassword",
            success: function (data) {
                $("#common-lg-DialogContent").html(data);

                $("#common-lg-dialog").modal('show');
                $(".preloader").hide();
            }
            
        });
    }

    this.CngProfile = function (id) {
        debugger;
        $(".preloader").show();
        $.ajax({
            type: 'GET',
            data: { id: id },
            url: "/Account/_profile",
            success: function (data) {
                $("#common-lg-DialogContent").html(data);

                $("#common-lg-dialog").modal('show');

                $(".preloader").hide();
            }
          
        });
    }



    this.ChangePassword = function () {
        debugger;

        if ($("#ChangePassword").valid()) {
            $(".loading-screen").show();
            encryptPwdWithoutUsername();
            var formData = $("#ChangePassword").serialize();
            $.ajax({
                type: "POST",
                data: formData,
                url: UrlContent("Account/CheckPassword"),
                success: function (data) {
                    $(".loading-screen").hide();
                    debugger;
                    if (data.isSuccess) {
                        TaskManagementSystem.Account.ToastrSuccess(data.message);        
                          window.location.href = UrlContent("Account/Login");
                                           
                    } else {
                        TaskManagementSystem.Account.ToastrError(data.message);         
                        TaskManagementSystem.Account.CngPassword().reload();
                    }
                }, error: function (xhr, status, error) {
                    $(".loading-screen").hide();
                }
            });


        }
    }

    this.ChangeProfile = function () {
        debugger;

        if ($("#profile").valid()) {
            $(".loading-screen").show();
            var formData = new FormData($("#profile")[0]);
            // Handle the file input field
            var fileUpload = $("#img").get(0);
            var files = fileUpload.files;

            // Append the file if there is one selected
            if (files.length > 0) {
                formData.append("HiteshTaskUserMasterModel.Image", files[0]);
            }

            $('.form-control').each(function (i, el) {
                formData.append($(el).attr("name"), $(el).val());
            });
            $.ajax({
             
                url: "/Account/Profile",
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,              
                success: function (data) {
                    $(".loading-screen").hide();
                    debugger;
                    if (data.isSuccess) {
                        TaskManagementSystem.Account.ToastrSuccess(data.message);
                        if (data.rid == 1) {
                            window.location.href = UrlContent("Admin/Index");
                        } else  {
                            window.location.href = UrlContent("User/Index");
                        }
                     
                    } else {
                        TaskManagementSystem.Account.ToastrError(data.message);
                        TaskManagementSystem.Account.CngPassword().reload();
                    }
                }, error: function (xhr, status, error) {
                    $(".loading-screen").hide();
                }
            });


        }
    }



    this.CheckEmail = function () {
        debugger;

        if ($("#checkemail").valid()) {
            $(".loading-screen").show();
            var formData = $("#checkemail").serialize();
            $.ajax({
                type: "POST",
                data: formData,
                url: "/Account/Checkemail",
                success: function (data) {
                    $(".loading-screen").hide();                 
                    if (data.isSuccess) {
                        debugger;
                        TaskManagementSystem.Account.ToastrSuccess(data.message);
                        window.location.href = UrlContent("Account/Login");

                    } else {
                        TaskManagementSystem.Account.ToastrError(data.message);
                        TaskManagementSystem.Account.CngPassword().reload();
                    }
                }, error: function (xhr, status, error) {
                    $(".loading-screen").hide();
                }
            });


        } else {
            $("#checkemail").addClass("was-validated");
            $(".loading-screen").hide();
        }

    }



    this.Reset = function () {
            debugger;

        if ($("#Reset").valid()) {
                $(".loading-screen").show();
               
            var formData = $("#Reset").serialize();
                $.ajax({
                    type: "POST",
                    data: formData,
                    url: UrlContent("Account/ResetPassword"),
                    success: function (data) {
                        $(".loading-screen").hide();
                        debugger;
                        if (data.isSuccess) {
                            TaskManagementSystem.Account.ToastrSuccess(data.message);
                            window.location.href = UrlContent("Account/Login");

                        } else {
                            TaskManagementSystem.Account.ToastrError(data.message);
                            TaskManagementSystem.Account.CngPassword().reload();
                        }
                    }, error: function (xhr, status, error) {
                        $(".loading-screen").hide();
                    }
                });


            }
        }
   

}