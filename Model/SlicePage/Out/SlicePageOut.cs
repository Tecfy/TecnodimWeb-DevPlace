﻿using Model.VM;

namespace Model.Out
{
    public class SlicePageOut : ResultServiceVM
    {
        public SlicePageOut()
        {
            result = new SlicePageVM();
        }

        public SlicePageVM result { get; set; }
    }
}
