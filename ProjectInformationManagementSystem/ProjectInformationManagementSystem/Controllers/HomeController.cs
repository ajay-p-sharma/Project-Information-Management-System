using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectInformationManagementSystem.Models;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Data.Entity;
using System.Web.Helpers;
using System.IO;

namespace ProjectInformationManagementSystem.Controllers
{
    public class HomeController : Controller
    {

        /// <summary>
        /// Returns the View for Homepage of the application.
        /// </summary>
        /// <param name="id">ID used to get the result text.</param>
        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// Returns the View for About page of the application.
        /// </summary>
        public ActionResult About()
        {


            return View();
        }


        /// <summary>
        /// Returns the View for Contact page of the application.
        /// </summary>
        public ActionResult Contact()
        {


            return View();
        }


        /// <summary>
        /// Returns the View for Service Pricing page of the application.
        /// </summary>
        public ActionResult PricingPage()
        {
            ViewBag.Message = "Project Pricing.";

            return View();
        }


        /// <summary>
        /// Returns the View and result of user registeration function of the application.
        /// </summary>
        public ActionResult UserRegistered()
        {
            return View();
        }


        /// <summary>
        /// Returns the View with form to register new user.
        /// </summary>
        public ActionResult Register()
        {
            return View();
        }


        /// <summary>
        /// Method to pass Register user Form data to create new user of the application.
        /// </summary>
        /// <param name="user">user object is passed as parameter.</param>
        [HttpPost]
        public ActionResult Register(user user)
        {
            pimsEntitiesNew db = new pimsEntitiesNew();
            try
            {
                var item = db.users.SingleOrDefault(x => x.email == user.email);
                using (db)
                    if (ModelState.IsValid && item == null)
                    {
                        var password = Crypto.HashPassword(user.password);

                        user.password = password;
                        {
                            db.users.Add(user);
                            db.SaveChanges();
                        }
                        ModelState.Clear();
                        TempData["registerSuccessMessage"] = user.first_name + " " + user.last_name + " successfully registered.";
                    }
                    else
                    {
                        TempData["registerErrorMessage"] = "User already exist. Please use a different Email ID or contact the PIMS Administrator.";
                    }
            }
            catch (Exception e)
            {
                TempData["registerErrorMessage"] = e;
            }
            return View("UserRegistered");
        }

        /// <summary>
        /// Method to get user information from database using user ID to update user information.
        /// </summary>
        /// <param name="user_id">user ID is passed as parameter.</param>
        [HttpGet]
        public ActionResult EditProfile(int? id)
        {
            pimsEntitiesNew db = new pimsEntitiesNew();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        /// <summary>
        /// Method to pass edit user Form data to update user information in the database.
        /// </summary>
        /// <param name="user">user object is passed as parameter.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(user user)
        {
            pimsEntitiesNew db = new pimsEntitiesNew();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }

                TempData["EditProfileMessage"] = "Profile Successfully updated!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error updating the user. Please make sure all fields have valid data.";
                return RedirectToAction("EditProfile");
            }
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Method to get user information from database using user ID to update user password information.
        /// </summary>
        /// <param name="user_id">user ID is passed as parameter.</param>
        [HttpGet]
        public ActionResult ChangePassword(int? id)
        {
            pimsEntitiesNew db = new pimsEntitiesNew();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        /// Method to post new or unchanged password information to database. This method is to update password only.
        /// </summary>
        /// <param name="user">user object is passed as parameter.</param>
        [HttpPost]
        public ActionResult ChangePassword(user user)
        {
            pimsEntitiesNew db = new pimsEntitiesNew();
            string newPassword = Crypto.HashPassword(user.password);
            user.password = newPassword;
            try
            {
                if (ModelState.IsValid)

                {

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }

                TempData["EditProfileMessage"] = "Profile Successfully updated!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error updating the user. Please make sure all fields have valid data.";
                return RedirectToAction("ChangePassword");
            }

            return View("Index");
        }

        /// <summary>
        /// Returns the View with form to Login.
        /// </summary>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// Method to post user credentials to be validated with database. This smethod is for validation only to control access to the application.
        /// </summary>
        /// <param name="user">user object is passed as parameter. But only email and password are matched for validation.</param>
        [HttpPost]
        public ActionResult Login(user user)
        {
            pimsEntitiesNew db = new pimsEntitiesNew();

            var userPassword = "";
            var userPass = (from d in db.users.Where(u => u.email == user.email)
                            select d.password).ToArray();

            if (userPass.Length == 0 || userPass.Length > 1)
            {
                ModelState.AddModelError("", "Invalid username or password. Please try again.");
                return View();
            }

            if (userPass[0] != null)
            {
                userPassword = userPass[0];
            }


            var verifyPassword = false;

            if (userPassword != null)
            {

                verifyPassword = Crypto.VerifyHashedPassword(userPassword, user.password);
            }

            else
            {
                ModelState.AddModelError("", "Invalid username or password. Please try again.");
            }


            if (verifyPassword != false)
            {
                try
                {
                    var usr = db.users.Single(u => u.email == user.email && verifyPassword != false);
                
                    var id = db.users.Where(p => p.email == user.email).Select(p => p.user_id).ToArray();
                    var userFirstName = db.users.Where(p => p.email == user.email).Select(p => p.first_name).ToArray();
                    var userLastName = db.users.Where(p => p.email == user.email).Select(p => p.last_name).ToArray();
                    var userType = db.users.Where(p => p.email == user.email).Select(p => p.user_type).ToArray();

                    if (usr != null)
                    {
                        Session["userID"] = id[0];
                        Session["userType"] = userType[0].ToString();
                        Session["firstName"] = userFirstName[0].ToString();
                        Session["lastName"] = userLastName[0].ToString();
                        Session["fullName"] = userFirstName[0].ToString() + " " + userLastName[0].ToString();
                        return RedirectToAction("Index");
                    }

                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password. Please try again.");
                    }
                }
                catch (InvalidOperationException e)
                {
                    ModelState.AddModelError("", "Invalid username or password. Please try again.");
                }

            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password. Please try again.");
            }

            return View();
        }


