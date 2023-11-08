using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.Model
{
    public class Vaccine
    {
        public string dose_id { get; set; }
        public int date_administered { get; set; }
        public string brand { get; set; }
        public string location { get; set; }
    }
}
