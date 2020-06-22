#pragma checksum "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Pesu\PesuStatus.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e12d69ce396ad21a32dfb736d6ee81a1885f4b43"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Pesu_PesuStatus), @"mvc.1.0.view", @"/Views/Pesu/PesuStatus.cshtml")]
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
#line 1 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.ResuViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.AccountViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.SopViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.ResultViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.AdminViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using System.Security.Claims;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using System.Globalization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Services;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e12d69ce396ad21a32dfb736d6ee81a1885f4b43", @"/Views/Pesu/PesuStatus.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0e7f7b740b7bb2427e2f02041ea2dc92751e6014", @"/Views/_ViewImports.cshtml")]
    public class Views_Pesu_PesuStatus : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Pesu\PesuStatus.cshtml"
  
    ViewData["Title"] = "PESU";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"card card-warning card-outline\">\r\n    <div class=\"card-header\">\r\n        <div class=\"row\">\r\n            <div class=\"col-md-6\">\r\n                <h4>\r\n                    SAMPLES IN ");
#nullable restore
#line 11 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Pesu\PesuStatus.cshtml"
                          Write(ViewBag.Province.ToUpper());

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<br />
                    <small>Total: <span class=""total_ctr""></span></small>
                </h4>
            </div>
            <div class=""col-md-6"">
                <div class=""fa-pull-right form-inline ml-3"">
                    <!-- INPUT -->
                    <input class=""form-control form-control-sm""
                           name=""search""
                           id=""searchDetail""
                           placeholder=""Search name""
                           type=""text"" />
                    <button id=""searchPatientBtn"" type=""button"" class=""btn btn-sm btn-success"">
                        <i class=""fa fa-search""></i>
                    </button>
                </div>
            </div>
        </div>
    </div>
    <div class=""card-body"" id=""list_sop_resu"" style=""overflow:auto !important;"">
        <div class=""overlay d-flex justify-content-center align-items-center"">
            <i class=""fas fa-2x fa-sync fa-spin""></i>
        </div>
        <br />
        <br");
            WriteLiteral(" />\r\n\r\n    </div>\r\n    <div id=\"pesu-status\" style=\"margin: auto; margin-bottom: 20px;\"></div>\r\n</div>\r\n\r\n\r\n<script>\r\n    var vessel = $(\'#pesu-status\');\r\n    var c = [];\r\n    c.push(qFix($(\'#searchDetail\').val()));\r\n    LoadList(c, vessel, \'");
#nullable restore
#line 46 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Pesu\PesuStatus.cshtml"
                    Write(Url.Action("PesuStatusJson", "Pesu"));

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
#line 54 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Pesu\PesuStatus.cshtml"
                            Write(Url.Action("PesuStatusJson", "Pesu"));

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
