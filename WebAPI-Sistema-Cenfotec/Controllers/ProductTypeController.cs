using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI_Sistema_Cenfotec.Models;

namespace WebAPI_Sistema_Cenfotec.Controllers
{
    public class ProductTypeController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/ProductType
        public IQueryable<tipo_producto> Gettipo_producto()
        {

            return db.tipo_producto;
        }

        // GET: api/ProductType/5
        [ResponseType(typeof(tipo_producto))]
        public IHttpActionResult Gettipo_producto(int id)
        {
            tipo_producto tipo_producto = db.tipo_producto.Find(id);
            if (tipo_producto == null)
            {
                return NotFound();
            }

            return Ok(tipo_producto);
        }

        // PUT: api/ProductType/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttipo_producto(int id, tipo_producto tipo_producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipo_producto.id_tipo_producto)
            {
                return BadRequest();
            }

            db.Entry(tipo_producto).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tipo_productoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProductType
        [ResponseType(typeof(tipo_producto))]
        public IHttpActionResult Posttipo_producto(tipo_producto tipo_producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tipo_producto.Add(tipo_producto);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipo_producto.id_tipo_producto }, tipo_producto);
        }

        // DELETE: api/ProductType/5
        [ResponseType(typeof(tipo_producto))]
        public IHttpActionResult Deletetipo_producto(int id)
        {
            tipo_producto tipo_producto = db.tipo_producto.Find(id);
            if (tipo_producto == null)
            {
                return NotFound();
            }

            db.tipo_producto.Remove(tipo_producto);
            db.SaveChanges();

            return Ok(tipo_producto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tipo_productoExists(int id)
        {
            return db.tipo_producto.Count(e => e.id_tipo_producto == id) > 0;
        }
    }
}