﻿using Microsoft.AspNetCore.Identity;

namespace VHV_FileSaver.Data.Models
{
    public class RoleClaim : IdentityRoleClaim<int>, IBaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public User? ModifiedBy { get; set; }
        public int? ModifiedById { get; set; }
    }
}