#pragma checksum "C:\CODEBASE\log-parser\LogParser\Pages\StatusCode.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "20223bd07f0b6a8006a5ffc0f194e38fb7a1450e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(LogParser.Pages.Pages_StatusCode), @"mvc.1.0.razor-page", @"/Pages/StatusCode.cshtml")]
namespace LogParser.Pages
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
#line 1 "C:\CODEBASE\log-parser\LogParser\Pages\_ViewImports.cshtml"
using LogParser;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"20223bd07f0b6a8006a5ffc0f194e38fb7a1450e", @"/Pages/StatusCode.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5c0474586b41ec0cb52d8ba02a999204dd1b1f3b", @"/Pages/_ViewImports.cshtml")]
    public class Pages_StatusCode : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\CODEBASE\log-parser\LogParser\Pages\StatusCode.cshtml"
  
    ViewData["Title"] = "Status Code @Model.ErrorStatusCode";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"container-fluid m-5 d-flex justify-content-center p-5\">\r\n    <div class=\"d-flex flex-column justify-content-center\">\r\n        <h1 class=\"text-danger display-1 text-center mb-5\">");
#nullable restore
#line 9 "C:\CODEBASE\log-parser\LogParser\Pages\StatusCode.cshtml"
                                                      Write(Model.ErrorStatusCode);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n        <i class=\"far fa-sad-tear\" style=\"font-size:250px;color:darkgray\"></i>\r\n    </div>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<StatusCodeModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<StatusCodeModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<StatusCodeModel>)PageContext?.ViewData;
        public StatusCodeModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
