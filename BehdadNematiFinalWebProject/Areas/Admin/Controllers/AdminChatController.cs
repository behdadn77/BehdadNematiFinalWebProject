﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BehdadNematiFinalWebProject.Areas.Admin.Controllers
{
    public class AdminChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}