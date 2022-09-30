using HRLeaves.Services.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRLeaves.Services.Data
{
    public interface ILeavesDataAccess
    {
        public void CreateLeave(Leave leave);
        public void EditLeave(Leave leave);
        public List<Leave> GetAllEmployeesLeavesRequests();
        public Leave GetEmployeeLeave(int empLeaveID);
        public string GetEmployeeLeaveFile(int empLeaveID);
    }
}
