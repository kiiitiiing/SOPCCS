#pragma checksum "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a5b86ab72546d6fa57ccd154b78b2aa52d639768"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Sop_SopIndexPartial), @"mvc.1.0.view", @"/Views/Sop/SopIndexPartial.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a5b86ab72546d6fa57ccd154b78b2aa52d639768", @"/Views/Sop/SopIndexPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0e7f7b740b7bb2427e2f02041ea2dc92751e6014", @"/Views/_ViewImports.cshtml")]
    public class Views_Sop_SopIndexPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<SopLess>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
 if (Model.Count() != 0)
{

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    <table class=""table table-hover table-bordered table-responsive table-striped"" style=""white-space: nowrap!important "">
        <thead class=""bg-gray"">
            <tr>
                <th class=""text-center text-sm"">
                    SAMPLE ID
                </th>
                <th class=""text-center text-sm"">
                    PATIENT NAME
                </th>
                <th class=""text-center text-sm"">
                    AGE
                </th>
                <th class=""text-center text-sm"">
                    SEX
                </th>
                <th class=""text-center text-sm"">
                    DATE OF BIRTH
                </th>
                <th class=""text-center text-sm"">
                    PCR RESULT
                </th>
                <th class=""text-center text-sm"">
                    DISEASE-REPORTING UNIT
                </th>
                <th class=""text-center text-sm"">
                    ADDRESS
                </th>
          ");
            WriteLiteral(@"      <th class=""text-center text-sm"" colspan=""2"">
                    DATE &amp; TIME OF COLLECTION
                </th>
                <th class=""text-center text-sm"">
                    REQUESTED BY
                </th>
                <th class=""text-center text-sm"">
                    CONTACT NUMBER
                </th>
                <th class=""text-center text-sm"">
                    TYPE OF SPECIMEN &amp; COLLECTION MEDIUM
                </th>
                <th class=""text-center text-sm"" colspan=""2"">
                    DATE &amp; TIME OF SPECIMEN RECEIPT
                </th>
                <th class=""text-center text-sm"">
                    DATE OF RESULT RELEASE
                </th>
            </tr>
        </thead>
        <tbody>
");
#nullable restore
#line 53 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
             foreach (var sop in Model)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n                    <td class=\"text-center text-sm\">\r\n                        <label>");
#nullable restore
#line 57 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                          Write(sop.SampleId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 60 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.PatientName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 63 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.Age);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 66 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.Sex);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 69 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.DateOfBirth.GetDate("dd-MMM-yy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 72 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.PCRResult);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 75 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.DRU);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 78 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.Address);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 81 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.DateTimeCollection.GetDate("dd-MMM-yy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 84 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.DateTimeCollection.GetDate("h:mm tt"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 87 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.RequestedBy);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 90 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.RequesterContact);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 93 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.SpecimenCollection);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 96 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.DateTimeReceipt.GetDate("dd-MMM-yy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 99 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.DateTimeReceipt.GetDate("h:mm tt"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td class=\"text-center text-sm\">\r\n                        ");
#nullable restore
#line 102 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
                   Write(sop.DateResult.GetDate("dd-MMM-yy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                </tr>\r\n");
#nullable restore
#line 105 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n    </table>\r\n");
#nullable restore
#line 108 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"alert alert-warning\">\r\n        <span>\r\n            <i class=\"fa fa-exclamation-triangle\"></i> No patients found!\r\n        </span>\r\n    </div>\r\n");
#nullable restore
#line 116 "C:\Users\user\Source\Repos\SOPCCS\SOPCOVIDChecker\Views\Sop\SopIndexPartial.cshtml"
}

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<SopLess>> Html { get; private set; }
    }
}
#pragma warning restore 1591
