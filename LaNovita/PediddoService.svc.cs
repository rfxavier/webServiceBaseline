using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using ViewMobile.Pediddo.Core.Crypto;
using ViewMobile.Pediddo.Core.Data.BLL;
using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Enumeration;
using ViewMobile.Pediddo.Core.Notifications;
using ViewMobile.Pediddo.Core.Security;
using ViewMobile.Pediddo.WebService.Mobile.Class.Messages;
using ViewMobile.Pediddo.WebService.Mobile.Enumerations;
using ViewMobile.Pediddo.WebService.Mobile.Messages;

namespace ViewMobile.Pediddo.WebService.Mobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class PediddoService : IPediddoService
    {
        public ReceiveUserAuthenticationResponse ReceiveUserAuthentication(ReceiveUserAuthenticationRequest request)
        {
            //string Identification, string Email, string Password, string PushToken, string DeviceOS
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveUserAuthenticationResponse response = new ReceiveUserAuthenticationResponse();

            Security securitypass = new Security();
            string password = securitypass.MD5(request.Password);

            Buyer _Buyer = new BuyerBLL().Authentication(request.Email, password);
            if (_Buyer != null)
            {

                if (true) //(_Buyer.Active.Value)
                {

                    if (true) //(!_Buyer.IsBlocked.Value)
                    {

                        if (true) //(_Buyer.SerialKey != null)
                        {

                            //int notificationNotRead = NotificationBLL.GetCountNotRead(_Buyer);
                            //int myOrderAwaiting = MyOrderBLL.GetCountAwaiting(_Buyer);
                            //int promotionNotRead = BuyerPromotionBLL.ListNotRead(_Buyer).Where(p=>p.Promotion.ExpirationDate >= DateTime.Now).Count();

                            //BuyerDevice _BuyerDevice;
                            //Device _Device;
                            //_Device = DeviceBLL.GetByDeviceToken(request.Identification);
                            //if (_Device == null)
                            //{
                            //    _BuyerDevice = new BuyerDevice();
                            //    _BuyerDevice.BuyerID = _Buyer.BuyerID;
                            //    _BuyerDevice.DateCreated = DateTime.Now;
                            //    _BuyerDevice.Active = true;

                            //    _Device = new Device();
                            //    _Device.DeviceToken = request.Identification;
                            //    _Device.PushToken = request.PushToken;
                            //    _Device.Status = 0;
                            //    _Device.DeviceOS = Convert.ToInt32(request.DeviceOs);
                            //    _Device.BuyerDevices = new EntitySet<BuyerDevice>();
                            //    _Device.BuyerDevices.Add(_BuyerDevice);
                            //    _Device.DateCreated = DateTime.Now;
                            //    _Device.Active = true;

                            //    _Device = new DeviceBLL().Save(_Device);
                                
                            //}
                            //else
                            //{
                            //    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, _Device);
                            //    if(_BuyerDevice == null)
                            //    {
                            //        _BuyerDevice = new BuyerDevice();
                            //        _BuyerDevice.BuyerID = _Buyer.BuyerID;
                            //        _BuyerDevice.DeviceID = _Device.DeviceID;
                            //        _BuyerDevice.DateCreated = DateTime.Now;
                            //        _BuyerDevice.Active = true;

                            //        new BuyerDeviceBLL().Save(_BuyerDevice);
                            //    }
                            //}

                            response.Name = _Buyer.FirstName + " " + _Buyer.LastName;
                            response.BuyerID = _Buyer.BuyerID;
                            response.Admin = _Buyer.Admin.Value;
                            response.PushToken = string.Empty; //_Device.PushToken;
                            response.SerialKey = _Buyer.SerialKey;

                            //response.PhotoBannerList = new List<int>();
                            //List<Banner> homeBannerList = BannerBLL.ListByType((int)BannerEnum.Type.HomeBanner);
                            //if (homeBannerList.Count > 0)
                            //{
                            //    response.PhotoBannerList = homeBannerList.Select(b => b.AppImageID).ToList();
                            //}

                            //response.ContributorNumber = _Buyer.Customer.CNPJ;
                            response.PhoneNumber = _Buyer.PhoneNumber;
                            response.Email = _Buyer.Email;

                            status = DeviceCommunicationLogStatus.Success;
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E007";
                            response.ErrorMessage = Resources.ErrorRes.E007;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E005";
                        response.ErrorMessage = Resources.ErrorRes.E005;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E004";
                    response.ErrorMessage = Resources.ErrorRes.E004;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E001";
                response.ErrorMessage = Resources.ErrorRes.E001;
            }

            return response;

        }

        public SendNewRegistrationResponse SendNewRegistration(SendNewRegistrationRequest request)
        {
            //string BuyerName, string PhoneNumber, string Email, string Password, string CustomerName, string CNPJ, string SegmentID
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendNewRegistrationResponse response = new SendNewRegistrationResponse();

            Buyer _Buyer = null;
            _Buyer = BuyerBLL.HasEmail(request.Email);
            if (_Buyer == null)
            {
                Customer _Customer = null;
                _Customer = CustomerBLL.GetByCNPJ(request.CNPJ);
                if (_Customer == null)
                {

                    #region Generacion de SerialKey y Encriptacion del password
                    /// <summary>
                    /// Array de bytes utilizado para gerar o SerialKey
                    /// </summary>
                    byte[] _MyCompanyKey = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
                                                                11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 
                                                                    21, 22, 23, 24 };

                    /// <summary>
                    /// Array de bytes utilizado para gerar o SerialKey
                    /// </summary>
                    byte[] _MyCompanyIV = { 1, 2, 3, 4, 5, 6, 7, 8 };

                    KeySeller locKeyHandler = new KeySeller(_MyCompanyKey, _MyCompanyIV);

                    string keyvalue = locKeyHandler.SellKey(request.Password, request.Email, request.Password, Key.ProductOrProductFeatureSet.Product2_PaidFullVersion);

                    Security security = new Security();
                    request.Password = security.MD5(request.Password);
                    #endregion

                    string[] names = request.BuyerName.Split(' ');
                    string firstName = string.Empty;
                    string lastName = string.Empty;

                    if (names.Count() == 1)
                    {
                        firstName = names[0];
                    }
                    else if (names.Count() == 2)
                    {
                        firstName = names[0];
                        lastName = names.Last();
                    }
                    else if (names.Count() == 3)
                    {
                        firstName = names[0];
                        lastName = names[1] + " " + names[2];
                    }
                    else if (names.Count() > 3)
                    {
                        firstName = names[0] + " " + names[1];
                        lastName = names[2] + " " + names[3];
                    }

                    _Buyer = new Buyer();
                    _Buyer.FirstName = firstName.ToString().Trim();
                    _Buyer.LastName = lastName == string.Empty ? _Buyer.LastName : lastName;
                    _Buyer.Email = request.Email;
                    _Buyer.Password = request.Password;
                    _Buyer.PhoneNumber = request.PhoneNumber;
                    _Buyer.SerialKey = keyvalue;
                    _Buyer.IsBlocked = false;
                    _Buyer.Admin = true;
                    _Buyer.DateCreated = DateTime.Now;
                    _Buyer.Active = true;

                    _Customer = new Customer();
                    _Customer.Name = request.CustomerName;
                    _Customer.CNPJ = request.CNPJ;
                    _Customer.SegmentID = Convert.ToInt32(request.SegmentID);
                    _Customer.DateCreated = DateTime.Now;
                    _Customer.Active = true;

                    _Buyer.Customer = _Customer;

                    #region Temporal para los que descargan del play store
                    BuyerSeller newBuyerSeller = new BuyerSeller();
                    newBuyerSeller.SellerID = 6;

                    _Buyer.BuyerSellers = new EntitySet<BuyerSeller>();
                    _Buyer.BuyerSellers.Add(newBuyerSeller);
                    #endregion

                    _Buyer = new BuyerBLL().Save(_Buyer);

                    ///TODO: Descomentar bloco de enviar email
                    // Bloco comentado para testes devido a falha na autenticação do usuário SMTP 
                    #region EMAIL 
                    //SendGrid.Mail myMessage = SendGrid.Mail.GetInstance();
                    //myMessage.AddTo("comercial@viewmobile.com.br");
                    //myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SenderEmailAccount"].ToString(), "Pediddo");
                    //myMessage.Subject = "Pediddo - Um novo usuario foi cadastrado";
                    //myMessage.Text = "#Comprador:\n" +
                    //                 "Nombre: " + _Buyer.FirstName + "\n" +
                    //                 "Email: " + _Buyer.Email + "\n" +
                    //                 "Telefono: " + _Buyer.PhoneNumber + "\n\n" +
                    //                 "#Empresa:\n" +
                    //                 "Nombre: " + _Customer.CNPJ + "\n" +
                    //                 "CNPJ: " + _Buyer.Email + "\n";

                    
                    ////// Create credentials, specifying your user name and password.
                    //var credentials = new NetworkCredential("viewmobile", "cade163c#");

                    //// Create an SMTP transport for sending email.
                    //var transportWeb = SendGrid.Transport.Web.GetInstance(credentials);
                    //// Send the email.
                    //transportWeb.Deliver(myMessage);
                    #endregion
                    response.BuyerID = _Buyer.BuyerID;
                    status = DeviceCommunicationLogStatus.Success;

                }
                else
                {
                    // The Device could not be validated.
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E020";
                    response.ErrorMessage = Resources.ErrorRes.E020;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                // The Device could not be validated.
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E008";
                response.ErrorMessage = Resources.ErrorRes.E008;
                status = DeviceCommunicationLogStatus.KnownError;
            }

            response.Apple();
            return response;
        }

        public SendLinkUpResponse SendLinkUpSeller(SendLinkUpRequest request)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendLinkUpResponse response = new SendLinkUpResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(request.Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(request.SerialKey);
                if (_Buyer != null)
                {
                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                CodeLink _CodeLink = null;
                                _CodeLink = CodeLinkBLL.GetByCode(request.Code);
                                if (_CodeLink != null)
                                {

                                    BuyerSeller _BuyerSeller = null;
                                    _BuyerSeller = BuyerSellerBLL.GetById(_Buyer.BuyerID, _CodeLink.SellerID.Value);
                                    if (_BuyerSeller == null)
                                    {
                                        CodeLink codeLinkUpdate = new CodeLink();
                                        codeLinkUpdate.CodeLinkID = _CodeLink.CodeLinkID;
                                        codeLinkUpdate.SellerID = _CodeLink.SellerID;
                                        codeLinkUpdate.Code = _CodeLink.Code;
                                        codeLinkUpdate.Used = true;
                                        codeLinkUpdate.DateCreated = _CodeLink.DateCreated;
                                        codeLinkUpdate.Active = _CodeLink.Active;

                                        BuyerSeller newBuyerSeller = new BuyerSeller();
                                        newBuyerSeller.BuyerID = _Buyer.BuyerID;
                                        newBuyerSeller.SellerID = _CodeLink.SellerID.Value;

                                        new CodeLinkBLL().Update(codeLinkUpdate, _CodeLink);
                                        new BuyerSellerBLL().Save(newBuyerSeller);

                                        response.SerialKey = _Buyer.SerialKey;
                                        status = DeviceCommunicationLogStatus.Success;

                                    }
                                    else
                                    {
                                        response.Status = ResponseCodeEnum.Error;
                                        response.ErrorCode = "E010";
                                        response.ErrorMessage = Resources.ErrorRes.E010;
                                    }
                                }
                                else
                                {
                                    response.Status = ResponseCodeEnum.Error;
                                    response.ErrorCode = "E009";
                                    response.ErrorMessage = Resources.ErrorRes.E009;
                                }

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }
        
        public ReceiveListSupplierResponse ReceiveListSupplier(ReceiveListSupplierRequest request)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveListSupplierResponse response = new ReceiveListSupplierResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(request.Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(request.SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                //if (BuyerBLL.HasPermission(ViewMobile.Order.Core.Enumeration.Permission.AppMovil.Realizar_Login, _Buyer))
                                //{

                                List<Supplier> SupplierList;
                                if(request.Page == "0")
                                {
                                    SupplierList = SupplierBLL.ListByBuyer(_Buyer);
                                }
                                else
                                {
                                    int skip = (Convert.ToInt32(request.Page) - 1) * 10;
                                    SupplierList = SupplierBLL.ListByBuyerByPage(_Buyer, skip);
                                }

                                List<Employee> SellerList;
                                response.SupplierList = new ReceiveListSupplierResponse.Supplier[SupplierList.Count()];
                                for (int i = 0; i < SupplierList.Count(); i++)
                                {
                                    response.SupplierList[i] = new ReceiveListSupplierResponse.Supplier();
                                    response.SupplierList[i].SupplierID = SupplierList[i].SupplierID;
                                    response.SupplierList[i].Name = SupplierList[i].Name;
                                    response.SupplierList[i].CNPJ = SupplierList[i].CNPJ == null ? "" : SupplierList[i].CNPJ;
                                    response.SupplierList[i].PhoneNumber = SupplierList[i].PhoneNumber == null ? "" : SupplierList[i].PhoneNumber;

                                    SellerList = SupplierList[i].Employees.Where(s => s.Seller != null && s.Seller.BuyerSellers.Any(bs => bs.BuyerID == _Buyer.BuyerID)).ToList();
                                    response.SupplierList[i].SellerList = new ReceiveListSupplierResponse.Seller[SellerList.Count()];
                                    for (int j = 0; j < SellerList.Count(); j++)
                                    {
                                        response.SupplierList[i].SellerList[j] = new ReceiveListSupplierResponse.Seller();
                                        response.SupplierList[i].SellerList[j].SellerID = SellerList[j].Seller.SellerID;
                                        response.SupplierList[i].SellerList[j].Name = SellerList[j].FirstName + " " + SellerList[j].LastName;
                                        response.SupplierList[i].SellerList[j].SupplierID = SupplierList[i].SupplierID;
                                    }
                                }

                                status = DeviceCommunicationLogStatus.Success;
                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }
        
        public SendCallMeResponse SendCallMe(SendCallMeRequest request)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendCallMeResponse response = new SendCallMeResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(request.Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(request.SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                //if (BuyerBLL.HasPermission(ViewMobile.Order.Core.Enumeration.Permission.AppMovil.Realizar_Login, _Buyer))
                                //{

                                Security security = new Security();
                                Random random = new Random();
                                string ReceiptCode = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(0, 1000).ToString();

                                MyOrder _CallMe = null;
                                _CallMe = MyOrderBLL.GetByStatus((int)MyOrderHistoryEnum.Status.AwaitingAttention, (int)MyOrderEnum.Type.CallMe, _Buyer, Convert.ToInt32(request.SupplierID));
                                if (_CallMe == null)
                                {
                                    _CallMe = new MyOrder();
                                    _CallMe.BuyerID = _Buyer.BuyerID;
                                    _CallMe.SupplierID = Convert.ToInt32(request.SupplierID);
                                    _CallMe.MyOrderType = (int)MyOrderEnum.Type.CallMe;
                                    _CallMe.ReceiptCode = ReceiptCode;
                                    _CallMe.RepeatOrder = false;
                                    _CallMe.DateCreated = DateTime.Now;
                                    _CallMe.Active = true;

                                    MyOrderHistory _CallMeHistory = new MyOrderHistory();
                                    _CallMeHistory.SellerID = request.SellerID != "0" ? Convert.ToInt32(request.SellerID) : _CallMeHistory.SellerID;
                                    _CallMeHistory.Status = (int)MyOrderHistoryEnum.Status.AwaitingAttention;
                                    _CallMeHistory.DateCreated = DateTime.Now;
                                    _CallMeHistory.Active = true;

                                    _CallMe.MyOrderHistories = new EntitySet<MyOrderHistory>();
                                    _CallMe.MyOrderHistories.Add(_CallMeHistory);

                                    _CallMe = new MyOrderBLL().Save(_CallMe);

                                    response.MyOrderID = _CallMe.MyOrderID;
                                    response.ReceiptCode = _CallMe.ReceiptCode;
                                    status = DeviceCommunicationLogStatus.Success;
                                }
                                else
                                {
                                    response.Status = ResponseCodeEnum.Error;
                                    response.ErrorCode = "E011";
                                    response.ErrorMessage = Resources.ErrorRes.E011;
                                }

                                //}
                                //else
                                //{
                                //    response.Status = ResponseCodeEnum.Error;
                                //    response.ErrorCode = "E006";
                                //    response.ErrorMessage = Resources.ErrorRes.E006;
                                //}
                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        //public SendMePromotionResponse SendMePromotion(string Identification, string SerialKey, string SupplierID, string SellerID)
        //{
        //    DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
        //    SendMePromotionResponse response = new SendMePromotionResponse();

        //    Device device = new Device();
        //    device = DeviceBLL.GetByDeviceToken(Identification);
        //    if (device != null)
        //    {

        //        Buyer _Buyer = null;
        //        _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
        //        if (_Buyer != null)
        //        {

        //            BuyerDevice _BuyerDevice;
        //            _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
        //            if (_BuyerDevice != null)
        //            {

        //                if (_Buyer.Active.Value)
        //                {

        //                    if (!_Buyer.IsBlocked.Value)
        //                    {

        //                        //if (BuyerBLL.HasPermission(ViewMobile.Order.Core.Enumeration.Permission.AppMovil.Realizar_Login, _Buyer))
        //                        //{

        //                        Security security = new Security();
        //                        string ReceiptCode = security.MD5(SupplierID + DateTime.Now.Ticks.ToString());

        //                        MyOrder _Promotion = null;
        //                        _Promotion = MyOrderBLL.GetByStatus((int)MyOrderHistoryEnum.Status.AwaitingAttention, (int)MyOrderEnum.Type.Promotion, _Buyer, Convert.ToInt32(SupplierID));
        //                        if (_Promotion == null)
        //                        {
        //                            _Promotion = new MyOrder();
        //                            _Promotion.BuyerID = _Buyer.BuyerID;
        //                            _Promotion.SupplierID = Convert.ToInt32(SupplierID);
        //                            _Promotion.MyOrderType = (int)MyOrderEnum.Type.Promotion;
        //                            _Promotion.ReceiptCode = ReceiptCode;
        //                            _Promotion.DateCreated = DateTime.Now;
        //                            _Promotion.Active = true;

        //                            MyOrderHistory _PromotionHistory = new MyOrderHistory();
        //                            _PromotionHistory.SellerID = SellerID != "0" ? Convert.ToInt32(SellerID) : _PromotionHistory.SellerID;
        //                            _PromotionHistory.Status = (int)MyOrderHistoryEnum.Status.AwaitingAttention;
        //                            _PromotionHistory.DateCreated = DateTime.Now;
        //                            _PromotionHistory.Active = true;

        //                            _Promotion.MyOrderHistories = new EntitySet<MyOrderHistory>();
        //                            _Promotion.MyOrderHistories.Add(_PromotionHistory);

        //                            _Promotion = new MyOrderBLL().Save(_Promotion);

        //                            response.MyOrderID = _Promotion.MyOrderID;
        //                            response.ReceiptCode = _Promotion.ReceiptCode;
        //                            status = DeviceCommunicationLogStatus.Success;
        //                        }
        //                        else
        //                        {
        //                            response.Status = ResponseCodeEnum.Error;
        //                            response.ErrorCode = "E012";
        //                            response.ErrorMessage = Resources.ErrorRes.E012;
        //                        }

        //                        //}
        //                        //else
        //                        //{
        //                        //    response.Status = ResponseCodeEnum.Error;
        //                        //    response.ErrorCode = "E006";
        //                        //    response.ErrorMessage = Resources.ErrorRes.E006;
        //                        //}
        //                    }
        //                    else
        //                    {
        //                        response.Status = ResponseCodeEnum.Error;
        //                        response.ErrorCode = "E005";
        //                        response.ErrorMessage = Resources.ErrorRes.E005;
        //                    }
        //                }
        //                else
        //                {
        //                    response.Status = ResponseCodeEnum.Error;
        //                    response.ErrorCode = "E004";
        //                    response.ErrorMessage = Resources.ErrorRes.E004;
        //                }

        //            }
        //            else
        //            {
        //                response.Status = ResponseCodeEnum.Error;
        //                response.ErrorCode = "E003";
        //                response.ErrorMessage = Resources.ErrorRes.E003;
        //                status = DeviceCommunicationLogStatus.KnownError;
        //            }
        //        }
        //        else
        //        {
        //            response.Status = ResponseCodeEnum.Error;
        //            response.ErrorCode = "E007";
        //            response.ErrorMessage = Resources.ErrorRes.E007;
        //            status = DeviceCommunicationLogStatus.KnownError;
        //        }
        //    }
        //    else
        //    {
        //        response.Status = ResponseCodeEnum.Error;
        //        response.ErrorCode = "E002";
        //        response.ErrorMessage = Resources.ErrorRes.E002;
        //    }

        //    response.Apple();
        //    return response;
        //}

        public ReceiveMyLastOrderResponse ReceiveMyLastOrder(ReceiveMyLastOrderRequest request)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveMyLastOrderResponse response = new ReceiveMyLastOrderResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(request.Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(request.SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                MyOrder _MyOrder = null;
                                //_MyOrder = MyOrderBLL.GetByStatus((int)MyOrderHistoryEnum.Status.AwaitingAttention, (int)MyOrderEnum.Type.LastOrder, _Buyer, Convert.ToInt32(request.SupplierID));
                                if (_MyOrder == null)
                                {

                                    MyOrder lastMyOrder = MyOrderBLL.GetLastByBuyer((int) MyOrderEnum.Type.LastOrder, _Buyer);

                                    if (lastMyOrder != null)
                                    {
                                        response.MyOrderID = lastMyOrder.MyOrderID;
                                        response.Description = lastMyOrder.Description ?? "";
                                        response.InvoiceNumber = lastMyOrder.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().InvoiceNumber ?? "";
                                        response.Cost = lastMyOrder.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Cost;
                                        response.Cost = response.Cost == null ? 0 : response.Cost;
                                        response.DateCreated = lastMyOrder.DateCreated.Value.ToString("dd/MM/yyyy");
                                        response.ReceiptCode = lastMyOrder.ReceiptCode;
                                        response.SupplierID = (int) lastMyOrder.SupplierID;
                                        response.SupplierName = lastMyOrder.Supplier.Name;
                                        response.SellerID = lastMyOrder.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Seller.SellerID;



                                        response.ReceiptCode = lastMyOrder.ReceiptCode;


                                        response.ProductList = new ReceiveMyLastOrderResponse.Product[lastMyOrder.MyOrderProducts.Count()];
                                        for (int i = 0; i < lastMyOrder.MyOrderProducts.Count(); i++)
                                        {
                                            response.ProductList[i] = new ReceiveMyLastOrderResponse.Product();
                                            response.ProductList[i].ProductId = lastMyOrder.MyOrderProducts[i].ProductID;
                                            response.ProductList[i].ProductName = lastMyOrder.MyOrderProducts[i].Product.Name;
                                            response.ProductList[i].Quantity = lastMyOrder.MyOrderProducts[i].Quantity.Value;
                                            response.ProductList[i].Amount = response.ProductList[i].Quantity;
                                            if (lastMyOrder.MyOrderProducts[i].SalePrice == null)
                                            {
                                                response.ProductList[i].PriceStr = "0.00";
                                            }
                                            else
                                            {
                                                response.ProductList[i].PriceStr = ((decimal) lastMyOrder.MyOrderProducts[i].SalePrice).ToString("#0.00").Replace(",",".");
                                            }
                                            response.ProductList[i].UnitMeasure = lastMyOrder.MyOrderProducts[i].Product.UnitMeasure == null ? string.Empty : lastMyOrder.MyOrderProducts[i].Product.UnitMeasure.Name;
                                            response.ProductList[i].ImageId = (int) lastMyOrder.MyOrderProducts[i].Product.AppImageID;
                                        }

                                        status = DeviceCommunicationLogStatus.Success;
                                    }
                                    else
                                    {
                                        response.Status = ResponseCodeEnum.Error;
                                        response.ErrorCode = "E014";
                                        response.ErrorMessage = Resources.ErrorRes.E014;
                                    }

                                }
                                else
                                {
                                    response.Status = ResponseCodeEnum.Error;
                                    response.ErrorCode = "E013";
                                    response.ErrorMessage = Resources.ErrorRes.E013;
                                }
                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public SendMyLastOrderResponse SendMyLastOrder(string Identification, string SerialKey, string SupplierID, string SellerID)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendMyLastOrderResponse response = new SendMyLastOrderResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {
                                Security security = new Security();
                                Random random = new Random();
                                string ReceiptCode = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(0, 1000).ToString();

                                MyOrder _MyOrder = null;
                                _MyOrder = MyOrderBLL.GetByStatus((int)MyOrderHistoryEnum.Status.AwaitingAttention, (int)MyOrderEnum.Type.LastOrder, _Buyer, Convert.ToInt32(SupplierID));
                                if (_MyOrder == null)
                                {
                                    _MyOrder = new MyOrder();
                                    _MyOrder.BuyerID = _Buyer.BuyerID;
                                    _MyOrder.SupplierID = Convert.ToInt32(SupplierID);
                                    _MyOrder.MyOrderType = (int)MyOrderEnum.Type.LastOrder;
                                    _MyOrder.ReceiptCode = ReceiptCode;
                                    _MyOrder.RepeatOrder = true;
                                    _MyOrder.DateCreated = DateTime.Now;
                                    _MyOrder.Active = true;

                                    MyOrderHistory _MyOrderHistory = new MyOrderHistory();
                                    _MyOrderHistory.SellerID = SellerID != "0" ? Convert.ToInt32(SellerID) : _MyOrderHistory.SellerID;
                                    _MyOrderHistory.Status = (int)MyOrderHistoryEnum.Status.AwaitingAttention;
                                    _MyOrderHistory.DateCreated = DateTime.Now;
                                    _MyOrderHistory.Active = true;

                                    MyOrder lastMyOrder = MyOrderBLL.GetLast((int)MyOrderEnum.Type.LastOrder, _Buyer, Convert.ToInt32(SupplierID));
                                    if (lastMyOrder != null)
                                    {
                                        _MyOrder.Description = lastMyOrder.Description;
                                        _MyOrderHistory.Cost = lastMyOrder.MyOrderHistories.OrderByDescending(o=>o.DateCreated).FirstOrDefault().Cost;
                                    }

                                    _MyOrder.MyOrderHistories = new EntitySet<MyOrderHistory>();
                                    _MyOrder.MyOrderHistories.Add(_MyOrderHistory);

                                    if (lastMyOrder.MyOrderProducts != null && lastMyOrder.MyOrderProducts.Count() > 0)
                                    {
                                        _MyOrder.MyOrderProducts = new EntitySet<MyOrderProduct>();
                                        MyOrderProduct newMyOrderProduct;
                                        foreach (MyOrderProduct myOrderProduct in lastMyOrder.MyOrderProducts)
                                        {
                                            newMyOrderProduct = new MyOrderProduct();
                                            newMyOrderProduct.ProductID = myOrderProduct.ProductID;
                                            newMyOrderProduct.Quantity = myOrderProduct.Quantity;
                                            newMyOrderProduct.Discount = myOrderProduct.Discount;
                                            newMyOrderProduct.DateCreated = DateTime.Now;
                                            newMyOrderProduct.Active = true;

                                            _MyOrder.MyOrderProducts.Add(newMyOrderProduct);
                                        }
                                    }

                                    _MyOrder = new MyOrderBLL().Save(_MyOrder);

                                    response.MyOrderID = _MyOrder.MyOrderID;
                                    response.ReceiptCode = _MyOrder.ReceiptCode;
                                    status = DeviceCommunicationLogStatus.Success;
                                }
                                else
                                {
                                    response.Status = ResponseCodeEnum.Error;
                                    response.ErrorCode = "E013";
                                    response.ErrorMessage = Resources.ErrorRes.E013;
                                }
                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public ReceiveListMyOrderResponse ReceiveListMyOrder(ReceiveListMyOrderRequest request)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveListMyOrderResponse response = new ReceiveListMyOrderResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(request.Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(request.SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                List<MyOrder> MyOrderList;
                                if (request.Page == "0")
                                {
                                    MyOrderList = MyOrderBLL.ListByBuyer(_Buyer);
                                }
                                else
                                {
                                    int skip = (Convert.ToInt32(request.Page) - 1) * 10;
                                    MyOrderList = MyOrderBLL.ListByBuyerByPage(_Buyer, skip);
                                }

                                response.MyOrderList = new ReceiveListMyOrderResponse.MyOrder[MyOrderList.Count()];
                                for (int i = 0; i < MyOrderList.Count(); i++)
                                {
                                    response.MyOrderList[i] = new ReceiveListMyOrderResponse.MyOrder();
                                    response.MyOrderList[i].MyOrderID = MyOrderList[i].MyOrderID;
                                    response.MyOrderList[i].MyOrderType = MyOrderList[i].MyOrderType.Value;
                                    response.MyOrderList[i].SupplierName = MyOrderList[i].Supplier.Name;
                                    response.MyOrderList[i].SellerName = MyOrderList[i].MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Seller.Employee.FirstName + " " +
                                        MyOrderList[i].MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Seller.Employee.LastName ?? "";
                                    response.MyOrderList[i].DateCreated = MyOrderList[i].DateCreated.Value.ToString("dd/MM/yyyy");
                                    response.MyOrderList[i].Status = MyOrderList[i].MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value;
                                    response.MyOrderList[i].StatusDescription = MyOrderHistoryEnum.InteractionIDDescription(response.MyOrderList[i].Status);
                                    response.MyOrderList[i].Observation = MyOrderList[i].MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Observation;
                                    response.MyOrderList[i].Observation = response.MyOrderList[i].Observation == null ? string.Empty : response.MyOrderList[i].Observation;
                                }

                                status = DeviceCommunicationLogStatus.Success;

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public ReceiveMyOrderDetailResponse ReceiveMyOrderDetail(string Identification, string SerialKey, string MyOrderID)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveMyOrderDetailResponse response = new ReceiveMyOrderDetailResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                MyOrder myOrder = MyOrderBLL.GetFullById(Convert.ToInt32(MyOrderID));

                                response.MyOrderID = myOrder.MyOrderID;
                                response.Description = myOrder.Description;
                                response.Cost = myOrder.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Cost.Value;
                                response.Observation = myOrder.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Observation;

                                response.ProductList = new ReceiveMyOrderDetailResponse.Product[myOrder.MyOrderProducts.Count()];
                                for(int i = 0 ; i < myOrder.MyOrderProducts.Count() ; i++)
                                {
                                    response.ProductList[i] = new ReceiveMyOrderDetailResponse.Product();
                                    response.ProductList[i].ProductID = myOrder.MyOrderProducts[i].ProductID;
                                    response.ProductList[i].ProductName = myOrder.MyOrderProducts[i].Product.Name;
                                    response.ProductList[i].Quantity = myOrder.MyOrderProducts[i].Quantity.Value;
                                    response.ProductList[i].Amount = myOrder.MyOrderProducts[i].Product.Cost - Convert.ToDecimal((myOrder.MyOrderProducts[i].Product.Cost * myOrder.MyOrderProducts[i].Product.Discount) / 100);
                                }

                                status = DeviceCommunicationLogStatus.Success;

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public ReceiveListMyNotificationResponse ReceiveListMyNotification(ReceiveListMyNotificationRequest request)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveListMyNotificationResponse response = new ReceiveListMyNotificationResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(request.Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(request.SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                //if (BuyerBLL.HasPermission(ViewMobile.Order.Core.Enumeration.Permission.AppMovil.Realizar_Login, _Buyer))
                                //{
                                List<Notification> MyNotificationList;
                                if (request.Page == "0")
                                {
                                    MyNotificationList = NotificationBLL.ListByBuyer(_Buyer);
                                }
                                else
                                {
                                    int skip = (Convert.ToInt32(request.Page) - 1) * 10;
                                    MyNotificationList = NotificationBLL.ListByBuyerByPage(_Buyer, skip);
                                }

                                response.MyNotificationList = new ReceiveListMyNotificationResponse.MyNotification[MyNotificationList.Count()];
                                for (int i = 0; i < MyNotificationList.Count(); i++)
                                {
                                    response.MyNotificationList[i] = new ReceiveListMyNotificationResponse.MyNotification();
                                    response.MyNotificationList[i].NotificationID = MyNotificationList[i].NotificationID;
                                    response.MyNotificationList[i].Type = MyNotificationList[i].Type;
                                    response.MyNotificationList[i].Title = MyNotificationList[i].ContentTitle;
                                    response.MyNotificationList[i].Message = MyNotificationList[i].ContentText;
                                    var notificationStatus = MyNotificationList[i].Status;
                                    if (notificationStatus != null)
                                    {
                                        response.MyNotificationList[i].Status = notificationStatus.Value;
                                    }
                                    else
                                    {
                                        response.MyNotificationList[i].Status = 0;
                                    }
                                    response.MyNotificationList[i].IsRead = MyNotificationList[i].IsRead;
                                    response.MyNotificationList[i].Menu = MyNotificationList[i].Menu ?? (int)NotificationEnum.Menu.Notification;
                                    response.MyNotificationList[i].Data = MyNotificationList[i].Data == null ? string.Empty : MyNotificationList[i].Data;
                                    response.MyNotificationList[i].DateCreated = MyNotificationList[i].DateCreated.ToString("dd/MM/yyyy");
                                }

                                response.MyNotificationList = response.MyNotificationList.OrderByDescending(n => n.Data).ToArray();

                                status = DeviceCommunicationLogStatus.Success;

                                //}
                                //else
                                //{
                                //    response.Status = ResponseCodeEnum.Error;
                                //    response.ErrorCode = "E006";
                                //    response.ErrorMessage = Resources.ErrorRes.E006;
                                //}
                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public ReceiveListSegmentResponse ReceiveListCustomer()
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveListSegmentResponse response = new ReceiveListSegmentResponse();

            List<Customer> CustomerList;
            CustomerList = CustomerBLL.GetByName("a").ToList();

            response.CustomerList = new ReceiveListSegmentResponse.Customer[CustomerList.Count()];
            for (int i = 0; i < CustomerList.Count(); i++)
            {
                response.CustomerList[i] = new ReceiveListSegmentResponse.Customer();
                response.CustomerList[i].CustomerID = CustomerList[i].CustomerID;
                response.CustomerList[i].Name = CustomerList[i].Name == null ? "" : CustomerList[i].Name;

                response.CustomerList[i].Bills = new List<ReceiveListSegmentResponse.Bill>();

                for (int j = 0; j < CustomerList[i].Bills.Count(); j++)
                {
                    var item = new ReceiveListSegmentResponse.Bill()
                    {
                        BillID = CustomerList[i].Bills[j].BillID,
                        ProductName = CustomerList[i].Bills[j].ProductName,
                        Value = CustomerList[i].Bills[j].Value,
                        Obs = CustomerList[i].Bills[j].Obs,
                        Paid = (bool) CustomerList[i].Bills[j].Paid
                    };

                    response.CustomerList[i].Bills.Add(item);
                }
            }

            status = DeviceCommunicationLogStatus.Success;
                           
            return response;
        }

        public SendProposalReplyResponse SendProposalReply(string Identification, string SerialKey, string MyOrderID, string Status)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendProposalReplyResponse response = new SendProposalReplyResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null)
                {
                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                //if (BuyerBLL.HasPermission(ViewMobile.Order.Core.Enumeration.Permission.AppMovil.Realizar_Login, _Buyer))
                                //{



                                MyOrder _MyOrder = null;
                                _MyOrder = MyOrderBLL.GetById(Convert.ToInt32(MyOrderID));
                                if (_MyOrder != null)
                                {
                                    MyOrderHistory lastMyOrderHistory = _MyOrder.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault();

                                    MyOrderHistory _MyOrderHistory = new MyOrderHistory();
                                    _MyOrderHistory.MyOrderID = _MyOrder.MyOrderID;
                                    _MyOrderHistory.Observation = null;
                                    _MyOrderHistory.SellerID = lastMyOrderHistory.SellerID;
                                    _MyOrderHistory.Status = Convert.ToInt32(Status);
                                    _MyOrderHistory.Cost = lastMyOrderHistory.Cost;
                                    _MyOrderHistory.DateCreated = DateTime.Now;
                                    _MyOrderHistory.Active = true;

                                    new MyOrderHistoryBLL().Save(_MyOrderHistory);

                                    response.SerialKey = _Buyer.SerialKey;
                                    status = DeviceCommunicationLogStatus.Success;

                                }
                                else
                                {
                                    response.Status = ResponseCodeEnum.Error;
                                    response.ErrorCode = "E015";
                                    response.ErrorMessage = Resources.ErrorRes.E015;
                                }
  
                                //}
                                //else
                                //{
                                //    response.Status = ResponseCodeEnum.Error;
                                //    response.ErrorCode = "E006";
                                //    response.ErrorMessage = Resources.ErrorRes.E006;
                                //}
                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public ReceiveListPromotionResponse ReceiveListPromotion(string Identification, string SerialKey, string Page)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveListPromotionResponse response = new ReceiveListPromotionResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                List<Promotion> PromotionList;
                                if (Page == "0")
                                {
                                    PromotionList = PromotionBLL.ListByBuyer(_Buyer);
                                }
                                else
                                {
                                    int skip = (Convert.ToInt32(Page) - 1) * 10;
                                    PromotionList = PromotionBLL.ListByBuyerByPage(_Buyer, skip);
                                }

                                response.PromotionList = new ReceiveListPromotionResponse.Promotion[PromotionList.Count()];
                                for (int i = 0; i < PromotionList.Count(); i++)
                                {
                                    response.PromotionList[i] = new ReceiveListPromotionResponse.Promotion();
                                    response.PromotionList[i].PromotionID = PromotionList[i].PromotionID;
                                    response.PromotionList[i].SupplierName = PromotionList[i].Seller.Employee.Supplier.Name;
                                    response.PromotionList[i].Type = PromotionList[i].Type.Value;
                                    response.PromotionList[i].Product = PromotionList[i].Product;
                                    response.PromotionList[i].DateCreated = PromotionList[i].DateCreated.Value;

                                }

                                status = DeviceCommunicationLogStatus.Success;

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public ReceiveListPromotionDetailResponse ReceiveListPromotionDetail(string Identification, string SerialKey, string PromotionID)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveListPromotionDetailResponse response = new ReceiveListPromotionDetailResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                //if (BuyerBLL.HasPermission(ViewMobile.Order.Core.Enumeration.Permission.AppMovil.Realizar_Login, _Buyer))
                                //{

                                Promotion _Promotion;
                                _Promotion = PromotionBLL.GetFullById(Convert.ToInt32(PromotionID));
                                if (_Promotion != null)
                                {
                                    response.PromotionID = _Promotion.PromotionID;
                                    response.SupplierName = _Promotion.Seller.Employee.Supplier.Name;
                                    response.SellerName = _Promotion.Seller.Employee.FirstName + " " + _Promotion.Seller.Employee.LastName ?? "";
                                    response.Type = _Promotion.Type.Value;
                                    response.Product = _Promotion.Product;
                                    response.Description = _Promotion.Description;
                                    response.Cost = _Promotion.Cost.Value;
                                    response.ImageUrl = _Promotion.ImageURL;
                                    response.ExpirationDate = _Promotion.ExpirationDate.Value;
                                    response.DateCreated = _Promotion.DateCreated.Value;

                                    status = DeviceCommunicationLogStatus.Success;
                                }

                                //}
                                //else
                                //{
                                //    response.Status = ResponseCodeEnum.Error;
                                //    response.ErrorCode = "E006";
                                //    response.ErrorMessage = Resources.ErrorRes.E006;
                                //}
                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public SendPromotionAnswerResponse SendPromotionAnswer(string Identification, string SerialKey, string PromotionID, string Answer)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendPromotionAnswerResponse response = new SendPromotionAnswerResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {
                                    
                                PromotionReading promotionReadingNew = new PromotionReading();    
                                promotionReadingNew.BuyerID = _Buyer.BuyerID;
                                promotionReadingNew.PromotionID = Convert.ToInt32(PromotionID);
                                promotionReadingNew.Status = Convert.ToInt32(Answer);
                                promotionReadingNew.DateCreated = DateTime.Now;
                                promotionReadingNew.Active = true;

                                if (Answer == "1")
                                {
                                    Promotion promotion = PromotionBLL.GetFullById(Convert.ToInt32(PromotionID));

                                    Security security = new Security();
                                    Random random = new Random();
                                    string ReceiptCode = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(0, 1000).ToString();

                                    MyOrder _MyOrder = new MyOrder();
                                    _MyOrder.BuyerID = _Buyer.BuyerID;
                                    _MyOrder.SupplierID = promotion.Seller.Employee.SupplierID;
                                    _MyOrder.PromotionID = Convert.ToInt32(PromotionID);
                                    _MyOrder.MyOrderType = (int)MyOrderEnum.Type.Promotion;
                                    _MyOrder.ReceiptCode = ReceiptCode;
                                    _MyOrder.Description = promotion.Product;
                                    _MyOrder.RepeatOrder = false;
                                    _MyOrder.DateCreated = DateTime.Now;
                                    _MyOrder.Active = true;

                                    MyOrderHistory _MyOrderHistory = new MyOrderHistory();
                                    _MyOrderHistory.SellerID = promotion.SellerID;
                                    _MyOrderHistory.Status = (int)MyOrderHistoryEnum.Status.AwaitingAttention;
                                    _MyOrderHistory.Cost = promotion.Cost;
                                    _MyOrderHistory.DateCreated = DateTime.Now;
                                    _MyOrderHistory.Active = true;

                                    _MyOrder.MyOrderHistories = new EntitySet<MyOrderHistory>();
                                    _MyOrder.MyOrderHistories.Add(_MyOrderHistory);

                                    _MyOrder = new MyOrderBLL().Save(_MyOrder);

                                }

                                new PromotionReadingBLL().Save(promotionReadingNew);

                                response.SerialKey = _Buyer.SerialKey;
                                status = DeviceCommunicationLogStatus.Success;

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public SendNotificationReadResponse SendNotificationRead(string Identification, string SerialKey, string NotificationID)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendNotificationReadResponse response = new SendNotificationReadResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                Notification notification = NotificationBLL.GetById(Convert.ToInt32(NotificationID));
                                if (notification != null)
                                {
                                    notification.IsRead = true;
                                    new NotificationBLL().Update(notification);
                                }
                                response.SerialKey = _Buyer.SerialKey;
                                status = DeviceCommunicationLogStatus.Success;
                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public SendUpdateBuyerDataResponse SendUpdateBuyerData(SendUpdateBuyerDataRequest request)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendUpdateBuyerDataResponse response = new SendUpdateBuyerDataResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(request.UUID);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(request.SerialKey);
                Seller _Seller = null;
                _Seller = SellerBLL.GetBySerialKey(request.SerialKey);
                if (_Buyer != null || _Seller != null)
                {

                    BuyerDevice _BuyerDevice = null;
                    EmployeeDevice _EmployeeDevice = null;
                    if (_Buyer != null)
                        _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_Seller != null)
                        _EmployeeDevice = EmployeeDeviceBLL.GetByEmployeeDevice(_Seller.Employee, device);

                    if (_BuyerDevice != null || _EmployeeDevice != null)
                    {

                        if ((_Buyer != null && _Buyer.Active.Value) || (_Seller != null && _Seller.Active))
                        {

                            if ((_Buyer != null && !_Buyer.IsBlocked.Value) || _Seller != null)
                            {

                                if(request.BuyerId > 0 && _Seller != null)
                                {
                                    _Buyer = BuyerBLL.GetById(request.BuyerId);
                                }

                                Buyer buyerRepeat = BuyerBLL.HasEmail(request.Email);
                                if(buyerRepeat == null || buyerRepeat.BuyerID == _Buyer.BuyerID)
                                {
                                    string[] names = request.BuyerName.Split(' ');
                                    string firstName = string.Empty;
                                    string lastName = string.Empty;

                                    if (names.Count() == 1)
                                    {
                                        firstName = names[0];
                                    }
                                    else if (names.Count() == 2)
                                    {
                                        firstName = names[0];
                                        lastName = names.Last();
                                    }
                                    else if (names.Count() == 3)
                                    {
                                        firstName = names[0];
                                        lastName = names[1] + " " + names[2];
                                    }
                                    else if (names.Count() > 3)
                                    {
                                        firstName = names[0] + " " + names[1];
                                        lastName = names[2] + " " + names[3];
                                    }

                                    _Buyer.FirstName = firstName;
                                    _Buyer.LastName = lastName == string.Empty ? _Buyer.LastName : lastName;
                                    _Buyer.Email = request.Email;
                                    _Buyer.PhoneNumber = request.PhoneNumber;

                                    Customer customer = CustomerBLL.GetById(_Buyer.CustomerID.Value);
                                    customer.Name = request.BuyerName;
                                    customer.CNPJ = request.ContributorNumber;
                                    customer.PhoneNumber = request.PhoneNumber;

                                    if (!string.IsNullOrWhiteSpace(request.ContributorName))
                                        customer.BusinessName = request.ContributorName;

                                    if(request.CountryId != 0)
                                        customer.CountryID = request.CountryId;

                                    new BuyerBLL().Update(_Buyer);
                                    new CustomerBLL().Update(customer);

                                    response.SerialKey = _Buyer.SerialKey;
                                    status = DeviceCommunicationLogStatus.Success;
                                }
                                else
                                {
                                    response.Status = ResponseCodeEnum.Error;
                                    response.ErrorCode = "E016";
                                    response.ErrorMessage = Resources.ErrorRes.E016;
                                }
                                

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            return response;
        }

        public SendChangePasswordResponse SendChangePassword(SendChangePasswordRequest request)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendChangePasswordResponse response = new SendChangePasswordResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(request.Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(request.SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {
                                Security security = new Security();
                                request.OldPassword = security.MD5(request.OldPassword);

                                Buyer buyerVerification = new BuyerBLL().Authentication(_Buyer.Email, request.OldPassword);
                                if (buyerVerification != null)
                                {
                                    request.NewPassword = security.MD5(request.NewPassword);
                                    _Buyer.Password = request.NewPassword;

                                    new BuyerBLL().Update(_Buyer);

                                    response.SerialKey = _Buyer.SerialKey;
                                    status = DeviceCommunicationLogStatus.Success;
                                }
                                else
                                {
                                    response.Status = ResponseCodeEnum.Error;
                                    response.ErrorCode = "E017";
                                    response.ErrorMessage = Resources.ErrorRes.E017;
                                }

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public SendForgotMyPasswordResponse SendForgotMyPassword(string Email)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendForgotMyPasswordResponse response = new SendForgotMyPasswordResponse();

            Buyer _Buyer = BuyerBLL.HasEmail(Email);
            if (_Buyer != null)
            {
                #region Encriptacion del password
                Security security = new Security();
                string passwordUnencrypted = Protocol.Generator(6);
                string Password = security.MD5(passwordUnencrypted);
                #endregion

                _Buyer.Password = Password;
                
                EmailNotification emailNotification = new EmailNotification()
                {
                    Active = true,
                    BodyText = "Cangorua - Nova senha\n\nPara entrar você deve usar a seguinte senha temporária: " + passwordUnencrypted + " . Lembre-se de redefinir sua senha. Vá ao menu lateral -->Trocar Senha",
                    DateCreated = DateTime.Now,
                    DateToSend = DateTime.Now,
                    EmailFrom = ConfigurationManager.AppSettings["SoporteEmailAccount"].ToString(),
                    EmailTo = _Buyer.Email,
                    Send = false,
                    Subject = "Cangorua - nova senha"
                };

                new BuyerBLL().Update(_Buyer);
                new EmailNotificationBLL().Save(emailNotification);

                response.SerialKey = _Buyer.SerialKey;
                status = DeviceCommunicationLogStatus.Success;
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E018";
                response.ErrorMessage = Resources.ErrorRes.E018;
            }
                          

            response.Apple();
            return response;
        }

        public ReceiveBuyerDataResponse ReceiveBuyerData(string Identification, string SerialKey)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveBuyerDataResponse response = new ReceiveBuyerDataResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {
                                Customer customer = CustomerBLL.GetById(_Buyer.CustomerID.Value);

                                response.BuyerName = (_Buyer.FirstName + " " + (_Buyer.LastName ?? "")).Trim();
                                response.PhoneNumber = _Buyer.PhoneNumber;
                                response.Email = _Buyer.Email;
                                response.Company = customer.Name;
                                response.CNPJ = customer.CNPJ;
                                response.CountryId = customer.CountryID ?? 193; //Si es nulo = Paraguay
                               

                                status = DeviceCommunicationLogStatus.Success;

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public ReceiveListProductResponse ReceiveListProduct(string Identification, string SerialKey, string CategoryID, string Page)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveListProductResponse response = new ReceiveListProductResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                Seller _Seller = null;
                _Seller = SellerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null || _Seller != null)
                {

                    BuyerDevice _BuyerDevice = null;
                    EmployeeDevice _EmployeeDevice = null;
                    if (_Buyer != null)
                        _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_Seller != null)
                        _EmployeeDevice = EmployeeDeviceBLL.GetByEmployeeDevice(_Seller.Employee, device);

                    if (_BuyerDevice != null || _EmployeeDevice != null)
                    {

                        if ((_Buyer != null && _Buyer.Active.Value) || (_Seller != null && _Seller.Active))
                        {

                            if ((_Buyer != null && !_Buyer.IsBlocked.Value) || _Seller != null)
                            {
                                List<Product> ProductList;
                                if (Page == "0")
                                {
                                    ProductList = ProductBLL.ListByCategoryID(Convert.ToInt32(CategoryID));
                                }
                                else
                                {
                                    int skip = (Convert.ToInt32(Page) - 1) * 10;
                                    ProductList = ProductBLL.ListByCategoryIDByPage(Convert.ToInt32(CategoryID), skip);
                                }

                                response.ProductList = new ReceiveListProductResponse.Product[ProductList.Count()];
                                for (int i = 0; i < ProductList.Count(); i++)
                                {
                                    response.ProductList[i] = new ReceiveListProductResponse.Product();
                                    response.ProductList[i].ProductId = ProductList[i].ProductID;
                                    response.ProductList[i].ProductName = ProductList[i].Name;
                                    response.ProductList[i].Price = ProductList[i].CostShow == true ? ProductList[i].Cost : 0;
                                    response.ProductList[i].ImageId = ProductList[i].AppImageID ?? 0;
                                    response.ProductList[i].ThumbImageId = ProductList[i].AppImageID ?? 0;
                                    response.ProductList[i].DateCreated = ProductList[i].DateCreated.Value;
                                    response.ProductList[i].Manufacturer = ProductList[i].Manufacturer;
                                    response.ProductList[i].UnitMeasure = ProductList[i].Udm;
                                }

                                status = DeviceCommunicationLogStatus.Success;

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            return response;
        }

        public ReceiveProductListResponse ReceiveProductList(ReceiveProductListRequest request)
        {

            ReceiveProductListResponse response = new ReceiveProductListResponse();

            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);


            if (buyerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk || sellerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk)
            {
                List<Product> productList;

                if (request.CustomerTypeId == 0)
                {
                    request.CustomerTypeId = buyerUserValidation.buyer.Customer.CustomerTypeID.Value;
                }

                if(request.Offer)
                {
                    productList = ProductBLL.ListInOffer(request.SupplierID, request.CustomerTypeId);
                }
                else if (request.Page == 0)
                {
                    if (request.IsNew)
                        productList = ProductBLL.ListByCategoryTempID(request.CategoryId, request.CustomerTypeId);
                    else
                        //productList = ProductBLL.ListByCategoryID(request.CategoryId);
                        productList = ProductBLL.ListBySupplierID(request.SupplierID);  
                }
                else
                {
                    int skip = (request.Page - 1) * 10;
                    //productList = ProductBLL.ListByCategoryIDByPage(request.CategoryId, skip);
                    productList = ProductBLL.ListBySupplierIDByPage(request.SupplierID, skip);
                    
                }

                response.ProductList = (from p in productList
                                        let priceListDetail = p.PriceListDetails.FirstOrDefault(pl => pl.PriceList.CustomerTypeID == request.CustomerTypeId)
                                        select new ReceiveProductListResponse.Product()
                                        {
                                            CategoryName = p.Category.Name,
                                            ProductId = p.ProductID,
                                            ProductName = p.Name,
                                            Price = priceListDetail != null ?
                                                    ((priceListDetail.OfferPrice ?? 0) != 0 ?
                                                    priceListDetail.OfferPrice.Value :
                                                    (priceListDetail.MarginDiscount == 0 ?
                                                    priceListDetail.SalePrice :
                                                    priceListDetail.DiscountPrice.Value)
                                                    ) : p.Cost,
                                            ImageId = p.AppImageID ?? 0,
                                            ThumbImageId = p.ThumbAppImageID ?? 0,
                                            DateCreated = p.DateCreated.Value,
                                            Manufacturer = p.Manufacturer,
                                            UnitMeasure = p.Udm,
                                            PriceListId = (priceListDetail != null ? priceListDetail.PriceListID : 0),
                                            OldPrice = (priceListDetail == null ? 0 :
                                                        ((priceListDetail.OfferPrice ?? 0) == 0 ? 0 :
                                                        (priceListDetail.MarginDiscount != 0 ? priceListDetail.DiscountPrice.Value : priceListDetail.SalePrice))),
                                            MinimumQuantity = (priceListDetail != null ? (priceListDetail.MinimumQuantity ?? 1) : 1),
                                            MaximumDiscount = (priceListDetail != null ? (priceListDetail.MaximumDiscount ?? 0) : 0)

                                        })
                                        .ToList();

            }
            else
            {
                ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                response.ErrorCode = error.ErrorCode;
                response.ErrorMessage = error.ErrorMessage;
            }

            return response;
        }

        public ReceiveProductListCatalogResponse ReceiveProductListCatalog(ReceiveProductListCatalogRequest request)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveProductListCatalogResponse response = new ReceiveProductListCatalogResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(request.Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(request.SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {
                                List<Supplier> SupplierList;

                                SupplierList = SupplierBLL.ListByBuyer(_Buyer);

                                List<Employee> SellerList;

                                response.SupplierList = new ReceiveProductListCatalogResponse.Supplier[SupplierList.Count()];
                                for (int i = 0; i < SupplierList.Count(); i++)
                                {
                                    response.SupplierList[i] = new ReceiveProductListCatalogResponse.Supplier();
                                    response.SupplierList[i].SupplierID = SupplierList[i].SupplierID;
                                    response.SupplierList[i].Name = SupplierList[i].Name;
                                    response.SupplierList[i].CNPJ = SupplierList[i].CNPJ == null ? "" : SupplierList[i].CNPJ;
                                    response.SupplierList[i].PhoneNumber = SupplierList[i].PhoneNumber == null ? "" : SupplierList[i].PhoneNumber;

                                    SellerList = SupplierList[i].Employees.Where(s => s.Seller != null && s.Seller.BuyerSellers.Any(bs => bs.BuyerID == _Buyer.BuyerID)).ToList();
                                    response.SupplierList[i].SellerList = new ReceiveProductListCatalogResponse.Seller[SellerList.Count()];
                                    for (int j = 0; j < SellerList.Count(); j++)
                                    {
                                        response.SupplierList[i].SellerList[j] = new ReceiveProductListCatalogResponse.Seller();
                                        response.SupplierList[i].SellerList[j].SellerID = SellerList[j].Seller.SellerID;
                                        response.SupplierList[i].SellerList[j].Name = SellerList[j].FirstName + " " + SellerList[j].LastName;
                                        response.SupplierList[i].SellerList[j].SupplierID = SupplierList[i].SupplierID;
                                    }
                                }


                                var customerTypeID = _Buyer.Customer.CustomerTypeID.Value;

                                for (int i = 0; i < SupplierList.Count(); i++)
                                {
                                    List<Product> productList;
                                    List<Product> productListOffer;
                                    List<Product> productListTopN;

                                    productListOffer = ProductBLL.ListInOffer(SupplierList[i].SupplierID, customerTypeID);

                                    productList = ProductBLL.ListBySupplierID(SupplierList[i].SupplierID);

                                    productListTopN = productList.OrderByDescending(p => p.DateCreated).Take(10).ToList();

                                    response.SupplierList.FirstOrDefault(x => x.SupplierID == SupplierList[i].SupplierID).ProductList = 
                                        (from p in productList
                                         let priceListDetail = p.PriceListDetails.FirstOrDefault(pl => pl.PriceList.CustomerTypeID == customerTypeID)
                                         select new ReceiveProductListCatalogResponse.Product()
                                         {
                                             CategoryName = p.Category.Name,
                                             ProductId = p.ProductID,
                                             ProductName = p.Name,
                                             Price = priceListDetail != null ?
                                                     ((priceListDetail.OfferPrice ?? 0) != 0 ?
                                                     priceListDetail.OfferPrice.Value :
                                                     (priceListDetail.MarginDiscount == 0 ?
                                                     priceListDetail.SalePrice :
                                                     priceListDetail.DiscountPrice.Value)
                                                     ) : p.Cost,
                                             PriceStr = (priceListDetail != null ?
                                                 ((priceListDetail.OfferPrice ?? 0) != 0 ?
                                                     priceListDetail.OfferPrice.Value :
                                                     (priceListDetail.MarginDiscount == 0 ?
                                                         priceListDetail.SalePrice :
                                                         priceListDetail.DiscountPrice.Value)
                                                 ) : p.Cost).ToString("#0.00"),
                                             ImageId = p.AppImageID ?? 0,
                                             ThumbImageId = p.ThumbAppImageID ?? 0,
                                             DateCreated = p.DateCreated.Value,
                                             DateCreatedString = p.DateCreated.Value.ToString("dd/MM/yyyy"),
                                             Manufacturer = p.Manufacturer,
                                             UnitMeasure = p.Udm == null ? string.Empty : p.Udm,
                                             PriceListId = (priceListDetail != null ? priceListDetail.PriceListID : 0),
                                             OldPrice = productListOffer.Any(po => po.ProductID == p.ProductID) ? p.Cost * Convert.ToDecimal(0.9) : p.Cost,
                                             OldPriceStr = (productListOffer.Any(po => po.ProductID == p.ProductID) ? p.Cost * Convert.ToDecimal(0.9) : p.Cost).ToString("#0.00"),
                                             //OldPrice = (priceListDetail == null ? 0 :
                                             //            ((priceListDetail.OfferPrice ?? 0) == 0 ? 0 :
                                             //            (priceListDetail.MarginDiscount != 0 ? priceListDetail.DiscountPrice.Value : priceListDetail.SalePrice))),
                                             MinimumQuantity = (priceListDetail != null ? (priceListDetail.MinimumQuantity ?? 1) : 1),
                                             MaximumDiscount = (priceListDetail != null ? (priceListDetail.MaximumDiscount ?? 0) : 0),
                                             IsOffer = productListOffer.Any(po => po.ProductID == p.ProductID),
                                             IsNew = productListTopN.Any(pn => pn.ProductID == p.ProductID),
                                             SupplierID = SupplierList[i].SupplierID
                                         }).ToList();
                                }

                                status = DeviceCommunicationLogStatus.Success;
                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            return response;

        }

        public ReceiveListCategoryResponse ReceiveListCategory(string Identification, string SerialKey, string SupplierID)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveListCategoryResponse response = new ReceiveListCategoryResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {
                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                Seller _Seller = null;
                _Seller = SellerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null || _Seller != null)
                {
                    BuyerDevice _BuyerDevice = null;
                    EmployeeDevice _EmployeeDevice = null;
                    if (_Buyer != null)
                        _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_Seller != null)
                        _EmployeeDevice = EmployeeDeviceBLL.GetByEmployeeDevice(_Seller.Employee, device);

                    if (_BuyerDevice != null || _EmployeeDevice != null)
                    {

                        if ((_Buyer != null && _Buyer.Active.Value) || (_Seller != null && _Seller.Active))
                        {

                            if ((_Buyer != null && !_Buyer.IsBlocked.Value) || _Seller != null)
                            {

                                List<Category> CategoryList = CategoryBLL.ListBySupplierID(Convert.ToInt32(SupplierID)).Where(ct=>ct.CategoryType == (int)CategoryEnum.CategoryType.Distributor).ToList();
                                response.CategoryList = new ReceiveListCategoryResponse.Category[CategoryList.Count()];
                                for (int i = 0; i < CategoryList.Count(); i++)
                                {
                                    response.CategoryList[i] = new ReceiveListCategoryResponse.Category();
                                    response.CategoryList[i].CategoryID = CategoryList[i].CategoryID;
                                    response.CategoryList[i].CategoryName = CategoryList[i].Name;
                                    response.CategoryList[i].DateCreated = CategoryList[i].DateCreated.Value;
                                }

                                status = DeviceCommunicationLogStatus.Success;

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        //public SendNewMyOrderResponse SendMyOrder(string Identification, string SerialKey, string SupplierID, string SellerID)
        //{
        //    DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
        //    SendNewMyOrderResponse response = new SendNewMyOrderResponse();

        //    Device device = new Device();
        //    device = DeviceBLL.GetByDeviceToken(Identification);
        //    if (device != null)
        //    {

        //        Buyer _Buyer = null;
        //        _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
        //        if (_Buyer != null)
        //        {

        //            BuyerDevice _BuyerDevice;
        //            _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
        //            if (_BuyerDevice != null)
        //            {

        //                if (_Buyer.Active.Value)
        //                {

        //                    if (!_Buyer.IsBlocked.Value)
        //                    {

        //                        Security security = new Security();
        //                        string ReceiptCode = security.MD5(SupplierID + DateTime.Now.Ticks.ToString());

        //                        MyOrder _MyOrder = new MyOrder();
        //                        _MyOrder.BuyerID = _Buyer.BuyerID;
        //                        _MyOrder.SupplierID = Convert.ToInt32(SupplierID);
        //                        _MyOrder.MyOrderType = (int)MyOrderEnum.Type.LastOrder;
        //                        _MyOrder.ReceiptCode = ReceiptCode;
        //                        _MyOrder.DateCreated = DateTime.Now;
        //                        _MyOrder.Active = true;

        //                        MyOrderHistory _MyOrderHistory = new MyOrderHistory();
        //                        _MyOrderHistory.SellerID = SellerID != "0" ? Convert.ToInt32(SellerID) : _MyOrderHistory.SellerID;
        //                        _MyOrderHistory.Status = (int)MyOrderHistoryEnum.Status.AwaitingAttention;
        //                        _MyOrderHistory.DateCreated = DateTime.Now;
        //                        _MyOrderHistory.Active = true;

        //                        _MyOrder.MyOrderHistories = new EntitySet<MyOrderHistory>();
        //                        _MyOrder.MyOrderHistories.Add(_MyOrderHistory);

        //                        _MyOrder = new MyOrderBLL().Save(_MyOrder);

        //                        response.MyOrderID = _MyOrder.MyOrderID;
        //                        response.ReceiptCode = _MyOrder.ReceiptCode;
        //                        status = DeviceCommunicationLogStatus.Success;

        //                    }
        //                    else
        //                    {
        //                        response.Status = ResponseCodeEnum.Error;
        //                        response.ErrorCode = "E005";
        //                        response.ErrorMessage = Resources.ErrorRes.E005;
        //                    }
        //                }
        //                else
        //                {
        //                    response.Status = ResponseCodeEnum.Error;
        //                    response.ErrorCode = "E004";
        //                    response.ErrorMessage = Resources.ErrorRes.E004;
        //                }

        //            }
        //            else
        //            {
        //                response.Status = ResponseCodeEnum.Error;
        //                response.ErrorCode = "E003";
        //                response.ErrorMessage = Resources.ErrorRes.E003;
        //                status = DeviceCommunicationLogStatus.KnownError;
        //            }
        //        }
        //        else
        //        {
        //            response.Status = ResponseCodeEnum.Error;
        //            response.ErrorCode = "E007";
        //            response.ErrorMessage = Resources.ErrorRes.E007;
        //            status = DeviceCommunicationLogStatus.KnownError;
        //        }
        //    }
        //    else
        //    {
        //        response.Status = ResponseCodeEnum.Error;
        //        response.ErrorCode = "E002";
        //        response.ErrorMessage = Resources.ErrorRes.E002;
        //    }

        //    response.Apple();
        //    return response;
        //}

        //public SendNewMyOrderProductResponse SendMyOrderProduct(string Identification, string SerialKey, string MyOrderID, string ProductID, string Quantity, string Amount)
        //{
        //    DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
        //    SendNewMyOrderProductResponse response = new SendNewMyOrderProductResponse();

        //    Device device = new Device();
        //    device = DeviceBLL.GetByDeviceToken(Identification);
        //    if (device != null)
        //    {

        //        Buyer _Buyer = null;
        //        _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
        //        if (_Buyer != null)
        //        {

        //            BuyerDevice _BuyerDevice;
        //            _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
        //            if (_BuyerDevice != null)
        //            {

        //                if (_Buyer.Active.Value)
        //                {

        //                    if (!_Buyer.IsBlocked.Value)
        //                    {
        //                        MyOrderProduct newMyOrderProduct = new MyOrderProduct();
        //                        newMyOrderProduct.MyOrderID = Convert.ToInt32(MyOrderID);
        //                        newMyOrderProduct.ProductID = Convert.ToInt32(ProductID);
        //                        newMyOrderProduct.Quantity = Convert.ToInt32(Quantity);
        //                        newMyOrderProduct.Discount = 0;
        //                        newMyOrderProduct.DateCreated = DateTime.Now;
        //                        newMyOrderProduct.Active = true;

        //                        new MyOrderProductBLL().Save(newMyOrderProduct);

        //                        response.SerialKey = _Buyer.SerialKey;
        //                        status = DeviceCommunicationLogStatus.Success;
        //                    }
        //                    else
        //                    {
        //                        response.Status = ResponseCodeEnum.Error;
        //                        response.ErrorCode = "E005";
        //                        response.ErrorMessage = Resources.ErrorRes.E005;
        //                    }
        //                }
        //                else
        //                {
        //                    response.Status = ResponseCodeEnum.Error;
        //                    response.ErrorCode = "E004";
        //                    response.ErrorMessage = Resources.ErrorRes.E004;
        //                }

        //            }
        //            else
        //            {
        //                response.Status = ResponseCodeEnum.Error;
        //                response.ErrorCode = "E003";
        //                response.ErrorMessage = Resources.ErrorRes.E003;
        //                status = DeviceCommunicationLogStatus.KnownError;
        //            }
        //        }
        //        else
        //        {
        //            response.Status = ResponseCodeEnum.Error;
        //            response.ErrorCode = "E007";
        //            response.ErrorMessage = Resources.ErrorRes.E007;
        //            status = DeviceCommunicationLogStatus.KnownError;
        //        }
        //    }
        //    else
        //    {
        //        response.Status = ResponseCodeEnum.Error;
        //        response.ErrorCode = "E002";
        //        response.ErrorMessage = Resources.ErrorRes.E002;
        //    }

        //    response.Apple();
        //    return response;
        //}

        public SendNewMyOrderResponse SendNewMyOrder(SendNewMyOrderRequest request)
        {
            //string Identification, string SerialKey, string MyOrderXml

            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendNewMyOrderResponse response = new SendNewMyOrderResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(request.Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(request.SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                //MyOrderXml = MyOrderXml.Replace('Y', '<').Replace('W', '>').Replace('Z', '/');

                                //Security security = new Security();

                                //XmlDocument xmlDoc = new XmlDocument();
                                //xmlDoc.LoadXml(request.MyOrderXml);


                                //string xpath = "NewOrderList/NewOrder";
                                //var newOrderNodesList = xmlDoc.SelectNodes(xpath);
                                var newOrderNodesList = request.NewOrderList;

                                decimal cost;
                                List<MyOrder> myOrderList = new List<MyOrder>();
                                MyOrder _MyOrder;
                                MyOrderProduct newMyOrderProduct;
                                foreach (SendNewMyOrderRequest.NewOrder orderNode in newOrderNodesList)
                                {
                                    int supplierID = Convert.ToInt32(orderNode.SupplierID);
                                    int sellerID = Convert.ToInt32(orderNode.SellerID);
                                    Seller seller = SellerBLL.GetById(sellerID);
                                    Random random = new Random();
                                    string ReceiptCode = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(0, 1000).ToString();

                                    _MyOrder = new MyOrder();
                                    _MyOrder.BuyerID = _Buyer.BuyerID;
                                    _MyOrder.SupplierID = seller.Employee.SupplierID;
                                    _MyOrder.MyOrderType = (int)MyOrderEnum.Type.LastOrder;
                                    _MyOrder.ReceiptCode = ReceiptCode;
                                    _MyOrder.DateCreated = DateTime.Now;
                                    _MyOrder.Active = true;
                                    _MyOrder.MyOrderHistories = new EntitySet<MyOrderHistory>();
                                    _MyOrder.MyOrderProducts = new EntitySet<MyOrderProduct>();

                                    cost = 0;
                                    var productNodesList = orderNode.ProductList;
                                    foreach(SendNewMyOrderRequest.Product productNode in productNodesList)
                                    {
                                        newMyOrderProduct = new MyOrderProduct();
                                        newMyOrderProduct.ProductID = Convert.ToInt32(productNode.ProductID);
                                        newMyOrderProduct.Quantity = Convert.ToInt32(productNode.Quantity);
                                        newMyOrderProduct.Discount = 0;
                                        newMyOrderProduct.QuantityInitial = newMyOrderProduct.Quantity;
                                        newMyOrderProduct.DiscountInitial = 0;
                                        newMyOrderProduct.QuantityInitial = newMyOrderProduct.Quantity;
                                        newMyOrderProduct.SalePrice = productNode.Amount;
                                        newMyOrderProduct.DateCreated = DateTime.Now;
                                        newMyOrderProduct.Active = true;
                                        
                                        _MyOrder.MyOrderProducts.Add(newMyOrderProduct);

                                        Product product = ProductBLL.GetById(newMyOrderProduct.ProductID);
                                        cost += product.Cost;
 
                                    }                                    

                                    MyOrderHistory _MyOrderHistory = new MyOrderHistory();
                                    _MyOrderHistory.SellerID = sellerID != 0 ? Convert.ToInt32(sellerID) : _MyOrderHistory.SellerID;
                                    _MyOrderHistory.Status = (int)MyOrderHistoryEnum.Status.AwaitingAttention;
                                    _MyOrderHistory.Cost = cost;
                                    _MyOrderHistory.DateCreated = DateTime.Now;
                                    _MyOrderHistory.Active = true;

                                    _MyOrder.MyOrderHistories.Add(_MyOrderHistory);

                                    myOrderList.Add(_MyOrder);

                                }

                                new MyOrderBLL().BulkSave(myOrderList);

                                response.NewOrderList = new SendNewMyOrderResponse.NewOrder[myOrderList.Count()];

                                for (int i = 0; i < myOrderList.Count(); i++)
                                {
                                    response.NewOrderList[i] = new SendNewMyOrderResponse.NewOrder();
                                    response.NewOrderList[i].MyOrderID = myOrderList[i].MyOrderID;
                                    response.NewOrderList[i].ReceiptCode = myOrderList[i].ReceiptCode;
                                }

                                #region Push Notifications
                                List<Notification> notificationList = new List<Notification>();

                                string[] message = Resources.NotificationPush.N001.Split('#');
                                notificationList.Add(new Notification()
                                {
                                    Active = true,
                                    Activity = 0,
                                    AppID = 1,
                                    BuyerID = _Buyer.BuyerID,
                                    Channel = device.PushToken,
                                    ContentText = message[1],
                                    ContentTitle = message[0],
                                    DateCreated = DateTime.Now,
                                    DateToSend = DateTime.Now,
                                    DeviceOS = "1",
                                    IsRead = true,
                                    Send = false,
                                    Type = 1
                                });

                                new NotificationBLL().BulkSave(notificationList);
                                #endregion

                                status = DeviceCommunicationLogStatus.Success;
                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            return response;
        }

        public ReceiveListMyBuyerResponse ReceiveListMyBuyer(string Identification, string SerialKey)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveListMyBuyerResponse response = new ReceiveListMyBuyerResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                List<Buyer> BuyerList = BuyerBLL.GetByCustomer(_Buyer.CustomerID.Value).Where(b=>b.BuyerID != _Buyer.BuyerID).ToList();
                                response.BuyerList = new ReceiveListMyBuyerResponse.Buyer[BuyerList.Count()];
                                for (int i = 0; i < BuyerList.Count(); i++)
                                {
                                    response.BuyerList[i] = new ReceiveListMyBuyerResponse.Buyer();
                                    response.BuyerList[i].BuyerID = BuyerList[i].BuyerID;
                                    response.BuyerList[i].BuyerName = BuyerList[i].FirstName + " " + BuyerList[i].LastName ?? "";
                                }

                                status = DeviceCommunicationLogStatus.Success;

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public SendNewBuyerResponse SendNewBuyer(string Identification, string SerialKey, string BuyerName, string PhoneNumber, string Email, string Password, string Admin)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendNewBuyerResponse response = new SendNewBuyerResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                Buyer newBuyer = null;
                                newBuyer = BuyerBLL.HasEmail(Email);
                                if (newBuyer == null)
                                {

                                    #region Generacion de SerialKey y Encriptacion del password
                                    /// <summary>
                                    /// Array de bytes utilizado para gerar o SerialKey
                                    /// </summary>
                                    byte[] _MyCompanyKey = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
                                                                11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 
                                                                    21, 22, 23, 24 };

                                    /// <summary>
                                    /// Array de bytes utilizado para gerar o SerialKey
                                    /// </summary>
                                    byte[] _MyCompanyIV = { 1, 2, 3, 4, 5, 6, 7, 8 };

                                    KeySeller locKeyHandler = new KeySeller(_MyCompanyKey, _MyCompanyIV);

                                    string keyvalue = locKeyHandler.SellKey(Password, Email, Password, Key.ProductOrProductFeatureSet.Product2_PaidFullVersion);

                                    Security security = new Security();
                                    Password = security.MD5(Password);
                                    #endregion

                                    newBuyer = new Buyer();
                                    newBuyer.CustomerID = _Buyer.CustomerID;
                                    newBuyer.FirstName = BuyerName.ToString().Trim();
                                    newBuyer.Email = Email;
                                    newBuyer.Password = Password;
                                    newBuyer.PhoneNumber = PhoneNumber;
                                    newBuyer.SerialKey = keyvalue;
                                    newBuyer.IsBlocked = false;
                                    newBuyer.Admin = Admin == "0" ? false : true;
                                    newBuyer.DateCreated = DateTime.Now;
                                    newBuyer.Active = true;

                                    newBuyer = new BuyerBLL().Save(newBuyer);

                                    response.BuyerID = newBuyer.BuyerID;
                                    status = DeviceCommunicationLogStatus.Success;
                                }
                                else
                                {
                                    // The Device could not be validated.
                                    response.Status = ResponseCodeEnum.Error;
                                    response.ErrorCode = "E008";
                                    response.ErrorMessage = Resources.ErrorRes.E008;
                                    status = DeviceCommunicationLogStatus.KnownError;
                                }

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public SendUpdateBuyerResponse SendUpdateBuyer(string Identification, string SerialKey, string BuyerID, string BuyerName, string PhoneNumber, string Password, string IsBlocked, string Admin)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendUpdateBuyerResponse response = new SendUpdateBuyerResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                Buyer buyer = null;
                                buyer = BuyerBLL.GetById(Convert.ToInt32(BuyerID));
                                if (buyer != null)
                                {
                                    Security security = new Security();
                                    string PasswordEncripted = security.MD5(Password);

                                    Buyer updateBuyer = new Buyer();
                                    updateBuyer.BuyerID = buyer.BuyerID;
                                    updateBuyer.CustomerID = buyer.CustomerID;
                                    updateBuyer.FirstName = BuyerName.ToString().Trim();
                                    updateBuyer.Email = buyer.Email;
                                    updateBuyer.Password = string.IsNullOrEmpty(Password.Trim()) ? buyer.Password : PasswordEncripted;
                                    updateBuyer.PhoneNumber = PhoneNumber;
                                    updateBuyer.SerialKey = buyer.SerialKey;
                                    updateBuyer.IsBlocked = IsBlocked == "0" ? false : true;
                                    updateBuyer.Admin = Admin == "0" ? false : true;
                                    updateBuyer.DateCreated = DateTime.Now;
                                    updateBuyer.Active = true;

                                    updateBuyer = new BuyerBLL().Update(updateBuyer, buyer);

                                    response.BuyerID = updateBuyer.BuyerID;
                                    status = DeviceCommunicationLogStatus.Success;
                                }
                                else
                                {
                                    // The Device could not be validated.
                                    response.Status = ResponseCodeEnum.Error;
                                    response.ErrorCode = "E019";
                                    response.ErrorMessage = Resources.ErrorRes.E019;
                                    status = DeviceCommunicationLogStatus.KnownError;
                                }

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public ReceiveMyBuyerDetailResponse ReceiveMyBuyerDetail(string Identification, string SerialKey, string BuyerID)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveMyBuyerDetailResponse response = new ReceiveMyBuyerDetailResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                Buyer buyer = null;
                                buyer = BuyerBLL.GetById(Convert.ToInt32(BuyerID));
                                if (buyer != null)
                                {
                                    response.BuyerID = buyer.BuyerID;
                                    response.BuyerName = buyer.FirstName + " " + buyer.LastName ?? "";
                                    response.Admin = buyer.Admin.Value;
                                    response.PhoneNumber = buyer.PhoneNumber;
                                    response.Email = buyer.Email;
                                    response.IsBlocked = buyer.IsBlocked == true ? 1 : 0;
                                    response.DateCreated = buyer.DateCreated.Value;
                                    
                                    status = DeviceCommunicationLogStatus.Success;
                                }
                                else
                                {
                                    // The Device could not be validated.
                                    response.Status = ResponseCodeEnum.Error;
                                    response.ErrorCode = "E019";
                                    response.ErrorMessage = Resources.ErrorRes.E019;
                                    status = DeviceCommunicationLogStatus.KnownError;
                                }
                                
                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public ReceiveListSupplierByBuyerResponse ReceiveListSupplierByBuyer(string Identification, string SerialKey, string BuyerID)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveListSupplierByBuyerResponse response = new ReceiveListSupplierByBuyerResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                Buyer buyer = BuyerBLL.GetById(Convert.ToInt32(BuyerID));
                                List<Supplier> SupplierList = SupplierBLL.ListByBuyer(buyer);

                                response.SupplierList = new ReceiveListSupplierByBuyerResponse.Supplier[SupplierList.Count()];
                                for (int i = 0; i < SupplierList.Count(); i++)
                                {
                                    response.SupplierList[i] = new ReceiveListSupplierByBuyerResponse.Supplier();
                                    response.SupplierList[i].SupplierID = SupplierList[i].SupplierID;
                                    response.SupplierList[i].Name = SupplierList[i].Name;
                                    response.SupplierList[i].CNPJ = SupplierList[i].CNPJ;
                                    response.SupplierList[i].PhoneNumber = SupplierList[i].PhoneNumber;
                                }

                                status = DeviceCommunicationLogStatus.Success;

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public SendRemoveBuyerSupplierResponse SendRemoveBuyerSupplier(string Identification, string SerialKey, string BuyerID, string SupplierID)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendRemoveBuyerSupplierResponse response = new SendRemoveBuyerSupplierResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                List<BuyerSeller> deleteBuyerSellerList = BuyerSellerBLL.ListByBuyerAndSupplier(Convert.ToInt32(BuyerID), Convert.ToInt32(SupplierID));
                                if (deleteBuyerSellerList.Count() > 0)
                                {
                                    BuyerSellerBLL _BuyerSellerBLL = new BuyerSellerBLL();
                                    _BuyerSellerBLL.BulkDelete(deleteBuyerSellerList);
                                }

                                status = DeviceCommunicationLogStatus.Success;
                               
                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            response.Apple();
            return response;
        }

        public SendNewUserRegistrationResponse SendNewUserRegistration(string BuyerName, string PhoneNumber, string Email, string Password, string CustomerName, string SegmentID, string TypeUser)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendNewUserRegistrationResponse response = new SendNewUserRegistrationResponse();

            Buyer _Buyer = null;
            _Buyer = BuyerBLL.HasEmail(Email);

            Seller _Seller = null;
            _Seller = SellerBLL.GetByEmail(Email);

            if ((_Buyer == null && TypeUser == "0") || (_Seller == null && TypeUser == "1"))
            {

                #region Generacion de SerialKey y Encriptacion del password
                /// <summary>
                /// Array de bytes utilizado para gerar o SerialKey
                /// </summary>
                byte[] _MyCompanyKey = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
                                                            11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 
                                                                21, 22, 23, 24 };

                /// <summary>
                /// Array de bytes utilizado para gerar o SerialKey
                /// </summary>
                byte[] _MyCompanyIV = { 1, 2, 3, 4, 5, 6, 7, 8 };

                KeySeller locKeyHandler = new KeySeller(_MyCompanyKey, _MyCompanyIV);

                string keyvalue = locKeyHandler.SellKey(Password, Email, Password, Key.ProductOrProductFeatureSet.Product2_PaidFullVersion);

                Security security = new Security();
                Password = security.MD5(Password);
                #endregion

                #region Separacion del Nombre
                string[] names = BuyerName.Split(' ');
                string firstName = string.Empty;
                string lastName = string.Empty;

                if (names.Count() == 1)
                {
                    firstName = names[0];
                }
                else if (names.Count() == 2)
                {
                    firstName = names[0];
                    lastName = names.Last();
                }
                else if (names.Count() == 3)
                {
                    firstName = names[0];
                    lastName = names[1] + " " + names[2];
                }
                else if (names.Count() > 3)
                {
                    firstName = names[0] + " " + names[1];
                    lastName = names[2] + " " + names[3];
                }
                #endregion


                _Buyer = new Buyer();
                _Buyer.FirstName = firstName.ToString().Trim();
                _Buyer.LastName = lastName == string.Empty ? _Buyer.LastName : lastName;
                _Buyer.Email = Email;
                _Buyer.Password = Password;
                _Buyer.PhoneNumber = PhoneNumber;
                _Buyer.SerialKey = keyvalue;
                _Buyer.IsBlocked = false;
                _Buyer.Admin = true;
                _Buyer.DateCreated = DateTime.Now;
                _Buyer.Active = true;

                Customer _Customer = new Customer();
                _Customer.Name = CustomerName;
                _Customer.SegmentID = Convert.ToInt32(SegmentID);
                _Customer.DateCreated = DateTime.Now;
                _Customer.Active = true;

                _Buyer.Customer = _Customer;

                #region Temporal para los que descargan del play store
                BuyerSeller newBuyerSeller = new BuyerSeller();
                newBuyerSeller.SellerID = 6;

                _Buyer.BuyerSellers = new EntitySet<BuyerSeller>();
                _Buyer.BuyerSellers.Add(newBuyerSeller);
                #endregion

                _Buyer = new BuyerBLL().Save(_Buyer);

                #region Email para nosotros
                SendGrid.Mail myMessage = SendGrid.Mail.GetInstance();
                myMessage.AddTo("comercial@viewmobile.com.br");
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SenderEmailAccount"].ToString(), "Pediddo");
                myMessage.Subject = "Pediddo - Um novo usuario foi cadastrado";
                myMessage.Text = "#Comprador:\n" +
                                    "Nombre: " + _Buyer.FirstName + "\n" +
                                    "Email: " + _Buyer.Email + "\n" +
                                    "Telefono: " + _Buyer.PhoneNumber + "\n\n" +
                                    "#Empresa:\n" +
                                    "Nombre: " + _Customer.CNPJ + "\n";

                //// Create credentials, specifying your user name and password.
                var credentials = new NetworkCredential("viewmobile", "cade163c#");

                // Create an SMTP transport for sending email.
                var transportWeb = SendGrid.Transport.Web.GetInstance(credentials);
                // Send the email.
                transportWeb.Deliver(myMessage);
                #endregion

                #region Email para el ususario
                myMessage = SendGrid.Mail.GetInstance();
                myMessage.AddTo(Email.Trim());
                myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SenderEmailAccount"].ToString(), "Pediddo");
                myMessage.Subject = "Pediddo";
                myMessage.Text = "Bienvenido. Sugiera a sus proveedores el uso de Pediddo.";

                //// Create credentials, specifying your user name and password.
                credentials = new NetworkCredential("viewmobile", "cade163c#");

                // Create an SMTP transport for sending email.
                transportWeb = SendGrid.Transport.Web.GetInstance(credentials);
                // Send the email.
                transportWeb.Deliver(myMessage);
                #endregion

                if (TypeUser == "1") //Si es Distribuidor
                {
                    Seller newSeller = new Seller();
                    newSeller.Active = true;
                    newSeller.DateCreated = DateTime.Now;
                    newSeller.IsAdmin = true;
                    newSeller.IsBlocked = false;

                    Employee newEmployee = new Employee();
                    newEmployee.Active = true;
                    newEmployee.BirthDate = null;
                    newEmployee.DateCreated = DateTime.Now;
                    newEmployee.Email = Email;
                    newEmployee.FirstName = firstName;
                    newEmployee.Gender = null;
                    newEmployee.LastName = lastName;
                    newEmployee.PhoneNumber = PhoneNumber;
                    newEmployee.Seller = newSeller;
                    newEmployee.Email = Email;
                    newEmployee.Password = Password;

                    Supplier newSupplier = new Supplier();
                    newSupplier.Active = true;
                    newSupplier.DateCreated = DateTime.Now;
                    newSupplier.IsPay = false;
                    newSupplier.Name = CustomerName;
                    newSupplier.SerialKey = keyvalue;
                    newSupplier.Employees = new EntitySet<Employee>();
                    newSupplier.Employees.Add(newEmployee);

                    newSupplier = new SupplierBLL().Save(newSupplier);

                    #region Email para nosotros
                    myMessage = SendGrid.Mail.GetInstance();
                    myMessage.AddTo("comercial@viewmobile.com.br");
                    myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SenderEmailAccount"].ToString(), "Pediddo");
                    myMessage.Subject = "Pediddo - Um novo usuario foi cadastrado";
                    myMessage.Text = "#Distribuidor:\n" +
                                        "Nombre: " + newEmployee.FirstName + " " + newEmployee.LastName + "\n" +
                                        "Email: " + newEmployee.Email + "\n" +
                                        "Telefono: " + newEmployee.PhoneNumber + "\n\n" +
                                        "Empresa: " + newSupplier.Name + "\n";

                    //// Create credentials, specifying your user name and password.
                    credentials = new NetworkCredential("viewmobile", "cade163c#");

                    // Create an SMTP transport for sending email.
                    transportWeb = SendGrid.Transport.Web.GetInstance(credentials);
                    // Send the email.
                    transportWeb.Deliver(myMessage);
                    #endregion

                    #region Email para el ususario
                    myMessage = SendGrid.Mail.GetInstance();
                    myMessage.AddTo(Email.Trim());
                    myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SenderEmailAccount"].ToString(), "Pediddo");
                    myMessage.Subject = "Pediddo";
                    myMessage.Html = NotificationHandler.GetEmailHtml("es","N001");

                    //// Create credentials, specifying your user name and password.
                    credentials = new NetworkCredential("viewmobile", "cade163c#");

                    // Create an SMTP transport for sending email.
                    transportWeb = SendGrid.Transport.Web.GetInstance(credentials);
                    // Send the email.
                    transportWeb.Deliver(myMessage);
                    #endregion

                }
                status = DeviceCommunicationLogStatus.Success;
            }
            else
            {
                // The Device could not be validated.
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E008";
                response.ErrorMessage = Resources.ErrorRes.E008;
                status = DeviceCommunicationLogStatus.KnownError;
            }

            response.Apple();
            return response;
        }

        public ReceiveSellerAuthenticationResponse ReceiveSellerAuthentication(ReceiveSellerAuthenticationRequest request)
        {
            ReceiveSellerAuthenticationResponse response = new ReceiveSellerAuthenticationResponse();
            
            Security securitypass = new Security();
            string password = securitypass.MD5(request.Password);

            SellerBLL sellerBLL = new SellerBLL();
            Seller seller = sellerBLL.Authentication(request.Email, password);
            if (seller != null)
            {
                if (seller.Active == true)
                {

                    if (seller.Employee.SerialKey != null)
                    {
                        EmployeeDevice _EmployeeDevice;
                        Device _Device;
                        _Device = DeviceBLL.GetByDeviceToken(request.UUID);
                        if (_Device == null)
                        {
                            _EmployeeDevice = new EmployeeDevice()
                            {
                                EmployeeID = seller.Employee.EmployeeID,
                                Active = true,
                                DateCreated = DateTime.Now
                            };

                            _Device = new Device();
                            _Device.DeviceToken = request.UUID;
                            _Device.PushToken = request.UUID;
                            _Device.Status = 0;
                            _Device.DeviceOS = request.DeviceOS;
                            _Device.EmployeeDevices = new EntitySet<EmployeeDevice>();
                            _Device.EmployeeDevices.Add(_EmployeeDevice);
                            _Device.DateCreated = DateTime.Now;
                            _Device.Active = true;

                            _Device = new DeviceBLL().Save(_Device);

                        }
                        else
                        {
                            _EmployeeDevice = EmployeeDeviceBLL.GetByEmployeeDevice(seller.Employee, _Device);
                            if (_EmployeeDevice == null)
                            {
                                _EmployeeDevice = new EmployeeDevice()
                                {
                                    EmployeeID = seller.Employee.EmployeeID,
                                    DeviceID = _Device.DeviceID,
                                    DateCreated = DateTime.Now,
                                    Active = true
                                };

                                new EmployeeDeviceBLL().Save(_EmployeeDevice);
                            }
                        }

                        response.Name = (seller.Employee.FirstName ?? "") + " " + (seller.Employee.LastName ?? "");
                        response.SellerID = seller.SellerID;
                        response.IsAdmin = seller.IsAdmin;
                        response.SupplierID = seller.Employee.SupplierID;
                        response.SerialKey = seller.Employee.SerialKey;
                        response.NotificationEndpoint = ConfigurationManager.AppSettings["PediddoNotificationEndpoint"].ToString();

                        SellerWorkingDay sellerWorkingDay = SellerWorkingDayBLL.GetLast(seller.SellerID);
                        if(sellerWorkingDay != null && sellerWorkingDay.Type == (int)SellerWorkingDayEnum.Type.Start)
                        {
                            response.WorkingDayStartDate = sellerWorkingDay.WorkingDayDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E007";
                        response.ErrorMessage = Resources.ErrorRes.E007;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E004";
                    response.ErrorMessage = Resources.ErrorRes.E004;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E001";
                response.ErrorMessage = Resources.ErrorRes.E001;
            }

            return response;
        }

        public SendRegisterNewVisitResponse SendRegisterNewVisit(SendRegisterNewVisitRequest request)
        {
            SendRegisterNewVisitResponse response = new SendRegisterNewVisitResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    #region Procesa Imagen...
                    AppImage appImage = null;
                    AppImage thumbAppImage = null;
                    if (!string.IsNullOrWhiteSpace(request.Base64StringImage))
                    {
                        CloudBlockBlob blockBlob;
                        Security security = new Security();
                        string photoName = security.MD5(request.SerialKey + DateTime.Now.Ticks.ToString()) + ".jpg";
                        string thumbName = security.MD5(request.SerialKey + "Thumb" + DateTime.Now.Ticks.ToString()) + ".jpg";
                        byte[] imageBytes = Convert.FromBase64String(request.Base64StringImage);

                        #region thumb
                        Image image;
                        Image thumbImage;
                        using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                        {
                            image = Image.FromStream(ms);
                            thumbImage = image.GetThumbnailImage(120, 120, () => false, IntPtr.Zero);
                        }
                        ImageConverter imageConverter = new ImageConverter();
                        byte[] thumbByte = (byte[])imageConverter.ConvertTo(thumbImage, typeof(byte[]));
                        #endregion

                        CloudStorageAccount storageAccount =
                            CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString.Pediddo"]);
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerAppImage"]);

                        blockBlob = container.GetBlockBlobReference(photoName);
                        using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                        {
                            blockBlob.UploadFromStream(ms);
                        }

                        blockBlob = container.GetBlockBlobReference(thumbName);
                        using (MemoryStream ms = new MemoryStream(thumbByte, 0, thumbByte.Length))
                        {
                            blockBlob.UploadFromStream(ms);
                        }

                        appImage = new AppImage()
                        {
                            Active = true,
                            DateCreated = DateTime.Now,
                            ImageName = photoName
                        };

                        thumbAppImage = new AppImage()
                        {
                            Active = true,
                            DateCreated = DateTime.Now,
                            ImageName = photoName
                        };
                    }
                    #endregion

                    Buyer buyer = null;
                    Customer customer = null;
                    CustomerVisit customerVisit = new CustomerVisit();
                    if(request.CustomerID == 0)
                    {
                        #region Crea Buyer y su relacion con el Seller...
                        BuyerSeller buyerSeller = new BuyerSeller()
                        {
                            Active = true,
                            CreatorSeller = true,
                            DateCreated = DateTime.Now,
                            OfficialSeller = true,
                            SellerID = sellerUserValidation.seller.SellerID,
                            Status = 1,
                        };

                        #region Generacion de SerialKey y Encriptacion del password

                        string word = string.IsNullOrWhiteSpace(request.ChiefName) ? (string.IsNullOrWhiteSpace(request.ContactName) ? request.Name : request.ContactName) : request.ChiefName;

                        /// <summary>
                        /// Array de bytes utilizado para gerar o SerialKey
                        /// </summary>
                        byte[] _MyCompanyKey = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
                                                                11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 
                                                                    21, 22, 23, 24 };

                        /// <summary>
                        /// Array de bytes utilizado para gerar o SerialKey
                        /// </summary>
                        byte[] _MyCompanyIV = { 1, 2, 3, 4, 5, 6, 7, 8 };

                        KeySeller locKeyHandler = new KeySeller(_MyCompanyKey, _MyCompanyIV);

                        string keyvalue = locKeyHandler.SellKey(DateTime.Now.Ticks.ToString(), word, DateTime.Now.Ticks.ToString(), Key.ProductOrProductFeatureSet.Product2_PaidFullVersion);

                        Security security = new Security();
                        string password = security.MD5("102030");
                        #endregion

                        #region Separa los Nombres
                        string[] names = word.Split(' ');

                        string firstName = string.Empty;
                        string lastName = string.Empty;

                        if (names.Count() == 1)
                        {
                            firstName = names[0];
                        }
                        else if (names.Count() == 2)
                        {
                            firstName = names[0];
                            lastName = names.Last();
                        }
                        else if (names.Count() == 3)
                        {
                            firstName = names[0];
                            lastName = names[1] + " " + names[2];
                        }
                        else if (names.Count() > 3)
                        {
                            firstName = names[0] + " " + names[1];
                            lastName = names[2] + " " + names[3];
                        }
                        #endregion

                        buyer = new Buyer()
                        {
                            Active = true,
                            Admin = true,
                            DateCreated = DateTime.Now,
                            FirstName = firstName,
                            Email = request.Email,
                            IsBlocked = false,
                            LastName = lastName,
                            PhoneNumber = string.IsNullOrWhiteSpace(request.ChiefPhone) ? (string.IsNullOrWhiteSpace(request.ContactPhone) ? request.Phone : request.ContactPhone) : request.ChiefPhone,
                            Password = password,
                            SerialKey = keyvalue
                        };

                        buyer.BuyerSellers = new EntitySet<BuyerSeller>();
                        buyer.BuyerSellers.Add(buyerSeller);
                        #endregion

                        #region Crea Customer y vincula con el Buyer creado...

                        int? segmentId = null;
                        if(request.SegmentId != 0)
                            segmentId = request.SegmentId;

                        customer = new Customer()
                        {
                            SegmentID = segmentId,
                            Name = request.Name,
                            Franchise = request.Franchise,
                            BusinessName = request.BusinessName,
                            CNPJ = request.BusinessDocument,
                            PhoneNumber = request.Phone,
                            Email = request.Email,
                            Latitude = request.Latitude,
                            Longitude = request.Longitude,
                            Observation = request.Observation,
                            ContactName = request.ContactName,
                            ContactPhone = request.ContactPhone,
                            ChiefName = request.ChiefName,
                            ChiefPhone = request.ChiefPhone,
                            Status = (int)CustomerEnum.Status.Potential,
                            CustomerTypeID = (int)CustomerEnum.CustomerTypeID.Distributor,
                            Vip = false,
                            DateCreated = DateTime.Now,
                            Active = true
                        };

                        CustomerLocation customerLocation = new CustomerLocation()
                        {
                            Active = true,
                            AppImage = appImage,
                            DateCreated = DateTime.Now,
                            Reference = request.Observation,
                            Latitude = request.Latitude,
                            Longitude = request.Longitude,
                            Description = "Visita del Vendedor"
                        };

                        customer.Buyers = new EntitySet<Buyer>();
                        customer.Buyers.Add(buyer);
                        customer.CustomerLocations = new EntitySet<CustomerLocation>();
                        customer.CustomerLocations.Add(customerLocation);
                        #endregion

                        #region Crea la Visita y vincula el customer creado...
                        customerVisit.Name = request.Name;
                        customerVisit.Franchise = request.Franchise;
                        customerVisit.Phone = request.Phone;
                        customerVisit.SegmentID = segmentId;
                        customerVisit.Latitude = request.Latitude;
                        customerVisit.Longitude = request.Longitude;
                        customerVisit.Observation = request.Observation;
                        customerVisit.Active = true;
                        customerVisit.DateCreated = DateTime.Now;
                        customerVisit.Customer = customer;
                        #endregion

                    }
                    else
                    {
                        buyer = BuyerBLL.GetBySellerCustomer(sellerUserValidation.seller.SellerID, request.CustomerID).FirstOrDefault();
                        customer = CustomerBLL.GetById(request.CustomerID);

                        #region Actualiza datos del buyer y status del customer...

                        #region Separa los Nombres
                        string[] names = (string.IsNullOrWhiteSpace(request.ChiefName) ? (string.IsNullOrWhiteSpace(request.ContactName) ? request.Name : request.ContactName) : request.ChiefName).Split(' ');

                        string firstName = string.Empty;
                        string lastName = string.Empty;

                        if (names.Count() == 1)
                        {
                            firstName = names[0];
                        }
                        else if (names.Count() == 2)
                        {
                            firstName = names[0];
                            lastName = names.Last();
                        }
                        else if (names.Count() == 3)
                        {
                            firstName = names[0];
                            lastName = names[1] + " " + names[2];
                        }
                        else if (names.Count() > 3)
                        {
                            firstName = names[0] + " " + names[1];
                            lastName = names[2] + " " + names[3];
                        }
                        #endregion

                        buyer.FirstName = firstName;
                        buyer.LastName = lastName;
                        buyer.Email = request.Email;
                        buyer.PhoneNumber = string.IsNullOrWhiteSpace(request.ChiefPhone) ? (string.IsNullOrWhiteSpace(request.ContactPhone) ? request.Phone : request.ContactPhone) : request.ChiefPhone;

                        customer.Status = (int)CustomerEnum.Status.Potential;
                        customer.ContactName = request.ContactName;
                        customer.ContactPhone = request.ContactPhone;
                        customer.ChiefName = request.ChiefName;
                        customer.ChiefPhone = request.ChiefPhone;
                        customer.Email = request.Email;
                        customer.BusinessName = request.BusinessName;
                        customer.CNPJ = request.BusinessDocument;
                        #endregion

                        #region Recupera y usa los datos guardados al marcar la oportunidad
                        customerVisit.CustomerID = customer.CustomerID;
                        customerVisit.Name = customer.Name;
                        customerVisit.Franchise = customer.Franchise;
                        customerVisit.Phone = customer.PhoneNumber;
                        customerVisit.SegmentID = customer.SegmentID.Value;
                        customerVisit.Latitude = customer.Latitude.Value;
                        customerVisit.Longitude = customer.Longitude.Value;
                        customerVisit.Observation = customer.Observation;
                        customerVisit.DateCreated = DateTime.Now;
                        customerVisit.Active = true;
                        #endregion
                    }

                    customerVisit.SellerID = sellerUserValidation.seller.SellerID;
                    customerVisit.ContactName = request.ContactName;
                    customerVisit.ContactPhone = request.ContactPhone;
                    customerVisit.ChiefName = request.ChiefName;
                    customerVisit.ChiefPhone = request.ChiefPhone;
                    customerVisit.Email = request.Email;
                    customerVisit.Description = request.Description;
                    customerVisit.BusinessName = request.BusinessName;
                    customerVisit.BusinessDocument = request.BusinessDocument;

                    if (!string.IsNullOrWhiteSpace(request.DeliveryDate))
                        customerVisit.DeliveryDate = DateTime.ParseExact(request.DeliveryDate, "ddMMyyyyHHmm", CultureInfo.InvariantCulture);
                    if (!string.IsNullOrWhiteSpace(request.VisitStart))
                        customerVisit.VisitStart = DateTime.ParseExact(request.VisitStart, "ddMMyyyyHHmmss", CultureInfo.InvariantCulture);
                    if (!string.IsNullOrWhiteSpace(request.VisitEnd))
                        customerVisit.VisitEnd = DateTime.ParseExact(request.VisitEnd, "ddMMyyyyHHmmss", CultureInfo.InvariantCulture);

                    #region Crea Los productos referentes a la visita...
                    customerVisit.CustomerVisitProducts = new EntitySet<CustomerVisitProduct>();
                    customerVisit.CustomerVisitProducts.AddRange
                    (
                        from p in request.ProductList
                        select new CustomerVisitProduct()
                        {
                            ProductID = p.ProductId,
                            CustomerResponseID = p.Type,
                            PeriodConsumptionID = p.PeriodType,
                            QuantityConsumption = p.Quantity
                        }
                    );
                    # endregion

                    if (request.CustomerID != 0)
                    {
                        new BuyerBLL().Update(buyer);
                        new CustomerBLL().Update(customer);
                    }

                    customerVisit = new CustomerVisitBLL().Save(customerVisit);

                    response.CustomerID = customerVisit.CustomerID;

                    break;
                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveVisitStatusListResponse ReceiveVisitStatusList(ReceiveVisitStatusListRequest request)
        {
            ReceiveVisitStatusListResponse response = new ReceiveVisitStatusListResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<CustomerResponse> customerResponseList = CustomerResponseBLL.List().Where(p => p.CustomerResponseID != 0).ToList();
                    response.VisitStatusList = new List<ReceiveVisitStatusListResponse.VisitStatus>();
                    response.VisitStatusList.AddRange
                    (
                        from r in customerResponseList
                        select new ReceiveVisitStatusListResponse.VisitStatus()
                        {
                            VisitStatusId = r.CustomerResponseID,
                            Name = r.Name
                        }
                    );
                    
                    break;
                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveSellerCustomerListResponse ReceiveSellerCustomerList(ReceiveSellerCustomerListRequest request)
        {
            ReceiveSellerCustomerListResponse response = new ReceiveSellerCustomerListResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<Customer> customerList = null;

                    customerList = CustomerBLL.GetBySupplierID(sellerUserValidation.seller.Employee.SupplierID, request.Query, request.Page);

                    //if (sellerUserValidation.seller.IsAdmin)
                    //{
                    //    customerList = CustomerBLL.GetBySupplierID(sellerUserValidation.seller.Employee.SupplierID, request.Query, request.Page);
                    //}
                    //else
                    //{
                    //    customerList = CustomerBLL.GetBySellerID(sellerUserValidation.seller.SellerID, request.Query, request.Page);
                    //}

                    response.SellerCustomerList = new List<ReceiveSellerCustomerListResponse.SellerCustomer>();
                    response.SellerCustomerList.AddRange
                    (
                        from c in customerList
                        where c.Status != (int)CustomerEnum.Status.Opportunity
                        select new ReceiveSellerCustomerListResponse.SellerCustomer()
                        {
                            CustomerID = c.CustomerID,
                            Name = c.Name,
                            BusinessName = c.BusinessName,
                            BusinessDocument = c.CNPJ,
                            Franchise = c.Franchise,
                            Phone = c.PhoneNumber,
                            SegmentId = c.SegmentID ?? 0,
                            SegmentName = c.Segment == null ? "" : c.Segment.Name,
                            Status = c.Status,
                            StatusName = (c.Status == 2 ? "Prospecto" : (c.Status == 3 ? "Activo" : "Inactivo")),
                            Latitude = c.CustomerLocations.Count >=1 ? c.CustomerLocations.FirstOrDefault().Latitude : 0,
                            Longitude = c.CustomerLocations.Count >= 1 ? c.CustomerLocations.FirstOrDefault().Longitude : 0,
                            Observation = c.Observation,
                            ContactName = c.ContactName,
                            ContactPhone = c.ContactPhone,
                            ChiefName = c.ChiefName,
                            ChiefPhone = c.ChiefPhone,
                            Email = c.Email,
                            SellerName = c.Buyers.FirstOrDefault().BuyerSellers.FirstOrDefault().Seller != null ? 
                                         (c.Buyers.FirstOrDefault().BuyerSellers.FirstOrDefault().Seller.Employee.FirstName ?? "") :
                                         "App Movil",
                            SellerPhone = c.Buyers.FirstOrDefault().BuyerSellers.FirstOrDefault().Seller != null ? 
                                          (c.Buyers.FirstOrDefault().BuyerSellers.FirstOrDefault().Seller.Employee.PhoneNumber ?? "") :
                                          ""
                        }
                    );
                    

                    break;
                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendNewSellerOrderResponse SendNewSellerOrder(SendNewSellerOrderRequest request)
        {
            SendNewSellerOrderResponse response = new SendNewSellerOrderResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    Customer customer = CustomerBLL.GetById(request.CustomerID);
                    CustomerLocation customerLocation = customer.CustomerLocations.FirstOrDefault();

                    Buyer _Buyer = BuyerBLL.GetBySellerCustomer(sellerUserValidation.seller.SellerID, request.CustomerID).FirstOrDefault();

                    BuyerSeller buyerSeller = null;
                    if(_Buyer == null)
                    {
                        _Buyer = BuyerBLL.GetByCustomer(customer.CustomerID).OrderByDescending(o=>o.DateCreated).FirstOrDefault();
                        buyerSeller = new BuyerSeller()
                        {
                            Active = true,
                            DateCreated = DateTime.Now,
                            Status = 1,
                            CreatorSeller = false,
                            OfficialSeller = false,
                            SellerID = sellerUserValidation.seller.SellerID,
                            BuyerID = _Buyer.BuyerID
                        };
                    }

                    Random random = new Random();
                    string ReceiptCode = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(0, 1000).ToString();

                    MyOrder _MyOrder = new MyOrder();
                    _MyOrder.BuyerID = _Buyer.BuyerID;
                    _MyOrder.SupplierID = sellerUserValidation.seller.Employee.SupplierID;

                    if(request.Sample)
                        _MyOrder.MyOrderType = (int)MyOrderEnum.Type.FreeSample;
                    else
                        _MyOrder.MyOrderType = (int)MyOrderEnum.Type.OrderSeller;

                    _MyOrder.ReceiptCode = ReceiptCode;
                    _MyOrder.RepeatOrder = false;
                    _MyOrder.DateCreated = DateTime.Now;
                    _MyOrder.Active = true;
                    _MyOrder.MyOrderHistories = new EntitySet<MyOrderHistory>();
                    _MyOrder.MyOrderProducts = new EntitySet<MyOrderProduct>();

                    decimal cost = 0;
                    decimal salePrice;
                    decimal price;
                    MyOrderProduct _MyOrderProduct;
                    List<MyOrderProduct> _MyOrderProductList = new List<MyOrderProduct>();
                    Product product;
                    PriceListDetail priceListDetail;
                    foreach (SendNewSellerOrderRequest.Product productItem in request.ProductList)
                    {
                        product = ProductBLL.GetById(productItem.ProductId);
                        priceListDetail = null;
                        if(productItem.PriceListId != null && productItem.PriceListId > 0)
                            priceListDetail = PriceListDetailBLL.GetById(productItem.PriceListId.Value, product.ProductID);

                        salePrice = priceListDetail != null ?
                                    ((priceListDetail.OfferPrice ?? 0) != 0 ?
                                    priceListDetail.OfferPrice.Value :
                                    (priceListDetail.MarginDiscount == 0 ?
                                    priceListDetail.SalePrice :
                                    priceListDetail.DiscountPrice.Value)
                                    ) : product.Cost;

                        price = salePrice;
                        if (productItem.Discount > 0)
                            price = Math.Ceiling(price * (((decimal)100 - productItem.Discount) / (decimal)100));

                        _MyOrderProduct = new MyOrderProduct();
                        _MyOrderProduct.Active = true;
                        _MyOrderProduct.DateCreated = DateTime.Now;
                        _MyOrderProduct.Discount = productItem.Discount;
                        _MyOrderProduct.DiscountInitial = 0;
                        _MyOrderProduct.IsAdded = false;
                        _MyOrderProduct.ProductID = product.ProductID;
                        _MyOrderProduct.Cost = price;
                        _MyOrderProduct.SalePrice = salePrice;
                        _MyOrderProduct.InOffer = (priceListDetail != null ? (priceListDetail.OfferPrice ?? 0) != 0 : false);
                        _MyOrderProduct.Quantity = productItem.Quantity;
                        _MyOrderProduct.QuantityInitial = productItem.Quantity;
                        _MyOrderProduct.PriceListID = (productItem.PriceListId == 0 ? null : productItem.PriceListId);
                        
                        _MyOrderProductList.Add(_MyOrderProduct);
                        cost = cost + (product.Cost * productItem.Quantity);
                    }
                    _MyOrder.MyOrderProducts.AddRange(_MyOrderProductList);

                    MyOrderHistory _MyOrderHistory = new MyOrderHistory();
                    _MyOrderHistory.SellerID = sellerUserValidation.seller.SellerID;
                    _MyOrderHistory.Status = (int)MyOrderHistoryEnum.Status.AwaitingAttention;
                    _MyOrderHistory.InteractionID = (int)MyOrderHistoryEnum.InteractionID.NewOrder;
                    _MyOrderHistory.Cost = cost;
                    _MyOrderHistory.PayMode = request.PayMode;
                    _MyOrderHistory.PaymentMethod = (int)MyOrderHistoryEnum.PaymentMethod.Cash;
                    _MyOrderHistory.PaymentDate = DateTime.ParseExact(request.PaymentDate, "ddMMyyyy", CultureInfo.InvariantCulture);
                    _MyOrderHistory.DeliveryDate = DateTime.ParseExact(request.DeliveryDate, "ddMMyyyyHHmm", CultureInfo.InvariantCulture);
                    _MyOrderHistory.DateCreated = DateTime.Now;
                    _MyOrderHistory.Active = true;
                    if (customerLocation != null)
                        _MyOrderHistory.CustomerLocationID = customerLocation.CustomerLocationID;


                    _MyOrder.MyOrderHistories.Add(_MyOrderHistory);


                    new MyOrderBLL().Save(_MyOrder);
                    if (customer.Status != (int)CustomerEnum.Status.Activo)
                    {
                        customer.Status = (int)CustomerEnum.Status.Activo;
                        new CustomerBLL().Update(customer);
                    }

                    if (buyerSeller != null)
                        new BuyerSellerBLL().Save(buyerSeller);


                    break;
                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendWorkingDayResponse SendWorkingDay(SendWorkingDayRequest request)
        {
            SendWorkingDayResponse response = new SendWorkingDayResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    DateTime workingDayDate = DateTime.ParseExact(request.WorkingDayDate, "ddMMyyyyHHmm", CultureInfo.InvariantCulture);
                    //SellerWorkingDay _SellerWorkingDay = SellerWorkingDayBLL.GetByDateAndType(sellerUserValidation.seller.SellerID, workingDayDate.Date, request.WorkingDayType);
                    //if (_SellerWorkingDay == null)
                    //{
                    TrackPoint trackPoint = new TrackPoint()
                    {
                        Active = true,
                        DateCreated = DateTime.Now,
                        Latitude = request.Latitude,
                        Longitude = request.Longitude,
                        TrackPointTypeID = (int)TrackPointTypeEnum.TrackPointTypeID.WorkingDay
                    };

                    SellerWorkingDay sellerWorkingDay = new SellerWorkingDay()
                    {
                        Active = true,
                        DateCreated = DateTime.Now,
                        SellerID = sellerUserValidation.seller.SellerID,
                        Type = request.WorkingDayType,
                        WorkingDayDate = workingDayDate,
                        TrackPoint = trackPoint
                    };

                    new SellerWorkingDayBLL().Save(sellerWorkingDay);

                    //}
                    //else
                    //{
                    //    response.Status = ResponseCodeEnum.Error;

                    //    if (request.WorkingDayType == (int)SellerWorkingDayEnum.Type.Start)
                    //    {
                    //        response.ErrorCode = "E022";
                    //        response.ErrorMessage = Resources.ErrorRes.E022;
                    //    }
                    //    else
                    //    {
                    //        response.ErrorCode = "E023";
                    //        response.ErrorMessage = Resources.ErrorRes.E023;
                    //    }
                    //}

                    //List<Seller> sellerAdminList = SellerBLL.ListAdmin(sellerUserValidation.seller.Employee.SupplierID);
                    List<Dealer> dealerAdminList = DealerBLL.ListAdmin(sellerUserValidation.seller.Employee.SupplierID);
                    string[] message = Resources.NotificationPush.N015.Split('#');
                    List<Notification> notificationList = new List<Notification>();
                    string sellerFullName = ((sellerUserValidation.seller.Employee.FirstName ?? "") + " " + (sellerUserValidation.seller.Employee.LastName ?? ""));
                    foreach (Dealer dealer in dealerAdminList)
                    {
                        
                        notificationList.Add(new Notification()
                        {
                            Active = true,
                            Activity = 0,
                            AppID = (int)AppEnum.AppID.PediddoDelivery,
                            Channel = dealer.Employee.SerialKey,
                            ContentText = message[1]
                                          .Replace("{SellerName}", sellerFullName)
                                          .Replace("{Type}", (request.WorkingDayType == 1 ? "iniciado" : "finalizado")),
                            ContentTitle = message[0].Replace("{TypeTitle}", (request.WorkingDayType == 1 ? "Inicio" : "Fin")),
                            DateCreated = DateTime.Now,
                            DateToSend = DateTime.Now,
                            DeviceOS = "1",
                            IsRead = true,
                            Send = false,
                            Type = 1
                        });
                    }

                    if (notificationList.Count() > 0)
                        new NotificationBLL().BulkSave(notificationList);


                    break;

                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendUpdateSellerCustomerResponse SendUpdateSellerCustomer(SendUpdateSellerCustomerRequest request)
        {
            SendUpdateSellerCustomerResponse response = new SendUpdateSellerCustomerResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    Customer customer = CustomerBLL.GetById(request.CustomerID);

                    CustomerLocation customerLocation = new CustomerLocation();
                    if (customer != null)
                    {

                        
                        customer.Name = request.Name;
                        customer.BusinessName = request.BusinessName;
                        customer.CNPJ = request.BusinessDocument;
                        customer.Franchise = request.Franchise;
                        customer.PhoneNumber = request.Phone;
                        customer.SegmentID = request.SegmentId;
                        customer.Status = request.Status;
                        customer.Latitude = request.Latitude;
                        customer.Longitude = request.Longitude;
                        customer.Observation = request.Observation;
                        customer.ContactName = request.ContactName;
                        customer.ContactPhone = request.ContactPhone;
                        customer.ChiefName = request.ChiefName;
                        customer.ChiefPhone = request.ChiefPhone;
                        customer.Email = request.Email;

                        if(customer.CustomerLocations.Count >= 1)
                        {
                            customerLocation = customer.CustomerLocations.FirstOrDefault();
                            customerLocation.Latitude = request.Latitude;
                            customerLocation.Longitude = request.Longitude;
                            
                            new CustomerLocationBLL().Update(customerLocation);
                        }
                        

                        new CustomerBLL().Update(customer);
                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E024";
                        response.ErrorMessage = Resources.ErrorRes.E024;
                    }

                    break;

                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendNewOpportunityResponse SendNewOpportunity(SendNewOpportunityRequest request)
        {
            SendNewOpportunityResponse response = new SendNewOpportunityResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            DealerUserValidator dealerUserValidator = new DealerUserValidator();
            DealerUserValidation dealerUserValidation = dealerUserValidator.Validate(request.SerialKey, request.UUID);

            if (sellerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk || dealerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk)
            {

                Buyer buyer = null;
                Customer customer = null;

                #region Crea Buyer y su relacion con el Seller...

                int sellerId = request.SellerId != 0 ? request.SellerId : sellerUserValidation.seller.SellerID;

                BuyerSeller buyerSeller = null;
                buyerSeller = new BuyerSeller()
                {
                    Active = true,
                    CreatorSeller = true,
                    DateCreated = DateTime.Now,
                    OfficialSeller = true,
                    SellerID = sellerId,
                    Status = 1,
                };

                #region Generacion de SerialKey y Encriptacion del password

                string word = request.Name;

                /// <summary>
                /// Array de bytes utilizado para gerar o SerialKey
                /// </summary>
                byte[] _MyCompanyKey = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
                                                        11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 
                                                            21, 22, 23, 24 };

                /// <summary>
                /// Array de bytes utilizado para gerar o SerialKey
                /// </summary>
                byte[] _MyCompanyIV = { 1, 2, 3, 4, 5, 6, 7, 8 };

                KeySeller locKeyHandler = new KeySeller(_MyCompanyKey, _MyCompanyIV);

                string keyvalue = locKeyHandler.SellKey(DateTime.Now.Ticks.ToString(), word, DateTime.Now.Ticks.ToString(), Key.ProductOrProductFeatureSet.Product2_PaidFullVersion);

                Security _security = new Security();
                string password = _security.MD5("102030");
                #endregion

                buyer = new Buyer()
                {
                    Active = true,
                    Admin = true,
                    DateCreated = DateTime.Now,
                    FirstName = request.Name,
                    IsBlocked = false,
                    PhoneNumber = request.Phone,
                    Password = password,
                    SerialKey = keyvalue
                };

                buyer.BuyerSellers = new EntitySet<BuyerSeller>();
                buyer.BuyerSellers.Add(buyerSeller);

                #endregion

                #region Crea Customer y vincula con el Buyer creado...

                int? segmentId = null;
                if (request.SegmentId != 0)
                    segmentId = request.SegmentId;

                customer = new Customer()
                {
                    Active = true,
                    DateCreated = DateTime.Now,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Name = request.Name,
                    Franchise = request.Franchise,
                    PhoneNumber = request.Phone,
                    Observation = request.Observation,
                    SegmentID = segmentId,
                    Status = (int)CustomerEnum.Status.Opportunity
                };

                customer.Buyers = new EntitySet<Buyer>();
                customer.Buyers.Add(buyer);
                #endregion

                #region Procesa Imagen...
                if (!string.IsNullOrWhiteSpace(request.Base64StringImage))
                {
                    CloudBlockBlob blockBlob;
                    Security security = new Security();
                    string photoName = security.MD5(request.SerialKey + DateTime.Now.Ticks.ToString()) + ".jpg";
                    string thumbName = security.MD5(request.SerialKey + "Thumb" + DateTime.Now.Ticks.ToString()) + ".jpg";
                    byte[] imageBytes = Convert.FromBase64String(request.Base64StringImage);

                    #region thumb
                    Image image;
                    Image thumbImage;
                    using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                    {
                        image = Image.FromStream(ms);
                        thumbImage = image.GetThumbnailImage(120, 120, () => false, IntPtr.Zero);
                    }
                    ImageConverter imageConverter = new ImageConverter();
                    byte[] thumbByte = (byte[])imageConverter.ConvertTo(thumbImage, typeof(byte[]));
                    #endregion

                    CloudStorageAccount storageAccount =
                        CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString.Pediddo"]);
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerAppImage"]);

                    blockBlob = container.GetBlockBlobReference(photoName);
                    using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                    {
                        blockBlob.UploadFromStream(ms);
                    }

                    blockBlob = container.GetBlockBlobReference(thumbName);
                    using (MemoryStream ms = new MemoryStream(thumbByte, 0, thumbByte.Length))
                    {
                        blockBlob.UploadFromStream(ms);
                    }

                    customer.AppImage = new AppImage()
                    {
                        Active = true,
                        DateCreated = DateTime.Now,
                        ImageName = photoName
                    };

                    customer.AppImage1 = new AppImage()
                    {
                        Active = true,
                        DateCreated = DateTime.Now,
                        ImageName = thumbName
                    };
                }
                #endregion

                customer = new CustomerBLL().Save(customer);

                #region Notificaciones Push...
                if (request.SellerId != 0)
                {
                    Seller seller = SellerBLL.GetById(request.SellerId);

                    List<Notification> notificationList = new List<Notification>();

                    string[] message = Resources.NotificationPush.N017
                                       .Replace("{DealerName}", (dealerUserValidation.dealer.Employee.FirstName ?? "") + " " + (dealerUserValidation.dealer.Employee.LastName ?? ""))
                                       .Replace("{CustomerName}", request.Name)
                                       .Split('#');
                    notificationList.Add(new Notification()
                    {
                        Active = true,
                        Activity = 0,
                        AppID = (int)AppEnum.AppID.PediddoPlus,
                        Channel = seller.Employee.SerialKey,
                        ContentText = message[1],
                        ContentTitle = message[0],
                        DateCreated = DateTime.Now,
                        DateToSend = DateTime.Now,
                        DeviceOS = "1",
                        IsRead = true,
                        Send = false,
                        Type = 1
                    });

                    new NotificationBLL().BulkSave(notificationList);
                }
                #endregion

                response.CustomerID = customer.CustomerID;
            }
            else
            {
                ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                response.ErrorCode = error.ErrorCode;
                response.ErrorMessage = error.ErrorMessage;
            }

            return response;
        }

        public ReceiveOpportunityListResponse ReceiveOpportunityList(ReceiveOpportunityListRequest request)
        {
            ReceiveOpportunityListResponse response = new ReceiveOpportunityListResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<Customer> customerOpportunityList = CustomerBLL.GetBySellerID(sellerUserValidation.seller.SellerID,
                                                                                       (int)CustomerEnum.Status.Opportunity,
                                                                                       request.Query,
                                                                                       request.Page);
                    
                    response.OpportunityList = new List<ReceiveOpportunityListResponse.Opportunity>();
                    response.OpportunityList.AddRange
                    (
                        from op in customerOpportunityList
                        select new ReceiveOpportunityListResponse.Opportunity()
                        {
                            CustomerID = op.CustomerID,
                            Franchise = op.Franchise,
                            Latitude = op.Latitude.Value,
                            Longitude = op.Longitude.Value,
                            Name = op.Name,
                            Observation = op.Observation,
                            Phone = op.PhoneNumber,
                            SegmentID = op.SegmentID ?? 0,
                            SegmentName = op.SegmentID != null ? op.Segment.Name : "",
                            ImageId = op.AppImageId ?? 0,
                            ThumbImageId = op.ThumbAppImageId ?? 0
                        }
                    );


                    break;

                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendSellerLocationResponse SendSellerLocation(SendSellerLocationRequest request)
        {
            SendSellerLocationResponse response = new SendSellerLocationResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    DateTime dateGPS = DateTime.ParseExact(request.DateGps, "ddMMyyyyHHmmss", CultureInfo.InvariantCulture);

                    SellerTrackPoint sellerTrackPoint = new SellerTrackPoint()
                    {
                        SellerID = sellerUserValidation.seller.SellerID,
                        TrackPoint = new TrackPoint()
                        {
                            Active = true,
                            DateCreated = DateTime.Now,
                            Latitude = request.Latitude,
                            Longitude = request.Longitude,
                            TrackPointTypeID = (int)TrackPointTypeEnum.TrackPointTypeID.Track,
                            DateGPS = dateGPS
                        }
                    };

                    new SellerTrackPointBLL().Save(sellerTrackPoint);

                    response.TrackingTime = Convert.ToInt32(ConfigurationManager.AppSettings["SendSellerLocationTrackingTime"]);

                    break;

                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveReportSaleResponse ReceiveReportSale(ReceiveReportSaleRequest request)
        {
            ReceiveReportSaleResponse response = new ReceiveReportSaleResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:
                    response.ReportSaleList = new List<ReceiveReportSaleResponse.ReportSale>();
                    List<MyOrder> myOrderList = MyOrderBLL.ListBySellerAndType(sellerUserValidation.seller, (int)MyOrderEnum.Type.OrderSeller, request.Month, request.Year);
                    if (myOrderList.Count() > 0)
                    {
                        response.ReportSaleList.Add(new ReceiveReportSaleResponse.ReportSale()
                        {
                            Type = 1,
                            Amount = myOrderList.Sum(o=>o.MyOrderHistories.OrderByDescending(oh=>oh.DateCreated).FirstOrDefault().Cost).Value
                        });
                    }

                    List<CustomerVisit> customerVisitList = CustomerVisitBLL.ListBySeller(sellerUserValidation.seller.SellerID, request.Month, request.Year).Where(c=>c.Customer.Status == (int)CustomerEnum.Status.Potential).ToList();
                    if (customerVisitList.Count() > 0)
                    {
                        response.ReportSaleList.Add(new ReceiveReportSaleResponse.ReportSale()
                        {
                            Type = 2,
                            Amount = customerVisitList.Sum(v=>v.CustomerVisitProducts.Sum(p=>p.QuantityConsumption * p.Product.Cost)).Value
                        });
                    }

                    break;

                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public Stream ReceiveImage(string Serial, string Identification, string ImageId)
        {
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(Serial, Identification);

            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(Serial, Identification);

            DealerUserValidator dealerUserValidator = new DealerUserValidator();
            DealerUserValidation dealerUserValidation = dealerUserValidator.Validate(Serial, Identification);

            MemoryStream response = new MemoryStream();

            byte[] byteArray;
            if (sellerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk || buyerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk || dealerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk)
            {
                AppImage image = AppImageBLL.GetById(Convert.ToInt32(ImageId));
                if (image != null)
                {
                    if (!string.IsNullOrWhiteSpace(image.ImageName))
                    {
                        CloudStorageAccount storageAccount =
                        CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString.Pediddo"]);
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerAppImage"]);

                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(image.ImageName);

                        WebOperationContext.Current.OutgoingResponse.ContentType = "image/png";
                        blockBlob.DownloadToStream(response);
                        response.Position = 0;
                    }
                }
                else
                {
                    byteArray = Encoding.UTF8.GetBytes(" La imagen solicitada no existe.");
                    response = new MemoryStream(byteArray);
                }

            }
            else
            {
                ErrorResponse error = sellerUserValidator.ProcessError(ValidationCodeEnum.UnknowMobileUser);
                byteArray = Encoding.UTF8.GetBytes(" " + error.ErrorCode + " - " + error.ErrorMessage);
                response = new MemoryStream(byteArray);
            }

            return response;
        }

        public SendCustomerRegistrationResponse SendCustomerRegistration(SendCustomerRegistrationRequest request)
        {
            SendCustomerRegistrationResponse response = new SendCustomerRegistrationResponse();

            Customer customer = null;

            customer = new Customer();

            customer.Name = request.CustomerName;
            customer.Email = request.Email;
            customer.Status = (int)CustomerEnum.Status.Activo;

            customer.DateCreated = DateTime.Now;
            customer.Active = true;

            customer = new CustomerBLL().Save(customer);

            response.CustomerID = customer.CustomerID;

            return response;
        }

        public SendNewBillResponse SendNewBill(SendNewBillRequest request)
        {
            SendNewBillResponse response = new SendNewBillResponse();

            Bill bill = null;

            bill = new Bill();

            bill.CustomerID = request.CustomerID;
            bill.ProductName = request.ProductName;
            bill.Value = request.Value;
            bill.Obs = request.Obs;
            bill.Paid = request.Paid;

            bill = new BillBLL().Save(bill);

            response.BillID = bill.BillID;

            return response;
        }


        public UpdateBillResponse UpdateBill(UpdateBillRequest request)
        {
            UpdateBillResponse response = new UpdateBillResponse();


            Bill bill = new BillBLL().GetById(request.BillID);

            bill.ProductName = request.ProductName;
            bill.Value = request.Value;
            bill.Obs = request.Obs;
            bill.Paid = request.Paid;

            bill = new BillBLL().Update(bill);

            response.BillID = bill.BillID;

            return response;
        }

        public SendCreateCustomerLocationResponse SendCreateCustomerLocation(SendCreateCustomerLocationRequest request)
        {
            SendCreateCustomerLocationResponse response = new SendCreateCustomerLocationResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);


            if (buyerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk || sellerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk)
            {
                    Buyer _buyer;
                    if (request.BuyerId > 0)
                        _buyer = BuyerBLL.GetById(request.BuyerId);
                    else
                        _buyer = buyerUserValidation.buyer;


                    CustomerLocation customerLocation = new CustomerLocation()
                    {
                        Active = true,
                        CustomerID = _buyer.CustomerID.Value,
                        Description = request.Description,
                        Address = request.Address,
                        CityID = request.CityID,
                        Complement = request.Complement,
                        DateCreated = DateTime.Now,
                        Latitude = request.Latitude,
                        Locality = request.Locality,
                        Longitude = request.Longitude,
                        Number = request.Number,
                        Reference = request.Reference,
                        Temporary = false
                    };

                    #region Procesa Imagen...
                    AppImage appImage = null;
                    if (!string.IsNullOrWhiteSpace(request.Base64StringImage))
                    {
                        CloudBlockBlob blockBlob;
                        Security security = new Security();
                        string photoName = security.MD5(request.SerialKey + DateTime.Now.Ticks.ToString()) + ".jpg";
                        byte[] imageBytes = Convert.FromBase64String(request.Base64StringImage);

                        CloudStorageAccount storageAccount =
                            CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString.Pediddo"]);
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerAppImage"]);

                        blockBlob = container.GetBlockBlobReference(photoName);
                        using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                        {
                            blockBlob.UploadFromStream(ms);
                        }

                        appImage = new AppImage()
                        {
                            Active = true,
                            DateCreated = DateTime.Now,
                            ImageName = photoName
                        };

                        customerLocation.AppImage = appImage;
                    }
                    #endregion

                    customerLocation = new CustomerLocationBLL().Save(customerLocation);

                    response.LocationID = customerLocation.CustomerLocationID;

            }
            else
            {
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

            }
            return response;
        }

        public ReceiveCustomerLocationListResponse ReceiveCustomerLocationList(ReceiveCustomerLocationListRequest request)
        {
            ReceiveCustomerLocationListResponse response = new ReceiveCustomerLocationListResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    response.LocationList = (from cl in CustomerLocationBLL.ListByCustomerID(buyerUserValidation.buyer.CustomerID.Value, false)
                                            select new ReceiveCustomerLocationListResponse.Location()
                                            {
                                                LocationID = cl.CustomerLocationID,
                                                Address = cl.Address,
                                                CityID = cl.CityID ?? 0,
                                                Complement = cl.Complement,
                                                Description = cl.Description,
                                                Latitude = cl.Latitude,
                                                Locality = cl.Locality,
                                                Longitude = cl.Longitude,
                                                Number = cl.Number,
                                                Reference = cl.Reference,
                                                ImageId = cl.AppImageID ?? 0
                                            })
                                            .ToList();

                    break;

                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveCategoryListResponse ReceiveCategoryList(ReceiveCategoryListRequest request)
        {
            ReceiveCategoryListResponse response = new ReceiveCategoryListResponse();

            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);


            if (buyerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk || sellerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk)
            {
                List<Category> categoryList;

                if (request.CategoryType != 0)
                {
                    categoryList = CategoryBLL.ListBySupplierID(request.SupplierId).Where(c => c.CategoryType == request.CategoryType).ToList();
                }
                else if(request.CategoryId != 0)
                {
                    categoryList = CategoryBLL.ListChildCategory(request.CategoryId);
                }
                else 
                {
                    if (request.CustomerTypeId == 0)
                    {
                        request.CustomerTypeId = buyerUserValidation.buyer.Customer.CustomerTypeID.Value;
                    }
                    categoryList = CategoryBLL.ListByCustomerTypeId(request.SupplierId, request.CustomerTypeId);
                }

                response.CategoryList = (from c in categoryList
                                         select new ReceiveCategoryListResponse.Category()
                                         {
                                             CategoryId = c.CategoryID,
                                             CategoryName = c.Name,
                                             HasChildren = CategoryBLL.ListChildCategory(c.CategoryID).Count > 0,
                                             ThumbImageId = c.ThumbAppImageID ?? 0
                                         }
                                        ).ToList();
            }
            else
            {
                ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                response.ErrorCode = error.ErrorCode;
                response.ErrorMessage = error.ErrorMessage;
            }

            return response;

        }

        public SendCreateMyOrderResponse SendCreateMyOrder(SendCreateMyOrderRequest request)
        {
            SendCreateMyOrderResponse response = new SendCreateMyOrderResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    CustomerLocation customerLocation = null;
                    if(request.LocationID == 0)
                    {
                        customerLocation = new CustomerLocation()
                        {
                            Active = true,
                            DateCreated = DateTime.Now,
                            Latitude = request.Latitude,
                            Longitude = request.Longitude,
                            Reference = request.Reference,
                            CustomerID = buyerUserValidation.buyer.CustomerID.Value,
                            Temporary = true
                        };

                        #region Procesa Imagen...
                        AppImage appImage = null;
                        if (!string.IsNullOrWhiteSpace(request.Base64StringImage))
                        {
                            CloudBlockBlob blockBlob;
                            Security security = new Security();
                            string photoName = security.MD5(request.SerialKey + DateTime.Now.Ticks.ToString()) + ".jpg";
                            byte[] imageBytes = Convert.FromBase64String(request.Base64StringImage);

                            CloudStorageAccount storageAccount =
                                CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString.Pediddo"]);
                            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                            CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerAppImage"]);

                            blockBlob = container.GetBlockBlobReference(photoName);
                            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                            {
                                blockBlob.UploadFromStream(ms);
                            }

                            appImage = new AppImage()
                            {
                                Active = true,
                                DateCreated = DateTime.Now,
                                ImageName = photoName
                            };

                            customerLocation.AppImage = appImage;
                        }
                        #endregion

                        //customerLocation = new CustomerLocationBLL().Save(customerLocation);
                    }


                    Random random = new Random();
                    string ReceiptCode = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(0, 1000).ToString();

                    MyOrder _MyOrder = new MyOrder();
                    _MyOrder.BuyerID = buyerUserValidation.buyer.BuyerID;
                    _MyOrder.MyOrderType = (int)MyOrderEnum.Type.Order;
                    
                    _MyOrder.ReceiptCode = ReceiptCode;
                    _MyOrder.RepeatOrder = false;
                    _MyOrder.DateCreated = DateTime.Now;
                    _MyOrder.Active = true;
                    _MyOrder.MyOrderHistories = new EntitySet<MyOrderHistory>();
                    _MyOrder.MyOrderProducts = new EntitySet<MyOrderProduct>();

                    decimal cost = 0;
                    decimal salePrice;
                    decimal price;
                    MyOrderProduct _MyOrderProduct;
                    List<MyOrderProduct> _MyOrderProductList = new List<MyOrderProduct>();
                    Product product;
                    PriceListDetail priceListDetail;
                    foreach (SendCreateMyOrderRequest.OrderProduct productItem in request.OrderProductList)
                    {
                        product = ProductBLL.GetById(productItem.ProductId);
                        priceListDetail = null;
                        if(productItem.PriceListId != null && productItem.PriceListId > 0)
                            priceListDetail = PriceListDetailBLL.GetById(productItem.PriceListId.Value, product.ProductID);

                        salePrice = priceListDetail != null ?
                                    ((priceListDetail.OfferPrice ?? 0) != 0 ?
                                    priceListDetail.OfferPrice.Value :
                                    (priceListDetail.MarginDiscount == 0 ?
                                    priceListDetail.SalePrice :
                                    priceListDetail.DiscountPrice.Value)
                                    ) : product.Cost;

                        price = salePrice;
                        if (productItem.Discount > 0)
                            price = Math.Ceiling(price * (((decimal)100 - productItem.Discount) / (decimal)100));

                        _MyOrderProduct = new MyOrderProduct();
                        _MyOrderProduct.Active = true;
                        _MyOrderProduct.DateCreated = DateTime.Now;
                        _MyOrderProduct.Discount = productItem.Discount;
                        _MyOrderProduct.DiscountInitial = 0;
                        _MyOrderProduct.IsAdded = false;
                        _MyOrderProduct.ProductID = product.ProductID;
                        _MyOrderProduct.Cost = price;
                        _MyOrderProduct.SalePrice = salePrice;
                        _MyOrderProduct.InOffer = (priceListDetail != null ? (priceListDetail.OfferPrice ?? 0) != 0 : false);
                        _MyOrderProduct.Quantity = productItem.Quantity;
                        _MyOrderProduct.QuantityInitial = productItem.Quantity;
                        _MyOrderProduct.PriceListID = (productItem.PriceListId == 0 ? null : productItem.PriceListId);
                        
                        _MyOrderProductList.Add(_MyOrderProduct);
                        cost = cost + (product.Cost * productItem.Quantity);
                    }
                    _MyOrder.MyOrderProducts.AddRange(_MyOrderProductList);


                    if (request.ShippingCost > 0)
                    {
                        ShippingCostRule rule = ShippingCostRuleBLL.GetById((int)ShippingCostRuleEnum.ShippingCostRuleId.OrderAmount);
                        if (rule.Active)
                        {
                            if (cost > rule.Value)
                            {
                                request.ShippingCost = 0;
                            }
                        }
                    }
                    response.ShippingCost = request.ShippingCost;

                    MyOrderHistory _MyOrderHistory = new MyOrderHistory();
                    _MyOrderHistory.SellerID = null;
                    _MyOrderHistory.Status = (int)MyOrderHistoryEnum.Status.AwaitingAttention;
                    _MyOrderHistory.InteractionID = (int)MyOrderHistoryEnum.InteractionID.NewOrder;
                    _MyOrderHistory.Cost = cost;
                    _MyOrderHistory.PayMode = (int)MyOrderHistoryEnum.PayMode.CashPayment;
                    _MyOrderHistory.PaymentMethod = request.PaymentMethod;

                    if (customerLocation != null)
                        _MyOrderHistory.CustomerLocation = customerLocation;
                    else
                        _MyOrderHistory.CustomerLocationID = request.LocationID;

                    _MyOrderHistory.ChangeOf = request.ChangeOf;
                    _MyOrderHistory.ShippingCost = request.ShippingCost;
                    _MyOrderHistory.PaymentDate = DateTime.Now;

                    if(string.IsNullOrWhiteSpace(request.DeliveryDate))
                        _MyOrderHistory.DeliveryDate = DateTime.Now;
                    else
                        _MyOrderHistory.DeliveryDate = DateTime.ParseExact(request.DeliveryDate, "ddMMyyyyHHmm", CultureInfo.InvariantCulture);

                    _MyOrderHistory.DateCreated = DateTime.Now;
                    _MyOrderHistory.Active = true;

                    _MyOrder.MyOrderHistories.Add(_MyOrderHistory);

                    Product prod = ProductBLL.GetById(request.OrderProductList.FirstOrDefault().ProductId);
                    Category category = CategoryBLL.GetById(prod.CategoryID.Value);
                    _MyOrder.SupplierID = category.SupplierID;

                    new MyOrderBLL().Save(_MyOrder);

                    Customer customer = CustomerBLL.GetById(buyerUserValidation.buyer.CustomerID.Value);
                    if(request.PaymentMethod == (int)MyOrderHistoryEnum.PaymentMethod.PediddoCredit)
                    {
                        decimal totalCost = Convert.ToDecimal(_MyOrderHistory.Cost + _MyOrderHistory.ShippingCost);
                        if(totalCost > customer.Credit)
                        {
                            customer.Credit = 0;
                        }
                        else
                        {
                            customer.Credit = customer.Credit - totalCost;
                        }

                        new CustomerBLL().Update(customer);
                    }

                    #region Notificaciones Push...
                    List<Notification> notificationList = new List<Notification>();

                    string[] message = Resources.NotificationPush.N001.Split('#');
                    notificationList.Add(new Notification()
                    {
                        Active = true,
                        Activity = 0,
                        AppID = request.AppID,
                        Channel = buyerUserValidation.buyer.SerialKey,
                        ContentText = message[1],
                        ContentTitle = message[0],
                        DateCreated = DateTime.Now,
                        DateToSend = DateTime.Now,
                        DeviceOS = "1",
                        IsRead = true,
                        Send = false,
                        Type = 1
                    });

                    message = Resources.NotificationPush.N003.Split('#');
                    notificationList.Add(new Notification()
                    {
                        Active = true,
                        Activity = 0,
                        AppID = (int)AppEnum.AppID.PediddoDelivery,
                        Channel = "PEDIDDO-DELIVERY-CHANNEL",
                        ContentText = message[1],
                        ContentTitle = message[0],
                        DateCreated = DateTime.Now,
                        DateToSend = DateTime.Now,
                        DeviceOS = "1",
                        IsRead = true,
                        Send = false,
                        Type = 1
                    });

                    if (request.PaymentMethod == (int)MyOrderHistoryEnum.PaymentMethod.PediddoCredit)
                    {
                        message = Resources.NotificationPush.N012.Replace("{SaldoCredito}", customer.Credit.ToString() + " G$.").Split('#');
                        notificationList.Add(new Notification()
                        {
                            Active = true,
                            Activity = 0,
                            AppID = request.AppID,
                            Channel = buyerUserValidation.buyer.SerialKey,
                            ContentText = message[1],
                            ContentTitle = message[0],
                            DateCreated = DateTime.Now,
                            DateToSend = DateTime.Now,
                            DeviceOS = "1",
                            IsRead = true,
                            Send = false,
                            Type = 1
                        });
                    }
                    new NotificationBLL().BulkSave(notificationList);
                    #endregion
                    

                    break;

                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }
        
        public ReceiveCustomerCreditResponse ReceiveCustomerCredit(ReceiveCustomerCreditRequest request)
        {
            ReceiveCustomerCreditResponse response = new ReceiveCustomerCreditResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);


            if (buyerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk || sellerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk)
            {
                Buyer buyer;
                if (request.BuyerId > 0)
                {
                    buyer = BuyerBLL.GetById(request.BuyerId);
                }
                else
                {
                    buyer = buyerUserValidation.buyer;
                }
                Customer customer = CustomerBLL.GetById(buyer.CustomerID.Value);
                response.CreditAmount = customer.Credit ?? 0;

            }
            else
            {
                ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                response.ErrorCode = error.ErrorCode;
                response.ErrorMessage = error.ErrorMessage;


            }
            return response;
        } 

        public ReceiveCalculateShippingCostResponse ReceiveCalculateShippingCost(ReceiveCalculateShippingCostRequest request)
        {
            ReceiveCalculateShippingCostResponse response = new ReceiveCalculateShippingCostResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);


            if (buyerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk || sellerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk)
            {

                    response.ShippingCost = 5000;
                    Customer customer;

                    if(buyerUserValidation.buyer != null)
                    {
                        customer = CustomerBLL.GetById(buyerUserValidation.buyer.CustomerID.Value);
                    }
                    else
                    {
                        CustomerLocation customerLocation = new CustomerLocationBLL().GetById(request.LocationID);
                        customer = CustomerBLL.GetById(customerLocation.CustomerID);
                    }

                    List<ShippingCostRule> shippingCostRuleList = ShippingCostRuleBLL.GetAll();
                    foreach(ShippingCostRule rule in shippingCostRuleList)
                    {
                        if (rule.ShippingCostRuleId == (int)ShippingCostRuleEnum.ShippingCostRuleId.CustomerWithCredit && rule.Active)
                        {
                            if (customer.Credit > 0)
                            {
                                response.ShippingCost = 0;
                            }
                        }
                        else if (rule.ShippingCostRuleId == (int)ShippingCostRuleEnum.ShippingCostRuleId.VipClient && rule.Active)
                        {
                            if ((customer.Vip ?? false))
                                response.ShippingCost = 0;
                        }
                        else if(rule.ShippingCostRuleId == (int)ShippingCostRuleEnum.ShippingCostRuleId.OrderAmount && rule.Active)
                        {
                            response.FreeShippingAmount = rule.Value.Value;
                        }
                    }
            }
            else
            {
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;
            }

            return response;
        }

        public SendCustomerCreditPurchaseResponse SendCustomerCreditPurchase(SendCustomerCreditPurchaseRequest request)
        {
            SendCustomerCreditPurchaseResponse response = new SendCustomerCreditPurchaseResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    CreditPurchase creditPurchase = new CreditPurchase()
                    {
                        Active = true,
                        Amount = request.CreditAmount,
                        CustomerID = buyerUserValidation.buyer.CustomerID.Value,
                        PaymentMethod = request.PaymentMethod,
                        DateCreated = DateTime.Now,
                        Status = (int)CreditPurchaseEnum.Status.AwaitingAttention
                    };

                    new CreditPurchaseBLL().Save(creditPurchase);

                    #region Notificaciones Push...
                    List<Notification> notificationList = new List<Notification>();

                    string[] message = Resources.NotificationPush.N002.Split('#');
                    notificationList.Add(new Notification()
                    {
                        Active = true,
                        Activity = 0,
                        AppID = request.AppID,
                        Channel = buyerUserValidation.buyer.SerialKey,
                        ContentText = message[1],
                        ContentTitle = message[0],
                        DateCreated = DateTime.Now,
                        DateToSend = DateTime.Now,
                        DeviceOS = "1",
                        IsRead = true,
                        Send = false,
                        Type = 1
                    });

                    message = Resources.NotificationPush.N004.Split('#');
                    notificationList.Add(new Notification()
                    {
                        Active = true,
                        Activity = 0,
                        AppID = (int)AppEnum.AppID.PediddoDelivery,
                        Channel = "PEDIDDO-DELIVERY-CHANNEL",
                        ContentText = message[1],
                        ContentTitle = message[0],
                        DateCreated = DateTime.Now,
                        DateToSend = DateTime.Now,
                        DeviceOS = "1",
                        IsRead = true,
                        Send = false,
                        Type = 1
                    });

                    new NotificationBLL().BulkSave(notificationList);
                    #endregion
                    



                    break;

                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendUpdateCustomerLocationResponse SendUpdateCustomerLocation(SendUpdateCustomerLocationRequest request)
        {
            SendUpdateCustomerLocationResponse response = new SendUpdateCustomerLocationResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            if (buyerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk ||
                sellerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk)
            {

                CustomerLocationBLL customerLocationBLL = new CustomerLocationBLL();

                CustomerLocation customerLocation = customerLocationBLL.GetById(request.LocationID);

                customerLocation.Latitude = request.Latitude;
                customerLocation.Longitude = request.Longitude;
                customerLocation.Description = request.Description;
                customerLocation.Address = request.Address;
                customerLocation.Number = request.Number;
                customerLocation.Complement = request.Complement;
                customerLocation.Locality = request.Locality;
                customerLocation.CityID = request.CityID;
                customerLocation.Reference = request.Reference;

                #region Procesa Imagen...
                if (!string.IsNullOrWhiteSpace(request.Base64StringImage))
                {

                    CloudBlockBlob blockBlob;
                    CloudStorageAccount storageAccount =
                        CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString.Pediddo"]);
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerAppImage"]);

                    AppImage appImageToUpdate = null;
                    string photoName = "";
                    if (customerLocation.AppImageID != null && customerLocation.AppImageID != 0)
                    {
                        appImageToUpdate = AppImageBLL.GetById(customerLocation.AppImageID.Value);
                        photoName = appImageToUpdate.ImageName;

                        blockBlob = container.GetBlockBlobReference(photoName);
                        blockBlob.Delete();

                        appImageToUpdate.Active = false;
                    }

                    Security security = new Security();
                    photoName = security.MD5(request.SerialKey + DateTime.Now.Ticks.ToString()) + ".jpg";
                    byte[] imageBytes = Convert.FromBase64String(request.Base64StringImage);

                    blockBlob = container.GetBlockBlobReference(photoName);
                    using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                    {
                        blockBlob.UploadFromStream(ms);
                    }

                    AppImage appImage = new AppImage()
                    {
                        Active = true,
                        DateCreated = DateTime.Now,
                        ImageName = photoName
                    };



                    AppImageBLL appImageBLL = new AppImageBLL();

                    appImage = appImageBLL.Save(appImage);

                    if (appImageToUpdate != null)
                        appImageBLL.Update(appImageToUpdate);

                    customerLocation.AppImageID = appImage.AppImageID;
                }
                #endregion

                customerLocationBLL.Update(customerLocation);

            }
            else
            {
                ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                response.ErrorCode = error.ErrorCode;
                response.ErrorMessage = error.ErrorMessage;
            }
            return response;
        }

        public ReceiveMyOrderListResponse ReceiveMyOrderList(ReceiveMyOrderListRequest request)
        {
            ReceiveMyOrderListResponse response = new ReceiveMyOrderListResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<MyOrder> myOrderList;
                    if (request.Page == 0)
                    {
                        myOrderList = MyOrderBLL.ListByBuyer(buyerUserValidation.buyer).OrderByDescending(o=>o.DateCreated).ToList();
                    }
                    else
                    {
                        int skip = (request.Page - 1) * 10;
                        myOrderList = MyOrderBLL.ListByBuyerByPage(buyerUserValidation.buyer, skip).OrderByDescending(o=>o.DateCreated).ToList();
                    }

                    response.MyOrderList = (from o in myOrderList
                                           select new ReceiveMyOrderListResponse.MyOrder()
                                           {
                                               DateCreated = o.DateCreated.Value.ToString("ddMMyyyyHHmm", CultureInfo.InvariantCulture),
                                               LocationName = o.MyOrderHistories.OrderByDescending(h => h.DateCreated).FirstOrDefault().CustomerLocation.Description,
                                               MyOrderID = o.MyOrderID,
                                               MyOrderType = o.MyOrderType.Value,
                                               ReceiptCode = o.ReceiptCode,
                                               ShippingCost = o.MyOrderHistories.OrderByDescending(h => h.DateCreated).FirstOrDefault().ShippingCost ?? 0,
                                               Status = o.MyOrderHistories.OrderByDescending(h => h.DateCreated).FirstOrDefault().Status.Value,
                                               TotalAmount = Convert.ToDecimal(o.MyOrderProducts.Sum(p => (p.Quantity * p.Cost)) + (o.MyOrderHistories.OrderByDescending(h => h.DateCreated).FirstOrDefault().ShippingCost ?? 0)),
                                               ProductList = (from op in o.MyOrderProducts
                                                             select new ReceiveMyOrderListResponse.MyOrder.Product()
                                                             {
                                                                 Amount = op.Cost.Value,
                                                                 ProductID = op.ProductID,
                                                                 ProductName = op.Product.Name,
                                                                 Quantity = op.Quantity.Value
                                                             }).ToList()
                                           }).ToList();

                    break;

                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveDealerAuthenticationResponse ReceiveDealerAuthentication(ReceiveDealerAuthenticationRequest request)
        {
            ReceiveDealerAuthenticationResponse response = new ReceiveDealerAuthenticationResponse();

            Security securitypass = new Security();
            string password = securitypass.MD5(request.Password);

            DealerBLL dealerBLL = new DealerBLL();
            Dealer dealer = dealerBLL.Authentication(request.Email, password);
            if (dealer != null)
            {
                if (dealer.Active == true)
                {

                    if (dealer.Employee.SerialKey != null)
                    {
                        EmployeeDevice _EmployeeDevice;
                        Device _Device;
                        _Device = DeviceBLL.GetByDeviceToken(request.UUID);
                        if (_Device == null)
                        {
                            _EmployeeDevice = new EmployeeDevice()
                            {
                                EmployeeID = dealer.Employee.EmployeeID,
                                Active = true,
                                DateCreated = DateTime.Now
                            };

                            _Device = new Device();
                            _Device.DeviceToken = request.UUID;
                            _Device.PushToken = request.UUID;
                            _Device.Status = 0;
                            _Device.DeviceOS = request.DeviceOS;
                            _Device.EmployeeDevices = new EntitySet<EmployeeDevice>();
                            _Device.EmployeeDevices.Add(_EmployeeDevice);
                            _Device.DateCreated = DateTime.Now;
                            _Device.Active = true;

                            _Device = new DeviceBLL().Save(_Device);

                        }
                        else
                        {
                            _EmployeeDevice = EmployeeDeviceBLL.GetByEmployeeDevice(dealer.Employee, _Device);
                            if (_EmployeeDevice == null)
                            {
                                _EmployeeDevice = new EmployeeDevice()
                                {
                                    EmployeeID = dealer.Employee.EmployeeID,
                                    DeviceID = _Device.DeviceID,
                                    DateCreated = DateTime.Now,
                                    Active = true
                                };

                                new EmployeeDeviceBLL().Save(_EmployeeDevice);
                            }
                        }

                        response.Name = (dealer.Employee.FirstName ?? "") + " " + (dealer.Employee.LastName ?? "");
                        response.IsAdmin = dealer.IsAdmin;
                        response.DealerID = dealer.DealerID;
                        response.SupplierID = dealer.Employee.SupplierID;
                        response.SerialKey = dealer.Employee.SerialKey;
                        response.NotificationEndpoint = ConfigurationManager.AppSettings["PediddoNotificationEndpoint"].ToString();
                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E007";
                        response.ErrorMessage = Resources.ErrorRes.E007;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E004";
                    response.ErrorMessage = Resources.ErrorRes.E004;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E001";
                response.ErrorMessage = Resources.ErrorRes.E001;
            }

            return response;
        }

        public ReceiveOrderListResponse ReceiveOrderList(ReceiveOrderListRequest request)
        {
            ReceiveOrderListResponse response = new ReceiveOrderListResponse();
            DealerUserValidator dealerUserValidator = new DealerUserValidator();
            DealerUserValidation dealerUserValidation = dealerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (dealerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<int> statusList = new List<int>()
                    {
                        (int)MyOrderHistoryEnum.Status.AwaitingAttention,
                        (int)MyOrderHistoryEnum.Status.Approved,
                        (int)MyOrderHistoryEnum.Status.GoingToPoint,
                        (int)MyOrderHistoryEnum.Status.PendingApproval,
                        (int)MyOrderHistoryEnum.Status.Processing
                    };

                    List<MyOrder> myOrderList = null;
                    List<spAdminOrderListResult> adminOrderListResult = null;
                    List<spDealerOrderListResult> dealerOrderListResult = null;
                    if(dealerUserValidation.dealer.IsAdmin)
                    {
                        statusList.Add((int)MyOrderHistoryEnum.Status.Nobody);
                        statusList.Add((int)MyOrderHistoryEnum.Status.NotAccepted);
                        statusList.Add((int)MyOrderHistoryEnum.Status.Rejected);

                        List<int> orderTypeList = new List<int>()
                        {
                            (int)MyOrderEnum.Type.Order,
                            (int)MyOrderEnum.Type.OrderSeller,
                            (int)MyOrderEnum.Type.FreeSample
                        };

                        if(request.Option == 0)
                        {
                            adminOrderListResult = MyOrderBLL.ListAdminOrder(dealerUserValidation.dealer.Employee.SupplierID, dealerUserValidation.dealer.DealerID, "0:1:5:7:9:10:", "3:4:5:", DateTime.Now.AddMonths(-1).Date, DateTime.Now.Date, request.Page);
                            //myOrderList = MyOrderBLL.ListBySupplierID(dealerUserValidation.dealer.Employee.SupplierID, dealerUserValidation.dealer.DealerID, statusList, orderTypeList, DateTime.Now.AddMonths(-3).Date, DateTime.Now.Date, request.Page);
                        }
                        else
                        {
                            //myOrderList = MyOrderBLL.ListBySupplierID(dealerUserValidation.dealer.Employee.SupplierID, dealerUserValidation.dealer.DealerID, statusList, orderTypeList, DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(1).Date, request.Page);
                            adminOrderListResult = MyOrderBLL.ListAdminOrder(dealerUserValidation.dealer.Employee.SupplierID, dealerUserValidation.dealer.DealerID, "0:1:5:7:9:10:", "3:4:5:", DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(1).Date, request.Page);
                        }
                        
                    }
                    else
                    {
                        if (request.Option == 0)
                        {
                            dealerOrderListResult = MyOrderBLL.ListDealerOrder(dealerUserValidation.dealer.DealerID, "0:1:7:", DateTime.Now.AddMonths(-1).Date, DateTime.Now.Date, request.Page);
                            //myOrderList = MyOrderBLL.ListByDealerID(dealerUserValidation.dealer.DealerID, statusList, DateTime.Now.AddMonths(-3).Date, DateTime.Now.Date, request.Page);
                        }
                        else
                        {
                            dealerOrderListResult = MyOrderBLL.ListDealerOrder(dealerUserValidation.dealer.DealerID, "0:1:7:", DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(1).Date, request.Page);
                            //myOrderList = MyOrderBLL.ListByDealerID(dealerUserValidation.dealer.DealerID, statusList, DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(1).Date, request.Page);
                        }
                        
                    }

                    if (adminOrderListResult != null)
                    {
                        response.OrderList = (
                                                from o in adminOrderListResult
                                                select new ReceiveOrderListResponse.Order()
                                                {
                                                    Address = o.Address ?? "",
                                                    Client = o.CustomerName + " (" + ((o.FirstName ?? "") + " " + (o.LastName ?? "")) + ")",
                                                    DateOrder = o.DeliveryDate.Value.ToString("ddMMyyyyHHmm", CultureInfo.InvariantCulture),
                                                    OrderID = o.MyOrderID,
                                                    OrderNumber = o.ReceiptCode,
                                                    Status = o.Status.Value,
                                                    OrderType = o.MyOrderType == (int)MyOrderEnum.Type.Order ? "Pedido Cliente" :
                                                                (o.MyOrderType == (int)MyOrderEnum.Type.OrderSeller ? "Pedido Vendedor" :
                                                                "Muestra Gratis"),
                                                    Latitude = o.Latitude,
                                                    Longitude = o.Longitude,
                                                    SellerName = (o.SellerFirstName ?? "") + " " + (o.SellerLastName ?? "")
                                                }
                                             ).ToList();
                    }
                    else
                    {
                        response.OrderList = (
                                                from o in dealerOrderListResult
                                                select new ReceiveOrderListResponse.Order()
                                                {
                                                    Address = o.Address ?? "",
                                                    Client = o.CustomerName + " (" + ((o.FirstName ?? "") + " " + (o.LastName ?? "")) + ")",
                                                    DateOrder = o.DeliveryDate.Value.ToString("ddMMyyyyHHmm", CultureInfo.InvariantCulture),
                                                    OrderID = o.MyOrderID,
                                                    OrderNumber = o.ReceiptCode,
                                                    Status = o.Status.Value,
                                                    OrderType = o.MyOrderType == (int)MyOrderEnum.Type.Order ? "Pedido Cliente" :
                                                                (o.MyOrderType == (int)MyOrderEnum.Type.OrderSeller ? "Pedido Vendedor" :
                                                                "Muestra Gratis"),
                                                    Latitude = o.Latitude,
                                                    Longitude = o.Longitude,
                                                    SellerName = (o.SellerFirstName ?? "") + " " + (o.SellerLastName ?? "")
                                                }
                                             ).ToList();
                    }

                    
                    //response.OrderList = (
                    //                        from o in myOrderList
                    //                        where o.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation != null
                    //                        select new ReceiveOrderListResponse.Order()
                    //                        {
                    //                            Address = o.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation.Address + ", " +
                    //                                        o.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation.Number.ToString(),
                    //                            Client = o.Buyer.Customer.Name + " (" + ((o.Buyer.FirstName ?? "") + " " + (o.Buyer.LastName ?? "")) + ")",
                    //                            DateOrder = o.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().DeliveryDate.Value.ToString("ddMMyyyyHHmm", CultureInfo.InvariantCulture),
                    //                            OrderID = o.MyOrderID,
                    //                            OrderNumber = o.ReceiptCode,
                    //                            Status = o.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().Status.Value,
                    //                            OrderType = o.MyOrderType == (int)MyOrderEnum.Type.Order ? "Pedido Cliente" :
                    //                                        (o.MyOrderType == (int)MyOrderEnum.Type.OrderSeller ? "Pedido Vendedor" :
                    //                                        "Muestra Gratis"),
                    //                            Latitude = o.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation.Latitude,
                    //                            Longitude = o.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation.Longitude,
                    //                            SellerName = (o.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().Seller.Employee.FirstName ?? "") + " " +
                    //                                            (o.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().Seller.Employee.LastName ?? "")
                    //                        }
                    //                        ).ToList();
                    
                    break;

                default:
                    ErrorResponse error = dealerUserValidator.ProcessError(dealerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveBuyerNotificationListResponse ReceiveBuyerNotificationList(ReceiveBuyerNotificationListRequest request)
        {
            ReceiveBuyerNotificationListResponse response = new ReceiveBuyerNotificationListResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<string> listChannel = new List<string>() { buyerUserValidation.buyer.SerialKey };
                    List<Notification> notificationList = NotificationBLL.ListByListChannel(listChannel, request.Page);
                    response.NotificationList = (from n in notificationList
                                                 select new ReceiveBuyerNotificationListResponse.Notification()
                                                 {
                                                     Activity = n.Activity,
                                                     Argument = n.Argument,
                                                     DateCreated = n.DateCreated.ToString("ddMMyyyyHHmm", CultureInfo.InvariantCulture),
                                                     DateToSend = (n.DateToSend == null ? "" : n.DateToSend.ToString("ddMMyyyyHHmm", CultureInfo.InvariantCulture)),
                                                     IsRead = n.IsRead,
                                                     Message = n.ContentText,
                                                     NotificationID = n.NotificationID,
                                                     Title = n.ContentTitle
                                                 }
                                                 ).ToList();

                    break;

                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendBuyerProfilePhotoResponse SendBuyerProfilePhoto(SendBuyerProfilePhotoRequest request)
        {
            SendBuyerProfilePhotoResponse response = new SendBuyerProfilePhotoResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    if(!string.IsNullOrWhiteSpace(request.Base64StringImage))
                    {
                        Buyer _Buyer = buyerUserValidation.buyer;

                        CloudBlockBlob blockBlob;
                        CloudStorageAccount storageAccount =
                                CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString.Pediddo"]);
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerAppImage"]);

                        AppImage appImageToUpdate = null;
                        string imageName;
                        if (_Buyer.AppImageId != null && _Buyer.AppImageId != 0)
                        {
                            appImageToUpdate = AppImageBLL.GetById(_Buyer.AppImageId.Value);
                            imageName = appImageToUpdate.ImageName;

                            blockBlob = container.GetBlockBlobReference(imageName);
                            blockBlob.Delete();

                            appImageToUpdate.Active = false;
                        }

                        Security security = new Security();
                        imageName = security.MD5(request.SerialKey + DateTime.Now.Ticks.ToString()) + ".jpg";
                        byte[] imageBytes = Convert.FromBase64String(request.Base64StringImage);

                        blockBlob = container.GetBlockBlobReference(imageName);
                        using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                        {
                            blockBlob.UploadFromStream(ms);
                        }

                        AppImage appImage = new AppImage()
                        {
                            Active = true,
                            DateCreated = DateTime.Now,
                            ImageName = imageName
                        };

                        AppImageBLL appImageBLL = new AppImageBLL();

                        appImage = appImageBLL.Save(appImage);

                        if (appImageToUpdate != null)
                            appImageBLL.Update(appImageToUpdate);

                        _Buyer.AppImageId = appImage.AppImageID;

                        new BuyerBLL().Update(_Buyer);

                        response.PhotoId = appImage.AppImageID;
                    }


                    break;

                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveContentToShareResponse ReceiveContentToShare(ReceiveContentToShareRequest request)
        {
            ReceiveContentToShareResponse response = new ReceiveContentToShareResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    ContentSharing contentSharing = ContentSharingBLL.GetByCode(ConfigurationManager.AppSettings["ShareStoreLink"].ToString());
                    if (contentSharing != null)
                    {
                        response.Title = contentSharing.Title;
                        response.Body = contentSharing.Body;
                        response.Link = contentSharing.LinkURL + buyerUserValidation.buyer.SerialKey;
                        response.ImageId = contentSharing.AppImageID.Value;
                    }
                   
                    break;

                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendContentToShareResponse SendContentToShare(SendContentToShareRequest request)
        {
            SendContentToShareResponse response = new SendContentToShareResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<EmailNotification> emailNotificationList = new List<EmailNotification>();
                    ContentSharing contentSharing = ContentSharingBLL.GetByCode(request.ContentCode);
                    if (contentSharing != null)
                    {
                        foreach (SendContentToShareRequest.Contact contact in request.ContactList)
                        {
                            bool isEmail = Regex.IsMatch(contact.Value.Trim(), 
                                                         @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                                                         RegexOptions.IgnoreCase);
                            if (isEmail)
                            {
                                ContentSharingLog contentSharingLog = new ContentSharingLog()
                                {
                                    Active = true,
                                    ContactEmail = contact.Value,
                                    ContactName = contact.Name,
                                    ContentSharingId = contentSharing.ContentSharingId,
                                    BuyerID = buyerUserValidation.buyer.BuyerID,
                                    DateCreated = DateTime.Now
                                };

                                contentSharingLog = new ContentSharingLogBLL().Save(contentSharingLog);

                                emailNotificationList.Add(new EmailNotification()
                                {
                                    Active = true,
                                    BodyHtml = contentSharing.Body.Replace("{BuyerName}", ((buyerUserValidation.buyer.FirstName ?? "") + " " + (buyerUserValidation.buyer.LastName ?? "")))
                                                                  .Replace("{NOME}", contact.Name)
                                                                  .Replace("{DATA}", contact.Name + ":" + contact.Value.Trim() + ":" + contentSharingLog.ContentSharingLogId.ToString()),
                                    DateCreated = DateTime.Now,
                                    DateToSend = DateTime.Now,
                                    EmailFrom = ConfigurationManager.AppSettings["VendaEmailAccount"].ToString(),
                                    EmailTo = contact.Value.Trim(),
                                    Send = false,
                                    Subject = contentSharing.Title.Replace("{BuyerName}", ((buyerUserValidation.buyer.FirstName ?? "") + " " + (buyerUserValidation.buyer.LastName ?? "")))
                                });
                            }
                        }
                    }

                    EmailNotificationBLL emailNotificationBLL = new EmailNotificationBLL();
                    foreach(EmailNotification emailNotification in emailNotificationList)
                    {
                        emailNotificationBLL.Save(emailNotification);
                    }

                    break;

                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveAdviceResponse ReceiveAdvice(ReceiveAdviceRequest request)
        {
            ReceiveAdviceResponse response = new ReceiveAdviceResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<Advice> adviceList = AdviceBLL.ListNotRead(buyerUserValidation.buyer.BuyerID);
                    if (adviceList.Count > 0)
                    {
                        Advice adviceToShow = adviceList.FirstOrDefault(a => a.Highlight == true);
                        if (adviceToShow == null)
                        {
                            adviceToShow = adviceList.OrderBy(a => a.DateCreated).FirstOrDefault();
                        }

                        if (adviceToShow != null)
                        {
                            response.AdviceId = adviceToShow.AdviceId;
                            response.Title = adviceToShow.Title;
                            response.Call = adviceToShow.Call;
                            response.Body = adviceToShow.Body;
                            response.AdviceButtonList = new List<ReceiveAdviceResponse.AdviceButton>();
                            response.ImageId = adviceToShow.AppImageId ?? 0;
                            response.Date = adviceToShow.DateCreated.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                            if (!string.IsNullOrWhiteSpace(adviceToShow.Argument))
                            {
                                Product product = ProductBLL.GetById(Convert.ToInt32(adviceToShow.Argument));
                                response.ProductID = product.ProductID;
                                response.ProductName = product.Name;
                                response.ProductPrice = product.Cost;
                            }

                            foreach (AdviceButton button in adviceToShow.AdviceButtons)
                            {
                                response.AdviceButtonList.Add(new ReceiveAdviceResponse.AdviceButton()
                                {
                                    Id = button.ButtonId,
                                    Text = button.Text
                                });
                            }
                        }
                    }

                    break;

                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendAdviceAnswerResponse SendAdviceAnswer(SendAdviceAnswerRequest request)
        {
            SendAdviceAnswerResponse response = new SendAdviceAnswerResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    Advice currentAdvice = AdviceBLL.GetById(request.AdviceId);
                    if (currentAdvice != null)
                    {
                        BuyerAdviceLog buyerAdviceLog = new BuyerAdviceLog()
                        {
                            Active = true,
                            AdviceId = currentAdvice.AdviceId,
                            ButtonId = request.Answer,
                            BuyerID = buyerUserValidation.buyer.BuyerID,
                            DateCreated = DateTime.Now
                        };

                        new BuyerAdviceLogBLL().Save(buyerAdviceLog);

                    }

                    break;

                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveAdviceListResponse ReceiveAdviceList(ReceiveAdviceListRequest request)
        {
            ReceiveAdviceListResponse response = new ReceiveAdviceListResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<Advice> adviceList;
                    if (request.Page == 0)
                    {
                        adviceList = AdviceBLL.ListAdvice(buyerUserValidation.buyer.BuyerID);
                    }
                    else
                    {
                        int skip = (request.Page - 1) * 10;
                        adviceList = AdviceBLL.ListAdvice(buyerUserValidation.buyer.BuyerID, skip);
                    }

                    if (adviceList.Count > 0)
                    {
                        response.AdviceList = new List<ReceiveAdviceListResponse.Advice>();
                        ReceiveAdviceListResponse.Advice advice;
                        foreach (Advice adviceItem in adviceList)
                        {
                            advice = new ReceiveAdviceListResponse.Advice();
                            advice.AdviceId = adviceItem.AdviceId;
                            advice.Title = adviceItem.Title;
                            advice.Call = adviceItem.Call;
                            advice.Body = adviceItem.Body;
                            advice.Date = adviceItem.DateCreated.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                            advice.ImageId = adviceItem.AppImageId ?? 0;
                            advice.AdviceButtonList = new List<ReceiveAdviceListResponse.Advice.AdviceButton>();

                            if (!string.IsNullOrWhiteSpace(adviceItem.Argument))
                            {
                                Product product = ProductBLL.GetById(Convert.ToInt32(adviceItem.Argument));
                                advice.ProductID = product.ProductID;
                                advice.ProductName = product.Name;
                                advice.ProductPrice = product.Cost;
                            }

                            foreach (AdviceButton button in adviceItem.AdviceButtons)
                            {
                                advice.AdviceButtonList.Add(new ReceiveAdviceListResponse.Advice.AdviceButton()
                                {
                                    Id = button.ButtonId,
                                    Text = button.Text
                                });
                            }
                            response.AdviceList.Add(advice);
                        }
                    }

                    break;

                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public Stream ReceiveBannerImage(string Serial, string Type)
        {
            MemoryStream response = new MemoryStream();
            byte[] byteArray;

            if (Serial.Equals(ConfigurationManager.AppSettings["BannerSerialKey"].ToString()))
            {

                AppImage image = AppImageBLL.GetById(5603);
                if (image != null)
                {
                    if (!string.IsNullOrWhiteSpace(image.ImageName))
                    {
                        CloudStorageAccount storageAccount =
                        CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString.Pediddo"]);
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerAppImage"]);

                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(image.ImageName);

                        WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpg";
                        blockBlob.DownloadToStream(response);
                        response.Position = 0;
                    }
                }
                else
                {
                    byteArray = Encoding.UTF8.GetBytes(" La imagen solicitada no existe.");
                    response = new MemoryStream(byteArray);
                }
            }
            else
            {
                byteArray = Encoding.UTF8.GetBytes(" Acceso denegado.");
                response = new MemoryStream(byteArray);
            }


            return response;
        }

        public ReceiveOrderResponse ReceiveOrder(ReceiveOrderRequest request)
        {
            ReceiveOrderResponse response = new ReceiveOrderResponse();
            DealerUserValidator dealerUserValidator = new DealerUserValidator();
            DealerUserValidation dealerUserValidation = dealerUserValidator.Validate(request.SerialKey, request.UUID);

            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            if (dealerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk ||
                sellerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk)
            {

                    MyOrder myOrder = MyOrderBLL.GetById(request.OrderId);

                    response.Address = myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation.Address + ", " +
                                       myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation.Number.ToString() + " - " +
                                       myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation.Complement + " (" +
                                       myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation.Reference + ")";

                    if (myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation.City != null)
                        response.City = myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation.City.CityName;
                    response.Client = myOrder.Buyer.Customer.Name + " (" + ((myOrder.Buyer.FirstName ?? "") + " " + (myOrder.Buyer.LastName ?? "")) + ")";
                    response.ClientEmail = myOrder.Buyer.Email;
                    response.ClientPhone = myOrder.Buyer.PhoneNumber;
                    response.DateOrder = myOrder.DateCreated.Value.ToString("ddMMyyyyHHmm", CultureInfo.InvariantCulture);
                    response.Locality = myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation.Locality;
                    response.OrderID = myOrder.MyOrderID;
                    response.OrderNumber = myOrder.ReceiptCode;
                    response.SellerName = myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().SellerID == null ?
                        "Aplicación móvil" :
                        ((myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().Seller.Employee.FirstName ?? "") + " " +
                        (myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().Seller.Employee.LastName ?? ""));
                    response.SellerPhone = myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().SellerID == null ?
                        "" :
                        (myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().Seller.Employee.PhoneNumber ?? "");
                    response.ShippingCost = myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().ShippingCost ?? 0;
                    response.StatusId = myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().Status.Value;
                    response.TotalAmount = Convert.ToDecimal(myOrder.MyOrderProducts.Sum(p => (p.Quantity * p.Product.Cost)) + response.ShippingCost);
                    response.ImageId = myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation.AppImageID ?? 0;
                    response.Latitude = myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation.Latitude;
                    response.Longitude = myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().CustomerLocation.Longitude;


                    string efectivo = "Efectivo";
                    if ((myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().ChangeOf ?? 0) > 0)
                    {
                        efectivo = efectivo + " - (Vuelto de: " + (myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().ChangeOf ?? 0).ToString("N") + ")";
                    }


                    response.PayMethod =
                        myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().PaymentMethod == (int)MyOrderHistoryEnum.PaymentMethod.Cash ? efectivo :
                        (myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().PaymentMethod == (int)MyOrderHistoryEnum.PaymentMethod.DebitCard ? "Tarj. Debito/Credito" :
                        (myOrder.MyOrderHistories.OrderByDescending(ord => ord.DateCreated).FirstOrDefault().PaymentMethod == (int)MyOrderHistoryEnum.PaymentMethod.PediddoCredit ? "Pre Pago" : efectivo));

                    response.ProductList = (from p in myOrder.MyOrderProducts
                                            select new ReceiveOrderResponse.Product()
                                            {
                                                Amount = p.Product.Cost,
                                                ProductID = p.ProductID,
                                                Name = p.Product.Name,
                                                Quantity = p.Quantity.Value,
                                                UnitMeasure = p.Product.Udm,
                                                Manufacturer = p.Product.Manufacturer
                                            }).ToList();
            }
            else
            {
                ErrorResponse error = dealerUserValidator.ProcessError(dealerUserValidation.code);
                response.ErrorCode = error.ErrorCode;
                response.ErrorMessage = error.ErrorMessage;

            }
            return response;
        }

        public ReceiveListDealerResponse ReceiveListDealer(ReceiveListDealerRequest request)
        {
            ReceiveListDealerResponse response = new ReceiveListDealerResponse();
            DealerUserValidator dealerUserValidator = new DealerUserValidator();
            DealerUserValidation dealerUserValidation = dealerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (dealerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<Dealer> dealerList;
                    if(request.Page == 0)
                    {
                        dealerList = DealerBLL.ListBySupplierID(dealerUserValidation.dealer.Employee.SupplierID);
                    }
                    else
                    {
                        int skip = (request.Page - 1) * 10;
                        dealerList = DealerBLL.ListBySupplierID(dealerUserValidation.dealer.Employee.SupplierID, skip);
                    }

                    response.DealerList = (
                                            from d in dealerList
                                            select new ReceiveListDealerResponse.Dealer()
                                            {
                                                DealerId = d.DealerID,
                                                Name = ((d.Employee.FirstName ?? "") + " " + (d.Employee.LastName ?? ""))
                                            }
                                          ).ToList();

                    break;

                default:
                    ErrorResponse error = dealerUserValidator.ProcessError(dealerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendAssignOrderResponse SendAssignOrder(SendAssignOrderRequest request)
        {
            SendAssignOrderResponse response = new SendAssignOrderResponse();
            DealerUserValidator dealerUserValidator = new DealerUserValidator();
            DealerUserValidation dealerUserValidation = dealerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (dealerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    Dealer dealer = DealerBLL.GetById(request.DealerId);
                    MyOrderHistory lastMyOrderHistory = MyOrderHistoryBLL.GetLast(request.OrderId);

                    MyOrderHistory myOrderHistory = new MyOrderHistory()
                    {
                        Active = lastMyOrderHistory.Active,
                        ChangeOf = lastMyOrderHistory.ChangeOf,
                        Cost = lastMyOrderHistory.Cost,
                        CustomerLocationID = lastMyOrderHistory.CustomerLocationID,
                        DateCreated = DateTime.Now,
                        DealerID = dealer.DealerID,
                        DeliveryDate = lastMyOrderHistory.DeliveryDate,
                        InteractionID = (int)MyOrderHistoryEnum.InteractionID.Assignment,
                        InvoiceNumber = lastMyOrderHistory.InvoiceNumber,
                        InvoiceDate = lastMyOrderHistory.InvoiceDate,
                        MyOrderID = lastMyOrderHistory.MyOrderID,
                        NotificationID = lastMyOrderHistory.NotificationID,
                        Observation = lastMyOrderHistory.Observation,
                        PaymentDate = lastMyOrderHistory.PaymentDate,
                        PaymentMethod = lastMyOrderHistory.PaymentMethod,
                        PayMode = lastMyOrderHistory.PayMode,
                        SellerID = lastMyOrderHistory.SellerID,
                        ShippingCost = lastMyOrderHistory.ShippingCost,
                        Status = (int)MyOrderHistoryEnum.Status.Processing,
                        InteractorEmployeeId = dealerUserValidation.dealer.DealerID,
                        TrackPointID = lastMyOrderHistory.TrackPointID,
                        Receiver = lastMyOrderHistory.Receiver,
                        AppImageID = lastMyOrderHistory.AppImageID
                    };

                    new MyOrderHistoryBLL().Save(myOrderHistory);

                    MyOrder myOrder = MyOrderBLL.GetById(request.OrderId);
                    
                    #region Notificaciones Push...
                    List<Notification> notificationList = new List<Notification>();

                    string[] message = Resources.NotificationPush.N005.Split('#');
                    notificationList.Add(new Notification()
                    {
                        Active = true,
                        Activity = 0,
                        AppID = (int)AppEnum.AppID.PediddoNew,
                        Channel = myOrder.Buyer.SerialKey,
                        ContentText = message[1],
                        ContentTitle = message[0],
                        DateCreated = DateTime.Now,
                        DateToSend = DateTime.Now,
                        DeviceOS = "1",
                        IsRead = true,
                        Send = false,
                        Type = 1
                    });

                    message = Resources.NotificationPush.N014.Split('#');
                    notificationList.Add(new Notification()
                    {
                        Active = true,
                        Activity = 0,
                        AppID = (int)AppEnum.AppID.PediddoDelivery,
                        Channel = dealer.Employee.SerialKey,
                        ContentText = message[1],
                        ContentTitle = message[0],
                        DateCreated = DateTime.Now,
                        DateToSend = DateTime.Now,
                        DeviceOS = "1",
                        IsRead = true,
                        Send = false,
                        Type = 1
                    });

                    message = Resources.NotificationPush.N016
                                       .Replace("CustomerName", myOrder.Buyer.Customer.Name)
                                       .Replace("DateOrder", myOrder.DateCreated.Value.AddHours(-4).ToString("dd/MM/yyyy HH:mm"))
                                       .Replace("DealerName", ((dealer.Employee.FirstName ?? "") + " " + (dealer.Employee.LastName ?? "")))
                                       .Split('#');
                    notificationList.Add(new Notification()
                    {
                        Active = true,
                        Activity = 0,
                        AppID = (int)AppEnum.AppID.PediddoPlus,
                        Channel = lastMyOrderHistory.Seller.Employee.SerialKey,
                        ContentText = message[1],
                        ContentTitle = message[0],
                        DateCreated = DateTime.Now,
                        DateToSend = DateTime.Now,
                        DeviceOS = "1",
                        IsRead = true,
                        Send = false,
                        Type = 1
                    });

                    new NotificationBLL().BulkSave(notificationList);
                    #endregion

                    #region Envio de Email...
                    EmailNotification emailNotification = null;

                    if (myOrder.MyOrderType != (int)MyOrderEnum.Type.Order)
                    {
                        emailNotification = new EmailNotification()
                        {
                            Active = true,
                            BodyText = "El Pedido para su cliente: " + myOrder.Buyer.Customer.Name +
                                        " generado el: " + myOrder.DateCreated.Value.AddHours(-4).ToString("dd/MM/yyyy HH:mm") +
                                        " fue Asignado al repartidor: " + (dealer.Employee.FirstName ?? "") + " " + (dealer.Employee.LastName ?? ""),
                            DateCreated = DateTime.Now,
                            DateToSend = DateTime.Now,
                            EmailFrom = ConfigurationManager.AppSettings["SoporteEmailAccount"].ToString(),
                            EmailTo = myOrder.MyOrderHistories.OrderByDescending(d => d.DateCreated).FirstOrDefault().Seller.Employee.Email,
                            Send = false,
                            Subject = "Pediddo - Hubo una interaccion en un pedido"
                        };
                        new EmailNotificationBLL().Save(emailNotification);
                    }
                        
                    #endregion

                    break;

                default:
                    ErrorResponse error = dealerUserValidator.ProcessError(dealerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendCreateOrderInteractionResponse SendCreateOrderInteraction(SendCreateOrderInteractionRequest request)
        {
            SendCreateOrderInteractionResponse response = new SendCreateOrderInteractionResponse();
            DealerUserValidator dealerUserValidator = new DealerUserValidator();
            DealerUserValidation dealerUserValidation = dealerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (dealerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    TrackPoint trackpoint = new TrackPoint()
                    {
                        Active = true,
                        DateCreated = DateTime.Now,
                        Latitude = request.Latitude,
                        Longitude = request.Longitude,
                        TrackPointTypeID = (int)TrackPointTypeEnum.TrackPointTypeID.MyOrderInteraction
                    };

                    MyOrderHistory lastMyOrderHistory = MyOrderHistoryBLL.GetLast(request.OrderId);
                    MyOrderHistory myOrderHistory = new MyOrderHistory()
                    {
                        Active = lastMyOrderHistory.Active,
                        ChangeOf = lastMyOrderHistory.ChangeOf,
                        Cost = lastMyOrderHistory.Cost,
                        CustomerLocationID = lastMyOrderHistory.CustomerLocationID,
                        DateCreated = DateTime.Now,
                        DealerID = lastMyOrderHistory.DealerID,
                        DeliveryDate = lastMyOrderHistory.DeliveryDate,
                        InteractionID = (int)MyOrderHistoryEnum.InteractionID.Delivery,
                        InvoiceNumber = lastMyOrderHistory.InvoiceNumber,
                        InvoiceDate = lastMyOrderHistory.InvoiceDate,
                        MyOrderID = lastMyOrderHistory.MyOrderID,
                        NotificationID = lastMyOrderHistory.NotificationID,
                        Observation = request.Observation,
                        PaymentDate = lastMyOrderHistory.PaymentDate,
                        PaymentMethod = lastMyOrderHistory.PaymentMethod,
                        PayMode = lastMyOrderHistory.PayMode,
                        SellerID = lastMyOrderHistory.SellerID,
                        ShippingCost = lastMyOrderHistory.ShippingCost,
                        Status = request.ResultId,
                        InteractorEmployeeId = dealerUserValidation.dealer.DealerID,
                        TrackPoint = trackpoint,
                        Receiver = request.Receiver
                    };


                    if (!string.IsNullOrWhiteSpace(request.Base64StringImage))
                    {

                        CloudBlockBlob blockBlob;
                        CloudStorageAccount storageAccount =
                                CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString.Pediddo"]);
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerAppImage"]);

                        string imageName;

                        Security security = new Security();
                        imageName = security.MD5(request.SerialKey + DateTime.Now.Ticks.ToString()) + ".jpg";
                        byte[] imageBytes = Convert.FromBase64String(request.Base64StringImage);

                        blockBlob = container.GetBlockBlobReference(imageName);
                        using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                        {
                            blockBlob.UploadFromStream(ms);
                        }

                        AppImage appImage = new AppImage()
                        {
                            Active = true,
                            DateCreated = DateTime.Now,
                            ImageName = imageName
                        };

                        myOrderHistory.AppImage = appImage;
                    }

                    new MyOrderHistoryBLL().Save(myOrderHistory);

                    #region Notificaciones Push...

                    MyOrder myOrder = MyOrderBLL.GetById(request.OrderId);
                    List<Notification> notificationList = new List<Notification>();

                    string[] message = null;
                    string[] messageSeller = null;

                    string customerName = myOrder.Buyer.Customer.Name;
                    string dateOrder = myOrder.DateCreated.Value.AddHours(-4).Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    switch(request.ResultId)
                    {
                        case (int)MyOrderHistoryEnum.Status.GoingToPoint: 
                            message = Resources.NotificationPush.N006.Split('#');
                            messageSeller = (Resources.NotificationPush.N018)
                                            .Replace("{CustomerName}", customerName)
                                            .Replace("{DateOrder}", dateOrder)
                                            .Split('#');
                            break;
                        case (int)MyOrderHistoryEnum.Status.Nobody:
                            message = Resources.NotificationPush.N008.Replace("{StatusPedido}", "No había nadie para recibir").Split('#');
                            messageSeller = (Resources.NotificationPush.N019)
                                            .Replace("{CustomerName}", customerName)
                                            .Replace("{DateOrder}", dateOrder)
                                            .Replace("{StatusPedido}", "No había nadie para recibir")
                                            .Split('#');
                            break;
                        case (int)MyOrderHistoryEnum.Status.NotAccepted: 
                            message = Resources.NotificationPush.N008.Replace("{StatusPedido}", "El Pedido no fue aceptado").Split('#');
                            messageSeller = (Resources.NotificationPush.N019)
                                            .Replace("{CustomerName}", customerName)
                                            .Replace("{DateOrder}", dateOrder)
                                            .Replace("{StatusPedido}", "El Pedido no fue aceptado")
                                            .Split('#');
                            break;
                        case (int)MyOrderHistoryEnum.Status.Delivered:
                            message = Resources.NotificationPush.N007.Split('#');
                            messageSeller = (Resources.NotificationPush.N020)
                                            .Replace("{CustomerName}", customerName)
                                            .Replace("{DateOrder}", dateOrder)
                                            .Split('#');
                            break;
                        default:
                            message = Resources.NotificationPush.N007.Split('#');
                            messageSeller = (Resources.NotificationPush.N020)
                                            .Replace("{CustomerName}", customerName)
                                            .Replace("{DateOrder}", dateOrder)
                                            .Split('#');
                            break;
                    }

                    notificationList.Add(new Notification()
                    {
                        Active = true,
                        Activity = 0,
                        AppID = (int)AppEnum.AppID.PediddoNew,
                        Channel = myOrder.Buyer.SerialKey,
                        ContentText = message[1],
                        ContentTitle = message[0],
                        DateCreated = DateTime.Now,
                        DateToSend = DateTime.Now,
                        DeviceOS = "1",
                        IsRead = true,
                        Send = false,
                        Type = 1
                    });

                    if (myOrder.MyOrderType != (int)MyOrderEnum.Type.Order)
                    {
                        notificationList.Add(new Notification()
                        {
                            Active = true,
                            Activity = 0,
                            AppID = (int)AppEnum.AppID.PediddoPlus,
                            Channel = lastMyOrderHistory.Seller.Employee.SerialKey,
                            ContentText = messageSeller[1],
                            ContentTitle = messageSeller[0],
                            DateCreated = DateTime.Now,
                            DateToSend = DateTime.Now,
                            DeviceOS = "1",
                            IsRead = true,
                            Send = false,
                            Type = 1
                        });
                    }
                    new NotificationBLL().BulkSave(notificationList);
                    #endregion

                    break;

                default:
                    ErrorResponse error = dealerUserValidator.ProcessError(dealerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendAdjustCustomerLocationResponse SendAdjustCustomerLocation(SendAdjustCustomerLocationRequest request)
        {
            SendAdjustCustomerLocationResponse response = new SendAdjustCustomerLocationResponse();
            DealerUserValidator dealerUserValidator = new DealerUserValidator();
            DealerUserValidation dealerUserValidation = dealerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (dealerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    CustomerLocation customerLocation;
                    if (request.OrderId != 0)
                    {
                        MyOrder myOrder = MyOrderBLL.GetById(request.OrderId);
                        customerLocation = myOrder.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().CustomerLocation;
                    }
                    else
                    {
                        customerLocation = new CustomerLocationBLL().GetById(request.LocationId);
                    }


                    bool upd = false;

                    if(request.Latitude != 0 && request.Longitude != 0)
                    {
                        customerLocation.Latitude = request.Latitude;
                        customerLocation.Longitude = request.Longitude;

                        upd = true;
                    }

                    #region Procesa Imagen...
                    if (!string.IsNullOrWhiteSpace(request.Base64StringImage))
                    {

                        CloudBlockBlob blockBlob;
                        CloudStorageAccount storageAccount =
                            CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString.Pediddo"]);
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerAppImage"]);

                        AppImage appImageToUpdate = null;
                        string photoName = "";
                        if (customerLocation.AppImageID != null && customerLocation.AppImageID != 0)
                        {
                            appImageToUpdate = AppImageBLL.GetById(customerLocation.AppImageID.Value);
                            photoName = appImageToUpdate.ImageName;

                            blockBlob = container.GetBlockBlobReference(photoName);
                            blockBlob.Delete();

                            appImageToUpdate.Active = false;
                        }

                        Security security = new Security();
                        photoName = security.MD5(request.SerialKey + DateTime.Now.Ticks.ToString()) + ".jpg";
                        byte[] imageBytes = Convert.FromBase64String(request.Base64StringImage);

                        blockBlob = container.GetBlockBlobReference(photoName);
                        using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                        {
                            blockBlob.UploadFromStream(ms);
                        }

                        AppImage appImage = new AppImage()
                        {
                            Active = true,
                            DateCreated = DateTime.Now,
                            ImageName = photoName
                        };



                        AppImageBLL appImageBLL = new AppImageBLL();

                        appImage = appImageBLL.Save(appImage);

                        if (appImageToUpdate != null)
                            appImageBLL.Update(appImageToUpdate);

                        customerLocation.AppImageID = appImage.AppImageID;

                        upd = true;
                    }
                    #endregion

                    if(upd)
                        new CustomerLocationBLL().Update(customerLocation);

                    break;

                default:
                    ErrorResponse error = dealerUserValidator.ProcessError(dealerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendDeliveryHelpResponse SendDeliveryHelp(SendDeliveryHelpRequest request)
        {
            SendDeliveryHelpResponse response = new SendDeliveryHelpResponse();
            DealerUserValidator dealerUserValidator = new DealerUserValidator();
            DealerUserValidation dealerUserValidation = dealerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (dealerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    MyOrder myOrder = MyOrderBLL.GetById(request.OrderId);
                    Seller seller = myOrder.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Seller;

                    List<Notification> notificationList = new List<Notification>();
                    string[] message = ("Ayuda!.#El Repartidor " + (dealerUserValidation.dealer.Employee.FirstName ?? "") + " " + (dealerUserValidation.dealer.Employee.FirstName ?? "") + " necesita comunicarse contigo.!").Split('#');

                    if(seller != null)
                    {
                        string emailTo = seller.Employee.Email;
                        EmailNotification emailNotification = new EmailNotification()
                        {
                            Active = true,
                            BodyText = "El Repartidor " + (dealerUserValidation.dealer.Employee.FirstName ?? "") + " " + (dealerUserValidation.dealer.Employee.FirstName ?? "") +
                            " necesita comunicarse contigo.!",
                            DateCreated = DateTime.Now,
                            DateToSend = DateTime.Now,
                            EmailFrom = ConfigurationManager.AppSettings["SoporteEmailAccount"].ToString(),
                            EmailTo = emailTo,
                            Send = false,
                            Subject = "Japo Entrega - Ayuda!."
                        };

                        new EmailNotificationBLL().Save(emailNotification);

                        notificationList.Add(new Notification()
                        {
                            Active = true,
                            Activity = 0,
                            AppID = (int)AppEnum.AppID.PediddoPlus,
                            Channel = seller.Employee.SerialKey,
                            ContentText = message[1],
                            ContentTitle = message[0],
                            DateCreated = DateTime.Now,
                            DateToSend = DateTime.Now,
                            DeviceOS = "1",
                            IsRead = true,
                            Send = false,
                            Type = 1
                        });
                    }

                    List<Seller> sellerAdminList = SellerBLL.ListAdmin(seller.Employee.SupplierID);
                    foreach(Seller sellerItem in sellerAdminList)
                    {
                        notificationList.Add(new Notification()
                        {
                            Active = true,
                            Activity = 0,
                            AppID = (int)AppEnum.AppID.PediddoDelivery,
                            Channel = sellerItem.Employee.SerialKey,
                            ContentText = message[1],
                            ContentTitle = message[0],
                            DateCreated = DateTime.Now,
                            DateToSend = DateTime.Now,
                            DeviceOS = "1",
                            IsRead = true,
                            Send = false,
                            Type = 1
                        });
                    }

                    new NotificationBLL().BulkSave(notificationList);

                    break;

                default:
                    ErrorResponse error = dealerUserValidator.ProcessError(dealerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveSellerAdviceResponse ReceiveSellerAdvice(ReceiveSellerAdviceRequest request)
        {
            ReceiveSellerAdviceResponse response = new ReceiveSellerAdviceResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<EmployeeAdvice> adviceList = EmployeeAdviceBLL.ListNotRead(sellerUserValidation.seller.SellerID, (int)EmployeeAdviceEnum.EmployeeType.Seller);
                    if (adviceList.Count > 0)
                    {
                        EmployeeAdvice adviceToShow = adviceList.FirstOrDefault(a => a.Highlight == true);
                        if (adviceToShow == null)
                        {
                            adviceToShow = adviceList.OrderBy(a => a.DateCreated).FirstOrDefault();
                        }

                        if (adviceToShow != null)
                        {
                            response.AdviceId = adviceToShow.EmployeeAdviceId;
                            response.Title = adviceToShow.Title;
                            response.Call = adviceToShow.Call;
                            response.Body = adviceToShow.Body;
                            response.AdviceButtonList = new List<ReceiveSellerAdviceResponse.AdviceButton>();
                            response.ImageId = adviceToShow.AppImageId ?? 0;
                            response.Date = adviceToShow.DateCreated.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                            if (!string.IsNullOrWhiteSpace(adviceToShow.Argument))
                            {
                                Product product = ProductBLL.GetById(Convert.ToInt32(adviceToShow.Argument));
                                response.ProductID = product.ProductID;
                                response.ProductName = product.Name;
                                response.ProductPrice = product.Cost;
                            }

                            foreach (EmployeeAdviceButton button in adviceToShow.EmployeeAdviceButtons)
                            {
                                response.AdviceButtonList.Add(new ReceiveSellerAdviceResponse.AdviceButton()
                                {
                                    Id = button.ButtonId,
                                    Text = button.Text
                                });
                            }
                        }
                    }

                    break;

                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendSellerAdviceAnswerResponse SendSellerAdviceAnswer(SendSellerAdviceAnswerRequest request)
        {
            SendSellerAdviceAnswerResponse response = new SendSellerAdviceAnswerResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    EmployeeAdvice currentAdvice = EmployeeAdviceBLL.GetById(request.AdviceId);
                    if (currentAdvice != null)
                    {
                        EmployeeAdviceLog employeeAdviceLog = new EmployeeAdviceLog()
                        {
                            Active = true,
                            EmployeeAdviceId = currentAdvice.EmployeeAdviceId,
                            ButtonId = request.Answer,
                            EmployeeID = sellerUserValidation.seller.SellerID,
                            DateCreated = DateTime.Now
                        };

                        new EmployeeAdviceLogBLL().Save(employeeAdviceLog);

                    }

                    break;

                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveSellerAdviceListResponse ReceiveSellerAdviceList(ReceiveSellerAdviceListRequest request)
        {
            ReceiveSellerAdviceListResponse response = new ReceiveSellerAdviceListResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<EmployeeAdvice> adviceList;
                    if (request.Page == 0)
                    {
                        adviceList = EmployeeAdviceBLL.ListAdvice(sellerUserValidation.seller.SellerID, (int)EmployeeAdviceEnum.EmployeeType.Seller);
                    }
                    else
                    {
                        int skip = (request.Page - 1) * 10;
                        adviceList = EmployeeAdviceBLL.ListAdvice(sellerUserValidation.seller.SellerID, (int)EmployeeAdviceEnum.EmployeeType.Seller, skip);
                    }

                    if (adviceList.Count > 0)
                    {
                        response.AdviceList = new List<ReceiveSellerAdviceListResponse.Advice>();
                        ReceiveSellerAdviceListResponse.Advice advice;
                        foreach (EmployeeAdvice adviceItem in adviceList)
                        {
                            advice = new ReceiveSellerAdviceListResponse.Advice();
                            advice.AdviceId = adviceItem.EmployeeAdviceId;
                            advice.Title = adviceItem.Title;
                            advice.Call = adviceItem.Call;
                            advice.Body = adviceItem.Body;
                            advice.Date = adviceItem.DateCreated.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                            advice.ImageId = adviceItem.AppImageId ?? 0;
                            advice.AdviceButtonList = new List<ReceiveSellerAdviceListResponse.Advice.AdviceButton>();

                            if (!string.IsNullOrWhiteSpace(adviceItem.Argument))
                            {
                                Product product = ProductBLL.GetById(Convert.ToInt32(adviceItem.Argument));
                                advice.ProductID = product.ProductID;
                                advice.ProductName = product.Name;
                                advice.ProductPrice = product.Cost;
                            }

                            foreach (EmployeeAdviceButton button in adviceItem.EmployeeAdviceButtons)
                            {
                                advice.AdviceButtonList.Add(new ReceiveSellerAdviceListResponse.Advice.AdviceButton()
                                {
                                    Id = button.ButtonId,
                                    Text = button.Text
                                });
                            }
                            response.AdviceList.Add(advice);
                        }
                    }

                    break;

                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveDealerAdviceResponse ReceiveDealerAdvice(ReceiveDealerAdviceRequest request)
        {
            ReceiveDealerAdviceResponse response = new ReceiveDealerAdviceResponse();
            DealerUserValidator dealerUserValidator = new DealerUserValidator();
            DealerUserValidation dealerUserValidation = dealerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (dealerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<EmployeeAdvice> adviceList = EmployeeAdviceBLL.ListNotRead(dealerUserValidation.dealer.DealerID, (int)EmployeeAdviceEnum.EmployeeType.Dealer);
                    if (adviceList.Count > 0)
                    {
                        EmployeeAdvice adviceToShow = adviceList.FirstOrDefault(a => a.Highlight == true);
                        if (adviceToShow == null)
                        {
                            adviceToShow = adviceList.OrderBy(a => a.DateCreated).FirstOrDefault();
                        }

                        if (adviceToShow != null)
                        {
                            response.AdviceId = adviceToShow.EmployeeAdviceId;
                            response.Title = adviceToShow.Title;
                            response.Call = adviceToShow.Call;
                            response.Body = adviceToShow.Body;
                            response.AdviceButtonList = new List<ReceiveDealerAdviceResponse.AdviceButton>();
                            response.ImageId = adviceToShow.AppImageId ?? 0;
                            response.Date = adviceToShow.DateCreated.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                            if (!string.IsNullOrWhiteSpace(adviceToShow.Argument))
                            {
                                Product product = ProductBLL.GetById(Convert.ToInt32(adviceToShow.Argument));
                                response.ProductID = product.ProductID;
                                response.ProductName = product.Name;
                                response.ProductPrice = product.Cost;
                            }

                            foreach (EmployeeAdviceButton button in adviceToShow.EmployeeAdviceButtons)
                            {
                                response.AdviceButtonList.Add(new ReceiveDealerAdviceResponse.AdviceButton()
                                {
                                    Id = button.ButtonId,
                                    Text = button.Text
                                });
                            }
                        }
                    }

                    break;

                default:
                    ErrorResponse error = dealerUserValidator.ProcessError(dealerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendDealerAdviceAnswerResponse SendDealerAdviceAnswer(SendDealerAdviceAnswerRequest request)
        {
            SendDealerAdviceAnswerResponse response = new SendDealerAdviceAnswerResponse();
            DealerUserValidator dealerUserValidator = new DealerUserValidator();
            DealerUserValidation dealerUserValidation = dealerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (dealerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    EmployeeAdvice currentAdvice = EmployeeAdviceBLL.GetById(request.AdviceId);
                    if (currentAdvice != null)
                    {
                        EmployeeAdviceLog employeeAdviceLog = new EmployeeAdviceLog()
                        {
                            Active = true,
                            EmployeeAdviceId = currentAdvice.EmployeeAdviceId,
                            ButtonId = request.Answer,
                            EmployeeID = dealerUserValidation.dealer.DealerID,
                            DateCreated = DateTime.Now
                        };

                        new EmployeeAdviceLogBLL().Save(employeeAdviceLog);

                    }

                    break;

                default:
                    ErrorResponse error = dealerUserValidator.ProcessError(dealerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveDealerAdviceListResponse ReceiveDealerAdviceList(ReceiveDealerAdviceListRequest request)
        {
            ReceiveDealerAdviceListResponse response = new ReceiveDealerAdviceListResponse();
            DealerUserValidator dealerUserValidator = new DealerUserValidator();
            DealerUserValidation dealerUserValidation = dealerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (dealerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<EmployeeAdvice> adviceList;
                    if (request.Page == 0)
                    {
                        adviceList = EmployeeAdviceBLL.ListAdvice(dealerUserValidation.dealer.DealerID, (int)EmployeeAdviceEnum.EmployeeType.Dealer);
                    }
                    else
                    {
                        int skip = (request.Page - 1) * 10;
                        adviceList = EmployeeAdviceBLL.ListAdvice(dealerUserValidation.dealer.DealerID, (int)EmployeeAdviceEnum.EmployeeType.Dealer, skip);
                    }

                    if (adviceList.Count > 0)
                    {
                        response.AdviceList = new List<ReceiveDealerAdviceListResponse.Advice>();
                        ReceiveDealerAdviceListResponse.Advice advice;
                        foreach (EmployeeAdvice adviceItem in adviceList)
                        {
                            advice = new ReceiveDealerAdviceListResponse.Advice();
                            advice.AdviceId = adviceItem.EmployeeAdviceId;
                            advice.Title = adviceItem.Title;
                            advice.Call = adviceItem.Call;
                            advice.Body = adviceItem.Body;
                            advice.Date = adviceItem.DateCreated.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                            advice.ImageId = adviceItem.AppImageId ?? 0;
                            advice.AdviceButtonList = new List<ReceiveDealerAdviceListResponse.Advice.AdviceButton>();

                            if (!string.IsNullOrWhiteSpace(adviceItem.Argument))
                            {
                                Product product = ProductBLL.GetById(Convert.ToInt32(adviceItem.Argument));
                                advice.ProductID = product.ProductID;
                                advice.ProductName = product.Name;
                                advice.ProductPrice = product.Cost;
                            }

                            foreach (EmployeeAdviceButton button in adviceItem.EmployeeAdviceButtons)
                            {
                                advice.AdviceButtonList.Add(new ReceiveDealerAdviceListResponse.Advice.AdviceButton()
                                {
                                    Id = button.ButtonId,
                                    Text = button.Text
                                });
                            }
                            response.AdviceList.Add(advice);
                        }
                    }

                    break;

                default:
                    ErrorResponse error = dealerUserValidator.ProcessError(dealerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveCustomerListResponse ReceiveCustomerList(ReceiveCustomerListRequest request)
        {
            ReceiveCustomerListResponse response = new ReceiveCustomerListResponse();
            DealerUserValidator dealerUserValidator = new DealerUserValidator();
            DealerUserValidation dealerUserValidation = dealerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (dealerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<Customer> customerList = CustomerBLL.ListNameQuery(request.Query);

                    if (request.JapoUser)
                        customerList = customerList.Where(r=>r.CustomerTypeID == (int)CustomerEnum.CustomerTypeID.Final).ToList();

                    response.CustomerDataList = new List<ReceiveCustomerListResponse.CustomerData>();
                    if (customerList.Count() > 0)
                    {
                        response.CustomerDataList = (from c in customerList
                                                     select new ReceiveCustomerListResponse.CustomerData()
                                                     {
                                                         Name = c.CustomerTypeID == (int)CustomerEnum.CustomerTypeID.Distributor ? c.Name :
                                                                ((c.Buyers.FirstOrDefault().FirstName ?? "") + " " + (c.Buyers.FirstOrDefault().LastName ?? "")),
                                                         Phone = c.PhoneNumber ?? "",
                                                         Segment = c.Segment == null ? "" : c.Segment.Name,
                                                         Email = c.Buyers.FirstOrDefault().Email,
                                                         BuyerId = c.Buyers.FirstOrDefault().BuyerID,
                                                         ContributorNumber = c.CNPJ,
                                                         ContributorName = c.BusinessName,
                                                         CountryId = c.CountryID ?? 0,
                                                         CustomerTypeId = c.CustomerTypeID ?? 0,
                                                         LocationList = (from l in c.CustomerLocations
                                                                         where l.Active && !(l.Temporary ?? false)
                                                                         select new ReceiveCustomerListResponse.CustomerData.Location()
                                                                         {
                                                                             Address = l.Address,
                                                                             ImageId = l.AppImageID ?? 0,
                                                                             Latitude = l.Latitude,
                                                                             LocationId = l.CustomerLocationID,
                                                                             Longitude = l.Longitude,
                                                                             Number = l.Number ?? 0,
                                                                             Description = l.Description,
                                                                             Locality = l.Locality,
                                                                             CityId = l.CityID ?? 0,
                                                                             Complement = l.Complement,
                                                                             Reference = l.Reference
                                                                         }).ToList()
                                                     }).ToList();
                    }

                    break;

                default:
                    ErrorResponse error = dealerUserValidator.ProcessError(dealerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveSellerListResponse ReceiveSellerList(ReceiveSellerListRequest request)
        {
            ReceiveSellerListResponse response = new ReceiveSellerListResponse();
            DealerUserValidator dealerUserValidator = new DealerUserValidator();
            DealerUserValidation dealerUserValidation = dealerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (dealerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<Seller> sellerList = SellerBLL.ListAll(dealerUserValidation.dealer.Employee.SupplierID);
                    response.SellerDataList = new List<ReceiveSellerListResponse.SellerData>();
                    foreach(Seller seller in sellerList)
                    {
                        response.SellerDataList.Add(new ReceiveSellerListResponse.SellerData()
                        {
                            SellerId = seller.SellerID,
                            Name = ((seller.Employee.FirstName ?? "") + " " + (seller.Employee.LastName ?? ""))
                        });
                    }

                    break;

                default:
                    ErrorResponse error = dealerUserValidator.ProcessError(dealerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendNewCustomerReVisitResponse SendNewCustomerReVisit(SendNewCustomerReVisitRequest request)
        {
            SendNewCustomerReVisitResponse response = new SendNewCustomerReVisitResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    CustomerReVisit customerReVisit = new CustomerReVisit()
                    {
                        Active = true,
                        CustomerID = request.CustomerId,
                        DateCreated = DateTime.Now,
                        NextStep = request.NextStep,
                        ReVisitResultId = request.ReVisitResultId,
                        Latitude = request.Latitude,
                        Longitude = request.Longitude
                    };

                    new CustomerReVisitBLL().Save(customerReVisit);
                    
                    break;

                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendCancelMyOrderResponse SendCancelMyOrder(SendCancelMyOrderRequest request)
        {
            SendCancelMyOrderResponse response = new SendCancelMyOrderResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            if (buyerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk || 
                sellerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk)
            {

                    MyOrder myOrder = MyOrderBLL.GetById(request.MyOrderID);
                    MyOrderHistory lastMyOrderHistory = MyOrderHistoryBLL.GetLast(request.MyOrderID);


                    int? interactorId = lastMyOrderHistory.InteractorEmployeeId;
                    if(sellerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk)
                        interactorId = sellerUserValidation.seller.SellerID;

                    if (lastMyOrderHistory.Status == (int)MyOrderHistoryEnum.Status.AwaitingAttention)
                    {
                        MyOrderHistory myOrderHistory = new MyOrderHistory()
                        {
                            Active = lastMyOrderHistory.Active,
                            ChangeOf = lastMyOrderHistory.ChangeOf,
                            Cost = lastMyOrderHistory.Cost,
                            CustomerLocationID = lastMyOrderHistory.CustomerLocationID,
                            DateCreated = DateTime.Now,
                            DealerID = lastMyOrderHistory.DealerID,
                            DeliveryDate = lastMyOrderHistory.DeliveryDate,
                            InteractionID = (int)MyOrderHistoryEnum.InteractionID.Cancellation,
                            InvoiceNumber = lastMyOrderHistory.InvoiceNumber,
                            InvoiceDate = lastMyOrderHistory.InvoiceDate,
                            MyOrderID = lastMyOrderHistory.MyOrderID,
                            NotificationID = lastMyOrderHistory.NotificationID,
                            Observation = lastMyOrderHistory.Observation,
                            PaymentDate = lastMyOrderHistory.PaymentDate,
                            PaymentMethod = lastMyOrderHistory.PaymentMethod,
                            PayMode = lastMyOrderHistory.PayMode,
                            SellerID = lastMyOrderHistory.SellerID,
                            ShippingCost = lastMyOrderHistory.ShippingCost,
                            Status = (int)MyOrderHistoryEnum.Status.Canceled,
                            InteractorEmployeeId = interactorId,
                            TrackPointID = lastMyOrderHistory.TrackPointID,
                            Receiver = lastMyOrderHistory.Receiver,
                            AppImageID = lastMyOrderHistory.AppImageID,
                            CancelDescription = lastMyOrderHistory.CancelDescription
                        };

                        new MyOrderHistoryBLL().Save(myOrderHistory);
                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E025";
                        response.ErrorMessage = Resources.ErrorRes.E025;
                    }
            }
            else
            {
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;
            }
            return response;
        }

        public ReceiveExtractCreditListResponse ReceiveExtractCreditList(ReceiveExtractCreditListRequest request)
        {
            ReceiveExtractCreditListResponse response = new ReceiveExtractCreditListResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:


                    break;

                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendDeleteLocationResponse SendDeleteLocation(SendDeleteLocationRequest request)
        {
            SendDeleteLocationResponse response = new SendDeleteLocationResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            if (buyerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk ||
                sellerUserValidation.code == ValidationCodeEnum.DeviceMobileUserOk)
            {

                CustomerLocationBLL bll = new CustomerLocationBLL();
                CustomerLocation customerLocation = bll.GetById(request.LocationId);

                customerLocation.Active = false;

                bll.Update(customerLocation);

            }
            else
            {
                ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                response.ErrorCode = error.ErrorCode;
                response.ErrorMessage = error.ErrorMessage;
            }
            return response;
        }

        public SendUpdateOrderProductResponse SendUpdateOrderProduct(SendUpdateOrderProductRequest request)
        {
            SendUpdateOrderProductResponse response = new SendUpdateOrderProductResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    List<MyOrderProduct> myOrderProductListDel = new List<MyOrderProduct>();
                    List<MyOrderProduct> myOrderProductListUpd = new List<MyOrderProduct>();
                    List<MyOrderProduct> myOrderProductListAdd = new List<MyOrderProduct>();

                    MyOrderProductBLL bll = new MyOrderProductBLL();
                    List<MyOrderProduct> myOrderProductList = MyOrderProductBLL.GetByMyOrderID(request.OrderId);

                    foreach(MyOrderProduct orderProduct in myOrderProductList)
                    {
                        if(request.OrderProductList.Any(op=>op.ProductID == orderProduct.ProductID && op.Quantity == 0))
                        {
                            myOrderProductListDel.Add(orderProduct);
                        }
                        else if(request.OrderProductList.Any(op=>op.ProductID == orderProduct.ProductID && op.Quantity != orderProduct.Quantity))
                        {
                            orderProduct.Quantity = request.OrderProductList.First(op => op.ProductID == orderProduct.ProductID).Quantity;
                            myOrderProductListUpd.Add(orderProduct);
                        }
                    }

                    Product product = null;
                    foreach(SendUpdateOrderProductRequest.OrderProduct orderProduct in request.OrderProductList)
                    {
                        if(!myOrderProductList.Any(op=>op.ProductID == orderProduct.ProductID))
                        {
                            product = ProductBLL.GetById(orderProduct.ProductID);
                            myOrderProductListAdd.Add(new MyOrderProduct()
                            { 
                                Active = true,
                                Cost = product.Cost,
                                DateCreated = DateTime.Now,
                                IsAdded = true,
                                MyOrderID = request.OrderId,
                                ProductID = product.ProductID,
                                Quantity = orderProduct.Quantity,
                                QuantityInitial = orderProduct.Quantity
                            });
                        }
                    }

                    if (myOrderProductListDel.Count > 0)
                        bll.BulkDelete(myOrderProductListDel);

                    if (myOrderProductListUpd.Count > 0)
                        bll.BulkUpdate(myOrderProductListUpd);

                    if (myOrderProductListAdd.Count > 0)
                        bll.BulkSave(myOrderProductListAdd);

                    break;

                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveSellerOrderListResponse ReceiveSellerOrderList(ReceiveSellerOrderListRequest request)
        {
            ReceiveSellerOrderListResponse response = new ReceiveSellerOrderListResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    string statusListString = "0:3:7:2:10:9:5:";
                    List<int> statusList = new List<int>()
                    {
                        (int)MyOrderHistoryEnum.Status.AwaitingAttention,
                        (int)MyOrderHistoryEnum.Status.Approved,
                        (int)MyOrderHistoryEnum.Status.GoingToPoint,
                        (int)MyOrderHistoryEnum.Status.PendingApproval,
                        (int)MyOrderHistoryEnum.Status.Processing,
                        (int)MyOrderHistoryEnum.Status.Nobody,
                        (int)MyOrderHistoryEnum.Status.NotAccepted,
                        (int)MyOrderHistoryEnum.Status.Rejected
                    };

                    string orderTypeListString = "3:4:";
                    List<int> orderTypeList = new List<int>()
                    {
                        (int)MyOrderEnum.Type.OrderSeller,
                        (int)MyOrderEnum.Type.FreeSample
                    };

                    DateTime deliveryDate = DateTime.ParseExact(request.DeliveryDate, "ddMMyyyy", CultureInfo.InvariantCulture);

                    List<spSellerOrderListResult> sellerOrderListResult;
                    if (sellerUserValidation.seller.IsAdmin)
                    {
                        sellerOrderListResult = MyOrderBLL.ListSellerOrder(sellerUserValidation.seller.Employee.SupplierID, 0, statusListString, orderTypeListString, deliveryDate, request.Page);
                    }
                    else
                    {
                        sellerOrderListResult = MyOrderBLL.ListSellerOrder(sellerUserValidation.seller.Employee.SupplierID, sellerUserValidation.seller.SellerID, statusListString, orderTypeListString, deliveryDate, request.Page);
                    }

                    response.OrderList = (
                                            from o in sellerOrderListResult
                                            select new ReceiveSellerOrderListResponse.Order()
                                            {
                                                Address = o.Address + ", " + o.Number.ToString(),
                                                Client = o.CustomerName + " (" + ((o.FirstName ?? "") + " " + (o.LastName ?? "")) + ")",
                                                DateOrder = o.DeliveryDate.Value.ToString("ddMMyyyyHHmm", CultureInfo.InvariantCulture),
                                                OrderID = o.MyOrderID,
                                                OrderNumber = o.ReceiptCode,
                                                Status = o.Status.Value,
                                                OrderType = o.MyOrderType == (int)MyOrderEnum.Type.Order ? "Pedido Cliente" :
                                                            (o.MyOrderType == (int)MyOrderEnum.Type.OrderSeller ? "Pedido Vendedor" :
                                                            "Muestra Gratis"),
                                                Latitude = o.Latitude,
                                                Longitude = o.Longitude,
                                                SellerName = ((o.SellerFirstName ?? "") + " " + (o.SellerLastName ?? "")),
                                                DealerName = string.IsNullOrWhiteSpace(o.DealerFirstName) ? "No Asignado" :
                                                             ((o.DealerFirstName ?? "") + " " +(o.DealerLastName ?? ""))
                                            }
                                         ).ToList();

                    break;

                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendNewJapoUserResponse SendNewJapoUser(SendNewJapoUserRequest request)
        {
            SendNewJapoUserResponse response = new SendNewJapoUserResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    Buyer buyer = BuyerBLL.HasEmail(request.Email);
                    if(buyer == null)
                    {
                        Customer customer = new Customer();
                    
                        customer.Name = request.CustomerName;
                        customer.BusinessName = request.ContributorName;
                        customer.CNPJ = request.ContributorNumber;
                        customer.PhoneNumber = request.PhoneNumber;
                        customer.Email = request.Email;
                        customer.Status = (int)CustomerEnum.Status.Activo;
                        customer.CustomerTypeID = request.CustomerTypeID;

                        if (request.CountryId != 0)
                            customer.CountryID = request.CountryId;

                        customer.DateCreated = DateTime.Now;
                        customer.Active = true;

                        #region Generacion de SerialKey y Encriptacion del password

                        request.Password = "102030";

                        /// <summary>
                        /// Array de bytes utilizado para gerar o SerialKey
                        /// </summary>
                        byte[] _MyCompanyKey = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
                                                                    11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 
                                                                        21, 22, 23, 24 };

                        /// <summary>
                        /// Array de bytes utilizado para gerar o SerialKey
                        /// </summary>
                        byte[] _MyCompanyIV = { 1, 2, 3, 4, 5, 6, 7, 8 };

                        KeySeller locKeyHandler = new KeySeller(_MyCompanyKey, _MyCompanyIV);

                        string keyvalue = locKeyHandler.SellKey(request.Password, request.Email, request.Password, Key.ProductOrProductFeatureSet.Product2_PaidFullVersion);

                        Security security = new Security();
                        request.Password = security.MD5(request.Password);
                        #endregion

                        #region Separa Nombres...
                        string[] names = request.CustomerName.Split(' ');
                        string firstName = string.Empty;
                        string lastName = string.Empty;

                        if (names.Count() == 1)
                        {
                            firstName = names[0];
                        }
                        else if (names.Count() == 2)
                        {
                            firstName = names[0];
                            lastName = names.Last();
                        }
                        else if (names.Count() == 3)
                        {
                            firstName = names[0];
                            lastName = names[1] + " " + names[2];
                        }
                        else if (names.Count() > 3)
                        {
                            firstName = names[0] + " " + names[1];
                            lastName = names[2] + " " + names[3];
                        }
                        #endregion

                        buyer = new Buyer()
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            PhoneNumber = request.PhoneNumber,
                            Email = request.Email,
                            Password = request.Password,
                            SerialKey = keyvalue,
                            IsBlocked = false,
                            Admin = true,
                            DateCreated = DateTime.Now,
                            Active = true
                        };

                        CustomerLocation customerLocation = new CustomerLocation()
                        {
                            Active = true,
                            Description = request.Description,
                            Address = request.Address,
                            CityID = request.CityID,
                            Complement = request.Complement,
                            DateCreated = DateTime.Now,
                            Latitude = request.Latitude,
                            Locality = request.Locality,
                            Longitude = request.Longitude,
                            Number = request.Number,
                            Reference = request.Reference,
                            Temporary = false
                        };

                        #region Procesa Imagen...
                        AppImage appImage = null;
                        if (!string.IsNullOrWhiteSpace(request.Base64StringImage))
                        {
                            CloudBlockBlob blockBlob;
                            security = new Security();
                            string photoName = security.MD5(request.SerialKey + DateTime.Now.Ticks.ToString()) + ".jpg";
                            byte[] imageBytes = Convert.FromBase64String(request.Base64StringImage);

                            CloudStorageAccount storageAccount =
                                CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString.Pediddo"]);
                            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                            CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerAppImage"]);

                            blockBlob = container.GetBlockBlobReference(photoName);
                            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                            {
                                blockBlob.UploadFromStream(ms);
                            }

                            appImage = new AppImage()
                            {
                                Active = true,
                                DateCreated = DateTime.Now,
                                ImageName = photoName
                            };

                            customerLocation.AppImage = appImage;
                        }
                        #endregion

                        customer.Buyers = new EntitySet<Buyer>();
                        customer.Buyers.Add(buyer);

                        customer.CustomerLocations = new EntitySet<CustomerLocation>();
                        customer.CustomerLocations.Add(customerLocation);

                        customer = new CustomerBLL().Save(customer);
                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E016";
                        response.ErrorMessage = Resources.ErrorRes.E016;
                    }
                    
                    break;

                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendCreateJapoUserOrderResponse SendCreateJapoUserOrder(SendCreateJapoUserOrderRequest request)
        {
            SendCreateJapoUserOrderResponse response = new SendCreateJapoUserOrderResponse();
            SellerUserValidator sellerUserValidator = new SellerUserValidator();
            SellerUserValidation sellerUserValidation = sellerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (sellerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    //CustomerLocation customerLocation = new CustomerLocationBLL().GetById(request.LocationId);

                    Buyer _Buyer = BuyerBLL.GetById(request.BuyerId);

                    Random random = new Random();
                    string ReceiptCode = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(0, 1000).ToString();

                    MyOrder _MyOrder = new MyOrder();
                    _MyOrder.BuyerID = _Buyer.BuyerID;
                    _MyOrder.MyOrderType = (int)MyOrderEnum.Type.Order;
                    
                    _MyOrder.ReceiptCode = ReceiptCode;
                    _MyOrder.RepeatOrder = false;
                    _MyOrder.DateCreated = DateTime.Now;
                    _MyOrder.Active = true;
                    _MyOrder.MyOrderHistories = new EntitySet<MyOrderHistory>();
                    _MyOrder.MyOrderProducts = new EntitySet<MyOrderProduct>();

                    decimal cost = 0;
                    decimal salePrice;
                    decimal price;
                    MyOrderProduct _MyOrderProduct;
                    List<MyOrderProduct> _MyOrderProductList = new List<MyOrderProduct>();
                    Product product;
                    PriceListDetail priceListDetail;
                    foreach (SendCreateJapoUserOrderRequest.OrderProduct productItem in request.OrderProductList)
                    {
                        product = ProductBLL.GetById(productItem.ProductId);
                        priceListDetail = null;
                        if(productItem.PriceListId != null && productItem.PriceListId > 0)
                            priceListDetail = PriceListDetailBLL.GetById(productItem.PriceListId.Value, product.ProductID);

                        salePrice = priceListDetail != null ?
                                    ((priceListDetail.OfferPrice ?? 0) != 0 ?
                                    priceListDetail.OfferPrice.Value :
                                    (priceListDetail.MarginDiscount == 0 ?
                                    priceListDetail.SalePrice :
                                    priceListDetail.DiscountPrice.Value)
                                    ) : product.Cost;

                        price = salePrice;
                        if (productItem.Discount > 0)
                            price = Math.Ceiling(price * (((decimal)100 - productItem.Discount) / (decimal)100));
                        

                        _MyOrderProduct = new MyOrderProduct();
                        _MyOrderProduct.Active = true;
                        _MyOrderProduct.DateCreated = DateTime.Now;
                        _MyOrderProduct.Discount = productItem.Discount;
                        _MyOrderProduct.DiscountInitial = 0;
                        _MyOrderProduct.IsAdded = false;
                        _MyOrderProduct.ProductID = product.ProductID;
                        _MyOrderProduct.Cost = price;
                        _MyOrderProduct.SalePrice = salePrice;
                        _MyOrderProduct.InOffer = (priceListDetail != null ? (priceListDetail.OfferPrice ?? 0) != 0 : false);
                        _MyOrderProduct.Quantity = productItem.Quantity;
                        _MyOrderProduct.QuantityInitial = productItem.Quantity;
                        _MyOrderProduct.PriceListID = (productItem.PriceListId == 0 ? null : productItem.PriceListId);
                        
                        _MyOrderProductList.Add(_MyOrderProduct);
                        cost = cost + (product.Cost * productItem.Quantity);
                    }
                    _MyOrder.MyOrderProducts.AddRange(_MyOrderProductList);

                    if (request.ShippingCost > 0)
                    {
                        ShippingCostRule rule = ShippingCostRuleBLL.GetById((int)ShippingCostRuleEnum.ShippingCostRuleId.OrderAmount);
                        if (rule.Active)
                        {
                            if (cost > rule.Value)
                            {
                                request.ShippingCost = 0;
                            }
                        }
                    }
                    //response.ShippingCost = request.ShippingCost;

                    MyOrderHistory _MyOrderHistory = new MyOrderHistory();
                    _MyOrderHistory.SellerID = null;
                    _MyOrderHistory.Status = (int)MyOrderHistoryEnum.Status.AwaitingAttention;
                    _MyOrderHistory.InteractionID = (int)MyOrderHistoryEnum.InteractionID.NewOrder;
                    _MyOrderHistory.Cost = cost;
                    _MyOrderHistory.PayMode = (int)MyOrderHistoryEnum.PayMode.CashPayment;
                    _MyOrderHistory.PaymentMethod = request.PaymentMethod;
                    _MyOrderHistory.CustomerLocationID = request.LocationId;

                    _MyOrderHistory.ChangeOf = request.ChangeOf;
                    _MyOrderHistory.ShippingCost = request.ShippingCost;
                    _MyOrderHistory.PaymentDate = DateTime.Now;

                    if(string.IsNullOrWhiteSpace(request.DeliveryDate))
                        _MyOrderHistory.DeliveryDate = DateTime.Now;
                    else
                        _MyOrderHistory.DeliveryDate = DateTime.ParseExact(request.DeliveryDate, "ddMMyyyyHHmm", CultureInfo.InvariantCulture);

                    _MyOrderHistory.DateCreated = DateTime.Now;
                    _MyOrderHistory.Active = true;

                    _MyOrder.MyOrderHistories.Add(_MyOrderHistory);

                    Product prod = ProductBLL.GetById(request.OrderProductList.FirstOrDefault().ProductId);
                    Category category = CategoryBLL.GetById(prod.CategoryID.Value);
                    _MyOrder.SupplierID = category.SupplierID;

                    new MyOrderBLL().Save(_MyOrder);

                    Customer customer = CustomerBLL.GetById(_Buyer.CustomerID.Value);
                    if(request.PaymentMethod == (int)MyOrderHistoryEnum.PaymentMethod.PediddoCredit)
                    {
                        decimal totalCost = Convert.ToDecimal(_MyOrderHistory.Cost + _MyOrderHistory.ShippingCost);
                        if(totalCost > customer.Credit)
                        {
                            customer.Credit = 0;
                        }
                        else
                        {
                            customer.Credit = customer.Credit - totalCost;
                        }

                        new CustomerBLL().Update(customer);
                    }

                    #region Notificaciones Push...
                    List<Notification> notificationList = new List<Notification>();

                    string[] message = Resources.NotificationPush.N001.Split('#');
                    notificationList.Add(new Notification()
                    {
                        Active = true,
                        Activity = 0,
                        AppID = (int)AppEnum.AppID.PediddoNew,
                        Channel = _Buyer.SerialKey,
                        ContentText = message[1],
                        ContentTitle = message[0],
                        DateCreated = DateTime.Now,
                        DateToSend = DateTime.Now,
                        DeviceOS = "1",
                        IsRead = true,
                        Send = false,
                        Type = 1
                    });

                    message = Resources.NotificationPush.N003.Split('#');
                    notificationList.Add(new Notification()
                    {
                        Active = true,
                        Activity = 0,
                        AppID = (int)AppEnum.AppID.PediddoDelivery,
                        Channel = "PEDIDDO-DELIVERY-CHANNEL",
                        ContentText = message[1],
                        ContentTitle = message[0],
                        DateCreated = DateTime.Now,
                        DateToSend = DateTime.Now,
                        DeviceOS = "1",
                        IsRead = true,
                        Send = false,
                        Type = 1
                    });

                    //if (request.PaymentMethod == (int)MyOrderHistoryEnum.PaymentMethod.PediddoCredit)
                    //{
                    //    message = Resources.NotificationPush.N012.Replace("{SaldoCredito}", customer.Credit.ToString() + " G$.").Split('#');
                    //    notificationList.Add(new Notification()
                    //    {
                    //        Active = true,
                    //        Activity = 0,
                    //        AppID = request.AppID,
                    //        Channel = _Buyer.SerialKey,
                    //        ContentText = message[1],
                    //        ContentTitle = message[0],
                    //        DateCreated = DateTime.Now,
                    //        DateToSend = DateTime.Now,
                    //        DeviceOS = "1",
                    //        IsRead = true,
                    //        Send = false,
                    //        Type = 1
                    //    });
                    //}

                    new NotificationBLL().BulkSave(notificationList);
                    #endregion

                    break;

                default:
                    ErrorResponse error = sellerUserValidator.ProcessError(sellerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveMeterCategoryListResponse ReceiveMeterCategoryList(ReceiveMeterCategoryListRequest request)
        {
            ReceiveMeterCategoryListResponse response = new ReceiveMeterCategoryListResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    Meter meter = MeterBLL.GetByName("CHURRASCOMETRO");
                    if (meter != null)
                    {
                        response.MeterId = meter.MeterId;
                        response.MeterCategoryList = from mtc in meter.MeterCategories
                                                     select new ReceiveMeterCategoryListResponse.MeterCategory()
                                                     {
                                                         MeterCategoryId = mtc.MeterCategoryId,
                                                         CategoryName = mtc.Name,
                                                         CategoryDescription = "",
                                                         Required = false,
                                                         Sort = mtc.Sort,
                                                         CategoryProductList = from mtcp in mtc.MeterCategoryProducts
                                                                               let priceListDetail = mtcp.Product.PriceListDetails.FirstOrDefault(pl => pl.PriceList.CustomerTypeID == 1)
                                                                               select new ReceiveMeterCategoryListResponse.MeterCategoryProduct()
                                                                               {
                                                                                   ProductId = mtcp.ProductID,
                                                                                   ProductName = mtcp.Product.Name,
                                                                                   Price = priceListDetail != null ?
                                                                                           ((priceListDetail.OfferPrice ?? 0) != 0 ?
                                                                                           priceListDetail.OfferPrice.Value :
                                                                                           (priceListDetail.MarginDiscount == 0 ?
                                                                                           priceListDetail.SalePrice :
                                                                                           priceListDetail.DiscountPrice.Value)
                                                                                           ) : mtcp.Product.Cost,
                                                                                   ImageId = mtcp.Product.AppImageID ?? 0,
                                                                                   ThumbImageId = mtcp.Product.ThumbAppImageID ?? 0,
                                                                                   Manufacturer = mtcp.Product.Manufacturer,
                                                                                   UnitMeasure = mtcp.Product.Udm,
                                                                                   PriceListId = (priceListDetail != null ? priceListDetail.PriceListID : 0),
                                                                                   OldPrice = (priceListDetail == null ? 0 :
                                                                                               ((priceListDetail.OfferPrice ?? 0) == 0 ? 0 :
                                                                                               (priceListDetail.MarginDiscount != 0 ? priceListDetail.DiscountPrice.Value : priceListDetail.SalePrice))),
                                                                                   MinimumQuantity = (priceListDetail != null ? (priceListDetail.MinimumQuantity ?? 1) : 1),
                                                                                   MaximumDiscount = (priceListDetail != null ? (priceListDetail.MaximumDiscount ?? 0) : 0)

                                                                               }
                                                     };
                    }
                    break;

                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public SendCalculateMeterResponse SendCalculateMeter(SendCalculateMeterRequest request)
        {
            SendCalculateMeterResponse response = new SendCalculateMeterResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                    Meter meter = MeterBLL.GetById(request.MeterId);
                    meter = MeterBLL.GetByName(meter.Name);

                    //foreach (MeterCategory meterCategory in meter)

                    break;

                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;

            }
            return response;
        }

        public ReceiveListSupplierByAroundResponse ReceiveListSupplierByAround(ReceiveListSupplierByAroundRequest request)
        {
            ReceiveListSupplierByAroundResponse response = new ReceiveListSupplierByAroundResponse();
            BuyerUserValidator buyerUserValidator = new BuyerUserValidator();
            BuyerUserValidation buyerUserValidation = buyerUserValidator.Validate(request.SerialKey, request.UUID);

            switch (buyerUserValidation.code)
            {
                case ValidationCodeEnum.DeviceMobileUserOk:

                List<Supplier> supplierList = SupplierBLL.ListByAround();
                response.SupplierDataList = new List<ReceiveListSupplierByAroundResponse.SupplierData>();
                foreach (Supplier supplier in supplierList)
                {
                    response.SupplierDataList.Add(new ReceiveListSupplierByAroundResponse.SupplierData()
                    {
                        SupplierId          = supplier.SupplierID,
                        Name                = supplier.Name,
                        PhoneNumber         = supplier.PhoneNumber,
                        SerialKey           = supplier.SerialKey,
                        CNPJ                = supplier.CNPJ,
                        ShippingCostRule    = 1500,
                        Distance            = "3km",                        
                        ImageId             = 32,
                        ThumbImageId        = 32,
                        DeliveryTime        = "30-40min",
                        Rating              = 1.5
                    });
                }
                    break;
                default:
                    ErrorResponse error = buyerUserValidator.ProcessError(buyerUserValidation.code);
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorMessage = error.ErrorMessage;

                    break;
            }
            return response;
        }

        public SendMinimumVersionResponse SendMinimumVersion(SendMinimumVersionRequest request)
        {
            SendMinimumVersionResponse response = new SendMinimumVersionResponse();

            VersionBLL versionBLL = new VersionBLL();
            Core.Data.DAO.Version _Version = versionBLL.GetMinimumVersion(request.OS);

            if (_Version != null)
            {
                response.MinimumVersion = _Version.Version1;
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E001";
                response.ErrorMessage = Resources.ErrorRes.E001;
            }

            return response;

        }

        public ResetBuyerSellerCodelinkResponse ResetBuyerSellerCode(string SellerID, string BuyerID, string Code)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ResetBuyerSellerCodelinkResponse response = new ResetBuyerSellerCodelinkResponse();

            BuyerSeller _BuyerSeller = null;
            _BuyerSeller = BuyerSellerBLL.GetById(Convert.ToInt32(BuyerID), Convert.ToInt32(SellerID));

            BuyerSellerBLL _BuyerSellerBLL = new BuyerSellerBLL();
            _BuyerSellerBLL.DeleteBuyerSeller(Convert.ToInt32(BuyerID), Convert.ToInt32(SellerID));

            //_BuyerSellerBLL.BulkDelete(new List<BuyerSeller> { _BuyerSeller });

            CodeLinkBLL _CodeLink = new CodeLinkBLL();
            _CodeLink.MarkCodeAsNotUsed(Code);

            status = DeviceCommunicationLogStatus.Success;

            //response.Apple();
            return response;
        }

        public SendSupplierRecommendationResponse SendSupplierRecommendation(SendSupplierRecommendationRequest request)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            SendSupplierRecommendationResponse response = new SendSupplierRecommendationResponse();

            SupplierRecommendation newSupplierRecommendation = null;

            newSupplierRecommendation = new SupplierRecommendation();

            newSupplierRecommendation.Identification = request.Identification;
            newSupplierRecommendation.SerialKey = request.SerialKey;
            newSupplierRecommendation.PhoneNumber = request.PhoneNumber;
            newSupplierRecommendation.Email = request.Email;
            newSupplierRecommendation.Company = request.Company;
            newSupplierRecommendation.SegmentID = Convert.ToInt32(request.SegmentID);
            newSupplierRecommendation = new SupplierRecommendationBLL().Save(newSupplierRecommendation);

            status = DeviceCommunicationLogStatus.Success;

            return response;
        }

        public ReceiveUpdateUserDeviceResponse UpdateUserDevice(ReceiveUpdateUserDeviceRequest request)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveUpdateUserDeviceResponse response = new ReceiveUpdateUserDeviceResponse();

            Buyer _Buyer = null;
            _Buyer = BuyerBLL.GetBySerialKey(request.SerialKey);
            if (_Buyer != null)
            {

                if (_Buyer.Active.Value)
                {

                    if (!_Buyer.IsBlocked.Value)
                    {

                        if (_Buyer.SerialKey != null)
                        {

                            BuyerDevice _BuyerDevice;
                            Device _Device;
                            _Device = DeviceBLL.GetByDeviceToken(request.Identification);
                            if (_Device != null)
                            {
                                _Device.PushToken = request.PushToken;

                                new DeviceBLL().Update(_Device);
                            }
                            status = DeviceCommunicationLogStatus.Success;
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E007";
                            response.ErrorMessage = Resources.ErrorRes.E007;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E005";
                        response.ErrorMessage = Resources.ErrorRes.E005;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E004";
                    response.ErrorMessage = Resources.ErrorRes.E004;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E001";
                response.ErrorMessage = Resources.ErrorRes.E001;
            }

            return response;

        }

        public ReceiveListMyOrderDashboardResponse ReceiveListMyOrderDashboard(ReceiveListMyOrderDashboardRequest request)
        {
            DeviceCommunicationLogStatus status = new DeviceCommunicationLogStatus();
            ReceiveListMyOrderDashboardResponse response = new ReceiveListMyOrderDashboardResponse();

            Device device = new Device();
            device = DeviceBLL.GetByDeviceToken(request.Identification);
            if (device != null)
            {

                Buyer _Buyer = null;
                _Buyer = BuyerBLL.GetBySerialKey(request.SerialKey);
                if (_Buyer != null)
                {

                    BuyerDevice _BuyerDevice;
                    _BuyerDevice = BuyerDeviceBLL.GetByBuyerDevice(_Buyer, device);
                    if (_BuyerDevice != null)
                    {

                        if (_Buyer.Active.Value)
                        {

                            if (!_Buyer.IsBlocked.Value)
                            {

                                List<MyOrder> MyOrderList;
                                MyOrderList = MyOrderBLL.ListByBuyer(_Buyer);

                                var MyOrderCustomList = MyOrderList.Select(x => new
                                {
                                    Id = x.MyOrderID,
                                    Status = x.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value,
                                    StatusDescription = MyOrderHistoryEnum.InteractionIDDescription(x.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value),
                                    StatusColor = MyOrderHistoryEnum.InteractionIDColor(x.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value)
                                });

                                var MyOrderListDashboard = MyOrderCustomList.GroupBy(x => new { Status = x.Status, StatusDescription = x.StatusDescription }).Select(y => new ReceiveListMyOrderDashboardResponse.MyOrder() { Status = y.Min(group => group.Status), StatusDescription = y.Min(group => group.StatusDescription), StatusColor = y.Min(group => group.StatusColor), Count = y.Count() }).ToList();

                                response.MyOrderListDashboard = MyOrderListDashboard;

                                status = DeviceCommunicationLogStatus.Success;

                            }
                            else
                            {
                                response.Status = ResponseCodeEnum.Error;
                                response.ErrorCode = "E005";
                                response.ErrorMessage = Resources.ErrorRes.E005;
                            }
                        }
                        else
                        {
                            response.Status = ResponseCodeEnum.Error;
                            response.ErrorCode = "E004";
                            response.ErrorMessage = Resources.ErrorRes.E004;
                        }

                    }
                    else
                    {
                        response.Status = ResponseCodeEnum.Error;
                        response.ErrorCode = "E003";
                        response.ErrorMessage = Resources.ErrorRes.E003;
                        status = DeviceCommunicationLogStatus.KnownError;
                    }
                }
                else
                {
                    response.Status = ResponseCodeEnum.Error;
                    response.ErrorCode = "E007";
                    response.ErrorMessage = Resources.ErrorRes.E007;
                    status = DeviceCommunicationLogStatus.KnownError;
                }
            }
            else
            {
                response.Status = ResponseCodeEnum.Error;
                response.ErrorCode = "E002";
                response.ErrorMessage = Resources.ErrorRes.E002;
            }

            return response;
        }


    }
}
