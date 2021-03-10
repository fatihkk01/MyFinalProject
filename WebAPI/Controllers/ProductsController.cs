using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
namespace WebAPI.Controllers
{
    //Controller isimleri çoğul olur örn -> Products
    //Attribute : Bir class ile ilgil bilgi verme ve imzalama yöntemidir.
    //Route : Bu isteği yaparken insanların apiye nasıl ulaşacağını belirlediğimiz kısımdır.
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //Loosly coupled = Gevşek bağımlılık 
        //Naming convention = İsimlendirme kuralı
        //IoC Container = Inversion of Control
        IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        //Dışarıdan birisinin nasıl çağıracağını söylemiş oldurk
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            //Swagger 
            //Dependency chain = Bağımlılık zinciri

            Thread.Sleep(5000);

            var result = _productService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(Product product)
        {
            var result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

    }
}
