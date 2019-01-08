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
    public class tipo_kpiController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/tipo_kpi
        public IQueryable<tipo_kpi> Gettipo_kpi()
        {
            return db.tipo_kpi;
        }

        // GET: api/tipo_kpi/5
        [ResponseType(typeof(tipo_kpi))]
        public IHttpActionResult Gettipo_kpi(int id)
        {
            tipo_kpi tipo_kpi = db.tipo_kpi.Find(id);
            if (tipo_kpi == null)
            {
                return NotFound();
            }

            return Ok(tipo_kpi);
        }

        // PUT: api/tipo_kpi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttipo_kpi(int id, tipo_kpi tipo_kpi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipo_kpi.id_tipo)
            {
                return BadRequest();
            }

            db.Entry(tipo_kpi).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tipo_kpiExists(id))
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

        // POST: api/tipo_kpi
        [ResponseType(typeof(tipo_kpi))]
        public IHttpActionResult Posttipo_kpi(tipo_kpi tipo_kpi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tipo_kpi.Add(tipo_kpi);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tipo_kpiExists(tipo_kpi.id_tipo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tipo_kpi.id_tipo }, tipo_kpi);
        }

        // DELETE: api/tipo_kpi/5
        [ResponseType(typeof(tipo_kpi))]
        public IHttpActionResult Deletetipo_kpi(int id)
        {
            tipo_kpi tipo_kpi = db.tipo_kpi.Find(id);
            if (tipo_kpi == null)
            {
                return NotFound();
            }

            db.tipo_kpi.Remove(tipo_kpi);
            db.SaveChanges();

            return Ok(tipo_kpi);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tipo_kpiExists(int id)
        {
            return db.tipo_kpi.Count(e => e.id_tipo == id) > 0;
        }
    }
}