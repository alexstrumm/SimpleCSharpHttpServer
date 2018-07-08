using System.Collections.Generic;
using System.Text;

namespace AspFromScratch.Controllers {
    class UsersController {
        static List<string> users = new List<string> {
                "Alex",
                "Laman",
                "Tural",
                "Ramin",
                "Samir",
                "Fuad",
                "Nadir",
                "Ismayil",
                "Iqor",
                "Mirza",
                "Namiq",
                "Tabriz",
                "Bunnyaminarmynow"
            };

        [HttpMethod("GET")]
        public string Index() {
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
            for (int i = 0; i < users.Count; ++i) {
                htmlContent
                    .AppendLine($"      <tr><td>{i}</td><td>{users[i]}</td></tr>");
            }
            htmlContent
                .AppendLine("   </table>")
                .AppendLine("</body>")
                .AppendLine("</html>");

            return htmlContent.ToString();
        }

        [HttpMethod("Post")]
        public string Create(string name) {
            users.Add(name);

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
            for (int i = 0; i < users.Count; ++i) {
                htmlContent
                    .AppendLine($"      <tr><td>{i}</td><td>{users[i]}</td></tr>");
            }
            htmlContent
                .AppendLine("   </table>")
                .AppendLine("</body>")
                .AppendLine("</html>");

            return htmlContent.ToString();
        }
    }
}
