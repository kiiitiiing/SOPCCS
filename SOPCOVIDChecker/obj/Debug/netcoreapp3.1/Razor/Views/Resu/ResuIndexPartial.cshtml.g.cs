#pragma checksum "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Resu\ResuIndexPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "95ba17e55c8af252a4a32afae7ad994aa0be68d3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Resu_ResuIndexPartial), @"mvc.1.0.view", @"/Views/Resu/ResuIndexPartial.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"95ba17e55c8af252a4a32afae7ad994aa0be68d3", @"/Views/Resu/ResuIndexPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0e7f7b740b7bb2427e2f02041ea2dc92751e6014", @"/Views/_ViewImports.cshtml")]
    public class Views_Resu_ResuIndexPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<ResultLess>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Resu\ResuIndexPartial.cshtml"
 if (Model.Count() != 0)
{

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    <table class=""table table-bordered table-striped"" style=""white-space: nowrap!important; overflow-y: auto !important;"">
        <thead class=""bg-gray"">
            <tr>
                <th class=""text-center text-sm"">
                    SAMPLE ID
                </th>
                <th class=""text-center text-sm"">
                    PATIENT NAME
                </th>
                <th class=""text-center text-sm"">
                    DRU
                </th>
                <th class=""text-center text-sm"">
                    PCR RESULT
                </th>
                <th class=""text-center text-sm"" colspan=""2"">
                    SAMPLE TAKEN
                </th>
            </tr>
        </thead>
        <tbody>
");
#nullable restore
#line 26 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Resu\ResuIndexPartial.cshtml"
             foreach (var sop in Model)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td class=\"text-center text-sm\">\r\n                    <label>");
#nullable restore
#line 30 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Resu\ResuIndexPartial.cshtml"
                      Write(sop.SampleId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                </td>\r\n                <td class=\"text-center text-sm\">\r\n                    ");
#nullable restore
#line 33 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Resu\ResuIndexPartial.cshtml"
               Write(sop.PatientName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td class=\"text-center text-sm\">\r\n                    ");
#nullable restore
#line 36 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Resu\ResuIndexPartial.cshtml"
               Write(sop.DRU);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td class=\"text-center text-sm\">\r\n                    ");
#nullable restore
#line 39 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Resu\ResuIndexPartial.cshtml"
               Write(sop.PCRResult);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td class=\"text-center text-sm\">\r\n                    ");
#nullable restore
#line 42 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Resu\ResuIndexPartial.cshtml"
               Write(sop.SampleTaken.GetDate("dd-MMM-yy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td class=\"text-center text-sm\">\r\n                    ");
#nullable restore
#line 45 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Resu\ResuIndexPartial.cshtml"
               Write(sop.SampleTaken.GetDate("hh:mm tt"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n");
            WriteLiteral("            </tr>\r\n");
#nullable restore
#line 59 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Resu\ResuIndexPartial.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n    </table>\r\n");
#nullable restore
#line 62 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Resu\ResuIndexPartial.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"alert alert-warning\">\r\n        <span>\r\n            <i class=\"fa fa-exclamation-triangle\"></i> No Sample found!\r\n        </span>\r\n    </div>\r\n");
#nullable restore
#line 70 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Resu\ResuIndexPartial.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<script>\r\n</script>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<ResultLess>> Html { get; private set; }
    }
}
#pragma warning restore 1591
