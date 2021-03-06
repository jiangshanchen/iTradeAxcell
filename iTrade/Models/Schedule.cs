﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using iTrade.CustomAttributes;

namespace iTrade.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }

        [Display(Name = "CourseID")]
        public int CourseID { get; set; }

        [Display(Name = "Course Code")]
        [StringLength(40)]
        public string CourseCode { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        [StringLength(100, ErrorMessage = "Course name can not be empty.", MinimumLength = 1)]
        public string CourseName { get; set; }

        [Display(Name = "Type")]
        [StringLength(30)]
        public string ScheduleType { get; set; }

        [Display(Name = "Title")]
        [StringLength(30)]
        public string ScheduleTitle { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ScheduleDate { get; set; }

        public DateTime? StartTime { get; set; }

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)$", ErrorMessage = "Invalid Time.")]
        public string StartTimeValue
        {
            get
            {
                return StartTime.HasValue ? StartTime.Value.ToString("hh:mm tt") : string.Empty;
            }

            set
            {
                StartTime = DateTime.Parse(value);
            }
        }

        public DateTime? EndTime { get; set; }

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)$", ErrorMessage = "Invalid Time.")]
        public string EndTimeValue
        {
            get
            {
                return EndTime.HasValue ? EndTime.Value.ToString("hh:mm tt") : string.Empty;
            }

            set
            {
                EndTime = DateTime.Parse(value);
            }
        }

        [StringLength(30)]
        public string Status { get; set; }

        [StringLength(100)]
        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }

        [StringLength(100)]
        [DataType(DataType.MultilineText)]
        public string Remark2 { get; set; }

        [Display(Name = "Created By")]
        [StringLength(40)]
        public string CreatedBy { get; set; }

        [Display(Name = "Created On")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Last Modified")]
        [StringLength(40)]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modibied On")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> ModifiedOn { get; set; }


    }
}