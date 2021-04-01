using System;
using AutoMapper;
using web_advert_web.ClientServices;
using web_advert_web.Models.Anuncios;

namespace web_advert_web.Classes
{
    public class AnuncioWebPerfil : Profile
    {
        public AnuncioWebPerfil()
        {
            CreateMap<CriarAnuncioModel, CriarAnuncioView>().ReverseMap();
            CreateMap<CriarAnuncioResponse, CriarAnuncioModelResponse>().ReverseMap();
        }
    }
}
