using System;
using System.Security.Cryptography;

namespace BlueNetWebApp.db
{
    public partial class Guest
    {
        /// <summary>
        /// Contructor to generate a random keynum
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Guest()
        {
            Keynum = new byte[16];
            using (var rng = RNGCryptoServiceProvider.Create())
            {
                rng.GetBytes(Keynum);
            }
            Visited = DateTime.UtcNow;
            this.IsConfirmed = false;
        }
    }
}
