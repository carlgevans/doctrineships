namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using DoctrineShips.Entities;

    public class AccountNotificationRecipientsViewModel
    {
        public IEnumerable<NotificationRecipient> NotificationRecipients { get; set; }
        
        // Form Data.
        public int[] RemoveList { get; set; }
        public int NotificationRecipientId { get; set; }
        public int AccountId { get; set; }

        [Required(ErrorMessage = "A Twitter Handle is required.")]
        [DisplayName("Twitter Handle")]
        [RegularExpression("^@(\\w){1,15}$", ErrorMessage = "That does not look like a valid Twitter @Handle.")]
        public string TwitterHandle { get; set; }

        [Required(ErrorMessage = "You must enter a description.")]
        [Display(Name = "Description")]
        [StringLength(30, ErrorMessage = "The description field must be shorter than 30 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "An Alert Interval Hours value is required.")]
        [DisplayName("Alert Interval Hours")]
        [Range(1, 168, ErrorMessage = "Alert Interval Hours should be between 1 and 168 hours (one week).")]
        public int AlertIntervalHours { get; set; }

        [DisplayName("Receives Daily Summary?")]
        [Range(0, 1, ErrorMessage = "That isn't a valid value.")]
        public bool ReceivesDailySummary { get; set; }

        [DisplayName("Receives Alerts?")]
        [Range(0, 1, ErrorMessage = "That isn't a valid value.")]
        public bool ReceivesAlerts { get; set; }

        public AccountNotificationRecipientsViewModel()
        {
            // Set default values.
            AlertIntervalHours = 12;
            ReceivesDailySummary = true;
            ReceivesAlerts = true;
        }
    }
}