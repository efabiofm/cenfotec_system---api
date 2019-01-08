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
    public class kpisController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/kpis
        public IQueryable<kpi> Getkpis()
        {
            return db.kpis;
        }

        // GET: api/kpis/5
        [ResponseType(typeof(kpi))]
        public IHttpActionResult Getkpi(int id)
        {
            kpi kpi = db.kpis.Find(id);
            if (kpi == null)
            {
                return NotFound();
            }

            return Ok(kpi);
        }

        // PUT: api/kpis/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putkpi(int id, kpi kpi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kpi.id_kpi)
            {
                return BadRequest();
            }

            db.Entry(kpi).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!kpiExists(id))
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

        // POST: api/kpis
        [ResponseType(typeof(kpi))]
        public IHttpActionResult Postkpi(kpi kpi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.kpis.Add(kpi);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (kpiExists(kpi.id_kpi))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = kpi.id_kpi }, kpi);
        }

        // DELETE: api/kpis/5
        [ResponseType(typeof(kpi))]
        public IHttpActionResult Deletekpi(int id)
        {
            kpi kpi = db.kpis.Find(id);
            if (kpi == null)
            {
                return NotFound();
            }

            db.kpis.Remove(kpi);
            db.SaveChanges();

            return Ok(kpi);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool kpiExists(int id)
        {
            return db.kpis.Count(e => e.id_kpi == id) > 0;
        }

        [Route("api/Kpis/getKPIVentas")]
        [HttpGet]
        public List<kpi> getKPIVentas()
        {
            //var results = db.kpis.Select(x => new { x.id_kpi, x.descripcion, x.indicador }).ToList();

            return (from a in db.kpis
                    where a.id_tipo == 1
                       select a).ToList();
            
        }
    }
}