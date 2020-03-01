using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductBLL.DTO;
using ProductBLL.Repositories;
using ProductBLL.Services;

namespace ProductCatalogAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    //[Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IDataRepository<ProductVM> Repository;
        public static IHostingEnvironment _environment;

        public ProductController(IDataRepository<ProductVM> _Repository, IHostingEnvironment environment)
        {
            Repository = new ProductSerivce();
            Repository = _Repository;
            _environment = environment;

        }
        // GET api/Product
        [HttpGet]
        [ProducesResponseType(typeof(ProductVM), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var entity = await Repository.GetAll();
            if (entity != null)
            {
                return Ok(entity);
            }
            return NoContent();
        }

        // GET api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetById(int id)
        {
            try
            {
                if (id!=0)
                {
                    var entity=await Repository.Get(id);
                    return Ok(entity);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        // POST api/Product
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreatProductFromSwagger([FromForm] ProductRequest product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var image = Request.Form.Files[0];
                    var entity = new ProductVM()
                    {
                        Name = product.Name,
                        Price = product.Price,
                        URL = ImageURL(image)
                    };
                    Repository.Add(entity);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddProduct()
        {
            try
            {
                var image = Request.Form.Files[0];
                string prodcutValues = Request.Form["info"];
                var product = JsonConvert.DeserializeObject<NewProduct>(prodcutValues);
                if (product!=null)
                {
                    var entity = new ProductVM()
                    {
                        Name = product.Name,
                        Price = product.Price,
                        URL = ImageURL(image)
                    };
                    Repository.Add(entity);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // PUT api/Product/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProductFromSwagger(int id, [FromBody] ProductRequest product)
        {
            try
            {
                if (id!=0&& product!=null)
                {
                   // DeleteOldPhoto(id);
                    var image = Request.Form.Files[0];
                    var entity = new ProductVM()
                    {
                        Name = product.Name,
                        Price = product.Price,
                        URL = ImageURL(image)
                    };
                    Repository.Update(entity,id);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpPut]
        public async Task<ActionResult> Put()
        {
            try
            {
                 var image = Request.Form.Files[0];
                 string prodcutValues = Request.Form["info"];
                 var product =  JsonConvert.DeserializeObject<NewProduct>(prodcutValues);
                //DeleteOldPhoto(product.Id);
                if (product!=null)
                {
                    var entity = new ProductVM()
                    {
                        Name = product.Name,
                        Price = product.Price,
                        URL = ImageURL(image)
                    };
                    Repository.Update(entity, product.Id);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet("{value}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<string>>> Search(string value)
        {
            var entity = await Repository.Search(value);
            if (entity != null)
            {
                return Ok(entity);
            }
            return NoContent();
        }

        // DELETE api/Product/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id!=0)
                {
                    Repository.Delete(id);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }

        [NonAction]
        public string ImageURL(IFormFile file)
        {
            string path = "";
            file = Request.Form.Files[0];
            var folderName = Path.Combine("~Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);
                if (!Directory.Exists(_environment.WebRootPath + "\\uploads\\"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\");
                }
                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\uploads\\" + fileName))
                {
                    file.CopyTo(filestream);
                    path = $"/uploads/{fileName}";
                    filestream.Flush();
                }
            }
            return path;
        }
        [NonAction]
        public bool DeleteOldPhoto(int id)
        {
           var path= Repository.GetOldePhotoPath(id);
            string fileName = path.Substring(9);
           // string _imageToBeDelete = _environment.WebRootPath
            string _imageToBeDelete = Path.Combine(_environment.WebRootPath + "\\uploads\\" + fileName);
            if (System.IO.File.Exists(_imageToBeDelete))
            {
                System.IO.File.Delete(fileName);
                return true;
            }
            return false;
        }
    }
}
