namespace DoctrineShips.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using AutoMapper;
    using DoctrineShips.Entities;
    using DoctrineShips.Service;
    using DoctrineShips.Validation;
    using DoctrineShips.Web.ViewModels;
    using Tools;
    
    [Authorize(Roles = "SiteAdmin")]
    public class SiteController : Controller
    {
        private readonly IDoctrineShipsServices doctrineShipsServices;
        private readonly string websiteDomain;

        public SiteController(IDoctrineShipsServices doctrineShipsServices)
        {
            this.doctrineShipsServices = doctrineShipsServices;
            this.websiteDomain = WebConfigurationManager.AppSettings["WebsiteDomain"];
        }

        public ActionResult Accounts()
        {
            SiteAccountsViewModel viewModel = new SiteAccountsViewModel();

            viewModel.Accounts = this.doctrineShipsServices.GetAccounts();
            
            // Populate the subscription plan selection list.
            viewModel.SubscriptionPlans = this.doctrineShipsServices.GetSubscriptionPlans()
                                         .Select(x => new SelectListItem
                                         {
                                             Value = x.SubscriptionPlanId.ToString(),
                                             Text = x.Name
                                         })
                                         .ToList();

            // Set the ViewBag to the TempData status value passed from the Add & Delete methods.
            ViewBag.Status = TempData["Status"];

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddAccount(SiteAccountsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string generatedKey = string.Empty;
                int newAccountId = 0;

                // Clean the passed description.
                string cleanDescription = Server.HtmlEncode(viewModel.Description);

                IValidationResult validationResult = this.doctrineShipsServices.AddAccount(cleanDescription, viewModel.SubscriptionPlanId, out generatedKey, out newAccountId);

                // If the validationResult is not valid, something did not validate in the service layer.
                if (validationResult.IsValid)
                {
                    TempData["Status"] = "The account was successfully added.";
                    if (generatedKey != string.Empty && newAccountId != 0)
                    {
                        // Assign the new key to TempData to be passed to the accounts view.
                        string authUrl = this.websiteDomain + "/Auth/" + newAccountId + "/" + generatedKey;
                        TempData["Status"] += " The account admin auth url is: <a href=\"" + authUrl + "\">" + authUrl + "</a>";
                    }
                }
                else
                {
                    TempData["Status"] = "Error: The account was not added, a validation error occured.<br /><b>Error Details: </b>";

                    foreach (var error in validationResult.Errors)
                    {
                        TempData["Status"] += error.Value + "<br />";
                    }
                }

                return RedirectToAction("Accounts");
            }
            else
            {
                // Re-populate the view model and return with any validation errors.
                viewModel.Accounts = this.doctrineShipsServices.GetAccounts();
                
                // Re-populate the subscription plan selection list.
                viewModel.SubscriptionPlans = this.doctrineShipsServices.GetSubscriptionPlans()
                                             .Select(x => new SelectListItem
                                             {
                                                 Value = x.SubscriptionPlanId.ToString(),
                                                 Text = x.Name
                                             })
                                             .ToList();

                return View("~/Views/Site/Accounts.cshtml", viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteAccount(SiteAccountsViewModel viewModel)
        {
            if (viewModel.RemoveList != null)
            {
                IValidationResult validationResults = new ValidationResult();

                foreach (var accountId in viewModel.RemoveList)
                {
                    // Delete the current account and merge any validation errors in to a validation result object.
                    validationResults.Merge(this.doctrineShipsServices.DeleteAccount(accountId));
                }

                // If the validationResult is not valid, something did not validate in the service layer.
                if (validationResults.IsValid)
                {
                    TempData["Status"] = "All selected accounts were successfully removed.";
                }
                else
                {
                    TempData["Status"] = "Error: One or more errors were detected while removing the selected accounts.<br /><b>Error Details: </b>";

                    foreach (var error in validationResults.Errors)
                    {
                        TempData["Status"] += error.Value + "<br />";
                    }
                }
            }

            return RedirectToAction("Accounts");
        }

        public ActionResult Customers()
        {
            SiteCustomersViewModel viewModel = new SiteCustomersViewModel();

            viewModel.Customers = this.doctrineShipsServices.GetCustomers();

            // Set the ViewBag to the TempData status value passed from the Add & Delete methods.
            ViewBag.Status = TempData["Status"];

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddCustomer(SiteCustomersViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                IValidationResult validationResult = this.doctrineShipsServices.AddCustomer(viewModel.Name, viewModel.Type);

                // If the validationResult is not valid, something did not validate in the service layer.
                if (validationResult.IsValid)
                {
                    TempData["Status"] = "The customer was successfully added.";
                }
                else
                {
                    TempData["Status"] = "Error: The customer was not added, a validation error occured.<br /><b>Error Details: </b>";

                    foreach (var error in validationResult.Errors)
                    {
                        TempData["Status"] += error.Value + "<br />";
                    }
                }

                return RedirectToAction("Customers");
            }
            else
            {
                // Re-populate the view model and return with any validation errors.
                viewModel.Customers = this.doctrineShipsServices.GetCustomers();
                return View("~/Views/Site/Customers.cshtml", viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteCustomer(SiteCustomersViewModel viewModel)
        {
            if (viewModel.RemoveList != null)
            {
                // Create a collection for the results of the delete operations.
                ICollection<bool> resultList = new List<bool>();

                foreach (var customerId in viewModel.RemoveList)
                {
                    resultList.Add(this.doctrineShipsServices.DeleteCustomer(customerId));
                }

                // If any of the deletions failed, output an error message.
                if (resultList.Contains(false))
                {
                    TempData["Status"] = "Error: One or more customers were not removed.";
                }
                else
                {
                    TempData["Status"] = "All selected customers were successfully removed.";
                }
            }

            return RedirectToAction("Customers");
        }

        public ActionResult SubscriptionPlans()
        {
            SiteSubscriptionPlansViewModel viewModel = new SiteSubscriptionPlansViewModel();

            viewModel.SubscriptionPlans = this.doctrineShipsServices.GetSubscriptionPlans();

            // Set the ViewBag to the TempData status value passed from the add and remove methods.
            ViewBag.Status = TempData["Status"];

            return View(viewModel);
        }

        public ActionResult Log()
        {
            SiteLogViewModel viewModel = new SiteLogViewModel();

            TimeSpan logPeriod = TimeSpan.FromDays(7);

            viewModel.LogMessages = this.doctrineShipsServices.GetLogMessages(logPeriod);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ClearLog()
        {
            this.doctrineShipsServices.ClearLog();

            return RedirectToAction("Log");
        }

        public ActionResult UpdateAccountState(string accountId, string isActive)
        {
            // Clean the controller parameters.
            int cleanAccountId = Conversion.StringToInt32(Server.HtmlEncode(accountId));
            bool cleanIsActive = Conversion.StringToBool(Server.HtmlEncode(isActive));

            // Update the state.
            this.doctrineShipsServices.UpdateAccountState(cleanAccountId, cleanIsActive);

            return RedirectToAction("Accounts");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddSubscriptionPlan(SiteSubscriptionPlansViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Create an Auto Mapper map between the subscription plan entity and the view model. Ignore the id as this will be auto-generated.
                Mapper.CreateMap<SiteSubscriptionPlansViewModel, SubscriptionPlan>().ForMember(x => x.SubscriptionPlanId, opt => opt.Ignore());

                // Populate a subscription plan with automapper and pass it back to the service layer for addition.
                SubscriptionPlan subscriptionPlan = Mapper.Map<SiteSubscriptionPlansViewModel, SubscriptionPlan>(viewModel);
                IValidationResult validationResult = this.doctrineShipsServices.AddSubscriptionPlan(subscriptionPlan);

                // If the validationResult is not valid, something did not validate in the service layer.
                if (validationResult.IsValid)
                {
                    TempData["Status"] = "The subscription plan was successfully added.";
                }
                else
                {
                    TempData["Status"] = "Error: The subscription plan was not added, a validation error occured.<br /><b>Error Details: </b>";

                    foreach (var error in validationResult.Errors)
                    {
                        TempData["Status"] += error.Value + "<br />";
                    }
                }

                return RedirectToAction("SubscriptionPlans");
            }
            else
            {
                // Re-populate the view model and return with any validation errors.
                viewModel.SubscriptionPlans = this.doctrineShipsServices.GetSubscriptionPlans();
                return View("~/Views/Site/SubscriptionPlans.cshtml", viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteSubscriptionPlan(SiteSubscriptionPlansViewModel viewModel)
        {
            if (viewModel.RemoveList != null)
            {
                // Create a collection for the results of the delete operations.
                ICollection<bool> resultList = new List<bool>();

                foreach (var subscriptionPlanId in viewModel.RemoveList)
                {
                    resultList.Add(this.doctrineShipsServices.DeleteSubscriptionPlan(subscriptionPlanId));
                }

                // If any of the deletions failed, output an error message.
                if (resultList.Contains(false))
                {
                    TempData["Status"] = "Error: One or more subscription plans were not removed. Are any of the plans still in use by accounts?";
                }
                else
                {
                    TempData["Status"] = "All selected subscription plans were successfully removed.";
                }
            }

            return RedirectToAction("SubscriptionPlans");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateAccountSubscriptionPlan(SiteAccountsViewModel viewModel)
        {
            if (viewModel != null)
            {
                // Update the subscription plan.
                IValidationResult validationResult = this.doctrineShipsServices.UpdateAccountSubscriptionPlan(viewModel.AccountId, viewModel.SubscriptionPlanId);

                // If the validationResult is not valid, something did not validate in the service layer.
                if (validationResult.IsValid)
                {
                    TempData["Status"] = "The subscription plan was successfully updated.";
                }
                else
                {
                    TempData["Status"] = "Error: The subscription plan was not updated, a validation error occured.<br /><b>Error Details: </b>";

                    foreach (var error in validationResult.Errors)
                    {
                        TempData["Status"] += error.Value + "<br />";
                    }
                }
            }

            return RedirectToAction("Accounts");
        }
    }
}
