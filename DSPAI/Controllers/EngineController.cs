using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSPAI.Models;
using Engine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSPAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngineController : ControllerBase
    {
        [HttpPost("Binarize")]
        public async Task<IActionResult> BinarizeImage(string path, int value)
        {
            Bitmap bitmap = new Bitmap(path);
            var engineService = new EngineService();
            var map = engineService.Binarization(bitmap, value);
            map.Save($"{path}");
            //todo

            return Ok();
        }
    }
}