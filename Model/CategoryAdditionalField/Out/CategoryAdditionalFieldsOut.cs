﻿using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class CategoryAdditionalFieldsOut : ResultServiceVM
    {
        public CategoryAdditionalFieldsOut()
        {
            result = new List<CategoryAdditionalFieldsVM>();
        }

        public List<CategoryAdditionalFieldsVM> result { get; set; }
    }
}
