using System;
using System.IO;
using System.Threading.Tasks;

namespace web_advert_web.Services
{
    public interface IFileUploader
    {
        Task<bool> EnviarArquivo(string nomeArquivo, Stream arquivo);
    }
}
