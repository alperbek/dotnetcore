using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fabrika.Models;

namespace Fabrika.Controllers
{
    [Route("Fabrika/restapi/[controller]")]
    public class ProductsController : Controller
    {
		private readonly FabrikaContext _context;
		
		public ProductsController(FabrikaContext context)
		{
			_context=context;
			
			if(_context.Products.Count()==0)
			{
				_context.Products.Add(new Product{Id=19201,Name="Lego Nexo Knights King I",UnitPrice=45});
				_context.Products.Add(new Product{Id=23942,Name="Lego Starwars Minifigure Jedi",UnitPrice=55});
				_context.Products.Add(new Product{Id=30021,Name="Star Wars �ay tak�m� ",UnitPrice=35.50});
				_context.Products.Add(new Product{Id=30492,Name="Star Wars kahve tak�m�",UnitPrice=24.40});
				
				_context.SaveChanges();
			}
		}
        
		/// <summary>
		/// T�m �r�n listesini verir
		/// </summary>
		/// <returns>Envanterin tamam�</returns>
        [HttpGet]
        public IEnumerable<Product> Get()
        {
			return _context.Products.ToList();
        }

        /// <summary>
		/// Id'ye g�re bir �r�n bilgisini verir
		/// </summary>
		/// <param name="id"></param>
        [HttpGet("{id}",Name="GetProduct")]
        public IActionResult Get(int id)
        {
			var product=_context.Products.FirstOrDefault(t=>t.Id==id);
			if(product==null)
			{
				return NotFound();
			}
			return new ObjectResult(product);
        }

		/// <summary>
		/// Yeni bir �r�n eklemek i�in kullan�l�r
		/// </summary>
		/// <remarks>
		/// Sample request:
		///  POST /Products
		///  {
		///  	"Id":1012,
		///     "Name":"�r�n ad�",
		///		"UnitPrice":49.99
		///	 }
		///
		/// </remarks>
		/// <param name="newProduct">Yeni �r�n bilgisi</param>
		/// <response code="201">Yeni olu�turulan �r�n bilgisi</response>
		/// <response code="400">Bir ��e g�nderilmedi</response>
        [HttpPost]
		[ProducesResponseType(typeof(Product),201)]
		[ProducesResponseType(typeof(Product),400)]
        public IActionResult Post([FromBody]Product newProduct)
        {
			if(newProduct==null)
				return BadRequest();

			_context.Products.Add(newProduct);
			_context.SaveChanges();
			return CreatedAtRoute("GetProduct",new {id=newProduct.Id},newProduct);				
        }

		/// <summary>
		/// Bir �r�n� g�nceller
		/// </summary>
		/// <param name="id">G�ncellenecek �r�n id</param>
		/// <param name="value">�r�n bilgisi</param>
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody]Product value)
        {
			if(value==null||value.Id!=id)
				return BadRequest();
				
			var product=_context.Products.FirstOrDefault(p=>p.Id==id);
			if(product==null)
				return NotFound();
			
			product.Name=value.Name;
			product.UnitPrice=value.UnitPrice;
			
			_context.Products.Update(product);
			_context.SaveChanges();
			
			return new NoContentResult();
        }

		/// <summary>
		/// Bir �r�n� silmek i�in kullan�l�r
		/// </summary>
		/// <param name="id">�r�n id'si</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
			var product=_context.Products.FirstOrDefault(p=>p.Id==id);
			if(product==null)
				return NotFound();
			
			_context.Products.Remove(product);
			_context.SaveChanges();
			
			return new NoContentResult();
        }
    }
}