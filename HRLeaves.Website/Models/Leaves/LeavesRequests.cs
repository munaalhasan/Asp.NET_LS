using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRLeaves.Website.Models.Leaves
{
    public class LeavesRequests
    {
        public List<LeavesRecord> LeavesList;

        public LeavesRequests()
        {
            LeavesList = new List<LeavesRecord>();               
        }
    }
}
