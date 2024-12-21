using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.Controllers
{
    [CheckSessionRole]
    public class SubjectMasterController : Controller
    {
        SubjectService objSubjectMasterService = new SubjectService();

       
        public ActionResult Index()
        {
           
            
            List<SubjectsViewModel> objSubjects = objSubjectMasterService.GetSubjects();
            return View(objSubjects);
        }



      
        public ActionResult Create()
        {
           
            SubjectsViewModel objSubject = new SubjectsViewModel();
            return View(objSubject);
        }

        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            
            SubjectsViewModel objSubject = objSubjectMasterService.GetSubjectsById(id).FirstOrDefault();

            if (objSubject == null)
            {
                return HttpNotFound(); 
            }

            return View(objSubject); 
        }

        
        [HttpPost]
      
        public ActionResult Edit(SubjectsViewModel objSubject, string Action)
        {
            try
            {

                if (Action == "Cancel")
                {
                    return RedirectToAction("Index"); // Redirect to the create page if cancel is clicked
                }
                else if (Action == "Back To List")
                {
                    return RedirectToAction("Index"); // Redirect to list page if back to list is clicked
                }
                else
                {
                    // If the model is valid, update the subject
                    if (ModelState.IsValid)
                    {
                        
                        SubjectsViewModel res = objSubjectMasterService.Update(objSubject);

                        if (res.Sub_Id > 0)
                        {
                            TempData["MessageModel"] = MessageModel.Success("Subject updated successfully!");
                            return RedirectToAction("Index"); // Redirect to the list page
                        }
                        else
                        {
                            TempData["MessageModel"] = MessageModel.Error("An error occurred while updating the subject.");
                        }
                    }
                    return View(objSubject); // Return view with model if validation fails
                }
            }
            catch
            {
                TempData["MessageModel"] = MessageModel.Error("An unexpected error occurred.");
                return View(objSubject); // Return the view with the model on error
            }
        }

        // POST action for creating a new subject
        [HttpPost]
    
        public ActionResult Create(SubjectsViewModel objSubject, string Action)
        {
            try
            {
                // Handle different button actions (e.g., "Cancel", "Back To List")
                if (Action == "Cancel")
                {
                    return RedirectToAction("Create"); // Redirect to the create page if cancel is clicked
                }
                else if (Action == "Back To List")
                {
                    return RedirectToAction("Index"); // Redirect to list page if back to list is clicked
                }
                else
                {
                   
                    if (ModelState.IsValid)
                    {
                        
                        SubjectsViewModel res = objSubjectMasterService.AddSubject(objSubject);

                        if (res.Sub_Id > 0)
                        {
                            TempData["MessageModel"] = MessageModel.Success("Subject Created successfully!");
                            return RedirectToAction("Index"); 
                        }
                        else
                        {
                            TempData["MessageModel"] = MessageModel.Error("An error occurred while Creating  the subject.");
                            return RedirectToAction("Create"); 
                        }
                    }
                    return View(); 
                }
            }
            catch
            {
                return View(); // Return to the Create view if an error occurs
            }
        }
    }
}