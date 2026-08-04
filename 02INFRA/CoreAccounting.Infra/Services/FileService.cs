using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Extensions;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using CoreAccounting.App.Contracts;
using DataModel.Domain.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Cryptography.Xml;
using static System.Net.WebRequestMethods;

namespace CoreAccounting.Infra.Services
{
    public class FileService : IFileService, IDisposable
    {
        private readonly AWSSettingsDTO _awsSettings;
        //private readonly IConfiguration _configuration;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILoggerService logger;

        #region Fields

        private static string? bucketName;
        private string? accessKey;
        private string? secretKey;
        private RegionEndpoint bucketRegion;
        private static IAmazonS3 s3Client;
        private string directoryPath;

        #endregion

        #region Ctor
        public FileService(IUnitOfWork UnitOfWork, IConfiguration Configuration, IOptions<AWSSettingsDTO> AWSSettingsAccessor, ILoggerService logger)
        {
            this.unitOfWork = UnitOfWork;
            //this._configuration = Configuration;
            this._awsSettings = AWSSettingsAccessor.Value;
            this.logger = logger;

            LoadAWSCredentials();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Upload Multiple files to AWS S3 bucket using the ReferenceID & Update Details in Database
        /// </summary>
        /// <param name="files"> File Data from Upload control </param>
        /// <param name="ReferenceId"> ReferenceID from the Entry (ex: JournalEntryID )</param>
        /// <returns> Status of file upload (Success / Failed) </returns>
        public async Task<FileResponse> UploadFiles(List<UploadFileRequest> UploadFiles, string ClientName, string ReferenceId, string CorpId, string TypeName)
        {
            FileResponse? fileResp = new FileResponse();
            List<UploadFileStreem>? lstFileStreem = new List<UploadFileStreem>();
            decimal? maxFileslimit;
            try
            {
                if (UploadFiles != null && UploadFiles.Count > 0 && UploadFiles.Any())
                {
                    maxFileslimit = Math.Round((decimal)UploadFiles.Sum(e => e.Length) / 1000000, 10);
                    if (maxFileslimit <= 20)
                    {
                        foreach (var file in UploadFiles)
                        {
                            if (file.FileContent != null && file.FileContent.Length > 0)
                                lstFileStreem.Add(new UploadFileStreem() { FileName = file.FileName, Length = file.Length, Stream = new System.IO.MemoryStream(file.FileContent) });
                        }
                        fileResp = await this.UploadFilesToAWS(lstFileStreem, ClientName, ReferenceId, CorpId, TypeName);
                    }
                    else
                        fileResp.Message = Constants.MSG_FILE_MAXLIMIT_EXCEEDED;
                }

                return fileResp;
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error @UploadFiles - " + ex.DeepParseMessage());
                fileResp.Message += (!string.IsNullOrEmpty(fileResp.Message) ? Environment.NewLine : "") + Constants.MSG_FILE_UPLOAD_FAILED + " " + ex.Message;
                return fileResp;
            }
            finally
            {
                fileResp = null;
                lstFileStreem = null;
            }
        }

        /// <summary>
        /// Upload Multiple files to AWS S3 bucket using the ReferenceID & Update Details in Database
        /// </summary>
        /// <param name="files"> File Data from Upload control </param>
        /// <param name="ReferenceId"> ReferenceID from the Entry (ex: JournalEntryID )</param>
        /// <returns> Status of file upload (Success / Failed) </returns>
        public async Task<FileResponse> UploadFiles(List<IFormFile> UploadFiles, string ClientName, string ReferenceId, string CorpId, string TypeName)
        {
            FileResponse? fileResp = new FileResponse();
            decimal? maxFileslimit;
            List<UploadFileStreem>? lstFileStreem = new List<UploadFileStreem>();
            try
            {
                if (UploadFiles != null && UploadFiles.Count > 0 && UploadFiles.Any())
                {
                    maxFileslimit = Math.Round((decimal)UploadFiles.Sum(e => e.Length) / 1000000, 10);
                    if (maxFileslimit <= 20)
                    {
                        foreach (var file in UploadFiles)
                        {
                            lstFileStreem.Add(new UploadFileStreem() { FileName = file.FileName, Length = file.Length, Stream = file.OpenReadStream() });
                        }
                        fileResp = await this.UploadFilesToAWS(lstFileStreem, ClientName, ReferenceId, CorpId , TypeName);

                        return fileResp;
                    }
                    else
                        fileResp.Message = Constants.MSG_FILE_MAXLIMIT_EXCEEDED;
                }

                return fileResp;
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error @UploadFiles - " + ex.DeepParseMessage());
                fileResp.Message += (!string.IsNullOrEmpty(fileResp.Message) ? Environment.NewLine : "") + Constants.MSG_FILE_UPLOAD_FAILED + " " + ex.Message;
                return fileResp;
            }
            finally
            {
                fileResp = null;
                lstFileStreem = null;
            }
        }

        /// <summary>
        /// To Create a Folder in AWS S3 bucket
        /// </summary>
        /// <param name="Foldername">name for the folder</param>
        /// <returns></returns>
        public async Task AWSFolderCreate(string Foldername)
        {
            string folderPath = Foldername;
            s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = folderPath

            };

            PutObjectResponse response = await s3Client.PutObjectAsync(request);

        }

        /// <summary>
        /// Dowloads all/selected files from AWS S3 bucket to local storage and updates details in Database
        /// </summary>
        /// <param name="FileReq">ReferenceID from the Entry (ex: JournalEntryID ) and Selected file name to be deleted</param>
        /// <returns> Confirmation message for download and files will be downloaded </returns>
        //public async Task<FileResponse> DownloadFiles(FileRequest FileReq)
        //{
        //    List<ImportDocumentDetails>? docDetails = null;
        //    FileResponse? fileResp = new FileResponse();
        //    ImportDocument? importDoc;
        //    byte[]? jId;
        //    try
        //    {
        //        if (FileReq.ReferenceID.StartsWith("0x"))
        //        {
        //            FileReq.ReferenceID = FileReq.ReferenceID.Substring(2);
        //        }
        //        jId = new PFAID(FileReq.ReferenceID).UID;

