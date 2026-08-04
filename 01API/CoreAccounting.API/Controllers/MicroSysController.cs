using Common.App.Contracts;
using Common.Domain;
using Common.Domain.Common;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using Common.API.ActionFilters;
using Common.API.Authorization;
using CoreAccounting.App.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Common.Domain.DTO.Model;
using CoreAccounting.Domain.DTO.Req;
using CoreAccounting.Domain.DTO.Resp;
using Common.Infra.Cache;
using Amazon.Runtime.Internal.Util;
using Common.Domain.DTO.App;
namespace CoreAccounting.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class MicroSysController : BaseController
    {
        #region Fields
        private readonly IMicroSysService coreProp;
        private readonly IFileService fileSrv;
        private readonly ICacheService cache;
        #endregion

        #region Ctor
        public MicroSysController(IMicroSysService coreProperty, IFileService fileService, ICacheService _cache)//, IOptions<JWTSettingsDTO>jwtSettingsDto )
        {
            this.coreProp = coreProperty;
            fileSrv = fileService;
            cache = _cache;
        }

        #endregion
        [Route("MicroSysCorporations")]
        [HttpGet]
        public async Task<IActionResult> GetCorporationsAndAddress(bool IsShowAll = false)
        {
            MicroSysCorporationsResponse corp = new MicroSysCorporationsResponse();
            try
            {
                using (IMicroSysService cp = coreProp)
                {
                    string clientID = (string)HttpContext.Items["ClientId"];

                    string userId = (string)HttpContext.Items["UserId"];
                    if (!IsShowAll && string.IsNullOrEmpty(userId)) { return NotFound(corp); }
                    //string cacheKey = string.Concat((!IsShowAll ? userId : clientID), "CorpList");
                    //corp = cache.GetData<MicroSysCorporationsResponse>(cacheKey);

                    //if (corp == null)
                    //{
                        corp = await cp.GetCorporationsAndAddress(IsShowAll ? string.Empty : userId, clientID);
                    //    cache.SetData<MicroSysCorporationsResponse>(cacheKey, corp);
                    //}

                    if (corp == null || corp.Corporations == null)
                        corp = new MicroSysCorporationsResponse();
                    return Ok(corp);
                }
            }
            catch { throw; }
        }

        [Route("MicroSysAccounts")]
        [HttpGet]
        public async Task<IActionResult> GetMicroSysAccounts([FromQuery] CorpIDAndAccTypeIDRequest data)
        {
            try
            {
                MicroSysAccountListResponse al;
                using (IMicroSysService cp = coreProp)
                {
                    //var userId = HttpContext.Items["UserId"];
                    //string cacheKey = string.Concat(data.CorpID, "AccList");
                    //al = cache.GetData<MicroSysAccountListResponse>(cacheKey);
                    //if (al == null)
                    //{
                        al = await cp.GetMicroSysAccounts(data);
                    //    cache.SetData<MicroSysAccountListResponse>(cacheKey, al);
                    //}
                    if (al == null || al.Accounts == null)
                        al = new MicroSysAccountListResponse();
                    return Ok(al);
                }
            }
            catch { throw; }
        }
        [Route("MicroSysVendorNames")]
        [HttpGet]
        public async Task<IActionResult> GetMicroSysVendorNames([FromQuery] CorpIDRequest data)
        {
            try
            {
                MicroSysVendorListResponse nl;
                using (IMicroSysService cp = coreProp)
                {
                    //string cacheKey = string.Concat(data.CorpID, "NameList");
                    //nl = cache.GetData<MicroSysVendorListResponse>(cacheKey);
                    //if (nl == null)
                    //{
                        nl = await cp.GetMicroSysVendorNames(data);
                    //    cache.SetData<MicroSysVendorListResponse>(cacheKey, nl);
                    //}

                    if (nl == null || nl.ListInfo == null)
                        nl = new MicroSysVendorListResponse();
                    return Ok(nl);
                }
            }
            catch { throw; }
        }
        [Route("PaymentStatus")]
        [HttpPost]
        public async Task<IActionResult> UpdatePaymentStatus(MicroSysRequest PaymentStatus)
        {
            JournalResponse? response = null;
            try
            {
                response = await coreProp.UpdatePaymentStatus(PaymentStatus);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { throw; }
            finally { response = null; }
        }
        [Route("MicroSysPaymentDet")]
        [HttpGet]
        public async Task<IActionResult> GetMicroSysPaymentDetails([FromQuery] PaymentStatusRequest data)
        {
            try
            {
                MicroSysPaymentDetailResponse payDet;
                using (IMicroSysService cp = coreProp)
                {
                    //string cacheKey = string.Concat(data.CorporationID, "NameList");
                    //payDet = cache.GetData<MicroSysPaymentDetailResponse>(cacheKey);
                    //if (payDet == null)
                    //{
                        payDet = await cp.GetMicroSysPaymentDetails(data);
                    //    cache.SetData<MicroSysPaymentDetailResponse>(cacheKey, payDet);
                    //}

                    if (payDet == null || payDet.ListInfo == null)
                        payDet = new MicroSysPaymentDetailResponse();
                    return Ok(payDet);
                }
            }
            catch { throw; }
        }
    }
}
