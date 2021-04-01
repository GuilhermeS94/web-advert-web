using System;
using advertapi_models;
using AutoMapper;
namespace web_advert_web.ClientServices
{
    public class AnuncioApiPerfil : Profile
    {
        public AnuncioApiPerfil()
        {
            CreateMap<AdvertModel, CriarAnuncioModel>().ReverseMap();
            CreateMap<CriarAnuncioResponse, CriarAnuncioModelResponse>().ReverseMap();
            CreateMap<ConfirmarAnuncio, ConfirmarAnuncioModelRequest>().ReverseMap();
        }
    }
}
