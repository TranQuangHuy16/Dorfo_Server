using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dorfo.Shared.Helpers
{
    public static class CloudinaryStorageHelper
    {
        /// <summary>
        /// Upload image từ byte[] trực tiếp lên Cloudinary
        /// </summary>
        public static async Task<string> UploadImageAsync(byte[] fileBytes, string fileName, IConfiguration config)
        {
            var cloudName = config["Cloudinary:CloudName"];
            var apiKey = config["Cloudinary:ApiKey"];
            var apiSecret = config["Cloudinary:ApiSecret"];

            var cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));

            using var stream = new MemoryStream(fileBytes);
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                PublicId = $"uploads/{Guid.NewGuid()}",
                Overwrite = true
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.AbsoluteUri;
        }
    }
}
