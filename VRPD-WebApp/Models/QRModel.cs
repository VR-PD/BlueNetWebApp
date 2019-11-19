using System;

namespace VRPDWebApp.Models
{
    /// <summary>
    /// A model class to retrieve all the properties from a read QR code and to prepare to send away again as a object array
    /// </summary>
    public class QRModel
    {
        /// <summary>
        /// Index of Created property in object array
        /// </summary>
        private const int iCreated = 1;

        /// <summary>
        /// Index of Keynum property in object array
        /// </summary>
        private const int iKeynum = 0;

        /// <summary>
        /// Index of UserID property in object array
        /// </summary>
        private const int iUserID = 2;

        /// <summary>
        /// Keynum read from QR code in bytes
        /// </summary>
        private readonly byte[] keynum;

        /// <summary>
        /// Created a QRModel object from each of its properties
        /// </summary>
        /// <param name="keynum">Keynum as byte array</param>
        /// <param name="created">Date and time of creation</param>
        /// <param name="userID">String of user id</param>
        public QRModel(byte[] keynum, DateTime created, string userID)
        {
            this.keynum = keynum;
            Created = created;
            UserID = userID;
        }

        /// <summary>
        /// Date and time of creation of QR code and visit of guest to website
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// String to identify whether the guest is a verified user
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Create a QRModel object from a raw object array containing all its properties
        /// </summary>
        /// <param name="arr">Raw object array containing all its properties</param>
        /// <returns>dgfdhjhk</returns>
        public static QRModel FromArray(object[] arr)
        {
            try
            {
                if (arr != null && arr[iKeynum] is byte[] && arr[iCreated] is DateTime && arr[iUserID] is string)
                    return new QRModel(arr[iKeynum] as byte[], (DateTime)arr[iCreated], arr[iUserID] as string);
                return null;
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        public byte[] GetKeynum() => keynum;

        /// <summary>
        /// Convert QRModel object to a object array to send away
        /// </summary>
        /// <returns>An object array containing all its QRModel properties</returns>
        public object[] ToArray() => new object[] { GetKeynum(), Created, UserID };

        public override string ToString()
        {
            return $"Keynum: {Convert.ToBase64String(keynum)}\nCreation: {Created.ToString()}\nUserID: {UserID}";
        }
    }
}
