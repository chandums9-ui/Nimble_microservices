using Common.App.Contracts;
using Common.Domain.DTO.Model.Base;

using Common.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Common.API.ActionFilters;
using UserMgmt.Domain.DataModel;
using UserMgmt.Domain.DTO.Req;
using UserMgmt.Domain.DTO.Resp;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Req;
using NLog.Layouts;
using UserMgmt.App.Contracts;
using Common.Domain.DTO.App;

namespace UserMgmt.API.Controllers
{
    [Route("v1/OpenApiauth")]
    [ApiController]
    public class UserMgmtAuthController : ControllerBase
    {
        private IAuthJwt authJwtUtil;
        private readonly IAuthJwtValidation authJwtValidation;
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserMgmtAuthController(IAuthJwt authJwt, IAuthJwtValidation _iAuthJwtValidation, IHttpContextAccessor _httpContextAccessor)
        {
            this.authJwtUtil = authJwt;
            this.authJwtValidation = _iAuthJwtValidation;
            this.httpContextAccessor = _httpContextAccessor;
        }

        [Route("token")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<IActionResult> GenerateToken(ApiAuthDTO data)
        {

            TokenRespDTO tokenRespDTO = await authJwtUtil.GenerateToken(data);
            if (tokenRespDTO == null || tokenRespDTO.StatusCode == (int)StatusCodes.Status200OK)
                return Ok(tokenRespDTO);
            else
                return Unauthorized(tokenRespDTO);

        }
        [Route("apitoken")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<IActionResult> GenerateAdminToken(ApiAuthDTOUrl data)
        {

            TokenRespDTO tokenRespDTO = await authJwtUtil.GenerateAdminToken(data);
            if (tokenRespDTO == null || tokenRespDTO.StatusCode == (int)StatusCodes.Status200OK)
                return Ok(tokenRespDTO);
            else
                return Unauthorized(tokenRespDTO);

        }
        [Route("OAuthToken")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<IActionResult> GenerateOAuthAdminToken(ApiAuthDTO data)
        {

            ApiAuthDTOUrl dataAPI = new ApiAuthDTOUrl();
            dataAPI.ClientSecret = data.ClientSecret;
            dataAPI.ClientID = data.ClientID;
            dataAPI.UrlName = "common";
            TokenRespDTO tokenRespDTO = await authJwtUtil.GenerateAdminToken(dataAPI);
            TokenMinRespDTO tokenMinResp = new TokenMinRespDTO();
            if (tokenRespDTO != null)
            {
                tokenMinResp.Token = tokenRespDTO.Token;
                tokenMinResp.StatusCode = tokenRespDTO.StatusCode;
                tokenMinResp.Status = tokenRespDTO.Status;
            }
            if (tokenMinResp == null || tokenMinResp.StatusCode == (int)StatusCodes.Status200OK)
                return Ok(tokenMinResp);
            else
                return Unauthorized(tokenMinResp);

        }
        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<IActionResult> GenerateLoginToken(ApiLoginAuthDTO data)
        {

            TokenRespDTO tokenRespDTO = await authJwtUtil.GenerateLoginToken(data);
            if (tokenRespDTO == null || tokenRespDTO.StatusCode == (int)StatusCodes.Status200OK)
                return Ok(tokenRespDTO);
            else
                return Unauthorized(tokenRespDTO);

        }
        [Route("UrlsList")]
        [HttpGet]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<IActionResult> GetURLInfoList()
        {
            URLInfoResponse result = null;
            try
            {

                result = await authJwtUtil.GetURLInfoList();
                if (result == null || result.StatusCode == (int)StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally
            { 
                //result = null;
            }

        }
        [Route("Register")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<IActionResult> RegisterClient(ClientRegisterRequest data)
        {

            ClientRegisterResponse tokenRespDTO = await authJwtUtil.Register(data);
            if (tokenRespDTO == null || tokenRespDTO.StatusCode == (int)StatusCodes.Status200OK)
                return Ok(tokenRespDTO);
            else
                return Unauthorized(tokenRespDTO);

        }
        [Route("ValidateToken")]
        [HttpPost]
        [ValidateModel]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateToken(ValidateTokenRequest token)
        {

            ValidateTokenRespDTO validateTokenRespDTO = await authJwtValidation.ValidateToken(token.Token);
            if (validateTokenRespDTO == null || validateTokenRespDTO.StatusCode == (int)StatusCodes.Status200OK)
                return Ok(validateTokenRespDTO);
            else
                return Unauthorized(validateTokenRespDTO);

        }

       
        [Route("Logout")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<IActionResult> Logout(ValidateTokenRequest data)
        {
            //var tokenInfo = (ValidateTokenRespDTO)httpContextAccessor.HttpContext.Items["UserInfoToken"];

            TokenRespDTO tokenRespDTO = await authJwtUtil.Logout(data);
            if (tokenRespDTO == null || tokenRespDTO.StatusCode == (int)StatusCodes.Status200OK)
                return Ok(tokenRespDTO);
            else
                return Unauthorized(tokenRespDTO);

        }
        [Route("ExchangeOAuth")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<IActionResult> ExchangeToken(ValidateTokenRequest data)
        {

            TokenRespDTO tokenRespDTO = await authJwtUtil.ExchangeToken(data.Token);
            if (tokenRespDTO == null || tokenRespDTO.StatusCode == (int)StatusCodes.Status200OK)
                return Ok(tokenRespDTO);
            else
                return Unauthorized(tokenRespDTO);

        }

        [Route("GetClientNamesList")]
        [HttpGet]
        public async Task<IActionResult> GetClientNamesList([FromQuery] long UrlID)
        {
            ClientInfoResponse result = null;
            try
            {
                result = await authJwtUtil.GetClientNamesList(UrlID);
                if (result == null || result.StatusCode == (int)StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, (!string.IsNullOrEmpty(result.Status)) ? result.Status : Constants.MSG_ENDPOINT_ERROR);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR);
            }
            finally
            {
                result = null;
            }

        }



    }
}
