﻿using Microsoft.AspNetCore.Identity;

namespace VHV_FileSaver.Data.Models
{
    public class Role : IdentityRole<int>, IBaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public User? ModifiedBy { get; set; }
        public int? ModifiedById { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}