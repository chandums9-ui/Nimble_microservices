using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreAccounting.API.Controllers
{
    public abstract class BaseController : ControllerBase
    {

        #region public Methods
        protected string GetUserID
        {
            get
            {
                return (string)HttpContext.Items["UserId"];
            }
        }
        protected string GetClientID 
        {
            get
            {
                return (string)HttpContext.Items["ClientId"];
            }
        }
        protected string GetClientName
        {
            get
            {
                return (string)HttpContext.Items["ClientName"];
            }
        }
        #endregion
    }
}
