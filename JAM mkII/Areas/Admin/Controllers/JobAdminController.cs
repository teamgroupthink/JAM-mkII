﻿using System.Linq;
using JAM_mkII.Areas.Admin.Models;
using JAM_mkII.Areas.Admin.Models.ViewModels;
using JAM_mkII.Models;
using JAM_mkII.Models.DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JAM_mkII.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class JobAdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private JobManagerContext Context { get; }

        public JobAdminController(JobManagerContext ctx, UserManager<User> userMngr,
            RoleManager<IdentityRole> roleMngr)
        {
            userManager = userMngr;
            roleManager = roleMngr;
            Context = ctx;
        }

        public IActionResult JobMgmt1()
        {
            var jobs =
                Context.Jobs
                    .Include(p => p.PositionName)
                    .Include(s => s.StoreName)
                    .OrderBy(j => j.JobId)
                    .ToList();
            var apps =
                Context.Applications
                    // .Include(p => p.PositionName)
                    // .Include(s => s.StoreName)
                    .OrderBy(a => a.ApplicationId)
                    .ToList();

            JobViewModel model = new()
            {
                Jobs = jobs,
                Applications = apps,
            };
            return View(model);
        }

        public IActionResult Add()
        {
            ViewBag.Action = "Add Job";
            return View("Edit", new Job());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit Job";
            var job = Context.Jobs.Find(id);
            return View(job);
        }

        [HttpPost]
        public IActionResult Edit(Job job)
        {
            if (ModelState.IsValid)
            {
                if (job.JobId == 0)
                    Context.Jobs.Add(job);
                else
                    Context.Jobs.Update(job);

                Context.SaveChanges();
                return RedirectToAction("JobMgmt1");
            }

            ViewBag.Action = job.JobId == 0 ? "Add" : "Edit";
            return View(job);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var job = Context.Jobs.Find(id);
            return View(job);
        }

        [HttpPost]
        public IActionResult Delete(Job job)
        {
            Context.Jobs.Remove(job);
            Context.SaveChanges();
            return RedirectToAction("JobMgmt");
        }
    }
}