using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspFromScratch.Services {
    public class FakeDataService : IDataService {
        static List<string> names = new List<string> {
                "Alex",
                "Laman",
                "Tural",
                "Ramin",
                "Fuad",
                "Nadir",
                "Ismayil",
                "Bunnyaminarmynow",
                "Iqor",
                "Mirza",
                "Namiq",
                "Tabriz",
                "Mensim",
                "Samir"
            };

        public void AddName(string name) {
            names.Add(name);
        }

        public IEnumerable<string> GetNames() {
            return names;
        }
    }
}
