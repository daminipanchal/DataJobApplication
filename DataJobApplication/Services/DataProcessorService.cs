using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using DataJobApplication.Interfaces;
using DataJobApplication.Models;

namespace DataJobApplication.Services
{
    public class DataProcessorService : IDataProcessorService
    {
        public static List<DataJobDTO> dataJobs = new List<DataJobDTO>();
        public DataProcessorService()
        {

            dataJobs.Add(new DataJobDTO { Id = Guid.NewGuid(), Name = "ABCJob", FilePathToProcess = "https://coderbyte.com/", Status = DataJobStatus.Completed, Links = GetDataJobLinks(), Results = GetDataJobResults() });
            dataJobs.Add(new DataJobDTO { Id = Guid.NewGuid(), Name = "PQRJob", FilePathToProcess = "https://google.com/", Status = DataJobStatus.New, Links = GetDataJobLinks(), Results = GetDataJobResults() });
            dataJobs.Add(new DataJobDTO { Id = Guid.NewGuid(), Name = "MNCJob", FilePathToProcess = "https://yahoo.com/", Status = DataJobStatus.Processing, Links = GetDataJobLinks(), Results = GetDataJobResults() });
        }

        #region Public Methods
        public DataJobDTO Create(DataJobDTO dataJob)
        {
            var job = new DataJobDTO { Id = dataJob.Id, Name = dataJob.Name, FilePathToProcess = dataJob.FilePathToProcess, Status = dataJob.Status, Links = dataJob.Links, Results = dataJob.Results };
            dataJobs.Add(job);
            return job;
        }

        public void Delete(Guid dataJobId)
        {
            int index = dataJobs.FindIndex(x => x.Id == dataJobId);
            if (index != -1)
            {
                dataJobs.RemoveAt(index);
            }
        }

        public IEnumerable<DataJobDTO> GetAllDataJobs()
        {
            return dataJobs;
        }

        public List<string> GetBackgroundProcessResults(Guid dataJobId)
        {
            var job = GetDataJob(dataJobId);
            if (job == null) return new List<string>();
            return job.Results.ToList();
        }

        public DataJobStatus GetBackgroundProcessStatus(Guid dataJobId)
        {
            var job = GetDataJob(dataJobId);
            if (job != null && job.Status != null)
            {
                return job.Status.Value;
            }
            return DataJobStatus.None;
        }

        public DataJobDTO GetDataJob(Guid id)
        {
            var job = GetAllDataJobs().Where(x => x.Id == id).FirstOrDefault();
            return job;
        }

        public IEnumerable<DataJobDTO> GetDataJobsByStatus(DataJobStatus status)
        {
            var jobs = GetAllDataJobs().Where(x => x.Status == status);
            return jobs;
        }

        public bool StartBackgroundProcess(Guid dataJobId)
        {
            //Start job process if it is not in processing
            var status = GetDataJob(dataJobId)?.Status;
            if (status != null && status != DataJobStatus.Processing)
            {
                return true;
            }
            return false;
        }

        public DataJobDTO Update(DataJobDTO dataJob)
        {
            if (dataJob == null) throw new ArgumentNullException(nameof(dataJob));
            int index = dataJobs.FindIndex(x => x.Id == dataJob.Id);
            if (index != -1)
            {
                dataJobs.RemoveAt(index);
                dataJobs.Add(dataJob);
            }
            return dataJob;
        }
        #endregion

        #region Private Methods
        private IEnumerable<Link> GetDataJobLinks()
        {
            var dataLinks = new List<Link>();
            dataLinks.Add(new Link { Rel = "help", Action = "Get", Href = "https://coderbyte.com/", Types = new string[] { "text/html" } });
            dataLinks.Add(new Link { Rel = "help", Action = "Get", Href = "https://google.com/", Types = new string[] { "text/html" } });
            dataLinks.Add(new Link { Rel = "help", Action = "Get", Href = "https://yahoo.com/", Types = new string[] { "text/html" } });
            return dataLinks;
        }
        private IEnumerable<string> GetDataJobResults()
        {
            var results = new List<string>();
            results.Add("Result1");
            results.Add("Result2");
            results.Add("Result3");
            return results;
        }
        #endregion
    }
}