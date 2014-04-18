namespace DoctrineShips.Validation
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;
    using EveData.Entities;
    
    /// <summary>
    /// An interface representing available Doctrine Ships validation checks. A list of methods exposing all of the Doctrine Ships validation functionality.
    /// </summary>
    public interface IDoctrineShipsValidation
    {
        /// <summary>
        /// Check that the passed contract is valid and ready to be written to the database.
        /// </summary>
        /// <param name="contract">A doctrine ships contract object.</param>
        /// <returns>Returns an IValidationResult.</returns>
        IValidationResult Contract(Contract contract);

        /// <summary>
        /// Check that the passed sales agent is valid and ready to be written to the database.
        /// </summary>
        /// <param name="salesAgent">A doctrine ships SalesAgent object.</param>
        /// <returns>Returns an IValidationResult.</returns>
        IValidationResult SalesAgent(SalesAgent salesAgent);

        /// <summary>
        /// Check that the passed api key is valid.
        /// </summary>
        /// <param name="apiKeyInfo">A populated IEveDataApiKey object.</param>
        /// <returns>Returns an IValidationResult.</returns>
        IValidationResult ApiKey(IEveDataApiKey apiKeyInfo);

        /// <summary>
        /// Check that the passed customer is valid and ready to be written to the database.
        /// </summary>
        /// <param name="salesAgent">A doctrine ships Customer object.</param>
        /// <returns>Returns an IValidationResult.</returns>
        IValidationResult Customer(Customer customer);

        /// <summary>
        /// Check that the passed ship fit is valid and ready to be written to the database.
        /// </summary>
        /// <param name="shipFit">A doctrine ships ShipFit object.</param>
        /// <returns>Returns an IValidationResult.</returns>
        IValidationResult ShipFit(ShipFit shipFit);

        /// <summary>
        /// Check that the passed ship fit component is valid and ready to be written to the database.
        /// </summary>
        /// <param name="shipFitComponent">A doctrine ships ShipFitComponent object.</param>
        /// <returns>Returns an IValidationResult.</returns>
        IValidationResult ShipFitComponent(ShipFitComponent shipFitComponent);

        /// <summary>
        /// Check that the passed component is valid and ready to be written to the database.
        /// </summary>
        /// <param name="component">A doctrine ships Component object.</param>
        /// <returns>Returns an IValidationResult.</returns>
        IValidationResult Component(Component component);

        /// <summary>
        /// Check that the passed account is valid and ready to be written to the database.
        /// </summary>
        /// <param name="account">A doctrine ships Account object.</param>
        /// <returns>Returns an IValidationResult.</returns>
        IValidationResult Account(Account account);

        /// <summary>
        /// Check that the passed access code is valid and ready to be written to the database.
        /// </summary>
        /// <param name="accessCode">A doctrine ships AccessCode object.</param>
        /// <returns>Returns an IValidationResult.</returns>
        IValidationResult AccessCode(AccessCode accessCode);

        /// <summary>
        /// Check that the passed setting profile is valid and ready to be written to the database.
        /// </summary>
        /// <param name="settingProfile">A doctrine ships SettingProfile object.</param>
        /// <returns>Returns an IValidationResult.</returns>
        IValidationResult SettingProfile(SettingProfile settingProfile);

        /// <summary>
        /// Check that the passed notification recipient is valid and ready to be written to the database.
        /// </summary>
        /// <param name="notificationRecipient">A doctrine ships NotificationRecipient object.</param>
        /// <returns>Returns an IValidationResult.</returns>
        IValidationResult NotificationRecipient(NotificationRecipient notificationRecipient);

        /// <summary>
        /// Check that the passed doctrine is valid and ready to be written to the database.
        /// </summary>
        /// <param name="doctrine">A doctrine ships Doctrine object.</param>
        /// <returns>Returns an IValidationResult.</returns>
        IValidationResult Doctrine(Doctrine doctrine);
    }
}