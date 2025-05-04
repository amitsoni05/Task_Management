TaskManagementSystem.Project = new function () {

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
      
        TaskManagementSystem.Project.Option = $.extend({}, TaskManagementSystem.Project.Option, options);
        TaskManagementSystem.Project.Option.Table = $("#" + TaskManagementSystem.Project.Option.TableId).DataTable({
            searching: false,
            serverSide: true,
            filter: true,
            orderMulti: true,
            bLengthChange: false,
            processing: true,
            lengthMenu: [[20, 40, 60, 50], [20, 40, 60, "All"]],
            pageLength: 10,
            ajax: {
                url: "/Project/GetList",
                type: "Post",
                datatype: "json",
                data: function (dtParms) {
                    dtParms.search.value = $("#txtSearch").val();

                    return dtParms;
                }
            },
            order: [[1, "ASC"]],
            "columns": [
                { "data": "title", "name": "title", "autoWidth": true, orderable: true },
                { "data": "description", "name": "description", "autoWidth": true, orderable: true },            
                { "data": "createdDate", "name": "createdDate", "autoWidth": false, orderable: true, width: "100px", },
                { "data": "createdName", "name": "createdName", "autoWidth": true, orderable: true },
                {
                    "data": "encProjectId", "className": "text-center", width: "100px", orderable: false,
                    "render": function (data, type, row) {

                        let btnDelete = '';
                        let btnEdit = '';
                        let btnUser = '';
                        let btnAddDoc = '';
                        let btnDowDoc = '';
                        btnEdit = '<button title="Edit"  data-toggle="tooltip" data-placement="bottom" data-original-title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="TaskManagementSystem.Project.AddProject(\'' + data + '\');"><i class="fa fa-pencil"></i></button>';
                        btnDelete = '<a  title="De-Activate" data-toggle="tooltip" data-placement="bottom" data-original-title="In-Active" class="btn btn-danger btn-sm mt-4 mr-1" onclick="TaskManagementSystem.Project.DeleteProject(\'' + data + '\',\'' + false + '\');"><i class="fa-solid fa-trash"></i></a>';
                        btnUser = '<button title="User"  data-toggle="tooltip" data-placement="bottom" data-original-title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="TaskManagementSystem.Project.ShowUser(\'' + data + '\');"><i class="fa-solid fa-user-plus"></i></button>';
                        btnAddDoc = '<button title="AddDoc"  data-toggle="tooltip" data-placement="bottom" data-original-title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="TaskManagementSystem.Project.AddDocument(\'' + data + '\');"><i class="fa-solid fa-folder-plus"></i></button>';
                        btnDowDoc = '<button title="DowDoc"  data-toggle="tooltip" data-placement="bottom" data-original-title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="TaskManagementSystem.Project.DowDocument(\'' + data + '\');"><i class="fa-solid fa-file-arrow-down"></i></button>';

                        //btnUpdImg = '<button title="Update Image"  data-toggle="tooltip" data-placement="bottom" data-original-title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="CricketCourtBookingSystem.Admin.ShowImg(\'' + data + '\');"><i class="fa-solid fa-upload"></i></button>';

                        //btnShowImg = '<button title="Show Uploaded Photos"  data-toggle="tooltip" data-placement="bottom" data-original-title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="CricketCourtBookingSystem.Admin.ShowUploadImg(\'' + data + '\');"><i class="fa-solid fa-images"></i></button>';
                        return btnEdit + btnDelete + btnUser + btnAddDoc + btnDowDoc;
                    }
                },
            ],
        });
    }


    this.Search = function () {
        TaskManagementSystem.Project.Option.Table.ajax.reload();
    }


    this.ShowUser = function (id) {
        debugger;
        $(".preloader").show();
        $.ajax({
            type: 'GET',
            data: { id: id },
            url: "/Project/_ShowUser",
            success: function (data) {
                $("#common-lg-DialogContent").html(data);

                $("#common-lg-dialog").modal('show');
                $(".preloader").hide();
            }
        });
    }

    this.AddDocument = function (id) {
      
        $(".preloader").show();
        $.ajax({
            type: 'GET',
            data: { id: id },
            url: "/Project/_AddDocument",
            success: function (data) {
                $("#common-lg-DialogContent").html(data);

                $("#common-lg-dialog").modal('show');
                $(".preloader").hide();
            }
        });
    }

    this.DowDocument = function (id) {

        $(".preloader").show();
        $.ajax({
            type: 'GET',
            data: { id: id },
            url: "/Project/_DowDocument",
            success: function (data) {
                $("#common-lg-DialogContent").html(data);

                $("#common-lg-dialog").modal('show');
                $(".preloader").hide();
            }
        });
    }






    this.AddProject = function (id) {

        $(".preloader").show();
        $.ajax({
            type: 'GET',
            data: { id: id },
            url: "/Project/_AddProject",
            success: function (data) {
                $("#common-lg-DialogContent").html(data);

                $("#common-lg-dialog").modal('show');
                $(".preloader").hide();
            }
        });
    }



    this.SaveProject= function () {
       

        if ($("#projectdata").valid()) {
            $(".loading-screen").show();
            var formData = $("#projectdata").serialize();

            $.ajax({
                url: UrlContent("Project/SaveProject"),
                type: "POST",
                data: formData,
                success: function (data) {
                    debugger;
                    $(".loading-screen").hide();
                    if (data.isSuccess) {
                        debugger;
                        $("#common-lg-dialog").modal('hide');
                        TaskManagementSystem.Project.ToastrSuccess("project Add Successfully");
                        window.location = UrlContent("Project/Index");
                        TaskManagementSystem.Project.Option.Table.ajax.reload();
                    } else {
                        $("#password").val("");
                        $("#txtCaptcha").val("");
                        TaskManagementSystem.Project.ToastrError("Please Enter A Valid Data");
                        document.getElementById("errorMessage").hidden = false;
                        document.getElementById("errorMessageText").innerText = data.message;

                    }
                }, error: function (xhr, status, error) {
                    $(".loading-screen").hide();
                }
            });
        } else {
            //TaskManagementSystem.Project.ToastrError("Please Select all the fileds");
        }
    }


    this.UpdateProject = function () {
        debugger;

        if ($("#projectdata").valid()) {
            $(".loading-screen").show();
            var formData = $("#projectdata").serialize();

            $.ajax({
                url: UrlContent("Project/SaveProject"),
                type: "POST",
                data: formData,
                success: function (data) {
                    debugger;
                    $(".loading-screen").hide();
                    if (data.isSuccess) {
                        debugger;
                        $("#common-lg-dialog").modal('hide');
                        TaskManagementSystem.Project.ToastrSuccess("Employee Update Successfully");
                        TaskManagementSystem.Project.Option.Table.ajax.reload();
                        window.location = UrlContent("Project/Index");
                    } else {
                        $("#password").val("");
                        $("#txtCaptcha").val("");
                        TaskManagementSystem.Project.ToastrError("Please Enter A Valid Data");
                        document.getElementById("errorMessage").hidden = false;
                        document.getElementById("errorMessageText").innerText = data.message;

                    }
                }, error: function (xhr, status, error) {
                    $(".loading-screen").hide();
                }
            });
        } else {
            TaskManagementSystem.Project.ToastrError("Please Select all the fileds");
        }
    }



    this.DeleteProject = function (id) {
        debugger;
        console.log("Attempting to delete court with ID:", id);  // Debugging

        //if (!id) {
        //    Swal.fire("Error!", "Invalid court ID.", "error");
        //    return;
        //}

        ;

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
                    url: "/Project/DeleteProject",  // Ensure this matches your actual route
                    type: "POST",
                    data: { id: id },
                    success: function (response) {
                        console.log("Response from server:", response); // Debugging
                        $(".preloader").hide();

                        if (response.isSuccess) {
                            // Check for correct response key
                            Swal.fire("Deleted!", response.message, "success").then(() => {
                                if (TaskManagementSystem?.Project?.Option?.Table) {
                                    TaskManagementSystem.Project.Option.Table.ajax.reload();
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

    let fileIdCounter = 1;
    let selectedFile = null;

    // Track file on selection
    $(document).on("change", "#files", function (event) {
        if (event.target.files.length > 1) {
            TaskManagementSystem.Project.ToastrError("Please select only one file.");
            $("#files").val("");
            return;
        }

        selectedFile = event.target.files[0];
    });

    let uploadedFiles = [];

    this.Upload = function () {
        if ($("#Document").valid()) {
            if (!selectedFile) {
                TaskManagementSystem.Project.ToastrError("Please select a file");
                return;
            }

            const formData = new FormData();
            formData.append("file", selectedFile);

            $.ajax({
                url: '/Project/UploadFile',
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false,
                success: function (res) {
                    if (res.success) {
                        $("#fileTable tbody").append(`
                        <tr id="row-${fileIdCounter}">
                            <td>${fileIdCounter}</td>
                            <td>${res.fileName}</td>
                            <td><button class="btn btn-sm btn-danger delete-btn" data-id="${fileIdCounter}" data-filename="${res.fileName}">Delete</button></td>
                        </tr>
                    `);

                        uploadedFiles.push(selectedFile);   // ✅ Add uploaded file reference
                        fileIdCounter++;
                        TaskManagementSystem.Project.ToastrSuccess("File uploaded and added to list.");
                    } else {
                        TaskManagementSystem.Project.ToastrError(res.message);
                    }

                    selectedFile = null;
                    $("#files").val("");
                },
                error: function () {
                    TaskManagementSystem.Project.ToastrError("Error uploading file.");
                }
            });
        } else {
            TaskManagementSystem.Project.ToastrError("Please select a file");
        }
    };

    $(document).on("click", ".delete-btn", function () {
        const id = $(this).data("id");
        const filename = $(this).data("filename");

        // Remove from table
        $(`#row-${id}`).remove();

        // ✅ Remove file from array by comparing the name of the file
        uploadedFiles = uploadedFiles.filter(f => f.name !== filename);

        TaskManagementSystem.Project.ToastrSuccess("File removed.");
    });

    this.SaveDocument = function () {
        if (uploadedFiles.length === 0) {
            TaskManagementSystem.Project.ToastrError("No uploaded files to save.");
            return;
        }

        let formData = new FormData();
        for (let i = 0; i < uploadedFiles.length; i++) {
            formData.append(`HiteshTaskDocumentSaveModel.Files`, uploadedFiles[i]);  // Append each file with a unique key
        }

        formData.append("HiteshTaskDocumentSaveModel.ProjectId", $("#Pid").val());

        $.ajax({
            url: UrlContent("Project/DocumentSave"),
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                TaskManagementSystem.Project.ToastrSuccess(response.message);
                $("#common-lg-dialog").modal('hide');
                TaskManagementSystem.Project.Option.Table.ajax.reload();
            },
            error: function () {
                TaskManagementSystem.Project.ToastrError("Error saving document.");
            }
        });
    }


    this.DownloadData = function (id) {
    

        $.ajax({
            type: "POST",
            url: UrlContent("Project/DownloadDocData"),
            data: { id: id },
            success: function (result) {
            
                window.location = UrlContent("Project/Download?documentName=" + result)
               
                $("#common-lg-dialog").modal('hide');
            },
        })
    }

    
    this.DeleteDocData = function (id) {
     
        console.log("Attempting to delete court with ID:", id);  // Debugging

        //if (!id) {
        //    Swal.fire("Error!", "Invalid court ID.", "error");
        //    return;
        //}

        ;

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
                    url: "/Project/DeleteDocData",  // Ensure this matches your actual route
                    type: "POST",
                    data: { id: id },
                    success: function (response) {
                        console.log("Response from server:", response); // Debugging
                        $(".preloader").hide();

                        if (response.isSuccess) {
                            // Check for correct response key
                            Swal.fire("Deleted!", response.message, "success").then(() => {
                                if (TaskManagementSystem?.Project?.Option?.Table) {
                                    TaskManagementSystem.Project.DowDocument();
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



}