using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model.Base
{
    public class AWSSettingsDTO
    {
        public string AccessKey { get; set; }
        public string BucketName { get; set; }
        public string BucketRegion { get; set; }
        public string DownloadFilePath { get; set; }
        public string SecretKey { get; set; }

        public string MaxFileSizeMB { get; set; }

        public string NoOfFilesLimit { get; set; }
        public string IOBucketName { get; set; }
        public string IOSecretKey { get; set; }

        public string IOAccessKey { get; set; }
        
    }

    //public class JWTSettingsDTO
    //{
    //    public string ClientSecret { get; set; }
    //    public string ClientID { get; set; }
    //    public string Issuer { get; set; }

    //    public string UserID { get; set; }
    //}
    public class ExternalApiURLs
    {
        public string CoreApiUrl { get; set; }
        public string UserMngtApiUrl { get; set; }
        public string STRApiUrl { get; set; }
    }
}
