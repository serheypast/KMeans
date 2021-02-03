using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using DSPAI.Models;
using Engine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSPAI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            //todo move to uploadService
            var filePath = Path.Combine("E:\\university\\DSPAI\\DSPAI\\", "FileStorage");
            if (file.Length > 0)
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using (var fileStream = new FileStream(Path.Combine(filePath, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            var engineService = new EngineService();
            var image = engineService.GetImageByFile(file);
            var bitMap = engineService.GetBitMap(image);
            var grayScaleBitMap = engineService.MakeGrayscale(bitMap);
            var histogram = engineService.CreateHistogrtam(grayScaleBitMap);


            return Ok(new HistogramDTO()
            {
                Histogram = histogram,
                Path = Path.Combine(filePath, file.FileName)
            });
        }

    }
}