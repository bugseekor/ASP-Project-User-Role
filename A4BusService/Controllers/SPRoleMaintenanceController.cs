// File Name : SPRoleMaintenanceController.cs
// An empty controller that manages roles
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
using System.Web;
using System.Web.Mvc;

namespace A4BusService.Controllers
{
    [Authorize]
    public class SPRoleMaintenanceController : Controller
    {
        public static ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager =
        new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        private RoleManager<IdentityRole> roleManager =
        new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

        // GET: SPRoleMaintenance
        /// <summary>
        /// shows list of all roles
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<IdentityRole> roles = roleManager.Roles.OrderBy(a => a.Name).ToList();
            return View(roles);
        }


        // add a new role, if role name is not already on file
        public ActionResult AddRole(string roleName)
        {
            if (roleName == null)
            {
                TempData["message"] = "role name shoudn't be null";
            }
            if (roleName.Trim() == "")
            {
                TempData["message"] = "no role name entered(blank)";
            }
            if (roleManager.RoleExists(roleName))
            {
                TempData["message"] = "role name input is already on file";
            }
            else
            {
                try
                {
                    IdentityResult result = roleManager.Create(new IdentityRole(roleName));
                    if (result.Succeeded)
                        TempData["message"] = "role added: " + roleName;
                    else
                        TempData["message"] = "role not added: " + result.Errors.ToList()[0];
                }
                catch (Exception ex)
                {
                    TempData["message"] = "exception thrown adding role: " + ex.GetBaseException().Message;
                }
            }

            return RedirectToAction("Index");
        }

        //delete role selected on the role list
        public ActionResult Delete(string roleName)
        {
            IdentityRole role = roleManager.FindByName(roleName);
            if (role == null)
                TempData["message"] = "role no on file: " + roleName;

            //if selected role is the "administrators", it won't delete it
            if( role.Name == "administrators")
            {
                TempData["message"] = "administrators can't be deleted.";
                return RedirectToAction("Index");
            }

            //if no users in the selected role, it simply delete the role
            if (role.Users.Count == 0)
            {
                try
                {
                    IdentityResult result = roleManager.Delete(role);
                    if (result.Succeeded)
                        TempData["message"] = "role deleted: " + roleName;
                    else
                        TempData["message"] = "delete failed: " + result.Errors.ToList()[0];
                }
                catch (Exception ex)
                {
                    TempData["message"] = "exception deleting role: " + ex.GetBaseException().Message;
                }
            }
                // if there are some users in the selected role, this lists the members on the view
            else
            {
                List<ApplicationUser> members = new List<ApplicationUser>();
                foreach (var item in role.Users)
                {
                    members.Add(userManager.FindById(item.UserId));
                    ViewBag.roleName = roleName;
                }
                return View(members);
            }
            return RedirectToAction("Index");
        }

        //after going through the user list, finally get confirmation for forcing deletion
        //and delete the selected role
        [HttpPost]
        public ActionResult delete(string roleName)
        {
            IdentityRole role = roleManager.FindByName(roleName);
            try
            {
                IdentityResult result = roleManager.Delete(role);
                if (result.Succeeded)
                    TempData["message"] = "role deleted: " + roleName;
                else
                    TempData["message"] = "delete failed: " + result.Errors.ToList()[0];
            }
            catch (Exception ex)
            {
                TempData["message"] = "exception deleting role: " + ex.GetBaseException().Message;
                List<ApplicationUser> members = new List<ApplicationUser>();
                foreach (var item in role.Users)
                {
                    members.Add(userManager.FindById(item.UserId));
                    ViewBag.roleName = roleName;
                }
                return View(members);
            }

            return RedirectToAction("Index");
        }

        //put the users who don't have role yet on dropdown list
        //list users who are in the selected role on the view
        public ActionResult Manage(string roleName)
        {
            IdentityRole role = roleManager.FindByName(roleName);

            List<IdentityUserRole> userRoles = role.Users.ToList();
            List<ApplicationUser> members = new List<ApplicationUser>();

            foreach (var item in userRoles)
            {
                members.Add(userManager.FindById(item.UserId));
            }
            List<ApplicationUser> allUsers = userManager.Users.ToList();
            List<ApplicationUser> nonMembers = new List<ApplicationUser>();
            foreach (var item in allUsers)
            {
                if (!members.Contains(item))
                    nonMembers.Add(item);
            }

            ViewBag.roleName = roleName;
            ViewBag.userName = User.Identity.Name;
            ViewBag.userId = new SelectList(nonMembers, "Id", "userName");
            return View(members);
        }

        //it is from a form(dropdownlist and submit button) of the view
        //add selected user to the role
        public ActionResult AddToRole(string roleName, string userId)
        {
            IdentityRole role = roleManager.FindByName(roleName);
            IdentityUser user = userManager.FindById(userId);

            try
            {
                userManager.AddToRole(userId: user.Id, role: role.Name);
            }
            catch (Exception ex)
            {
                TempData["message"] = "exception adding user to user role: " + ex.GetBaseException().Message;
            }
            return RedirectToAction("Manage", new { roleName = roleName });
        }

        //remove selected user from the role
        public ActionResult RemoveUser(string roleName, string userId)
        {
            IdentityRole role = roleManager.FindByName(roleName);
            IdentityUser user = userManager.FindById(userId);
            IdentityRole roleAdministrators = roleManager.FindByName("administrators");

            //if logged on user who is in administrator role tries to remove the user's role,
            //it denies to remove
            if(User.IsInRole("administrator") && User.Identity.ToString() == userId)
            {
                TempData["message"] = "you can't remove your role from administratros";
            }
            else
            {
                try
                {
                    userManager.RemoveFromRole(userId: user.Id, role: role.Name);
                    TempData["message"] = user.Email + "was removed from " + role.Name + " role";
                }
                catch (Exception ex)
                {
                    TempData["message"] = "exception removing user role: " + ex.GetBaseException().Message;
                }
            }
            
            //going back to user list of the role
            return RedirectToAction("Manage", new { roleName = roleName });
        }
    }
}