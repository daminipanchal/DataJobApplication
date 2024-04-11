using DataJobApplication.Interfaces;
using DataJobApplication.Models;
using DataJobApplication.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataJobApplication.Controllers
{
    public class DataJobsController : ApiController
    {
        private IDataProcessorService _dataProcessorService;

        public DataJobsController()
        {
            this._dataProcessorService = new DataProcessorService();
        }

        #region WebAPIMethods

        /// <summary>
        /// 1. Get All Data Jobs
        /// </summary>
        /// <returns>Return All Data Jobs</returns>
        [HttpGet]
        [Route("api/datajobs")]
        public IHttpActionResult GetAllDataJobs()
        {
            try
            {
                var jobs = this._dataProcessorService.GetAllDataJobs();
                if (jobs != null)
                {
                    return Ok(jobs);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        /// <summary>
        /// 2. Get Data Job By Id
        /// </summary>
        /// <param name="id">Id of the Job</param>
        /// <returns>Return Job which matches the Job Id</returns>
        [HttpGet]
        [Route("api/datajobs/getdatajobbyid")]
        public IHttpActionResult GetDataJob(string id)
        {
            Guid jobId;
            try
            {
                if (Guid.TryParse(id, out jobId))
                {
                    var job = this._dataProcessorService.GetDataJob(jobId);
                    if (job != null)
                    {
                        return Ok(job);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest($"JobId: {id} is not valid");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 3. Get Data Job By Status
        /// </summary>
        /// <param name="status">provide status of the job</param>
        /// <returns>get all jobs using the status value</returns>
        [HttpGet]
        [Route("api/datajobs/getdatajobsbystatus")]
        public IHttpActionResult GetDataJobsByStatus(string status)
        {
            try
            {
                if (status.Equals(DataJobStatus.New.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return Ok(this._dataProcessorService.GetDataJobsByStatus(DataJobStatus.New));
                }
                else if (status.Equals(DataJobStatus.Completed.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return Ok(this._dataProcessorService.GetDataJobsByStatus(DataJobStatus.Completed));
                }
                else if (status.Equals(DataJobStatus.Processing.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return Ok(this._dataProcessorService.GetDataJobsByStatus(DataJobStatus.Processing));
                }
                else
                {
                    return BadRequest($"JobStatus: {status} is not valid");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 4. Get Background process results
        /// </summary>
        /// <param name="id">Id of the Job</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/datajobs/getbackgroundprocessresults")]
        public IHttpActionResult GetBackgroundProcessResults(string id)
        {
            Guid jobId;
            try
            {
                if (Guid.TryParse(id, out jobId))
                {
                    var job = this._dataProcessorService.GetBackgroundProcessResults(jobId);
                    if (job.Any())
                    {
                        return Ok(job);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest($"JobId: {id} is not valid");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        /// <summary>
        ///5. Get Background process status
        /// </summary>
        /// <param name="id">Id of the Job</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/datajobs/getbackgroundprocessstatus")]
        public IHttpActionResult GetBackgroundProcessStatus(string id)
        {
            Guid jobId;
            try
            {
                if (Guid.TryParse(id, out jobId))
                {
                    var status = this._dataProcessorService.GetBackgroundProcessStatus(jobId);
                    if (!status.Equals(DataJobStatus.None))
                    {
                        return Ok(status.ToString());
                    }
                    return Content(HttpStatusCode.NotFound, $"JobId: {id} JobStatus Not Found");
                }
                else
                {
                    return BadRequest($"JobId: {id} is not valid");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 6.Start background process of job
        /// </summary>
        /// <param name="id">Id of the Job</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/datajobs/startbackgroundprocess")]
        public IHttpActionResult StartBackgroundProcess(string id)
        {
            Guid jobId;
            try
            {
                if (Guid.TryParse(id, out jobId))
                {
                    var status = this._dataProcessorService.StartBackgroundProcess(jobId);
                    if (status)
                    {
                        return Ok(status.ToString());
                    }
                    return Content(HttpStatusCode.OK, $"JobId: {id} JobStatus is already running");
                }
                else
                {
                    return BadRequest($"JobId: {id} is not valid");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 7.Delete Job
        /// </summary>
        /// <param name="id">Id of the Job</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/datajobs/deletejob")]
        public IHttpActionResult DeleteJob(string id)
        {
            Guid jobId;
            try
            {
                if (Guid.TryParse(id, out jobId))
                {
                    this._dataProcessorService.Delete(jobId);
                    return Ok();
                }
                else
                {
                    return BadRequest($"JobId: {id} is not valid");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 8.Create Data Job
        /// </summary>
        /// <param name="dataJob"></param>
        /// <return>Newly Created Job</return></returns>
        [HttpPost]
        [Route("api/datajobs/create")]
        public IHttpActionResult Create(DataJobDTO dataJob)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var job = this._dataProcessorService.Create(dataJob);
                    return Ok(job);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        #endregion
    }
}
