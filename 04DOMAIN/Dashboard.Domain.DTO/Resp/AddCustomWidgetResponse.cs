using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class AddCustomWidgetResponse : StatusDTO, IDisposable
    {
        public Int64 WidgetID { get; set; }
        public string WidgetName { get; set; }
        public Int16 WidgetType { get; set; }
        public string CompValue { get; set; }
        public WidgetAccesResponse WidgetAccesDetails { get; set; }

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
        // ~WidgetUserSettingsResponse()
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

    public class WidgetAccesResponse
    {
        public bool IsEnable { get; set; }
        public bool Delete {  get; set; }
        public bool Navigate {  get; set; }
        public bool FormulaBuilder {  get; set; }
    }
}
