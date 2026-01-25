using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Modules.Base.Infrastructure.Data.EF.Constants
{
    /// <summary>
    /// Collection of constants 
    /// used when defining db models
    /// </summary>
    public static class DbFieldConstants
    {
        /// <summary>
        /// Constant to define string lengths of 128 bytes
        /// </summary>
        public const int x128 = 128;

        /// <summary>
        /// Constant to define string lengths of 256 bytes
        /// </summary>
        public const int x256 = 256;
        /// <summary>
        /// Constant to define string lengths of 1024 bytes
        /// </summary>

        public const int x1024 = 1024;

        /// <summary>
        /// Constant to define string lengths of 2048 bytes
        /// </summary>
        public const int x2048 = 2048;

        /// <summary>
        /// Constant to define string lengths of 4000 bytes
        /// <para>
        /// Which is the same length as NVARCHAR(Max)
        /// </para>
        /// </summary>
        public const int x4000 = 4000;

        /*
        /// <summary>
        /// Constant to define string lengths of 4096 bytes
        /// </summary>
        public const int x4096 = 4096;
        */

        /// <summary>
        /// Constant to define string lengths of x128
        /// </summary>
        public const int StringLengthTiny = x128;

        /// <summary>
        /// Constant to define string lengths of x256
        /// </summary>
        public const int StringLengthSmall = x256;

        /// <summary>
        /// Constant to define string lengths of x1024
        /// </summary>
        public const int StringLengthMedium = x1024;

        /// <summary>
        /// Constant to define string lengths of x2048
        /// </summary>
        public const int StringLengthLarge = x2048;

        /// <summary>
        /// Constant to define string lengths of x4000
        /// <para>
        /// Note that this is NOT the same as Max,
        /// which is much larger
        /// </para>
        /// </summary>
        public const int StringLengthXLarge = x4000;

        /// <summary>
        /// Constant to define string lengths of 2147483647
        /// <para>
        /// Which is equal to NVARCHAR(Max) </para>
        /// </summary>
        public const int StringLengthMax = 2147483647;


        /// <summary>
        /// Constant to define string length required to 
        /// encode Guids
        /// <para>
        /// TODO: Verify that if set to Unicode, doesn't truncate.
        /// </para>
        /// </summary>
        public const int GuidStringLength = 36;
    }
}

