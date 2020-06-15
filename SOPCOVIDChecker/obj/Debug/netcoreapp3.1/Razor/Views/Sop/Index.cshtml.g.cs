#pragma checksum "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Sop\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dbca2b1bb3dcb233091fadbf691fb4bd1f057f49"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Sop_Index), @"mvc.1.0.view", @"/Views/Sop/Index.cshtml")]
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
using SOPCOVIDChecker.Models.AccountViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using System.Security.Claims;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using System.Globalization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Services;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dbca2b1bb3dcb233091fadbf691fb4bd1f057f49", @"/Views/Sop/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"77d7513032fe7bef2e8f22f4fa4328cdea6b33e1", @"/Views/_ViewImports.cshtml")]
    public class Views_Sop_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Sop\Index.cshtml"
  
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
                <span class=""total_ctr""></span>
            </div>
            <div class=""col-md-6"">
                <div class=""fa-pull-right form-inline ml-3"">
                    <!-- INPUT -->
                    <input class=""form-control form-control-sm""
                           name=""search""
                           id=""searchDetail""
                           placeholder=""Lastname""
                           type=""text""");
            BeginWriteAttribute("value", "\r\n                           value=\"", 758, "\"", 815, 1);
#nullable restore
#line 22 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Sop\Index.cshtml"
WriteAttributeValue("", 794, ViewBag.SearchFilter, 794, 21, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" />
                    <button id=""searchPatientBtn"" type=""button"" class=""btn btn-sm btn-success"">
                        <i class=""fa fa-search""></i>
                    </button>
                    <button type=""button"" id=""add-sop""
                            class=""btn btn-sm btn-primary""
                            data-toggle=""ajax-modal""
                            data-target=""#large_modal""
                            data-action=""#large_content""
                            data-url=""");
#nullable restore
#line 31 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Sop\Index.cshtml"
                                 Write(Url.Action("AddSopModal","Sop"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n                        <i class=\"fa fa-user\"></i>\r\n                        &nbsp;Add User\r\n                    </button>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>");
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
