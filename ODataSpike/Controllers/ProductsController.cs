using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using ODataSpike.Models;

namespace ODataSpike.Controllers
{
    public class ProductsController: ODataController
    {
        private ProductsContext db = new ProductsContext();

        [EnableQuery]
        public IQueryable<Product> Get()
        {
            return db.Products;
        }

        [EnableQuery]
        public SingleResult<Product> Get([FromODataUri] int key)
        {
            var result = db.Products.Where(p=>p.Id==key);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return Created(product);
        } 

        private bool ProductExists(int key)
        {
            return db.Products.Any(p => p.Id == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}