using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.location
{
    public class Diocese : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? Address { get; set; }
        public string? Thumbnail { get; set; }
        public int CountChurch => _churchList.Count;
        private List<Church> _churchList { get; set ; } = new List<Church>();

        public IReadOnlyCollection<Church> Churchs => _churchList.AsReadOnly();

        public Diocese(string name, string? websizte, string address, string thumbnail, Guid Userid)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            Name = name;
            Website = websizte;
            Address = address;
            Thumbnail = thumbnail;
            CreatorUserId = Userid;
            LastModifierUserId = Userid;
        }

        public void SetChurch(List<Church> churches)
        {
            _churchList.Clear();
            _churchList.AddRange(churches);
        }
    }
}