        //        importDoc = (await unitOfWork.ImportDocuments.GetAll(e => e.JournalEntryId == jId && e.Status == 1)).ToList().FirstOrDefault();
        //        if (importDoc != null)
        //        {
        //            docDetails = (await unitOfWork.ImportDocumentDetails.GetAll(e => e.ImportDocumentId == importDoc.Id && e.Status == 1)).ToList();
        //            if (docDetails != null && docDetails.Any())
        //            {
        //                if (!string.IsNullOrEmpty(FileReq.FileName)) //Either single file/all files
        //                    docDetails = docDetails.Where(s => s.FileName == FileReq.FileName).ToList();

        //                fileResp = await DownloadFilesFromAWS(docDetails, importDoc, FileReq);
        //                if (fileResp.FileCount > 0)
        //                    fileResp.Message += (!string.IsNullOrEmpty(fileResp.Message) ? Environment.NewLine : "") + "Files downloaded at " + directoryPath;
        //            }
        //        }
        //        else
        //            fileResp.Message = Constants.MSG_NO_DATA_FOUND;

        //        return fileResp;
        //    }
        //    catch (Exception ex)
        //    {
        //        fileResp.Message += (!string.IsNullOrEmpty(fileResp.Message) ? Environment.NewLine : "") + Constants.MSG_FILE_DOWNLOAD_FAILED + " " + ex.Message;
        //        return fileResp;
        //    }
        //    finally
        //    {
        //        jId = null;
        //        docDetails = null;
        //        fileResp = null;
        //        importDoc = null;
        //    }
        //}


        //public async Task<FileResponse> DownloadFiles(FileRequest FileReq)
        //{
        //    List<ImportDocumentDetails>? docDetails = null;
        //    FileResponse? fileResp = new FileResponse();
        //    ImportDocument? importDoc;
        //    byte[]? jId;
        //    try
        //    {
        //        if (FileReq.ReferenceID.StartsWith("0x"))
        //        {
        //            FileReq.ReferenceID = FileReq.ReferenceID.Substring(2);
        //        }
        //        jId = new PFAID(FileReq.ReferenceID).UID;

        //        importDoc = (await unitOfWork.ImportDocuments.GetAll(e => e.JournalEntryId == jId && e.Status == 1)).ToList().FirstOrDefault();
        //        if (importDoc != null)
        //        {
        //            docDetails = (await unitOfWork.ImportDocumentDetails.GetAll(e => e.ImportDocumentId == importDoc.Id && e.Status == 1)).ToList();
        //            if (docDetails != null && docDetails.Any())
        //            {
        //                if (!string.IsNullOrEmpty(FileReq.FileName)) //Either single file/all files
        //                    docDetails = docDetails.Where(s => s.FileName == FileReq.FileName).ToList();

        //                fileResp = await DownloadFilesFromAWS(docDetails, importDoc, FileReq);
        //                if (fileResp.FileCount > 0)
        //                    fileResp.Message += (!string.IsNullOrEmpty(fileResp.Message) ? Environment.NewLine : "") + "Files downloaded at " + directoryPath;
        //            }
        //        }
        //        else
        //            fileResp.Message = Constants.MSG_NO_DATA_FOUND;

        //        return fileResp;
        //    }
        //    catch (Exception ex)
        //    {
        //        fileResp.Message += (!string.IsNullOrEmpty(fileResp.Message) ? Environment.NewLine : "") + Constants.MSG_FILE_DOWNLOAD_FAILED + " " + ex.Message;
        //        return fileResp;
        //    }
        //    finally
        //    {
        //        jId = null;
        //        docDetails = null;
        //        fileResp = null;
        //        importDoc = null;
        //    }
        //}

        //public async Task<Dictionary<string, byte[]>> DownloadFiles(FileRequest FileReq)
        //{
        //    List<ImportDocumentDetails>? docDetails = null;
        //    ImportDocument? importDoc;
        //    try
        //    {

        //        if (FileReq != null && !string.IsNullOrEmpty(FileReq.ReferenceID))
        //        {
        //            if (FileReq.ReferenceID.StartsWith("0x"))
        //            {
        //                FileReq.ReferenceID = FileReq.ReferenceID.Substring(2);
        //            }

        //            importDoc = (await unitOfWork.ImportDocuments.GetAll(e => e.JournalEntryId == new PFAID(FileReq.ReferenceID).UID && e.Status == 1)).ToList().FirstOrDefault();
        //            if (importDoc != null)
        //            {
        //                docDetails = (await unitOfWork.ImportDocumentDetails.GetAll(e => e.ImportDocumentId == importDoc.Id && e.Status == 1)).ToList();
        //                if (docDetails != null && docDetails.Any())
        //                {
        //                    if (!string.IsNullOrEmpty(FileReq.FileName)) //Either single file/all files
        //                        docDetails = docDetails.Where(s => s.FileName == FileReq.FileName).ToList();

        //                    return await DownloadFilesFromAWS(docDetails, importDoc, FileReq);
        //                }
        //            }
        //            else
        //                return new Dictionary<string, byte[]>();
        //        }

        //        return new Dictionary<string, byte[]>();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(Constants.MSG_FILE_DOWNLOAD_FAILED + " " + ex.Message);
        //    }
        //}


