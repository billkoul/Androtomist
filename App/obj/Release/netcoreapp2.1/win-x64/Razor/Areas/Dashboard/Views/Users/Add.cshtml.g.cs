#pragma checksum "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7f2cc1378e6b55cc624ddeb50aa43078df842564"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Dashboard_Views_Users_Add), @"mvc.1.0.view", @"/Areas/Dashboard/Views/Users/Add.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Dashboard/Views/Users/Add.cshtml", typeof(AspNetCore.Areas_Dashboard_Views_Users_Add))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7f2cc1378e6b55cc624ddeb50aa43078df842564", @"/Areas/Dashboard/Views/Users/Add.cshtml")]
    public class Areas_Dashboard_Views_Users_Add : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Androtomist.Models.Database.Entities.User>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(51, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 4 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
     if (ViewData["is_me"].ToString() == "yes") {
            ViewData["Title"] = "My Account";
        }
        else {
            ViewData["Title"] = (Model.Exists() ? "Edit user [" + Model.NAME + "]" : "Add new user");
        }

#line default
#line hidden
#line 9 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
         
        Layout = "~/Views/Shared/_Layout.cshtml";
    

#line default
#line hidden
            BeginContext(354, 190, true);
            WriteLiteral("\r\n<div>\r\n    <h3>\r\n        User properties\r\n    </h3>\r\n    <!--begin::Form-->\r\n    <form id=\"form_new_user\" novalidate=\"novalidate\">\r\n        <input type=\"hidden\" id=\"user_id\" name=\"user_id\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 544, "\"", 590, 1);
#line 19 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
WriteAttributeValue("", 552, Model.Exists() ? Model.USER_ID : -1, 552, 38, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(591, 285, true);
            WriteLiteral(@" />

            <div class=""col-md-6"">
                <div class=""form-group m-form__group"">
                    <label for=""USR_NAME"">Name</label>
                    <input type=""text"" class=""form-control m-input"" id=""USR_NAME"" name=""USR_NAME"" placeholder=""User name"" required");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 876, "\"", 919, 1);
#line 24 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
WriteAttributeValue("", 884, Model.Exists() ? Model.NAME : "", 884, 35, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(920, 423, true);
            WriteLiteral(@" />
                    <div class=""invalid-feedback"">This field is required.</div>
                </div>
            </div>
            <div class=""col-md-6"">
                <div class=""form-group m-form__group"">
                    <label for=""USR_SURNAME"">Surname</label>
                    <input type=""text"" class=""form-control m-input"" id=""USR_SURNAME"" name=""USR_SURNAME"" placeholder=""User Surname"" required");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 1343, "\"", 1389, 1);
#line 31 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
WriteAttributeValue("", 1351, Model.Exists() ? Model.SURNAME : "", 1351, 38, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1390, 364, true);
            WriteLiteral(@" />
                    <div class=""invalid-feedback"">This field is required.</div>
                </div>
            </div>

            <div class=""col-md-6"">
                <label for=""USR_USER_NAME"">Username</label>
                <input type=""text"" class=""form-control m-input"" id=""USR_USER_NAME"" name=""USR_USER_NAME"" placeholder=""username"" required");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 1754, "\"", 1802, 1);
#line 38 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
WriteAttributeValue("", 1762, Model.Exists() ? Model.USER_NAME : "", 1762, 40, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1803, 353, true);
            WriteLiteral(@" />
                <div class=""invalid-feedback"">This field is required.</div>
            </div>

            <div class=""form-group m-form__group"">
                <label for=""USR_PASSWORD"">Password</label>
                <input type=""password"" class=""form-control m-input"" id=""USR_PASSWORD"" name=""USR_PASSWORD"" placeholder=""password"" required");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 2156, "\"", 2203, 1);
#line 44 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
WriteAttributeValue("", 2164, Model.Exists() ? Model.PASSWORD : "", 2164, 39, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2204, 334, true);
            WriteLiteral(@" />
                <div class=""invalid-feedback"">This field is required.</div>
            </div>

            <div class=""form-group m-form__group"">
                <label for=""USR_EMAIL"">Email</label>
                <input type=""text"" class=""form-control m-input"" id=""USR_EMAIL"" name=""USR_EMAIL"" placeholder=""email"" required");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 2538, "\"", 2582, 1);
#line 50 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
WriteAttributeValue("", 2546, Model.Exists() ? Model.EMAIL : "", 2546, 36, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2583, 155, true);
            WriteLiteral(" />\r\n                <div class=\"invalid-feedback\">This field is required.</div>\r\n            </div>\r\n\r\n            <div class=\"m-form__group form-group\"  ");
            EndContext();
            BeginContext(2740, 66, false);
#line 54 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
                                               Write(ViewData["is_me"].ToString() == "yes" ? "style=display:none;" : "");

#line default
#line hidden
            EndContext();
            BeginContext(2807, 90, true);
            WriteLiteral(">\r\n                <label>User level</label>\r\n                <div class=\"m-radio-list\">\r\n");
            EndContext();
#line 57 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
                      
                        foreach (USER_LEVEL USER_LEVEL in (USER_LEVEL[])Enum.GetValues(typeof(USER_LEVEL)))
                        {
                            if (((int)Enum.Parse(typeof(USER_LEVEL), this.ViewData["user_level"].ToString())) <= 1)
                            {

#line default
#line hidden
            BeginContext(3205, 149, true);
            WriteLiteral("                            <label class=\"m-radio m-radio--state-success\">\r\n                                <input type=\"radio\" name=\"USR_USER_LEVEL\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 3354, "\"", 3373, 1);
#line 63 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
WriteAttributeValue("", 3362, USER_LEVEL, 3362, 11, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3374, 1, true);
            WriteLiteral(" ");
            EndContext();
            BeginContext(3377, 65, false);
#line 63 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
                                                                                          Write(Model.Exists() && Model.USER_LEVEL == USER_LEVEL ? "checked" : "");

#line default
#line hidden
            EndContext();
            BeginContext(3443, 44, true);
            WriteLiteral(" required>\r\n                                ");
            EndContext();
            BeginContext(3488, 10, false);
#line 64 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
                           Write(USER_LEVEL);

#line default
#line hidden
            EndContext();
            BeginContext(3498, 87, true);
            WriteLiteral("\r\n                                <span></span>\r\n                            </label>\r\n");
            EndContext();
#line 67 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
                                }
                            }
                        

#line default
#line hidden
            BeginContext(3678, 209, true);
            WriteLiteral("                </div>\r\n                <div class=\"invalid-feedback\">This field is required.</div>\r\n            </div>\r\n\r\n        <div>\r\n            <div class=\"row\">\r\n                <div class=\"col-lg-9\">\r\n");
            EndContext();
#line 77 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
                      
                        if (Model.Exists())
                        {

#line default
#line hidden
            BeginContext(3983, 128, true);
            WriteLiteral("                            <button type=\"submit\" id=\"submit_it\" class=\"btn btn-success\" name=\"submit_it\">Update User</button>\r\n");
            EndContext();
#line 81 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
                        }
                        else
                        {

#line default
#line hidden
            BeginContext(4195, 128, true);
            WriteLiteral("                            <button type=\"submit\" id=\"submit_it\" class=\"btn btn-success\" name=\"submit_it\">Create User</button>\r\n");
            EndContext();
#line 85 "C:\Users\user\Desktop\Androtomist\App\Areas\Dashboard\Views\Users\Add.cshtml"
                        }
                    

#line default
#line hidden
            BeginContext(4373, 83, true);
            WriteLiteral("                </div>\r\n            </div>\r\n        </div>\r\n    </form>\r\n</div>\r\n\r\n");
            EndContext();
            DefineSection("ScriptsFooter", async() => {
                BeginContext(4479, 796, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var form_elem = $('#form_new_user');

        $(document).ready(function () {

            $(document).on(""click"", ""#submit_it"", function () {

                $.post(""/dashboard/users/newuser/"", { formJson: JSON.stringify(form_elem.serializeArray()) }, function (data) {
                        if (data[""result""] == ""1"") {
                            window.location = ""/dashboard/users/index/"";
                        } else {
                            show_warning(data[""msg""], data[""data""]);
                        }
                    }, 'json').fail(function () {
                        show_error(""Cannot save user log."", ""Something went wrong"");
                    });
            });

        });

    </script>
");
                EndContext();
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Androtomist.Models.Database.Entities.User> Html { get; private set; }
    }
}
#pragma warning restore 1591
