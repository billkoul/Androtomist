#pragma checksum "C:\Users\user\Desktop\Androtomist\App\Areas\Results\Views\Results\Show.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1d8a43a51500477d7e23e7650993a5928b243459"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Results_Views_Results_Show), @"mvc.1.0.view", @"/Areas/Results/Views/Results/Show.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Results/Views/Results/Show.cshtml", typeof(AspNetCore.Areas_Results_Views_Results_Show))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1d8a43a51500477d7e23e7650993a5928b243459", @"/Areas/Results/Views/Results/Show.cshtml")]
    public class Areas_Results_Views_Results_Show : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Androtomist.Models.Database.Datatables.ResultsDatatable>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(65, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\Users\user\Desktop\Androtomist\App\Areas\Results\Views\Results\Show.cshtml"
  
    ViewData["Title"] = "View Results";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(162, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
            DefineSection("ScriptsFooter", async() => {
                BeginContext(189, 292, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Androtomist.Models.Database.Datatables.ResultsDatatable> Html { get; private set; }
    }
}
#pragma warning restore 1591
