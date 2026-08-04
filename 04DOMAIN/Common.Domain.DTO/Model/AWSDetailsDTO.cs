using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{
    public class AWSDetailsDTO
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string[] Files { get; set; }
        public string IOBucketName { get; set; }
        public string ImagesFolder { get; set; }
        public string S3Path { get; set; }

    }
}
