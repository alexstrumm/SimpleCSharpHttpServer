using System.Collections.Generic;
using System.Text;
using AspFromScratch.Services;
using System.Linq;

namespace AspFromScratch.Controllers {
    public class UsersController {
        private readonly IDataService dataService;

        public UsersController(IDataService dataService) {
            this.dataService = dataService;
        }

        [HttpMethod("GET")]
        public string Index() {
            var names = this.dataService.GetNames().ToArray();

            var htmlContent = new StringBuilder()
                .AppendLine("<!DOCTYPE html>")
                .AppendLine("<html>")
                .AppendLine("   <head>")
                .AppendLine("       <title>My Simple Web Server with C# and HttpListener</title>")
                .AppendLine("       <style>")
                .AppendLine("           h { color: red; }")
                .AppendLine("           p { color: blue; }")
                .AppendLine("       </style>")
                .AppendLine("   </head>")
                .AppendLine("<body>")
                .AppendLine("   <form action='create' method='post'>")
                .AppendLine("       <input type='text' name='name' placeholder='name'>")
                .AppendLine("       <button type='submit'>create</button>")
                .AppendLine("   </form>")
                .AppendLine("   <table>")
                .AppendLine("       <tr><td>ID</td><td>Name</td></tr>");
            for (int i = 0; i < names.Length; ++i) {
                htmlContent
                    .AppendLine($"      <tr><td>{i}</td><td>{names[i]}</td></tr>");
            }
            htmlContent
                .AppendLine("   </table>")
                .AppendLine("</body>")
                .AppendLine("</html>");

            return htmlContent.ToString();
        }

        [HttpMethod("Post")]
        public string Create(string name) {
            this.dataService.AddName(name);
            var names = this.dataService.GetNames().ToArray();

            var htmlContent = new StringBuilder()
                .AppendLine("<!DOCTYPE html>")
                .AppendLine("<html>")
                .AppendLine("   <head>")
                .AppendLine("       <title>My Simple Web Server with C# and HttpListener</title>")
                .AppendLine("       <style>")
                .AppendLine("           h { color: red; }")
                .AppendLine("           p { color: blue; }")
                .AppendLine("       </style>")
                .AppendLine("   </head>")
                .AppendLine("<body>")
                .AppendLine("   <form action='create' method='post'>")
                .AppendLine("       <input type='text' name='name' placeholder='name'>")
                .AppendLine("       <button type='submit'>create</button>")
                .AppendLine("   </form>")
                .AppendLine("   <table>")
               .AppendLine("       <tr><td>ID</td><td>Name</td></tr>");
            for (int i = 0; i < names.Length; ++i) {
                htmlContent
                    .AppendLine($"      <tr><td>{i}</td><td>{names[i]}</td></tr>");
            }
            htmlContent
                .AppendLine("   </table>")
                .AppendLine("</body>")
                .AppendLine("</html>");

            return htmlContent.ToString();
        }
    }
}
