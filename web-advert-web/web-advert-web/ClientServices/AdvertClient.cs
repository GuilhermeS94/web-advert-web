using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using advertapi_models;
using Microsoft.Extensions.Configuration;
using AutoMapper;

namespace web_advert_web.ClientServices
{
    public class AdvertClient : IAdvertClient
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        public AdvertClient(IConfiguration config, HttpClient client, IMapper mapper)
        {
            _config = config;
            _client = client;
            _mapper = mapper;

            client.BaseAddress = new Uri(_config.GetSection("ApiAnuncios").GetValue<string>("BaseUri"));
            client.DefaultRequestHeaders.Add("Content-type", "application/json");
        }

        public async Task<bool> Confirmar(ConfirmarAnuncioModelRequest confirmar)
        {
            ConfirmarAnuncio entradaAPI = _mapper.Map<ConfirmarAnuncio>(confirmar);
            string jsonEntrada = JsonSerializer.Serialize(entradaAPI);

            HttpResponseMessage respostaAPI = await _client.PutAsync($"{_client.BaseAddress}/confirmar", new StringContent(jsonEntrada)).ConfigureAwait(false);
            

            return respostaAPI.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<CriarAnuncioModelResponse> Criar(CriarAnuncioModel anuncio)
        {
            AdvertModel entradaAPI = _mapper.Map<AdvertModel>(anuncio);
            string jsonEntrada = JsonSerializer.Serialize(entradaAPI);
            
            HttpResponseMessage resposta = await _client.PostAsync($"{_client.BaseAddress}/criar", new StringContent(jsonEntrada)).ConfigureAwait(false);
            string jsonSaidaAPI = await resposta.Content.ReadAsStringAsync();

            CriarAnuncioResponse saidaAPI = JsonSerializer.Deserialize<CriarAnuncioResponse>(jsonSaidaAPI);
            CriarAnuncioModelResponse retorno = _mapper.Map<CriarAnuncioModelResponse>(saidaAPI);

            return retorno;
        }
    }
}
