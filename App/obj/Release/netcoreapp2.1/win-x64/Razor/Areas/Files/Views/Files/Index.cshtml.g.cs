#pragma checksum "C:\Users\user\Desktop\Androtomist\App\Areas\Files\Views\Files\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9351b4ade50d657ccc3db23831c643481b87823c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Files_Views_Files_Index), @"mvc.1.0.view", @"/Areas/Files/Views/Files/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Files/Views/Files/Index.cshtml", typeof(AspNetCore.Areas_Files_Views_Files_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9351b4ade50d657ccc3db23831c643481b87823c", @"/Areas/Files/Views/Files/Index.cshtml")]
    public class Areas_Files_Views_Files_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Androtomist.Models.Database.Datatables.UploadsDatatable>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(65, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\Users\user\Desktop\Androtomist\App\Areas\Files\Views\Files\Index.cshtml"
  
    ViewData["Title"] = "Existing files";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(164, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            DefineSection("ScriptsHeader", async() => {
                BeginContext(189, 186, true);
                WriteLiteral("\r\n    <link href=\"/assets/vendors/custom/datatables/datatables.bundle.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n    <link href=\"/css/data_table.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n");
                EndContext();
            }
            );
            BeginContext(378, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(381, 84, false);
#line 13 "C:\Users\user\Desktop\Androtomist\App\Areas\Files\Views\Files\Index.cshtml"
Write(await Component.InvokeAsync("DataTableComponent", new { AbstractDatatable = Model }));

#line default
#line hidden
            EndContext();
            BeginContext(465, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
            DefineSection("ScriptsFooter", async() => {
                BeginContext(492, 292, true);
                WriteLiteral(@"
    <script defer src=""/assets/vendors/custom/datatables/datatables.bundle.js"" type=""text/javascript""></script>

    <script defer type=""text/javascript"">
        jQuery(document).ready(function ($) {
            DatatablesSearchOptionsColumnSearch.init();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Androtomist.Models.Database.Datatables.UploadsDatatable> Html { get; private set; }
    }
}
#pragma warning restore 1591
