using Journey_of_faith.Infrastructure.identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.faith_notifications
{
    public class ReminderSetting
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int MinutesBefore { get; set; }
        public string? SpeechGender { get; set; }
        public double? SpeechSpeed { get; set; }

        public ApplicationUser User { get; set; } = null!;
    }
}
