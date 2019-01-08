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
    public class TemplatesController : ApiController
    {
        private DBContext db = new DBContext();

        // GET api/Templates
        public IQueryable<plantilla> Getplantillas()
        {
            return db.plantillas;
        }

        // GET api/Templates/5
        [ResponseType(typeof(plantilla))]
        public IHttpActionResult Getplantilla(int id)
        {
            plantilla plantilla = db.plantillas.Find(id);
            if (plantilla == null)
            {
                return NotFound();
            }
            db.Entry(plantilla).Collection(p => p.preguntas).Load();
            return Ok(plantilla);
        }

        // PUT api/Templates/5
        public IHttpActionResult Putplantilla(int id, plantilla plantilla)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != plantilla.id_plantilla) return BadRequest();
            plantilla databaseTemplate = db.plantillas.Find(plantilla.id_plantilla);
            db.Entry(databaseTemplate).Collection(p => p.preguntas).Load();
            int count = databaseTemplate.preguntas.Count;
            for (int i = 0; i < count; i++)
            {
                databaseTemplate.preguntas.Remove(databaseTemplate.preguntas.ElementAt(0));
            }
            count = plantilla.preguntas.Count;
            for (int j = 0; j < count; j++)
            {
                databaseTemplate.preguntas.Add(db.preguntas.Find(plantilla.preguntas.ElementAt(j).id_pregunta));
            }
            databaseTemplate.nombre = plantilla.nombre;
            databaseTemplate.descripcion = plantilla.descripcion;
            db.Entry(databaseTemplate).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!plantillaExists(id))
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

        // POST api/Templates
        [ResponseType(typeof(plantilla))]
        public IHttpActionResult Postplantilla(plantilla plantilla)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            for (int i = 0; i < plantilla.preguntas.Count; i++)
            {
                pregunta pregunta = plantilla.preguntas.ElementAt(i);
                plantilla.preguntas.Remove(pregunta);
                plantilla.preguntas.Add(db.preguntas.Find(pregunta.id_pregunta));
            }
            db.plantillas.Add(plantilla);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = plantilla.id_plantilla }, plantilla);
        }

        // DELETE api/Templates/5
        [ResponseType(typeof(plantilla))]
        public IHttpActionResult Deleteplantilla(int id)
        {
            plantilla plantilla = db.plantillas.Find(id);
            if (plantilla == null)
            {
                return NotFound();
            }

            db.plantillas.Remove(plantilla);
            db.SaveChanges();

            return Ok(plantilla);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool plantillaExists(int id)
        {
            return db.plantillas.Count(e => e.id_plantilla == id) > 0;
        }
    }
}