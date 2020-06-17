#pragma checksum "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Sop\SopIndex.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2952aceebb5f596a2e6b5dbcbe2f664fdc9a0ab2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Sop_SopIndex), @"mvc.1.0.view", @"/Views/Sop/SopIndex.cshtml")]
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
using SOPCOVIDChecker.Models.AdminViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using System.Security.Claims;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using System.Globalization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Services;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2952aceebb5f596a2e6b5dbcbe2f664fdc9a0ab2", @"/Views/Sop/SopIndex.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"519d5f25b27282f78adef58f0ce574341b1c4600", @"/Views/_ViewImports.cshtml")]
    public class Views_Sop_SopIndex : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Sop\SopIndex.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
    <div class=""card card-success card-outline"">
        <div class=""card-header"">
            <div class=""row"">
                <div class=""col-md-6"">
                    Test Results for SARS-CoV-2 Viral RNA Detection by Real Time-Polymerase Chain Reaction
                    <br />
                    Total Patients: <span class=""total_ctr""></span>
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
                        <bu");
            WriteLiteral(@"tton type=""button"" id=""add-sop""
                                class=""btn btn-sm btn-primary""
                                data-toggle=""ajax-modal""
                                data-target=""#large_modal""
                                data-action=""#large_content""
                                data-url=""");
#nullable restore
#line 30 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Sop\SopIndex.cshtml"
                                     Write(Url.Action("AddSopModal","Sop"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@""">
                            <i class=""fa fa-user""></i>
                            &nbsp;New patient
                        </button>
                    </div>
                </div>
            </div>
        </div>
        <div class=""card-body"" id=""list_sop"">
            <div class=""overlay d-flex justify-content-center align-items-center"">
                <i class=""fas fa-2x fa-sync fa-spin""></i>
            </div>
            <br />
            <br />

        </div>
        <div id=""sop-sop"" style=""margin: auto; margin-bottom: 20px;""></div>
    </div>

    <script>
    var vessel = $('#sop-sop');
    LoadList('', vessel, '");
#nullable restore
#line 51 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Sop\SopIndex.cshtml"
                     Write(Url.Action("SopFormJson","Sop"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\');\r\n\r\n    $(function () {\r\n        $(\'#searchPatientBtn\').on(\'click\', function (event) {\r\n            event.preventDefault();\r\n            event.stopImmediatePropagation();\r\n            var q = $(\'#searchDetail\').val();\r\n            LoadList(q, vessel, \'");
#nullable restore
#line 58 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Sop\SopIndex.cshtml"
                            Write(Url.Action("SopFormJson","Sop"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\');\r\n        });\r\n    })\r\n    </script>");
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
