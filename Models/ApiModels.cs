using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiHoteles.Models
{
    public class ApiModels
    {
    }

    public class RequestHotel {
        public int id_hotel { set; get; }
        public string nom_hotel { set; get; }
	    public int categoria  { set; get; }
        public float precio { set; get; }
        public IFormFile img1 { set; get; }
	    public IFormFile img2 { set; get; }
	    public IFormFile img3 { set; get; }
    }


    public class ResponseHotel
    {
        public int id_hotel { set; get; }
        public string nom_hotel { set; get; }
        public int categoria { set; get; }
        public float precio { set; get; }
        public int calificacion { set;get; }
        public string imagen_1 { set; get; }
        public string imagen_2 { set; get; }
        public string imagen_3 { set; get; }

    }

    public class FiltroCatCal { 
        public int categoriaHotel { set; get; }
        public int calificacionHotel { set; get; }
    }

    public class Comentario
    {
        public int id_comentario { set; get; }
        public int id_hotel { set; get; }
        public string nom_Cliente { set; get; }
        public int calificacion { set; get; }
        public string comentario { set; get; }

    }
}
