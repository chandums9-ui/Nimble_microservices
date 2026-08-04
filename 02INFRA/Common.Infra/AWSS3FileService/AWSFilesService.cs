using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Extensions;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Common.Infra.AWSS3FileService
{
    public class AWSServices : IAWSFileService, IDisposable
    {
        private readonly AWSSettingsDTO awsSettings;
        private readonly ILoggerService logger;

        #region Ctor
        public AWSServices(IOptions<AWSSettingsDTO> AWSSettingsAccessor, ILoggerService logger)
        {

            //this._configuration = Configuration;
            this.awsSettings = AWSSettingsAccessor.Value;
            this.logger = logger;
        }
        #endregion
        #region Methods
        public async Task<AWSFileResponse> DeleteFiles(UploadFilesReq FileReq, string clientName)
        {

            using (IAmazonS3 s3Client = new AmazonS3Client(awsSettings.AccessKey, awsSettings.SecretKey, GetBuccketRegion()))
            {
                AWSFileResponse awsResp = new AWSFileResponse();
                string foldername = $"{clientName}/{FileReq.CorporationID}/{FileReq.Type}/{FileReq.RefID}";
                foreach (AWSFileInfo file in FileReq.Files)
                {
                    DeleteObjectRequest delete = new DeleteObjectRequest
                    {
                        BucketName = awsSettings.BucketName,
                        Key = foldername + "/" + file.FileName,
                    };
                    DeleteObjectResponse delResp = await s3Client.DeleteObjectAsync(delete);
                    awsResp.Files.Add(new FileUploadResp { FileName = file.FileName, Message = "file deleted", Status = (int)HttpStatusCode.OK });
                    awsResp.RefID = FileReq.RefID;
                }
                awsResp.StatusCode = (int)HttpStatusCode.OK;
                awsResp.Status = "File found";
                return awsResp;
            }

        }
        public async Task<StatusDTO> DownloadFilesFromAWS(AWSDetailsDTO awsdetails)
        {
            StatusDTO res = new StatusDTO();
            try
            {
                string download = awsdetails.ImagesFolder + "\\";
                IAmazonS3 S3Client = new AmazonS3Client(awsdetails.AccessKey, awsdetails.SecretKey, Amazon.RegionEndpoint.USEast1);
                {
                    foreach (var file in awsdetails.Files)
                    {
                        if (file != null)
                        {
                            GetObjectRequest getObjectRequest = new GetObjectRequest
                            {
                                BucketName = awsdetails.IOBucketName,
                                Key = file
                            };

                            using (var fileresponse = await S3Client.GetObjectAsync(getObjectRequest))
                            {
                                string downPath = download + file.Split('/').Last();
                                await fileresponse.WriteResponseStreamToFileAsync(downPath, false, new CancellationTokenSource().Token);
                                fileresponse.Dispose();
                            }
                        }
                    }
                }                
                res.Status = Constants.MSG_FILES_DOWNLOAD_SUCCESS;
                res.StatusCode = StatusCodes.Status200OK;
                return res;
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error @DownloadFilesFromAWS: " + ex.DeepParseMessage());
                res.Status = Constants.MSG_NOT_SAVE;
                res.StatusCode = StatusCodes.Status500InternalServerError;
                return res;
            }
            finally
            {
                res = null;
            }

        }
        public async Task<AWSFileDownloadResp> DownloadAll(AWSFileResponse FileReq, string clientName)
        {

            using (IAmazonS3 s3Client = new AmazonS3Client(awsSettings.AccessKey, awsSettings.SecretKey, GetBuccketRegion()))
            {
                AWSFileDownloadResp awsResp = new AWSFileDownloadResp();
                AWSDownloadFiles awsFile = new AWSDownloadFiles();
                string foldername = $"{clientName}/{FileReq.CorporationID}/{FileReq.Type}/{FileReq.RefID}";
                foreach (FileUploadResp file in FileReq.Files)
                {
                    var request = new GetObjectRequest
                    {
                        BucketName = awsSettings.BucketName,
                        Key = foldername + "/" + file.FileName,
                    };

                    using (GetObjectResponse response = await s3Client.GetObjectAsync(request))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await response.ResponseStream.CopyToAsync(memoryStream);
                            awsFile.Files.Add(new AWSFileInfo { FileName = file.FileName, FileContent = memoryStream.ToArray() });
                        }
                    }

                }
                awsResp.File = awsFile;
                awsResp.StatusCode = (int)HttpStatusCode.OK;
                awsResp.Status = "File found";
                return awsResp;
            }
        }

        private string ReplaceCharactrs(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return fileName;

            var result = fileName.Select(c =>
            {
                // Convert any dash punctuation to normal hyphen
                if (char.GetUnicodeCategory(c) == UnicodeCategory.DashPunctuation)
                    return '-';

                // Handle specific symbols outside DashPunctuation
                return c switch
                {
                    '−' => '-', // Unicode minus sign
                    '‐' => '-', // Hyphen
                    '-' => '-', // Non-breaking hyphen
                    '‒' => '-', // Figure dash
                    '–' => '-', // En dash
                    '—' => '-', // Em dash
                    '―' => '-', // Horizontal bar

                    '＿' => '_', // Full width underscore

                    '＠' => '@',
                    '＃' => '#',
                    '＝' => '=',
                    '＋' => '+',
                    '＆' => '&',

                    '（' => '(',
                    '）' => ')',

                    '［' => '[',
                    '］' => ']',

                    '｛' => '{',
                    '｝' => '}',

                    '，' => ',',
                    '．' => '.',

                    _ => c
                };
            });

            return new string(result.ToArray());
        }

        public async Task<AWSFileResponse> UploadFiles(UploadFilesReq FileReq, string clientName)
        {

            using (IAmazonS3 s3Client = new AmazonS3Client(awsSettings.AccessKey, awsSettings.SecretKey, GetBuccketRegion()))
            {
                AWSFileResponse awsResp = new AWSFileResponse();

                FileReq.RefID = !string.IsNullOrEmpty(FileReq.RefID) ? FileReq.RefID :new PFAID().ToString();

                string foldername = $"{clientName}/{FileReq.CorporationID}/{FileReq.Type}/{FileReq.RefID}";
                
                decimal maxFileslimit = Math.Round((decimal)FileReq.Files.Sum(e => e.Length) / 1048576, 10);
                decimal confMaxFileSize = Convert.ToDecimal(awsSettings.MaxFileSizeMB);
                if (confMaxFileSize <= 20)
                {
                    awsResp.CorporationID = FileReq.CorporationID;
                    awsResp.RefID = FileReq.RefID;
                    awsResp.Type = FileReq.Type;
                    awsResp.FolderPath = foldername;
                    List<String> existingFiles = await GetFilesinFolder(foldername, s3Client);
                    foreach (AWSFileInfo file in FileReq.Files)
                    {
                        using (TransferUtility transferUtility = new TransferUtility(s3Client))
                        {
                            logger.LogFile($"Before FileReq.Files: {JsonConvert.SerializeObject($"{string.Join(" ", System.Text.Encoding.Unicode.GetBytes(file.FileName))}\n")}" + Environment.NewLine, "FilesView", "S3Files");
                            file.FileName = ReplaceCharactrs(file.FileName);
                            logger.LogFile($"After FileReq.Files: {JsonConvert.SerializeObject($"{string.Join(" ", System.Text.Encoding.Unicode.GetBytes(file.FileName))}\n")}" + Environment.NewLine, "FilesView", "S3Files");


                            string filepath = $"{foldername}/{file.FileName.Trim()}";
                            if (!existingFiles.Contains(file.FileName.Trim()))
                            {
                                MemoryStream fileStream = new System.IO.MemoryStream(file.FileContent);
                                var uploadRequest = new TransferUtilityUploadRequest
                                {
                                    BucketName = awsSettings.BucketName,
                                    InputStream = fileStream,
                                    StorageClass = S3StorageClass.Standard,
                                    CannedACL = S3CannedACL.Private,
                                    Key = filepath
                                };
                                await transferUtility.UploadAsync(uploadRequest);
                                awsResp.Files.Add(new FileUploadResp { FileName = file.FileName, Length = file.Length, Message = "file uploaded", Status = (int)HttpStatusCode.OK });
                            }
                        }
                    }
                    awsResp.StatusCode = (int)HttpStatusCode.OK;
                    awsResp.Status = "File uploaded successfully";
                }
                else
                {
                    awsResp.StatusCode = (int)HttpStatusCode.NotFound;
                    awsResp.Status = "Not able to create folder or access folder";
                }
                return awsResp;
            }
        }
        public async Task<FilesViewResponse> GetFiles(AWSFileResponse FileReq, string clientName)
        {
            FilesViewResponse filesResp = new FilesViewResponse();
            using (IAmazonS3 s3Client = new AmazonS3Client(awsSettings.AccessKey, awsSettings.SecretKey, GetBuccketRegion()))
            {
                AWSFileResponse awsResp = new AWSFileResponse();
                string foldername = $"{clientName}/{FileReq.CorporationID}/{FileReq.Type}/{FileReq.RefID}";
                var request = new ListObjectsV2Request
                {
                    BucketName = awsSettings.BucketName,
                    Prefix = foldername.TrimEnd('/') + "/", // Ensure trailing slash for folder
                    Delimiter = "/"
                };
                // Get existing files in the folder
                var listObjectsResponse = await s3Client.ListObjectsV2Async(request);

                if (listObjectsResponse != null)
                {
                    if (listObjectsResponse.S3Objects.Any())
                    {
                        logger.LogFile($"File View: {JsonConvert.SerializeObject($"{string.Join(" ", System.Text.Encoding.Unicode.GetBytes(listObjectsResponse.S3Objects.FirstOrDefault().Key))}\n")}" + Environment.NewLine, "FilesView", "S3Files");
                    }
                    foreach (FileUploadResp file in FileReq.Files)
                    {
                        string encodeFileName = Uri.EscapeDataString(file.FileName);
                        string key = foldername + "/" + file.FileName;
                        S3Object? s3Object = listObjectsResponse.S3Objects.Find(e => e.Key == key);
                        if (s3Object != null)
                        {
                            key = Uri.EscapeDataString(key);
                            filesResp.Files.Add(new FileView()
                            {
                                Filename = file.FileName,
                                AwsPath = $"https://{awsSettings.BucketName}.s3.us-east-1.amazonaws.com/{foldername + "/" + encodeFileName}",
                                ReferenceID=FileReq.RefID
                            });
                        }
                    }
                }
                if (filesResp.Files.Any())
                {
                    filesResp.StatusCode = (int)HttpStatusCode.OK;
                    filesResp.Status = "File found";
                }
                else
                {
                    filesResp.StatusCode = (int)HttpStatusCode.NotFound;
                    filesResp.Status = "File not found";
                }
                return filesResp;
            }

        }

        public async Task<AWSBulkDownloadResp> BulkDownloadAll(List<AWSFileResponse> FileReqs, string clientName)
        {

            using (IAmazonS3 s3Client = new AmazonS3Client(awsSettings.AccessKey, awsSettings.SecretKey, GetBuccketRegion()))
            {
                AWSBulkDownloadResp awsBulkResp = new AWSBulkDownloadResp();
                foreach (AWSFileResponse FileReq in FileReqs)
                {
                    if (FileReq.Files.Count() > 0)
                    {
                        string foldername = $"{clientName}/{FileReq.CorporationID}/{FileReq.Type}/{FileReq.RefID}";
                        AWSDownloadFiles downloadFile = new AWSDownloadFiles();
                        downloadFile.CorporationID = FileReq.CorporationID;
                        downloadFile.RefID = FileReq.RefID;
                        downloadFile.Type = FileReq.Type;
                        foreach (FileUploadResp file in FileReq.Files)
                        {
                            var request = new GetObjectRequest
                            {
                                BucketName = awsSettings.BucketName,
                                Key = foldername + "/" + file.FileName,
                            };

                            using (GetObjectResponse response = await s3Client.GetObjectAsync(request))
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    await response.ResponseStream.CopyToAsync(memoryStream);
                                    downloadFile.Files.Add(new AWSFileInfo { FileName = file.FileName, FileContent = memoryStream.ToArray() });
                                }
                            }
                        }
                        awsBulkResp.Files.Add(downloadFile);
                    }
                }
                awsBulkResp.StatusCode = (int)HttpStatusCode.OK;
                awsBulkResp.Status = "Files Downloaded successfully";
                return awsBulkResp;
            }
        }
        public async Task<AWSFileResponse> MoveOCRFiles(MoveFilesReq Req)
        {
            AWSFileResponse response = new AWSFileResponse();
            string[] files = Req.FilePath.Split(',');

            if (files.Count() > 0)
            {

                using (IAmazonS3 s3Client = new AmazonS3Client(awsSettings.IOAccessKey, awsSettings.IOSecretKey, GetBuccketRegion()))
                {

                    var bucketName = awsSettings.IOBucketName;

                    string targetPath = $"{Req.ClientName}/{Req.CorporationID}/{Req.Type}/{Req.RefID}";
                   // bool folderExists = await CheckFolderExistsAsync(targetPath);
                    response.FolderPath = targetPath;
                    IAmazonS3 destClient = new AmazonS3Client(awsSettings.AccessKey, awsSettings.SecretKey, GetBuccketRegion());
                    //if (!folderExists)
                    //{
                    //    folderExists = await AWSFolderCreate(targetPath, destClient);
                    //}
                    foreach (string file in files)
                    {
                        var listRequest = new ListObjectsV2Request
                        {
                            BucketName = bucketName,
                            Prefix = file
                        };

                        var listResponse = await s3Client.ListObjectsV2Async(listRequest);

                        if (listResponse.S3Objects.Count == 0)
                        {
                            response.Status = Constants.MSG_FILE_NO_DATA_FOUND;
                            response.StatusCode = StatusCodes.Status404NotFound;
                        }
                        else
                        {

                            //if (folderExists)
                            //{
                                foreach (var s3Object in listResponse.S3Objects)
                                {
                                    if (string.IsNullOrEmpty(s3Object.Key) && s3Object.Size! > 0)
                                        continue;
                                    string sourceKey = s3Object.Key;
                                    var fileName = Path.GetFileName(sourceKey);

                                    var copyRequest = new CopyObjectRequest
                                    {
                                        SourceBucket = bucketName,
                                        SourceKey = sourceKey,
                                        DestinationBucket = awsSettings.BucketName,
                                        DestinationKey = targetPath + "/" + fileName.Trim()
                                    };

                                    var copyResponse = await s3Client.CopyObjectAsync(copyRequest);
                                    if (copyResponse.HttpStatusCode != HttpStatusCode.OK)
                                    {
                                        response.Status += $"{fileName},";
                                    }
                                    else
                                    {
                                        response.Files.Add(new FileUploadResp
                                        {
                                            FileName = fileName,
                                            Length = s3Object.Size,
                                            Status = (short)Status.Active
                                        });
                                    }
                                }
                            //}
                         
                        }
                    }
                    if (response.Files.Count() > 0)
                    {
                        response.StatusCode = string.IsNullOrEmpty(response.Status) ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError;
                        response.Status += string.IsNullOrEmpty(response.Status) ? Constants.MSG_FILE_UPLOAD_SUCCESS : Constants.MSG_FILE_UPLOAD_FAILED;

                    }
                    else
                    {
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        response.Status = Constants.MSG_FILE_UPLOAD_FAILED;
                    }
                }

            }
            else
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Status = Constants.MSG_FILE_NO_DATA_FOUND;
            }
            return response;
        }
        private async Task<List<string>> GetFilesinFolder(string folderName, IAmazonS3 s3Client)
        {
            List<string> existingFiles = new List<string>();
            var request = new ListObjectsV2Request
            {
                BucketName = awsSettings.BucketName,
                Prefix = folderName.TrimEnd('/') + "/", // Ensure trailing slash for folder
                Delimiter = "/"
            };
            // Get existing files in the folder
            var listObjectsResponse = await s3Client.ListObjectsV2Async(request);
            foreach (var obj in listObjectsResponse.S3Objects)
            {
                existingFiles.Add(obj.Key);
            }
            return existingFiles;
        }
        private RegionEndpoint GetBuccketRegion()
        {
            return RegionEndpoint.GetBySystemName(awsSettings.BucketRegion);
        }
        private async Task<bool> AWSFolderCreate(string Foldername, IAmazonS3 s3Client)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = awsSettings.BucketName,
                Key = Foldername

            };

            PutObjectResponse response = await s3Client.PutObjectAsync(request);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        private async Task<bool> CheckFolderExistsAsync(string folderPath)
        {
            using (var s3Client = new AmazonS3Client(awsSettings.AccessKey, awsSettings.SecretKey, GetBuccketRegion()))
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = awsSettings.BucketName,
                    Prefix = folderPath.TrimEnd('/') + "/", // Ensure trailing slash for folder
                    Delimiter = "/"
                };

                var response = await s3Client.ListObjectsV2Async(request);
                return response.S3Objects.Any() || response.CommonPrefixes.Any();
            }
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

            }
            // Dispose unmanaged resources
        }

    }
}
#endregion
