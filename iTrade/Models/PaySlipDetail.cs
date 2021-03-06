﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iTrade.Models
{
    public class PaySlipDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "No")]
        public int PaySlipDetailID { get; set; }

        [Display(Name = "PaySlip ID")]
        public int PaySlipID { get; set; }

        [Display(Name = "Tutor ID")]
        public int TutorID { get; set; }

        [Display(Name = "Date")]
        public string Date { get; set; }

        [Display(Name = "Start Time")]
        [StringLength(25)]
        public string StartTime { get; set; }

        [Display(Name = "End Time")]
        [StringLength(25)]
        public string EndTime { get; set; }

        [Display(Name = "Hour")]
        public double StudyHour { get; set; }

        [Display(Name = "Hourly Rate")]
        public double HourlyRate { get; set; }

        [Display(Name = "Student Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Amount")]
        public double Amount { get; set; }

        public int PriceID { get; set; }

        public string ClassCode { get; set; }

        [Display(Name = "Class Desc")]
        [StringLength(25)]
        public string ClassDesc { get; set; }

        [Display(Name = "ClassType")]
        [StringLength(25)]
        public string ClassType { get; set; }

    }
}