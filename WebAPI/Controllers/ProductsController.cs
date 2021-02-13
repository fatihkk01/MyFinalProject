using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet]
        public List<Product> Get()
        {
            //Dependency chain = Bağımlılık zinciri
            
            var result = _productService.GetAll();
            return result.Data;
        }
    }
}
