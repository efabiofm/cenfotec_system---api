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
    public class QuestionsController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/Questions
        public IQueryable<pregunta> Getpreguntas()
        {
            return db.preguntas;
        }

        // GET: api/Questions/5
        [ResponseType(typeof(pregunta))]
        public IHttpActionResult Getpregunta(int id)
        {
            pregunta pregunta = db.preguntas.Find(id);
            if (pregunta == null)
            {
                return NotFound();
            }

            return Ok(pregunta);
        }

        // PUT: api/Questions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putpregunta(int id, pregunta pregunta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pregunta.id_pregunta)
            {
                return BadRequest();
            }

            db.Entry(pregunta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!preguntaExists(id))
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

        // POST: api/Questions
        [ResponseType(typeof(pregunta))]
        public IHttpActionResult Postpregunta(pregunta pregunta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.preguntas.Add(pregunta);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pregunta.id_pregunta }, pregunta);
        }

        // DELETE: api/Questions/5
        [ResponseType(typeof(pregunta))]
        public IHttpActionResult Deletepregunta(int id)
        {
            pregunta pregunta = db.preguntas.Find(id);
            if (pregunta == null)
            {
                return NotFound();
            }

            db.preguntas.Remove(pregunta);
            db.SaveChanges();

            return Ok(pregunta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool preguntaExists(int id)
        {
            return db.preguntas.Count(e => e.id_pregunta == id) > 0;
        }
    }
}