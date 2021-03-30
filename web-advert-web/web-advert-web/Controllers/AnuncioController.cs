using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_advert_web.Models.Anuncios;
using web_advert_web.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web_advert_web.Controllers
{
    public class AnuncioController : Controller
    {
        private readonly IFileUploader _upFile;

        public AnuncioController(IFileUploader upFile)
        {
            _upFile = upFile;
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
                string id = "";
                string nomeArquivo = "";
                string nomeArquivoCompleto = "";

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

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return View(anuncio);
        }
    }
}
