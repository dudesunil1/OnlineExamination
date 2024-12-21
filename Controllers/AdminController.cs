using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineExamination.BLL;
namespace OnlineExamination.Controllers
{
    //Test Conflict
    public class AdminController : Controller
    {
        
        AdminService objAdminService = new AdminService();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AdminLoginViewModel objAdminLogin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    bool isLogin = objAdminService.Login(objAdminLogin.UserName, objAdminLogin.Password);
                    if (isLogin)
                    {
                        return RedirectToAction("Dashboard", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                }
                return View(objAdminLogin);
            }
            catch
            {
                return View();
            }
        }
    }
}