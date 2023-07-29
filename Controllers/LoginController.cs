using System.Text.RegularExpressions;
using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace ControleDeContatos.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;
        public LoginController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }
        public IActionResult Index()
        {
            //Se o usuario estiver logado, redirtecionar para Home
            if (_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "Home");
            return View();
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoDoUsuario();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorLogin(loginModel.Login);

                    if (usuario != null)
                    {
                        if (usuario.SenhaValida(loginModel.Senha))
                        {
                            _sessao.CriarSessaoDoUsuario(usuario);
                            return RedirectToAction("Index", "Home");
                        }

                        TempData["MensagemErro"] = "Senha invalida";
                    }
                    TempData["MensagemErro"] = "Login e/ou senha invalido(os)";
                }
                return View("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Dados inválidos erro:{erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
