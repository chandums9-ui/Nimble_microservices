using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using Dashboard.Domain.DTO.Model;
using Dashboard.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    /// <summary>
    /// Widget details
    /// </summary>
    public class WidgetResponse : StatusDTO, IDisposable
    {
        public int TotalCount { get; set; }
        public List<WidgetsDTO> widgetList { get; set; }

        public WidgetPrivilegeDTO widgetPrivileges { get; set; }


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
        // ~WidgetResponse()
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
    public class WidgetFormulaResponse:StatusDTO
    {
        public long FormulaId { get; set; }
    }
    public class WidgetLoadResponse : StatusDTO, IDisposable
    {
        public WidgetFormulaDTO widgetformula { get; set; } = new WidgetFormulaDTO();
        public WidgetSettingsFormulaDTO WidgetSettingsFormula { get; set; } = new WidgetSettingsFormulaDTO();
        public List<WidgetFormulaDetailsDTO> WidgetFormulaDetails { get; set; }=new List<WidgetFormulaDetailsDTO>();

        #region Disposeresponse
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
        // ~WidgetResponse()
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

    public class WidgetAllFormulaResponse:StatusDTO
    {
        public List<WidgetLoadResponse> allFomrulaResponse {  get; set; }=new List<WidgetLoadResponse>();
    }
    public class IDSubTyepResponse
    {
        public string AccountOrGroupKey { get; set; }
        public int AccountOrGroupSubType { get; set; }
        public int DeptId {  get; set; }    
    }
    public class CustomWidgetFormulas
    {
        public List<WidgetLoadResponse> listOfFormulDetails { get; set; } = new List<WidgetLoadResponse>();
    }
    public class Analysisresponse:StatusDTO
    {
        public List<AnalysisProperties> AnalysisProperties { get; set; }=new List<AnalysisProperties>();
           
    }
    public class AnalysisProperties
    {
        public string CorpId { get; set; }
        public long FormulaId { get; set; }
        public string DepartmentName { get; set; }
        public decimal Amount { get; set; }
        public short Order {  get; set; }   
        public decimal StatsorHours { get; set; }
        public decimal PercnetOfincome { get; set; }
        public decimal POR { get; set; }
        public long Type { get; set; }
        public long SubType { get; set; }
        public string TrendsName { get; set; }
        public decimal PAR { get; set; }
    }
}
