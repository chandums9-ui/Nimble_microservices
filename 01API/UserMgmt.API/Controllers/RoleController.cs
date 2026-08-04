using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMgmt.App.Contracts;
using UserMgmt.Domain.DataModel;
using UserMgmt.Domain.DTO.Req;
using UserMgmt.Domain.DTO.Resp;
using UserMgmt.Domain.DTO.Resp;

namespace UserMgmt.API.Controllers
{
    [Route("v1/Role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        #region Fields
        private readonly IRolePrivilege rolePrivilege;
        #endregion

        #region Ctor
        public RoleController(IRolePrivilege rolePrivilege)
        {
            this.rolePrivilege = rolePrivilege;
        }
        #endregion

        #region Role Privileges

        /// <summary>
        /// Based on data Create the RoleName and ParentID
        /// </summary>
        /// <param name="Data">Here data request RoleName and ParentID is Empty </param>
        /// <returns>It will Return the Status and Status Codes </returns>
        [Route("Create")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult>CreateRole(RoleSaveUpdateRequest Data)
        {
            string ClientID = (string)HttpContext.Items["ClientId"];
            RoleReposnse? result = null;
            try
            {
                 result = await rolePrivilege.CreateRole(Data, ClientID);
                if (result != null) { return Ok(result); }
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
        /// Based on dataRequest Update the RoleName And Status
        /// </summary>
        /// <param name="Data"> Here data request Role and Status</param>
        /// <returns>It will return the Status and Status Codes</returns>
        [Route("Update")]
        [HttpPut]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult>UpdateRole(RoleSaveUpdateRequest Data)
        {
            RoleReposnse result = null;
            string ClientID = (string)HttpContext.Items["ClientId"];
            try
            {
                result = await rolePrivilege.UpdateRole(Data, ClientID);
                if (result != null) { return Ok(result); }
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
        /// Based on data request  Delete the Role And Check the RoleId As it being used or not
        /// </summary>
        /// <param name="Data">Here data request RoleDeleteRequest</param>
        /// <returns>It will return the Status and Status Codes</returns>
        [Route("Delete")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> DeleteRole(RoleDeleteRequest Data)
        {
            RoleReposnse result = null;
            try
            {
                result = await rolePrivilege.DeleteRole(Data);
                if (result != null) { return Ok(result); }
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
        /// Based on data Request Save RoleSingleMenus
        /// </summary>
        /// <param name="Data"> Here data request RolePrivilegesDTO is Create,View,Update,Delete </param>
        /// <returns>It will return the Status and Status Codes</returns>
        [Route("SingleMenu")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> CreateSingleRoleMenus(RolePrivilegesRequest Data)
        {
            string ClientID = (string)HttpContext.Items["ClientId"];
            RoleReposnse? result=null;
            try
            {
                var status = await rolePrivilege.CreateSingleRoleMenus(Data, ClientID);
                if (status != null) { return Ok(status); }
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
        /// Based on data request Save MultipleRoleList
        /// </summary>
        /// <param name="Data">Here data request RoleID and RolePrivilegesList</param>
        /// <returns>It will return the Status and Status Codes</returns>
        [Route("MultipleMenu")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> CreateRoleMultipleMenus(RolePrivilegesMultipleRequest Data)
        {
            string ClientID = (string)HttpContext.Items["ClientId"];
            RoleReposnse? result= null;
            try
            {
                result = await rolePrivilege.CreateRoleMultipleMenus(Data, ClientID);
                if (result != null) { return Ok(result); }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result= null; }
        }
        /// <summary>
        /// Based on data request Assign RoleFullAccessPrivileges
        /// </summary>
        /// <param name="Data">Here data request RoleFullAccess </param>
        /// <returns>It will return the Status and Status Codes</returns>
        [Route("FullMenu")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> SaveRoleFullPrivileges(RoleFullAccessRequest Data)
        {
            string ClientID = (string)HttpContext.Items["ClientId"];
            RoleReposnse? result = null;
            try
            {
                result = await rolePrivilege.CreateRoleFullPrivileges(Data, ClientID);
                if (result != null) { return Ok(result); }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally { result= null; }
        }
        /// <summary>
        /// Based on roleID request GetRolePrivilegesList 
        /// </summary>
        /// <param name="roleID"> Here data request Get Role</param>
        /// <returns> It will Generate RolePrivilegesList</returns>
        [Route("list")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> RoleList(long roleID)
        {
            RolePrivilageList? result = null;
            try
            {
                result = await rolePrivilege.RoleMenuPrivilegesList(roleID);
                if (result != null) return Ok(result);
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
        /// It Returns SubRoles list of User based on UserId
        /// </summary>
        /// <param name="Data">data represents SubRoleRequest</param>
        /// <returns>It returns List of SubRoles </returns>
        [Route("SubRole")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> SubRole(SubRoleRequest Data)
        {
            SubRoleResponse? result = null;
            try
            {
                result = await rolePrivilege.SubRoles(Data);
                if (result != null) { return Ok(result); }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
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
