#region Class1 文件信息
/***********************************************************
**文 件 名：Class1 
**命名空间：CommonLibrary.Sequentials 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2017/11/20 10:52:10 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System;
using System.Security.Cryptography;

namespace CommonLibrary.Helpers
{
    /// <summary>
    /// 可生成连续GUID的帮助类
    /// </summary>
    public static partial class GuidHelper
    {
        #region Static Fields
        /// <summary>
        /// Provides cryptographically strong random data for GUID creation.
        /// </summary>
        private static readonly RNGCryptoServiceProvider RandomGenerator = new RNGCryptoServiceProvider();
        #endregion

        /// <summary>
        /// 生成连续的GUID
        /// </summary>
        /// <param name="guidType">连续Guid类型</param>
        /// <returns></returns>
        public static Guid Create(GuidType guidType = GuidType.AsString)
        {
            // We start with 16 bytes of cryptographically strong random data.
            var randomBytes = new byte[10];
            RandomGenerator.GetBytes(randomBytes);

            // An alternate method: use a normally-created GUID to get our initial
            // random data:
            // byte[] randomBytes = Guid.NewGuid().ToByteArray();
            // This is faster than using RNGCryptoServiceProvider, but I don't
            // recommend it because the .NET Framework makes no guarantee of the
            // randomness of GUID data, and future versions (or different
            // implementations like Mono) might use a different method.

            // Now we have the random basis for our GUID.  Next, we need to
            // create the six-byte block which will be our timestamp.

            // We start with the number of milliseconds that have elapsed since
            // DateTime.MinValue.  This will form the timestamp.  There's no use
            // being more specific than milliseconds, since DateTime.Now has
            // limited resolution.

            // Using millisecond resolution for our 48-bit timestamp gives us
            // about 5900 years before the timestamp overflows and cycles.
            // Hopefully this should be sufficient for most purposes. :)
            var timestamp = DateTime.UtcNow.Ticks / 10000L;

            // Then get the bytes
            var timestampBytes = BitConverter.GetBytes(timestamp);

            // Since we're converting from an Int64, we have to reverse on
            // little-endian systems.
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            var guidBytes = new byte[16];

            switch (guidType)
            {
                case GuidType.AsString:
                case GuidType.AsBinary:

                    // For string and byte-array version, we copy the timestamp first, followed
                    // by the random data.
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                    // If formatting as a string, we have to compensate for the fact
                    // that .NET regards the Data1 and Data2 block as an Int32 and an Int16,
                    // respectively.  That means that it switches the order on little-endian
                    // systems.  So again, we have to reverse.
                    if (guidType == GuidType.AsString && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }

                    break;

                case GuidType.AtEnd:

                    // For sequential-at-the-end versions, we copy the random data first,
                    // followed by the timestamp.
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;
            }

            return new Guid(guidBytes);
        }
    }

    /// <summary>
    /// 连续Guid类型
    /// </summary>
    public enum GuidType
    {
        /// <summary>
        /// The GUID should be sequential when formatted using the
        /// <see cref="Guid.ToString()" /> method.
        /// </summary>
        AsString,

        /// <summary>
        /// The GUID should be sequential when formatted using the
        /// <see cref="Guid.ToByteArray" /> method.
        /// </summary>
        AsBinary,

        /// <summary>
        /// The sequential portion of the GUID should be located at the end
        /// of the Data4 block.
        /// </summary>
        AtEnd
    }
}
