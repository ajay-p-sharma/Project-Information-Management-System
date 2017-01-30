using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using ProjectInformationManagementSystem.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ProjectInformationManagementSystem.Controllers
{
    public class TrackerController : Controller
    {

        private pimsEntitiesNew db = new pimsEntitiesNew();

        public object SqlMethods { get; private set; }

        /// <summary>
        /// Method to show page with project data tracking options.
        /// </summary>
        public ActionResult Tracker()
        {
            return View();
        }

        /// <summary>
        /// Method to show page with averag project turnaround times for different project service request types.
        /// </summary>
        [HttpGet]
        public PartialViewResult TurnAround()
        {

            pimsEntitiesNew db = new pimsEntitiesNew();
            var queryExtraction = from d in db.new_project
                        .Where(d => d.service_requested == "Extraction" &&
                           d.date_completed.HasValue && d.date_completed.Value.Year != 0001)
                                  select d;
            int extractionSum = 0;
            int extractionCount = 0;
            foreach (new_project p in queryExtraction)
            {
                DateTime start = p.date_submitted;
                DateTime finish = p.date_completed.Value;
                TimeSpan difference = finish - start;

                int diff = difference.Days;

                extractionSum += diff;
                extractionCount += 1;
            }
            if (extractionCount == 0)
            {
                extractionCount = 1;
            }
            double extractionAverage = extractionSum / extractionCount;
            Session["averageExtractionTAT"] = extractionAverage;


            var queryLibPrep = from d in db.new_project
                        .Where(d => d.service_requested == "Library Preparation" &&
                           d.date_completed.HasValue && d.date_completed.Value.Year != 0001)
                               select d;
            int LibSum = 0;
            int Libcount = 0;
            foreach (new_project p in queryLibPrep)
            {
                DateTime start = p.date_submitted;
                DateTime finish = p.date_completed.Value;
                TimeSpan difference = finish - start;

                int diff = difference.Days;

                LibSum += diff;
                Libcount += 1;
            }

            double LibAverage = LibSum / Libcount;
            Session["LibraryPrepTAT"] = LibAverage;




            var queryCapture = from d in db.new_project
                        .Where(d => d.service_requested == "Library Capture" &&
                           d.date_completed.HasValue && d.date_completed.Value.Year != 0001)
                               select d;
            int captureSum = 0;
            int captureCount = 0;
            foreach (new_project p in queryCapture)
            {
                DateTime start = p.date_submitted;
                DateTime finish = p.date_completed.Value;
                TimeSpan difference = finish - start;

                int diff = difference.Days;

                captureSum += diff;
                captureCount += 1;
            }
            if (captureCount == 0)
            {
                captureCount = 1;
            }

            double captureAverage = captureSum / captureCount;
            Session["captureTAT"] = captureAverage;



            var querySeq = from d in db.new_project
                       .Where(d => d.service_requested == "Sequencing" &&
                          d.date_completed.HasValue && d.date_completed.Value.Year != 0001)
                           select d;
            int seqSum = 0;
            int seqCount = 0;
            foreach (new_project p in querySeq)
            {
                DateTime start = p.date_submitted;
                DateTime finish = p.date_completed.Value;
                TimeSpan difference = finish - start;

                int diff = difference.Days;

                seqSum += diff;
                seqCount += 1;
            }
            if (seqCount == 0)
            {
                seqCount = 1;
            }
            double seqAverage = seqSum / seqCount;
            Session["sequencingTAT"] = seqAverage;

            return PartialView("_TurnAroundTimes");
        }

        /// <summary>
        /// Method to show create partial view of all the extraction projects data.
        /// </summary>
        public PartialViewResult extractionTrack()
        {
            pimsEntitiesNew dc = new pimsEntitiesNew();
            
                // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
                var data = dc.extraction_data.ToList();


                return PartialView("_extractionTracking",data);
            
        }

        /// <summary>
        /// Method to show create partial view of all the Library Prep projects data.
        /// </summary>
        public PartialViewResult LibraryPrepTrack()
        {
            pimsEntitiesNew dc = new pimsEntitiesNew();

            // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            var data = dc.library_prep.ToList();


            return PartialView("_LibraryPrepTrack", data);

        }


        /// <summary>
        /// Method to show create partial view of all the Capture projects data.
        /// </summary>
        public PartialViewResult captureTrack()
        {
            pimsEntitiesNew dc = new pimsEntitiesNew();

            // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            var data = dc.capture_data.ToList();


            return PartialView("_captureTrack", data);

        }


        /// <summary>
        /// Method to show create partial view of all the Sequencing projects data.
        /// </summary>
        public PartialViewResult sequencingTrack()
        {
            pimsEntitiesNew dc = new pimsEntitiesNew();

            // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            var data = dc.sequencing_data.ToList();


            return PartialView("_sequencingTrack", data);

        }

        /// <summary>
        /// Method to show view with detailed information about projects between two dates entered by the user..
        /// </summary>
        public ActionResult GenerateReport()
        {
            return View();
        }

        public PartialViewResult Report(FormCollection fc)
        {
          

            DateTime startDate = Convert.ToDateTime(fc["DateBegin"]);

            DateTime endDate = Convert.ToDateTime(fc["DateEnd"]);

            if(endDate <= startDate)
            {
                ViewBag.Error = "End Date must be higher than Begin Date";
                return PartialView("_Report");
            }

            App_Start.Helper.Report report = new App_Start.Helper.Report();
          
            var newProjecstData = from x in db.new_project
                                  where x.date_submitted >= startDate
                                  where x.date_submitted <= endDate
                                  orderby x.project_id ascending
                                  select x;

            report.numberOfProjects = newProjecstData.Count();



            var numExtractionProjects = from x in db.new_project
                                        where x.date_submitted >= startDate
                                        where x.date_submitted <= endDate
                                        where x.service_requested.ToLower()=="extraction"
                                        orderby x.project_id ascending
                                        select x.project_id;

            report.numberOfExtractionProjects = numExtractionProjects.Count();

            var numLibPrepProjects = from x in db.new_project
                                     where x.date_submitted >= startDate
                                     where x.date_submitted <= endDate
                                     where x.service_requested.ToLower() == "library preparation"
                                        orderby x.project_id ascending
                                        select x.project_id;

           report.numberOfLibraryPrepProjects = numLibPrepProjects.Count();

            var numCaptureProjects = from x in db.new_project
                                        where x.date_submitted >= startDate
                                        where x.date_submitted <= endDate
                                        where x.service_requested.ToLower() == "library capture"
                                        orderby x.project_id ascending
                                        select x.project_id;

            report.numberOfCaptureProjects = numCaptureProjects.Count();

            var numSequencingProjects = from x in db.new_project
                                        where x.date_submitted >= startDate
                                        where x.date_submitted <= endDate
                                        where x.service_requested.ToLower() == "sequencing"
                                        orderby x.project_id ascending
                                        select x.project_id;

            report.numberOfSequencingProjects = numSequencingProjects.Count();

            var projectedTotalcostData = db.new_project.Where(a=> a.date_submitted >= startDate &&
                                                                     a.date_submitted <= endDate).Sum(a=> (double?)a.service_cost) ?? 0;
                                         
           
                report.totalCost = projectedTotalcostData;
            

            var totalRecoveredCostData = db.new_project.Where(b => b.date_submitted >= startDate &&
                                                                    b.date_submitted<=endDate &&
                                                                    b.current_status.ToLower()== "complete").Sum(b => (double?)b.service_cost) ?? 0;
             
            report.totalCostCharged = totalRecoveredCostData;
           

            var totalPendingCharesData = db.new_project.Where(c => c.date_submitted >= startDate &&
                                                                    c.date_submitted <= endDate &&
                                                                    c.current_status.ToLower() != "complete").Sum(c => (double?)c.service_cost) ?? 0;


          
            report.totalCostPending = totalPendingCharesData;
            

            TempData["startDate"] = startDate;
            TempData["endDate"] = endDate;

            ViewBag.Report = report;

            return PartialView("_Report");
        }

    }
}