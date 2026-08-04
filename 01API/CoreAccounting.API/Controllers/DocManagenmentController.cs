using Azure;
using BankFeed.Infra.DataRepos;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using CoreAccounting.App.Contracts;
using DataModel.Domain.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Ocsp;
using System.Diagnostics.Contracts;
using System.Net;
using System.Text.Json;

namespace CoreAccounting.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class DocManagenmentController : BaseController
    {
        #region Fields
        private readonly IImportDoc importDoc;
        private readonly IAWSFileService awsFileService;
        #endregion

        #region Ctor
        public DocManagenmentController(IImportDoc _importDoc, IAWSFileService _fileSrv)
        {
            this.importDoc = _importDoc;
            this.awsFileService = _fileSrv;
        }

        #endregion
        #region Private Methods

        private string GetClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        #endregion

        #region DocManagement
        [Route("docs/Upload")]
        [HttpPost]
        public async Task<IActionResult> UploadFiles(UploadFilesReq req)
        {
            string clientName = GetClientName();
            AWSFileResponse awsResp = await awsFileService.UploadFiles(req, clientName);
            if (awsResp != null && awsResp.StatusCode == (int)HttpStatusCode.OK)
            {
                bool isSaved = await importDoc.SaveFilePath(awsResp);
                return Ok(awsResp);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
        }
        [Route("docs/Download")]
        [HttpPost]
        public async Task<IActionResult> DownloadFiles(AwsFileReq req)
        {
            string clientName = GetClientName();
            AWSFileResponse awsResp = await importDoc.GetFiles(req);
            if (awsResp != null && awsResp.StatusCode == (int)HttpStatusCode.OK)
            {
                AWSFileDownloadResp awsDownloadResp = await awsFileService.DownloadAll(awsResp, clientName);
                if (awsDownloadResp != null && awsDownloadResp.StatusCode == (int)HttpStatusCode.OK)
                    return Ok(awsDownloadResp);
                else
                    return StatusCode(StatusCodes.Status404NotFound, Constants.MSG_FILE_NO_DATA_FOUND);
            }

            else
                return StatusCode(StatusCodes.Status404NotFound, Constants.MSG_FILE_NO_DATA_FOUND);
        }

        [Route("docs/listFilesView")]
        [HttpPost]
        public async Task<IActionResult> GetListFiles(FileRequest req)
        {
            string clientName = GetClientName();
            FilesViewResponse fileViewResp = new FilesViewResponse();
            foreach (var refID in req.ReferenceID)
            {
                AWSFileResponse awsResp = await importDoc.GetFiles(new AwsFileReq { ReferenceID = refID });
                if (awsResp != null && awsResp.StatusCode == (int)HttpStatusCode.OK)
                {
                    var fileViewResult = await awsFileService.GetFiles(awsResp, clientName);
                    if (fileViewResult != null && fileViewResult.StatusCode == (int)HttpStatusCode.OK)
                    {
                        fileViewResp.Files.AddRange(fileViewResult.Files);
                        fileViewResp.StatusCode = awsResp.StatusCode;
                        fileViewResp.Status = awsResp.Status;
                    }
                }
            }
            if (fileViewResp != null && fileViewResp.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(fileViewResp);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
        }

        [Route("docs/Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteFiles(UploadFilesReq req)
        {
            string clientName = GetClientName();
            AWSFileResponse awsResp = await awsFileService.DeleteFiles(req, clientName);
            if (awsResp != null && awsResp.StatusCode == (int)HttpStatusCode.OK)
            {
                bool isSaved = await importDoc.Delete(awsResp);
                return Ok(awsResp);
            }
            else
                return StatusCode(StatusCodes.Status404NotFound, Constants.MSG_FILE_NO_DATA_FOUND);
        }
        [Route("docs/filesview")]
        [HttpPost]
        public async Task<IActionResult> GetFiles(AwsFileReq req)
        {
            string clientName = GetClientName();
            FilesViewResponse fileViewResp =new FilesViewResponse();    
            AWSFileResponse awsResp = await importDoc.GetFiles(req);
            if (awsResp != null && awsResp.StatusCode == (int)HttpStatusCode.OK)
            {
                fileViewResp = await awsFileService.GetFiles(awsResp, clientName);
                if (fileViewResp != null && fileViewResp.StatusCode == (int)HttpStatusCode.OK)
                    return Ok(fileViewResp);
                else
                    return Ok(fileViewResp);
            }
            else if (awsResp != null && !string.IsNullOrEmpty(awsResp.Status))
            {
                fileViewResp.Status = Constants.MSG_FILE_NO_DATA_FOUND;
                fileViewResp.StatusCode = StatusCodes.Status404NotFound;
                return Ok(fileViewResp);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
        }

        [Route("docs/DownloadAll")]
        [HttpPost]
        public async Task<IActionResult> DownloadAllFiles(FileRequest req)
        {
            string clientName = GetClientName();
            List<AWSFileResponse> awsResp = await importDoc.GetAllFiles(req.ReferenceID);
            AWSBulkDownloadResp awsBulkResp = null;
            if (awsResp != null && awsResp.Any())
            {
                awsBulkResp = await awsFileService.BulkDownloadAll(awsResp, clientName);
            }
            if (awsBulkResp != null && awsBulkResp.StatusCode == (int)HttpStatusCode.OK)
                return Ok(awsBulkResp);
            else
                return StatusCode(StatusCodes.Status404NotFound, Constants.MSG_FILE_NO_DATA_FOUND);
        }

        [Route("docs/FileUploadDumpAll")]
        [HttpPost]
        public async Task<IActionResult> FileUploadDumpAll()
        {
            string clientName = GetClientName();
            string imdResp = await importDoc.DumpAllFiles();
            return Ok(imdResp);
        }

        [Route("docs/GetFileUploadAll")]
        [HttpPost]
        public async Task<IActionResult> GetFileUploadAll([FromBody] string FileTypeName)
        {
            try
            {
                string clientName = GetClientName();
                var imdResp = await importDoc.GetAllFileUrl(clientName, FileTypeName);
                return Ok(imdResp);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("docs/RepayAttachments")]
        [HttpGet]
        public async Task<IActionResult> GetRepayFiles(string GroupName)
        {
            try
            {

                List<FilesViewResponse> response = new List<FilesViewResponse>();
                if (!string.IsNullOrEmpty(GroupName))
                {

                    var awsResp = await importDoc.GetRepayFiles(GroupName);

                    if (awsResp.Item1.Any())
                    {
                        foreach (var item in awsResp.Item1)
                        {
                            var fileViewResp = await awsFileService.GetFiles(item, awsResp.Item2);
                            if (fileViewResp != null && fileViewResp.StatusCode == (int)HttpStatusCode.OK)
                                response.Add(fileViewResp);

                        }

                    }
                    return Ok(response);
                }
                else
                {
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Route("docs/RepayDownloadFile")]
        [HttpPost]
        public async Task<IActionResult> GetRepayDownloadFiles(RepayDownloadFile files)
        {
            try
            {
                List<FilesViewResponse> response = new List<FilesViewResponse>();
                if (!string.IsNullOrEmpty(files.JournalEntryId))
                {

                    var awsResp = await importDoc.RepayDownloadAttachment(files);
                    if(awsResp.Item1!=null)
                    {
                        AWSFileDownloadResp awsDownloadResp = await awsFileService.DownloadAll(awsResp.Item1, awsResp.Item2);
                        if (awsDownloadResp != null && awsDownloadResp.StatusCode == (int)HttpStatusCode.OK)
                            return Ok(awsDownloadResp);
                        else
                            return StatusCode(StatusCodes.Status404NotFound, Constants.MSG_FILE_NO_DATA_FOUND);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, Constants.MSG_FILE_NO_DATA_FOUND);
                    }
                   
                   
                }
                else
                {
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
