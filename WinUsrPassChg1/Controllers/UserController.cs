using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinUsrPassChg1.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangePassword() {
            var username = Request.Form["username"];
            var oldpassword = Request.Form["oldpass"];
            var newpassword = Request.Form["newpass"];
            var confirmpassword = Request.Form["confirm"];

            var logStr = string.Format("Got username - {0} ; oldpass - {1} ; newpass = {2} ; confirm - {3}", username, oldpassword, newpassword, confirmpassword);
            string respMsg;

            try
            {
                var ctx = new PrincipalContext(ContextType.Machine);
                var usrac = UserPrincipal.FindByIdentity(ctx, username);
                if (null == usrac)
                {
                    respMsg = "User not found";
                }
                else
                {
                    usrac.ChangePassword(oldpassword, newpassword);
                    respMsg = "Password change successful";
                }
            }
            catch (PasswordException pex) {
                if (-2147024810 == pex.InnerException.HResult)
                {
                    respMsg = "Old password was not correctly entered";
                }
                else {
                    respMsg = pex.Message + "  \r\n<br/>" + pex.StackTrace;
                }
            }
            catch (Exception ex)
            {
                respMsg = logStr + "  \r\n<br/>" + ex.Message + "  \r\n<br/>" + ex.StackTrace;
            }

            TempData["Status"] = respMsg;

            return RedirectToAction("Index");
        }
    }
}