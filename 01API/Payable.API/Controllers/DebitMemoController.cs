using Common.Domain.DTO.Resp;
using Common.Domain.DTO.App;
using Common.API.ActionFilters;
using Common.API.Authorization;
using Payable.App.Contracts;
using Payable.Domain.DTO.Req;
using Microsoft.AspNetCore.Mvc;
using Common.App.Contracts;
using Common.Domain.DTO.Req;
using static Payable.Domain.DTO.Resp.PayableResponseDTO;

namespace Payable.API.Controllers
{
    [Route("v1")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class DebitMemoController : ControllerBase
    {
        #region Fields
        private readonly ICommonService commonSrv;
        private readonly IDebitMemoService debitMemoSrv;

        #endregion

        #region Ctor
        public DebitMemoController(ICommonService coreProperty, IDebitMemoService debitMemoService)
        {
            this.commonSrv = coreProperty;
            this.debitMemoSrv=debitMemoService;
        }

        #endregion


        #region DebitMemo

        /// <summary>
        /// It gets List Of DebitMemo Based on corporationid
        /// </summary>
        /// <param name="DebitMemoListData">It represents BillListReq</param>
        /// <returns>It returns List of DebitMemo to BillList</returns>
        [Route("DebitMemo/List")]
        [HttpPost]
        public async Task<IActionResult> GetDebitMemos([FromQuery] JournalSearchRequest DebitMemoListReq)
        {
            JEListResponse? response = null;
            try
            {
                response = await debitMemoSrv.GetDebitMemos(DebitMemoListReq);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }

        }

        /// <summary>
        /// It gets debitmemo based DebitMemoId
        /// </summary>
        /// <param name="DebitMemoId">It represents LoadByIDReq</param>
        /// <returns>It returns DebitMemo Data to BillLoadResponse</returns>
        [Route("DebitMemo/Load")]
        [HttpPost]
        public async Task<IActionResult> GetDebitMemo(LoadByIDRequest DebitMemoId)
        {
            BillEntryResponse? response = null;
            try
            {
                response = await debitMemoSrv.GetDebitMemo(DebitMemoId);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }

        }

        /// <summary>
        /// It Creates DebitMemo Based on DebitMemoData
        /// </summary>
        /// <param name="DebitMemoData">It represents BillEntryReq</param>
        /// <returns>It returns DebitMemo Id,status,statusmsg</returns>
        //[Route("DebitMemo/Create")]
        //[HttpPost]
        //public async Task<IActionResult> CreateDebitMemo( BillEntryRequest DebitMemoData)
        //{
        //    JournalResponse? response = null;
        //    try
        //    {
        //        response = await debitMemoSrv.CreateDebitMemo(DebitMemoData);
        //        if (response != null && response.StatusCode == StatusCodes.Status200OK)
        //            return Ok(response);
        //        else
        //            return NotFound(response);
        //    }
        //    catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        //    finally { response = null; }

        //}

        /// <summary>
        /// It updates the DebitMemo Based on DebitMemoData
        /// </summary>
        /// <param name="DebitMemoData">It represents BillEntryReq</param>
        /// <returns>It returns updated DebitMemoId,status,statusmsg</returns>
        //[Route("DebitMemo/Update")]
        //[HttpPost]
        //public async Task<IActionResult> UpdateDebitMemo( BillEntryRequest DebitMemoData)
        //{
        //    JournalResponse? response = null;
        //    try
        //    {
        //        response = await debitMemoSrv.UpdateDebitMemo(DebitMemoData);
        //        if (response != null && response.StatusCode == StatusCodes.Status200OK)
        //            return Ok(response);
        //        else
        //            return NotFound(response);
        //    }
        //    catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
        //    finally { response = null; }

        //}

        /// <summary>
        /// It Deletes the DebitMemo based on DebitMemoID
        /// </summary>
        /// <param name="DebitMemoId">It represents LoadByIdReq</param>
        /// <returns>It returns Deleted DebitMemoID,status,statusmsg</returns>
        [Route("DebitMemo/Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteDebitMemo(LoadByIDRequest DebitMemoId)
        {
            JournalResponse? response = null;
            try
            {
                response = await debitMemoSrv.DeleteDebitMemo(DebitMemoId);
                if (response != null && response.StatusCode == StatusCodes.Status200OK)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, Constants.MSG_ENDPOINT_ERROR); }
            finally { response = null; }
        }

        #endregion
    }
}
