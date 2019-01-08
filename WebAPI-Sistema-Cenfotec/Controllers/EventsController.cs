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
    public class EventsController : ApiController
    {
        private DBContext db = new DBContext();


        // GET api/Events
        [HttpGet]
        public IQueryable<evento> Geteventos()
        {
            return db.eventos;
        }

        // GET api/Events/5
        [ResponseType(typeof(evento))]
        public IHttpActionResult Getevento(int id)
        {
            evento evento = db.eventos.Find(id);
            if (evento == null)
            {
                return NotFound();
            }

            return Ok(evento);
        }

        // PUT api/Events/5
        [HttpPut]
        public IHttpActionResult Putevento(int id, evento evento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != evento.id_evento)
            {
                return BadRequest();
            }

            db.Entry(evento).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!eventoExists(id))
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

        // POST api/Events
        [ResponseType(typeof(evento))]
        public IHttpActionResult Postevento(evento evento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.eventos.Add(evento);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = evento.id_evento }, evento);
        }

        // DELETE api/Events/5
        [HttpDelete]
        [ResponseType(typeof(evento))]
        public IHttpActionResult Deleteevento(int id)
        {
            evento evento = db.eventos.Find(id);
            if (evento == null)
            {
                return NotFound();
            }

            db.eventos.Remove(evento);
            db.SaveChanges();

            return Ok(evento);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool eventoExists(int id)
        {
            return db.eventos.Count(e => e.id_evento == id) > 0;
        }
    }
}