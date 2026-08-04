using Common.App.Contracts;
using Microsoft.AspNetCore.Http;
using Common.Domain.DTO;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model;

namespace Common.API.Authorization
{
    public class AuthJwtMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthJwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context,  IAuthJwtValidation jwtUtils,IURLConnection uRLConnection)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                ValidateTokenRespDTO validToken = await jwtUtils.ValidateToken(token);

                if (validToken != null && validToken.StatusCode == (int)StatusCodes.Status200OK)
                {
                    // attach user to context on successful jwt validation
                    //string[] val = userId.Split('|');
                    context.Items["UserId"] = validToken.UserID;
                    context.Items["ClientId"] = validToken.ClientID;
                    context.Items["ClientName"] = validToken.ClientName;
                    context.Items["UserInfoID"] = validToken.UserInfoID;
                    context.Items["ClientInfoID"] = validToken.ClientInfoID;
                    context.Items["UrlID"] = validToken.UrlID;
                    context.Items["UserInfoToken"] = validToken;
                    context.Items["ClientUrl"] = validToken.ClientUrl;
                    uRLConnection.UrlName = validToken.ClientName;

                }
            }
            await _next(context);


        }
       
    }
}
