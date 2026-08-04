using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMgmt.App.Contracts;
using UserMgmt.Domain.DataModel;
using UserMgmt.Domain.DTO.Req;
using UserMgmt.Domain.DTO.Resp;

namespace UserMgmt.API.Controllers
{

    [Route("v1/userPcLink")]
    [ApiController]
    public class UserPcLinkController : ControllerBase
    {
        #region Field
        private readonly IUserPcLink userPcLink;
        #endregion

        #region Ctor
        public UserPcLinkController(IUserPcLink userPcLink)
        {
            this.userPcLink = userPcLink;
        }
        #endregion

        #region UserPCLink

        /// <summary>
        /// Based on UserCorpPCLinkRequest Create the CorporationPCLinks
        /// </summary>
        /// <param name="Data">Here data UserCorporationList and PcLinkList</param>
        /// <returns>It will return the Status and StatusCode </returns>
        [Route("createPcLink")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> UserPcLink(UserCoporationsPCLinkRequest Data)
        {
            UserCorpResponse? result = null;
            try
            {
                result = await userPcLink.CreateUserCorpPcLink(Data);
                if (result != null)
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
        /// Based on UserPCUpdateRequest update the CorporationPcLinksList and Remove
        /// </summary>
        /// <param name="Data">Here data UserCorporation</param>
        /// <returns>It will return the Status and StatusCode </returns>
        [Route("UpdatePcLink")]
        [HttpPut]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> UpdateUserPCLink(UserPCUpdateRequest Data)
        {
            UserCorpResponse? result = null;
            try
            {
                result = await userPcLink.UpdateUserPCLink(Data);
                if (result != null)
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
        /// Based on UserPCDeleteRequest Delete the UserPCllinks 
        /// </summary>
        /// <param name="Data">Here data UserCorpID and ProfitCenterID</param>
        /// <returns>It will return the Status and StatusCode</returns>
        [Route("DeletePcLink")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> DeleteUserPcLink(UserPCDeleteRequest Data)
        {
            UserCorpResponse? result = null;
            try
            {
                result = await userPcLink.DeleteCorpPcLink(Data);
                if (result != null)
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
        #endregion
    }
}
