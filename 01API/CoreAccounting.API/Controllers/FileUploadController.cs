//using Azure;
using Common.App.Contracts;
using Common.Domain;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.API.Authorization;
using CoreAccounting.App.Contracts;
using CoreAccounting.App.Services;
using Microsoft.AspNetCore.Mvc;
using DataModel.Domain.DataModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Text;

namespace CoreAccounting.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class FileUploadController : BaseController
    {
        #region Fields
        private readonly ICoreProperty coreProp;
        private readonly IFileService fileSrv;

        #endregion

        #region Ctor
        public FileUploadController(ICoreProperty coreProperty, IFileService fileService)
        {
            this.coreProp = coreProperty;
            fileSrv = fileService;
        }

        #endregion
        private string GetClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }

        #region AWS File Upload

        /// <summary>
        /// Upload Multiple files to AWS S3 bucket using the ReferenceID & Update Details in Database
        /// </summary>
        /// <param name="files"> File Data from Upload control </param>
        /// <param name="RefId"> ReferenceID from the Entry (ex: JournalEntryID )</param>
        /// <returns> Status of file upload (Success / Failed) && status codes </returns>
        [Route("AWS/Uploads")]
        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<UploadFileRequest> files, [FromQuery] string RefId, [FromQuery] string CorpID, string TypeName)
        {
            FileResponse? response = null;
            try
            {
                response = await fileSrv.UploadFiles(files, GetClientName(), RefId, CorpID, TypeName);
                if (response != null && response.FileCount > 0)
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Message)) ? response.Message : Constants.MSG_ENDPOINT_ERROR);

            }
            catch { throw; }
            finally { response = null; }

        }

        /// <summary>
        /// Upload Multiple files to AWS S3 bucket using AWS credentials & Update Details in Database
        /// </summary>
        /// <param name="files"> File Data from Upload control </param>
        /// <param name="RefId"> ReferenceID from the Entry (ex: JournalEntryID )</param>
        /// <returns> Status of file upload (Success / Failed) </returns>
        [Route("AWS/Upload")]
        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files, [FromQuery] string RefId, [FromQuery] string CorpID, string TypeName)
        {
            FileResponse? response = null;
            try
            {
                response = await fileSrv.UploadFiles(files, GetClientName(), RefId, CorpID, TypeName);
                if (response != null && response.FileCount > 0)
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Message)) ? response.Message : Constants.MSG_ENDPOINT_ERROR);

            }
            catch { throw; }
            finally { response = null; }

        }

        /// <summary>
        /// Dowload Files from AWS Cloud to local storage & Update Details in Database
        /// </summary>
        /// <param name="file">ReferenceID from the Entry (ex: JournalEntryID )</param>
        /// <returns> Confirmation message for download and files will be downloaded</returns>
        //[Route("AWS/DownloadAll")]
        //[HttpPost]
        //public async Task<IActionResult> DownloadFiles(FileRequest file)
        //{
        //    FileResponse? response = null;
        //    try
        //    {
        //        
        //        response = await fileSrv.DownloadFiles(file);
        //        if (response != null && response.FileCount > 0)
        //            return Ok(response);
        //        else
        //            return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Message)) ? response.Message : Constants.MSG_ENDPOINT_ERROR);
        //    }
        //    catch { throw; }
        //    finally { response = null; }
        //}

        [Route("AWS/DownloadAll")]
        [HttpPost]
        public async Task<IActionResult> DownloadFiles(FileRequest file)
        {
            FileResponse? response = null;
            try
            {   
                var fileBytes = await fileSrv.DownloadFiles(file);
                return Ok(fileBytes);
                //string json = JsonConvert.SerializeObject(fileBytes);

                //// Convert JSON string to byte array
                //byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

                //// Return the JSON bytes as a file attachment
                //return File(jsonBytes, "application/json", "files.json");

                //return File(fileBytes, "application/octet-stream", "files.zip");
                //if (response != null && response.FileCount > 0)
                //    return Ok(response);
                //else
                //    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Message)) ? response.Message : Constants.MSG_ENDPOINT_ERROR);
            }
            catch { throw; }
           
        }

        [Route("AWS/DownloadFormat")]
        [HttpPost]
        public async Task<IActionResult> DownloadFormat(FileRequest file)
        {
            Dictionary<string, byte[]> response = null;
            try
            {
                var fileBytes = await fileSrv.DownloadFiles(file);
                if (fileBytes != null && fileBytes.Any() && fileBytes.Where(t=>file.ReferenceID.Contains(t.Key)).Any())
                {
                    response= fileBytes.Where(t => file.ReferenceID.Contains(t.Key)).FirstOrDefault().Value;
                }
                return Ok(response);
            }
            catch { throw; }
        }

        //[Route("AWS/DownloadAll")]
        //[HttpPost]
        //public async Task<IActionResult> DownloadFiles(FileRequest fileName)
        //{
        //    try
        //    {
        //        byte[] fileBytes = await fileSrv.DownloadFiles(fileName);
        //        return File(fileBytes, "application/octet-stream", fileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error downloading file: {ex.Message}");
        //    }
        //}

        /// <summary>
        /// Download selected file from AWS Folder based on filename
        /// </summary>
        /// <param name="file">ReferenceID from the Entry (ex: JournalEntryID ), Filename to download</param>
        /// <returns>Confirmation for Delete</returns>
        //[Route("AWS/DownloadFile")]
        //[HttpPost]
        //public async Task<IActionResult> DownloadFile(FileRequest file)
        //{
        //    FileResponse? response = null;
        //    try
        //    {
        //        if (file == null || (file != null && string.IsNullOrEmpty(file.FileName)))
        //            return StatusCode(StatusCodes.Status400BadRequest, Constants.RQD_FILE_NAME_TO_DOWNLOAD);
        //        response = await fileSrv.DownloadFiles(file);
        //        if (response != null && response.FileCount > 0)
        //            return Ok(response);
        //        else
        //            return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Message)) ? response.Message : Constants.MSG_ENDPOINT_ERROR);
        //    }
        //    catch { throw; }
        //    finally { response = null; }
        //}

        /// <summary>
        /// Delete Files Stored in AWS S3 Bucket & Update Details in Database
        /// </summary>
        /// <param name="file">ReferenceID from the Entry (ex: JournalEntryID )</param>
        /// <returns>Confirmation for Delete</returns>
        [Route("AWS/DeleteAll")]
        [HttpPost]
        public async Task<IActionResult> DeleteAllFiles(FileRequest file)
        {
            FileResponse? response = null;
            try
            {
                if (file != null)
                    file.FileName = null; //since we have to delete all files related to reference id

                response = await fileSrv.DeleteFiles(file);
                if (response != null && response.FileCount > 0)
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Message)) ? response.Message : Constants.MSG_ENDPOINT_ERROR);

            }
            catch { throw; }
            finally { response = null; }

        }

        /// <summary>
        /// Delete selected file from AWS Folder based on the filename & Update Details in Database
        /// </summary>
        /// <param name="file">ReferenceID from the Entry (ex: JournalEntryID ), selected filename to Delete</param>
        /// <returns>Confirmation for Delete</returns>
        [Route("AWS/DeleteFile")]
        [HttpPost]
        public async Task<IActionResult> DeleteFile(FileRequest file)
        {
            FileResponse? response = null;   
            try
            {
                if (file == null || (file != null && string.IsNullOrEmpty(file.FileName)))
                    return StatusCode(StatusCodes.Status400BadRequest, Constants.RQD_FILE_NAME_TO_DELETE);

                response = await fileSrv.DeleteFiles(file);
                if (response != null && response.FileCount > 0)
                    return Ok(response);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(response.Message)) ? response.Message : Constants.MSG_ENDPOINT_ERROR);

            }
            catch { throw; }
            finally { response = null; }

        }

        /// <summary>
        /// To view Files stored in AWS by cross-checking the database records 
        /// </summary>
        /// <param name="file">ReferenceID from the Entry (ex: JournalEntryID )</param>
        /// <returns>Displays File name and Details</returns>
        [Route("AWS/GetFiles")]
        [HttpPost]
        public async Task<IActionResult> GetFiles(FileRequest file)
        {
            dynamic res;
            try
            {
                res = await fileSrv.GetFiles(file);
                if (res != null)
                    return Ok(res);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, res);
            }
            catch { throw; }
            finally { res = null; }
        }

        #endregion
    }
}
