using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

using NLog;
using Swashbuckle.Swagger.Annotations;

using ODataProductService.Models;
using ODataProductService.Repository;

namespace ODataProductService.Controllers
{
    public class ProductsController : ODataController
    {
        private readonly ILogger _log;
        IProductRepository _repository;

        public ProductsController()
        {

        }

        public ProductsController(IProductRepository repository, ILogger log)
        {
            _repository = repository;
            _log = log;
        }

        #region Get entity/entities
        [ODataRoute("Products/")]
        [EnableQuery]
        [SwaggerOperation("GetAll")]
        public IEnumerable<Product> Get()
        {
            _log.Info("ProductsController: Entering Get...");
            return _repository.GetAll();
        }

        [EnableQuery]
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound, "Product not found.")]
        public IHttpActionResult Get([FromODataUri] int key)
        {
            Product result = _repository.GetById(key);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        #endregion


        #region Create entity
        public async Task<IHttpActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.AddAsync(product);
            return CreatedAtRoute("ODataRoute", new { id = product.Id }, product);
        }
        #endregion

        #region Update entity
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Product> product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _repository.GetByIdAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            product.Patch(entity);
            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }

                throw;
            }
            return Updated(entity);
        }

        //public async Task<IHttpActionResult> Put([FromODataUri] int key, Product update)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    if (key != update.Id)
        //    {
        //        return BadRequest();
        //    }
        //    db.Entry(update).State = EntityState.Modified;
        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductExists(key))
        //        {
        //            return NotFound();
        //        }

        //        throw;
        //    }
        //    return Updated(update);
        //}
        #endregion

        #region Delete entity
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var product = await _repository.GetByIdAsync(key);
            if (product == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(product);
            return StatusCode(HttpStatusCode.NoContent);
        }
        #endregion

        private bool ProductExists(int key)
        {
            return _repository.GetAll().Any(p => p.Id == key);
        }
        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}