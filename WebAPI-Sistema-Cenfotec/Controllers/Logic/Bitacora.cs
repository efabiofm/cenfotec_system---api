using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI_Sistema_Cenfotec.Models;
using WebAPI_Sistema_Cenfotec.Recursos;

namespace WebAPI_Sistema_Cenfotec.Controllers.Logic
{
    public class Bitacora
    {
        private DBContext db = new DBContext();
        private static Bitacora bitacora;

        private Bitacora(){}
        public static Bitacora getInstance(){
            if(bitacora==null) bitacora = new Bitacora();
            return bitacora;
        }
        public void addBitacora(string paccion, int puserId)
        {
            bitacora bitacora = new bitacora();
            bitacora.accion = paccion;
            bitacora.fecha = DateTime.Now;
            bitacora.id_usuario = puserId;
            db.bitacoras.Add(bitacora);
            db.SaveChanges();
        }
        public void addBitacora(string paccion, int puserId, int puserIdTwo) //Two users
        {
            bitacora bitacora = new bitacora();
            bitacora.accion = paccion;
            bitacora.fecha = DateTime.Now;
            bitacora.id_usuario = puserId;
            bitacora.id_usuario_afectado = puserIdTwo;
            db.bitacoras.Add(bitacora);
            db.SaveChanges();
        }
    }
}