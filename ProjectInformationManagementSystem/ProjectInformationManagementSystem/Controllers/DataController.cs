using ProjectInformationManagementSystem.App_Start.Helper;
using ProjectInformationManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectInformationManagementSystem.Controllers
{
    public class DataController : Controller
    {
        private pimsEntitiesNew db = new pimsEntitiesNew();



        /// <summary>
        /// Returns the View with data entry success or error messages.
        /// </summary>
        public ActionResult DataResults()
        {
            
            return View();
        }


        /// <summary>
        /// Method to retun status of a project. This method is used in another ActionResult method.
        /// </summary>
        public string projectStatus(int i)
        {
            var currentStatus = (from project in db.new_project
                               where project.project_id == i
                               select project.current_status).ToArray();

            string status = currentStatus[0].ToLower();
            //string projectStatus = db.new_project.Where(p => p.project_id == i).Select(p => p.current_status).ToString();
            return status;
        }

        /// <summary>
        /// Method to display page with summary of projects with number of samples and option to add data.
        /// </summary>
        [HttpGet]
        public ActionResult Summary()
        {
            IEnumerable<int> projects = db.sample_detail.Select(p => p.project_id).Distinct();

            
            List<SamplesSummary> samplelist = new List<SamplesSummary>();
            foreach(int  i in projects.ToList())
            {
                var projStatus = projectStatus(i);
                if (projStatus !="complete")
                {
                    SamplesSummary sampDet = new SamplesSummary();
                    sampDet.projectNum = i;
                    sampDet.numberOfSamples = db.sample_detail.Where(p => p.project_id == i).Count();
                    samplelist.Add(sampDet);
                }
            }

            ViewBag.Counts = samplelist.ToList().OrderByDescending(a=> a.projectNum);
           
            ViewBag.Results = projects.ToList();
            return View();
        }


        /// <summary>
        /// Method to display page with form to add data for extraction project.
        /// </summary>
        [HttpGet]
        public ActionResult EnterExtractionData()
        {

            int count = Convert.ToInt32(TempData["count"]);
            ViewBag.totalCount = count;
            return View();
        }


        /// <summary>
        /// Method to post data for extraction project.
        /// </summary>
        /// <param name="extraction_data ed">List of "extraction_data" objects is passed as parameter.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnterExtractionData(List<extraction_data> ed)
        {
            var testData = ed.ToList();

            var proj = testData[0].project_id;

            var testAgainst = (from p in db.extraction_data
                              where p.project_id == proj
                              select p.project_id).ToArray();

            if (testAgainst.Count() != 0)
            {
                
                    TempData["Error"] = "Data for this project already exist. Duplicate entries are not allowed!";
            }

            else{
                try
                {
                    foreach (extraction_data i in ed)
                        if (ModelState.IsValid)
                        {
                            db.extraction_data.Add(i);
                        }
                    db.SaveChanges();
                    ModelState.Clear();

                    TempData["success"] = "Extraction Data sucessfully saved!";

                }
                catch (Exception e)
                {
                    TempData["Error"] = "Error occured:\n* All fields are required!\n* All fields must have correct data types!";

                }

            }
            return RedirectToAction("DataResults", "Data");
        }


        /// <summary>
        /// Method to display page with form to add data for library_prep project.
        /// </summary>
        [HttpGet]
        public ActionResult EnterLibraryPrepData()
        {
            int count = Convert.ToInt32(TempData["count"]);
            ViewBag.totalCount = count;
            return View();
        }

        /// <summary>
        /// Method to post data for library_prep project.
        /// </summary>
        /// <param name="library_prep lib">List of "library_prep" objects is passed as parameter.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnterLibraryPrepData(List<library_prep> lib)
        {

            var testData = lib.ToList();

            var proj = testData[0].project_id;

            var testAgainst = (from p in db.library_prep
                              where p.project_id == proj
                              select p.project_id).ToArray();

            if (testAgainst.Count() != 0)
            {

                TempData["Error"] = "Data for this project already exist. Duplicate entries are not allowed!";
            }

            else
            {
                try
                {
                    foreach (library_prep i in lib)
                        if (ModelState.IsValid)
                        {
                            db.library_prep.Add(i);
                        }
                    db.SaveChanges();
                    ModelState.Clear();
                    TempData["success"] = "Library Prep Data sucessfully saved!";
                }

                catch (Exception e)
                {
                    TempData["Error"] = "Error occured: \n* All fields are required! \n* All fields must have correct data types!";
                }
            }

            
            return RedirectToAction("DataResults", "data");
        }


        /// <summary>
        /// Method to display page with form to add data for capture project.
        /// </summary>

        [HttpGet]
        public ActionResult EnterCaptureData()
        {
            int count = Convert.ToInt32(TempData["count"]);
            ViewBag.totalCount = count;
            return View();
        }


        /// <summary>
        /// Method to post data for capture_data project.
        /// </summary>
        /// <param name="capture_data cd">List of "capture_data" objects is passed as parameter.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnterCaptureData(List<capture_data> cd)
        {

            var testData = cd.ToList();

            var proj = testData[0].project_id;

            var testAgainst = (from p in db.capture_data
                              where p.project_id == proj
                              select p.project_id).ToArray();

            if (testAgainst.Count() != 0)
            {

                TempData["Error"] = "Data for this project already exist. Duplicate entries are not allowed!";
            }

            else
            {
                try
                {
                    foreach (capture_data i in cd)
                        if (ModelState.IsValid)
                        {
                            db.capture_data.Add(i);
                        }
                    db.SaveChanges();
                    ModelState.Clear();

                    TempData["success"] = "Capture data sucessfully saved!";
                }
                catch (Exception e)
                {
                    TempData["Error"] = "Error: Please make sure the data types are correct!\n";
                }
            }

           
            return RedirectToAction("DataResults", "data");
        }


        /// <summary>
        /// Method to display page with form to add data for sequencing project.
        /// </summary>
        [HttpGet]
        public ActionResult EnterSequencingData()
        {
            int count = Convert.ToInt32(TempData["count"]);
            ViewBag.totalCount = count;
            return View();
        }

        /// <summary>
        /// Method to post data for sequencing project.
        /// </summary>
        /// <param name="sequencing_data sd">List of "sequencing_data" objects is passed as parameter.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnterSequencingData(List<sequencing_data> sd)
        {
            var testData = sd.ToList();

            var proj = testData[0].project_id;

            var testAgainst = (from p in db.sequencing_data
                              where p.project_id == proj
                              select p.project_id).ToArray();

            if (testAgainst.Count() != 0)
            {

                TempData["Error"] = "Data for this project already exist. Duplicate entries are not allowed!";
            }

            else
            {
                try
                {
                    foreach (sequencing_data i in sd)
                        if (ModelState.IsValid)
                        {
                            db.sequencing_data.Add(i);
                        }
                    db.SaveChanges();
                    ModelState.Clear();
                    TempData["success"] = "Sequencing data sucessfully saved!";
                }
                catch (Exception e)
                {
                    TempData["Error"] = "Error: Please make sure the data types are correct!\n";
                }
            }

            
            return RedirectToAction("DataResults", "data");
        }


        /// <summary>
        /// Method to present view with form to add data for projects. 
        /// Method uses service requested datatype of new_project to present appropriate page with form.
        /// </summary>
        /// <param name="id">Project ID is passed as parameter.</param>
        [HttpGet]
        public ActionResult AddData(int? id)
        {
            List<sample_detail> samples = new List<sample_detail>();
            string request;
            try
            {
                
                if (id == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
               
                if (id != null)
                {
                    foreach (sample_detail i in db.sample_detail )
                        if (i.project_id == id)
                        {
                            samples.Add(i);
                        }
                }

             

               ViewBag.SampleCount = samples.Count();

                var requestType = (from project in db.new_project
                                   where project.project_id == id
                                   select project.service_requested).ToArray();

                request = requestType[0].ToLower();

                if (request == "extraction")
                {
                    TempData["count"] = ViewBag.SampleCount;
                     ViewBag.Samples= samples.ToArray();
                    TempData["samples"] = ViewBag.Samples;
                    return RedirectToAction("EnterExtractionData","Data");
                }


                else if (request.ToLower() == "library preparation")
                {
                    TempData["count"] = ViewBag.SampleCount;
                    ViewBag.Samples = samples.ToArray();
                    TempData["samples"] = ViewBag.Samples;

                    return RedirectToAction("EnterLibraryPrepData", "Data");
                }

                else if (request.ToLower() == "library capture")
                {
                    TempData["count"] = ViewBag.SampleCount;
                    ViewBag.Samples = samples.ToArray();
                    TempData["samples"] = ViewBag.Samples;
                    return RedirectToAction("EnterCaptureData","Data");
                }

                else if (request.ToLower() == "sequencing")
                {
                    TempData["count"] = ViewBag.SampleCount;
                    ViewBag.Samples = samples.ToArray();
                    TempData["samples"] = ViewBag.Samples;
                    return RedirectToAction("EnterSequencingData","Data");
                }
            }
            catch(Exception e)
            {
                ViewBag.Error = e;
            }

            return View();
          
        }
       
    }
}