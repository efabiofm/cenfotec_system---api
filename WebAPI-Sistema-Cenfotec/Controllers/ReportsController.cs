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
    public class ReportsController : ApiController
    {
        private DBContext db = new DBContext();

        // GET api/Reports
        public IQueryable<reporte> Getreportes()
        {
            return db.reportes;
        }

        // GET api/Reports/5
        [ResponseType(typeof(reporte))]
        public IHttpActionResult Getreporte(int id)
        {
            reporte reporte = db.reportes.Find(id);
            if (reporte == null)
            {
                return NotFound();
            }

            return Ok(reporte);
        }

        // PUT api/Reports/5
        public IHttpActionResult Putreporte(int id, reporte reporte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reporte.id_reporte)
            {
                return BadRequest();
            }

            db.Entry(reporte).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!reporteExists(id))
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

        // POST api/Reports
        [ResponseType(typeof(reporte))]
        public IHttpActionResult Postreporte(reporte reporte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.reportes.Add(reporte);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (reporteExists(reporte.id_reporte))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = reporte.id_reporte }, reporte);
        }

        // DELETE api/Reports/5
        [ResponseType(typeof(reporte))]
        public IHttpActionResult Deletereporte(int id)
        {
            reporte reporte = db.reportes.Find(id);
            if (reporte == null)
            {
                return NotFound();
            }

            db.reportes.Remove(reporte);
            db.SaveChanges();

            return Ok(reporte);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool reporteExists(int id)
        {
            return db.reportes.Count(e => e.id_reporte == id) > 0;
        }
    }
}