        public async Task<List<KeyValuePair<string, Dictionary<string, byte[]>>>> DownloadFiles(FileRequest FileReq)
        {
            List<KeyValuePair<string, Dictionary<string, byte[]>>> result = new List<KeyValuePair<string, Dictionary<string, byte[]>>>();

            try
            {
                if (FileReq != null && FileReq.ReferenceID != null && FileReq.ReferenceID.Any() && FileReq.ReferenceID.Count > 0)
                {
                    foreach (var referenceId in FileReq.ReferenceID)
                    {
                        string refId = referenceId;

                        if (refId.StartsWith("0x"))
                        {
                            refId = refId.Substring(2);
                        }

                        ImportDocument? importDoc = (await unitOfWork.ImportDocuments.GetAll(e => e.JournalEntryId == new PFAID(refId).UID && e.Status == 1)).ToList().FirstOrDefault();

                        if (importDoc != null)
                        {
                            List<ImportDocumentDetails>? docDetails = (await unitOfWork.ImportDocumentDetails.GetAll(e => e.ImportDocumentId == importDoc.Id && e.Status == 1)).ToList();

                            if (docDetails != null && docDetails.Any())
                            {
                                if (!string.IsNullOrEmpty(FileReq.FileName)) //Either single file/all files
                                {
                                    docDetails = docDetails.Where(s => s.FileName == FileReq.FileName).ToList();
                                }

                                // Call DownloadFilesFromAWS for each referenceId and document details
                                Dictionary<string, byte[]> fileData = await DownloadFilesFromAWS(docDetails, importDoc, FileReq);

                                // Add the result with the current referenceId as key
                                result.Add(new KeyValuePair<string, Dictionary<string, byte[]>>(refId, fileData));
                            }
                        }
                        else
                        {
                            // If no importDoc is found for the referenceId, add an empty dictionary with that referenceId
                            //result.Add(new KeyValuePair<string, Dictionary<string, byte[]>>(refId, new Dictionary<string, byte[]>()));
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.MSG_FILE_DOWNLOAD_FAILED + " " + ex.Message);
            }
        }




        //public async Task<FileResponse> DownloadFiles(FileRequest FileReq)
        //{
        //    try
        //    {
        //        using (var s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion))
        //        {
        //            string BucketName = bucketName;
        //            string key = folder+"/" + fileName;

        //            using (GetObjectResponse response = await s3Client.GetObjectAsync(new GetObjectRequest
        //            {
        //                BucketName = BucketName,
        //                Key = key
        //            }))
        //            {
        //                using (var memoryStream = new MemoryStream())
        //                {
        //                    await response.ResponseStream.CopyToAsync(memoryStream);
        //                    return memoryStream.ToArray();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // You can handle exceptions here or pass them to the caller
        //        throw new Exception($"Error downloading file: {ex.Message}", ex);
        //    }

        //}




        /// <summary>
        /// Delete all/selected file stored in AWS S3 bucket and updates details in Database
        /// </summary>
        /// <param name="FileReq">ReferenceID from the Entry (ex: JournalEntryID ) and Selected file name to be deleted</param>
        /// <returns>Confirmation for Delete</returns>
        public async Task<FileResponse> DeleteFiles(FileRequest FileReq)
        {
            FileResponse? fileResp = new FileResponse();
            List<ImportDocumentDetails>? idList = null;
            ImportDocument? doc;
            try
            {
                if (FileReq != null && FileReq.ReferenceID != null && FileReq.ReferenceID.Count > 0 && FileReq.ReferenceID.Any())
                {
                    foreach (var refid in FileReq.ReferenceID)
                    {
                        string referenceid = refid.ToString();
                        if (referenceid.StartsWith("0x"))
                        {
                            referenceid = referenceid.Substring(2);
                        }
                        byte[] jId = new PFAID(referenceid).UID;

                        doc = (await unitOfWork.ImportDocuments.GetAll(e => e.JournalEntryId == jId && e.Status == 1)).FirstOrDefault();
                        if (doc != null)
                        {
                            idList = (await unitOfWork.ImportDocumentDetails.GetAll(e => e.ImportDocumentId == doc.Id)).ToList();
                            if (idList != null && idList.Any())
                            {
                                if (!string.IsNullOrEmpty(FileReq.FileName)) //Either single file/all files
                                    idList = idList.Where(s => s.FileName == FileReq.FileName).ToList();

                                fileResp = await DeleteFilesFromAWS(idList, doc, FileReq);
                            }
                        }
                        else
                            fileResp.Message = Constants.MSG_NO_DATA_FOUND;

                    }
                }
                return fileResp;
            }
            catch (Exception ex)
            {
                fileResp.Message += (!string.IsNullOrEmpty(fileResp.Message) ? Environment.NewLine : "") + Constants.MSG_FILE_DOWNLOAD_FAILED + " " + ex.Message;
                return fileResp;
            }
            finally
            {
                fileResp = null;
            }
        }

        /// <summary>
        /// To view Files stored in AWS by cross-checking the database records 
        /// </summary>
        /// <param name="file">ReferenceID from the Entry (ex: JournalEntryID )</param>
        /// <returns>Displays File name and Details</returns>
        public async Task<List<dynamic>> GetFiles(FileRequest FileReq)
        {
            List<dynamic> fileList = new List<dynamic>();
            List<ImportDocumentDetails> importDoclist;
            S3Object? s3Object;
            FilesView fileView = new FilesView();
            ImportDocument? doc;
            ListObjectsResponse? objects;
            byte[] jId;
            string key;
            try
            {
                if (FileReq != null && FileReq.ReferenceID != null && FileReq.ReferenceID.Count>0 && FileReq.ReferenceID.Any())
                {
                    foreach(var item in FileReq.ReferenceID)
                    {
                        string refeid = item?.ToString();
                        if (!string.IsNullOrEmpty(refeid) && refeid.StartsWith("0x"))
                        {
                            refeid = refeid.Substring(2);
                        }
                        jId = new PFAID(refeid).UID;
                        doc = (await unitOfWork.ImportDocuments.GetAll(e => e.JournalEntryId == jId && e.Status == 1)).FirstOrDefault();
                        s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
                        objects = await s3Client.ListObjectsAsync(bucketName);
                        if (doc != null)
                        {
                            importDoclist = (await unitOfWork.ImportDocumentDetails.GetAll(e => e.ImportDocumentId == doc.Id && e.Status == 1)).ToList();
                            if (importDoclist.Count > 0)
                            {
                                foreach (var docDetail in importDoclist)
                                {
                                    //var result = doc.FolderPath.Split('/');
                                    //key = result[2] + "/" + FileReq.ReferenceID + "/" + docDetail.FileName;
                                    if (doc.FolderPath != null)
                                    {
                                        var folderParts = doc.FolderPath.TrimStart('~', '/')?.Split('/');
                                        var bucket = folderParts[0];
                                        var folder = string.Join("/", folderParts.Skip(1));
                                        key = folder + "/" + docDetail.FileName;
                                        s3Object = objects.S3Objects.Find(e => e.Key == key);
                                        if (s3Object != null)
                                        {
                                            fileList.Add(new FilesView()
                                            {
                                                Filename = docDetail.FileName,
                                                UploadTime = s3Object.LastModified.ToString(),
                                                AwsPath = $"https://{bucketName}.s3.us-east-1.amazonaws.com/{key}"
                                            });
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            fileList.Add(new FileResponse() { Message = Constants.MSG_FILE_NO_DATA_FOUND });
                        }
                    }
                }
                return fileList;
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error @GetFiles - " + ex.DeepParseMessage());
                fileList.Add(new FileResponse() { Message = Constants.MSG_FILE_GET_ERROR });
                return fileList;
            }
            finally
            {
                fileList = null;
                jId = null;
                importDoclist = null; ;
                key = null;
                s3Object = null; ;
                fileView = null;
                doc = null;
                s3Client = null;
            }

        }


        #endregion

        #region Private Methods
        /// <summary>
        /// Private method to upload files to AWS cloud storage
        /// </summary>
        /// <param name="UploadFiles">a class with all the upload files data</param>
        /// <param name="ReferenceId">ReferenceID from the Entry (ex: JournalEntryID )</param>
        /// <returns></returns>
        //private async Task<FileResponse> UploadFilesToAWS(List<UploadFileStreem> UploadFiles, string ClientName, string ReferenceId, string CorpId, string TypeName)
        //{
        //    FileResponse fileResp = new FileResponse();
        //    fileResp.Message = string.Empty;
        //    TransferUtilityUploadRequest? transferUtilityUploadRequest = null;
        //    TransferUtility? transferUtility = null;
        //    DirectoryInfo di;
        //    ImportDocument? Idoc = new ImportDocument();
        //    long partSize = 5 * 1024 * 1024;//5MB
        //    bool result;
        //    string filepath;
        //    string foldername = $"{ClientName}/{CorpId}/{TypeName}/{ReferenceId}";
        //    //string foldername = DateTime.Now.ToShortDateString().Replace('/', '-');
        //    int uploadedfiles = 0;
        //    int dbResp;
        //    try
        //    {
        //        s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
        //        transferUtility = new TransferUtility(s3Client);
        //        di = new DirectoryInfo(foldername);
        //        if (!di.Exists)
        //        {
        //            await AWSFolderCreate(foldername);
        //        }
        //        var refe = new PFAID(ReferenceId);
        //        var refid = refe.UID;
        //        //Get the existing DocID if any files already uploaded with the 'ReferenceId', otherwise create document entry in DB and get the DocID
        //        ImportDocument OldEntry = (await unitOfWork.ImportDocuments.GetAll(s=>s.JournalEntryId== refid && s.Status==1)).FirstOrDefault();
        //        //ImportDocument OldEntry = (await unitOfWork.ImportDocuments.GetAll(x => new PFAID(x.JournalEntryId).ToString() == ReferenceId && x.Status == 1)).FirstOrDefault();
        //        if (OldEntry != null && OldEntry.Status == 1)
        //        {
        //            Idoc.Id = OldEntry.Id;
        //        }
        //        else
        //        {
        //            Idoc.Id = 0;
        //            Idoc.JournalEntryId = new PFAID(ReferenceId).UID;
        //            Idoc.FolderPath = "~/" + bucketName + "/" + foldername;
        //            //Idoc.FolderPath = foldername;
        //            Idoc.Status = 1;
        //          await  unitOfWork.ImportDocuments.Add(Idoc);
        //            unitOfWork.Save();
        //        }

        //        if (Idoc.Id > 0)
        //        {
        //            foreach (var filedata in UploadFiles)
        //            {
        //                filepath = $"{foldername}/{filedata.FileName.Trim()}";
        //                //filepath = foldername+ReferenceId+filedata.FileName;

        //                //Check wether the file already uploaded to AWS
        //                result = s3Client.GetAllObjectKeysAsync(bucketName, "", new Dictionary<string, object>()).Result.Contains(filepath);
        //                if (!(bool)result)
        //                {
        //                    //Upload the file to AWS
        //                    transferUtilityUploadRequest = new TransferUtilityUploadRequest
        //                    {
        //                        BucketName = bucketName,
        //                        InputStream = filedata.Stream,
        //                        StorageClass = S3StorageClass.Standard,
        //                        CannedACL = S3CannedACL.Private,
        //                        PartSize = (long)partSize,
        //                        Key = filepath
        //                    };
        //                    await transferUtility.UploadAsync(transferUtilityUploadRequest);
        //                    transferUtility.Dispose();

        //                    //Save the file name & size in DB
        //                   await unitOfWork.ImportDocumentDetails.Add(new ImportDocumentDetails()
        //                    {
        //                        Id = 0,
        //                        ImportDocumentId = Idoc.Id,
        //                        FileName = filedata.FileName,
        //                        FileSize = Math.Round((decimal)filedata.Length / 1000000, 10).ToString(),
        //                        Status = 1
        //                    });
        //                    dbResp = unitOfWork.Save();
        //                    uploadedfiles++;

        //                    if (dbResp > 0)
        //                        fileResp.FileCount = uploadedfiles;
        //                    else
        //                        fileResp.Message += string.Format(Constants.MSG_FILE_DB_UPDTE_ERROR, filedata.FileName) + System.Environment.NewLine;
        //                }
        //                else
        //                    fileResp.Message += string.Format(Constants.MSG_FILE_EXISTS, filedata.FileName) + System.Environment.NewLine;

        //            }

        //            if (uploadedfiles > 0 && fileResp.FileCount == UploadFiles.Count())
        //                fileResp.Message = fileResp.FileCount + " " + Constants.MSG_FILE_UPLOAD_SUCCESS;
        //        }
        //        return fileResp;
        //    }
        //    catch (Exception ex)
        //    {
        //        fileResp.Message += (!string.IsNullOrEmpty(fileResp.Message) ? Environment.NewLine : "") + Constants.MSG_FILE_UPLOAD_FAILED + " " + ex.Message;
        //        return fileResp;
        //    }
        //    finally
        //    {
        //        transferUtilityUploadRequest = null;
        //        if (transferUtility != null)
        //            transferUtility.Dispose();
        //        transferUtility = null;
        //        di = null;
        //        Idoc = null;
        //        filepath = string.Empty;
        //        foldername = string.Empty;
        //    }
        //}


        private async Task<FileResponse> UploadFilesToAWS(List<UploadFileStreem> UploadFiles, string ClientName, string ReferenceId, string CorpId, string TypeName)
        {
            FileResponse fileResp = new FileResponse();
            fileResp.Message = string.Empty;
            ImportDocument Idoc;
            bool newAssign = false;
            s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);

            using (TransferUtility transferUtility = new TransferUtility(s3Client))
            {
                string foldername = $"{ClientName}/{CorpId}/{TypeName}/{ReferenceId}";
                bool folderExists = await CheckFolderExistsAsync(bucketName, foldername);
                if (!folderExists)
                {
                    await AWSFolderCreate(foldername);
                }

                var referenceId = new PFAID(ReferenceId);
                var refIdBytes = referenceId.UID;
                Idoc = (await unitOfWork.ImportDocuments.GetAll(s => s.JournalEntryId == refIdBytes && s.Status == 1)).FirstOrDefault();
                bool isNew = Idoc == null;
                if (isNew)
                {
                    Idoc = new ImportDocument();
                    Idoc.JournalEntryId = new PFAID(ReferenceId).UID;
                    Idoc.FolderPath = "~/" + bucketName + "/" + foldername;
                    Idoc.Status = 1;
                    await unitOfWork.ImportDocuments.Add(Idoc);
                    await unitOfWork.SaveAsync();
                    //newAssign = true;
                }


                if (Idoc.Id > 0 )
                {
                    newAssign = false;
                    int uploadedFilesCount = 0;
                    var existingFiles = new HashSet<string>();

                    var request = new ListObjectsV2Request
                    {
                        BucketName = bucketName,
                        Prefix = foldername.TrimEnd('/') + "/", // Ensure trailing slash for folder
                        Delimiter = "/"
                    };
                    // Get existing files in the folder
                    var listObjectsResponse = await s3Client.ListObjectsV2Async(request);
                    foreach (var obj in listObjectsResponse.S3Objects)
                    {
                        existingFiles.Add(obj.Key);
                    }

                    //var uploadRequests = new List<TransferUtilityUploadRequest>();
                    foreach (var fileData in UploadFiles)
                    {
                        string filepath = $"{foldername}/{fileData.FileName.Trim()}";
                        if (!existingFiles.Contains(filepath))
                        {
                            var uploadRequest = new TransferUtilityUploadRequest
                            {
                                BucketName = bucketName,
                                InputStream = fileData.Stream,
                                StorageClass = S3StorageClass.Standard,
                                CannedACL = S3CannedACL.Private,
                                Key = filepath
                            };
                            await transferUtility.UploadAsync(uploadRequest);

                            //uploadRequests.Add(uploadRequest);

                            // Save the file name & size in DB
                            await unitOfWork.ImportDocumentDetails.Add(new ImportDocumentDetails()
                            {
                                ImportDocumentId = Idoc.Id,
                                FileName = fileData.FileName,
                                FileSize = Math.Round((decimal)fileData.Length / 1000000, 10).ToString(),
                                Status = 1
                            });

                                //Idoc.ImportDocumentDetails.Add(new ImportDocumentDetails()
                                //{
                                //    //ImportDocumentId = Idoc.Id,
                                //    FileName = fileData.FileName,
                                //    FileSize = Math.Round((decimal)fileData.Length / 1000000, 10).ToString(),
                                //    Status = 1
                                //});

                            uploadedFilesCount++;
                        }
                        else
                        {
                            fileResp.Message += string.Format(Constants.MSG_FILE_EXISTS, fileData.FileName) + System.Environment.NewLine;
                        }
                    }

                    // Upload files in bulk
                    //await transferUtility.UploadAsync(uploadRequests);

                    //if (isNew)
                    //    await unitOfWork.ImportDocuments.Add(Idoc);
                    await unitOfWork.SaveAsync();

                    fileResp.FileCount = uploadedFilesCount;
                    if (uploadedFilesCount == UploadFiles.Count)
                    {
                        fileResp.Message = fileResp.FileCount + " " + Constants.MSG_FILE_UPLOAD_SUCCESS;
                    }
                }
            }

            return fileResp;
        }

        //private async Task<FileResponse> UploadFilesToAWS(List<UploadFileStreem> UploadFiles, string ClientName, string ReferenceId, string CorpId, string TypeName)
        //{
        //    FileResponse fileResp = new FileResponse();
        //    fileResp.Message = string.Empty;
        //    ImportDocument Idoc;
        //    bool newAssign = false;
        //    s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);

        //    using (TransferUtility transferUtility = new TransferUtility(s3Client))
        //    {
        //        string foldername = $"{ClientName}/{CorpId}/{TypeName}/{ReferenceId}";
        //        bool folderExists = await CheckFolderExistsAsync(bucketName, foldername);
        //        if (!folderExists)
        //        {
        //            await AWSFolderCreate(foldername);
        //        }

        //        var referenceId = new PFAID(ReferenceId);
        //        var refIdBytes = referenceId.UID;
        //        Idoc = (await unitOfWork.ImportDocuments.GetAll(s => s.JournalEntryId == refIdBytes && s.Status == 1)).FirstOrDefault();
        //        bool isNew = Idoc == null;
        //        if (isNew)
        //        {
        //            Idoc = new ImportDocument();
        //            Idoc.JournalEntryId = new PFAID(ReferenceId).UID;
        //            Idoc.FolderPath = "~/" + bucketName + "/" + foldername;
        //            //Idoc.FolderPath = foldername;
        //            Idoc.Status = 1;
        //            newAssign = true;

        //        }

        //        if (Idoc.Id > 0 || newAssign)
        //        {
        //            newAssign= false;
        //            int uploadedFilesCount = 0;
        //            foreach (var fileData in UploadFiles)
        //            {
        //                string filepath = $"{foldername}/{fileData.FileName.Trim()}";
        //                bool fileExists = s3Client.GetAllObjectKeysAsync(bucketName, "", new Dictionary<string, object>()).Result.Contains(filepath);
        //                if (!fileExists)
        //                {
        //                    TransferUtilityUploadRequest transferUtilityUploadRequest = new TransferUtilityUploadRequest
        //                    {
        //                        BucketName = bucketName,
        //                        InputStream = fileData.Stream,
        //                        StorageClass = S3StorageClass.Standard,
        //                        CannedACL = S3CannedACL.Private,
        //                        Key = filepath
        //                    };
        //                    await transferUtility.UploadAsync(transferUtilityUploadRequest);


        //                    //Save the file name & size in DB
        //                    Idoc.ImportDocumentDetails.Add(new ImportDocumentDetails()
        //                    {
        //                      //  Id = 0,
        //                       // ImportDocumentId = Idoc.Id,
        //                        FileName = fileData.FileName,
        //                        FileSize = Math.Round((decimal)fileData.Length / 1000000, 10).ToString(),
        //                        Status = 1
        //                    });

        //                    uploadedFilesCount++;
        //                }
        //                else
        //                {
        //                    fileResp.Message += string.Format(Constants.MSG_FILE_EXISTS, fileData.FileName) + System.Environment.NewLine;
        //                }
        //            }
        //            if(isNew)
        //                await unitOfWork.ImportDocuments.Add(Idoc);
        //            //unitOfWork.Save();
        //            await unitOfWork.SaveAsync();

        //            fileResp.FileCount = uploadedFilesCount;
        //            if (uploadedFilesCount == UploadFiles.Count)
        //            {
        //                fileResp.Message = fileResp.FileCount + " " + Constants.MSG_FILE_UPLOAD_SUCCESS;
        //            }
        //        }
        //    }

        //    return fileResp;

        //}



        private async Task<bool> CheckFolderExistsAsync(string bucketName, string folderPath)
        {
            try
            {
                
                using (var s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion))
                {
                    var request = new ListObjectsV2Request
                    {
                        BucketName = bucketName,
                        Prefix = folderPath.TrimEnd('/') + "/", // Ensure trailing slash for folder
                        Delimiter = "/"
                    };

                    var response = await s3Client.ListObjectsV2Async(request);
                    return response.S3Objects.Any() || response.CommonPrefixes.Any();
                }
            }
            catch (AmazonS3Exception)
            {
                return false; // Assume folder doesn't exist in case of error
            }
        }


        //private async Task<FileResponse> UploadFilesToAWS(List<UploadFileStreem> UploadFiles, string ClientName, string ReferenceId, string CorpId, string TypeName)
        //{
        //    FileResponse fileResp = new FileResponse();
        //    fileResp.Message = string.Empty;
        //    TransferUtilityUploadRequest? transferUtilityUploadRequest = null;
        //    TransferUtility? transferUtility = null;
        //    DirectoryInfo di;
        //    ImportDocument? Idoc = new ImportDocument();
        //    long partSize = 5 * 1024 * 1024;//5MB
        //    bool result;
        //    string filepath;
        //    string foldername = $"{ClientName}/{CorpId}/{TypeName}/{ReferenceId}";
        //    //string foldername = DateTime.Now.ToShortDateString().Replace('/', '-');
        //    int uploadedfiles = 0;
        //    int dbResp;
        //    try
        //    {
        //        s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
        //        transferUtility = new TransferUtility(s3Client);
        //        di = new DirectoryInfo(foldername);
        //        if (!di.Exists)
        //        {
        //            await AWSFolderCreate(foldername);
        //        }
        //        var refe = new PFAID(ReferenceId);
        //        var refid = refe.UID;
        //        //Get the existing DocID if any files already uploaded with the 'ReferenceId', otherwise create document entry in DB and get the DocID
        //        ImportDocument OldEntry = (await unitOfWork.ImportDocuments.GetAll(s => s.JournalEntryId == refid && s.Status == 1)).FirstOrDefault();
        //        //ImportDocument OldEntry = (await unitOfWork.ImportDocuments.GetAll(x => new PFAID(x.JournalEntryId).ToString() == ReferenceId && x.Status == 1)).FirstOrDefault();
        //        if (OldEntry != null && OldEntry.Status == 1)
        //        {
        //            Idoc.Id = OldEntry.Id;
        //        }
        //        else
        //        {
        //            Idoc.Id = 0;
        //            Idoc.JournalEntryId = new PFAID(ReferenceId).UID;
        //            Idoc.FolderPath = "~/" + bucketName + "/" + foldername;
        //            //Idoc.FolderPath = foldername;
        //            Idoc.Status = 1;
        //            await unitOfWork.ImportDocuments.Add(Idoc);
        //            unitOfWork.Save();
        //        }

        //        if (Idoc.Id > 0)
        //        {
        //            foreach (var filedata in UploadFiles)
        //            {
        //                filepath = $"{foldername}/{filedata.FileName.Trim()}";
        //                //filepath = foldername+ReferenceId+filedata.FileName;

        //                //Check wether the file already uploaded to AWS
        //                result = s3Client.GetAllObjectKeysAsync(bucketName, "", new Dictionary<string, object>()).Result.Contains(filepath);
        //                if (!(bool)result)
        //                {
        //                    //Upload the file to AWS
        //                    transferUtilityUploadRequest = new TransferUtilityUploadRequest
        //                    {
        //                        BucketName = bucketName,
        //                        InputStream = filedata.Stream,
        //                        StorageClass = S3StorageClass.Standard,
        //                        CannedACL = S3CannedACL.Private,
        //                        PartSize = (long)partSize,
        //                        Key = filepath
        //                    };
        //                    await transferUtility.UploadAsync(transferUtilityUploadRequest);
                            

        //                    //Save the file name & size in DB
        //                    await unitOfWork.ImportDocumentDetails.Add(new ImportDocumentDetails()
        //                    {
        //                        Id = 0,
        //                        ImportDocumentId = Idoc.Id,
        //                        FileName = filedata.FileName,
        //                        FileSize = Math.Round((decimal)filedata.Length / 1000000, 10).ToString(),
        //                        Status = 1
        //                    });
                            
        //                    uploadedfiles++;

        //                    if (dbResp > 0)
        //                        fileResp.FileCount = uploadedfiles;
        //                    else
        //                        fileResp.Message += string.Format(Constants.MSG_FILE_DB_UPDTE_ERROR, filedata.FileName) + System.Environment.NewLine;
        //                }
        //                else
        //                    fileResp.Message += string.Format(Constants.MSG_FILE_EXISTS, filedata.FileName) + System.Environment.NewLine;

        //            }
        //            unitOfWork.Save();

        //            if (uploadedfiles > 0 && fileResp.FileCount == UploadFiles.Count())
        //                fileResp.Message = fileResp.FileCount + " " + Constants.MSG_FILE_UPLOAD_SUCCESS;
        //        }
        //        return fileResp;
        //    }
        //    catch (Exception ex)
        //    {
        //        fileResp.Message += (!string.IsNullOrEmpty(fileResp.Message) ? Environment.NewLine : "") + Constants.MSG_FILE_UPLOAD_FAILED + " " + ex.Message;
        //        return fileResp;
        //    }
        //    finally
        //    {
                
        //        if (transferUtility != null)
        //            transferUtility.Dispose();
        //        transferUtility = null;
                
        //    }
        //}

        //public async Task<bool> FileExistsAsync(string bucketName, string key)
        //{
        //    try
        //    {
        //        var request = new GetObjectMetadataRequest
        //        {
        //            BucketName = bucketName,
        //            Key = key
        //        };

        //        await s3Client.GetObjectMetadataAsync(request);
        //        return true; // Object exists
        //    }
        //    catch (Amazon.S3.AmazonS3Exception e) when (e.StatusCode == HttpStatusCode.NotFound)
        //    {
        //        return false; // Object does not exist
        //    }
        //}


        /// <summary>
        /// Private method to download files from AWS
        /// </summary>
        /// <param name="impDocdetails">Documnets details from database table</param>
        /// <param name="doc">Import details from database</param>
        /// <param name="fileReq">ReferenceID from the Entry (ex: JournalEntryID ), selected filename to download</param>
        /// <returns></returns>
        //private async Task<FileResponse> DownloadFilesFromAWS(List<ImportDocumentDetails> impDocdetails, ImportDocument doc, FileRequest fileReq)
        //{
        //    int downloadCount = 0;
        //    FileResponse fileResp = new FileResponse();
        //    GetObjectRequest request;
        //    string? fileName = null;
        //    try
        //    {
        //        if (!Directory.Exists(directoryPath))
        //        {
        //            Directory.CreateDirectory(directoryPath);
        //        }
        //        s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
        //        //var result = doc.FolderPath.Split('/');
        //        //var bucket = result[1];
        //        //var folder = result[2];
        //        // Split the folder path to extract bucket and folder names
        //        var folderParts = doc.FolderPath.TrimStart('~', '/').Split('/');
        //        var bucket = folderParts[0];
        //        var folder = string.Join("/", folderParts.Skip(1)); // Join remaining parts as folder path

        //        foreach (var Detail in impDocdetails)
        //        {
        //            fileName = Detail.FileName;
        //            request = new GetObjectRequest
        //            {
        //                BucketName = bucketName,
        //                //Key = folder + "/" + fileReq.ReferenceID + "/" + Detail.FileName,
        //                Key = folder + "/"+ Detail.FileName,

        //            };

        //            using (GetObjectResponse response = await s3Client.GetObjectAsync(request))
        //            {
        //                //string filePath = directoryPath + "/" + folder + "/" + fileReq.ReferenceID + "/" + Detail.FileName;
        //                string filePath = directoryPath + "/" + folder + "/"+ Detail.FileName;
        //                await response.WriteResponseStreamToFileAsync(filePath, false, CancellationToken.None);
        //                response.Dispose();
        //            }
        //            downloadCount++;

        //            fileResp.Message += Detail.FileName + " downloaded." + System.Environment.NewLine;
        //            fileResp.FileCount = downloadCount;
        //        }
        //        if (downloadCount > 0 && impDocdetails.Count == downloadCount)
        //            fileResp.Message = Constants.MSG_FILE_DOWNLOAD_SUCCESS;

        //        return fileResp;
        //    }
        //    catch (Exception ex)
        //    {
        //        fileResp.Message += (!string.IsNullOrEmpty(fileResp.Message) ? Environment.NewLine : "") + Constants.MSG_FILE_DOWNLOAD_FAILED + " " + fileName;
        //        return fileResp;
        //    }
        //    finally
        //    {
        //        request = null;
        //    }

        //}


        //private async Task<List<byte[]>> DownloadFilesFromAWS(List<ImportDocumentDetails> impDocdetails, ImportDocument doc, FileRequest fileReq)
        //{
        //    //int downloadCount = 0;
        //    //FileResponse fileResp = new FileResponse();
        //    GetObjectRequest request;
        //    string? fileName = null;
        //    List<byte[]> fileBytesList = new List<byte[]>();
        //    try
        //    {
        //        //if (!Directory.Exists(directoryPath))
        //        //{
        //        //    Directory.CreateDirectory(directoryPath);
        //        //}
        //        s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
        //        //var result = doc.FolderPath.Split('/');
        //        //var bucket = result[1];
        //        //var folder = result[2];
        //        // Split the folder path to extract bucket and folder names
        //        var folderParts = doc.FolderPath.TrimStart('~', '/').Split('/');
        //        var bucket = folderParts[0];
        //        var folder = string.Join("/", folderParts.Skip(1)); // Join remaining parts as folder path

        //        foreach (var Detail in impDocdetails)
        //        {
        //            fileName = Detail.FileName;
        //            request = new GetObjectRequest
        //            {
        //                BucketName = bucketName,
        //                //Key = folder + "/" + fileReq.ReferenceID + "/" + Detail.FileName,
        //                Key = folder + "/" + Detail.FileName,

        //            };

        //            using (GetObjectResponse response = await s3Client.GetObjectAsync(request))
        //            {
        //                //string filePath = directoryPath + "/" + folder + "/" + fileReq.ReferenceID + "/" + Detail.FileName;
        //                //string filePath = directoryPath + "/" + folder + "/" + Detail.FileName;
        //                //await response.WriteResponseStreamToFileAsync(filePath, false, CancellationToken.None);

        //                using (var memoryStream = new MemoryStream())
        //                {
        //                    await response.ResponseStream.CopyToAsync(memoryStream);
        //                    fileBytesList.Add(memoryStream.ToArray());
        //                }
        //                response.Dispose();
        //            }
        //            //downloadCount++;

        //            //fileResp.Message += Detail.FileName + " downloaded." + System.Environment.NewLine;
        //            //fileResp.FileCount = downloadCount;
        //        }
        //        //if (downloadCount > 0 && impDocdetails.Count == downloadCount)
        //        //    fileResp.Message = Constants.MSG_FILE_DOWNLOAD_SUCCESS;

        //        return fileBytesList;
        //    }
        //    catch (Exception ex)
        //    {
        //        //fileResp.Message += (!string.IsNullOrEmpty(fileResp.Message) ? Environment.NewLine : "") + Constants.MSG_FILE_DOWNLOAD_FAILED + " " + fileName;
        //        return fileBytesList;
        //    }
        //    finally
        //    {
        //        request = null;
        //    }

        //}


        private async Task<Dictionary<string, byte[]>> DownloadFilesFromAWS(List<ImportDocumentDetails> impDocdetails, ImportDocument doc, FileRequest fileReq)
        {
            Dictionary<string, byte[]> fileDataDict = new Dictionary<string, byte[]>();
            try
            {
                // Split the folder path to extract bucket and folder names
                var folderParts = doc.FolderPath.TrimStart('~', '/').Split('/');
                var bucket = folderParts[0];
                var folder = string.Join("/", folderParts.Skip(1)); // Join remaining parts as folder path

                using (var s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion))
                {
                    foreach (var detail in impDocdetails)
                    {
                        var request = new GetObjectRequest
                        {
                            BucketName = bucket,
                            Key = folder + "/" + detail.FileName,
                        };

                        using (GetObjectResponse response = await s3Client.GetObjectAsync(request))
                        using (var memoryStream = new MemoryStream())
                        {
                            await response.ResponseStream.CopyToAsync(memoryStream);
                            fileDataDict.Add(detail.FileName, memoryStream.ToArray());
                        }
                    }
                }

                return fileDataDict;
            }
            catch (Exception ex)
            {
                // Log the exception here
                Console.WriteLine($"An error occurred: {ex.Message}");
                return fileDataDict;
            }
        }


        /// <summary>
        /// Private method to Delete files from AWS
        /// </summary>
        /// <param name="impDocdetails">Documnets details from database table</param>
        /// <param name="doc">Import details from database</param>
        /// <param name="FileReq">ReferenceID from the Entry (ex: JournalEntryID ), selected filename to Delete</param>
        /// <returns></returns>
        private async Task<FileResponse> DeleteFilesFromAWS(List<ImportDocumentDetails> impDocdetails, ImportDocument doc, FileRequest FileReq)
        {
            int filesDeleted = 0;
            string[] result;
            DeleteObjectResponse? awsResp = null;
            FileResponse fileResp = new FileResponse();
            string? fileName = null;
            try
            {
                foreach (var docDetail in impDocdetails)
                {
                    fileName = docDetail.FileName;
                    //result = doc.FolderPath.Split('/');
                    // Extract bucket and folder names from the document's folder path
                    var folderParts = doc.FolderPath.TrimStart('~', '/').Split('/');
                    var bucket = folderParts[0];
                    var folder = string.Join("/", folderParts.Skip(1)); // Join remaining parts as folder path
                    s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
                    DeleteObjectRequest delete = new DeleteObjectRequest
                    {
                        BucketName = bucketName,
                        Key = folder + "/" + docDetail.FileName,
                    };
                    awsResp = s3Client.DeleteObjectAsync(delete).Result;
                    if (awsResp == null)
                    {
                        fileResp.Message += string.Format(Constants.MSG_FILE_DELETE_ERROR, docDetail.FileName, Environment.NewLine);
                    }
                    else
                    {
                        
                        await unitOfWork.ImportDocumentDetails.Delete(docDetail);
                        filesDeleted += unitOfWork.Save();
                    }
                }
                int ClearRes = (await unitOfWork.ImportDocumentDetails.GetAll(x => x.ImportDocumentId == doc.Id && x.Status == 1)).ToList().Count;
                if (ClearRes == 0)
                {
                    doc.Status = 3;
                 await   unitOfWork.ImportDocuments.Update(doc);
                    unitOfWork.Save();
                }
                if (filesDeleted > 0 && filesDeleted == impDocdetails.Count)
                {
                    fileResp.Message = Constants.MSG_FILE_DELETE_SUCCESS;
                    fileResp.FileCount = filesDeleted;
                }
                else
                    fileResp.Message += " " + Constants.MSG_FILE_UPDATE_DB_ERROR;

                return fileResp;
            }
            catch (Exception ex)
            {
                fileResp.Message += (!string.IsNullOrEmpty(fileResp.Message) ? Environment.NewLine : "") + Constants.MSG_FILE_DELETE_FAILED + " - " + fileName;
                return fileResp;
            }
            finally
            {
                result = null;
                awsResp = null;
            }
        }

        /// <summary>
        /// Method to access AWS credentials
        /// </summary>
        private void LoadAWSCredentials()
        {
            // var Credentilas = _configuration.GetSection("AwsSettings").GetChildren().ToList();
            bucketName = _awsSettings.BucketName;//  Credentilas.Find(s => s.Key == "BucketName").Value;
            accessKey = _awsSettings.AccessKey;//  Credentilas.Find(s => s.Key == "AccessKey").Value;
            //string region = _awsSettings.BucketRegion;// Credentilas.Find(s => s.Key == "BucketRegion").Value;
            bucketRegion = RegionEndpoint.GetBySystemName(_awsSettings.BucketRegion);
            secretKey = _awsSettings.SecretKey;// Credentilas.Find(s => s.Key == "SecretKey").Value;
            directoryPath = _awsSettings.DownloadFilePath;// Credentilas.Find(s => s.Key == "DownloadFilePath").Value;
        }
        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources
                if (s3Client != null)
                {
                    s3Client.Dispose();
                    s3Client = null;
                }
            }
            // Dispose unmanaged resources
        }


        #endregion
    }



    internal class UploadFileStreem
    {
        /// <summary>
        /// Gets the file name from the Content-Disposition header.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets the file length in bytes.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// FileData Stream
        /// </summary>
        public Stream Stream { get; set; }
    }


}
