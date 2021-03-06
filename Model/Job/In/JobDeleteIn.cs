﻿using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class JobDeleteIn : BaseIn
    {
        [Required]
        [Display(Name = "Job", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int jobId { get; set; }
    }
}
