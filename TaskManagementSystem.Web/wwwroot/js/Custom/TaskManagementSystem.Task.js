TaskManagementSystem.Task = new function () {

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
  
        TaskManagementSystem.Task.Option = $.extend({}, TaskManagementSystem.Task.Option, options);
        TaskManagementSystem.Task.Option.Table = $("#" + TaskManagementSystem.Task.Option.TableId).DataTable({
            searching: false,
            serverSide: true,
            filter: true,
            orderMulti: true,
            bLengthChange: false,
            processing: true,
            lengthMenu: [[20, 40, 60, 50], [20, 40, 60, "All"]],
            pageLength: 10,
            ajax: {
                url: "/Task/GetList",
                type: "Post",
                datatype: "json",
                data: function (dtParms) {
                    dtParms.search.value = $("#txtSearch").val();

                    return dtParms;
                }
            },
            order: [[1, "ASC"]],
            "columns": [
                { "data": "title", "name": "title", "autoWidth": false, orderable: true, width: "100px", },
                { "data": "description", "name": "description", "autoWidth": true, orderable: true },
                { "data": "createdDate", "name": "createdDate", "autoWidth": false, orderable: true, width: "100px", },             
                { "data": "deadLine", "name": "deadLine", "autoWidth": true, orderable: true },      
                { "data": "assignTo", "name": "assignTo", "autoWidth": true, orderable: true },      
                { "data": "completeBy", "name": "completeBy", "autoWidth": true, orderable: true },
                { "data": "projectName", "name": "projectName", "autoWidth": true, orderable: true },
                {
                    "data": "priorityString", "name": "priorityString", "autoWidth": true, orderable: true,
                     "render": function (data, type, row) {
                         let btn = '';
                         if (data == 'Low') {
                             btn = '<span class="badge bg-danger">Low</span>';
                         }
                         else if (data == 'Medium') {
                             btn = '<span class="badge bg-secondary">Medium</span>';
                         }
                         else if (data == 'High') {
                             btn = '<span class="badge bg-success">High</span>';
                         }
                        
                         return btn;
                     }


                },
                {
                    "data": "statusString", "name": "statusString", "autoWidth": true, orderable: true,

                    "render": function (data, type, row) {
             
                        let btn = '';
                        if (row.status == 1) {
                            btn = '<span class="badge bg-danger">UnAvailabel</span>';
                        }
                        else if (row.status == 3) {
                            btn = '<span class="badge bg-secondary">Pending</span>';
                        }
                        else if (row.status == 2) {
                            btn = '<span class="badge bg-success">Confirm</span>';
                        }
                        else {
                            btn = '<span class="badge waves-effect waves-light btn-info">In Progress</span>';
                        }
                        return btn;
                    }
                },
                {
                    "data": "encTaskId", "className": "text-center", width: "100px", orderable: false,
                    "render": function (data, type, row) {

                        let btnDelete = '';
                        let btnEdit = '';
                        if (row.status == 2 || row.status==1) {
                            btnUser = '<button title="User"  data-toggle="tooltip" data-placement="bottom" data-original-title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="TaskManagementSystem.Task.ShowUser(\'' + data + '\');"><i class="fa-solid fa-user-plus"></i></button>';

                        } else {
                            btnEdit = '<button title="Edit"  data-toggle="tooltip" data-placement="bottom" data-original-title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="TaskManagementSystem.Task.AddTask(\'' + data + '\');"><i class="fa fa-pencil"></i></button>';
                            btnDelete = '<button  title="De-Activate" data-toggle="tooltip" data-placement="bottom" data-original-title="In-Active" class="btn btn-danger btn-sm mt-4 mr-1" onclick="TaskManagementSystem.Task.DeleteTask(\'' + data + '\',\'' + false + '\');"><i class="fa-solid fa-trash"></i></button>';
                            btnUser = '<button title="User"  data-toggle="tooltip" data-placement="bottom" data-original-title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="TaskManagementSystem.Task.ShowUser(\'' + data + '\');"><i class="fa-solid fa-user-plus"></i></button>';

                        }

                     
                        return btnEdit + btnDelete + btnUser;
                    }
                },
            ],
        });
    }


    this.Search = function () {
        TaskManagementSystem.Task.Option.Table.ajax.reload();
    }


    this.ShowUser = function (id) {
        debugger;
        $(".preloader").show();
        $.ajax({
            type: 'GET',
            data: { id: id },
            url: "/Task/_ShowUser",
            success: function (data) {
                $("#common-lg-DialogContent").html(data);

                $("#common-lg-dialog").modal('show');
                $(".preloader").hide();
            }
        });
    }



    this.AddTask = function (id) {
      
        $(".preloader").show();
        $.ajax({
            type: 'GET',
            data: { id: id },
            url: "/Task/_AddTask",
            success: function (data) {
                $("#common-lg-DialogContent").html(data);

                $("#common-lg-dialog").modal('show');
                $(".preloader").hide();
            }
        });
    }



   


    this.UpdateTask = function () {
        debugger;

        if ($("#Taskdata").valid()) {
            $(".loading-screen").show();
            var formData = $("#Taskdata").serialize();

            $.ajax({
                url: UrlContent("Task/SaveTask"),
                type: "POST",
                data: formData,
                success: function (data) {
                    debugger;
                    $(".loading-screen").hide();
                    if (data.isSuccess) {
                        debugger;
                        $("#common-lg-dialog").modal('hide');
                        TaskManagementSystem.Task.ToastrSuccess("Employee Update Successfully");
                        TaskManagementSystem.Task.Option.Table.ajax.reload();
                        window.location = UrlContent("Task/Index");
                    } else {
                        $("#password").val("");
                        $("#txtCaptcha").val("");
                        TaskManagementSystem.Task.ToastrError("Please Enter A Valid Data");
                        document.getElementById("errorMessage").hidden = false;
                        document.getElementById("errorMessageText").innerText = data.message;

                    }
                }, error: function (xhr, status, error) {
                    $(".loading-screen").hide();
                }
            });
        } else {
            TaskManagementSystem.Task.ToastrError("Please Select all the fileds");
        }
    }



    this.DeleteTask = function (id) {
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
                $(".preloader").show();
                $.ajax({
                    url: "/Task/DeleteTask",  // Ensure this matches your actual route
                    type: "POST",
                    data: { id: id },
                    success: function (response) {
                        console.log("Response from server:", response); // Debugging
                        $(".preloader").hide();

                        if (response.isSuccess) {
                            // Check for correct response key
                            Swal.fire("Deleted!", response.message, "success").then(() => {
                                if (TaskManagementSystem?.Task?.Option?.Table) {
                                    TaskManagementSystem.Task.Option.Table.ajax.reload();
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
    let uploadedFiles = [];
    let documentType = [];

    const documentTypeMap = {
        "Image": "Image",
        "PDF": "PDF",
        "Excel": "Excel",
         "1": "Image",
        "2": "PDF",
        "3": "Excel"
    };

    // Append row to file table
    this.appendFileRow = function (fileName, docTypeText) {
        
        $("#fileTable .no-data").remove();

        const rowId = `row-${fileIdCounter}`;
        $("#fileTable tbody").append(`
        <tr id="${rowId}">
            <td>${fileIdCounter}</td>
            <td>${docTypeText}</td>
            <td>${fileName}</td>
            <td>
                <button type="button" class="btn btn-danger btn-sm delete-btn" data-row-id="${rowId}" data-filename="${fileName}">
                    Delete
                </button>
            </td>
        </tr>
    `);

        fileIdCounter++;
    };

    // Remove row from file table
    this.removeFileRow = function (rowId, fileName) {
        $(`#${rowId}`).remove();
        uploadedFiles = uploadedFiles.filter(f => f.fileName !== fileName);
        documentType = documentType.filter(d => d.fileName !== fileName);
    };

    // Handle file selection
    $(document).on("change", "#files", function (e) {
        debugger;
        if (e.target.files.length > 1) {
            TaskManagementSystem.Task.ToastrError("Please select only one file.");
            
            $("#files").val("");
            return;
        }

        selectedFile = e.target.files[0];
        //uploadedFiles.push({ file: selectedFile, fileName: selectedFile.name });
    });

    // Handle file deletion
    $(document).on("click", ".delete-btn", function () {
        debugger;
        const rowId = $(this).data("row-id");
        const fileName = $(this).data("filename");

        TaskManagementSystem.Task.removeFileRow(rowId, fileName);
        TaskManagementSystem.Task.ToastrSuccess("file removed");

        if ($("#fileTable tbody tr").length === 0) {
            $("#fileTable tbody").append(`
            <tr class="no-data">
                <td colspan="4" class="text-center">No Data Available</td>
            </tr>
        `);
        }
        
    });

    // Upload file button click
    this.uploadFile = function () {
      
        const docTypeValue = $("#FileSelect").val();
        const docTypeText = documentTypeMap[docTypeValue];
      
        if (!docTypeValue) {
            TaskManagementSystem.Task.ToastrError("Please select a document type.");
             
              return;
           }

        if (!selectedFile) {
            TaskManagementSystem.Task.ToastrError("Please select a file.");

            return;
        }
   

        const formData = new FormData();
        formData.append("file", selectedFile);

        $.ajax({
            url: '/Task/UploadFile',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (res) {
                debugger;
                if (res.success) {
                    TaskManagementSystem.Task.appendFileRow(res.fileName, docTypeText);

                    uploadedFiles.push({ file: selectedFile, fileName: res.fileName });
                    documentType.push({ fileName: res.fileName, file: docTypeText });
                    TaskManagementSystem.Task.ToastrSuccess("File uploaded successfully");
                    
                } else {
                    TaskManagementSystem.Task.ToastrError(res.message);

                    
                }

                selectedFile = null;
                $("#files").val("");
            },
            error: function () {
                TaskManagementSystem.task.ToastrError("Error uploading file.");
            }
        });
    };

   
    if (Array.isArray(window.existingFiles)) {
        debugger;
        window.existingFiles.forEach(file => {
            const docTypeText = documentTypeMap[file.DocumentType];
            TaskManagementSystem.Task.appendFileRow(file.DocumentName, docTypeText);

            uploadedFiles.push({ file: null, fileName: file.DocumentName });
            documentType.push({ fileName: file.DocumentName, file: file.DocumentType });
            fileIdCounter++;
        }); 
        console.log("Loaded existingFiles:", window.existingFiles);
    }

    // Save Task
    this.SaveTask = function () {
        debugger;
        var filestype = $("#FileSelect").val();
        var file = $("#files").val();
        if ($("#Taskdata").valid() || ( filestype=='' && file=='')) {
            $(".loading-screen").show();

            let formData = new FormData();

            uploadedFiles.forEach(f => {
               formData.append("HiteshTaskAssignTaskModel.Files", f.file);
            });
                   

            documentType.forEach(d => {
                formData.append("HiteshTaskAssignTaskModel.DocumentTypeList", d.file);
            });

            (window.existingFiles || []).forEach(file => {
                formData.append("HiteshTaskAssignTaskModel.ExistingFiles", file.DocumentName);
                formData.append("HiteshTaskAssignTaskModel.ExistingDocumentTypes", file.DocumentType);
            });

            $("#Taskdata").serializeArray().forEach(field => {
                formData.append(field.name, field.value);
            });

            $.ajax({
                url: UrlContent("Task/SaveTask"),
                type: "POST",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    $(".loading-screen").hide();
                    if (data.success) {
                        $("#common-lg-dialog").modal('hide');
                        TaskManagementSystem.Task.ToastrSuccess("Task Add Successfully");
                        window.location = UrlContent("Task/Index");
                        location.reload();
                    } else {
                        $("#password").val("");
                        TaskManagementSystem.Project.ToastrError("Please Enter A Valid Data");
                        $("#errorMessageText").text(data.message);
                    }
                },
                error: function () {
                    $(".loading-screen").hide();
                    TaskManagementSystem.Task.ToastrError("Server error while saving task");

                }
            });
        } else {
            TaskManagementSystem.Task.ToastrError("Please fill in all required fields.");
        }
    };




}