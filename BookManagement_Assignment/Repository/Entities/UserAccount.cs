    using System;
    using System.Collections.Generic;

    namespace Repository.Entities;

    public partial class UserAccount
    {
        public int MemberId { get; set; }

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public int Role { get; set; }
    }
