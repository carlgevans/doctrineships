namespace DoctrineShips.Validation.Checks
{
    using System.Text.RegularExpressions;
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;

    internal sealed class AccountCheck
    {
        private readonly IDoctrineShipsRepository doctrineShipsRepository;

        internal AccountCheck(IDoctrineShipsRepository doctrineShipsRepository)
        {
            this.doctrineShipsRepository = doctrineShipsRepository;
        }

        internal IValidationResult Account(Account account)
        {
            IValidationResult validationResult = new ValidationResult();

            // Null checks.
            if (account.Description == string.Empty || account.Description == null)
            {
                validationResult.AddError("Description.Null", "Description can not be empty or null.");
            }
            else
            {
                // Range checks.
                if (account.Description.Length > 30)
                {
                    validationResult.AddError("Description.Length", "Description should be less than 30 characters in length.");
                }
            }

            return validationResult;
        }

        internal IValidationResult AccessCode(AccessCode accessCode)
        {
            IValidationResult validationResult = new ValidationResult();

            // Do not permit site admins to be added.
            if (accessCode.Role == Role.SiteAdmin)
            {
                validationResult.AddError("Role.Type", "A SiteAdmin may not be added.");
            }

            // Null checks.
            if (accessCode.Description == string.Empty || accessCode.Description == null)
            {
                validationResult.AddError("Description.Null", "Description can not be empty or null.");
            }
            else
            {
                // Range checks.
                if (accessCode.Description.Length > 30)
                {
                    validationResult.AddError("Description.Length", "Description should be less than 30 characters in length.");
                }
            }

            return validationResult;
        }

        internal IValidationResult SettingProfile(SettingProfile settingProfile)
        {
            IValidationResult validationResult = new ValidationResult();

            var existingSettingProfile = this.doctrineShipsRepository.GetSettingProfileReadOnly(settingProfile.SettingProfileId);

            // Does the setting profile exist in the database?
            if (existingSettingProfile == null)
            {
                validationResult.AddError("SettingProfile.Null", "The setting profile being modified does not exist in the database.");
            }
            else
            {
                // Does the setting profile being modified belong to the requesting account?
                if (existingSettingProfile.AccountId != settingProfile.AccountId)
                {
                    validationResult.AddError("SettingProfile.Permission", "The setting profile being modified does not belong to the requesting account.");
                }
            }

            // Null checks.
            if (settingProfile.TwitterHandle == null)
            {
                validationResult.AddError("TwitterHandle.Null", "TwitterHandle cannot be null.");
            }

            // Regex checks.
            if (settingProfile.TwitterHandle != null)
            {
                if (!Regex.Match(settingProfile.TwitterHandle, "^@(\\w){1,15}$").Success)
                {
                    validationResult.AddError("TwitterHandle.Format", "Invalid twitter handle.");
                }
            }

            // Range checks.
            if (settingProfile.BrokerPercentage < 0 || settingProfile.BrokerPercentage > 100)
            {
                validationResult.AddError("BrokerPercentage.Range", "BrokerPercentage is outside of expected ranges.");
            }

            if (settingProfile.SalesTaxPercentage < 0 || settingProfile.SalesTaxPercentage > 100)
            {
                validationResult.AddError("SalesTaxPercentage.Range", "SalesTaxPercentage is outside of expected ranges.");
            }

            if (settingProfile.ContractMarkupPercentage < 0 || settingProfile.ContractMarkupPercentage > 100)
            {
                validationResult.AddError("ContractMarkupPercentage.Range", "ContractMarkupPercentage is outside of expected ranges.");
            }

            if (settingProfile.ContractBrokerFee < 0 || settingProfile.ContractBrokerFee > double.MaxValue)
            {
                validationResult.AddError("ContractBrokerFee.Range", "ContractBrokerFee is outside of expected ranges.");
            }

            if (settingProfile.ShippingCostPerM3 < 0 || settingProfile.ShippingCostPerM3 > double.MaxValue)
            {
                validationResult.AddError("ShippingCostPerM3.Range", "ShippingCostPerM3 is outside of expected ranges.");
            }

            if (settingProfile.BuyStationId < 0 || settingProfile.BuyStationId > int.MaxValue)
            {
                validationResult.AddError("BuyStationId.Range", "BuyStationId is outside of expected ranges.");
            }

            if (settingProfile.SellStationId < 0 || settingProfile.SellStationId > int.MaxValue)
            {
                validationResult.AddError("SellStationId.Range", "SellStationId is outside of expected ranges.");
            }

            if (settingProfile.AlertThreshold < 0 || settingProfile.AlertThreshold > int.MaxValue)
            {
                validationResult.AddError("AlertThreshold.Range", "AlertThreshold is outside of expected ranges.");
            }

            return validationResult;
        }

        internal IValidationResult NotificationRecipient(NotificationRecipient notificationRecipient)
        {
            IValidationResult validationResult = new ValidationResult();

            // Null checks.
            if (notificationRecipient.TwitterHandle == null)
            {
                validationResult.AddError("TwitterHandle.Null", "TwitterHandle cannot be null.");
            }

            if (notificationRecipient.Description == null || notificationRecipient.Description == string.Empty)
            {
                validationResult.AddError("Description.Null", "Description cannot be null or an empty string.");
            }

            // Regex checks.
            if (notificationRecipient.TwitterHandle != null)
            {
                if (!Regex.Match(notificationRecipient.TwitterHandle, "^@(\\w){1,15}$").Success)
                {
                    validationResult.AddError("TwitterHandle.Format", "Invalid twitter handle.");
                }
            }

            // Range checks.
            if (notificationRecipient.AlertIntervalHours < 1 || notificationRecipient.AlertIntervalHours > 168)
            {
                validationResult.AddError("AlertIntervalHours.Range", "AlertIntervalHours is outside of expected ranges. AlertIntervalHours should be between 1 and 168 hours (one week).");
            }

            return validationResult;
        }
    }
}
