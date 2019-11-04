using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRPD_WebApp.Models
{
    public class QRModel
    {
        public const int iCreated = 1;
        public const int iKeynum = 0;
        public const int iUserID = 2;

        public QRModel(byte[] keynum, DateTime created, string userID)
        {
            Keynum = keynum;
            Created = created;
            UserID = userID;
        }

        public DateTime Created { get; set; }
        public byte[] Keynum { get; set; }
        public string UserID { get; set; }

        public static QRModel FromArray(object[] arr)
        {
            try
            {
                if (arr[iKeynum] is byte[] && arr[iCreated] is DateTime && arr[iUserID] is string)
                    return new QRModel(arr[iKeynum] as byte[], (DateTime)arr[iCreated], arr[iUserID] as string);
            }
            catch (IndexOutOfRangeException)
            {
            }
            throw new ArgumentException();
        }

        public object[] ToArray() => new object[] { Keynum, Created, UserID };
    }
}
