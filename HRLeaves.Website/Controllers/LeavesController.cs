using HRLeaves.Services.Data;
using HRLeaves.Services.Logic;
using HRLeaves.Website.Models.Leaves;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Mime;

namespace HRLeaves.Website.Controllers.Leaves
{
    public class LeavesController :Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LeavesController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            ILeavesDataAccess dataAccess = new DataAccessDB("Server=servername;Database=databasename;User Id= theuserid;Password=thepassword");
            LeavesService service = new LeavesService(dataAccess);
            List<Leave> leavesList = service.GetAllLeavesRequests();
            LeavesRequests model = new LeavesRequests();

            foreach (var employeeLeave in leavesList)
            {
                model.LeavesList.Add(new LeavesRecord()
                {
                    LeavesID=employeeLeave.ID,
                    EmployeeName= employeeLeave.EmployeeName,
                    LeaveType=employeeLeave.offType.ToString(),
                    Reason=employeeLeave.Type.ToString()
                });
            }
            
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateEmployeeLeave model = new CreateEmployeeLeave();
            model.OffTypeSelected = "DaysOff";
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeLeave reqLeaveModel)
        {
            ILeavesDataAccess dataAccess = new DataAccessDB("Server=(localDB)\\MSSQLLocalDB;Database=LeavesManagement;User Id=ITG;Password=123456");
            LeavesService service = new LeavesService(dataAccess);

            /*dbLeave.StartDate = reqLeaveModel.StartDate.ToShortDateString();
            dbLeave.EndDate = reqLeaveModel.EndDate.ToShortDateString();
            dbLeave.StartTime = reqLeaveModel.StartTime.ToString(@"hh\:mm");
            dbLeave.EndTime = reqLeaveModel.EndTime.ToString(@"hh\:mm");*/

            //string uniqueFileName=null;
            string filePath=null;
            if (reqLeaveModel.selectedType == "SickLeave")
            {               
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "EmployeesFiles");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + reqLeaveModel.sickFile.FileName;
                filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //var fs = new FileStream(filePath, FileMode.Create);
                //File(fs, "application/vnd.ms-word", "documentfile.doc");
                reqLeaveModel.sickFile.CopyTo(new FileStream(filePath, FileMode.Create));                
            }
            
            Leave dbLeave =new Leave();           
            dbLeave.EmployeeName = reqLeaveModel.Name;
            dbLeave.Type = (LeavesTypes)Enum.Parse(typeof(LeavesTypes), reqLeaveModel.selectedType);
            dbLeave.offType = (OffTypes)Enum.Parse(typeof(OffTypes), reqLeaveModel.OffTypeSelected);
            if (reqLeaveModel.OffTypeSelected == "DaysOff")
            {
                dbLeave.StartDate = reqLeaveModel.StartDate;
                dbLeave.EndDate = reqLeaveModel.EndDate;
            }
            else
            {
                dbLeave.Day = reqLeaveModel.Day;
                dbLeave.StartTime = reqLeaveModel.StartTime;
                dbLeave.numberOfHours = reqLeaveModel.numberOfHours;
            }
            dbLeave.note = reqLeaveModel.note;
            dbLeave.FilePath = filePath;

            service.Create(dbLeave); 
            return View();
        }
        
        [HttpGet]
        public IActionResult Confirm(int leaveReqID)
        {
            ILeavesDataAccess dataAccess = new DataAccessDB("Server=(localDB)\\MSSQLLocalDB;Database=LeavesManagement;User Id=ITG;Password=123456");
            LeavesService service = new LeavesService(dataAccess);
            Leave employeeLeave = service.GetEmployeeLeaveByID(leaveReqID);
            ConfirmLeave model = new ConfirmLeave();
            model.LeavesID = leaveReqID;
            model.Name = employeeLeave.EmployeeName;
            model.LeaveReason = employeeLeave.Type.ToString();
            if (model.LeaveReason == "SickLeave")
            {
                model.filePath = employeeLeave.FilePath;
            }
            model.OffType = employeeLeave.offType.ToString();
            if (model.OffType == "DaysOff")
            {
                model.StartDate = employeeLeave.StartDate;
                model.EndDate = employeeLeave.EndDate;
            }
            else if (model.OffType == "HoursOff")
            {
                model.Day = employeeLeave.Day;
                model.StartTime = employeeLeave.StartTime;
                model.numberOfHours = employeeLeave.numberOfHours;
            }
            model.note = employeeLeave.note;
            
            return View(model);
        }


        [HttpPost]
        public IActionResult Confirm(ConfirmLeave model)
        {
            ILeavesDataAccess dataAccess = new DataAccessDB("Server=(localDB)\\MSSQLLocalDB;Database=LeavesManagement;User Id=ITG;Password=123456");
            LeavesService service = new LeavesService(dataAccess);
            Leave confirmedLeave=new Leave();
            confirmedLeave.ID = model.LeavesID;
            confirmedLeave.Status = model.status;                        
            service.Edit(confirmedLeave);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult View22()
        {
            return View();
        }
    }
}
