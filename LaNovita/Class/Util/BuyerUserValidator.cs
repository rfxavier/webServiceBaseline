using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;
using ViewMobile.Pediddo.Core.Data.BLL;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class BuyerUserValidation
    {
        public Buyer buyer { get; set; }
        public Device device { get; set; }
        public ValidationCodeEnum code { get; set; }
    }

    public class BuyerUserValidator
    {
        public BuyerUserValidation Validate(string serialKey, string uuid)
        {
            Buyer buyer = BuyerBLL.GetBySerialKey(serialKey);
            BuyerUserValidation userValidation = new BuyerUserValidation();

            if (buyer != null)
            {
                userValidation.buyer = buyer;
                Device device = DeviceBLL.GetByDeviceToken(uuid);
                if (device != null)
                {
                    userValidation.device = device;
                    BuyerDevice buyerDevice = BuyerDeviceBLL.GetByBuyerDevice(buyer, device);
                    if (buyerDevice != null)
                    {
                        userValidation.code = ValidationCodeEnum.DeviceMobileUserOk;
                    }
                    else
                    {
                        userValidation.code = ValidationCodeEnum.DeviceMobileUserInvalid;
                    }
                }
                else
                {
                    userValidation.code = ValidationCodeEnum.UnknowDevice;
                }
            }
            else
            {
                userValidation.code = ValidationCodeEnum.UnknowMobileUser;
            }

            return userValidation;
        }

        public ErrorResponse ProcessError(ValidationCodeEnum code)
        {
            ErrorResponse errorResponse = new ErrorResponse();

            switch (code)
            {
                case ValidationCodeEnum.UnknowDevice:
                    errorResponse.ErrorCode = "E002";
                    errorResponse.ErrorMessage = Resources.ErrorRes.E002;
                    break;
                case ValidationCodeEnum.UnknowMobileUser:
                    errorResponse.ErrorCode = "E005";
                    errorResponse.ErrorMessage = Resources.ErrorRes.E005;
                    break;
            }

            return errorResponse;
        }
    }
}