﻿using DataEF.DataAccess;
using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Linq;

namespace Repository
{
    public partial class JobRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        JobCategoryRepository jobCategoryRepository = new JobCategoryRepository();

        #region .: API :.

        public JobsByRegistrationOut GetJobsByRegistration(JobsByRegistrationIn jobsByRegistrationIn)
        {
            JobsByRegistrationOut jobsByRegistrationOut = new JobsByRegistrationOut();
            registerEventRepository.SaveRegisterEvent(jobsByRegistrationIn.id, jobsByRegistrationIn.key, "Log - Start", "Repository.JobRepository.GetJobsByRegistration", "");

            using (var db = new DBContext())
            {
                jobsByRegistrationOut.result = db.Jobs
                                                .Where(x => x.Active == true
                                                         && x.DeletedDate == null
                                                         && x.Users.Registration == jobsByRegistrationIn.registration
                                                         && x.JobCategories.Count(y => y.Received == false) > 0)
                                                .Select(x => new JobsByRegistrationVM()
                                                {
                                                    JobId = x.JobId,
                                                    Registration = x.Registration,
                                                    Name = x.Name,
                                                    JobCategories = x.JobCategories.Where(y => y.Received == false)
                                                    .Select(y => new JobCategoriesByRegistrationVM()
                                                    {
                                                        JobCategoryId = y.JobCategoryId,
                                                        Category = y.Categories.Code + " - " + y.Categories.Name,
                                                    }).ToList()
                                                })
                                                .ToList();
            }

            registerEventRepository.SaveRegisterEvent(jobsByRegistrationIn.id, jobsByRegistrationIn.key, "Log - End", "Repository.JobRepository.GetJobsByRegistration", "");
            return jobsByRegistrationOut;
        }

        public JobCreateOut CreateJob(JobCreateIn jobsCreateIn)
        {
            JobCreateOut jobCreateOut = new JobCreateOut();

            registerEventRepository.SaveRegisterEvent(jobsCreateIn.id, jobsCreateIn.key, "Log - Start", "Repository.JobRepository.CreateJob", "");

            using (var db = new DBContext())
            {
                Jobs job = new Jobs
                {
                    Active = true,
                    CreatedDate = DateTime.Now,
                    UserId = jobsCreateIn.userId,
                    JobStatusId = (int)EJobStatus.New,
                    Registration = jobsCreateIn.registration,
                    Name = jobsCreateIn.name,
                    Sent = false
                };

                db.Jobs.Add(job);
                db.SaveChanges();

                jobCreateOut.result.jobId = job.JobId;
            }

            registerEventRepository.SaveRegisterEvent(jobsCreateIn.id, jobsCreateIn.key, "Log - End", "Repository.JobRepository.CreateJob", "");
            return jobCreateOut;
        }

        #endregion
    }
}