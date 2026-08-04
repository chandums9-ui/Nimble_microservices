using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMgmt.App.Contracts;
using UserMgmt.Domain.DTO.Req;
using UserMgmt.Domain.DTO.Resp;

namespace UserMgmt.API.Controllers
{
    [Route("v1/Corplink")]
    [ApiController]
    public class UserCorporationLinkController : ControllerBase
    {
        #region Field
        private readonly IUserCorpLink userCorpLink;
        #endregion

        #region Ctor
        public UserCorporationLinkController(IUserCorpLink userCorpLink)
        {
            this.userCorpLink = userCorpLink;
        }
        #endregion

        #region UserCorporation 
        /// <summary>
        /// Based on the dataRequest and UserIDRequest Save the Corporation UserLink
        /// </summary>
        /// <param name="Data">Here data send the CorporationLists </param>
        /// <param name="UserID"> Here UserID send get UserClientLink Details</param>
        /// <returns> It will return the Status and StatusCode </returns>
        [Route("Save")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult>SaveCorpLink(List<UserCorpLinkRequest> Data, string UserID)
        {
            UserCorpResponse? result=null;
            try
            {
                result = await userCorpLink.CreateCorporation(Data, UserID);
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
        /// Based on the data Request and UserID Request update the Corporation UserLink
        /// </summary>
        /// <param name="Data">Here data send the UserCorpLinkRequest List</param>
        /// <param name="UserID">Here UserID send get UserClientLink Details</param>
        /// <returns>It will return the Status and StatusCode </returns>
        [Route("Update")]
        [HttpPut]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult>UpdateCorpList(List<UserCorpLinkRequest> Data, string UserID)
        {
            UserCorpResponse? result= null;
            try
            {
                result = await userCorpLink.UpdateCorporation(Data, UserID);
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
        /// Based on ModelBaseCorporationID Request Delete the Corporation UserLink
        /// </summary>
        /// <param name="Data"> Here data send the CorporationID</param>
        /// <returns>It will return the Status and StatusCode</returns>
        [Route("Delete")]
        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> DeleteCorporation(UserCorpLinkRequest Data)
        {
            UserCorpResponse? result = null;
            try 
            {
                result = await userCorpLink.DeleteCorporation(Data);
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
