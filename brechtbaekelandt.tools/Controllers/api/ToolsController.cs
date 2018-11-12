using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brechtbaekelandt.tools.Controllers.api
{
    [Produces("application/json")]
    [Route("api/tools")]
    public class ToolsController : Controller
    {
        [Route("guid")]
        public IActionResult CreateGuidActionResult()
        {
            return this.Json(Guid.NewGuid());
        }

        [HttpPost("js/minify")]
        public IActionResult MinifyJavascriptActionResult([FromBody] string javascript)
        {
            var minifiedString = new Minifier().MinifyJavaScript(javascript);

            return this.Json(minifiedString);
        }

        [Route("css/minify")]
        public IActionResult MinifyCssActionResult([FromBody] string css)
        {
            var minifiedString = new Minifier().MinifyStyleSheet(css);

            return this.Json(minifiedString);
        }
    }
}