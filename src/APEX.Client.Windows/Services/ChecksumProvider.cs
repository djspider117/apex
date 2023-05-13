using System;
using System.IO;
using System.Security.Cryptography;

namespace APEX.Client.Windows.Services
{
    public sealed class ChecksumProvider : IChecksumProvider, IDisposable
    {
        private HashAlgorithm _hashFunction;

        public ChecksumProvider()
        {
            _hashFunction = SHA256.Create();
        }

        public string GetChecksum(string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Open);

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

