using Common.API.ActionFilters;
using Common.API.Authorization;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.Mvc;
using Payable.App.Contracts;
using Payable.Domain.DTO.Model;
using Payable.Domain.DTO.Req;
using Payable.Domain.DTO.Resp;

namespace Payable.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class BillEntryAndBillPaymentLinkController : ControllerBase
    {
        #region Fields
        private readonly ICommonService commonSrv;
        private readonly IBillAndBillPaymentLinkService billEnbillAndBillPaymentLinkSrv;
        private readonly IUnitOfWork uow;
        //private readonly IFileService fileSrv;

        #endregion

        #region Ctor
        public BillEntryAndBillPaymentLinkController(ICommonService coreProperty, IUnitOfWork _uow, IBillAndBillPaymentLinkService billAndBillPaymentLinkService)
        {
            this.commonSrv = coreProperty;
            //this.fileSrv = fileService;
            this.billEnbillAndBillPaymentLinkSrv = billAndBillPaymentLinkService;
            this.uow = _uow;
        }

        #endregion

        #region Private Methods
        private string getUserID()
        {
            return (string)HttpContext.Items["UserId"];
        }
        private string getClientID()
        {
            return (string)HttpContext.Items["ClientId"];
        }
        private string getClientName()
        {
            return (string)HttpContext.Items["ClientName"];
        }
        #endregion


        [Route("BillsToLink")]
        [HttpPost]
        [ProducesResponseType(typeof(LoadBillsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadBillsToLinkViewGridData(BillsAndPaymentsReq request)
        {
            request.IsPaginationRequired = true;
            BillsAndPaymentsList response = new BillsAndPaymentsList();
            request.UserID = getUserID();
            response = await billEnbillAndBillPaymentLinkSrv.GetBillsToLink(request);
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("LinkOrPostPayment")]
        [HttpPost]
        [ProducesResponseType(typeof(LoadBillsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LinkOrPostPayments(List<PostBillPayRequest> request)
        {
            BillPaymentResponse response = new BillPaymentResponse();
            response = await billEnbillAndBillPaymentLinkSrv.PostSingleOrBulkBillPayment(request, getUserID(), getClientID(), getClientName());
            if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("AutoMatchOrLink")]
        [HttpPost]
        [ProducesResponseType(typeof(LoadBillsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AutomatchOrLink(BillsAndPaymentsReq request)
        {
            StatusDTO response = new StatusDTO();
            request.UserID = getUserID();
            response = await billEnbillAndBillPaymentLinkSrv.SaveAutoMatchedDetails(request);
            if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);

        }
        [Route("AutoMatchOrLinkList")]
        [HttpPost]
        [ProducesResponseType(typeof(LoadBillsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAutomatchedList(BillsAndPaymentsReq request)
        {
            AutoMatchedList response = new AutoMatchedList();
            request.UserID = getUserID();
            response = await billEnbillAndBillPaymentLinkSrv.GetAutoMatchedList(request);
            if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);

        }

        [Route("DeleteImportedBillPayment")]
        [HttpPost]
        [ProducesResponseType(typeof(LoadBillsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteImportedBillPayment([FromBody] long ID)
        {
            JournalResponse response = new JournalResponse();
            response = await billEnbillAndBillPaymentLinkSrv.DeleteIomportedBillPayment(ID);
            if (response != null && response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else
                return NotFound(response);

        }
        [Route("MatchedBillDetailsByJeID")]
        [HttpPost]
        [ProducesResponseType(typeof(LoadBillsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMatchedBillDetails([FromBody] string JeId)
        {
            MatchedBillDetails response = new MatchedBillDetails();
            response = await billEnbillAndBillPaymentLinkSrv.GetMatchedBillDetails(JeId);
            if (response != null)
                return Ok(response);
            else
                return NotFound(response);

        }

        [Route("MoveToOpenSingleOrBulk")]
        [HttpPost]
        [ProducesResponseType(typeof(LoadBillsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MoveToOpenSingleOrBulk(BillsAndPaymentsReq request)
        {
            StatusDTO response = new StatusDTO();
            response = await billEnbillAndBillPaymentLinkSrv.MoveToOpenSingleOrBulk(request);
            if (response != null && !string.IsNullOrEmpty(response.Status))
                return Ok(response);
            else
                return NotFound(response);
        }

        [Route("BulkBillAndPaymentLinking")]
        [HttpPost]
        [ProducesResponseType(typeof(BillPaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkBillAndBillPaymentLinking(List<BulkPostBillPayRequest> bbp)
        {
            BillPaymentResponse response = new BillPaymentResponse();
            try
            {
                response = await billEnbillAndBillPaymentLinkSrv.BulkBillPayLink(bbp, getUserID(), getClientID(), getClientName());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BillPaymentResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = Constants.MSG_ENDPOINT_ERROR
                });
            }
        }

        [Route("PostAllBillPayLinkPayments")]
        [HttpPost]
        [ProducesResponseType(typeof(BillPaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAllBillPayLinkPayments(BillsAndPaymentsReq request)
        {
            BillPaymentResponse response = new BillPaymentResponse();
            try
            {
                response = await billEnbillAndBillPaymentLinkSrv.PostAllBillPayLinkPayments(request, getUserID(), getClientID(), getClientName());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BillPaymentResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = Constants.MSG_ENDPOINT_ERROR
                });
            }
        }

        [Route("BillPayLinkVendors")]
        [HttpPost]
        [ProducesResponseType(typeof(BillPaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBillPayLinkVendors(BillsAndPaymentsReq request)
        {
            VendorListResponse response = new VendorListResponse();
            try
            {
                response = await billEnbillAndBillPaymentLinkSrv.GetBillPayLinkVendors(request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BillPaymentResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = Constants.MSG_ENDPOINT_ERROR
                });
            }
        }

        [Route("IsJournalPaymentExist")]
        [HttpPost]
        [ProducesResponseType(typeof(BillPaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsJournalPaymentExist(BillsAndPaymentsReq request)
        {
            GenericBoolResponse response = new GenericBoolResponse();
            try
            {
                response = await billEnbillAndBillPaymentLinkSrv.IsJournalPaymentExist(request);
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BillPaymentResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = Constants.MSG_ENDPOINT_ERROR
                });
            }
        }

        [Route("ListPaymentIDS")]
        [HttpPost]
        [ProducesResponseType(typeof(BillPaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaymentIDS(BillsAndPaymentsReq request)
        {
            AutoMatchedPaymentIDS response = new AutoMatchedPaymentIDS();
            try
            {
                response = await billEnbillAndBillPaymentLinkSrv.GetPaymentIDS(request, getClientID());
                if (response != null)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new AutoMatchedPaymentIDS
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = Constants.MSG_ENDPOINT_ERROR
                });
            }
        }
    }

}
