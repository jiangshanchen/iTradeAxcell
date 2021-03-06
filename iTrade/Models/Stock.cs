﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iTrade.Models
{
    public class Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "No")]
        public int StockID { get; set; }

        [Display(Name = "Product ID")]
        public int ProductID { get; set; }

        [Display(Name = "SKU")]
        [StringLength(60)]
        public string SKU { get; set; }

        [Display(Name = "Product Name")]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Display(Name = "Barcode")]
        [StringLength(48)]
        public string Barcode { get; set; }

        [Display(Name = "Initial Qty")]
        public double InitQty { get; set; }

        [Display(Name = "In")]
        public double StockIn { get; set; }

        [Display(Name = "Out")]
        public double StockOut { get; set; }

        [Display(Name = "Stock Adjusted")]
        public double StockAdjusted { get; set; }

        [Display(Name = "In-Stock")]
        public double InStock { get; set; }

        [Display(Name = "Allocated")]
        public double Allocated { get; set; }

        [Display(Name = "On Hand")]
        public double OnHand { get; set; }

        [Display(Name = "Incoming")]
        public double OnOrder { get; set; }

        [Display(Name = "KIV")]
        public double OnKiv { get; set; }

        [Display(Name = "Modibied On")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ModifiedOn { get; set; }


    }
}