#pragma checksum "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "14e924a45b3228ba6cc92f9fd6f020a60cca0a31"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__ScriptsPartial), @"mvc.1.0.view", @"/Views/Shared/_ScriptsPartial.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.ResuViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.AccountViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.SopViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.ResultViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.AdminViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using System.Security.Claims;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using System.Globalization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Services;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"14e924a45b3228ba6cc92f9fd6f020a60cca0a31", @"/Views/Shared/_ScriptsPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0e7f7b740b7bb2427e2f02041ea2dc92751e6014", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__ScriptsPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
    <script>
        $(function () {
            $('button[data-toggle=""ajax-modal""]').click(function (event) {
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
     ");
            WriteLiteral(@"       });

            $('a[data-toggle=""ajax-modal""]').click(function (event) {
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

            var mo");
            WriteLiteral(@"dals = $('#all_modals');
            modals.on('click', 'button[data-save=""modal""]', function (event) {
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
                        console.log(errors);
                        if (errors == '') {
                            console.log('wtf')
         ");
            WriteLiteral(@"                   $.when(modals.find('.modal.show').modal('hide')).done(function () {
                                LoadingModal('#' + contentId);
                                if (contentId == 'new_sop') {
                                    Toast(""Submitted to LAB"")
                                    var vessel = $('#sop-sop');
                                    Window.href = '");
#nullable restore
#line 77 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                                              Write(Url.Action("SopIndex","Sop"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"';
                                }
                                else if (contentId == 'lab_modal') {
                                    Toast(""Sent to Lab"")
                                    var vessel = $('#resu-index');
                                    LoadList('', vessel, '");
#nullable restore
#line 82 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                                                     Write(Url.Action("ResuIndex","Resu"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"');
                                }
                                else if (contentId == 'add_staff_modal') {
                                    Toast(""Added new staff!"")
                                    var vessel = $('#result-staff');
                                    LoadList('', vessel, '");
#nullable restore
#line 87 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                                                     Write(Url.Action("LabUsersJson","Result"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"');
                                }
                                else if (contentId == 'add_patient_modal' || contentId == 'edit_patient_modal') {
                                    Toast(""Succes!"")
                                    var vessel = $('#sop-patients');
                                    var c = [];
                                    c.push(qFix($('#searchDetail').val()));
                                    LoadList(c, vessel, '");
#nullable restore
#line 94 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                                                    Write(Url.Action("PatientsJson","Sop"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"');
                                }
                                else if (contentId == 'edit_user_modal' || contentId == 'register_modal') {
                                    Toast(""Succesfull!"")
                                    var vessel = $('#admin-rhu');
                                    LoadList('', vessel, '");
#nullable restore
#line 99 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                                                     Write(Url.Action("RhuJson","Admin"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"');
                                }
                                else if (contentId == 'update_facility' || contentId == 'add_facility') {
                                    Toast(""Success!"")
                                    var vessel = $('#admin-facilities');
                                    var c = [];
                                    c.push(qFix($('#searchDetail').val()));
                                    LoadList(c, vessel, '");
#nullable restore
#line 106 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                                                    Write(Url.Action("FacilitiesJson","Admin"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"');
                                }
                                else if (contentId == 'result_modal') {
                                    var vessel = $('#lab-index');
                                    var c = [];
                                    c.push(qFix($('#searchDetail').val()));
                                    LoadList(c, vessel, '");
#nullable restore
#line 112 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                                                    Write(Url.Action("LabJson", "Result"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"');
                                    Toast(""Form Complete!"")
                                }
                            });
                        }
                        else {
                            content.find('.modal-body').replaceWith(newBody);
                            if (contentId == 'new_sop') {
                                var disabled = content.find('#notavailable');
                                if (disabled.is(':checked') == true) {
                                    content.find('#dateOnset').attr('disabled', true);
                                }
                            }
                            else if (contentId == 'add_patient_modal') {
                                var disabled = content.find('#sameAddress');
                                if (disabled.is(':checked') == true) {
                                    content.find('#permanent_address').css('display', 'none');
                                }
                            }
  ");
            WriteLiteral(@"                      }
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
        });

        function SameAddressToggle(field) {
            field.toggle();
        }

        function DisabledAddress(address, disabled) {
            console.log('wtfss');
            if (disabled == 'tr");
            WriteLiteral(@"ue') {
                address.css(""display"", ""none"");
            }
        }

        function OpenModal(btn) {
            var url = btn.data('url');
            var target = btn.data('target');
            var action = btn.data('action');
            console.log(url);
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
                        if (xhr.status === 401) {
                            location.reload();
                        }
                        else {
                            console.log(xhr.status)
                            $('body').find('#loadings').modal('toggle');
    ");
            WriteLiteral(@"                        alert(xhr.responseText);
                            alert(thrownError);
                        }

                    }
                });
            });
        }

        function MuncityOnChange(id, url) {
            var barangaysSelect = $('#barangays');
            if (id != '') {
                $.when(GetBarangayFiltered(id, url)).done(function (output) {
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
                    .appe");
            WriteLiteral(@"nd($('<option>', {
                        value: '',
                        text: 'Select Barangay'
                    }));
            }
        }

        function PMuncityOnChange(id, url) {
            var barangaysSelect = $('#pbarangays');
            if (id != '') {
                $.when(GetBarangayFiltered(id, url)).done(function (output) {
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
                     ");
            WriteLiteral(@"   value: '',
                        text: 'Select Barangay'
                    }));
            }
        }

        function ProvinceOnChange(id, url) {
            var MuncitySelect = $('#muncityFilters');
            if (id != '') {
                $.when(GetMuncityFiltered(id, url)).done(function (output) {
                    MuncitySelect.empty()
                        .append($('<option>', {
                            value: '',
                            text: 'Select City/Municipality'
                        }));
                    jQuery.each(output, function (i, item) {
                        MuncitySelect.append($('<option>', {
                            value: item.id,
                            text: item.description
                        }));
                    });
                });
            }
            else {
                MuncitySelect.empty()
                    .append($('<option>', {
                        value: '',
                      ");
            WriteLiteral(@"  text: 'Select City/Municipality'
                    }));
            }
        }

        function SetDisabled(input, attr) {
            input.attr(attr);
        }

        function CheckboxOnChange() {
            var inputOnset = $('#dateOnset');
            if (inputOnset.attr('disabled') == 'disabled') {
                inputOnset.removeAttr('disabled');
                inputOnset.attr(""data-val-required"", true);
            }
            else {
                inputOnset.val('');
                inputOnset.attr('disabled', true);
                inputOnset.removeAttr(""data-val-required"");
            }
        }

        function CaclAge(date) {
            var birth = new Date(date);
            var curr = new Date();
            var diff = curr.getTime() - birth.getTime();
            document.getElementById(""Patient_Age"").value = Math.floor(diff / (1000 * 60 * 60 * 24 * 365.25));
        }

        function GetMuncityFiltered(id, url) {
            var urls = url + '");
            WriteLiteral(@"?provinceId=' + id;
            console.log(urls);
            return $.ajax({
                url: urls,
                type: 'get',
                async: true
            });
        }

        function GetBarangayFiltered(id, url) {
            var urls = url + '?muncityId=' + id;
            console.log(urls);
            return $.ajax({
                url: urls,
                type: 'get',
                async: true
            });
        }

        function LoadingModal(id) {
            var urls = '");
#nullable restore
#line 318 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                   Write(Url.Action("ModalLoading", "Account"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"';
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
                ""closeButton"": false,
                ""debug"": false,
                ""newestOnTop"": false,
                ""progressBar"": false,
                ""positionClass"": ""toast-bottom-right"",
                ""preventDuplicates"": false,
                ""onclick"": null,
                ""showDuration"": ""300"",
                ""hideDuration"": ""1000"",
                ""timeOut"": ""5000"",
                ""extendedTimeOut"": ""1000"",
                ""showEasi");
            WriteLiteral(@"ng"": ""swing"",
                ""hideEasing"": ""linear"",
                ""showMethod"": ""fadeIn"",
                ""hideMethod"": ""fadeOut""
            }
            Command: toastr[""success""](message)

        }

        function AddLoading() {
            var loadi = '<div class=""d-flex justify-content-center align-items-center"" style=""position: absolute !important; top: 1; left: 50%""><i class=""fas fa-2x fa-sync fa-spin""></i></div>';
            return loadi;
        }


        //PAGINATIONS
        function LoadList(q, container, urls) {
            //var container = $('#' + vessel);
            var size = 5;
            var ctr = 0;
            var url = urls + '?q=' + q[0] + '&dr=' + q[1] + '&f=' + q[2];
            var showArrows = true;
            console.log(url);
            container.pagination({
                dataSource: function (done) {
                    $.ajax({
                        type: 'GET',
                        url: url,
                        success: fun");
            WriteLiteral(@"ction (response) {
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
                    console.log(container.attr('id'));
                    var params = GetParams(container.attr('id'));
                    $.when(CallPartialView(response,params)).done(function (output) {
                        $('.total_ctr').html(ctr);
                        if (ctr <= size)
                            container.hide();
                        else
                            container.show();
                        container.prev().empty();
        ");
            WriteLiteral(@"                container.prev().html(output);
                    })
                }
            })
        }

        function qFix(q) {
            if (q === undefined)
                return '';
            else
                return q;
        }

        function GetParams(id) {
            var params = [];
            var url = """";
            switch (id) {
                case 'sop-sop': {
                    url = '");
#nullable restore
#line 419 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                      Write(Url.Action("SopIndexPartial","Sop"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n");
            WriteLiteral("                    break;\r\n                }\r\n                case \'admin-rhu\': {\r\n                    url = \'");
#nullable restore
#line 425 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                      Write(Url.Action("RhuUsersPartial", "Admin"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n                    params.push(\'RhuUsersPartial\');\r\n                    params.push(\'Admin\');\r\n                    break;\r\n                }\r\n                case \'admin-pesu\': {\r\n                    url = \'");
#nullable restore
#line 431 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                      Write(Url.Action("PesuUsersPartial", "Admin"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n                    params.push(\'PesuUsersPartial\');\r\n                    params.push(\'Admin\');\r\n                    break;\r\n                }\r\n                case \'admin-resu\': {\r\n                    url = \'");
#nullable restore
#line 437 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                      Write(Url.Action("ResuUsersPartial", "Admin"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n                    params.push(\'ResuUsersPartial\');\r\n                    params.push(\'Admin\');\r\n                    break;\r\n                }\r\n                case \'admin-lab\': {\r\n                    url = \'");
#nullable restore
#line 443 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                      Write(Url.Action("LabUsersPartial", "Admin"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n                    params.push(\'LabUsersPartial\');\r\n                    params.push(\'Admin\');\r\n                    break;\r\n                }\r\n                case \'admin-facilities\': {\r\n                    url = \'");
#nullable restore
#line 449 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                      Write(Url.Action("FacilitiesPartial", "Admin"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n                    params.push(\'FacilitiesPartial\');\r\n                    params.push(\'Admin\');\r\n                    break;\r\n                }\r\n                case \'resu-index\': {\r\n                    url = \'");
#nullable restore
#line 455 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                      Write(Url.Action("ResuIndexPartial", "Resu"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n                    params.push(\'ResuIndexPartial\');\r\n                    params.push(\'Resu\');\r\n                    break;\r\n                }\r\n                case \'lab-index\': {\r\n                    url = \'");
#nullable restore
#line 461 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                      Write(Url.Action("LabIndexPartial", "Result"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n                    params.push(\'LabIndexPartial\');\r\n                    params.push(\'Result\');\r\n                    break;\r\n                }\r\n                case \'result-staff\': {\r\n                    url = \'");
#nullable restore
#line 467 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                      Write(Url.Action("LabUsersPartial", "Result"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n                    params.push(\'LabUsersPartial\');\r\n                    params.push(\'Result\');\r\n                    break;\r\n                }\r\n                case \'sop-status\': {\r\n                    url = \'");
#nullable restore
#line 473 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                      Write(Url.Action("SampleStatusPartial", "Sop"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n                    params.push(\'SampleStatusPartial\');\r\n                    params.push(\'Sop\');\r\n                    break;\r\n                }\r\n                case \'pesu-status\': {\r\n                    url = \'");
#nullable restore
#line 479 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                      Write(Url.Action("PesuStatusPartial", "Pesu"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n                    params.push(\'PesuStatusPartial\');\r\n                    params.push(\'Pesu\');\r\n                    break;\r\n                }\r\n                case \'resu-status\': {\r\n                    url = \'");
#nullable restore
#line 485 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                      Write(Url.Action("ResuStatusPartial", "Resu"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n                    params.push(\'ResuStatusPartial\');\r\n                    params.push(\'Resu\');\r\n                    break;\r\n                }\r\n                case \'sop-patients\': {\r\n                    url = \'");
#nullable restore
#line 491 "C:\Users\KitingKo\Desktop\DATA BACKUP\PISTING YAWA\SOPCCS\SOPCOVIDChecker\Views\Shared\_ScriptsPartial.cshtml"
                      Write(Url.Action("PatientsPartial", "Sop"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"'
                    params.push('PatientsPartial');
                    params.push('Sop');
                    break;
                }
            }

            return url;
        }

        //TEMPLATES

        function CallPartialView(data, url) {
            return $.ajax({
                url: url,
                type: ""POST"",
                async: true,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(data)
            });
        }
    </script>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591