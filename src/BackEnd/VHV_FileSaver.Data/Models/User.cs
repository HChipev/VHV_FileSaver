﻿using Microsoft.AspNetCore.Identity;

namespace VHV_FileSaver.Data.Models
{
    public class User : IdentityUser<int>, IBaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public User? ModifiedBy { get; set; }
        public int? ModifiedById { get; set; }
        public ICollection<RoleClaim> ModifiedRoleClaims { get; set; }
        public ICollection<Role> ModifiedRoles { get; set; }
        public ICollection<UserClaim> ModifiedUserClaims { get; set; }
        public ICollection<User> ModifiedUsers { get; set; }
        public ICollection<UserLogin> ModifiedUserLogins { get; set; }
        public ICollection<UserRole> ModifiedUserRoles { get; set; }
        public ICollection<UserToken> ModifiedUserTokens { get; set; }
    }
}