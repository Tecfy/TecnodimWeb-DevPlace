﻿using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class PageRatingIn
    {
        [Required]
        [Display(Name = "Page", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int page { get; set; }

        [Display(Name = "Rotate", ResourceType = typeof(i18n.Resource))]
        public int? rotate { get; set; }
    }
}
