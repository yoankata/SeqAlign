using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SeqAlign.Controllers
{
    public class AlignmentController : ControllerBase
    {
        private readonly IHostingEnvironment environment;
        public AlignmentController(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        [HttpPost]
        [Route("/upload")]
        public async Task Upload()
        {
            if (HttpContext.Request.Form.Files.Any())
            {
                foreach (var file in HttpContext.Request.Form.Files)
                {
                    var path = Path.Combine(environment.ContentRootPath, "uploads", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
        }
    }
}
