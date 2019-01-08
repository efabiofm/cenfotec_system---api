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
    public class FollowUpsController : ApiController
    {
        private DBContext db = new DBContext();

        // GET api/FollowUps
        public IQueryable<seguimiento> Getseguimientos()
        {
            return db.seguimientos;
        }

        // GET api/FollowUps/5
        [ResponseType(typeof(seguimiento))]
        public IHttpActionResult Getseguimiento(int id)
        {
            seguimiento seguimiento = db.seguimientos.Find(id);
            if (seguimiento == null)
            {
                return NotFound();
            }

            return Ok(seguimiento);
        }

        [Route("api/FollowUps/Prospectus/{id:int}")]
        public IQueryable<seguimiento> Getseguimientos(int id)
        {
            return db.seguimientos.Where(s => s.id_prospecto == id);
        }

        // PUT api/FollowUps/5
        public IHttpActionResult Putseguimiento(int id, seguimiento seguimiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != seguimiento.id_seguimiento)
            {
                return BadRequest();
            }
            seguimiento.fecha_actualizacion = DateTime.Now;
            db.Entry(seguimiento).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!seguimientoExists(id))
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

        // POST api/FollowUps
        [ResponseType(typeof(seguimiento))]
        public IHttpActionResult Postseguimiento(seguimiento seguimiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            seguimiento.fecha_actualizacion = DateTime.Now;
            seguimiento.fecha_creacion = DateTime.Now;
            db.seguimientos.Add(seguimiento);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = seguimiento.id_seguimiento }, seguimiento);
        }

        // DELETE api/FollowUps/5
        [ResponseType(typeof(seguimiento))]
        public IHttpActionResult Deleteseguimiento(int id)
        {
            seguimiento seguimiento = db.seguimientos.Find(id);
            if (seguimiento == null)
            {
                return NotFound();
            }

            db.seguimientos.Remove(seguimiento);
            db.SaveChanges();

            return Ok(seguimiento);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool seguimientoExists(int id)
        {
            return db.seguimientos.Count(e => e.id_seguimiento == id) > 0;
        }
    }
}