using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bll.Algorithm;
using Dto;
using Bll;
using System.Web.Http.Cors;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AnalyzeReceiptController : ApiController
    {
        StartAlgoritem start = new StartAlgoritem();
        [HttpPost]
        [Route("api/AnalyzeReceipt")]
        public async Task<AllReceipt> PostFormData()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                var content = new StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(true));
                foreach (var header in Request.Content.Headers)
                {
                    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
                await content.ReadAsMultipartAsync(provider);
                string uploadingFileName = provider.FileData.Select(x => x.LocalFileName).FirstOrDefault();
                string text=File.ReadAllText(uploadingFileName);
                string originalFileName = String.Concat(root, "\\" + (provider.Contents[0].Headers.ContentDisposition.FileName).Trim(new Char[] { '"' }));
                var filename = provider.Contents[0].Headers.ContentDisposition.FileName;
                if (File.Exists(originalFileName))
                {
                    File.Delete(originalFileName);
                }
                File.Move(uploadingFileName, originalFileName);
               

                return start.AnlyzeReceipt(originalFileName);
            }
            catch (Exception)
            {
                return null;
            }


        }
    }
}