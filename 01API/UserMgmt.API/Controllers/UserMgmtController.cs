using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using UserMgmt.App.Contracts;
using UserMgmt.Domain.DataModel;
using UserMgmt.Domain.DTO.Req;
using UserMgmt.Domain.DTO.Resp;

namespace UserMgmt.API.Controllers
{
    [Route("v1/user")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class UserMgmtController : ControllerBase
    {
        #region Fields
        private IUserMgmt userMgmt;
        private readonly IHttpContextAccessor httpContextAccessor;
        #endregion

        #region Ctor
        public UserMgmtController(IUserMgmt _userMgmt, IHttpContextAccessor _httpContextAccessor)
        {
            this.userMgmt = _userMgmt;
            this.httpContextAccessor = _httpContextAccessor;
        }
        #endregion
        private long getUrlID()
        {
            string urlID = (string)HttpContext.Items["UrlID"];
            long id = Convert.ToInt64(urlID);
            return id;
        }
        #region UserMgmt
        /// <summary>
        /// Based On Data request  create user and Client Create
        /// </summary>
        /// <param name="data"> here Data Send User Details </param>
        /// <returns>It will return Status and StatusCode</returns>
        /// 
        [Route("ExchangeOAuth")]
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> ExchangeAuthToken(ExchangeAuth data)
        {

            TokenRespDTO tokenRespDTO = await userMgmt.ExchangeAuthToken(data.AuthenticationID, getUrlID());
            if (tokenRespDTO == null || tokenRespDTO.StatusCode == (int)StatusCodes.Status200OK)
                return Ok(tokenRespDTO);
            else
                return Unauthorized(tokenRespDTO);

        }


        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserInfoRequest user)
        {
            UserInfoResponse result = null;
            try
            {
                var tokenInfo = (ValidateTokenRespDTO)httpContextAccessor.HttpContext.Items["UserInfoToken"];

                result = await userMgmt.CreaterUser(user, tokenInfo);
                if (result == null || result.StatusCode == (int)StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }
        }
        [Route("UserInfo")]
        [HttpPost]
        public async Task<IActionResult> GetUserInfo(UserInfoReq user)
        {
            UserInfoResp result = null;
            try
            {
               
                result = await userMgmt.GetUserInfo(user);
                if (result == null || result.StatusCode == (int)StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }
        }


        [Route("GetPwd")]
        [HttpPost]
        public async Task<IActionResult> GetUserPassword(UserPwdReq userPwdReq)
        {
            UserPwdResp result = null;
           result = await userMgmt.GetPwd(userPwdReq);
           if (result != null)
                return Ok(result);
             else 
               return StatusCode(StatusCodes.Status405MethodNotAllowed,  Constants.MSG_UserNotAvailble);
            
        }

        [Route("UserMigration")]
        [HttpPost]
        public async Task<IActionResult> UsersMigration(List<UserInfoRequest> user)
        {
            UserInfoResponse result = null;
            try
            {
               
                result = await userMgmt.UsersMigratioin(user);
                if (result == null || result.StatusCode == (int)StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }
        }
        [Route("ClientMigration")]
        [HttpPost]
        public async Task<IActionResult> ClientsMigration(List<ClientRegisterRequest> user)
        {
            ClientRegisterResponse result = null;
            try
            {

                result = await userMgmt.ClientsMigration(user);
                if (result == null || result.StatusCode == (int)StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }
        }
        /// <summary>
        ///  Based on data request it will update the user password
        /// </summary>
        /// <param name="data">here data send Old password and New Password</param>
        /// <returns>It will Display Response of password update or not messages</returns>
        [Route("paswordupdate")]
        [HttpPost]
        public async Task<IActionResult> Updatepasseord(UpadatePasswordRequest data)
        {
            UserInfoResponse result = null;
            try
            {
                var Name = (string)httpContextAccessor.HttpContext.Items["ClientName"];
                result = await userMgmt.UpdatePassword(data);
                if (result != null || result.StatusCode == (int)StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }
        }
        /// <summary>
        /// Based on data request it will delete user
        /// </summary>
        /// <param name="data"> Here data send userID  and check the userid is link to clientlink and client and then it will delete</param>
        /// <returns>It will Display Status and Status Code  </returns>
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(ModelBaseUserID data)
        {
            UserInfoResponse result = null;
            try
            {
                long clientInfoID =Convert.ToInt64((string)httpContextAccessor.HttpContext.Items["ClientInfoID"]);
                result = await userMgmt.DeleteUser(data, clientInfoID);
                if (result == null || result.StatusCode == (int)StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }
        }
        [Route("DeleteClient")]
        [HttpPost]
        public async Task<IActionResult> DeleteClient(ClientDeletRequest data)
        {
            UserInfoResponse result = null;
            try
            {
                
                result = await userMgmt.DeleteClient(data);
                if (result == null || result.StatusCode == (int)StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }
        }
        /// <summary>
        /// Based on UserName it will check user available if avaialble it will update password
        /// </summary>
        /// <param name="UserName">Here UserName send has parameter to user available or not</param>
        /// <param name="newpassword">Here New password to change old password</param>
        /// <returns>It will return Status and Status Code message</returns>
        [Route("forgotpassword")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string UserName, string newpassword)
        {
            UserInfoResponse result = null;
            try
            {
                result = await userMgmt.ForgotPassworduserupdate(UserName, newpassword);
                if (result == null || result.StatusCode == (int)StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }
        }


        [Route("ReportTo")]
        [HttpPost]
        public async Task<IActionResult> GetReportUserDetails(UserRequest data)
        {
            ReportUsersResponse result = null;
            try
            {
                result = await userMgmt.GetReportToUsers(data);
                if (result == null || result.StatusCode == (int)StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }

        }
        [Route("RolesLoad")]
        [HttpPost]
        public async Task<IActionResult> GetRoles(string clientID, long roleID)
        {
            RolesResponse result = null;
            try
            {
                result = await userMgmt.GetRolesDetails(clientID, roleID);
                if (result == null || result.StatusCode == (int)StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }

        }


        [Route("GetListUserInfo")]
        [HttpPost]
        public async Task<IActionResult> GetListUserInfo(GetUserNameRequest request)
        {
            GetUserNameResponse result = null;
            try
            {
                
                result = await userMgmt.GetListUserInfo(request);
                if (result == null || result.StatusCode == (int)StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }

        }

        
        [Route("GetClientInfoList")]
        [HttpGet]
        public async Task<IActionResult> GetClientInfoList([FromQuery]long UrLID)
        {
            ClientInfoResponse result = null;
            try
            {
                result = await userMgmt.GetClientInfoList(UrLID);
                if (result == null || result.StatusCode == (int)StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }

        }

        [Route("GetClientIdsList")]
        [HttpPost]
        public async Task<IActionResult> GetClientIdsList()
        {
            List<ClientIdList> result = null;
            try
            {
                long urlKey = Convert.ToInt64((string)HttpContext.Items["UrlID"]);
                result = await userMgmt.GetClientIdsList(urlKey);
                   
                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }

        }
        [Route("GetUserDetails")]
        [HttpPost]
        public async Task<IActionResult> GetUserDetails()
        {
            UserDetails result = null;
            try
            {
                string LoginUserID = (string)HttpContext.Items["UserId"];

                result = await userMgmt.GetUserDetails(LoginUserID);

                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result = null; }

        }

        #endregion
    }
}
