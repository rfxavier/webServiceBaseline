using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ViewMobile.Pediddo.WebService.Mobile.Class.Messages;
using ViewMobile.Pediddo.WebService.Mobile.Messages;

namespace ViewMobile.Pediddo.WebService.Mobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IPediddoService
    {
        [OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveUserAuthentication/{Identification},{Email},{Password},{PushToken},{DeviceOS}")]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveUserAuthentication")]
        ReceiveUserAuthenticationResponse ReceiveUserAuthentication(ReceiveUserAuthenticationRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendMinimumVersion")]
        SendMinimumVersionResponse SendMinimumVersion(SendMinimumVersionRequest request);

        [OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendNewRegistration/{BuyerName},{PhoneNumber},{Email},{Password},{CustomerName},{CNPJ},{SegmentID}")]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendNewRegistration")]
        SendNewRegistrationResponse SendNewRegistration(SendNewRegistrationRequest request);

        [OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendLinkUpSeller/{Identification},{SerialKey},{Code}")]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendLinkUpSeller")]
        SendLinkUpResponse SendLinkUpSeller(SendLinkUpRequest request);

        [OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveListSupplier/{Identification},{SerialKey},{Page}")]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveListSupplier")]
        ReceiveListSupplierResponse ReceiveListSupplier(ReceiveListSupplierRequest request);

        [OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendCallMe/{Identification},{SerialKey},{SupplierID,{SellerID}")]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendCallMe")]
        SendCallMeResponse SendCallMe(SendCallMeRequest request);

        //[OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendMePromotion/{Identification},{SerialKey},{SupplierID},{SellerID}")]
        //SendMePromotionResponse SendMePromotion(string Identification, string SerialKey, string SupplierID, string SellerID);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveMyLastOrder")]
        ReceiveMyLastOrderResponse ReceiveMyLastOrder(ReceiveMyLastOrderRequest request);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendMyLastOrder/{Identification},{SerialKey},{SupplierID},{SellerID}")]
        SendMyLastOrderResponse SendMyLastOrder(string Identification, string SerialKey, string SupplierID, string SellerID);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveListMyOrder")]
        ReceiveListMyOrderResponse ReceiveListMyOrder(ReceiveListMyOrderRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveListMyOrderDashboard")]
        ReceiveListMyOrderDashboardResponse ReceiveListMyOrderDashboard(ReceiveListMyOrderDashboardRequest request);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveMyOrderDetail/{Identification},{SerialKey},{MyOrderID}")]
        ReceiveMyOrderDetailResponse ReceiveMyOrderDetail(string Identification, string SerialKey, string MyOrderID);

        [OperationContract]
        [WebInvoke(Method="POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveListMyNotification")]
        ReceiveListMyNotificationResponse ReceiveListMyNotification(ReceiveListMyNotificationRequest request);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveListCustomer")]
        ReceiveListSegmentResponse ReceiveListCustomer();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendProposalReply/{Identification},{SerialKey},{MyOrderID},{Status}")]
        SendProposalReplyResponse SendProposalReply(string Identification, string SerialKey, string MyOrderID, string Status);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveListPromotion/{Identification},{SerialKey},{Page}")]
        ReceiveListPromotionResponse ReceiveListPromotion(string Identification, string SerialKey, string Page);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveListPromotionDetail/{Identification},{SerialKey},{PromotionID}")]
        ReceiveListPromotionDetailResponse ReceiveListPromotionDetail(string Identification, string SerialKey, string PromotionID);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendPromotionAnswer/{Identification},{SerialKey},{PromotionID},{Answer}")]
        SendPromotionAnswerResponse SendPromotionAnswer(string Identification, string SerialKey, string PromotionID, string Answer);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendNotificationRead/{Identification},{SerialKey},{NotificationID}")]
        SendNotificationReadResponse SendNotificationRead(string Identification, string SerialKey, string NotificationID);

        [OperationContract]
        [WebInvoke(Method="POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendUpdateBuyerData")]
        SendUpdateBuyerDataResponse SendUpdateBuyerData(SendUpdateBuyerDataRequest request);

        [OperationContract]
        [WebInvoke(Method="POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendChangePassword")]
        SendChangePasswordResponse SendChangePassword(SendChangePasswordRequest request);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendForgotMyPassword/{Email}")]
        SendForgotMyPasswordResponse SendForgotMyPassword(string Email);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveBuyerData/{Identification},{SerialKey}")]
        ReceiveBuyerDataResponse ReceiveBuyerData(string Identification, string SerialKey);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveListProduct/{Identification},{SerialKey},{CategoryID},{Page}")]
        ReceiveListProductResponse ReceiveListProduct(string Identification, string SerialKey, string CategoryID, string Page);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveProductList")]
        ReceiveProductListResponse ReceiveProductList(ReceiveProductListRequest request);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveListCategory/{Identification},{SerialKey},{SupplierID}")]
        ReceiveListCategoryResponse ReceiveListCategory(string Identification, string SerialKey, string SupplierID);

        //**Endpoint Temporário
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ResetBuyerSellerCode/{SellerID},{BuyerID},{Code}")]
        ResetBuyerSellerCodelinkResponse ResetBuyerSellerCode(string SellerID, string BuyerID, string Code);


        //[OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendMyOrder/{Identification},{SerialKey},{SupplierID},{SellerID}")]
        //SendMyOrderResponse SendMyOrder(string Identification, string SerialKey, string SupplierID, string SellerID);

        //[OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendMyOrderProduct/{Identification},{SerialKey},{MyOrderID},{ProductID},{Quantity},{Amount}")]
        //SendMyOrderProductResponse SendMyOrderProduct(string Identification, string SerialKey, string MyOrderID, string ProductID, string Quantity, string Amount);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendNewMyOrder")]
        SendNewMyOrderResponse SendNewMyOrder(SendNewMyOrderRequest request);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveListMyBuyer/{Identification},{SerialKey}")]
        ReceiveListMyBuyerResponse ReceiveListMyBuyer(string Identification, string SerialKey);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveMyBuyerDetail/{Identification},{SerialKey},{BuyerID}")]
        ReceiveMyBuyerDetailResponse ReceiveMyBuyerDetail(string Identification, string SerialKey, string BuyerID);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendNewBuyer/{Identification},{SerialKey},{BuyerName},{PhoneNumber},{Email},{Password},{Admin}")]
        SendNewBuyerResponse SendNewBuyer(string Identification, string SerialKey, string BuyerName, string PhoneNumber, string Email, string Password, string Admin);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendUpdateBuyer/{Identification},{SerialKey},{BuyerID},{BuyerName},{PhoneNumber},{Password},{IsBlocked},{Admin}")]
        SendUpdateBuyerResponse SendUpdateBuyer(string Identification, string SerialKey, string BuyerID, string BuyerName, string PhoneNumber, string Password, string IsBlocked, string Admin);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveListSupplierByBuyer/{Identification},{SerialKey},{BuyerID}")]
        ReceiveListSupplierByBuyerResponse ReceiveListSupplierByBuyer(string Identification, string SerialKey, string BuyerID);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendRemoveBuyerSupplier/{Identification},{SerialKey},{BuyerID},{SupplierID}")]
        SendRemoveBuyerSupplierResponse SendRemoveBuyerSupplier(string Identification, string SerialKey, string BuyerID, string SupplierID);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendNewUserRegistration/{BuyerName},{PhoneNumber},{Email},{Password},{CustomerName},{SegmentID},{TypeUser}")]
        SendNewUserRegistrationResponse SendNewUserRegistration(string BuyerName, string PhoneNumber, string Email, string Password, string CustomerName, string SegmentID, string TypeUser);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveSellerAuthentication")]
        ReceiveSellerAuthenticationResponse ReceiveSellerAuthentication(ReceiveSellerAuthenticationRequest request);
        
        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendRegisterNewVisit")]
        SendRegisterNewVisitResponse SendRegisterNewVisit(SendRegisterNewVisitRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveVisitStatusList")]
        ReceiveVisitStatusListResponse ReceiveVisitStatusList(ReceiveVisitStatusListRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveSellerCustomerList")]
        ReceiveSellerCustomerListResponse ReceiveSellerCustomerList(ReceiveSellerCustomerListRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendNewSellerOrder")]
        SendNewSellerOrderResponse SendNewSellerOrder(SendNewSellerOrderRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendWorkingDay")]
        SendWorkingDayResponse SendWorkingDay(SendWorkingDayRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendUpdateSellerCustomer")]
        SendUpdateSellerCustomerResponse SendUpdateSellerCustomer(SendUpdateSellerCustomerRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendNewOpportunity")]
        SendNewOpportunityResponse SendNewOpportunity(SendNewOpportunityRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveOpportunityList")]
        ReceiveOpportunityListResponse ReceiveOpportunityList(ReceiveOpportunityListRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendSellerLocation")]
        SendSellerLocationResponse SendSellerLocation(SendSellerLocationRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveReportSale")]
        ReceiveReportSaleResponse ReceiveReportSale(ReceiveReportSaleRequest request);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ReceiveImage/{Serial}/{Identification}/{ImageId}")]
        Stream ReceiveImage(string Serial, string Identification, string ImageId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendCustomerRegistration")]
        SendCustomerRegistrationResponse SendCustomerRegistration(SendCustomerRegistrationRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendNewBill")]
        SendNewBillResponse SendNewBill(SendNewBillRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/UpdateBill")]
        UpdateBillResponse UpdateBill(UpdateBillRequest request);



        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendCreateCustomerLocation")]
        SendCreateCustomerLocationResponse SendCreateCustomerLocation(SendCreateCustomerLocationRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveCustomerLocationList")]
        ReceiveCustomerLocationListResponse ReceiveCustomerLocationList(ReceiveCustomerLocationListRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveCategoryList")]
        ReceiveCategoryListResponse ReceiveCategoryList(ReceiveCategoryListRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendCreateMyOrder")]
        SendCreateMyOrderResponse SendCreateMyOrder(SendCreateMyOrderRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveCustomerCredit")]
        ReceiveCustomerCreditResponse ReceiveCustomerCredit(ReceiveCustomerCreditRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveCalculateShippingCost")]
        ReceiveCalculateShippingCostResponse ReceiveCalculateShippingCost(ReceiveCalculateShippingCostRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendCustomerCreditPurchase")]
        SendCustomerCreditPurchaseResponse SendCustomerCreditPurchase(SendCustomerCreditPurchaseRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendUpdateCustomerLocation")]
        SendUpdateCustomerLocationResponse SendUpdateCustomerLocation(SendUpdateCustomerLocationRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveMyOrderList")]
        ReceiveMyOrderListResponse ReceiveMyOrderList(ReceiveMyOrderListRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveDealerAuthentication")]
        ReceiveDealerAuthenticationResponse ReceiveDealerAuthentication(ReceiveDealerAuthenticationRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveOrderList")]
        ReceiveOrderListResponse ReceiveOrderList(ReceiveOrderListRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveBuyerNotificationList")]
        ReceiveBuyerNotificationListResponse ReceiveBuyerNotificationList(ReceiveBuyerNotificationListRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendBuyerProfilePhoto")]
        SendBuyerProfilePhotoResponse SendBuyerProfilePhoto(SendBuyerProfilePhotoRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveContentToShare")]
        ReceiveContentToShareResponse ReceiveContentToShare(ReceiveContentToShareRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendContentToShare")]
        SendContentToShareResponse SendContentToShare(SendContentToShareRequest request);


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveAdvice")]
        ReceiveAdviceResponse ReceiveAdvice(ReceiveAdviceRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendAdviceAnswer")]
        SendAdviceAnswerResponse SendAdviceAnswer(SendAdviceAnswerRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveAdviceList")]
        ReceiveAdviceListResponse ReceiveAdviceList(ReceiveAdviceListRequest request);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ReceiveBannerImage/{Serial}/{Type}")]
        Stream ReceiveBannerImage(string Serial, string Type);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveOrder")]
        ReceiveOrderResponse ReceiveOrder(ReceiveOrderRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveListDealer")]
        ReceiveListDealerResponse ReceiveListDealer(ReceiveListDealerRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendAssignOrder")]
        SendAssignOrderResponse SendAssignOrder(SendAssignOrderRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendCreateOrderInteraction")]
        SendCreateOrderInteractionResponse SendCreateOrderInteraction(SendCreateOrderInteractionRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendAdjustCustomerLocation")]
        SendAdjustCustomerLocationResponse SendAdjustCustomerLocation(SendAdjustCustomerLocationRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendDeliveryHelp")]
        SendDeliveryHelpResponse SendDeliveryHelp(SendDeliveryHelpRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveSellerAdvice")]
        ReceiveSellerAdviceResponse ReceiveSellerAdvice(ReceiveSellerAdviceRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendSellerAdviceAnswer")]
        SendSellerAdviceAnswerResponse SendSellerAdviceAnswer(SendSellerAdviceAnswerRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveSellerAdviceList")]
        ReceiveSellerAdviceListResponse ReceiveSellerAdviceList(ReceiveSellerAdviceListRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveDealerAdvice")]
        ReceiveDealerAdviceResponse ReceiveDealerAdvice(ReceiveDealerAdviceRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendDealerAdviceAnswer")]
        SendDealerAdviceAnswerResponse SendDealerAdviceAnswer(SendDealerAdviceAnswerRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveDealerAdviceList")]
        ReceiveDealerAdviceListResponse ReceiveDealerAdviceList(ReceiveDealerAdviceListRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveCustomerList")]
        ReceiveCustomerListResponse ReceiveCustomerList(ReceiveCustomerListRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveSellerList")]
        ReceiveSellerListResponse ReceiveSellerList(ReceiveSellerListRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendNewCustomerReVisit")]
        SendNewCustomerReVisitResponse SendNewCustomerReVisit(SendNewCustomerReVisitRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendCancelMyOrder")]
        SendCancelMyOrderResponse SendCancelMyOrder(SendCancelMyOrderRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveExtractCreditList")]
        ReceiveExtractCreditListResponse ReceiveExtractCreditList(ReceiveExtractCreditListRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendDeleteLocation")]
        SendDeleteLocationResponse SendDeleteLocation(SendDeleteLocationRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendUpdateOrderProduct")]
        SendUpdateOrderProductResponse SendUpdateOrderProduct(SendUpdateOrderProductRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveSellerOrderList")]
        ReceiveSellerOrderListResponse ReceiveSellerOrderList(ReceiveSellerOrderListRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendNewJapoUser")]
        SendNewJapoUserResponse SendNewJapoUser(SendNewJapoUserRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendCreateJapoUserOrder")]
        SendCreateJapoUserOrderResponse SendCreateJapoUserOrder(SendCreateJapoUserOrderRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveMeterCategoryList")]
        ReceiveMeterCategoryListResponse ReceiveMeterCategoryList(ReceiveMeterCategoryListRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendCalculateMeter")]
        SendCalculateMeterResponse SendCalculateMeter(SendCalculateMeterRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveListSupplierByAround")]
        ReceiveListSupplierByAroundResponse ReceiveListSupplierByAround(ReceiveListSupplierByAroundRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/SendSupplierRecommendation")]
        SendSupplierRecommendationResponse SendSupplierRecommendation(SendSupplierRecommendationRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/ReceiveProductListCatalog")]
        ReceiveProductListCatalogResponse ReceiveProductListCatalog(ReceiveProductListCatalogRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "/UpdateUserDevice")]
        ReceiveUpdateUserDeviceResponse UpdateUserDevice(ReceiveUpdateUserDeviceRequest request);

    }
}