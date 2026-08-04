using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.DTO.Model.Base.Contracts;
using Payable.Domain.DTO;
using Payable.Domain.DTO.Model;
using Common.Domain.DTO.App;
using Microsoft.AspNetCore.Http;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model;
using Payable.Domain.DTO.Req;

namespace Payable.Domain.DTO.Resp
{
    public class GetACHDetailsResponse : CorporationACHDTO, IStatusDTO
    {
        public GetACHDetailsResponse()
        {
            this.Status = Constants.MSG_ACHNEXT;
            this.StatusCode = StatusCodes.Status404NotFound;
        }
        /// <summary>
        /// ACHProviderType enum
        /// </summary>
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class CreateACHVendorResponse : StatusDTO
    {
        public CreateACHVendorResponse()
        {
            StatusCode = StatusCodes.Status500InternalServerError;
            Status = Constants.MSG_FAILED;
        }
        public string CustomerToken { get; set; }
        public string AddressToken { get; set; }
    }
    public class GetVendorAccDetailsResponse : StatusDTO
    {
        public string AccountHolderName { get; set; }
        public string FundingSourceAccountNumber { get; set; }
        public string AccountType { get; set; }
        public string RoutingNumber { get; set; }
        public string CVV { get; set; }
        public string Title { get; set; }
        public string NameOnCard { get; set; }
        public string CCAccountNumber { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
        public string CardType { get; set; }
        public bool IsDefault { get; set; }

    }
    public class CreatAccDetailsResponse : StatusDTO
    {
        public CreatAccDetailsResponse()
        {
            StatusCode = StatusCodes.Status500InternalServerError;
            Status = Constants.MSG_FAILED;
        }
        public string PayMethodToken { get; set; }
        public string MaskedAccNum { get; set; }
        public bool IsDefault { get; set; }
    }
    public class AddressValidationResponse : StatusDTO
    {
        public AddressValidationResponse()
        {
            Status = Constants.MSG_SUCCESS;
            StatusCode = StatusCodes.Status200OK;
        }
        public bool HasErrors { get; set; } = false;
    }
    public class ACHDetailsValidationResponse : StatusDTO
    {
        public ACHDetailsValidationResponse()
        {
            Status = Constants.MSG_SUCCESS;
            StatusCode = StatusCodes.Status200OK;
        }
    }
    public class CreateACHVendorSuccessResponse : StatusDTO
    {
        public CreateForteVendorSuccessResponse Forte { get; set; }
        public CreateSwirePayVendorSuccessResponse Swirepay { get; set; }
    }
    public class CreateFundingSourceSuccessResponse : StatusDTO
    {
        public CreateForteAccSuccessResponse Forte { get; set; }
        public CreateSwirePayAccSuccessResponse Swirepay { get; set; }
    }
    public class CreateCCFundingSourceSuccessResponse : StatusDTO
    {
        public CreateForteCCSuccessResponse Forte { get; set; }
    }
    public class UpdateACHAdressSuccessResponse : StatusDTO
    {
        public UpdateForteAddressSuccessResponse Forte { get; set; }
        public UpdateSwirePayAddressSuccessResponse SwirePay { get; set; }
    }
    public class UpdateACHVendorSuccessResponse : StatusDTO
    {
        public UpdateForteVendorSuccessResponse Forte { get; set; }
        public UpdateSwirePayVendorSuccessResponse Swirepay { get; set; }
    }
    public class GetFundingSourceSuccessResponse : StatusDTO
    {
        public GetForteFundingSourceSuccessResponse Forte { get; set; }
        public GetSwirePayFundingSourceSuccessResponse Swirepay { get; set; }

    }
    public class DeleteFundingSourceSuccessResponse : StatusDTO
    {
        public DeleteForteFundingSourceSuccessResponse Forte { get; set; }
        public DeleteSwirePayFundingSourceSuccessResponse Swirepay { get; set; }

    }
    public class DeleteVendorSuccessResponse : StatusDTO
    {
        public DeleteForteVendorSuccessResponse Forte { get; set; }
        public DeleteSwirePayVendorSuccessResponse Swirepay { get; set; }

    }
    public class DeleteForteVendorSuccessResponse
    {
        public string statusCode { get; set; }
        public SwirePayDeleteResponse data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public SwirePayDeleteResponse result { get; set; }

    }
    public class DeleteSwirePayVendorSuccessResponse
    {
        public string statusCode { get; set; }
        public SwirePayData data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public SwirePayDeleteResponse result { get; set; }

    }
    public class DeleteForteFundingSourceSuccessResponse
    {
        public string statusCode { get; set; }
        public ForteDeleteData data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public ForteDeleteData result { get; set; }

    }
    public class DeleteSwirePayFundingSourceSuccessResponse
    {
        public string statusCode { get; set; }
        public SwirePayData data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public SwirePayDeleteResponse result { get; set; }

    }
    public class GetForteFundingSourceSuccessResponse
    {
        public string statusCode { get; set; }
        public ForteGetFundingAccData data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public ForteGetFundingAccData result { get; set; }
    }
    public class GetSwirePayFundingSourceSuccessResponse
    {
        public string statusCode { get; set; }
        public SwirePayAccCreateData data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public SwirePayAccResult result { get; set; }

    }
    public class UpdateForteVendorSuccessResponse
    {
        public string statusCode { get; set; }
        public ForteVendUpdateData data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public ForteVendUpdateData result { get; set; }
    }
    public class UpdateSwirePayVendorSuccessResponse
    {
        public string statusCode { get; set; }
        public SwirePayData data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public SwirePayResult result { get; set; }

    }
    public class UpdateForteAddressSuccessResponse
    {
        public string statusCode { get; set; }
        public ForteAddUpdateData data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public ForteAddUpdateData result { get; set; }

    }
    public class UpdateSwirePayAddressSuccessResponse
    {
        public string statusCode { get; set; }
        public SwirePayData data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public SwirePayResult result { get; set; }

    }
    public class CreateForteCCSuccessResponse
    {
        public string statusCode { get; set; }
        public ForteCCCreateData data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public ForteCCCreateData result { get; set; }
    }
    public class CreateForteAccSuccessResponse
    {
        public string statusCode { get; set; }
        public ForteAccCreateData data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public ForteAccCreateData result { get; set; }
    }
    public class CreateSwirePayAccSuccessResponse
    {
        public string statusCode { get; set; }
        public SwirePayAccCreateData data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public SwirePayAccResult result { get; set; }

    }
    public class CreateForteVendorSuccessResponse
    {
        public string statusCode { get; set; }
        public VendorCreateResult data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public VendorCreateResult result { get; set; }
    }
    public class CreateSwirePayVendorSuccessResponse
    {
        public string statusCode { get; set; }
        public SwirePayData data { get; set; }
        public string message { get; set; }
        public string pgResponse { get; set; }
        public SwirePayResult result { get; set; }

    }
}
