using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_advert_web.ClientServices;
using web_advert_web.Models.Anuncios;
using web_advert_web.Services;
using AutoMapper;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web_advert_web.Controllers
{
    public class AnuncioController : Controller
    {
        private readonly IFileUploader _upFile;
        private readonly IAdvertClient _advApiClient;
        private readonly IMapper _mapper;

        public AnuncioController(IFileUploader upFile, IAdvertClient advApiClient, IMapper mapper)
        {
            _upFile = upFile;
            _advApiClient = advApiClient;
            _mapper = mapper;
        }

        public IActionResult Criar(CriarAnuncioView anuncio)
        {
            return View(anuncio);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CriarAnuncioView anuncio, IFormFile arquivo)
        {
            if (ModelState.IsValid)
            {
                string nomeArquivo = "";
                string nomeArquivoCompleto = "";
                CriarAnuncioModel entradaAPI = _mapper.Map<CriarAnuncioModel>(anuncio);
                CriarAnuncioModelResponse saidaAPI = await _advApiClient.Criar(entradaAPI).ConfigureAwait(false);
                string id = saidaAPI.Id;

                if (arquivo != null)
                {
                    nomeArquivo = (!string.IsNullOrEmpty(arquivo.FileName)) ? Path.GetFileName(arquivo.FileName) : id;
                    nomeArquivoCompleto = $"{id}/{nomeArquivo}";

                    try
                    {
                        using (Stream rs = arquivo.OpenReadStream())
                        {
                            bool enviou = await _upFile.EnviarArquivo(nomeArquivoCompleto, rs).ConfigureAwait(false);
                            if (!enviou)
                                throw new Exception("Nao foi possivel enviar o arquivo.");
                        }

                        var confirmar = new ConfirmarAnuncioModelRequest {
                            Id = id,
                            Status = StatusAnuncio.Ativo
                        };
                        bool confirmarAPI = await _advApiClient.Confirmar(confirmar).ConfigureAwait(false);

                        if (!confirmarAPI)
                        {
                            throw new Exception($"Nao pode realizar confirmacao do anuncio ID: {id}");
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        var confirmar = new ConfirmarAnuncioModelRequest
                        {
                            Id = id,
                            Status = StatusAnuncio.Pendente
                        };
                        bool confirmarAPI = await _advApiClient.Confirmar(confirmar).ConfigureAwait(false);
                    }
                }
            }

            return View(anuncio);
        }
    }
}
