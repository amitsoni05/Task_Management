

TaskManagementSystem.User = new function () {

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
        debugger;
        TaskManagementSystem.User.Option = $.extend({}, TaskManagementSystem.User.Option, options);
        TaskManagementSystem.User.Option.Table = $("#" + TaskManagementSystem.User.Option.TableId).DataTable({
            searching: false,
            serverSide: true,
            filter: true,
            orderMulti: true,
            bLengthChange: false,
            processing: true,
            lengthMenu: [[20, 40, 60, 50], [20, 40, 60, "All"]],
            pageLength: 10,
            ajax: {
                url: "/User/GetList",
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
                //{
                //    "data": "encProjectId", "className": "text-center", width: "100px", orderable: false,
                //    "render": function (data, type, row) {

                //        let btnDelete = '';
                //        let btnEdit = '';

                //        btnEdit = '<button title="Edit"  data-toggle="tooltip" data-placement="bottom" data-original-title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="TaskManagementSystem.Project.AddProject(\'' + data + '\');"><i class="fa fa-pencil"></i></button>';
                //        btnDelete = '<a  title="De-Activate" data-toggle="tooltip" data-placement="bottom" data-original-title="In-Active" class="btn btn-danger btn-sm mt-4 mr-1" onclick="TaskManagementSystem.Project.DeleteProject(\'' + data + '\',\'' + false + '\');"><i class="fa-solid fa-trash"></i></a>';
                //        //btnUpdImg = '<button title="Update Image"  data-toggle="tooltip" data-placement="bottom" data-original-title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="CricketCourtBookingSystem.Admin.ShowImg(\'' + data + '\');"><i class="fa-solid fa-upload"></i></button>';

                //        //btnShowImg = '<button title="Show Uploaded Photos"  data-toggle="tooltip" data-placement="bottom" data-original-title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="CricketCourtBookingSystem.Admin.ShowUploadImg(\'' + data + '\');"><i class="fa-solid fa-images"></i></button>';
                //        return btnEdit + btnDelete;
                //    }
                //},
            ],
        });
    }


    

    this.TaskInit = function (options) {
        debugger;
        TaskManagementSystem.User.Option = $.extend({}, TaskManagementSystem.User.Option, options);
        TaskManagementSystem.User.Option.Table = $("#" + TaskManagementSystem.User.Option.TableId).DataTable({
            searching: false,
            serverSide: true,
            filter: true,
            orderMulti: true,
            bLengthChange: false,
            processing: true,
            lengthMenu: [[20, 40, 60, 50], [20, 40, 60, "All"]],
            pageLength: 10,
            ajax: {
                url: "/User/GetTaskList",
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
                        else if (row.status==4) {
                            btn = '<span class="badge waves-effect waves-light btn-info">In Progress</span>';
                        }
                        return btn;
                    }
                },
                {
                    "data": "encTaskId", "className": "text-center", width: "150px", orderable: false,
                    "render": function (data, type, row) {

                        let Progress = '';
                        let Complete = '';
                        let UnAvailabel = '';
                        let Message = '';
                        if (row.status == 3) {
                            Progress = '<button  title="Progress" data-toggle="tooltip" data-placement="bottom" data-original-title="In-Active" class="btn btn-primary btn-sm mt-4 mr-1" onclick="TaskManagementSystem.User.Status(\'' + data + '\',\'' + 5 + '\');"><i class="fa-solid fa-spinner"></i></button>&nbsp;';
                            Complete = '<button  title="Complete" data-toggle="tooltip" data-placement="bottom" data-original-title="In-Active" class="btn bg-success btn-sm mt-4 mr-1" onclick="TaskManagementSystem.User.Status(\'' + data + '\',\'' + 2 + '\');"><i class="fa-solid fa-circle-check"></i></button>&nbsp;';
                            UnAvailabel = '<button  title="UnAvailabel" data-toggle="tooltip" data-placement="bottom" data-original-title="In-Active" class="btn btn-danger btn-sm mt-4 mr-1" onclick="TaskManagementSystem.User.Status(\'' + data + '\',\'' + 1 + '\');"> <i class="fa-solid fa-ban"></i></button>';
                            Message = '<button  title="Message" data-toggle="tooltip" data-placement="bottom" data-original-title="In-Active" class="btn btn-primary btn-sm mt-4 mr-1" onclick="TaskManagementSystem.User.Message(\'' + data + '\');"> <i class="fa-solid fa-message"></i></button>';

                        }
                     
                        //btnUpdImg = '<button title="Update Image"  data-toggle="tooltip" data-placement="bottom" data-original-title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="CricketCourtBookingSystem.Admin.ShowImg(\'' + data + '\');"><i class="fa-solid fa-upload"></i></button>';

                        //btnShowImg = '<button title="Show Uploaded Photos"  data-toggle="tooltip" data-placement="bottom" data-original-title="Edit" class="btn btn-primary btn-sm mt-4 mr-1" onclick="CricketCourtBookingSystem.Admin.ShowUploadImg(\'' + data + '\');"><i class="fa-solid fa-images"></i></button>';
                        return Progress + Complete + UnAvailabel + Message;
                    }
                },
            ],
        });
    }
   

    this.Search = function () {
        TaskManagementSystem.User.Option.Table.ajax.reload();
    }

    this.Status = function (Encid,id) {
        debugger;
        $(".loading-screen").show();
        $.ajax({
            url: "/User/StatusChange",
            type: "POST",
            data: { Encid: Encid, id: id },
            success: function (data) {
                $(".loading-screen").hide();
                if (data.isSuccess) {
                    debugger;
                    if (data.status == 2) {
                        $("#common-lg-dialog").modal('hide');
                        //TaskManagementSystem.User.ToastrSuccess("Task Complete Successfully");
                        window.location = "/User/Task";
                        TaskManagementSystem.User.Option.Table.ajax.reload();

                    }
                    else if (data.status == 5) {
                        $("#common-lg-dialog").modal('hide');
                        //TaskManagementSystem.User.ToastrSuccess("Task In Progress");
                        window.location = "/User/Task";
                        TaskManagementSystem.User.Option.Table.ajax.reload();

                    }
                    //else if (data.status == 1) {
                    //    $("#common-lg-dialog").modal('hide');
                    //    TaskManagementSystem.User.ToastrSuccess("Event Cancel Successfully");
                    //    TaskManagementSystem.User.BookOption.Table.ajax.reload();
                    //}

                } else {
                    TaskManagementSystem.User.ToastrError("Get Some Error in cancel");
                    TaskManagementSystem.User.Option.Table.ajax.reload();

                }
                window.location = "/User/Task";
                TaskManagementSystem.User.Option.Table.ajax.reload();

            },
            error: function (xhr, status, error) {
                $(".loading-screen").hide();
                TaskManagementSystem.User.ToastrError("Something went wrong: " + error);
            }
        });
    };



    this.Message = function (id) {
        debugger;
        $(".preloader").show();
        $.ajax({
            type: 'GET',
            data: {
                id: id            
            },      
        
            url: "/User/_ShowMessage",
            success: function (data) {
                $("#common-md-DialogContent").html(data);

                $("#common-md-dialog").modal('show');

                $(".preloader").hide();
            }
        });
    }


    this.SendMessage = function () {
        if ($("#MsgData").valid()) {
            $(".loading-screen").show();
            var formData = $("#MsgData").serialize();

            $.ajax({
                url: UrlContent("User/MsgSave"),
                type: "POST",
                data: formData,
                success: function (data) {
                    debugger;
                    $(".loading-screen").hide();
                    if (data.isSuccess) {
                       
                        TaskManagementSystem.User.Message(data.recId);
                    } else {
                        TaskManagementSystem.User.ToastrError("Get Some Error !");
                    }
                },
                error: function () {
                    $(".loading-screen").hide();
                    TaskManagementSystem.User.ToastrError("Something went wrong.");
                }
            });
        } else {
            TaskManagementSystem.User.ToastrError("Please Write !!");
        }
    }





    this.ToDoOption = {
        Calendar: null,
        CalenderId: "",
    };

    this.ToDoInit = function (options) {
            debugger;
        TaskManagementSystem.User.ToDoOption = $.extend({}, TaskManagementSystem.User.ToDoOption, options);

        let calendarEl = document.getElementById(TaskManagementSystem.User.ToDoOption.CalenderId);

        TaskManagementSystem.User.ToDoOption.Calendar = new FullCalendar.Calendar(calendarEl, {
            initialView: 'dayGridMonth',
            headerToolbar: {
                left: 'prev,next today',
                center: 'title',
                right: ''
            },
    
            showNonCurrentDates: false,
            eventTimeFormat: {
                hour: 'numeric',
                minute: '2-digit',
                omitZeroMinute: true,
                meridiem: true
            },

            eventSources: [
                {
                    events: function (fetchInfo, successCallback, failureCallback) {
                        console.log(fetchInfo);
                        var requestData = {
                            month: fetchInfo.start.getMonth() + 1,
                            year: fetchInfo.start.getFullYear()
                        };

                        $.ajax({
                            url: UrlContent("user/GetToDoCount"),
                            type: 'POST',
                            data: requestData,
                            success: function (data) {
                                debugger;
                                console.log(data);
                                successCallback(data);
                            },
                            error: function (err) {
                                failureCallback(err);
                            }
                        });
                    }
                },
            ],
            eventContent: function (info) {
                debugger;
                const count = info.event.extendedProps.totalCount ;
                const complete = info.event.extendedProps.complete;
                const pending = info.event.extendedProps.pending ;
                var htmlContent = ``;
             

                if (complete > 0) {
                    htmlContent += `
                                  <div class="text-center mt-2">
                                  <span class="badge bg-success" style="font-size: 16px; padding: 10px 20px;">✅ Completed: ${complete}</span>
                                  </div>
                                  `;
                }

                if (pending > 0) {
                    htmlContent += `
                                   <div class="text-center mt-2">
                                 <span class="badge bg-warning text-dark" style="font-size: 16px; padding: 10px 20px;">⏳ Pending: ${pending}</span>
                                 </div>
                                `;
                }


                return { html: htmlContent };
            },


            editable: false,
            disableDragging: true,
            droppable: false,
            eventLimit: true,
            selectable: true,

            eventClick: function (calEvent, jsEvent, view) {
                debugger;
                var currentDate = moment(calEvent.event.extendedProps.slotDate).format("YYYY-MM-DD");
                $.ajax({
                    url: UrlContent("User/_GetTaskByDate"),
                    type: "GET",
                    data: { date: currentDate },
                    success: function (response) {
                        $("#common-lg-DialogContent").html(response);
                        $("#common-lg-dialog").modal("show");
                    }
                });
            }

          
        });

        TaskManagementSystem.User.ToDoOption.Calendar.render();
    };

  


    }

