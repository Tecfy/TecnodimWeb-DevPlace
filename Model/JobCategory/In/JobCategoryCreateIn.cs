﻿namespace Model.In
{
    public class JobCategoryCreateIn : BaseIn
    {
        public int jobId { get; set; }

        public int categoryId { get; set; }

        public string code { get; set; }
    }
}