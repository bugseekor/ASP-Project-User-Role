// File Name : SPUserMaintenanceController.cs
// An empty controller that manages users
//
// Author : Sam Sangkyun Park
// Date Created : Nov. 30, 2015
// Revision History : Version 1 created : Nov. 30, 2015

using A4BusService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace A4BusService.Controllers
{
    [Authorize]
    public class SPUserMaintenanceController : Controller
    {

        public static ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager =
        new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        private RoleManager<IdentityRole> roleManager =
        new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

        // GET: SPUserMaintenance
        
        // shows list of all users
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<ApplicationUser> users = userManager.Users.OrderBy(a => a.UserName).ToList();

            IdentityRole role = roleManager.FindByName("administrators");
            ViewBag.administratorsRole = role.Id;

            return View(users);
        }

        /// <summary>
        /// delete user from all roles enrolled in and from user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            ApplicationUser user = userManager.FindById(id);

            if (user == null)
            {
                TempData["message"] = "user no on file: " + id;
            }
            try
            {
                // remove all user roles
                var roles = userManager.GetRoles(id).ToList();
                foreach (var item in roles)
                {
                    userManager.RemoveFromRole(userId: id, role: item);
                }
                
                // delete user
                IdentityResult result = userManager.Delete(user);
                if (result.Succeeded)
                    TempData["message"] = "user deleted: " + user.UserName;
                else
                    TempData["message"] = "delete failed: " + result.Errors.ToList()[0];
            }
            catch (Exception ex)
            {
                TempData["message"] = "exception deleting user: " + ex.GetBaseException().Message;
            }
            
            return RedirectToAction("Index");
        }

        /// <summary>
        /// reset user's password
        /// show up password input control and confirmation control
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // only members of the administrators role can reset passwords 
        public ActionResult ResetPassword(String id)
        {
            if(User.IsInRole("administrators"))
            {
                ApplicationUser user = userManager.FindById(id);
            
                if (user == null)
                {
                    TempData["message"] = "user no on file: " + id;
                    return RedirectToAction("Index");
                }

                //administrators' password can't be changed
                foreach (var item in user.Roles)
                {
                    if(item.RoleId == roleManager.FindByName("administrators").Id)
                    {
                        TempData["message"] = "can't change administrators' password";
                        return RedirectToAction("Index");
                    }
                }

                ResetPasswordViewModel userNewPassword = new ResetPasswordViewModel();
                userNewPassword.Code = user.Id;
                userNewPassword.Email = user.Email;

                return View(userNewPassword);
            }
            else
            {
                TempData["message"] = "only users in administrators role can reset password";
                return RedirectToAction("Index");
            }

        }

        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = userManager.FindById(model.Code);
            if (user == null)
            {
                TempData["message"] = "user no on file: " + model.Code;
                return View(model);
            }

            try
            {
                var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("MusicStore");

                //generate token to encryption
                userManager.UserTokenProvider =
                   new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<ApplicationUser>
                       (provider.Create("PasswordReset"));

                string passwordToken = userManager.GeneratePasswordResetToken(model.Code);
                //reset password with encryption
                IdentityResult identityResult = userManager.ResetPassword(model.Code, passwordToken, model.Password);

                TempData["message"] = "user password is reset: " + model.Email;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["message"] = "exception resetting user password: " + ex.GetBaseException().Message;
                return View(model);
            }
            
        }

        /// <summary>
        /// if the user locked, it is unlocked and vice-versa.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LockUnlock(string id)
        {
            ApplicationUser user = userManager.FindById(id);

            if (user == null)
            {
                TempData["message"] = "user no on file: " + id;
            }
            try
            {
                if(user.LockoutEnabled == true)
                {
                    user.LockoutEnabled = false;
                    user.LockoutEndDateUtc = null;
                }
                else
                {
                    user.LockoutEnabled = true;
                }
                db.SaveChanges();
                
                TempData["message"] = "user locked/unlocked: " + user.UserName;
            }
            catch (Exception ex)
            {
                TempData["message"] = "exception lock/unlock user: " + ex.GetBaseException().Message;
            }

            return RedirectToAction("Index");
        }
    }
}