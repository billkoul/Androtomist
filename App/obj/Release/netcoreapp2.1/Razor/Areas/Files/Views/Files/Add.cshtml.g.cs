#pragma checksum "C:\Users\user\Desktop\Androtomist\App\Areas\Files\Views\Files\Add.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a6b809d641a4b3b2b66a4a0a957327fd56f58ee3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Files_Views_Files_Add), @"mvc.1.0.view", @"/Areas/Files/Views/Files/Add.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Files/Views/Files/Add.cshtml", typeof(AspNetCore.Areas_Files_Views_Files_Add))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a6b809d641a4b3b2b66a4a0a957327fd56f58ee3", @"/Areas/Files/Views/Files/Add.cshtml")]
    public class Areas_Files_Views_Files_Add : Androtomist.Models.Database.FormElement<Androtomist.Models.Database.Entities.Submission>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(116, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 4 "C:\Users\user\Desktop\Androtomist\App\Areas\Files\Views\Files\Add.cshtml"
  
    ViewData["Title"] = "Upload";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(404, 418, true);
            WriteLiteral(@"<style>
    .dz-progress {
        display: none;
    }
</style>
<div>
    <div>
        <div>
            <h3>
                Upload new file
            </h3>
        </div>
    </div>
    <!--begin::Form-->
    <form id=""form_new_upload"" novalidate=""novalidate"" enctype=""multipart/form-data"">
        <input type=""hidden"" id=""RequestVerificationToken""
               name=""RequestVerificationToken""");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 822, "\"", 856, 1);
#line 31 "C:\Users\user\Desktop\Androtomist\App\Areas\Files\Views\Files\Add.cshtml"
WriteAttributeValue("", 830, GetAntiXsrfRequestToken(), 830, 26, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(857, 20, true);
            WriteLiteral(">\r\n        <div>\r\n\r\n");
            EndContext();
#line 34 "C:\Users\user\Desktop\Androtomist\App\Areas\Files\Views\Files\Add.cshtml"
               F(INPUT.SUB_ID); 

#line default
#line hidden
            BeginContext(912, 131, true);
            WriteLiteral("\r\n        </div>\r\n        <div>\r\n            <div>\r\n                <div class=\"row\">\r\n                    <div class=\"col-lg-9\">\r\n");
            EndContext();
            BeginContext(1071, 123, true);
            WriteLiteral("                            <button type=\"button\" class=\"btn btn-primary\" id=\"submit_it\" name=\"submit_it\">Upload</button>\r\n");
            EndContext();
            BeginContext(1221, 154, true);
            WriteLiteral("                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </form>\r\n    <!--end::Form-->\r\n</div>\r\n<!--end::Portlet-->\r\n\r\n");
            EndContext();
            DefineSection("ScriptsFooter", async() => {
                BeginContext(1398, 4104, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var form_elem = $('#form_new_upload');
        Dropzone.autoDiscover = false;
        $(document).ready(function () {

            $(document).on(""click"", "".upload_remove"", function (e) {
                e.preventDefault();

                var num = parseInt($(this).parent().prop(""id"").match(/\d+/g), 10);

                $.ajax({
                    url: '/files/files/deleteupload/' + num,
                    type: 'POST',
                    data: {
                        formJson: JSON.stringify(form_elem.serializeArray())
                    },
                    headers: {
                        ""Content-type"": ""application/x-www-form-urlencoded"",
                        ""RequestVerificationToken"": $('#RequestVerificationToken').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                    },
                    error: function (data) {
                    }
 ");
                WriteLiteral(@"               });

                $('#copy' + num).remove();

            });

            $("".input_dropzone"").dropzone({
                url: ""/files/files/uploadfiles"",
                uploadMultiple: false,
                maxFiles: 1,
                acceptedFiles: "".apk"",
                autoQueue: true,
                autoProcessQueue: true,
                maxFilesize: 400,
                addRemoveLinks: true,
                timeout: 0,
                success: function (file, e) {
                    this.removeAllFiles(true);
                    var id = ""file"";
                    $('#copy1').find('.upload_name').html(file.name);
                    $('#copy1').find('.upload_size').html(file.size);
                    $('#copy1').find('.thumbnail').prop('src', '');
                    $('#copy1').find('.thumbnail').prop('src', file.dataURL);
                    if (e !== null && !isNaN(e)) {
                        var id = ""file"";

                        var count =");
                WriteLiteral(@" $('div[target = ""' + id + '""]').length;
                        if (count < 10) {
                            var $div = $('#copy1[target = ""' + id + '""]:last');
                            var $last = $('div[target = ""' + id + '""]:last');
                            //var num = parseInt($last.prop(""id"").match(/\d+/g), 10) + 1;
                            var num = e;
                            var $klon = $div.clone().prop('id', 'copy' + num);
                            $last.after($klon);

                            $klon.show();
                        }
                    } else {
                        show_warning(""error"", ""error while uploading file"");
                    }
                },
                sending: function (file, xhr, formData) {
                    formData.append(""SUB_ID"", $(this.element).find("".SUB_ID"").val());
                    //console.log(file.size);
                }
            });

            $(""#submit_it"").on(""click"", function () {
        ");
                WriteLiteral(@"        $.ajax({
                    url: '/files/files/newupload/',
                    type: 'POST',
                    data: {
                        formJson: JSON.stringify(form_elem.serializeArray())
                    },
                    headers: {
                        ""Content-type"": ""application/x-www-form-urlencoded"",
                        ""RequestVerificationToken"": $('#RequestVerificationToken').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        if (data[""result""] == ""1"") {
                            window.location = ""/files/files/index/"";
                        } else {
                            show_warning(data[""msg""], data[""data""]);
                        }
                    },
                    error: function (data) {
                        show_warning(data[""msg""], data[""data""]);
                    }
                });
            });

        });

    </s");
                WriteLiteral("cript>\r\n");
                EndContext();
            }
            );
        }
        #pragma warning restore 1998
#line 9 "C:\Users\user\Desktop\Androtomist\App\Areas\Files\Views\Files\Add.cshtml"
           
    public string GetAntiXsrfRequestToken()
    {
        return Xsrf.GetAndStoreTokens(Context).RequestToken;
    }

#line default
#line hidden
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public Microsoft.AspNetCore.Antiforgery.IAntiforgery Xsrf { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Androtomist.Models.Database.Entities.Submission> Html { get; private set; }
    }
}
#pragma warning restore 1591
