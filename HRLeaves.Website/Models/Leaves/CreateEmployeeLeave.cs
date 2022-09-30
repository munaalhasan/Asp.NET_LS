using HRLeaves.Services;
using HRLeaves.Services.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HRLeaves.Website.Models.Leaves
{
    public class CreateEmployeeLeave
    {       
        public string Name { get; set; }       
        public LeavesTypes Types { get; set; }        
        public string selectedType { get; set; }
        
        public OffTypes OffTypes { get; set; }
        public string OffTypeSelected { get; set; }
        public IFormFile sickFile { get; set; }

        [Required]
        [DataType(DataType.Date)]
        //[Display(Name = "Start date"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }       

        
        [Required]
        [DataType(DataType.Date)]
        //[Display(Name = "End date"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime EndDate { get; set; }

        [Required]
        [DataType(DataType.Date)]      
        public DateTime Day { get; set; }


        [Required]
        [DataType(DataType.Time)]
        //[Display(Name = "Start time"), DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }
        

        public int numberOfHours { get; set; }

        public string note { get; set; }

    }
}
