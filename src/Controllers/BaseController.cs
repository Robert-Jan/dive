using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Dive.App.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly List<string> _notifications = new();

        protected void SetNotification(string title, string message, bool success = true)
        {
            string type = success ? "success" : "danger";

            _notifications.Add($"{type}::{title}::{message}");

            TempData["Notifications"] = _notifications;
        }
    }
}