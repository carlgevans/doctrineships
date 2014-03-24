namespace DoctrineShips.Service.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using AutoMapper;
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;
    using DoctrineShips.Validation;
    using EveData;
    using EveData.Entities;
    using Tools;

    /// <summary>
    /// A class dealing with Doctrine Ships accounts.
    /// </summary>
    internal sealed class AccountManager
    {
        private readonly IDoctrineShipsRepository doctrineShipsRepository;
        private readonly IEveDataSource eveDataSource;
        private readonly IDoctrineShipsValidation doctrineShipsValidation;
        private readonly ISystemLogger logger;

        /// <summary>
        /// Initialises a new instance of an Account Manager.
        /// </summary>
        /// <param name="doctrineShipsRepository">An IDoctrineShipsRepository instance.</param>
        /// <param name="eveDataSource">An IEveDataSource instance.</param>
        /// <param name="doctrineShipsValidation">An IDoctrineShipsValidation instance.</param>
        /// <param name="logger">An ISystemLogger logger instance.</param>
        internal AccountManager(IDoctrineShipsRepository doctrineShipsRepository, IEveDataSource eveDataSource, IDoctrineShipsValidation doctrineShipsValidation, ISystemLogger logger)
        {
            this.doctrineShipsRepository = doctrineShipsRepository;
            this.eveDataSource = eveDataSource;
            this.doctrineShipsValidation = doctrineShipsValidation;
            this.logger = logger;
        }

        /// <summary>
        /// Fetches and returns a Doctrine Ships account.
        /// </summary>
        /// <param name="accountId">The id of the account for which an account object should be returned.</param>
        /// <returns>An account object.</returns>
        internal Account GetAccount(int accountId)
        {
            return this.doctrineShipsRepository.GetAccount(accountId);
        }

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships accounts.
        /// </summary>
        /// <returns>A list of account objects.</returns>
        internal IEnumerable<Account> GetAccounts()
        {
            return this.doctrineShipsRepository.GetAccounts();
        }

        /// <summary>
        /// <para>Adds an account with a default setting profile and an account admin access code.</para>
        /// </summary>
        /// <param name="description">A short description for the new account.</param>
        /// <param name="generatedKey">An out string parameter containing the account admin key or an emptry string on failure.</param>
        /// <param name="newAccountId">An out int parameter containing the new account id or 0 on failure.</param>
        /// <returns>Returns a validation result object.</returns>
        internal IValidationResult AddAccount(string description, out string generatedKey, out int newAccountId)
        {
            generatedKey = string.Empty;
            newAccountId = 0;
            IValidationResult validationResult = new ValidationResult();

            // Populate a new account object.
            Account newAccount = new Account();

            // Populate the remaining properties.
            newAccount.Description = description;
            newAccount.DateCreated = DateTime.UtcNow;
            newAccount.IsActive = true;
            newAccount.SettingProfileId = 0;

            // Validate the new account.
            validationResult = this.doctrineShipsValidation.Account(newAccount);
            if (validationResult.IsValid == true)
            {
                // Add the new account and read it back to get the account id.
                newAccount = this.doctrineShipsRepository.CreateAccount(newAccount);
                this.doctrineShipsRepository.Save();

                // Assign the new account id to the passed out parameter.
                newAccountId = newAccount.AccountId;

                // Add a default account admin access code for the account and capture the generated key.
                generatedKey = this.AddAccessCode(newAccount.AccountId, "Default Account Admin", Role.Admin);

                // Create a default setting profile with the new account Id and assign it to the account.
                newAccount.SettingProfileId = this.AddDefaultSettingProfile(newAccount.AccountId);
                this.doctrineShipsRepository.UpdateAccount(newAccount);
                this.doctrineShipsRepository.Save();

                // Log the addition.
                logger.LogMessage("Account '" + newAccount.Description + "' Successfully Added.", 2, "Message", MethodBase.GetCurrentMethod().Name);
            }

            return validationResult;
        }

        /// <summary>
        /// Deletes an account and all access codes, settings profiles, ship fits, sales agents and contracts.
        /// </summary>
        /// <param name="accountId">The account Id being deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        internal bool DeleteAccount(int accountId)
        {
            Account account = this.doctrineShipsRepository.GetAccount(accountId);

            if (account != null)
            {
                // Delete the account and log the event.
                this.doctrineShipsRepository.DeleteAccount(account.AccountId);
                this.doctrineShipsRepository.Save();
                logger.LogMessage("Account '" + account.Description + "' Successfully Deleted.", 1, "Message", MethodBase.GetCurrentMethod().Name);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Authenticate an account id and key combination.
        /// </summary>
        /// <param name="accountId">The account id to be authenticated.</param>
        /// <param name="key">The passcode to be authenticated against the account id.</param>
        /// <param name="bypassAccountChecks">Bypass account id checking so that any account id may be used.</param>
        /// <returns>Returns a role. The role will be 'None' if authentication has failed.</returns>
        internal Role Authenticate(int accountId, string key, bool bypassAccountChecks = false)
        {
            // Bypass account id checking so that a site admin can log in as any account id.
            if (bypassAccountChecks == true)
            {
                var siteAdmins = this.doctrineShipsRepository.GetSiteAdminAccessCodes();

                if (siteAdmins != null)
                {
                    // Compare each site admin access code returned.
                    foreach (var accessCode in siteAdmins)
                    {
                        var hashedKey = Security.GenerateHash(key, accessCode.Salt);

                        // Compare the hashed key to the hash retrieved from the database.
                        if (hashedKey == accessCode.Key)
                        {
                            // Update the access code LastLogin timestamp, save and log.
                            accessCode.LastLogin = DateTime.UtcNow;
                            this.doctrineShipsRepository.UpdateAccessCode(accessCode);
                            this.doctrineShipsRepository.Save();

                            logger.LogMessage("Authentication Success (Bypass Account Check Mode) For Account Id: " + accountId + " With Code: '" + accessCode.Description + "'.", 2, "Message", MethodBase.GetCurrentMethod().Name);
                            return accessCode.Role;
                        }
                    }
                }
            }
            else
            {
                // Retrieve the account (which also includes its access codes).
                var account = this.doctrineShipsRepository.GetAccount(accountId);

                if (account != null)
                {
                    // Is the account active?
                    if (account.IsActive == true)
                    {
                        // Are there any access codes for the account?
                        if (account.AccessCodes != null && account.AccessCodes.Count != 0)
                        {
                            // Compare each access code returned for this account.
                            foreach (var accessCode in account.AccessCodes)
                            {
                                // Is this particular access key active?
                                if (accessCode.IsActive == true)
                                {
                                    var hashedKey = Security.GenerateHash(key, accessCode.Salt);

                                    // Compare the hashed key to the hash retrieved from the database.
                                    if (hashedKey == accessCode.Key)
                                    {
                                        // Update the access code LastLogin timestamp, save and log.
                                        accessCode.LastLogin = DateTime.UtcNow;
                                        this.doctrineShipsRepository.UpdateAccessCode(accessCode);
                                        this.doctrineShipsRepository.Save();

                                        logger.LogMessage("Authentication Success For Account Id: " + accountId + " With Code: '" + accessCode.Description +"'.", 2, "Message", MethodBase.GetCurrentMethod().Name);
                                        return accessCode.Role;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            logger.LogMessage("Authentication Attempt Failed For Account Id: " + accountId, 0, "Message", MethodBase.GetCurrentMethod().Name);
            return Role.None;
        }

        /// <summary>
        /// Get a list of account access codes.
        /// </summary>
        /// <param name="accountId">The id of the account for which an access code list should be returned.</param>
        /// <returns>Returns a list of access codes.</returns>
        internal IEnumerable<AccessCode> GetAccessCodes(int accountId)
        {            
            return this.doctrineShipsRepository.GetAccessCodesForAccount(accountId);
        }

        /// <summary>
        /// Fetches and returns a setting profile for a particular account.
        /// </summary>
        /// <param name="accountId">The id of the account for which a setting profile object should be returned.</param>
        /// <returns>A setting profile object.</returns>
        internal SettingProfile GetAccountSettingProfile(int accountId)
        {
            var settingProfile = this.doctrineShipsRepository.GetSettingProfileForAccount(accountId);

            if (settingProfile == null)
            {
                // If a setting profile is not found for the account, log it and throw an exception.
                logger.LogMessage("A Setting Profile Was Not Found For Account Id: " + accountId, 0, "Message", MethodBase.GetCurrentMethod().Name);
                throw new ArgumentException("A Setting Profile Was Not Found For Account Id: " + accountId);
            }

            return settingProfile;
        }

        /// <summary>
        /// Adds a default setting profile to an account. This method does not commit to the database itself.
        /// </summary>
        /// <param name="accountId">The id of the account for which a setting profile should be generated.</param>
        /// <returns>The new setting profile id or 0 if an error occurs.</returns>
        internal int AddDefaultSettingProfile(int accountId)
        {
            int settingProfileId = 0;

            // Fetch the default setting profile (Id 0).
            var defaultSettingProfile = this.doctrineShipsRepository.GetSettingProfileReadOnly(0);

            if (defaultSettingProfile != null)
            {
                // Create an Auto Mapper map between setting profile entities, ignoring the primary key.
                Mapper.CreateMap<SettingProfile, SettingProfile>()
                                        .ForMember(x => x.SettingProfileId, opt => opt.Ignore())
                                        .ForMember(x => x.AccountId, opt => opt.Ignore());

                // Assign the default setting profile to a new setting profile object.
                SettingProfile newSettingProfile = Mapper.Map<SettingProfile, SettingProfile>(defaultSettingProfile);

                // Assign the passed account id to the new object.
                newSettingProfile.AccountId = accountId;

                // Add the new setting profile and read it back to capture the new auto-generated id.
                newSettingProfile = this.doctrineShipsRepository.CreateSettingProfile(newSettingProfile);

                // Assign the new setting profile id as the return value.
                settingProfileId = newSettingProfile.SettingProfileId;
            }
            else
            {
                // If a default setting profile is not found, log it and throw an exception.
                logger.LogMessage("A Default Setting Profile Was Not Found.", 0, "Message", MethodBase.GetCurrentMethod().Name);
                throw new ArgumentException("A Default Setting Profile Was Not Found.");
            }

            return settingProfileId;
        }

        /// <summary>
        /// <para>Deletes a setting profile from an accountId and a settingProfileId.</para>
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the setting profile being deleted.</param>
        /// <param name="settingProfileId">The Id of the setting profile to be deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        internal bool DeleteSettingProfile(int accountId, int settingProfileId)
        {
            SettingProfile settingProfile = this.doctrineShipsRepository.GetSettingProfile(settingProfileId);

            if (settingProfile != null)
            {
                // If the account Id matches the account Id of the setting profile being deleted, continue.
                if (accountId == settingProfile.AccountId)
                {
                    // Delete the setting profile and log the event.
                    this.doctrineShipsRepository.DeleteSettingProfile(settingProfile.SettingProfileId);
                    this.doctrineShipsRepository.Save();
                    logger.LogMessage("Setting Profile '" + settingProfile.SettingProfileId + "' Successfully Deleted For Account Id: " + settingProfile.AccountId, 1, "Message", MethodBase.GetCurrentMethod().Name);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Updates a setting profile for a particular account.
        /// </summary>
        /// <param name="settingProfile">A setting profile object to be updated.</param>
        /// <returns>Returns true if the update was successful or false if not.</returns>
        internal bool UpdateSettingProfile(SettingProfile settingProfile)
        {
            // Validate the setting profile updates.
            if (this.doctrineShipsValidation.SettingProfile(settingProfile).IsValid == true)
            {
                // Update the existing record, save and log.
                this.doctrineShipsRepository.UpdateSettingProfile(settingProfile);
                this.doctrineShipsRepository.Save();
                logger.LogMessage("Setting Profile Updated For Account Id: " + settingProfile.AccountId, 2, "Message", MethodBase.GetCurrentMethod().Name);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Add an access key to an account.
        /// </summary>
        /// <param name="accountId">The id of the account for which an access code should be added.</param>
        /// <param name="description">A short description for the new access code.</param>
        /// <param name="role">The role that.</param>
        /// <returns>Returns a randomly generated key.</returns>
        internal string AddAccessCode(int accountId, string description, Role role)
        {
            AccessCode accessCode = null;
            string generatedKey = string.Empty;

            // Retrieve the account.
            var account = this.doctrineShipsRepository.GetAccount(accountId);

            // Does the account exist?
            if (account != null)
            {
                accessCode = new AccessCode();

                // If this is a user account, use a shorter key.
                if (role == Role.User)
                {
                    generatedKey = Security.GenerateRandomString(6);
                }
                else
                {
                    generatedKey = Security.GenerateRandomString(16);
                }

                accessCode.AccountId = accountId;
                accessCode.Role = role;
                accessCode.Description = description;
                accessCode.Salt = Security.GenerateSalt(128);
                accessCode.Key = Security.GenerateHash(generatedKey, accessCode.Salt);
                accessCode.LastLogin = DateTime.UtcNow;
                accessCode.DateCreated = DateTime.UtcNow;
                accessCode.IsActive = true;

                // Validate the new access code.
                if (this.doctrineShipsValidation.AccessCode(accessCode).IsValid == true)
                {
                    // Add the new access code and read it back.
                    accessCode = this.doctrineShipsRepository.CreateAccessCode(accessCode);
                    this.doctrineShipsRepository.Save();
                    logger.LogMessage("Access Code (Role: " + accessCode.Role + ") Added For Account Id: " + accessCode.AccountId, 2, "Message", MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    generatedKey = string.Empty;
                }
            }

            return generatedKey;
        }

        /// <summary>
        /// <para>Deletes an access code from an accountId and a accessCodeId.</para>
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the access code being deleted.</param>
        /// <param name="accessCode">The Id of the access code to be deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        internal bool DeleteAccessCode(int accountId, int accessCodeId)
        {
            AccessCode accessCode = this.doctrineShipsRepository.GetAccessCode(accessCodeId);

            if (accessCode != null)
            {
                // If the code being deleted is a Site Admin, do not continue.
                if (accessCode.Role != Role.SiteAdmin)
                {
                    // If the account Id matches the account Id of the access code being deleted, continue.
                    if (accountId == accessCode.AccountId)
                    {
                        // Delete the access code and log the event.
                        this.doctrineShipsRepository.DeleteAccessCode(accessCode.AccessCodeId);
                        this.doctrineShipsRepository.Save();
                        logger.LogMessage("Access Code '" + accessCode.Description + "' Successfully Deleted For Account Id: " + accessCode.AccountId, 1, "Message", MethodBase.GetCurrentMethod().Name);

                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the state of an access code.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the access code being changed.</param>
        /// <param name="accessCodeId">The id of the access code to be changed.</param>
        /// <param name="isActive">The required boolean state.</param>
        /// <returns>Returns true if the change was successful or false if not.</returns>
        internal bool UpdateAccessCodeState(int accountId, int accessCodeId, bool isActive)
        {
            AccessCode accessCode = this.doctrineShipsRepository.GetAccessCode(accessCodeId);

            if (accessCode != null)
            {
                // If the code being changed is a Site Admin, do not continue.
                if (accessCode.Role != Role.SiteAdmin)
                {
                    // If the account Id matches the account Id of the access code being changed, continue.
                    if (accountId == accessCode.AccountId)
                    {
                        // Change the state of the access code and log the event.
                        accessCode.IsActive = isActive;
                        this.doctrineShipsRepository.UpdateAccessCode(accessCode);
                        this.doctrineShipsRepository.Save();

                        if (isActive == true)
                        {
                            logger.LogMessage("Access Code '" + accessCode.Description + "' Successfully Enabled For Account Id: " + accessCode.AccountId, 2, "Message", MethodBase.GetCurrentMethod().Name);
                        }
                        else 
                        { 
                            logger.LogMessage("Access Code '" + accessCode.Description + "' Successfully Disabled For Account Id: " + accessCode.AccountId, 2, "Message", MethodBase.GetCurrentMethod().Name); 
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the state of an account.
        /// </summary>
        /// <param name="accountId">The id of the account to be changed.</param>
        /// <param name="isActive">The required boolean state.</param>
        /// <returns>Returns true if the change was successful or false if not.</returns>
        internal bool UpdateAccountState(int accountId, bool isActive)
        {
            Account account = this.doctrineShipsRepository.GetAccount(accountId);

            if (account != null)
            {
                // Change the state of the account and log the event.
                account.IsActive = isActive;
                this.doctrineShipsRepository.UpdateAccount(account);
                this.doctrineShipsRepository.Save();

                if (isActive == true)
                {
                    logger.LogMessage("Account '" + account.Description + "' Successfully Enabled.", 2, "Message", MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    logger.LogMessage("Account '" + account.Description + "' Successfully Disabled.", 2, "Message", MethodBase.GetCurrentMethod().Name);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships notification recipients for a particular account.
        /// </summary>
        /// <param name="accountId">The account for which notification recipients should be returned.</param>
        /// <returns>A list of notification recipients objects.</returns>
        internal IEnumerable<NotificationRecipient> GetNotificationRecipients(int accountId)
        {
            return this.doctrineShipsRepository.GetNotificationRecipients(accountId);
        }

        /// <summary>
        /// <para>Deletes a notification recipient from an accountId and a notificationRecipientId.</para>
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the notification recipient being deleted.</param>
        /// <param name="notificationRecipientId">The Id of the notification recipient to be deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        internal bool DeleteNotificationRecipient(int accountId, int notificationRecipientId)
        {
            NotificationRecipient notificationRecipient = this.doctrineShipsRepository.GetNotificationRecipient(notificationRecipientId);

            if (notificationRecipient != null)
            {
                // If the account Id matches the account Id of the notification recipient being deleted, continue.
                if (accountId == notificationRecipient.AccountId)
                {
                    // Delete the notification recipient and log the event.
                    this.doctrineShipsRepository.DeleteNotificationRecipient(notificationRecipient.NotificationRecipientId);
                    this.doctrineShipsRepository.Save();
                    logger.LogMessage("Notification Recipient '" + notificationRecipient.Description + "' Successfully Deleted For Account Id: " + notificationRecipient.AccountId, 1, "Message", MethodBase.GetCurrentMethod().Name);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// <para>Adds a notification recipient for an account.</para>
        /// </summary>
        /// <param name="accountId">The id of the account for which a recipient should be added.</param>
        /// <param name="twitterHandle">The twitter handle for the recipient.</param>
        /// <param name="description">A short description for the recipient.</param>
        /// <returns>Returns a validation result object.</returns>
        internal IValidationResult AddNotificationRecipient(int accountId, string twitterHandle, string description)
        {
            IValidationResult validationResult = new ValidationResult();

            // Populate a new account object.
            NotificationRecipient newNotificationRecipient = new NotificationRecipient();

            // Populate the remaining properties.
            newNotificationRecipient.AccountId = accountId;
            newNotificationRecipient.TwitterHandle = twitterHandle;
            newNotificationRecipient.Description = description;
            newNotificationRecipient.ReceivesDailySummary = true;
            newNotificationRecipient.ReceivesAlerts = true;
            newNotificationRecipient.AlertIntervalHours = 12;
            newNotificationRecipient.Method = NotificationMethod.DirectMessage;
            newNotificationRecipient.LastAlert = DateTime.UtcNow;
            
            // Validate the new notification recipient.
            validationResult = this.doctrineShipsValidation.NotificationRecipient(newNotificationRecipient);
            if (validationResult.IsValid == true)
            {
                // Add the new notification recipient.
                this.doctrineShipsRepository.CreateNotificationRecipient(newNotificationRecipient);
                this.doctrineShipsRepository.Save();

                // Log the addition.
                logger.LogMessage("Notification Recipient '" + newNotificationRecipient.Description + "' Successfully Added For Account Id: " + newNotificationRecipient.AccountId, 2, "Message", MethodBase.GetCurrentMethod().Name);
            }

            return validationResult;
        }

        /// <summary>
        /// Updates a notification recipient for a particular account.
        /// </summary>
        /// <param name="notificationRecipient">A partially populated notification recipient object to be updated.</param>
        /// <returns>Returns a validation result object.</returns>
        internal IValidationResult UpdateNotificationRecipient(NotificationRecipient notificationRecipient)
        {
            IValidationResult validationResult = new ValidationResult();

            var existingNotificationRecipient = this.doctrineShipsRepository.GetNotificationRecipient(notificationRecipient.NotificationRecipientId);

            if (existingNotificationRecipient != null)
            {
                if (existingNotificationRecipient.AccountId != notificationRecipient.AccountId)
                {
                    validationResult.AddError("NotificationRecipient.Permission", "The notification recipient being modified does not belong to the requesting account.");
                }
                else
                {
                    // Map the updates to the existing notification recipient.
                    existingNotificationRecipient.AlertIntervalHours = notificationRecipient.AlertIntervalHours;
                    existingNotificationRecipient.ReceivesAlerts = notificationRecipient.ReceivesAlerts;
                    existingNotificationRecipient.ReceivesDailySummary = notificationRecipient.ReceivesDailySummary;

                    // Validate the notification recipient updates.
                    validationResult = this.doctrineShipsValidation.NotificationRecipient(existingNotificationRecipient);
                    if (validationResult.IsValid == true)
                    {
                        // Update the existing record, save and log.
                        this.doctrineShipsRepository.UpdateNotificationRecipient(existingNotificationRecipient);
                        this.doctrineShipsRepository.Save();
                        logger.LogMessage("Notification Recipient '" + existingNotificationRecipient.Description + "' Successfully Updated For Account Id: " + existingNotificationRecipient.AccountId, 2, "Message", MethodBase.GetCurrentMethod().Name);
                    }
                }
            }

            return validationResult;
        }
    }
}
