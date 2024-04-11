using System;
using System.Collections.Generic;
using DataJobApplication.Models;

namespace DataJobApplication.Interfaces
{
    public interface IDataProcessorService
    {
        IEnumerable<DataJobDTO> GetAllDataJobs();
        IEnumerable<DataJobDTO> GetDataJobsByStatus(DataJobStatus status);
        DataJobDTO GetDataJob(Guid id);
        DataJobDTO Create(DataJobDTO dataJob);
        DataJobDTO Update(DataJobDTO dataJob);
        void Delete(Guid dataJobId);
        bool StartBackgroundProcess(Guid dataJobId);
        DataJobStatus GetBackgroundProcessStatus(Guid dataJobId);
        List<string> GetBackgroundProcessResults(Guid dataJobId);
    }
}
