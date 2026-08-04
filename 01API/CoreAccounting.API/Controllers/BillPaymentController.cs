using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.App.Contracts;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Req;
using Common.Domain.Mapper;
using CoreAccounting.App.Contracts;
using CoreAccounting.Domain.DTO.Req;
using CoreAccounting.Domain.DTO.Resp;
using Microsoft.AspNetCore.Mvc;

// TODO: Should Move this file to Payable module.
namespace CoreAccounting.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class BillPaymentController : BaseController
    {


        #region Fields

        private readonly IBillPaymentService billpaysrv;

        #endregion

        #region Ctor
        public BillPaymentController(IBillPaymentService billPaymentService)
        {
            this.billpaysrv = billPaymentService;
        }

        #endregion

        #region Daily Sale

        [Route("BillPayment/Create")]
        [HttpPost]

        /// <summary>
        /// It creates BillPaymentry in related tables
        /// </summary>
        /// <param name="PaymentReq"></param>
        /// <returns>It returns JournalResponse</returns>
        public async Task<IActionResult> CreateBillPay(BillPaymentRequest PaymentReq)
        {
            SinglePostData? response = null;
            try
            {
                response = await billpaysrv.CreateBillPayment(PaymentReq);
                if (response.StatusCode == StatusCodes.Status200OK && response != null)
                    return Ok(response);
                else
                    return NotFound(response);

            }
            catch { throw; }
            finally { response = null; }
        }
        #endregion

    }
}
