using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

namespace ControleDeContatos.ViewComponents
{
    public class Menu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sessaoUsuario = HttpContext.Session.GetString("sessaoUsuarioLogado");

            if (string.IsNullOrEmpty(sessaoUsuario)) return null;

            UsuarioModel model = JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);
            return View(model);
        }
    }
}