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
    public class RolsController : ApiController
    {
        private DBContext db = new DBContext();

        [ResponseType(typeof(rol))]
        // GET api/Rols
        public IEnumerable<rol> Getrols()
        {
            return db.rols;
        }

        // GET api/Rols/5
        [ResponseType(typeof(rol))]
        public IHttpActionResult Getrol(int id)
        {
            rol rol = db.rols.Find(id);
            if (rol == null)
            {
                return NotFound();
            }
            db.Entry(rol).Collection(p => p.permisos).Load();
            return Ok(rol);
        }
        /// <summary>
        /// autor: Alejandro Bermudez Vargas
        /// fecha: 1/11/2015
        /// </summary>

        // PUT api/Rols/5
        public IHttpActionResult Putrol(int id, rol rol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rol.id_rol)
            {
                return BadRequest();
            }
            
            rol databaseRol = db.rols.Find(rol.id_rol);
            db.Entry(databaseRol).Collection(p => p.permisos).Load();
            int count = databaseRol.permisos.Count;//Porque sino va disminuyendose y se sale a la mitad
            for (int i = 0; i < count; i++)
            {
                databaseRol.permisos.Remove(databaseRol.permisos.ElementAt(0));
            }
            count = rol.permisos.Count;
            for (int i = 0; i < count; i++)
            {
                databaseRol.permisos.Add(db.permisos.Find(rol.permisos.ElementAt(i).id_permiso));
            }
            databaseRol.activo = rol.activo;
            databaseRol.nombre = rol.nombre;
            db.Entry(databaseRol).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!rolExists(id))
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
        /// <summary>
        /// autor: Alejandro Bermudez Vargas
        /// fecha: 1/11/2015
        /// </summary>
        // POST api/Rols
        [ResponseType(typeof(rol))]
        public IHttpActionResult Postrol(rol rol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            for (int i = 0; i < rol.permisos.Count; i++)
            {
                permiso permiso = rol.permisos.ElementAt(i);
                rol.permisos.Remove(permiso);
                rol.permisos.Add(db.permisos.Find(permiso.id_permiso));
            }
            
            db.rols.Add(rol);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = rol.id_rol }, rol);
        }
        /// <summary>
        /// autor: Alejandro Bermudez Vargas
        /// fecha: 1/11/2015
        /// </summary>
        // DELETE api/Rols/5
        [ResponseType(typeof(rol))]
        public IHttpActionResult Deleterol(int id)
        {
            rol rol = db.rols.Find(id);
            if (rol == null)
            {
                return NotFound();
            }

            db.rols.Remove(rol);
            db.SaveChanges();

            return Ok(rol);
        }

        /// <summary>
        /// autor: Alejandro Bermudez Vargas
        /// fecha: 1/11/2015
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool rolExists(int id)
        {
            return db.rols.Count(e => e.id_rol == id) > 0;
        }
    }
}