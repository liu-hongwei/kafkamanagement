// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolPrimitiveType.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Kafka.Management.KafkaProtocol
{
    using System;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The primitive types used in kafka protocol.
    /// </summary>
    public static class KafkaProtocolPrimitiveType
    {
        /// <summary>
        /// fixed width of 1 byte.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int8", Justification = "no need to change.")]
        public const byte TypeInt8Size = 1;

        /// <summary>
        /// fixed width of 2 bytes.
        /// </summary>
        public const byte TypeInt16Size = 2;

        /// <summary>
        /// fixed width of 4 bytes.
        /// </summary>
        public const byte TypeInt32Size = 4;

        /// <summary>
        /// fixed width of 8 bytes.
        /// </summary>
        public const byte TypeInt64Size = 8;

        /// <summary>
        /// variable bytes use int32, which is 4 bytes.
        /// </summary>
        public const byte TypeBytesSize = TypeInt32Size;

        /// <summary>
        /// variable string use int16, which is 2 bytes.
        /// </summary>
        public const byte TypeStringSize = TypeInt16Size;

        /// <summary>
        /// arrays use int32, which is 4 bytes.
        /// </summary>
        public const byte TypeArraySize = TypeInt32Size;

        /// <summary>
        /// Return the byte count of string.
        /// </summary>
        /// <param name="text">The variable string content. </param>
        /// <returns>The byte counts of variable string.</returns>
        public static int GetByteSize(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return TypeStringSize;
            }

            return TypeStringSize + Encoding.UTF8.GetByteCount(text);
        }

        /// <summary>
        /// Return the byte count of array of string.
        /// </summary>
        /// <param name="texts">list of string.</param>
        /// <returns>The byte counts of array type.</returns>
        public static int GetByteSize(string[] texts)
        {
            if (texts == null)
            {
                return TypeArraySize;
            }

            return TypeArraySize + texts.Sum(GetByteSize);
        }

        /// <summary>
        /// Gets the bytes of int32 value.
        /// </summary>
        /// <param name="data">The int32 value.</param>
        /// <returns>The bytes of the int32 value.</returns>
        public static byte[] GetBytes(int data)
        {
            var buffer = BitConverter.GetBytes(data);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }

            return buffer;
        }

        /// <summary>
        /// Gets the bytes of int16 value.
        /// </summary>
        /// <param name="data">The int16 value.</param>
        /// <returns>The bytes of the int16.</returns>
        public static byte[] GetBytes(short data)
        {
            var buffer = BitConverter.GetBytes(data);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }

            return buffer;
        }

        /// <summary>
        /// Gets the bytes of int64 value.
        /// </summary>
        /// <param name="data">The int64 value.</param>
        /// <returns>The bytes of the int64.</returns>
        public static byte[] GetBytes(long data)
        {
            var buffer = BitConverter.GetBytes(data);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }

            return buffer;
        }

        /// <summary>
        /// Gets the int16 value from bytes.
        /// </summary>
        /// <param name="data">The bytes of int16 value.</param>
        /// <returns>The int16 value.</returns>
        public static int GetInt16(byte[] data)
        {
            // data in big edian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }

            // convert bytes to short
            return BitConverter.ToInt16(data, 0);
        }

        /// <summary>
        /// Gets the int32 value from bytes.
        /// </summary>
        /// <param name="data">The bytes of int32 value.</param>
        /// <returns>The int32 value.</returns>
        public static int GetInt32(byte[] data)
        {
            // data in big edian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }

            // convert bytes to int
            return BitConverter.ToInt32(data, 0);
        }

        /// <summary>
        /// Gets the int64 value from bytes.
        /// </summary>
        /// <param name="data">The bytes of int64 value.</param>
        /// <returns>The int64 value.</returns>
        public static long GetInt64(byte[] data)
        {
            // data in big edian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }

            // convert bytes to long
            return BitConverter.ToInt64(data, 0);
        }

        ////static byte[] GetBytes(byte[] data)
        ////{
        ////    if (BitConverter.IsLittleEndian)
        ////    {
        ////        Array.Reverse(data);
        ////    }

        ////    return data;
        ////}
        
        ////static string GetString(byte[] data)
        ////{
        ////    // data in big edian
        ////    if (BitConverter.IsLittleEndian)
        ////    {
        ////        Array.Reverse(data);
        ////    }

        ////    return Encoding.UTF8.GetString(data, 0, data.Length);
        ////}

        ////static string GetString(byte[] data, int start, int length)
        ////{
        ////    // data in big edian
        ////    if (BitConverter.IsLittleEndian)
        ////    {
        ////        Array.Reverse(data, start, length);
        ////    }

        ////    return Encoding.UTF8.GetString(data, start, length);
        ////}
    }
}