namespace DoctrineShips.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.UI;
    using AutoMapper;
    using DevTrends.MvcDonutCaching;
    using DoctrineShips.Entities;
    using DoctrineShips.Service;
    using DoctrineShips.Service.Entities;
    using DoctrineShips.Validation;
    using DoctrineShips.Web.ViewModels;
    using Tools;

    [Authorize(Roles = "Admin,SiteAdmin")]
    public class AccountController : Controller
    {
        private readonly IDoctrineShipsServices doctrineShipsServices;

        public AccountController(IDoctrineShipsServices doctrineShipsServices)
        {
            this.doctrineShipsServices = doctrineShipsServices;
        }

        [AllowAnonymous]
        public ActionResult Authenticate(string accountId, string key, string secondKey, string redir)
        {
            Role role = Role.None;

            // Cleanse the passed account id and password string to prevent XSS.
            int cleanAccountId = Conversion.StringToInt32(Server.HtmlEncode(accountId));
            string cleanKey = Conversion.StringToSafeString(Server.HtmlEncode(key));
            string cleanSecondKey = Conversion.StringToSafeString(Server.HtmlEncode(secondKey));
            string cleanRedir = Server.HtmlEncode(redir);

            // Was the second key passed and is it correct?
            if (cleanSecondKey == this.doctrineShipsServices.Settings.SecondKey)
            {
                // Bypass the account id checks.
                role = doctrineShipsServices.Authenticate(cleanAccountId, cleanKey, bypassAccountChecks: true);
            }
            else
            {
                // Authenticate the passed accountId and key.
                role = doctrineShipsServices.Authenticate(cleanAccountId, cleanKey);
            }

            if (role != Role.None)
            {
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1,                            // Version number of the ticket.
                cleanAccountId.ToString(),    // Username for the ticket.
                DateTime.UtcNow,              // Issue date of the ticket.
                DateTime.UtcNow.AddDays(30),  // Expiry date of the ticket.
                true,                         // Persistence enabled for the ticket?
                role.ToString(),              // User-specific data which in this case is their roles.
                "/");                         // The path for the ticket.

                HttpCookie newCookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                                    FormsAuthentication.Encrypt(authTicket));

                // Set the expiry date of the new cookie to 30 days.
                newCookie.Expires = DateTime.UtcNow.AddDays(30);

                Response.Cookies.Add(newCookie);

                // If a local and clean url has been passed as a parameter, redirect to it.
                if (!String.IsNullOrEmpty(cleanRedir) && Url.IsLocalUrl(cleanRedir))
                {
                    return Redirect(cleanRedir);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [DonutOutputCache(Duration = 300, VaryByCustom = "Account", Location = OutputCacheLocation.Server)]
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RoleCheck()
        {
            string role = string.Empty;
            string account = "Account: " + User.Identity.Name;

            if (User.IsInRole("User"))
            {
                role = "Role: 'User'";
            }
            else if (User.IsInRole("Admin"))
            {

                role = "Role: 'Admin'";
            }
            else if (User.IsInRole("SiteAdmin"))
            {

                role = "Role: 'Site Admin'";
            }
            else
            {
                role += "Role: 'None'";
            }

            return Content(account + " " + role);
        }

        public ActionResult ShipFits()
        {
            AccountShipFitsViewModel viewModel = new AccountShipFitsViewModel();

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            viewModel.ShipFits = this.doctrineShipsServices.GetShipFitList(accountId);

            // Set the ViewBag to the TempData status value passed from the modify methods.
            ViewBag.Status = TempData["Status"];

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteShipFit(AccountShipFitsViewModel viewModel)
        {
            if (viewModel.RemoveList != null)
            {
                // Convert the currently logged-in account id to an integer.
                int accountId = Conversion.StringToInt32(User.Identity.Name);

                // Create a collection for the results of the delete operations.
                ICollection<bool> resultList = new List<bool>();

                foreach (var shipFitId in viewModel.RemoveList)
                {
                    resultList.Add(this.doctrineShipsServices.DeleteShipFit(accountId, shipFitId));
                }

                // If any of the deletions failed, output an error message.
                if (resultList.Contains(false))
                {
                    TempData["Status"] = "Error: One or more ship fits were not removed.";
                }
                else
                {
                    TempData["Status"] = "All selected ship fits were successfully removed.";
                }
            }

            return RedirectToAction("ShipFits");
        }

        public ActionResult SalesAgents()
        {
            AccountSalesAgentsViewModel viewModel = new AccountSalesAgentsViewModel();

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            viewModel.SalesAgents = this.doctrineShipsServices.GetSalesAgents(accountId);

            // Set the ViewBag to the TempData status value passed from the Add & Delete methods.
            ViewBag.Status = TempData["Status"];

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public async Task<ActionResult> AddSalesAgent(AccountSalesAgentsViewModel viewModel)
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            if (ModelState.IsValid)
            {
                // Clean the passed api key.
                string cleanApiKey = Conversion.StringToSafeString(Server.HtmlEncode(viewModel.ApiKey));

                IValidationResult validationResult = await this.doctrineShipsServices.AddSalesAgent(viewModel.ApiId, cleanApiKey, accountId);
                
                // If the validationResult is not valid, something did not validate in the service layer.
                if (validationResult.IsValid)
                {
                    TempData["Status"] = "The sales agent was successfully added.";
                }
                else
                {
                    TempData["Status"] = "Error: The sales agent was not added, a validation error occured.<br /><b>Error Details: </b>";

                    foreach (var error in validationResult.Errors)
                    {
                        TempData["Status"] += error.Value + "<br />";
                    }
                }

                return RedirectToAction("SalesAgents");
            }
            else
            {
                // Re-populate the view model and return with any validation errors.
                viewModel.SalesAgents = this.doctrineShipsServices.GetSalesAgents(accountId);
                return View("~/Views/Account/SalesAgents.cshtml", viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteSalesAgent(AccountSalesAgentsViewModel viewModel)
        {
            if (viewModel.RemoveList != null)
            {
                // Convert the currently logged-in account id to an integer.
                int accountId = Conversion.StringToInt32(User.Identity.Name);

                // Create a collection for the results of the delete operations.
                ICollection<bool> resultList = new List<bool>();

                foreach (var salesAgentId in viewModel.RemoveList)
                {
                    resultList.Add(this.doctrineShipsServices.DeleteSalesAgent(accountId, salesAgentId));
                }

                // If any of the deletions failed, output an error message.
                if (resultList.Contains(false))
                {
                    TempData["Status"] = "Error: One or more sales agents were not removed.";
                }
                else
                {
                    TempData["Status"] = "All selected sales agents were successfully removed.";
                }
            }

            return RedirectToAction("SalesAgents");
        }

        public ActionResult Settings()
        {
            // Create an Auto Mapper map between the setting profile entity and the view model.
            Mapper.CreateMap<SettingProfile, AccountSettingsViewModel>();

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Fetch the account settings and map them to the view model.
            AccountSettingsViewModel viewModel = Mapper.Map<SettingProfile, AccountSettingsViewModel>(this.doctrineShipsServices.GetAccountSettingProfile(accountId));

            // Set the ViewBag to the TempData status value passed from the UpdateSettings method.
            ViewBag.Status = TempData["Status"];

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateSettings(AccountSettingsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Create an Auto Mapper map between the setting profile entity and the view model.
                Mapper.CreateMap<AccountSettingsViewModel, SettingProfile>();

                // Convert the currently logged-in account id to an integer.
                viewModel.AccountId = Conversion.StringToInt32(User.Identity.Name);

                // Populate a setting profile with automapper and pass it back to the service layer for update.
                SettingProfile settingProfile = Mapper.Map<AccountSettingsViewModel, SettingProfile>(viewModel);
                var result = this.doctrineShipsServices.UpdateSettingProfile(settingProfile);

                // If the result is false, something did not validate in the service layer.
                if (result != false)
                {
                    TempData["Status"] = "The settings were successfully updated.";
                }
                else
                {
                    TempData["Status"] = "Error: The settings were not updated, a validation error occured.";
                }
                
                return RedirectToAction("Settings");
            }
            else
            {
                return View("~/Views/Account/Settings.cshtml", viewModel);
            }
        }

        public ActionResult AccessCodes()
        {
            AccountAccessCodesViewModel viewModel = new AccountAccessCodesViewModel();

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Set the ViewBag to the TempData status value passed from the Add & Delete methods.
            ViewBag.Status = TempData["Status"];

            // Retrieve a current list of access codes for the current account.
            viewModel.AccessCodes = this.doctrineShipsServices.GetAccessCodes(accountId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddAccessCode(AccountAccessCodesViewModel viewModel)
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            if (ModelState.IsValid)
            {
                // Ensure that the description input is safe before being passed to the service layer.
                string description = Conversion.StringToSafeString(Server.HtmlEncode(viewModel.Description));

                // Generate a new access code.
                string newKey = this.doctrineShipsServices.AddAccessCode(accountId, description, (Role)viewModel.SelectedRole);

                // If the new key string is empty, something did not validate in the service layer.
                if (newKey != string.Empty)
                {
                    // Assign the new key to TempData to be passed to the AccessCodes view.
                    string authUrl = this.doctrineShipsServices.Settings.WebsiteDomain + "/A/" + accountId + "/" + newKey;
                    TempData["Status"] = "Success, the auth url is: <a href=\"" + authUrl + "\">" + authUrl + "</a>";
                }
                else
                {
                    TempData["Status"] = "Error: The access code was not added, a validation error occured.";
                }

                return RedirectToAction("AccessCodes");
            }
            else
            {
                // Re-populate the view model and return with any validation errors.
                viewModel.AccessCodes = this.doctrineShipsServices.GetAccessCodes(accountId);
                return View("~/Views/Account/AccessCodes.cshtml", viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteAccessCode(AccountAccessCodesViewModel viewModel)
        {
            if (viewModel.RemoveList != null)
            {
                // Convert the currently logged-in account id to an integer.
                int accountId = Conversion.StringToInt32(User.Identity.Name);

                // Create a collection for the results of the delete operations.
                ICollection<bool> resultList = new List<bool>();

                foreach (var accessCodeId in viewModel.RemoveList)
                {
                    resultList.Add(this.doctrineShipsServices.DeleteAccessCode(accountId, accessCodeId));
                }

                // If any of the deletions failed, output an error message.
                if (resultList.Contains(false))
                {
                    TempData["Status"] = "Error: One or more access codes were not removed.";
                }
                else
                {
                    TempData["Status"] = "All selected access codes were successfully removed.";
                }
            }

            return RedirectToAction("AccessCodes");
        }

        public ActionResult ImportShipFits()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ImportEveXml(HttpPostedFileBase xmlFile)
        {
            IEnumerable<ShipFit> importedShipFits = null;

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            if (xmlFile != null && xmlFile.ContentLength > 0 && xmlFile.ContentType == "text/xml")
            {
                StreamReader streamReader = new StreamReader(xmlFile.InputStream);
                importedShipFits = this.doctrineShipsServices.ImportShipFit(streamReader.ReadToEnd().ToString(), accountId);
            }

            // Store the temporary imported ship fit list in the http session.
            System.Web.HttpContext.Current.Session["importedShipFits"] = importedShipFits;

            return RedirectToAction("ImportResults");
        }

        public ActionResult ImportResults()
        {
            ShipFitImportResultsViewModel viewModel = new ShipFitImportResultsViewModel();

            // Retrieve the temporary imported ship fit list from the http session and assign it to the view model.
            viewModel.ShipFits = System.Web.HttpContext.Current.Session["importedShipFits"] as IEnumerable<ShipFit>;

            return View(viewModel);
        }

        public ActionResult UpdateAccessCodeState(string accessCodeId, string isActive)
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Clean the controller parameters.
            int cleanAccesscodeId = Conversion.StringToInt32(Server.HtmlEncode(accessCodeId));
            bool cleanIsActive = Conversion.StringToBool(Server.HtmlEncode(isActive));

            // Update the state.
            this.doctrineShipsServices.UpdateAccessCodeState(accountId, cleanAccesscodeId, cleanIsActive);

            return RedirectToAction("AccessCodes");
        }

        public ActionResult UpdateSalesAgentState(string salesAgentId, string isActive)
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Clean the controller parameters.
            int cleanSalesAgentId = Conversion.StringToInt32(Server.HtmlEncode(salesAgentId));
            bool cleanIsActive = Conversion.StringToBool(Server.HtmlEncode(isActive));

            // Update the state.
            this.doctrineShipsServices.UpdateSalesAgentState(accountId, cleanSalesAgentId, cleanIsActive);

            return RedirectToAction("SalesAgents");
        }

        public ActionResult UpdateShipFitMonitoringState(string shipFitId, string isActive)
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Clean the controller parameters.
            int cleanShipFitId = Conversion.StringToInt32(Server.HtmlEncode(shipFitId));
            bool cleanIsActive = Conversion.StringToBool(Server.HtmlEncode(isActive));

            // Update the state.
            this.doctrineShipsServices.UpdateShipFitMonitoringState(accountId, cleanShipFitId, cleanIsActive);

            return RedirectToAction("ShipFits");
        }

        public ActionResult ForceContractRefresh(string salesAgentId)
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Clean the controller parameters.
            int cleanSalesAgentId = Conversion.StringToInt32(Server.HtmlEncode(salesAgentId));

            // Force a contract refresh for the passed sales agent.
            this.doctrineShipsServices.ForceContractRefresh(accountId, cleanSalesAgentId);

            return RedirectToAction("SalesAgents");
        }

        public ActionResult NotificationRecipients()
        {
            AccountNotificationRecipientsViewModel viewModel = new AccountNotificationRecipientsViewModel();

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            viewModel.NotificationRecipients = this.doctrineShipsServices.GetNotificationRecipients(accountId);

            // Set the ViewBag to the TempData status value passed from the Add & Delete methods.
            ViewBag.Status = TempData["Status"];

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteNotificationRecipient(AccountNotificationRecipientsViewModel viewModel)
        {
            if (viewModel.RemoveList != null)
            {
                // Convert the currently logged-in account id to an integer.
                int accountId = Conversion.StringToInt32(User.Identity.Name);

                // Create a collection for the results of the delete operations.
                ICollection<bool> resultList = new List<bool>();

                foreach (var notificationRecipientId in viewModel.RemoveList)
                {
                    resultList.Add(this.doctrineShipsServices.DeleteNotificationRecipient(accountId, notificationRecipientId));
                }

                // If any of the deletions failed, output an error message.
                if (resultList.Contains(false))
                {
                    TempData["Status"] = "Error: One or more notification recipients were not removed.";
                }
                else
                {
                    TempData["Status"] = "All selected notification recipients were successfully removed.";
                }
            }

            return RedirectToAction("NotificationRecipients");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddNotificationRecipient(AccountNotificationRecipientsViewModel viewModel)
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            if (ModelState.IsValid)
            {
                // Clean the passed parameters.
                string cleanTwitterHandle = Server.HtmlEncode(viewModel.TwitterHandle);
                string cleanDescription = Conversion.StringToSafeString(Server.HtmlEncode(viewModel.Description));

                IValidationResult validationResult = this.doctrineShipsServices.AddNotificationRecipient(accountId, cleanTwitterHandle, cleanDescription);

                // If the validationResult is not valid, something did not validate in the service layer.
                if (validationResult.IsValid)
                {
                    TempData["Status"] = "The notification recipient was successfully added.";
                }
                else
                {
                    TempData["Status"] = "Error: The notification recipient was not added, a validation error occured.<br /><b>Error Details: </b>";

                    foreach (var error in validationResult.Errors)
                    {
                        TempData["Status"] += error.Value + "<br />";
                    }
                }

                return RedirectToAction("NotificationRecipients");
            }
            else
            {
                // Re-populate the view model and return with any validation errors.
                viewModel.NotificationRecipients = this.doctrineShipsServices.GetNotificationRecipients(accountId);
                return View("~/Views/Account/NotificationRecipients.cshtml", viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateNotificationRecipients(AccountNotificationRecipientsViewModel viewModel)
        {
            if (viewModel != null)
            {
                // Create an Auto Mapper map between the notification recipient entity and the view model.
                Mapper.CreateMap<AccountNotificationRecipientsViewModel, NotificationRecipient>();

                // Convert the currently logged-in account id to an integer.
                viewModel.AccountId = Conversion.StringToInt32(User.Identity.Name);

                // Populate a notification recipient with automapper and pass it back to the service layer for update.
                NotificationRecipient notificationRecipient = Mapper.Map<AccountNotificationRecipientsViewModel, NotificationRecipient>(viewModel);
                IValidationResult validationResult = this.doctrineShipsServices.UpdateNotificationRecipient(notificationRecipient);

                // If the validationResult is not valid, something did not validate in the service layer.
                if (validationResult.IsValid)
                {
                    TempData["Status"] = "The notification recipient was successfully updated.";
                }
                else
                {
                    TempData["Status"] = "Error: The notification recipient was not updated, a validation error occured.<br /><b>Error Details: </b>";

                    foreach (var error in validationResult.Errors)
                    {
                        TempData["Status"] += error.Value + "<br />";
                    }
                }
            }

            return RedirectToAction("NotificationRecipients");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateShipFit(AccountShipFitsViewModel viewModel)
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            if (ModelState.IsValid)
            {
                // Create an Auto Mapper map between the ship fit entity and the view model.
                Mapper.CreateMap<AccountShipFitsViewModel, ShipFit>();

                // Sanitise the form values.
                viewModel.AccountId = accountId;
                viewModel.Name = Conversion.StringToSafeString(viewModel.Name);
                viewModel.Role = Conversion.StringToSafeString(viewModel.Role);
                viewModel.Notes = Conversion.StringToSafeString(viewModel.Notes);

                // Populate a ship fit with automapper and pass it back to the service layer for update.
                ShipFit shipFit = Mapper.Map<AccountShipFitsViewModel, ShipFit>(viewModel);
                IValidationResult validationResult = this.doctrineShipsServices.UpdateShipFit(shipFit);

                // If the validationResult is not valid, something did not validate in the service layer.
                if (validationResult.IsValid)
                {
                    TempData["Status"] = "The ship fit was successfully updated.";
                }
                else
                {
                    TempData["Status"] = "Error: The ship fit was not updated, a validation error occured.<br /><b>Error Details: </b>";

                    foreach (var error in validationResult.Errors)
                    {
                        TempData["Status"] += error.Value + "<br />";
                    }
                }

                return RedirectToAction("ShipFits");
            }
            else
            {
                // Re-populate the view model and return with any validation errors.
                viewModel.ShipFits = this.doctrineShipsServices.GetShipFitList(accountId);
                ViewBag.Status = "Error: The ship fit was not updated, a validation error occured.";
                return View("~/Views/Account/ShipFits.cshtml", viewModel);
            }
        }

        public ActionResult Doctrines()
        {
            AccountDoctrinesViewModel viewModel = new AccountDoctrinesViewModel();

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            viewModel.Doctrines = this.doctrineShipsServices.GetDoctrineList(accountId).OrderBy(x => x.DoctrineId);

            // Set the ViewBag to the TempData status value passed from the Add & Delete methods.
            ViewBag.Status = TempData["Status"];

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddDoctrine(AccountDoctrinesViewModel viewModel)
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            if (ModelState.IsValid)
            {
                // Create an Auto Mapper map between the ship fit entity and the view model.
                Mapper.CreateMap<AccountDoctrinesViewModel, Doctrine>();

                // Sanitise the form values.
                viewModel.AccountId = accountId;
                viewModel.Name = Conversion.StringToSafeString(viewModel.Name);
                viewModel.Description = Conversion.StringToSafeString(viewModel.Description);
                viewModel.ImageUrl = Server.HtmlEncode(viewModel.ImageUrl) ?? string.Empty;

                // Populate a ship fit with automapper and pass it back to the service layer for update.
                Doctrine doctrine = Mapper.Map<AccountDoctrinesViewModel, Doctrine>(viewModel);
                IValidationResult validationResult = this.doctrineShipsServices.AddDoctrine(doctrine);

                // If the validationResult is not valid, something did not validate in the service layer.
                if (validationResult.IsValid)
                {
                    TempData["Status"] = "The doctrine was successfully added.";
                }
                else
                {
                    TempData["Status"] = "Error: The doctrine was not added, a validation error occured.<br /><b>Error Details: </b>";

                    foreach (var error in validationResult.Errors)
                    {
                        TempData["Status"] += error.Value + "<br />";
                    }
                }

                return RedirectToAction("Doctrines");
            }
            else
            {
                // Re-populate the view model and return with any validation errors.
                viewModel.Doctrines = this.doctrineShipsServices.GetDoctrineList(accountId).OrderBy(x => x.DoctrineId);
                return View("~/Views/Account/Doctrines.cshtml", viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteDoctrine(AccountDoctrinesViewModel viewModel)
        {
            if (viewModel.RemoveList != null)
            {
                // Convert the currently logged-in account id to an integer.
                int accountId = Conversion.StringToInt32(User.Identity.Name);

                // Create a collection for the results of the delete operations.
                ICollection<bool> resultList = new List<bool>();

                foreach (var doctrineId in viewModel.RemoveList)
                {
                    resultList.Add(this.doctrineShipsServices.DeleteDoctrine(accountId, doctrineId));
                }

                // If any of the deletions failed, output an error message.
                if (resultList.Contains(false))
                {
                    TempData["Status"] = "Error: One or more doctrines were not removed.";
                }
                else
                {
                    TempData["Status"] = "All selected doctrines were successfully removed.";
                }
            }

            return RedirectToAction("Doctrines");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateDoctrine(AccountDoctrinesViewModel viewModel)
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            if (ModelState.IsValid)
            {
                // Create an Auto Mapper map between the doctrine entity and the view model.
                Mapper.CreateMap<AccountDoctrinesViewModel, Doctrine>();

                // Sanitise the form values.
                viewModel.AccountId = accountId;
                viewModel.Name = Conversion.StringToSafeString(viewModel.Name);
                viewModel.Description = Conversion.StringToSafeString(viewModel.Description);
                viewModel.ImageUrl = Server.HtmlEncode(viewModel.ImageUrl) ?? string.Empty;

                // Populate a doctrine with automapper and pass it back to the service layer for update.
                Doctrine doctrine = Mapper.Map<AccountDoctrinesViewModel, Doctrine>(viewModel);
                IValidationResult validationResult = this.doctrineShipsServices.UpdateDoctrine(doctrine);

                // If the validationResult is not valid, something did not validate in the service layer.
                if (validationResult.IsValid)
                {
                    TempData["Status"] = "The doctrine was successfully updated.";
                }
                else
                {
                    TempData["Status"] = "Error: The doctrine was not updated, a validation error occured.<br /><b>Error Details: </b>";

                    foreach (var error in validationResult.Errors)
                    {
                        TempData["Status"] += error.Value + "<br />";
                    }
                }

                return RedirectToAction("Doctrines");
            }
            else
            {
                // Re-populate the view model and return with any validation errors.
                viewModel.Doctrines = this.doctrineShipsServices.GetDoctrineList(accountId);
                ViewBag.Status = "Error: The doctrine was not updated, a validation error occured.";
                return View("~/Views/Account/Doctrines.cshtml", viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateDoctrineShipFits(AccountDoctrinesViewModel viewModel)
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            if (ModelState.IsValid)
            {
                IValidationResult validationResult = this.doctrineShipsServices.UpdateDoctrineShipFits(accountId, viewModel.DoctrineId, viewModel.DoctrineShipFitIds);

                // If the validationResult is not valid, something did not validate in the service layer.
                if (validationResult.IsValid)
                {
                    TempData["Status"] = "The doctrine ship fit list was successfully updated.";
                }
                else
                {
                    TempData["Status"] = "Error: The doctrine ship fit list was not updated, a validation error occured.<br /><b>Error Details: </b>";

                    foreach (var error in validationResult.Errors)
                    {
                        TempData["Status"] += error.Value + "<br />";
                    }
                }

                return RedirectToAction("Doctrines");
            }
            else
            {
                // Re-populate the view model and return with any validation errors.
                viewModel.Doctrines = this.doctrineShipsServices.GetDoctrineList(accountId);
                ViewBag.Status = "Error: The doctrine ship fit list was not updated, a validation error occured.";
                return View("~/Views/Account/Doctrines.cshtml", viewModel);
            }
        }
    }
}
