using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Foundation.Options;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Flash.Central.Core.Services
{
    public class S3PictureService : IPictureService
    {
        private readonly S3ClientOptions _clientOptions;
        private readonly AmazonS3Client _s3Client;

        public S3PictureService(IOptions<S3ClientOptions> clientOptions)
        {
            _clientOptions = clientOptions.Value;
            //_s3Client = ConfigureS3Client(_clientOptions);
        }

        public async Task<string> Save(byte[] image, CancellationToken ct)
        {
            using var memoryStream = new MemoryStream();
            var key = $"{Guid.NewGuid()}.jpg";

            memoryStream.Seek(0, SeekOrigin.End);
            await memoryStream.WriteAsync(image, 0, image.Length);

            var request = new PutObjectRequest()
            {
                CannedACL = S3CannedACL.PublicRead,
                InputStream = memoryStream,
                BucketName = _clientOptions.BucketName,
                Key = key
            };
            await _s3Client.PutObjectAsync(request, ct);

            return key;
        }

        public async Task<bool> Delete(string key, CancellationToken ct)
        {
            await _s3Client.DeleteObjectAsync(_clientOptions.BucketName, key, ct);

            return true;
        }

        public Task<string> GetImagesUploadPath(CancellationToken ct)
        {
            return Task.FromResult(_clientOptions.ServiceURL);
        }

        private AmazonS3Client ConfigureS3Client(S3ClientOptions options)
        {
            var configuration = new AmazonS3Config()
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(options.RegionEndpoint),
                ServiceURL = options.ServiceURL,
                ForcePathStyle = options.ForcePathStyle
            };

            var credentials = new BasicAWSCredentials(options.AccessKey, options.SecretKey);

            return new AmazonS3Client(credentials, configuration);
        }
    }
}
