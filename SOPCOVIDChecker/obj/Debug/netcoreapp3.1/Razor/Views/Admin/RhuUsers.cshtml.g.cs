#pragma checksum "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Admin\RhuUsers.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "49a10893d3341330cae9dd0cb59b9a0bc57f1510"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_RhuUsers), @"mvc.1.0.view", @"/Views/Admin/RhuUsers.cshtml")]
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
#line 1 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.ResuViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.AccountViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.SopViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.ResultViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.AdminViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using System.Security.Claims;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using System.Globalization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Services;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"49a10893d3341330cae9dd0cb59b9a0bc57f1510", @"/Views/Admin/RhuUsers.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0e7f7b740b7bb2427e2f02041ea2dc92751e6014", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_RhuUsers : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Admin\RhuUsers.cshtml"
  
    ViewData["Title"] = "RHU Users";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
    <div class=""col-md-12"">
        <div class=""card card-success card-outline"">
            <div class=""card-header"">
                <div class=""fa-pull-right form-inline ml-3"">
                    <div class=""form-actions no-color"">
                        <input type=""text"" class=""form-control form-control-sm"" id=""searchDetail"" placeholder=""Search name"" name=""name"" />
                        <button type=""button"" id=""searchPatientBtn"" class=""btn btn-sm btn-success"">
                            <i class=""fa fa-search""></i>
                            &nbsp;Search
                        </button>
                        <button type=""button"" id=""register""
                                class=""btn btn-sm btn-primary""
                                data-toggle=""ajax-modal""
                                data-target=""#small_modal""
                                data-action=""#small_content""
                                data-url=""");
#nullable restore
#line 21 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Admin\RhuUsers.cshtml"
                                     Write(Url.Action("Register","Account", new { level = "RHU" }));

#line default
#line hidden
#nullable disable
            WriteLiteral(@""">
                            <i class=""fa fa-user""></i>
                            &nbsp;Add User
                        </button>
                    </div>
                </div>
                <h4>RHU Users</h4>
            </div>
            <div class=""card-body"">
            </div>
            <div id=""admin-rhu"" style=""margin: auto; margin-bottom: 20px;""></div>
        </div>
    </div>

<script>
    var vessel = $('#admin-rhu');
    var c = [];
    c.push(qFix($('#searchDetail').val()));
    LoadList(c, vessel, '");
#nullable restore
#line 39 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Admin\RhuUsers.cshtml"
                    Write(Url.Action("RhuJson","Admin"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"');

    $(function () {
        $('#searchPatientBtn').on('click', function (event) {
            event.preventDefault();
            event.stopImmediatePropagation();
            var q = []
            q.push(qFix($('#searchDetail').val()));
            LoadList(q, vessel, '");
#nullable restore
#line 47 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Admin\RhuUsers.cshtml"
                            Write(Url.Action("RhuJson","Admin"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\');\r\n        });\r\n    })\r\n</script>");
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
