using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.Model
{
    public class Issue
    {
        public string issue_id { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public bool resolve
        {
            get
            {
                return closed_date > open_date;
            }
            set
            {
                resolve = value;
            }
        }
        public int open_date { get; set; }
        public int closed_date { get; set; }
    }
}
