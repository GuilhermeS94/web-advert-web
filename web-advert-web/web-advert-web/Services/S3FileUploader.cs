using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Amazon.S3;
using Amazon.S3.Model;

namespace web_advert_web.Services
{
    public class S3FileUploader : IFileUploader
    {
        private readonly IConfiguration _config;

        public S3FileUploader(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> EnviarArquivo(string nomeArquivo, Stream arquivo)
        {
            bool saida = true;
            if (string.IsNullOrEmpty(nomeArquivo)) throw new ArgumentException("Nome do arquivo deve ser informado.");

            string bucket = _config.GetValue<string>("ImageBucket");

            using (AmazonS3Client s3Client = new AmazonS3Client())
            {
                if (arquivo.Length > 0)
                    if (arquivo.CanSeek)
                        arquivo.Seek(0, SeekOrigin.Begin);

                PutObjectRequest s3Request = new PutObjectRequest {
                    AutoCloseStream = true,
                    BucketName = bucket,
                    InputStream = arquivo,
                    Key = nomeArquivo
                };

                PutObjectResponse s3Response = await s3Client.PutObjectAsync(s3Request).ConfigureAwait(false);
                saida = s3Response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }

            return saida;
        }
    }
}
