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
using System.Security.Cryptography;
using System.IO;
using System.Text;
using WebAPI_Sistema_Cenfotec.Controllers.Logic;
using WebAPI_Sistema_Cenfotec.Recursos;

namespace WebAPI_Sistema_Cenfotec.Controllers
{
    /// <summary>
    /// autor: Alejandro Bermudez Vargas
    /// fecha: 1/11/2015
    /// </summary>
    public class UsersController : ApiController
    {
        private DBContext db = new DBContext();

        // GET api/Users
        public IQueryable<usuario> Getusuarios()
        {
            return db.usuarios;
        }

        /// <summary>
        /// <autor>Alejandro Bermudez Vargas</autor>
        /// <date>4/12/2015</date>
        /// <usecase>Create evaluation</usecase>
        /// </summary>
        [Route("api/Users/getTeachers")]
        [HttpGet]
        public IQueryable<usuario> getTeachers()
        {
            return (db.usuarios.Where(u => u.id_rol == 5));
        }

        // GET api/Users/5
        [ResponseType(typeof(usuario))]
        public IHttpActionResult Getusuario(int id)
        {
            usuario usuario = db.usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            usuario.rol = db.rols.Find(usuario.id_rol);
            return Ok(usuario);
        }
        /// <summary>
        /// autor: Alejandro Bermudez Vargas
        /// fecha: 1/11/2015
        /// </summary>
        // GET api/Users/Login/{correo}/{password}
        [Route("api/Users/Login")]
        [HttpPost]
        [ResponseType(typeof(usuario))]
        public IHttpActionResult Login(usuario usuario)
        {
            string result = AES256.encryptPassword(usuario.password);
            usuario user = db.usuarios.FirstOrDefault(u => u.correo == usuario.correo && u.password == result);
            if (user == null) return NotFound();
            user.rol = db.rols.Find(user.id_rol);
            db.Entry(user.rol).Collection(p => p.permisos).Load();
            sesion sesion = new sesion();
            sesion.fecha = DateTime.Now;
            sesion.id_usuario = user.id_usuario;
            db.sesions.Add(sesion);
            Bitacora.getInstance().addBitacora(BitacoraActions.SIGN_IN, user.id_usuario);
            db.SaveChanges();
            return Ok(user);
        }
        /// <summary>
        /// autor: Alejandro Bermudez Vargas
        /// fecha: 1/11/2015
        /// </summary>
        [Route("api/Users/SignOut")]
        [HttpPost]
        [ResponseType(typeof(usuario))]
        public IHttpActionResult cerrarSesion(usuario usuario)
        {
            Bitacora.getInstance().addBitacora(BitacoraActions.SIGN_OUT, usuario.id_usuario);
            return Ok();
        }

        // PUT api/Users/5
        public IHttpActionResult Putusuario(int id, usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.id_usuario)
            {
                return BadRequest();
            }
            usuario.rol = db.rols.Find(usuario.id_rol);
            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usuarioExists(id))
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
        [Route("api/Users/change/{id}")]
        [HttpPut]
        public IHttpActionResult updateUserAndPassword(int id, usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.id_usuario)
            {
                return BadRequest();
            }
            usuario.password = AES256.encryptPassword(usuario.password);
            usuario.rol = db.rols.Find(usuario.id_rol);
            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usuarioExists(id))
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
        // POST api/Users
        [ResponseType(typeof(usuario))]
        public IHttpActionResult Postusuario(usuario usuario)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (usuario.productos==null)
            {

            }
            else
            {
                int count = usuario.productos.Count;

                for (int i = 0; i < usuario.productos.Count; i++)
                {
                    producto pproducto = usuario.productos.ElementAt(i);
                    usuario.productos.Remove(pproducto);
                    usuario.productos.Add(db.productos.Find(pproducto.id_producto));
                }
            }
                usuario.password = AES256.encryptPassword(usuario.password);
                db.usuarios.Add(usuario);
                db.SaveChanges();
                historial_contrasennas historial = new historial_contrasennas();
                historial.id_usuario = usuario.id_usuario;
                historial.contraseña = usuario.password;
                db.historial_contrasennas.Add(historial);
                db.SaveChanges();
            
            return CreatedAtRoute("DefaultApi", new { id = usuario.id_usuario }, usuario);
        }
        /// <summary>
        /// autor: Alejandro Bermudez Vargas
        /// fecha: 1/11/2015
        /// </summary>
        // DELETE api/Users/5
        [ResponseType(typeof(usuario))]
        public IHttpActionResult Deleteusuario(int id)
        {
            usuario usuario = db.usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            db.usuarios.Remove(usuario);
            db.SaveChanges();

            return Ok(usuario);
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

        private bool usuarioExists(int id)
        {
            return db.usuarios.Count(e => e.id_usuario == id) > 0;
        }

        // PUT api/Users/assign/5
        [Route("api/Users/assign/{id}")]
        [HttpPut]
        public IHttpActionResult Putprospusuario(int id, usuario pusuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pusuario.id_usuario)
            {
                return BadRequest();
            }
            usuario user = db.usuarios.Find(pusuario.id_usuario);
            db.Entry(user).Collection(p => p.prospectos).Load();

            if (pusuario.prospectos != null)
            {
                int cantProspectos = user.prospectos.Count;
                for (int x = 0; x < cantProspectos; x++) 
                {
                    user.prospectos.Remove(user.prospectos.ElementAt(0));
                }

                for (int i = 0; i < pusuario.prospectos.Count; i++)
                {
                    user.prospectos.Add(db.prospectos.Find(pusuario.prospectos.ElementAt(i).id_prospecto));
                }
            }
            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usuarioExists(id))
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

        // GET api/Users/getassign/5
        [Route("api/Users/getassign/{id}")]
        [HttpGet]
        public IHttpActionResult Getassign(int id)
        {
            usuario usuario = db.usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            usuario.rol = db.rols.Find(usuario.id_rol);
            db.Entry(usuario).Collection(p => p.prospectos).Load();
            return Ok(usuario);
        }
    }
}