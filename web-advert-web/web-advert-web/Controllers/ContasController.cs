using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using web_advert_web.Models.Contas;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web_advert_web.Controllers
{
    public class ContasController : Controller
    {
        private readonly SignInManager<CognitoUser> _loginManager;
        private readonly UserManager<CognitoUser> _usuarioManager;
        private readonly CognitoUserPool _pool;
        public ContasController(SignInManager<CognitoUser> loginManager, UserManager<CognitoUser> usuarioManager, CognitoUserPool pool)
        {
            _loginManager = loginManager;
            _usuarioManager = usuarioManager;
            _pool = pool;
        }

        public async Task<IActionResult> Cadastrar()
        {
            Cadastrar model = new Cadastrar();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(Cadastrar cadastrar)
        {
            if (ModelState.IsValid)
            {
                CognitoUser usuario = _pool.GetUser(cadastrar.Email);
                if(usuario.Status != null)
                {
                    ModelState.AddModelError("UsuarioExiste", "Usuario ja cadastrado");
                    return View(cadastrar);
                }

                usuario.Attributes.Add("name", cadastrar.Email);
                IdentityResult criou = await _usuarioManager.CreateAsync(usuario, cadastrar.Senha).ConfigureAwait(false);
                

                if (criou.Succeeded)
                {
                    return RedirectToAction("ConfirmarUsuario");
                }
                else
                {
                    foreach (IdentityError erro in criou.Errors)
                    {
                        ModelState.AddModelError(erro.Code, erro.Description);
                    }
                }
            }

            return View(cadastrar);
        }

        public async Task<IActionResult> ConfirmarUsuario(ConfirmarUsuario confirmar)
        { 
            return View(confirmar);
        }

        [HttpPost]
        [ActionName("ConfirmarUsuario")]
        public async Task<IActionResult> ConfirmarUsuarioPost(ConfirmarUsuario confirmar)
        {
            if (ModelState.IsValid)
            {
                CognitoUser usuario = await _usuarioManager.FindByEmailAsync(confirmar.Email).ConfigureAwait(false);
                if(usuario == null)
                {
                    ModelState.AddModelError("NaoEncontrado", "Usuario nao encontrado");
                    return View(confirmar);
                }

                IdentityResult confirmou = await (_usuarioManager as CognitoUserManager<CognitoUser>)
                    .ConfirmSignUpAsync(usuario, confirmar.Codigo, true).ConfigureAwait(false);

                if (confirmou.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (IdentityError erro in confirmou.Errors)
                    {
                        ModelState.AddModelError(erro.Code, erro.Description);
                    }
                }
            }

            return View(confirmar);
        }

        [HttpGet]
        public async Task<IActionResult> Login(Login login)
        {
            return View(login);
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginPost(Login login)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult loginSucesso = await _loginManager.PasswordSignInAsync(login.Email, login.Senha, login.LembrarMe, false).ConfigureAwait(false);

                if (loginSucesso.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {    
                    ModelState.AddModelError("LoginErro", "Email ou senha invalidos");
                }
            }

            return View("Login", login);
        }
    }
}
