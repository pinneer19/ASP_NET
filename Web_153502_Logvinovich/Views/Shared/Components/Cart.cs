using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Web_153502_Logvinovich.Views.Home.Components
{

    public class Cart : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }

}
