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
    /// <summary>
    /// autor: Alejandro Bermudez Vargas
    /// fecha: 1/11/2015
    /// </summary>
    public class ProductsController : ApiController
    {
        private DBContext db = new DBContext();

        // GET api/Products
        public IQueryable<producto> Getproductos()
        {
            return db.productos;
        }

        // GET api/Products/5
        [ResponseType(typeof(producto))]
        public IHttpActionResult Getproducto(int id)
        {
            producto producto = db.productos.Find(id);
            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        // PUT api/Products/5
        public IHttpActionResult Putproducto(int id, producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != producto.id_producto)
            {
                return BadRequest();
            }

            db.Entry(producto).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productoExists(id))
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

        // POST api/Products
        [ResponseType(typeof(producto))]
        public IHttpActionResult Postproducto(producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            tipo_producto t = db.tipo_producto.Find(producto.id_tipo_product);
            producto.tipo_producto = t;
            db.productos.Add(producto);
            
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = producto.id_producto }, producto);
        }

        // DELETE api/Products/5
        [ResponseType(typeof(producto))]
        public IHttpActionResult Deleteproducto(int id)
        {
            producto producto = db.productos.Find(id);
            if (producto == null)
            {
                return NotFound();
            }

            db.productos.Remove(producto);
            db.SaveChanges();

            return Ok(producto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool productoExists(int id)
        {
            return db.productos.Count(e => e.id_producto == id) > 0;
        }


        [Route("api/Sales/totalProductos")]
        [HttpGet]
        public int totalProductos()
        {
            var query = (from a in db.productos
                         select a.id_producto).Count();
            return query;
        }
    }
}