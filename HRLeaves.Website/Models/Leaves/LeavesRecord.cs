using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRLeaves.Website.Models.Leaves
{
    public class LeavesRecord
    {
        public int LeavesID { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }
        public string Reason { get; set; }
    }
}
