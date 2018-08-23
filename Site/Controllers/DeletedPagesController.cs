﻿using Microsoft.AspNet.Identity;
using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Site.Controllers
{
    [RoutePrefix("Api/DeletedPages")]
    public class DeletedPagesController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DeletedPageRepository deletedPageRepository = new DeletedPageRepository();

        [Authorize, HttpPost, Route("")]
        public DeletedPageOut Post(DeletedPageIn deletedPageIn)
        {
            DeletedPageOut deletedPageOut = new DeletedPageOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    deletedPageIn.userId = new Guid(User.Identity.GetUserId());
                    deletedPageIn.key = Key;

                    deletedPageOut = deletedPageRepository.SaveDeletedPage(deletedPageIn);
                }
                else
                {
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        var errors = modelState.Errors;
                        if (errors.Any())
                        {
                            foreach (ModelError error in errors)
                            {
                                throw new Exception(error.ErrorMessage);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.GetUserId()), Key, "Erro", "Tecnodim.Controllers.DeletedPagesController.Post", ex.Message);

                deletedPageOut.successMessage = null;
                deletedPageOut.messages.Add(ex.Message);
            }

            return deletedPageOut;
        }
    }
}