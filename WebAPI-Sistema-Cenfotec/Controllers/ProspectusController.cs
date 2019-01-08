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
    public class ProspectusController : ApiController
    {
        private DBContext db = new DBContext();

        // GET api/Prospectus
        public IQueryable<prospecto> Getprospectos()
        {
            return db.prospectos;
        }

        // GET api/Prospectus/5
        [ResponseType(typeof(prospecto))]
        public IHttpActionResult Getprospecto(int id)
        {
            prospecto prospecto = db.prospectos.Find(id);
            db.Entry(prospecto).Collection(i => i.tipo_producto).Load();
            if (prospecto == null)
            {
                return NotFound();
            }

            return Ok(prospecto);
        }

        // PUT api/Prospectus/5
        public IHttpActionResult Putprospecto(int id, prospecto prospecto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != prospecto.id_prospecto)
            {
                return BadRequest();
            }

            prospecto prospectoBD = db.prospectos.Find(id);
            prospectoBD.id_evento = prospecto.id_evento;

            if (prospecto.tipo_producto != null)
            {
                
                db.Entry(prospectoBD).Collection(p => p.tipo_producto).Load();

                int cantIntereses = prospectoBD.tipo_producto.Count;
                for (int x = 0; x < cantIntereses; x++)
                {
                    prospectoBD.tipo_producto.Remove(prospectoBD.tipo_producto.ElementAt(0));
                }

                for (int i = 0; i < prospecto.tipo_producto.Count; i++)
                {
                    tipo_producto tp = prospecto.tipo_producto.ElementAt(i);
                    prospectoBD.tipo_producto.Add(db.tipo_producto.Find(tp.id_tipo_producto));
                }
            }

            if (prospectoBD.id_evento == 0)
            {
                prospectoBD.id_evento = null;
            }
            db.Entry(prospectoBD).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!prospectoExists(id))
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

        // POST api/Prospectus
        [ResponseType(typeof(prospecto))]
        public IHttpActionResult Postprospecto(prospecto prospecto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (prospecto.tipo_producto != null)
            {
                for (int i = 0; i < prospecto.tipo_producto.Count; i++)
                {
                    tipo_producto tp = prospecto.tipo_producto.ElementAt(i);
                    prospecto.tipo_producto.Remove(tp);
                    prospecto.tipo_producto.Add(db.tipo_producto.Find(tp.id_tipo_producto));
                }
            }
            if (prospecto.id_evento == 0) 
            {
                prospecto.id_evento = null;
            }
            try
            {
                prospecto.fecha_creacion = DateTime.Now;
                prospecto.fecha_actualizacion = DateTime.Now;
                db.prospectos.Add(prospecto);
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (prospectoExists(prospecto.id_prospecto))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = prospecto.id_prospecto }, prospecto);
        }

        // DELETE api/Prospectus/5
        [ResponseType(typeof(prospecto))]
        public IHttpActionResult Deleteprospecto(int id)
        {
            prospecto prospecto = db.prospectos.Find(id);
            if (prospecto == null)
            {
                return NotFound();
            }

            db.prospectos.Remove(prospecto);
            db.SaveChanges();

            return Ok(prospecto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool prospectoExists(int id)
        {
            return db.prospectos.Count(e => e.id_prospecto == id) > 0;
        }

        [Route("api/Sales/getTotalProspectosClientes")]
        [HttpGet]
        public int getTotalProspectosClientes()
        {
            var total = (from a in db.prospectos
                         where a.cliente == true
                         select a).Count();
            return total;
        }


        [Route("api/Sales/getTotalProspectos")]
        [HttpGet]
        public int getTotalProspectos()
        {
            var total = (from a in db.prospectos
                         select a).Count();
            return total;
        }
    }
}