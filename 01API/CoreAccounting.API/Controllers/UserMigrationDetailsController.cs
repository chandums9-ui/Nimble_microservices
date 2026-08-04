using Common.API.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.API.Authorization;
using CoreAccounting.App.Contracts;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using CoreAccounting.App.Service;
namespace CoreAccounting.API.Controllers
{
    [Route("v1/UserMigr")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class UserMigrationDetailsController : BaseController
    {
        #region Fields
        private readonly IUserMigration userMigration;

        #endregion
        #region Constructor
        public UserMigrationDetailsController(IUserMigration _userMigration)
        {
            this.userMigration = _userMigration;
        }
        #endregion
        #region Endpoints
        [Route("MigrUsers")]
        [HttpGet]
        public async Task<IActionResult> GetUsersMigrationInformation()
        {
         
            string urlName = HttpContext.Items["ClientName"].ToString();
            UserMigrationsResp response = await userMigration.GetUserMigrationInformation(urlName);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);

        }
       
        [Route("MigrReverseUsers")]
        [HttpGet]
        public async Task<IActionResult> GetUsersRiverseMigrationInformation()
        {

            string urlName = HttpContext.Items["ClientName"].ToString();
            UserMigrationsResp response = await userMigration.GetUserReverseMigrationInformation(urlName);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);

        }
        [Route("MigrClients")]
        [HttpGet]
        public async Task<IActionResult> GetClientsMigrationInformation()
        {
           
            string urlName =HttpContext.Items["ClientName"].ToString();
            ClientMigrationsResp response = await userMigration.GetClientsMigrationInformation(urlName);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        [Route("MigrReverseClients")]
        [HttpGet]
        public async Task<IActionResult> GetClientsReverseMigrationInformation()
        {

            string urlName = HttpContext.Items["ClientName"].ToString();
            ClientMigrationsResp response = await userMigration.GetClientsReverseMigrationInformation(urlName);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }
        #endregion

        #region Migration of Users
        [Route("SharingMigartionUsersInfo")]
        [HttpPost]
        public async Task<IActionResult> SharingMigartionUsersInfo()
        {

            string urlName = HttpContext.Items["ClientName"].ToString();
            long urlID = Convert.ToInt64(HttpContext.Items["UrlID"].ToString());
            MigrationRes response = await userMigration.SharingMigartionUsersInfo(urlID);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response != null && response.StatusCode == StatusCodes.Status500InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            else
                return NotFound(response);
        }


        #endregion
    }

}
