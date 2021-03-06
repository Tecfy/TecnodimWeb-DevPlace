﻿using System.Collections.Generic;

namespace Model.In
{
    public class DocumentsIn : BaseIn
    {
        public int unityId { get; set; }

        public string registration { get; set; }

        public string name { get; set; }

        public int documentStatusId { get; set; }

        public int documentId { get; set; }

        public string externalId { get; set; }

        public List<int> documentStatusIds { get; set; }
    }
}
