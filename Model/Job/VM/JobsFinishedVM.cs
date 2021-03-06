﻿using System.Collections.Generic;

namespace Model.VM
{
    public class JobsFinishedVM
    {
        public int jobId { get; set; }

        public int jobCategoryId { get; set; }

        public string externalId { get; set; }

        public string registration { get; set; }

        public string categoryId { get; set; }
        
        public string title { get; set; }

        public string user { get; set; }

        public string extension { get; set; }

        public string unityCode { get; set; }

        public string unityName { get; set; }

        public bool pbEmbarked { get; set; }

        public List<JobCategoryPagesFinishedVM> pages { get; set; }

        public List<AdditionalFieldSaveVM> additionalFields { get; set; }
    }
}
