﻿using System.ComponentModel.DataAnnotations;

namespace Model.VM
{
    public class AspNetUserClaimVM
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int Id { get; set; }

        [Display(Name = "User", ResourceType = typeof(i18n.Resource))]
        public string UserId { get; set; }

        [Display(Name = "ClaimType", ResourceType = typeof(i18n.Resource))]
        public string ClaimType { get; set; }

        [Display(Name = "ClaimValue", ResourceType = typeof(i18n.Resource))]
        public string ClaimValue { get; set; }
    }
}
