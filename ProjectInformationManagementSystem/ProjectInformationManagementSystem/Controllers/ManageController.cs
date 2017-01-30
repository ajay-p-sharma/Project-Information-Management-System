using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectInformationManagementSystem.Models;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Web.Helpers;

namespace ProjectInformationManagementSystem.Controllers
{
    public class ManageController : Controller
    {
        pimsEntitiesNew db = new pimsEntitiesNew();

        /// <summary>
        /// Method to present page with form to enter new Project information.
        /// </summary>
        [HttpGet]
        public ActionResult AddNewProject()
        {
            return View();
        }

        /// <summary>
        /// Method to post data for new project.
        /// </summary>
        /// <param name="new_project newProject">"new_project" object is passed as parameter.</param>
        [HttpPost]
        public ActionResult AddNewProject(new_project newProject)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    using (pimsEntitiesNew db = new pimsEntitiesNew())
                    {

                        db.new_project.Add(newProject);
                        db.SaveChanges();
                    }
                    ModelState.Clear();
                    using (pimsEntitiesNew dbpid = new pimsEntitiesNew())
                    {
                        var crum = dbpid.new_project.Max(d => d.project_id);
                        ViewBag.Result = "Your Project:" + crum.ToString() + " successfully added to the records";
                    }
                }
            }

            catch (Exception ex)
            {
                ViewBag.Error=ex.Message;

            }
            return View();
        }


        /// <summary>
        /// Method to get page with form to edit project data for new project.
        /// </summary>
        /// <param name="project_id">"project_is" is passed as parameter.</param>
        [HttpGet]
        public ActionResult EditProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            
           
            new_project project = db.new_project.Find(id);
            if (id == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }


        /// <summary>
        /// Method to post updated project data for new project.
        /// </summary>
        /// <param name="new_project">"new_project" object is passed as parameter.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProject(new_project project)
        {
            try
            {
                
                if (ModelState.IsValid)
                   
                {
                    db.Entry(project).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ProjectQueue");
                }
                
                ViewBag.Result = "Information successfully updated";
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("projectSubmissionError", ex.Message);
            }
            return View(project);
        }


        /// <summary>
        /// Method to get and display project details for new project.
        /// </summary>
        /// <param name="id">"project_id" is passed as parameter.</param>
        [HttpGet]
        public ActionResult ProjectDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            new_project project = db.new_project.Find(id);
            if (id == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }


        /// <summary>
        /// Method to confirm deletion of project data.
        /// </summary>
        /// <param name="id">"project_id" is passed as parameter.</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            incoming_project inProject = db.incoming_project.Find(id);
            db.incoming_project.Remove(inProject);
            db.SaveChanges();
            return RedirectToAction("TemporaryProjectQueue");    
        }


        /// <summary>
        /// Method to confirm deletion of project data.
        /// </summary>
        /// <param name="id">"project_id" is passed as parameter.</param>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            incoming_project incoming_project = db.incoming_project.Find(id);
            if (id == null)
            {
                return HttpNotFound();
            }
            return View(incoming_project);
            
        }


        private pimsEntitiesNew newdb = new pimsEntitiesNew();


        /// <summary>
        /// Method to display information of all the projects in database.
        /// </summary>
        [HttpGet]
        public ActionResult ProjectQueue()
        {
            return View(newdb.new_project.ToList());
         }


        /// <summary>
        /// Method to display information of all the projects recently submitted by the users.
        /// </summary>
        [HttpGet]
        public ActionResult TemporaryProjectQueue()
        {
            return View(newdb.incoming_project.ToList());
        }


        /// <summary>
        /// Method to display information of a project submitted by the user.
        /// </summary>
        /// <param name="id">"new_project_id" is passed as parameter.</param>
        [HttpGet]
        public ActionResult TempProjectDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            incoming_project incoming_project = db.incoming_project.Find(id);
            if (id == null)
            {
                return HttpNotFound();
            }
            return View(incoming_project);
        }

        /// <summary>
        /// Method to upgrade a incoming_project submitted by user to new_project accepted by the GCL.
        /// </summary>
        /// <param name="id">"new_project_id" is passed as parameter.</param>
        [HttpGet]
        public ActionResult PostIncomingToNewProject(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            incoming_project projectIn = db.incoming_project.Find(id);
            if (id == null)
            {
                return HttpNotFound();
            }
            TempData["project_name"] = projectIn.project_name;
            TempData["lab_head"] = projectIn.lab_head;
            TempData["investigator"] = projectIn.contact_name;
            TempData["email"] = projectIn.contact_email;
            TempData["contact_phone"] = projectIn.contact_phone;
            TempData["service_requested"] = projectIn.service_requested;
            TempData["sample_type"] = projectIn.sample_type;
            TempData["number_of_samples"] = projectIn.number_of_samples;
            TempData["species"] = projectIn.species;
            TempData["service_requested"] = projectIn.service_requested;
            TempData["downstream_process"] = projectIn.downstream_process;
            TempData["comments"] = projectIn.comments;
            return View();

        }


        /// <summary>
        /// Method to post a incoming_project submitted by user as new_project accepted by the GCL.
        /// </summary>
        /// <param name="newPproject newProject">"new_project" object is passed as parameter.</param>
        [HttpPost]
        public ActionResult PostIncomingToNewProject(new_project newProject)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (pimsEntitiesNew db = new pimsEntitiesNew())
                    {

                        db.new_project.Add(newProject);
                        db.SaveChanges();
                    }
                    ModelState.Clear();
                    using (pimsEntitiesNew dbpid = new pimsEntitiesNew())
                    {
                        var crum = dbpid.new_project.Max(d => d.project_id);
                        ViewBag.Result = "Your Project:" + crum.ToString() + " successfully added to the records";
                    }
                }
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("projectSubmissionError", ex.Message);

            }
            return View();
        }



        /// <summary>
        /// Method to present user management options. This view display list of users in the database along wit options to create, update, edit and delet eoptions. 
        /// </summary>
        [HttpGet]
        public ActionResult ManageUsers()
        {
           
            var users = db.users.ToList();
            return View(users);
        }


        /// <summary>
        /// Method to edit user data.
        /// </summary>
        /// <param name="id">"user_id" is passed as parameter.</param>
        [HttpGet]
        public ActionResult EditUser(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }


            user editUser = db.users.Find(id);
            if (id == null)
            {
                return HttpNotFound();
            }
            return View(editUser);
        }

        /// <summary>
        /// Method to post updated user data.
        /// </summary>
        /// <param name="user">"user" object is passed as parameter.</param>
        [HttpPost]
        public ActionResult EditUser(user user)
        {
            try
            {
                string newPassword = Crypto.HashPassword(user.password);
                user.password = newPassword;
                if (ModelState.IsValid)

                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("projectSubmissionError", ex.Message);
            }
            return RedirectToAction("ManageUsers");
        }


        /// <summary>
        /// Method to delete user data.
        /// </summary>
        /// <param name="id">"user_id" is passed as parameter.</param>
        [HttpGet]
        public ActionResult DeleteUser(int? id)
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
        /// Method to delete user data.
        /// </summary>
        /// <param name="id">"user_id" is passed as parameter.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(int id)
        {
            pimsEntitiesNew db = new pimsEntitiesNew();
            user userToDelete = db.users.Find(id);
            db.users.Remove(userToDelete);
            db.SaveChanges();
            return RedirectToAction("ManageUsers");
        }


        public ActionResult Samples()
        {

            return View();
        }
        
        public PartialViewResult showSamples(FormCollection fc)
        {
            int id = Convert.ToInt32(fc["searchProjectId"]);
            pimsEntitiesNew db = new pimsEntitiesNew();
            List<sample_detail>samples = new List<sample_detail>();
            try
            {
                samples = db.sample_detail
                              .Where(s => s.project_id == id).ToList();

                if (samples.Count() == 0)
                {
                    ViewBag.Error = "No samples found for this project";
                }
            }catch(Exception e)
            {
                ViewBag.Error = "No samples found for this project";
            }
            return PartialView("_SampleData",samples);
        }

        [HttpGet]
        public PartialViewResult EditSample(int? id)
        {
            if (id == null)
            {
                ViewBag.Error="Sample Not found";
            }


            sample_detail editSample = db.sample_detail.Find(id);
            
            return PartialView("_EditSample",editSample);
        }


        public ActionResult EditSample(sample_detail sample)
        {
            try
            {
                if (ModelState.IsValid)

                {
                    db.Entry(sample).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error="Check Sample Details.\nGCL ID must be unique values.";
            }
            return View("Samples");
        }

        [HttpGet]
        public PartialViewResult DeleteSample(int? id)
        {
            sample_detail deleteSample = new sample_detail();
            if (id == null)
            {
                ViewBag.Error = "Sample Not found";
            }
            try
            {
                deleteSample = db.sample_detail.Find(id);
            }catch(Exception e)
            {
                ViewBag.Error = "Error occured. Sample Not found";
            }
            return PartialView("_DeleteSample", deleteSample);
           
        }

        [HttpPost]
        public ActionResult DeleteSample(int id)
        {
            try
            {
                sample_detail sampleToDelete = db.sample_detail.Find(id);
                db.sample_detail.Remove(sampleToDelete);
                db.SaveChanges();
            }catch(Exception e)
            {
                ViewBag.Error = "Error occured. Sample Not found";
            }
            return View("Samples");
        }

    }

}
