using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveCategoryListResponse : ServiceResponseBase
    {
        public List<Category> CategoryList;

        public class Category
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public bool HasChildren { get; set; }
            public int ThumbImageId { get; set; }
        }
        
    }
}