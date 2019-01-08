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
    public class SalesController : ApiController
    {
        private DBContext db = new DBContext();

        // GET api/Sales
        public IQueryable<venta> Getventas()
        {
            return db.ventas;
        }

        // GET api/Sales/5
        [ResponseType(typeof(venta))]
        public IHttpActionResult Getventa(int id)
        {
            venta venta = db.ventas.Find(id);
            if (venta == null)
            {
                return NotFound();
            }

            return Ok(venta);
        }

        // PUT api/Sales/5
        public IHttpActionResult Putventa(int id, venta venta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != venta.id_venta)
            {
                return BadRequest();
            }

            db.Entry(venta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ventaExists(id))
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

        // POST api/Sales
        [ResponseType(typeof(venta))]
        public IHttpActionResult Postventa(venta venta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ventas.Add(venta);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = venta.id_venta }, venta);
        }

        // DELETE api/Sales/5
        [ResponseType(typeof(venta))]
        public IHttpActionResult Deleteventa(int id)
        {
            venta venta = db.ventas.Find(id);
            if (venta == null)
            {
                return NotFound();
            }

            db.ventas.Remove(venta);
            db.SaveChanges();

            return Ok(venta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ventaExists(int id)
        {
            return db.ventas.Count(e => e.id_venta == id) > 0;
        }

        [Route("api/Sales/getTotalMontoVentas")]
        [HttpGet]
        public decimal gettotalMontoVentas()
        {
            var query = (from a in db.ventas
                         select a.monto).Sum();
            if (query.Equals(null))
            {
                return 0;
            }
            else
            {
                return query.Value;
            }

        }

        [Route("api/Sales/getTotalVentas")]
        [HttpGet]
        public int getTotalVentas()
        {
            var query = (from a in db.ventas
                         select a.id_venta).Count();
            return query;
        }
    }
}