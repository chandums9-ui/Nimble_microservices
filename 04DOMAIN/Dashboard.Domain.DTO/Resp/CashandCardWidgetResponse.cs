using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class CashandCardWidgetResponse : StatusDTO
    {
        public List<CashCardGridData> CashCardGridList { get; set; } = new List<CashCardGridData>();

    }

    public class CashCardGridData {
        public string ID { get; set; } = string.Empty;
        public string Date { get; set; }

        public List<GridDto> ColumnsListData { get; set; } = new List<GridDto>();

        public List<DepositDto>? DepoistDetail { get; set; } = new List<DepositDto>();
        public string StatusDate { get; set; }

        public decimal PreviousRunningBalance { get; set; }

        public string? Comments { get; set; }
    }

    public class DepositDto {
        public string? DepositDate { get; set; }
        public bool? HasAttachment { get; set; } = false;

    }


    public class GridDto {
    
        public string ColumnName { get; set; }

        public decimal Amount { get; set; }

        public int PaymentStatus { get; set; }

        public bool PaidOutsCol { get; set; } = false;
    }


    public class CashCardWidgetCardResponse : StatusDTO
    {
        public List<WidgetCardResponse> WidgetCardListData { get; set;} = new List<WidgetCardResponse>();
    }

    public class WidgetCardResponse {
        public string CardName { get; set; }
        public decimal CardAmount { get; set; }
    }


}
