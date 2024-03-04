﻿namespace LearningManagementSystem.Models.MailSender
{
    public class MailSettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool SmtpAuth { get; set; }
        public bool SmtpStartTlsEnable { get; set; }
    }
}
