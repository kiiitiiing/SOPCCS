#pragma checksum "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\Admin\PatientsPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f36e87c8ff91c482117aefbd09ee22b12916acf9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_PatientsPartial), @"mvc.1.0.view", @"/Views/Admin/PatientsPartial.cshtml")]
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
#line 1 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.ResuViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.AccountViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.SopViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.ResultViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.AdminViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Models.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using System.Security.Claims;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using System.Globalization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\_ViewImports.cshtml"
using SOPCOVIDChecker.Services;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f36e87c8ff91c482117aefbd09ee22b12916acf9", @"/Views/Admin/PatientsPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d24f3d626af1eda3b26526485233e1624bc76d60", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_PatientsPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<PaginatedList<ListPatientModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\Admin\PatientsPartial.cshtml"
 if (Model.Count() > 0)
{

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    <table class=""table table-hover table-striped"">
        <thead class=""bg-gray-dark"">
            <tr>
                <th>
                    Name
                </th>
                <th>
                    Sex
                </th>
                <th>
                    Age
                </th>
                <th style=""white-space: nowrap;"">
                    Date of Birth
                </th>
                <th>
                    Address
                </th>
                <th>
                    Action
                </th>
            </tr>
        </thead>
        <tbody>
");
#nullable restore
#line 29 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\Admin\PatientsPartial.cshtml"
             foreach (var patient in Model)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n                    <td>\r\n                        <a class=\"text-primary\"");
            BeginWriteAttribute("onclick", "\r\n                           onclick=\"", 855, "\"", 981, 4);
            WriteAttributeValue("", 893, "showInPopup(\'", 893, 13, true);
#nullable restore
#line 34 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\Admin\PatientsPartial.cshtml"
WriteAttributeValue("", 906, Url.Action("EditPatient","Admin", new { patientId = patient.Id }), 906, 66, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 972, "\',", 972, 2, true);
            WriteAttributeValue(" ", 974, "\'def\')", 975, 7, true);
            EndWriteAttribute();
            WriteLiteral("\r\n                           style=\"cursor: pointer; font-weight:500;\">\r\n                            ");
#nullable restore
#line 36 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\Admin\PatientsPartial.cshtml"
                       Write(patient.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </a>\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 40 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\Admin\PatientsPartial.cshtml"
                   Write(patient.Sex);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 43 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\Admin\PatientsPartial.cshtml"
                   Write(patient.Age);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 46 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\Admin\PatientsPartial.cshtml"
                   Write(patient.DateOfBirth);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 49 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\Admin\PatientsPartial.cshtml"
                   Write(patient.Address);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        <a id=\"add-sop\"\r\n                           class=\"btn btn-sm btn-primary text-center\"");
            BeginWriteAttribute("onclick", "\r\n                           onclick=\"", 1667, "\"", 1791, 4);
            WriteAttributeValue("", 1705, "showInPopup(\'", 1705, 13, true);
#nullable restore
#line 54 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\Admin\PatientsPartial.cshtml"
WriteAttributeValue("", 1718, Url.Action("AddSopModal","Sop", new { patientId = patient.Id }), 1718, 64, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1782, "\',", 1782, 2, true);
            WriteAttributeValue(" ", 1784, "\'def\')", 1785, 7, true);
            EndWriteAttribute();
            WriteLiteral("\r\n                           style=\"width: 100%\">\r\n                            <i class=\"far fa-file-alt\"></i>\r\n                            &nbsp;\r\n                        </a>\r\n                    </td>\r\n                </tr>\r\n");
#nullable restore
#line 61 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\Admin\PatientsPartial.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n    </table>\r\n");
#nullable restore
#line 64 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\Admin\PatientsPartial.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"alert alert-warning\">\r\n        <span>\r\n            <i class=\"fa fa-exclamation-triangle\"></i> No patients found!\r\n        </span>\r\n    </div>\r\n");
#nullable restore
#line 72 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\Admin\PatientsPartial.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<div style=\"padding-top: 20px !important;\">\r\n    ");
#nullable restore
#line 76 "D:\Systems\SOPCCS\SOPCOVIDChecker\Views\Admin\PatientsPartial.cshtml"
Write(await Html.PartialAsync("~/Views/Shared/_PageList.cshtml", new PageListModel
    {
        Action = "Patients",
        HasNextPage = Model.HasNextPage,
        HasPreviousPage = Model.HasPreviousPage,
        PageIndex = Model._pageIndex,
        TotalPages = Model._totalPages,
        Container = "list_patient",
        Parameters = new Dictionary<string, string>
        {
            { "page", Model._pageIndex.ToString() },
            { "q", ViewBag.Search }
        }
    }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<PaginatedList<ListPatientModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
