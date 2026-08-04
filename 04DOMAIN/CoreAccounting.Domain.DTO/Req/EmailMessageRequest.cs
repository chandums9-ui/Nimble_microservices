using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Req
{
    public class EmailMessageRequest
    {
        public string? To { get; set; }
        public string? Cc { get; set; }
        public string? Bcc { get; set; }
        public string? Subject { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;

        // public List<IFormFile> Attachments { get; set; }
        public string? AttachmentPaths { get; set; }
    }
}
