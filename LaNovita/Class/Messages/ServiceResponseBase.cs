using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewMobile.Pediddo.WebService.Mobile.Enumerations;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    /// <summary>
    /// Provides the base response to WebServices calls
    /// Every response sent thru the webservices should create a class and inherit this base class
    /// </summary>
    public abstract class ServiceResponseBase
    {
        #region Members
        private ResponseCodeEnum _status = ResponseCodeEnum.Success;
        private string _errorCode = string.Empty;
        private string _errorMessage = string.Empty;
        private int _notificationID = 0;
        private string _playlist = string.Empty;

        private DateTime _localTimeStamp = DateTime.Now.ToUniversalTime();
        //private DateTime? _dateScheduledDelivery;
        #endregion

        #region Properties
        /// <summary>
        /// The local timestamp of the server expressed in Coordinated Universal Time (UTC).
        /// </summary>
        /// <remarks>Anyone that needs to use this time should convert it to a local DateTime.</remarks>
        public DateTime LocalTimeStamp
        {
            get { return _localTimeStamp; }
        }

        /// <summary>
        /// The status of the requested operation
        /// </summary>
        public ResponseCodeEnum Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// The error code associated to the unsuccessful operation (if any).
        /// </summary>
        /// <remarks>By default, if the operation was successful, returns an empty string.</remarks>
        public string ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        /// <summary>
        /// The error message associated to the error ocurred on the unsuccessful operation (if any).
        /// </summary>
        /// <remarks>By default, if the opeation was successful, returns an empty string.</remarks>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        /// <summary>
        /// Gets or sets the NotificationID.
        /// </summary>
        /// <value>The NotificationID.</value>
        public int NotificationID
        {
            get { return _notificationID; }
            set { _notificationID = value; }
        }

        /// <summary>
        /// The date that the consumer scheduled to delivery his/her shopping list
        /// </summary>
        //public DateTime? DateScheduledDelivery
        //{
        //    get { return _dateScheduledDelivery; }
        //    set { _dateScheduledDelivery = value; }
        //}

        /// <summary>
        /// XML Apple
        /// </summary>
        /// <remarks>By default, if the opeation was successful, returns an empty string.</remarks>
        public string Playlist
        {
            get { return _playlist; }
            set { _playlist = value; }
        }

        #endregion

        //#region Methods
        //protected byte[] Serialize<Entity>(Entity entity)
        //{
        //    MemoryStream buffer = new MemoryStream();
        //    XmlSerializer serializer = new XmlSerializer(typeof(Entity));

        //    serializer.Serialize(buffer, entity);
        //    return buffer.ToArray();
        //}

        //protected static XmlDocument ToXml(byte[] data)
        //{
        //    MemoryStream buffer = new MemoryStream(data);
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(buffer);

        //    return doc;
        //}

        //protected static Entity ToEntity<Entity>(byte[] data)
        //{
        //    MemoryStream buffer = new MemoryStream(data);
        //    XmlSerializer serializer = new XmlSerializer(typeof(Entity));

        //    return (Entity)serializer.Deserialize((buffer));
        //}

        //protected XmlDocument ToXmlDocument<Entity>(Entity entity)
        //{
        //    XmlDocument xml = new XmlDocument();
        //    MemoryStream buffer = new MemoryStream();
        //    XmlSerializer serializer = new XmlSerializer(typeof(Entity));

        //    serializer.Serialize(buffer, entity);
        //    buffer.Position = 0;

        //    xml.Load(buffer);
        //    return xml;
        //}

        //protected string ToXmlString<Entity>(Entity entity)
        //{
        //    return ToXmlDocument<Entity>(entity).OuterXml;
        //}


        //#endregion

        //#region ISerializable Members
        ////public abstract byte[] Serialize();
        //#endregion

    }
}