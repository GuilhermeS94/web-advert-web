using System;
using System.Threading.Tasks;

namespace web_advert_web.ClientServices
{
    public interface IAdvertClient
    {
        Task<CriarAnuncioModelResponse> Criar(CriarAnuncioModel anuncio);
        Task<bool> Confirmar(ConfirmarAnuncioModelRequest confirmar);
    }
}
