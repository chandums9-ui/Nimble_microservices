//using Azure;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMgmt.App.Contracts;
using UserMgmt.Domain.DataModel;
using UserMgmt.Domain.DTO.Model;
using UserMgmt.Domain.DTO.Req;
using UserMgmt.Domain.DTO.Resp;

namespace UserMgmt.API.Controllers
{

    [Route("v1/userprivileges")]
    [ApiController]
    public class UserPrivilegesController : ControllerBase
    {
        #region Fields

        private readonly IUserPrivileges userPrivileges;
        #endregion

        #region Ctor
        public UserPrivilegesController(IUserPrivileges userPrivileges)
        {
            this.userPrivileges = userPrivileges;
        }
        #endregion

        #region UserPrivileges

        /// <summary>
        /// It will create UserPrivileges based on UserPrivilegesRequest
        /// </summary>
        /// <param name="Data">Data represents UserPrivilegesRequest</param>
        /// <returns> It will return Status and StatusCode</returns>
        [Route("Create")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> CreateSinglePrivileges(UserPrivilegesRequest Data)
        {
            UserPrivilegesResponse result=null;
            try
            {
                result = await userPrivileges.CreateSingleMenus(Data);
                if (result != null)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result=null; }
        }
        /// <summary>
        /// It will update UserPrivileges based on PrivilegesMutliple List and UserID
        /// </summary>
        /// <param name="Data">Data represents MultiplePrivilegesMenus </param>
        /// <param name="UserID">It represents UserID request </param>
        /// <returns> It will return Status</returns>
        [Route("Multiple")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> CreateMultipleMenus(List<UserPrivilegesBaseDTO> Data, string UserID)
        {
            UserPrivilegesResponse result = null;
            try
            {
                result = await userPrivileges.SaveMultipleMenus(Data, UserID);
                if (result != null) return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; };
        }
        /// <summary>
        /// It will Give the Full Access to the Sub Users
        /// </summary>
        /// <param name="Data">Data represents UserFullAccessRequest </param>
        /// <returns>It will return Status and StatusCode</returns>
        [Route("FullAccess")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> SaveFullPrivilges(UserFullAccessRequest Data)
        {
            UserPrivilegesResponse result = null;
            try
            {
                result = await userPrivileges.SaveFullMenus(Data);
                if (result != null)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; };
        }

        /// <summary>
        /// It will load the UserPrivileges based on Data ModelBaseUserIDRequest
        /// </summary>
        /// <param name="Data">Data respresents ModelBaseUserIDRequest</param>
        /// <returns>It will return the</returns>
        [Route("list")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> GetPrivilegesList(LoadByIDRequest Data)
        {
            UserPrivilegesListResponse result = null;
            string userId = (string)HttpContext.Items["UserId"];
            try
            {
                Data.ID = userId;    
                result = await userPrivileges.GetPrevilegesbyUserID(Data);
                if (result != null) return Ok(result);
                else
                return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; };
        }
        [Route("Menu")]
        [HttpPost]     
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult>GetMenus(MenuPrivilegesRequest Data)
        {
            PrivilegesResponse result = null;
            try
            {
                var uid = (string)HttpContext.Items["UserId"];
                Data.UserID = uid;
                result = await userPrivileges.GetPrevilegesbyuserIDMenu(Data);
                if (result != null) return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; };

        }

    }
        #endregion    
}
