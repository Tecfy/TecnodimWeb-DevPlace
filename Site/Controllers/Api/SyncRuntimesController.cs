﻿using Model.In;
using Model.Out;
using Repository;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace Site.Api.Controllers
{
    [RoutePrefix("Api/SyncRuntimes")]
    public class SyncRuntimesController : ApiController
    {
        private RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        private SyncRuntimeRepository syncRuntimeRepository = new SyncRuntimeRepository();

        [AllowAnonymous, HttpGet]
        public HttpResponseMessage GetSyncRuntimes()
        {
            var currentContext = HttpContext.Current;

            System.Threading.Tasks.Task objTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                HttpContext.Current = currentContext;
                SyncRuntimesOut syncRuntimesOut = new SyncRuntimesOut();
                string Key = Guid.NewGuid().ToString();

                try
                {
                    SyncRuntimesIn syncRuntimesIn = new SyncRuntimesIn { key = Key };
                    string urlBase = WebConfigurationManager.AppSettings["UrlBase"] + "/Api";

                    syncRuntimesOut = syncRuntimeRepository.GetSyncRuntimes(syncRuntimesIn);

                    foreach (var item in syncRuntimesOut.result)
                    {
                        registerEventRepository.SaveRegisterEvent("", Key, "Log - Start", "Tecnodim.Controllers.SyncRuntimesController.GetSyncRuntimes", string.Format("Ultima Execução: {0}, Intervalo: {1}, Data atual: {2}, URL: {3}", item.LastExecution.ToString("dd/MM/yyyy HH:mm:ss"), item.Interval, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), item.URL));

                        if (item.LastExecution.AddMinutes(item.Interval) < DateTime.Now)
                        {
                            using (HttpClient httpClient = new HttpClient())
                            {
                                var url = "";

                                if (item.IsExternal)
                                {
                                    url = item.URL;
                                }
                                else
                                {
                                    url = string.Format("{0}/{1}", urlBase, item.URL);
                                }

                                var response = httpClient.GetAsync(url).Result;
                                if (response.IsSuccessStatusCode)
                                {
                                    SyncRuntimeSaveIn syncRuntimeSaveIn = new SyncRuntimeSaveIn { key = Key, SyncRuntimeId = item.SyncRuntimeId };

                                    syncRuntimeRepository.SaveSyncRuntimes(syncRuntimeSaveIn);
                                }
                                else
                                {
                                    registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.SyncRuntimesController.GetSyncRuntimes", string.Format("Chamada da URL {0} data {1}, executada com falha. Erro: {2}", item.URL, DateTime.Now.ToShortTimeString(), response.RequestMessage));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    registerEventRepository.SaveRegisterEvent("", Key, "Erro", "Tecnodim.Controllers.SyncRuntimesController.GetSyncRuntimes", ex.Message);

                    syncRuntimesOut.successMessage = null;
                    syncRuntimesOut.messages.Add(ex.Message);
                }
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}