using HRLeaves.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRLeaves.Services.Logic
{
    public class LeavesService
    {
        ILeavesDataAccess dataAccess;
        public LeavesService(ILeavesDataAccess data)
        {
            dataAccess = data;
        }
        public List<Leave> GetAllLeavesRequests()
        {
            return dataAccess.GetAllEmployeesLeavesRequests();
        }
        public void Create(Leave newRequest)
        {
            dataAccess.CreateLeave(newRequest);
        }
        public void Edit(Leave confirmedLeave)
        {
            dataAccess.EditLeave(confirmedLeave);
        }
        public Leave GetEmployeeLeaveByID(int empLeave)
        {
            return dataAccess.GetEmployeeLeave(empLeave);
        }
        
        public string GetEmployeeLeaveFileByID(int empLeave)
        {
            return dataAccess.GetEmployeeLeaveFile(empLeave);
        }
    }
}
