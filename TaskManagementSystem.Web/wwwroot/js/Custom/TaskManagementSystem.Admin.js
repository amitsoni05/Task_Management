TaskManagementSystem.Admin = new function () {
  
    this.ToastrSuccess = function (msg) {
        toastr.success(msg);
    }

    this.ToastrError = function (msg) {
        toastr.error(msg);
    }

    this.ToastrRemove = function () {
        toastr.remove();
    }

    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
        TableLengthId: "",
        TableLength: "",
    };

    this.Init = function (options) {
        TaskManagementSystem.Admin.Option = $.extend({}, TaskManagementSystem.Admin.Option, options);

        const opts = TaskManagementSystem.Admin.Option;

        opts.Table = $("#" + opts.TableId).DataTable({
            searching: false,
            serverSide: true,
            filter: true,
            orderMulti: true,
            bLengthChange: false,
            processing: true,
            lengthMenu: [[20, 40, 60, -1], [20, 40, 60, "All"]],
            pageLength: 10,
            ajax: {
                url: "/Admin/GetList",
                type: "POST",
                datatype: "json",
                data: function (dtParms) {
                    dtParms.search.value = $("#" + opts.SearchId).val();
                    return dtParms;
                }
            },
            // Default sorting by second column (index 1)
            order: [[1, "asc"]],
            columns: [
                { data: "fullName", name: "fullName", autoWidth: false, orderable: true, width: "50px" },
                { data: "email", name: "email", autoWidth: false, orderable: true, width: "50px" },
                { data: "createdDate", name: "createdDate", autoWidth: false, orderable: true, width: "100px" },
                { data: "createdName", name: "createdName", autoWidth: true, orderable: true, width: "50px" },
                {
                    data: "active", name: "active", autoWidth: true, orderable: true,
                    render: function (data) {
                        return data
                            ? '<span class="badge bg-success">Active</span>'
                            : '<span class="badge bg-danger">In-Active</span>';
                    }
                },
                {
                    data: "encUserId", className: "text-center", width: "100px", orderable: false,
                    render: function (data, type, row) {
                        let btnEdit = '', btnDelete = '';
                        if (row.active) {
                            btnEdit = `<button title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="TaskManagementSystem.Admin.AddEmployee('${data}');"><i class="fa fa-pencil"></i></button>`;
                            btnDelete = `<button title="De-Activate" class="btn btn-danger btn-sm mt-4 mr-1" onclick="TaskManagementSystem.Admin.DeleteEmployee('${data}', false);"><i class="fa-solid fa-trash"></i></button>`;
                        }
                        return btnEdit + btnDelete;
                    }
                },
            ]
        });
    };


    this.Search = function () {
        TaskManagementSystem.Admin.Option.Table.ajax.reload();
    }




    this.AddEmployee = function (id) {
        $(".preloader").show();
        $.ajax({
            type: 'GET',        
            data: { id: id },         
            url: "/Admin/_AddEmployee",
            success: function (data) {
                $("#common-lg-DialogContent").html(data);

                $("#common-lg-dialog").modal('show');
                $(".preloader").hide();
            }
        });
    }



    this.SaveEmployee = function () {
        debugger;

        if ($("#empdata").valid()) {
            $(".loading-screen").show();
            var formData = $("#empdata").serialize();
          
            $.ajax({
                url: UrlContent("Admin/SaveEmployee"),
                type: "POST",
                data: formData,          
                success: function (data) {
                    debugger;
                    $(".loading-screen").hide();
                    if (data.isSuccess) {
                        debugger;
                        $("#common-lg-dialog").modal('hide');
                        TaskManagementSystem.Admin.ToastrSuccess("Employee Add Successfully");                     
                        window.location = UrlContent("Admin/Index");
                        TaskManagementSystem.Admin.Option.Table.ajax.reload();
                    } else {
                        $("#password").val("");
                        $("#txtCaptcha").val("");
                        TaskManagementSystem.Admin.ToastrError("Please Enter A Valid Data");
                    }
                }, error: function (xhr, status, error) {
                    $(".loading-screen").hide();
                }
            });
        } else {
            TaskManagementSystem.Admin.ToastrError("Please Select all the fileds");
        }
    }




    this.DeleteEmployee = function (id) {
        debugger;
        console.log("Attempting to delete court with ID:", id);  // Debugging

        if (!id) {
            Swal.fire("Error!", "Invalid court ID.", "error");
            return;
        }

     
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#d33",
            cancelButtonColor: "#3085d6",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "/Admin/DeleteEmployee",  // Ensure this matches your actual route
                    type: "POST",
                    data: { id: id },
                    success: function (response) {
                        console.log("Response from server:", response); // Debugging
                        $(".preloader").hide();

                        if (response.isSuccess) {
                        // Check for correct response key
                            Swal.fire("Deleted!", response.message, "success").then(() => {
                                if (TaskManagementSystem?.Admin?.Option?.Table) {
                                    TaskManagementSystem.Admin.Option.Table.ajax.reload();
                                } else {
                                    location.reload();
                                }
                            });
                        } else {
                            Swal.fire("Error!", response.message || "Deletion failed.", "error");
                        }
                    },
                    error: function (xhr, status, error) {
                        $(".preloader").hide();
                        console.error("AJAX Error:", error);
                        Swal.fire("Error!", "Something went wrong.", "error");
                    }
                });
            }
        });
    }




    this.UploadExcel = function () {
      
        $(".preloader").show();
        $.ajax({
            type: 'GET',
            url: "/Admin/_UploadExcel",
            success: function (data) {
                $("#common-lg-DialogContent").html(data);
               
                $("#common-lg-dialog").modal('show');
                $(".preloader").hide();
            }
        });
    }


    this.ExcelData = function () {

        var formElement = document.getElementById("ExcelData");
        var formData = new FormData(formElement);

        $.ajax({
            type: "POST",
            url: UrlContent("Admin/UploadExcel"),
            data: formData,
            contentType: false,
            processData: false,
            success: function (result) {
                debugger;
                console.log(result);
                if (result.isSuccess) {
                    $("#Summary").css("display", "block");
                    $("#total").text(result.total);
                    $("#success").text(result.success);
                    $("#error").text(result.errorcount);
                    if (result.errorcount > 0) {
                        $("#dwbtn").show();
                    } else {
                        $("#common-lg-dialog").modal('hide');
                        Swal.fire(
                            "Uploaded",
                            "Data Insert Successfully",
                            "success",
                        );
                        
                    }
                   
                } else {
                    TaskManagementSystem.Admin.ToastrError("Upload Error");
                }
               
            }, error: function () {
                TaskManagementSystem.Admin.ToastrError("Error saving document.");
            }
        })
    }


    //this.DownloadExcel = function () {
    //    debugger;
    //    $(".preloader").show();
    //    let id = $("#ddlSearch").val(); // Get selected value

    //    $.ajax({
    //        type: "GET",
    //        url: "/Admin/DownloadExcel", // Call the correct API
    //        data: { status: id }, // Pass the correct parameter
    //        success: function () {
    //            var url = "/Admin/DownloadExcel?status=" + id; // Append status to URL
    //            window.location.href = url; // Trigger the download
    //        },
    //        error: function (xhr, status, error) {
    //            CricketCourtBookingSystem.Admin.ToastrError("Error:", error); // Log errors
    //            CricketCourtBookingSystem.Admin.ToastrError("Failed to download Excel file!");
    //        },
    //        complete: function () {
    //            $(".preloader").hide(); // Hide preloader after AJAX call completes
    //        }
    //    });
    //};

}