        /// <summary>
        /// Method to log out of the application. Takes back to login page.
        /// </summary>
        public ActionResult LogOut()
        {
            Session.Clear();

            return RedirectToAction("Login", "Home");
        }


        /// <summary>
        /// Method to present page with form to submit new incoming project. Used by users submitting new request.
        /// </summary>
        public ActionResult SubmitNewProject()
        {
            return View();
        }


        /// <summary>
        /// Method to post new incoming project form data.
        /// </summary>
        /// <param name="incoming_project">"incoming_project" object is passed as parameter.</param>
        [HttpPost]
        public ActionResult SubmitNewProject(incoming_project newIncomingProject)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    using (pimsEntitiesNew db = new pimsEntitiesNew())
                    {
                        db.incoming_project.Add(newIncomingProject);
                        db.SaveChanges();
                    }
                    ModelState.Clear();

                    using (pimsEntitiesNew dbpid = new pimsEntitiesNew())
                    {
                        var crum = dbpid.incoming_project.Max(d => d.temp_project_id);
                        ViewBag.successMessage = "Temporary Project ID:" + crum.ToString() + "\nYour project information has been successfully submitted.\nPlease bring your samples to the GCL along with the Temporary Project ID.";
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("projectSubmitError", ex.Message);

            }
            return View();
        }

        /// <summary>
        /// Method to present page to upload sample data using excel file.
        /// </summary>
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }


        /// <summary>
        /// Method to post sample information using excel file.
        /// </summary>
        /// <param name="excel file">"excel file is passed as parameter.</param>
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFile(HttpPostedFileBase fileIn)
        {
            try
            {
                if (Request != null)
                {

                    HttpPostedFileBase file = Request.Files["UploadedFile"];
                    DataSet ds = new DataSet();

                    if ((file != null) && (file.ContentLength > 0))
                    {
                        string fileExtension = System.IO.Path.GetExtension(file.FileName);

                        if (fileExtension == ".xls" || fileExtension == ".xlsx")
                        {
                            string fileName = file.FileName;
                            string fileContentType = file.ContentType;
                            byte[] fileBytes = new byte[file.ContentLength];
                            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                            var sampleList = new List<sample_detail>();
                            using (var package = new ExcelPackage(file.InputStream))
                            {
                                var currentSheet = package.Workbook.Worksheets;
                                var workSheet = currentSheet.First();
                                var noOfCol = workSheet.Dimension.End.Column;
                                var noOfRow = workSheet.Dimension.End.Row;

                                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                {
                                    var sample = new sample_detail();
                                    sample.project_id = int.Parse(workSheet.Cells[rowIterator, 1].Value.ToString());
                                    sample.gcl_id = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    sample.sample_id = workSheet.Cells[rowIterator, 3].Value.ToString();

                                    sampleList.Add(sample);
                                }

                                foreach (var i in sampleList)
                                {
                                    if (ModelState.IsValid)
                                    {
                                        using (pimsEntitiesNew db = new pimsEntitiesNew())
                                        {
                                            sample_detail newsample = new sample_detail();

                                            newsample.project_id = i.project_id;
                                            newsample.gcl_id = i.gcl_id;
                                            newsample.sample_id = i.sample_id;

                                            db.sample_detail.Add(newsample);
                                            db.SaveChanges();
                                        }
                                    }
                                }
                                ModelState.Clear();

                                ViewBag.Message = "File data successfully updated!";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Make sure that GCL ID for samples are unique.\nAll Fields must have values and correct Datatypes.";
            }

            return View("UploadFile");
        }



        /// <summary>
        /// Method to present view to add sample information.
        /// </summary>
        [HttpGet]
        public ActionResult AddSampleDetails()
        {
            // This is only for show by default one row for insert data to the database
            List<sample_detail> ci = new List<sample_detail> { new sample_detail { project_id = 0, gcl_id = "", sample_id = "" } };
            return View(ci);
        }


        /// <summary>
        /// Method to post sample information from web form.
        /// </summary>
        /// <param name="sample_detail">"List of "sample_detail" objects is passed as parameter.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSampleDetails(List<sample_detail> ci)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (pimsEntitiesNew dc = new pimsEntitiesNew())
                    {
                        foreach (var i in ci)
                        {
                            dc.sample_detail.Add(i);

                        }
                        dc.SaveChanges();
                        ModelState.Clear();
                        ViewBag.Message = "Data successfully saved!";

                        ci = new List<sample_detail> { new sample_detail { project_id = 0, gcl_id = "", sample_id = "" } };
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex;
            }
            return View(ci);
        }

        /// <summary>
        /// Method for users to download excel file template to create excel file to upload sample details.
        /// </summary>
        public ActionResult SampleDetailTemplate()
        {
            string path = "C:/Users/rkpan/Documents/Visual Studio 2015/Projects/ProjectInformationManagementSystem/ProjectInformationManagementSystem/Miscellaneous Files/Template_samples.xlsx";

            string extension = new FileInfo(path).Extension;
            if (extension != null || extension != string.Empty)
            {
                return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            return View("Index");
        }
    }
}


    




   
        

