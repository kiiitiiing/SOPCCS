#pragma checksum "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Admin\LabUsersPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fc86429c8b84c68fab983472922fcfd5f7bd80af"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_LabUsersPartial), @"mvc.1.0.view", @"/Views/Admin/LabUsersPartial.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fc86429c8b84c68fab983472922fcfd5f7bd80af", @"/Views/Admin/LabUsersPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0e7f7b740b7bb2427e2f02041ea2dc92751e6014", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_LabUsersPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<UserLess>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n");
#nullable restore
#line 4 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Admin\LabUsersPartial.cshtml"
 if (Model.Count() != 0)
{

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<div class=""table-responsive"">
        <table class=""table table-striped table-hover"" style=""font-size: 14px;"">
            <thead>
                <tr class=""bg-black"">
                    <th>Name</th>
                    <th>Facility</th>
                    <th>Contact / Email</th>
                    <th>Address</th>
                    <th>Username</th>
                </tr>
            </thead>
            <tbody>
");
#nullable restore
#line 17 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Admin\LabUsersPartial.cshtml"
                 foreach (var item in Model)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                    <tr>
                        <td>
                            <a class=""text-warning text-bold""
                               style=""cursor:pointer; font-size: 16px !important;""
                               data-toggle=""ajax-modal""
                               data-target=""#small_modal""
                               data-action=""#small_content""
                               data-url=""");
#nullable restore
#line 26 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Admin\LabUsersPartial.cshtml"
                                    Write(Url.Action("UpdateUser","Admin", new { userId = item.Id }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n                                ");
#nullable restore
#line 27 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Admin\LabUsersPartial.cshtml"
                           Write(item.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </a>\r\n                        </td>\r\n                        <td>");
#nullable restore
#line 30 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Admin\LabUsersPartial.cshtml"
                       Write(item.Facility);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td>\r\n                            ");
#nullable restore
#line 32 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Admin\LabUsersPartial.cshtml"
                       Write(item.ContactNo);

#line default
#line hidden
#nullable disable
            WriteLiteral("<br />\r\n                            <small class=\"text-success\">(");
#nullable restore
#line 33 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Admin\LabUsersPartial.cshtml"
                                                    Write(item.Email);

#line default
#line hidden
#nullable disable
            WriteLiteral(")</small>\r\n                        </td>\r\n                        <td>");
#nullable restore
#line 35 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Admin\LabUsersPartial.cshtml"
                       Write(item.Address);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 36 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Admin\LabUsersPartial.cshtml"
                       Write(item.Username);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    </tr>\r\n");
#nullable restore
#line 38 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Admin\LabUsersPartial.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </tbody>\r\n        </table>\r\n    </div>\r\n");
#nullable restore
#line 42 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Admin\LabUsersPartial.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"alert alert-warning\">\r\n        <span>\r\n            <i class=\"fa fa-exclamation-triangle\"></i> No Users found!\r\n        </span>\r\n    </div>\r\n");
#nullable restore
#line 50 "C:\Users\KitingKo\source\repos\SOPCOVIDChecker\SOPCOVIDChecker\Views\Admin\LabUsersPartial.cshtml"
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<UserLess>> Html { get; private set; }
    }
}
#pragma warning restore 1591
