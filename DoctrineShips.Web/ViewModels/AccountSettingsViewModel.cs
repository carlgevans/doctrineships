namespace DoctrineShips.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using DoctrineShips.Entities;

    public class AccountSettingsViewModel
    {
        public int SettingProfileId { get; set; }
        public int AccountId { get; set; }

        public ICollection<SelectListItem> BrokerPercentages { get; set; }
        public ICollection<SelectListItem> SalesTaxPercentages { get; set; }
        public ICollection<SelectListItem> ContractMarkupPercentages { get; set; }

        [Required(ErrorMessage = "A Broker Percentage is required.")]
        [DisplayName("Broker Percentage")]
        [Range(0, 100, ErrorMessage = "That isn't a valid percentage.")]
        public double BrokerPercentage { get; set; }

        [Required(ErrorMessage = "A Sales Tax Percentage is required.")]
        [DisplayName("Sales Tax Percentage")]
        [Range(0, 100, ErrorMessage = "That isn't a valid percentage.")]
        public double SalesTaxPercentage { get; set; }

        [Required(ErrorMessage = "A Contract Markup Percentage is required.")]
        [DisplayName("Contract Markup Percentage")]
        [Range(0, 100, ErrorMessage = "That isn't a valid percentage.")]
        public double ContractMarkupPercentage { get; set; }

        [Required(ErrorMessage = "A Contract Broker Fee is required.")]
        [DisplayName("Contract Broker Fee")]
        [Range(0, Double.MaxValue, ErrorMessage = "Are you sure the contract broker fees are that high?")]
        public double ContractBrokerFee { get; set; }

        [Required(ErrorMessage = "A Shipping Cost (Per m3) is required.")]
        [DisplayName("Shipping Cost (Per m3)")]
        [Range(0, Double.MaxValue, ErrorMessage = "Are you sure the shipping cost is that high per m3?")]
        public double ShippingCostPerM3 { get; set; }

        [Required(ErrorMessage = "A Buy Station Id is required.")]
        [DisplayName("Buy Station Id")]
        [Range(0, Int32.MaxValue, ErrorMessage = "That does not look like a valid Buy Station Id.")]
        public int BuyStationId { get; set; }

        [Required(ErrorMessage = "A Sell Station Id is required.")]
        [DisplayName("Sell Station Id")]
        [Range(0, Int32.MaxValue, ErrorMessage = "That does not look like a valid Sell Station Id.")]
        public int SellStationId { get; set; }

        [Required(ErrorMessage = "An Alert Threshold is required.")]
        [DisplayName("Alert Threshold")]
        [Range(0, Int32.MaxValue, ErrorMessage = "That does not look like a valid Alert Threshold.")]
        public int AlertThreshold { get; set; }

        [Required(ErrorMessage = "A Twitter Handle is required. Leave it as @DoctrineShips if you do not wish to specify a handle.")]
        [DisplayName("Account Twitter Handle")]
        [RegularExpression("^@(\\w){1,15}$", ErrorMessage = "That does not look like a valid Twitter @Handle.")]
        public string TwitterHandle { get; set; }

        public AccountSettingsViewModel()
        {
            // Populate the dropdown lists.
            BrokerPercentages = new List<SelectListItem>();
            SalesTaxPercentages = new List<SelectListItem>();
            ContractMarkupPercentages = new List<SelectListItem>();

            BrokerPercentages.Add(new SelectListItem
            {
                Text = "Broker Relations 0 (1%)",
                Value = "1.01",
                Selected = true
            });

            BrokerPercentages.Add(new SelectListItem
            {
                Text = "Broker Relations 1 (0.95%)",
                Value = "1.0095"
            });

            BrokerPercentages.Add(new SelectListItem
            {
                Text = "Broker Relations 2 (0.90%)",
                Value = "1.0090"
            });

            BrokerPercentages.Add(new SelectListItem
            {
                Text = "Broker Relations 3 (0.85%)",
                Value = "1.0085"
            });

            BrokerPercentages.Add(new SelectListItem
            {
                Text = "Broker Relations 4 (0.80%)",
                Value = "1.008"
            });

            BrokerPercentages.Add(new SelectListItem
            {
                Text = "Broker Relations 5 (0.75%)",
                Value = "1.0075",
            });

            SalesTaxPercentages.Add(new SelectListItem
            {
                Text = "Accounting 0 (1.5%)",
                Value = "1.015",
                Selected = true
            });

            SalesTaxPercentages.Add(new SelectListItem
            {
                Text = "Accounting 1 (1.35%)",
                Value = "1.0135"
            });

            SalesTaxPercentages.Add(new SelectListItem
            {
                Text = "Accounting 2 (1.2%)",
                Value = "1.012"
            });

            SalesTaxPercentages.Add(new SelectListItem
            {
                Text = "Accounting 3 (1.05%)",
                Value = "1.0105"
            });

            SalesTaxPercentages.Add(new SelectListItem
            {
                Text = "Accounting 4 (0.9%)",
                Value = "1.009"
            });

            SalesTaxPercentages.Add(new SelectListItem
            {
                Text = "Accounting 5 (0.75%)",
                Value = "1.0075"
            });

            ContractMarkupPercentages.Add(new SelectListItem
            {
                Text = "Very Generous (10%)",
                Value = "1.1",
                Selected = true
            });

            ContractMarkupPercentages.Add(new SelectListItem
            {
                Text = "Generous (15%)",
                Value = "1.15"
            });

            ContractMarkupPercentages.Add(new SelectListItem
            {
                Text = "Not Too Bad (20%)",
                Value = "1.2"
            });

            ContractMarkupPercentages.Add(new SelectListItem
            {
                Text = "Getting Greedy (25%)",
                Value = "1.25"
            });

            ContractMarkupPercentages.Add(new SelectListItem
            {
                Text = "Greedy Bastard (30%)",
                Value = "1.3"
            });

            ContractMarkupPercentages.Add(new SelectListItem
            {
                Text = "Donald Trump (35%)",
                Value = "1.35"
            });

            ContractMarkupPercentages.Add(new SelectListItem
            {
                Text = "Enron (40%)",
                Value = "1.40"
            });

            ContractMarkupPercentages.Add(new SelectListItem
            {
                Text = "Screw Them All, Right? (45%)",
                Value = "1.45"
            });

            ContractMarkupPercentages.Add(new SelectListItem
            {
                Text = "Whoa! (50%)",
                Value = "1.5"
            });
        }
    }
}