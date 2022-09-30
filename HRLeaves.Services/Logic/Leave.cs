using System;
using System.Collections.Generic;
using System.Text;

namespace HRLeaves.Services.Logic
{
    public class Leave
    {
        public int ID;
        public string EmployeeName;
        public LeavesTypes Type;
        public DateTime StartDate;
        public DateTime EndDate;
        public DateTime Day;
        public DateTime StartTime;
        public int numberOfHours;
        public string note;
        public string FilePath;
        public LeavesStatus Status;
        public OffTypes offType;
        public Leave()
        {

        }
    }
}
