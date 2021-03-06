﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iTrade.Models
{
    public class PoKivOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "KIV Order ID")]
        public int KorID { get; set; }

        [Display(Name = "KIV Order#")]
        public string KorNo { get; set; }

        [Display(Name = "Type")]
        [StringLength(15)]
        public string InvType { get; set; }

        [Display(Name = "Invoice Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InvDate { get; set; }

        [Display(Name = "P/O Reference")]
        [StringLength(25)]
        public string PoNo { get; set; }

        [Display(Name = "INV Reference")]
        [StringLength(25)]
        public string InvRef { get; set; }

        [Required]
        public int CustNo { get; set; }

        [Required]
        [Display(Name = "Company")]
        [StringLength(60, ErrorMessage = "Company name can not be empty.", MinimumLength = 1)]
        public string CustName { get; set; }

        [StringLength(60)]
        public string CustName2 { get; set; }

        [Display(Name = "Address")]
        [StringLength(100)]
        public string Addr1 { get; set; }

        [Display(Name = " ")]
        [StringLength(100)]
        public string Addr2 { get; set; }

        [Display(Name = " ")]
        [StringLength(100)]
        public string Addr3 { get; set; }

        [Display(Name = " ")]
        [StringLength(100)]
        public string Addr4 { get; set; }

        [Display(Name = "Attn")]
        [StringLength(30)]
        public string Attn { get; set; }

        [Display(Name = "Phone")]
        [StringLength(20)]
        public string PhoneNo { get; set; }

        [Display(Name = "Fax")]
        [StringLength(20)]
        public string FaxNo { get; set; }

        [Display(Name = "Delivery Address")]
        [StringLength(100)]
        public string DeliveryAddress { get; set; }

        [Display(Name = "Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> DeliveryDate { get; set; }

        [Display(Name = "Time")]
        [StringLength(20)]
        public string DeliveryTime { get; set; }

        //[Display(Name = "Pre Discount Amount")]
        //public decimal PreDiscAmount { get; set; }

        //public decimal Discount { get; set; }

        //public decimal Amount { get; set; }

        //public decimal Gst { get; set; }

        //[Display(Name = "Total Amount")]
        //public decimal Nett { get; set; }

        //[Display(Name = "Paid Amount")]
        //public decimal PaidAmount { get; set; }

        //[Display(Name = "Is Paid?")]
        //public Boolean IsPaid { get; set; }

        //[Display(Name = "Paid On")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //public Nullable<DateTime> PaidDate { get; set; }

        [Display(Name = "Terms")]
        [StringLength(100)]
        public string PaymentTerms { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [Display(Name = "Location ID")]
        public int LocationID { get; set; }

        [Display(Name = "Location")]
        [StringLength(60)]
        public string LocationName { get; set; }

        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }

        public int PersonID { get; set; }

        [Display(Name = "Sales Person")]
        [StringLength(60)]
        public string PersonName { get; set; }

        [Display(Name = "Created By")]
        [StringLength(60)]
        public string CreatedBy { get; set; }

        [Display(Name = "Created On")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Last Modified")]
        [StringLength(60)]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified On")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> ModifiedOn { get; set; }

    }
}