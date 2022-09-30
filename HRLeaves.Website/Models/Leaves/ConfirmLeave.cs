using HRLeaves.Services.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRLeaves.Website.Models.Leaves
{
    public class ConfirmLeave
    {
        public int LeavesID { get; set; }
        public string Name { get; set; }
        public string LeaveReason { get; set; }
        public string filePath { get; set; }

        public string OffType { get; set; }

        
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }


        [DataType(DataType.Date)]
        public DateTime Day { get; set; }


        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        
        public int numberOfHours { get; set; }

        public string note { get; set; }

        public LeavesStatus status { get; set; }
    }
}
