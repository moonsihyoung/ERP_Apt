using Erp_Apt_Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Controllers
{
    [Route("pdf")]
    public class PdfController : Controller
    {
        ICompositeViewEngine _wedew;

        public PdfController(ICompositeViewEngine wedew)
        {
            _wedew = wedew;
        }

        [Route("invoice")]
        public async Task<IActionResult> InvoiceAsync(int Aid, string Name)
        {
            using (var stringWriter = new StringWriter())
            {
                var viewResult = _wedew.FindView(ControllerContext, "Index", false);
                if (viewResult.View == null)
                {
                    throw new ArgumentNullException("보여 줄 수 없음.");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    viewDictionary,
                    TempData,
                    stringWriter,
                    new HtmlHelperOptions()
                    );

                await viewResult.View.RenderAsync(viewContext);
                var htmlToPdf = new HtmlToPdf(1000, 1414);
                htmlToPdf.Options.DrawBackground = true;

                var pdf = htmlToPdf.ConvertHtmlString(stringWriter.ToString());
                var pdfBytes = pdf.Save();
                string vae = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Second.ToString();
                using (var streamWriter = new StreamWriter(@"C:\" + vae + ".pdf"))
                {
                    await streamWriter.BaseStream.WriteAsync(pdfBytes, 0, pdfBytes.Length);
                }
                return File(pdfBytes, "application/pdf");

            }
        }
        

        [Route("website")]
        public async Task<IActionResult> WebsiteAsync(string Aid, string Name, string AptCode)
        {
            var mobileView = new HtmlToPdf();
            mobileView.Options.WebPageWidth = 1024;

            string Ad = Aid + "/" + Name + "/" + AptCode; //http://new.wedew.co.kr/Defect/Print/1094/Defect
            var tabletView = new HtmlToPdf();
            tabletView.Options.WebPageWidth = 1024;

            var desktopView = new HtmlToPdf();
            desktopView.Options.WebPageWidth = 1920;
            var strcode = "http://new.wedew.co.kr/Defect/Print/" + Ad;
            var pdf = tabletView.ConvertUrl(strcode);
            //mobileView.ConvertUrl("http://wedew.kr");
            //pdf.Append(tabletView.ConvertUrl("http://new.khmais.net"));
            //pdf.Append(desktopView.ConvertUrl("http://new.khmais.net"));

            var pdfBytes = pdf.Save();

            var te = DateTime.Now.Year.ToString();

            using (var streamWriter = new StreamWriter(@"D:\Temp\" + AptCode + te + ".pdf"))
            {
                await streamWriter.BaseStream.WriteAsync(pdfBytes, 0, pdfBytes.Length);
            }

            return File(pdfBytes, "application/pdf");
        }
    }
}
