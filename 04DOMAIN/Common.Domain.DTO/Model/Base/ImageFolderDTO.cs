using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model.Base
{
    public  class NimbleImageFolderPathDTO
    {
        public string ImagesFolder { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string BucketRegion { get; set; }
        public string IOBucketName { get; set; }
    }
}
