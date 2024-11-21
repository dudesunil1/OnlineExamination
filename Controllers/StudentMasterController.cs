using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.Controllers
{
    public class StudentMasterController : Controller
    {

        StudentService objStudentService = new StudentService();
        [HttpGet]
        public ActionResult Index()
        {
            List<StudentMasterModel> objPublications = objStudentService.GetStudent();
            return View(objPublications);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StudentMasterModel objstudent, string Action, HttpPostedFileBase Stud_Photo)
        {
            try
            {

                if (Action == "Cancel")
                {
                    return RedirectToAction("Index");
                }
                else if (Action == "Back To List")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        StudentMasterModel res;
                        string fileName = objstudent.Stud_Photo;

                        if (Stud_Photo != null && Stud_Photo.ContentLength > 0)
                        {
                            // Check if the file is an image (optional)
                            string[] allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                            string fileExtension = Path.GetExtension(Stud_Photo.FileName).ToLower();

                             res = objStudentService.Update(objstudent, fileExtension);
                            if (res.Stud_Id > 0)
                            {
                                if (allowedExtensions.Contains(fileExtension))
                                {

                                    string folderPath = Server.MapPath("~/Content/StudentPhotos");
                                    fileName = Path.GetFileNameWithoutExtension(Stud_Photo.FileName);
                                    string uniqueFileName = $"{res.Stud_Id}{fileExtension}";

                                    string filePath = Path.Combine(folderPath, uniqueFileName);

                                    ViewBag.FileExtension = fileExtension;
                                    Stud_Photo.SaveAs(filePath);
                                }
                                TempData["MessageModel"] = MessageModel.Success("Student Updated successfully!");
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["MessageModel"] = MessageModel.Error("An error occurred while saving the Publication.");

                            }
                        }
                        else
                        {
                             res = objStudentService.Update(objstudent, fileName);
                            if (res.Stud_Id > 0)
                            {
                                TempData["MessageModel"] = MessageModel.Success("Student Updated successfully!");
                                return RedirectToAction("Index");

                            }
                            else
                            {
                                TempData["MessageModel"] = MessageModel.Error("An error occurred while saving the Publication.");

                            }


                        }

                    }

                    return View(objstudent);
                }
            }
            catch
            {
                TempData["MessageModel"] = MessageModel.Error("An unexpected error occurred.");
                return View(objstudent); // Return view with the model on error
            }
        }

        public ActionResult Details(int id)
        {
            StudentMasterModel objQuestion = objStudentService.GetStudentById(id).FirstOrDefault();
            
            return View(objQuestion);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.grouplist = MasterService.GetGroups();
            StudentMasterModel objPublication = objStudentService.GetStudentById(id).FirstOrDefault();


            if (objPublication == null)
            {
                return HttpNotFound();
            }

            return View(objPublication);
        }

        [HttpGet]
        
        public ActionResult Create()
        {
            ViewBag.grouplist = MasterService.GetGroups();
            StudentMasterModel objStudent = new StudentMasterModel();
            return View(objStudent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentMasterModel objStudent, string Action, HttpPostedFileBase Stud_Photo)
        {
            try
            {
                if (Action == "Cancel")
                {
                    return RedirectToAction("Create");
                }
                else if (Action == "Back To List")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    if (ModelState.IsValid) 
                    {

                        if (Stud_Photo != null && Stud_Photo.ContentLength > 0)
                        {
                            
                            string[] allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                            string fileExtension = Path.GetExtension(Stud_Photo.FileName).ToLower();

                            StudentMasterModel res = objStudentService.Add(objStudent, fileExtension);
                             if (res.Stud_Id > 0)
                                 {
                           
                                if (allowedExtensions.Contains(fileExtension))
                                {
                              
                                    string folderPath = Server.MapPath("~/Content/StudentPhotos");
                                    string fileName = Path.GetFileNameWithoutExtension(Stud_Photo.FileName);
                                    string uniqueFileName = $"{res.Stud_Id}{fileExtension}";

                                    string filePath = Path.Combine(folderPath, uniqueFileName);

                                    ViewBag.FileExtension = fileExtension;
                                    Stud_Photo.SaveAs(filePath);
                                }
                            }

                            TempData["MessageModel"] = MessageModel.Success("Student Saved successfully!");
                            return RedirectToAction("Index");

                        }
                        else
                        {
                           // ViewBag.grouplist = MasterService.GetGroups();
                            TempData["MessageModel"] = MessageModel.Error("please enter image");
                        }
                    }
                    ViewBag.grouplist = MasterService.GetGroups();
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

    }
}