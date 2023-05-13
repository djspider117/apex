using System;
using System.IO;
using System.Security.Cryptography;

namespace APEX.Client.Windows.Services
{
    public sealed class MD5ChecksumProvider : IChecksumProvider, IDisposable
    {
        private MD5 _hashFunction;

        public MD5ChecksumProvider()
        {
            _hashFunction = MD5.Create();
        }

        public string GetChecksum(string filePath)
        {
            using var stream = new FileStream(filePath, new FileStreamOptions { BufferSize = 1200000 });

            var checksum = _hashFunction.ComputeHash(stream);
            return BitConverter.ToString(checksum).Replace("-", string.Empty);
        }

        public void Dispose()
        {
            _hashFunction.Dispose();
            _hashFunction = null;
        }
    }
}

