﻿using BloopMvc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BloopMvc.Controllers
{
    public class HomeController : Controller
    {
        List<BloopFile> models = new List<BloopFile>();
        string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data";
        int fileId = 1;

        public HomeController()
        {
            SetupModel();
        }

        public ActionResult Index()
        {
            return View(models);
        }

        public ActionResult Content(int Id)
        {
            var bFile = from b in models
                             where b.Id == Id
                             select b;

            if (bFile.Count() == 1)
                return View(bFile.First<BloopFile>());
            else
                return RedirectToAction("Index");
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }

        private void SetupModel()
        {
            if (Directory.Exists(path))
            {
                String[] files = Directory.GetFiles(path);

                foreach (String file in files)
                {
                    if (file.EndsWith(".txt"))
                    {
                        var model = new BloopFile();

                        model.Id = fileId++;
                        model.Name = Path.GetFileName(file);
                        model.DateTime = System.IO.File.GetLastWriteTime(file);
                        model.Content = System.IO.File.ReadAllText(file);
                        models.Add(model);
                    }
                }
            }
            else
            {
                Console.WriteLine("{0} does not exist.", path);
            }  
        }

    }
}