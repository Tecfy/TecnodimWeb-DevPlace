﻿using DataEF.DataAccess;
using Helper.Enum;
using Helper.ServerMap;
using Model.In;
using Model.Out;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Configuration;

namespace Repository
{
    public partial class PDFRepository
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private DocumentRepository documentRepository = new DocumentRepository();

        #region .: Methods :.

        public PDFsOut GetPDFs(DocumentIn documentIn)
        {
            PDFsOut pdfOut = new PDFsOut();
            registerEventRepository.SaveRegisterEvent(documentIn.id, documentIn.key, "Log - Start", "Repository.PDFRepository.GetPDFs", "");

            DocumentOut documentOut = documentRepository.GetECMDocumentById(documentIn);

            string path = ServerMapHelper.GetServerMap(WebConfigurationManager.AppSettings["Path"]);
            string pathImages = Path.Combine(path, "Pages", documentOut.result.Hash.ToString(), "Images");
            string pathThumb = Path.Combine(path, "Pages", documentOut.result.Hash.ToString(), "Thumbs");

            string name = documentOut.result.ExternalId + ".pdf";
            string pathFile = Path.Combine(path, "Documents", name);

            if (!Directory.Exists(pathImages))
            {
                Directory.CreateDirectory(pathImages);
            }

            if (!Directory.Exists(pathThumb))
            {
                Directory.CreateDirectory(pathThumb);
            }

            if (!Directory.Exists(Path.Combine(path, "Documents")))
            {
                Directory.CreateDirectory(Path.Combine(path, "Documents"));
            }

            if (Directory.GetFiles(pathImages).Length > 0 && Directory.GetFiles(pathThumb).Length > 0)
            {
                RemainingDocumenPagestIn remainingDocumenPagestIn = new RemainingDocumenPagestIn() { documentId = documentIn.documentId, id = documentIn.id, key = documentIn.key };

                List<int> pages = new List<int>();
                pages = documentRepository.GetRemainingDocumentPages(remainingDocumenPagestIn);

                pdfOut.result.pages = new List<int>();
                pdfOut.result.path = WebConfigurationManager.AppSettings["UrlBase"] + "/Files/Pages/" + documentOut.result.Hash.ToString() + "/Images/{0}.jpg";
                pdfOut.result.pathThumb = WebConfigurationManager.AppSettings["UrlBase"] + "/Files/Pages/" + documentOut.result.Hash.ToString() + "/Thumbs/{0}.jpg";

                for (int i = 1; i <= documentOut.result.Pages; i++)
                {
                    if (!pages.Contains(i))
                    {
                        pdfOut.result.pages.Add(i);
                    }
                }
            }
            else
            {
                if (!documentOut.result.Download)
                {
                    documentRepository.GetECMDocument(documentIn, pathFile);

                    if (Directory.GetFiles(pathImages).Length > 0 && Directory.GetFiles(pathThumb).Length > 0)
                    {
                        RemainingDocumenPagestIn remainingDocumenPagestIn = new RemainingDocumenPagestIn() { documentId = documentIn.documentId, id = documentIn.id, key = documentIn.key };

                        List<int> pages = new List<int>();
                        pages = documentRepository.GetRemainingDocumentPages(remainingDocumenPagestIn);

                        pdfOut.result.pages = new List<int>();
                        pdfOut.result.path = WebConfigurationManager.AppSettings["UrlBase"] + "/Files/Pages/" + documentOut.result.Hash.ToString() + "/Images/{0}.jpg";
                        pdfOut.result.pathThumb = WebConfigurationManager.AppSettings["UrlBase"] + "/Files/Pages/" + documentOut.result.Hash.ToString() + "/Thumbs/{0}.jpg";

                        for (int i = 1; i <= documentOut.result.Pages; i++)
                        {
                            if (!pages.Contains(i))
                            {
                                pdfOut.result.pages.Add(i);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception(i18n.Resource.FileNotFound);
                }
            }

            using (var db = new DBContext())
            {
                if (pdfOut.result == null)
                {
                    Documents document = db.Documents.Where(x => x.DocumentId == documentIn.documentId).FirstOrDefault();

                    if (document.DocumentStatusId == (int)EDocumentStatus.PartiallySlice)
                    {
                        documentRepository.PostDocumentUpdateSatus(new DocumentUpdateIn { id = documentIn.id, key = documentIn.key, documentId = documentIn.documentId, documentStatusId = (int)EDocumentStatus.Slice });
                    }
                }
            }

            registerEventRepository.SaveRegisterEvent(documentIn.id, documentIn.key, "Log - End", "Repository.PDFRepository.GetPDFs", "");
            return pdfOut;
        }

        #endregion
    }
}
