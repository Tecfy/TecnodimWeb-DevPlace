﻿namespace Model.VM
{
    public class ClippingPageVM
    {
        public int clippingPageId { get; set; }

        public int page { get; set; }

        public int? Rotate { get; set; }

        public string thumb { get; set; }

        public string image { get; set; }
    }
}
