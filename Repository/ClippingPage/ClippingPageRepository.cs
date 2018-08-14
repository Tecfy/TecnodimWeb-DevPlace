﻿using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Linq;

namespace Repository
{
    public partial class ClippingPageRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        #region .: Methods :.

        public ClippingPageOut SaveClippingPage(ClippingPageIn clippingPageIn)
        {
            ClippingPageOut clippingPageOut = new ClippingPageOut();

            registerEventRepository.SaveRegisterEvent(clippingPageIn.userId.Value, clippingPageIn.key.Value, "Log - Start", "Repository.ClippingPageRepository.SaveClippingPage", "");

            using (var db = new DBContext())
            {
                ClippingPages clippingPages = new ClippingPages();
                clippingPages.ClippingId = clippingPageIn.clippingId;
                clippingPages.Page = clippingPageIn.page;

                db.ClippingPages.Add(clippingPages);
                db.SaveChanges();

                clippingPageOut.result.clippingPageId = clippingPages.ClippingPageId;
            }

            registerEventRepository.SaveRegisterEvent(clippingPageIn.userId.Value, clippingPageIn.key.Value, "Log - End", "Repository.ClippingPageRepository.SaveClippingPage", "");
            return clippingPageOut;
        }

        public void UpdateClippingPage(ClippingPageUpdateIn clippingPageUpdateIn)
        {
            registerEventRepository.SaveRegisterEvent(clippingPageUpdateIn.userId.Value, clippingPageUpdateIn.key.Value, "Log - Start", "Repository.ClippingPageRepository.UpdateClippingPage", "");

            using (var db = new DBContext())
            {
                ClippingPages clippingPages = db.ClippingPages.Find(clippingPageUpdateIn.clippingPageId);

                if (clippingPages != null)
                {
                    clippingPages.Rotate = clippingPageUpdateIn.rotate;

                    db.Entry(clippingPages).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            registerEventRepository.SaveRegisterEvent(clippingPageUpdateIn.userId.Value, clippingPageUpdateIn.key.Value, "Log - End", "Repository.ClippingPageRepository.UpdateClippingPage", "");
        }

        #endregion
    }
}
