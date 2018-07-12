using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspFromScratch.Services {
    public interface IDataService {
        IEnumerable<string> GetNames();
        void AddName(string name);
    }
}
