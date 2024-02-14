using Microsoft.AspNetCore.Mvc;
using NetCoreLinqToSqlInjection.Models;

namespace NetCoreLinqToSqlInjection.Controllers
{
    public class CochecitosController : Controller
    {
        private ICoche car;

        public CochecitosController(ICoche coche)
        {
            this.car = coche;
        }

        public IActionResult Index()
        {
            return View(this.car);
        }

        [HttpPost]
        public IActionResult Index(string accion)
        {
            if (accion.ToLower() == "acelerar")
            {
                this.car.Acelerar();
            }
            else
            {
                this.car.Frenar();
            }
            return View(this.car);
        }
    }
}
