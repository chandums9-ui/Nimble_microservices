using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class WidgetRecordsDTO : StatusDTO,IDisposable
    {
        public List<WidgetRecordsResponse> WidgetRecords { get; set; } = new List<WidgetRecordsResponse>();
        #region Dispose
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~WidgetPrivilegeResponse()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
    public class WidgetRecordsResponse
    {
        public long? WidgetID { get; set; }
        public long FormulaID { get; set; }
        public string DisplayFormula { get; set; }
        public string ActualFormula { get; set; }
        public string FormulaName { get; set; }
        public Int16 SortOrder { get; set; }
        public decimal StatsorHours { get; set; } 
        public decimal PercnetOfincome { get; set; }
        public decimal POR { get; set; }
        public decimal PAR { get; set; }
        //public decimal Amount { get;set; }
        //public decimal BudAmount { get;set; }
        public decimal BudPercentofIncome { get;set; }
        public decimal BudPOR { get; set;}
        public decimal BudPAR { get;set; }
        public decimal BudStatsorHours {  get; set; }   
        //public decimal LyAmount {  get; set; }  
        public decimal LyPercnetOfincome { get; set; }  
        public decimal LyPOR { get; set; }  
        public decimal LyPAR { get; set; }  
        public decimal LyStatsorHours { get; set; } 

        public AmountResponse AmtResponse { get; set; }=new AmountResponse();
    }


    public class AmountResponse
    {
        public decimal AcutalAmount { get; set; }
        public decimal BudAmount { get; set; }  
        public decimal LYAmount { get; set; }
    }

}
