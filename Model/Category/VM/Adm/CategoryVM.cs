﻿using System.ComponentModel.DataAnnotations;

namespace Model.VM
{
    public class CategoryVM
    {
        [Display(Name = "Sequence", ResourceType = typeof(i18n.Resource))]
        public int CategoryId { get; set; }

        [Display(Name = "Parent", ResourceType = typeof(i18n.Resource))]
        public string Parent { get; set; }

        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public string Code { get; set; }

        [Display(Name = "Category", ResourceType = typeof(i18n.Resource))]
        public string Name { get; set; }
    }
}
