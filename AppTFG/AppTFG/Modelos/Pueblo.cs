﻿using SQLite;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppTFG.Modelos
{
    public class Pueblo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ImagenPrincipal { get; set; }
        public double CoordenadaX { get; set; }
        public double CoordenadaY { get; set; }
        public List<Foto> Fotos { get; set; }
        public List<Video> Videos { get; set; }
        public List<Ruta> Rutas { get; set; }
        public List<Actividad> Actividades { get; set; }
        [ForeignKey("FK_NombreUsuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}

