using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;
using ViewMobile.Pediddo.Core.Data.BLL;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SellerUserValidation
    {
        public Seller seller { get; set; }
        public Device device { get; set; }
        public ValidationCodeEnum code { get; set; }
    }

    public class SellerUserValidator
    {
        public SellerUserValidation Validate(string serialKey, string uuid)
        {
            Seller seller = SellerBLL.GetBySerialKey(serialKey);
            SellerUserValidation userValidation = new SellerUserValidation();

            if (seller != null)
            {
                userValidation.seller = seller;
                Device device = DeviceBLL.GetByDeviceToken(uuid);
                if (device != null)
                {
                    userValidation.device = device;
                    //SellerDevice sellerDevice = SellerDeviceBLL.GetBySellerDevice(seller, device);
                    Employee employee = EmployeeBLL.GetById(seller.SellerID);
                    EmployeeDevice employeeDevice = EmployeeDeviceBLL.GetByEmployeeDevice(employee, device);
                    if (employeeDevice != null)
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