using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DailySales.Domain.DTO.Model
{
    public class PMSInfoDTO
    {
        public long ID { get; set; }
        public string pmsName { get; set; }
        public short pmsType { get; set; }
        public long PmsClientInfoID { get; set; }

        public string ClientID { get; set; }
        public long URLID { get; set; }
        public string FacilitityID { get; set; }
        public string CorpID { get; set; }
        public string StoreID { get; set; }
        public short Status { get; set; }
    }
    public class PMSInfoDetailsDTO
    {
        public long ID { get; set; }
        public int PMSInfoID { get; set; }
        public string FileName { get; set; }
        public short Mandatory { get; set; }
        public short Status { get; set; }
    }
    public class PMSClientInfoDTO
    {
        public long ID { get; set; }
        public long PMSInfoID { get; set; }
        public byte[] ClientID { get; set; }
        public int URLID { get; set; }
        public short Status { get; set; }
    }

    public class PMSClientInfoDetailsDTO
    {
        public long ID { get; set; }
        public long PMSClientInfoID { get; set; }
        public long PMSInfoDetailID { get; set; }
        public short Status { get; set; }
    }

    public class PMSFacilityMapDTO
    {
        public long ID { get; set; }
        public string FacilitityID { get; set; }
        public long PMSClientInfoID { get; set; }
        public byte[] ClientID { get; set; }
        public byte[] CorpID { get; set; }
        public byte[] StoreID { get; set; }
        public long URLID { get; set; }
        public short Status { get; set; }
    }
    public class PMSFacilityMapDetailsDTO
    {
        public long ID { get; set; }
        public long PMSFacilityMapID { get; set; }
        public string FileName { get; set; }

        public short Status { get; set; }
    }


    public class PMSCorpMapDTO
    {
        public long ID { get; set; }
        public byte[] CorporationId { get; set; }
        public string FacilityId { get; set; }
        public short Status { get; set; }
        public short Type { get; set; }
        public byte[] MapID { get; set; }
        public byte[] StoreID { get; set; }
        public string AmountReadFrom { get; set; }
        public string AliasFacilityId { get; set; }
    }
}
