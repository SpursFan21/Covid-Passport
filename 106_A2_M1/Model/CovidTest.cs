using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.Model
{
    public class CovidTest
    {
        public string test_id { get; set; }
        public long test_date { get; set; }
        public int result { get; set; }
        public string test_type { get; set; }
        public string formatted_test_date { get; set; } // Added for displaying date
        public string formatted_iso_date { get; set; } = "No Isolation Required"; // Added for displaying iso date
    }
}
