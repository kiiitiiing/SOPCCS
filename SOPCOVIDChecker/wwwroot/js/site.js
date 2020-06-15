$(function () {
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        var target = $(this).data('target');
        var action = $(this).data('action');
        var content = $(action);
        $.when($(target).modal('show')).done(function () {
            $.ajax({
                url: url,
                tpye: 'get',
                async: true,
                success: function (data) {
                    content.html(data);
                },
                timeout: 60000,
                error: function (xhr, ajaxOptions, thrownError) {
                    $('body').find('#loadings').modal('toggle');
                    alert(xhr.responseText);
                    alert(thrownError);
                }
            });
        });
    });

    $('a[data-toggle="ajax-modal"]').click(function (event) {
        console.log('wtf');
        var url = $(this).data('url');
        var target = $(this).data('target');
        var action = $(this).data('action');
        var content = $(action);
        $.when($(target).modal('show')).done(function () {
            $.ajax({
                url: url,
                tpye: 'get',
                async: true,
                success: function (data) {
                    content.html(data);
                },
                timeout: 60000,
                error: function (xhr, ajaxOptions, thrownError) {
                    $('body').find('#loadings').modal('toggle');
                    alert(xhr.responseText);
                    alert(thrownError);
                }
            });
        });
    });

    var modals = $('#all_modals');
    modals.on('click', 'button[data-save="modal"]', function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();
        var content = $($(this).parent()).parent();
        var form = content.find('form');
        var contentId = content.attr('id');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();
        console.log(contentId);

        $.ajax({
            url: actionUrl,
            type: 'post',
            async: true,
            data: dataToSend,
            success: function (output) {
                var newBody = $('.modal-body', output);
                var errors = newBody.find('span.text-danger').text();
                if (errors == '') {
                    $.when(modals.find('.modal.show').modal('hide')).done(function () {
                        LoadingModal('#' + contentId);
                        if (contentId == 'add_patient_modal') {
                            Toast("Added new patient")
                            var vessel = $('#patients-patients');
                            LoadPatients('', vessel, 'Patients', 'PatientsJson');
                        }
                        else if (contentId == 'admin_patient_modal') {
                            Toast("Admitted patient")
                            LoadListPatients();
                        }
                        Toast('');
                    });
                }
                else
                    content.find('.modal-body').replaceWith(newBody);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.responseText);
                alert(thrownError);
            }
        })
    });


    var smallModal = $('#small_modal');
    smallModal.on('hidden.bs.modal', function () {
        var contentId = smallModal.find('.modal-dialog.modal-sm').attr('id');
        LoadingModal('#' + contentId);
    });
    var largeModal = $('#large_modal');
    largeModal.on('hidden.bs.modal', function () {
        var contentId = largeModal.find('.modal-dialog.modal-lg').attr('id');
        LoadingModal('#' + contentId);
    });
})

function MuncityOnChange(id,url) {
    var barangaysSelect = $('#barangays');
    if (id != '') {
        $.when(GetBarangayFiltered(id,url)).done(function (output) {
            barangaysSelect.empty()
                .append($('<option>', {
                    value: '',
                    text: 'Select Barangay'
                }));
            jQuery.each(output, function (i, item) {
                barangaysSelect.append($('<option>', {
                    value: item.id,
                    text: item.description
                }));
            });
        });
    }
    else {
        barangaysSelect.empty()
            .append($('<option>', {
                value: '',
                text: 'Select Barangay'
            }));
    }
}

function CaclAge(date) {
    var birth = new Date(date);
    var curr = new Date();
    var diff = curr.getTime() - birth.getTime();
    document.getElementById("Patient_Age").value = Math.floor(diff / (1000 * 60 * 60 * 24 * 365.25));
}

function GetBarangayFiltered(id,url) {
    var urls = url + '?muncityId=' + id;
    return $.ajax({
        url: urls,
        type: 'get',
        async: true
    });
}

function LoadingModal(id) {
    var urls = "/SOPCCS/Account/ModalLoading";
    return $.ajax({
        url: urls,
        type: 'get',
        async: true,
        success: function (output) {
            $(id).empty();
            $(id).html(output);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
            alert(thrownError);
        }
    });
}

//TOAST

function Toast(message) {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    Command: toastr["success"](message)

}


//PAGINATIONS
function LoadList(q, container, urls) {
    //var container = $('#' + vessel);
    var size = 5;
    var ctr = 0;
    var url = urls + '?q=' + q;
    var showArrows = true;
    console.log(url);
    container.pagination({
        dataSource: function (done) {
            $.ajax({
                type: 'GET',
                url: url,
                success: function (response) {
                    done(response);
                    ctr = response.length;
                }
            });
        },
        locator: 'items',
        pageSize: size,
        ajax: {
            beforeSend: function () {
                var html = AddLoading();
                container.prev().html(html);
            }
        },
        callback: function (response, pagination) {
            var action = '';
            var controller = '';
            if (container.attr('id') == 'sop-sop') {
                action = 'SopIndexPartial';
                controller = 'Sop'
            }
            else if (container.attr('id') == 'patients-actions') {
                action = 'ActivitiesPartial';
                controller = 'Patients';
            }
            else if (container.attr('id') == 'admin-patients') {
                action = 'IndexPartial';
                controller = 'Admin';
            }
            $.when(CallPartialView(response, controller, action)).done(function (output) {
                $('.total_ctr').html(ctr);
                if (ctr <= size)
                    container.hide();
                else
                    container.show();
                container.prev().empty();
                container.prev().html(output);
            })
        }
    })
}

//TEMPLATES

function CallPartialView(data, controller, action) {
    console.log(JSON.stringify(data));
    return $.ajax({
        url: "/SOPCCS/" + controller + "/" + action,
        type: "POST",
        async: true,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data)
    });
}