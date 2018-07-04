using System;


namespace AspFromScratch {
    class Program {
        static void Main(string[] args) {
            var server = new WebServer().On("http://localhost:40001/").Start();
            while (true) {
                var input = Console.ReadLine();
                if (input.ToUpper().Equals("EXIT")) {
                    break;
                }
            }
        }
    }
}
