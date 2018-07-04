using System.Text;

namespace AspFromScratch.Controllers {
    class UsersController {
        [HttpMethod("GET")]
        public string Index() {
            var users = new string[] {
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
                "Bunnyarmynow"
            };

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
                .AppendLine("   <table>")
                .AppendLine("       <tr><td>ID</td><td>Name</td></tr>");
            for (int i = 0; i < users.Length; ++i) {
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
