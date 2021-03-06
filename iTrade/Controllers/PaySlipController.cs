﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using iTrade.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace iTrade.Controllers
{
    public class PaySlipController : Controller
    {
        private StarDbContext db = new StarDbContext();
        // GET: PaySlip
        public ActionResult Index(string txtFilter)
        {
            var result = db.PaySlips.Take(200).ToList();

            if (!string.IsNullOrEmpty(txtFilter))
            {
                result = db.PaySlips.Where(x => x.TutorName.Contains(txtFilter) || x.TutorCode.StartsWith(txtFilter)).Take(200).ToList();
            }

            return View(result);
        }

        public ActionResult Create()
        {
            PaySlip stu = new PaySlip();
            ViewData["TutorAll"] = db.Tutors.Where(x => x.IsActive == true).ToList();
            return View(stu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PaySlip det)
        {
            if (ModelState.IsValid)
            {
                db.PaySlips.Add(det);
                db.SaveChanges();

                var p = det.PaySlipID;
                var status = "Closed";
                var classAttendance = new List<ClassAttendance>();
                classAttendance = db.ClassAttendances.Where(
                    m => m.TutorID == det.TutorID &&
                    m.Status == status &&
                    m.AttendDate >= det.PaymentDate &&
                    m.AttendDate <= det.PaymentDate2).ToList();

                PaySlipDetail paySlipDetail = new PaySlipDetail();

                foreach (var item in classAttendance)
                {
                    paySlipDetail.Date = Convert.ToString(item.AttendDate.Day) + '/' + Convert.ToString(item.AttendDate.Month) + '/' + Convert.ToString(item.AttendDate.Year);
                    paySlipDetail.ClassType = db.ClassSchedules.Where(m => m.ScheduleID == item.ScheduleID).FirstOrDefault().ScheduleType;
                    paySlipDetail.ClassCode = db.Pricebooks.Where(m => m.PriceID == item.PriceID).FirstOrDefault().CourseCode;
                    paySlipDetail.ClassDesc = item.CourseName;
                    paySlipDetail.StartTime = item.StartTimeValue;
                    paySlipDetail.EndTime = item.EndTimeValue;
                    paySlipDetail.TutorID = item.TutorID;
                    paySlipDetail.PaySlipID = det.PaySlipID;
                    paySlipDetail.PriceID = item.PriceID;

                    var qty = db.ClassAttendees.Where(m => m.AttendID == item.AttendID).ToList();
                    paySlipDetail.Quantity = qty.Count;

                    var diff =Convert.ToDateTime(item.EndTimeValue) - Convert.ToDateTime(item.StartTimeValue);
                    paySlipDetail.StudyHour = Convert.ToDouble(diff.Hours)+ Convert.ToDouble(diff.Minutes)/60;
                    try
                    {
                        paySlipDetail.HourlyRate = db.TutorRates.Where(
                            m => m.TutorID == item.TutorID &&
                            m.PriceID == item.PriceID &&
                            m.ClassType == paySlipDetail.ClassType &&
                            m.MinAttend <= paySlipDetail.Quantity &&
                            m.MaxAttend >= paySlipDetail.Quantity
                            ).FirstOrDefault().Rate;
                    }
                    catch (Exception ex)
                    {
                        paySlipDetail.HourlyRate = 0;
                    }
                    paySlipDetail.Amount = paySlipDetail.HourlyRate * paySlipDetail.StudyHour;

                    det.Total += paySlipDetail.Amount;

                    db.PaySlipDetails.Add(paySlipDetail);
                    db.SaveChanges();
                }
                return RedirectToAction("Edit/" + p);
            }

            return View(det);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaySlip stu = db.PaySlips.Find(id);
            if (stu == null)
            {
                return HttpNotFound();
            }

            ViewData["TutorAll"] = db.Tutors.Where(x => x.IsActive == true).ToList();
            return View(stu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PaySlip ps)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ps).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Edit/" + ps.PaySlipID);
                }
                catch (Exception ex){
                }
            }

            return View(ps);
        }

        public ActionResult Detail(int id)
        {
            var p = new List<PaySlipDetail>();
            p = db.PaySlipDetails.Where(x => x.PaySlipID == id).OrderBy(x =>x.Date).ToList();

            ViewData["PriceBookAll"] = db.Pricebooks.ToList();
            return PartialView(p);
        }

        public ActionResult Item(int id)
        {
            PaySlip ps = db.PaySlips.Find(id);
            PaySlipDetail p = new PaySlipDetail();
            p.PaySlipID = ps.PaySlipID;
            p.TutorID = ps.TutorID;

            ViewData["PriceBookAll"] = db.Pricebooks.ToList();
            return PartialView(p);
        }

        public void AddItem(PaySlipDetail det)
        {           
            PaySlip paySlip = db.PaySlips.Where(m => m.PaySlipID == det.PaySlipID).FirstOrDefault();
            TutorRate tut = db.TutorRates.Where(m => m.PriceID == det.PriceID).FirstOrDefault();

            det.ClassCode = tut.CourseCode;
            det.ClassDesc = tut.CourseName;

            paySlip.Total += det.Amount;

            db.PaySlipDetails.Add(det);
            int x = db.SaveChanges();
            int p = det.PaySlipID;

            if (x != 0)
            {
                Response.Redirect("Edit/" + p);
            }

        }

        public JsonResult AutoCompleteSelected(string search)
        {
            int tuID = Convert.ToInt32(search);
            if (search != null)
            {
                var c = db.Tutors
                           .Where(x => x.TutorID == tuID)
                           .ToList().FirstOrDefault();

                if (c != null)
                {

                    return Json(new { result = c }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                };

            }
            else
            {
                return null;
            }

        }

        public JsonResult AutoSelect(int priceID, double quantity)
        {
            if (priceID != 0 && quantity > 0)
            {
                var c = db.TutorRates.Where(x => x.PriceID == priceID && x.MinAttend <= quantity && x.MaxAttend >=quantity).FirstOrDefault();

                if (c != null)
                {

                    return Json(new { result = c }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                };

            }
            else
            {
                var m = db.TutorRates.Where(x => x.PriceID == priceID).FirstOrDefault();               

                if (m != null)
                {
                    m.Rate = 0;
                    return Json(new { result = m }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }

        }

        public JsonResult AutoHour(string starttime, string endtime)
        {
            if (starttime != null && endtime != null )
            {
                var hour = Convert.ToDateTime(endtime) - Convert.ToDateTime(starttime);

                if (hour != null)
                {
                    return Json(new { result = hour.Hours }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                };

            }
            else
            {
                return null;
            }

        }

        public ActionResult DeleteConfirmed(int id)
        {
            var det = db.PaySlipDetails.Find(id);
            var newdet = db.PaySlips.Find(det.PaySlipID);

            if (det != null)
            {
                db.Entry(det).State = EntityState.Deleted;
                newdet.Total -= det.Amount;
                db.SaveChanges();
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPaySlip(int id)
        {
            string userName = User.Identity.Name;
            LocalReport localReport = new LocalReport();

            localReport.ReportPath = @"ReportTemplate/rptPaySlip.rdlc";
            localReport.EnableExternalImages = true;

            var payslip = new List<PaySlip>();
            var payslipdet = new List<PaySlipDetail>();

            payslip = db.PaySlips.Where(m => m.PaySlipID == id).ToList();
            payslipdet = db.PaySlipDetails.Where(m => m.PaySlipID == id).ToList();
        
            ReportDataSource rdsPaySlip = new ReportDataSource("DataSet1", payslip);
            ReportDataSource rdsPaySlipDet = new ReportDataSource("DataSet2", payslipdet);

            localReport.DataSources.Add(rdsPaySlip);
            localReport.DataSources.Add(rdsPaySlipDet);

            localReport.Refresh();

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension = "pdf";

            string deviceInfo = @"<DeviceInfo>
                        <OutputFormat>PDF</OutputFormat>
                       <PageWidth>8.27in</PageWidth>
                        <PageHeight>11.69in</PageHeight>
                        <MarginTop>0.1in</MarginTop>
                        <MarginLeft>0.1in</MarginLeft>
                        <MarginRight>0in</MarginRight>
                        <MarginBottom>0in</MarginBottom>
                    </DeviceInfo>";

            Warning[] warnings;

            string[] streams;

            byte[] renderedBytes;

            renderedBytes = localReport.Render(
                            reportType,
                            deviceInfo,
                            out mimeType,
                            out encoding,
                            out fileNameExtension,
                            out streams,
                            out warnings);

            var doc = new Document();
            var reader = new PdfReader(renderedBytes);

            using (FileStream fs = new FileStream(Server.MapPath("~/Reports/docInvoice.pdf"), FileMode.Create))
            {
                PdfStamper stamper = new PdfStamper(reader, fs);
                string Printer = "";
                if (Printer == null || Printer == "")
                {
                    stamper.JavaScript = "var pp = getPrintParams();pp.interactive = pp.constants.interactionLevel.automatic;pp.printerName = getPrintParams().printerName;print(pp);\r";
                    stamper.Close();
                }
                else
                {
                    stamper.JavaScript = "var pp = getPrintParams();pp.interactive = pp.constants.interactionLevel.automatic;pp.printerName = " + Printer + ";print(pp);\r";
                    stamper.Close();
                }
            }

            reader.Close();

            FileStream fss = new FileStream(Server.MapPath("~/Reports/docInvoice.pdf"), FileMode.Open);
            byte[] bytes = new byte[fss.Length];
            fss.Read(bytes, 0, Convert.ToInt32(fss.Length));
            fss.Close();
            System.IO.File.Delete(Server.MapPath("~/Reports/docInvoice.pdf"));
            return File(bytes, "application/pdf");
        }

    }
}