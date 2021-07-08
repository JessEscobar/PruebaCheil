using ApiHoteles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApiHoteles.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        string sqlstring;
        SqlCommand cmd;
        SqlDataReader dr;
        SqlConnection con;

        private bool Update(string comman)
        {
            bool isUpdate = false;
            sqlstring = comman;
            using (con = new SqlConnection(ConfigurationManager.AppSettings["StringConnection"]))
            {
                cmd = new SqlCommand(sqlstring, con);
                con.Open();
                try
                {
                    int a = (int)cmd.ExecuteNonQuery();
                    if (a >= 0)
                        isUpdate = true;
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                }
            }
            return isUpdate;
        }

        [HttpGet]
        public JsonResult ConsultarHoteles()
        {

            string ConectionStrigs = ConfigurationManager.AppSettings["StringConnection"];
            List<ResponseHotel> Hoteles = new List<ResponseHotel>();

            try
            {
                sqlstring = @"SELECT H.id_hotel, H.nom_hotel,H.categoria,H.precio,H.img1,H.img2,H.img3, AVG(C.calificacion) Calificación
                                FROM Hotel H
                                LEFT JOIN Comentario C ON c.id_hotel=H.id_hotel
                                GROUP BY H.id_hotel, H.nom_hotel,H.categoria,H.precio,H.img1,H.img2,H.img3";
                using (con = new SqlConnection(ConectionStrigs))
                {
                    con.Open();
                    cmd = new SqlCommand(sqlstring, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var calificacion = dr["Calificación"].ToString();

                        if (calificacion == "") {
                            calificacion = "0";
                        }


                        var lista = new ResponseHotel
                        {

                            id_hotel = Convert.ToInt32(dr["id_hotel"].ToString())
                            ,
                            nom_hotel = dr["nom_hotel"].ToString()
                            ,
                            categoria = Convert.ToInt32(dr["categoria"].ToString())
                            ,
                            precio = Convert.ToInt32(dr["precio"].ToString())
                            ,
                            calificacion = Convert.ToInt32(calificacion)
                            ,
                            imagen_1 = dr["img1"].ToString()
                            ,
                            imagen_2 = dr["img2"].ToString()
                            ,
                            imagen_3 = dr["img3"].ToString()

                        };

                        Hoteles.Add(lista);

                    }
                    dr.Close();
                }

                return Json(new { Estado = true, Data = Hoteles });
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Estado = false, Data = "No se pudo consultar los datos" });

            }

        }

        [HttpGet]
        public JsonResult FiltarHoteles([FromBody] FiltroCatCal data)
        {

            string ConectionStrigs = ConfigurationManager.AppSettings["StringConnection"];
            List<ResponseHotel> Hoteles = new List<ResponseHotel>();

            try
            {
                sqlstring = @"SELECT H.id_hotel, H.nom_hotel,H.categoria,H.precio,H.img1,H.img2,H.img3,AVG(C.calificacion) Calificación
                                FROM Hotel H
                                LEFT JOIN Comentario C ON c.id_hotel=H.id_hotel
                                WHERE H.categoria="+data.categoriaHotel+" AND Calificacion="+data.calificacionHotel+
                                " GROUP BY H.id_hotel, H.nom_hotel,H.categoria,H.precio,H.img1,H.img2,H.img3";
                using (con = new SqlConnection(ConectionStrigs))
                {
                    con.Open();
                    cmd = new SqlCommand(sqlstring, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var calificacion = dr["Calificación"].ToString();

                        if (calificacion == "")
                        {
                            calificacion = "0";
                        }


                        var lista = new ResponseHotel
                        {

                            id_hotel = Convert.ToInt32(dr["id_hotel"].ToString())
                            ,
                            nom_hotel = dr["nom_hotel"].ToString()
                            ,
                            categoria = Convert.ToInt32(dr["categoria"].ToString())
                            ,
                            precio = Convert.ToInt32(dr["precio"].ToString())
                            ,
                            calificacion = Convert.ToInt32(calificacion)
                            ,
                            imagen_1 = dr["img1"].ToString()
                            ,
                            imagen_2 = dr["img2"].ToString()
                            ,
                            imagen_3 = dr["img3"].ToString()

                        };

                        Hoteles.Add(lista);

                    }
                    dr.Close();
                }

                return Json(new { Estado = true, Data = Hoteles });
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Estado = false, Data = "No se pudo consultar los datos" });

            }

        }

        [HttpGet]
        public JsonResult FiltarOrdenHoteles(int ordenar)
        {
 
            string ConectionStrigs = ConfigurationManager.AppSettings["StringConnection"];
            List<ResponseHotel> Hoteles = new List<ResponseHotel>();

            try
            {
                string orden="";
                if (ordenar == 1)
                {
                    orden = " ORDER BY H.precio ASC";

                }
                else if (ordenar == 0)
                {
                    orden = " ORDER BY H.precio DESC";

                }

                sqlstring = @"SELECT H.id_hotel, H.nom_hotel,H.categoria,H.precio,H.img1,H.img2,H.img3,AVG(C.calificacion) Calificación
                                FROM Hotel H
                                LEFT JOIN Comentario C ON c.id_hotel=H.id_hotel          
                                GROUP BY H.id_hotel, H.nom_hotel,H.categoria,H.precio,H.img1,H.img2,H.img3"+ orden;
                using (con = new SqlConnection(ConectionStrigs))
                {
                    con.Open();
                    cmd = new SqlCommand(sqlstring, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var calificacion = dr["Calificación"].ToString();

                        if (calificacion == "")
                        {
                            calificacion = "0";
                        }


                        var lista = new ResponseHotel
                        {

                            id_hotel = Convert.ToInt32(dr["id_hotel"].ToString())
                            ,
                            nom_hotel = dr["nom_hotel"].ToString()
                            ,
                            categoria = Convert.ToInt32(dr["categoria"].ToString())
                            ,
                            precio = Convert.ToInt32(dr["precio"].ToString())
                            ,
                            calificacion = Convert.ToInt32(calificacion)
                            ,
                            imagen_1 = dr["img1"].ToString()
                            ,
                            imagen_2 = dr["img2"].ToString()
                            ,
                            imagen_3 = dr["img3"].ToString()

                        };

                        Hoteles.Add(lista);

                    }
                    dr.Close();
                }

                return Json(new { Estado = true, Data = Hoteles });
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Estado = false, Data = "No se pudo consultar los datos" });

            }

        }

        #region CRUD HOTELES

        [HttpPost]
        public async Task<JsonResult> CrearHoteles([FromForm] RequestHotel data)
        {
            bool estado;
            string mensaje;
            string ConectionStrigs = ConfigurationManager.AppSettings["StringConnection"];
            List<ResponseHotel> Hoteles = new List<ResponseHotel>();

            try
            {
                sqlstring = @"INSERT INTO Hotel (nom_hotel,categoria,precio,img1,img2,img3) VALUES(" + "'" + data.nom_hotel + "'," + data.categoria + "," + data.precio + ",'" + data.img1.FileName + "'" + ",'" + data.img2.FileName + "','" + data.img3.FileName + "')";

                var requeststatus = Request;
                string path = @"wwwroot/Content/img/Hoteles/";
                var filenameImg1 = requeststatus.Form.Files[0].FileName;
                var filenameImg2 = requeststatus.Form.Files[1].FileName;
                var filenameImg3 = requeststatus.Form.Files[2].FileName;
                var fileContentImg1 = Request.Form.Files[0];
                var fileContentImg2 = Request.Form.Files[1];
                var fileContentImg3 = Request.Form.Files[2];

                string SavePathImg1 = path + filenameImg1;
                var filePathImg1 = Path.Combine(path, requeststatus.Form.Files[0].FileName);

                string SavePathImg2 = path + filenameImg2;
                var filePathImg2 = Path.Combine(path, requeststatus.Form.Files[1].FileName);

                string SavePathImg3 = path + filenameImg3;
                var filePathImg3 = Path.Combine(path, requeststatus.Form.Files[2].FileName);


                if (filenameImg1.Length < 2097152 || filenameImg2.Length < 2097152|| filenameImg3.Length < 2097152)
                {
                    if (Update(sqlstring))
                    {
                        mensaje = "Se ha creado el Hotel " + data.nom_hotel;
                        estado = true;
                        using (var stream = System.IO.File.Create(filePathImg1))
                        {
                            await fileContentImg1.CopyToAsync(stream);
                        }
                        using (var stream = System.IO.File.Create(filePathImg2))
                        {
                            await fileContentImg2.CopyToAsync(stream);
                        }
                        using (var stream = System.IO.File.Create(filePathImg3))
                        {
                            await fileContentImg3.CopyToAsync(stream);
                        }
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(new { Estado = estado, Mensaje = mensaje });
                    }

                    else
                    {
                        mensaje = "No Se pudo crear el hotel " + data.nom_hotel;
                        estado = false;
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(new { Estado = estado, Mensaje = mensaje });
                    }
                }
                else {
                    mensaje = "No Se pudo crear el Hotel " + data.nom_hotel;
                    estado = false;
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { Estado = estado, Mensaje = mensaje });
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Estado = false, Data = "No se pudo crear el hotel" });

            }

        }

        [HttpGet]
        public JsonResult BuscartarHoteles(string nombreHotel)
        {

            string ConectionStrigs = ConfigurationManager.AppSettings["StringConnection"];
            List<ResponseHotel> Hoteles = new List<ResponseHotel>();

            try
            {
                sqlstring = @"SELECT H.id_hotel, H.nom_hotel,H.categoria,H.precio,H.img1,H.img2,H.img3, AVG(C.calificacion) Calificación
                                FROM Hotel H
                                LEFT JOIN Comentario C ON c.id_hotel=H.id_hotel " +
                                "WHERE H.nom_hotel like('%" + nombreHotel +"%') "+
                                "GROUP BY H.id_hotel, H.nom_hotel,H.categoria,H.precio,H.img1,H.img2,H.img3";
                using (con = new SqlConnection(ConectionStrigs))
                {
                    con.Open();
                    cmd = new SqlCommand(sqlstring, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var calificacion = dr["Calificación"].ToString();

                        if (calificacion == "")
                        {
                            calificacion = "0";
                        }


                        var lista = new ResponseHotel
                        {

                            id_hotel = Convert.ToInt32(dr["id_hotel"].ToString())
                            ,
                            nom_hotel = dr["nom_hotel"].ToString()
                            ,
                            categoria = Convert.ToInt32(dr["categoria"].ToString())
                            ,
                            precio = Convert.ToInt32(dr["precio"].ToString())
                            ,
                            calificacion = Convert.ToInt32(calificacion)
                            ,
                            imagen_1 = dr["img1"].ToString()
                            ,
                            imagen_2 = dr["img2"].ToString()
                            ,
                            imagen_3 = dr["img3"].ToString()

                        };

                        Hoteles.Add(lista);

                    }
                    dr.Close();
                }

                return Json(new { Estado = true, Data = Hoteles });
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Estado = false, Data = "No se pudo consultar los datos" });

            }

        }

        [HttpPost]
        public async Task<JsonResult> UpdateHotel([FromForm] RequestHotel data)
        {
            bool estado;
            string mensaje;
            try
            {
                if (data.img1 != null && data.img2 != null && data.img3 != null)
                {

                    sqlstring = @"UPDATE Hotel 
                                SET nom_hotel ='" + data.nom_hotel + "'," +
                                        "categoria =" + data.categoria + "," +
                                        "precio =" + data.precio + "," +
                                        "img1='" + data.img1.FileName + "'," +
                                        "img2='" + data.img2.FileName + "'," +
                                        "img3='" + data.img3.FileName + "' " +
                                    "WHERE id_hotel =" + data.id_hotel;

                    var requeststatus = Request;
                    string path = @"wwwroot/Content/img/Hoteles/";
                    var filenameImg1 = requeststatus.Form.Files[0].FileName;
                    var filenameImg2 = requeststatus.Form.Files[1].FileName;
                    var filenameImg3 = requeststatus.Form.Files[2].FileName;
                    var fileContentImg1 = Request.Form.Files[0];
                    var fileContentImg2 = Request.Form.Files[1];
                    var fileContentImg3 = Request.Form.Files[2];

                    string SavePathImg1 = path + filenameImg1;
                    var filePathImg1 = Path.Combine(path, requeststatus.Form.Files[0].FileName);

                    string SavePathImg2 = path + filenameImg2;
                    var filePathImg2 = Path.Combine(path, requeststatus.Form.Files[1].FileName);

                    string SavePathImg3 = path + filenameImg3;
                    var filePathImg3 = Path.Combine(path, requeststatus.Form.Files[2].FileName);


                    if (filenameImg1.Length < 2097152 || filenameImg2.Length < 2097152 || filenameImg3.Length < 2097152)
                    {
                        if (Update(sqlstring))
                        {
                            mensaje = "Se ha actualizado el Hotel " + data.nom_hotel;
                            estado = true;
                            using (var stream = System.IO.File.Create(filePathImg1))
                            {
                                await fileContentImg1.CopyToAsync(stream);
                            }
                            using (var stream = System.IO.File.Create(filePathImg2))
                            {
                                await fileContentImg2.CopyToAsync(stream);
                            }
                            using (var stream = System.IO.File.Create(filePathImg3))
                            {
                                await fileContentImg3.CopyToAsync(stream);
                            }
                            Response.StatusCode = (int)HttpStatusCode.Created;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }

                        else
                        {
                            mensaje = "No Se pudo actualizar el hotel " + data.nom_hotel;
                            estado = false;
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }
                    }
                    else
                    {
                        mensaje = "No Se pudo actualizar el Hotel el tamaño de la imagen supera los 2MB" + data.nom_hotel;
                        estado = false;
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(new { Estado = estado, Mensaje = mensaje });
                    }
                }

                else if (data.img1 != null && data.img2 == null && data.img3 == null)
                {

                    sqlstring = @"UPDATE Hotel 
                                SET nom_hotel ='" + data.nom_hotel + "'," +
                                        "categoria =" + data.categoria + "," +
                                        "precio =" + data.precio + "," +
                                        "img1='" + data.img1.FileName + "' " +
                                    "WHERE id_hotel =" + data.id_hotel;

                    var requeststatus = Request;
                    string path = @"wwwroot/Content/img/Hoteles/";
                    var filenameImg = requeststatus.Form.Files[0].FileName;

                    var fileContentImg = Request.Form.Files[0];


                    string SavePathImg = path + filenameImg;
                    var filePathImg = Path.Combine(path, requeststatus.Form.Files[0].FileName);


                    if (filenameImg.Length < 2097152)
                    {
                        if (Update(sqlstring))
                        {
                            mensaje = "Se ha actualizado el Hotel " + data.nom_hotel;
                            estado = true;
                            using (var stream = System.IO.File.Create(filePathImg))
                            {
                                await fileContentImg.CopyToAsync(stream);
                            }
                            Response.StatusCode = (int)HttpStatusCode.Created;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }

                        else
                        {
                            mensaje = "No Se pudo actualizar el hotel " + data.nom_hotel;
                            estado = false;
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }
                    }
                    else
                    {
                        mensaje = "No Se pudo actualizar el Hotel el tamaño de la imagen supera los 2MB" + data.nom_hotel;
                        estado = false;
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(new { Estado = estado, Mensaje = mensaje });
                    }
                }

                else if (data.img1 == null && data.img2 != null && data.img3 == null)
                {

                    sqlstring = @"UPDATE Hotel 
                                SET nom_hotel ='" + data.nom_hotel + "'," +
                                        "categoria =" + data.categoria + "," +
                                        "precio =" + data.precio + "," +
                                        "img2='" + data.img2.FileName + "' " +
                                    "WHERE id_hotel =" + data.id_hotel;

                    var requeststatus = Request;
                    string path = @"wwwroot/Content/img/Hoteles/";
                    var filenameImg = requeststatus.Form.Files[1].FileName;

                    var fileContentImg = Request.Form.Files[1];


                    string SavePathImg = path + filenameImg;
                    var filePathImg = Path.Combine(path, requeststatus.Form.Files[1].FileName);


                    if (filenameImg.Length < 2097152)
                    {
                        if (Update(sqlstring))
                        {
                            mensaje = "Se ha actualizado el Hotel " + data.nom_hotel;
                            estado = true;
                            using (var stream = System.IO.File.Create(filePathImg))
                            {
                                await fileContentImg.CopyToAsync(stream);
                            }
                            Response.StatusCode = (int)HttpStatusCode.Created;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }

                        else
                        {
                            mensaje = "No Se pudo actualizar el hotel " + data.nom_hotel;
                            estado = false;
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }
                    }
                    else
                    {
                        mensaje = "No Se pudo actualizar el Hotel el tamaño de la imagen supera los 2MB" + data.nom_hotel;
                        estado = false;
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(new { Estado = estado, Mensaje = mensaje });
                    }
                }

                else if (data.img1 == null && data.img2 == null && data.img3 != null)
                {

                    sqlstring = @"UPDATE Hotel 
                                SET nom_hotel ='" + data.nom_hotel + "'," +
                                        "categoria =" + data.categoria + "," +
                                        "precio =" + data.precio + "," +
                                        "img3='" + data.img3.FileName + "' " +
                                    "WHERE id_hotel =" + data.id_hotel;

                    var requeststatus = Request;
                    string path = @"wwwroot/Content/img/Hoteles/";
                    var filenameImg = requeststatus.Form.Files[2].FileName;

                    var fileContentImg = Request.Form.Files[2];


                    string SavePathImg = path + filenameImg;
                    var filePathImg = Path.Combine(path, requeststatus.Form.Files[2].FileName);


                    if (filenameImg.Length < 2097152)
                    {
                        if (Update(sqlstring))
                        {
                            mensaje = "Se ha actualizado el Hotel " + data.nom_hotel;
                            estado = true;
                            using (var stream = System.IO.File.Create(filePathImg))
                            {
                                await fileContentImg.CopyToAsync(stream);
                            }
                            Response.StatusCode = (int)HttpStatusCode.Created;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }

                        else
                        {
                            mensaje = "No Se pudo actualizar el hotel " + data.nom_hotel;
                            estado = false;
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }
                    }
                    else
                    {
                        mensaje = "No Se pudo actualizar el Hotel el tamaño de la imagen supera los 2MB" + data.nom_hotel;
                        estado = false;
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(new { Estado = estado, Mensaje = mensaje });
                    }
                }

                else if (data.img1 == null && data.img2 == null && data.img3 == null)
                {

                    sqlstring = @"UPDATE Hotel 
                                SET nom_hotel ='" + data.nom_hotel + "'," +
                                        "categoria =" + data.categoria + "," +
                                        "precio =" + data.precio +                                        
                                    " WHERE id_hotel =" + data.id_hotel;

 
                        if (Update(sqlstring))
                        {
                            mensaje = "Se ha actualizado el Hotel " + data.nom_hotel;
                            estado = true;
                            Response.StatusCode = (int)HttpStatusCode.Created;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }

                        else
                        {
                            mensaje = "No Se pudo actualizar el hotel " + data.nom_hotel;
                            estado = false;
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }
                    
                }

                else if (data.img1 != null && data.img2 != null && data.img3 == null)
                {

                    sqlstring = @"UPDATE Hotel 
                                SET nom_hotel ='" + data.nom_hotel + "'," +
                                        "categoria =" + data.categoria + "," +
                                        "precio =" + data.precio + "," +
                                        "img1='" + data.img1.FileName + "'," +
                                        "img2='" + data.img2.FileName + "' " +            
                                    "WHERE id_hotel =" + data.id_hotel;

                    var requeststatus = Request;
                    string path = @"wwwroot/Content/img/Hoteles/";
                    var filenameImg1 = requeststatus.Form.Files[0].FileName;
                    var filenameImg2 = requeststatus.Form.Files[1].FileName;

                    var fileContentImg1 = Request.Form.Files[0];
                    var fileContentImg2 = Request.Form.Files[1];


                    string SavePathImg1 = path + filenameImg1;
                    var filePathImg1 = Path.Combine(path, requeststatus.Form.Files[0].FileName);

                    string SavePathImg2 = path + filenameImg2;
                    var filePathImg2 = Path.Combine(path, requeststatus.Form.Files[1].FileName);




                    if (filenameImg1.Length < 2097152 || filenameImg2.Length < 2097152)
                    {
                        if (Update(sqlstring))
                        {
                            mensaje = "Se ha actualizado el Hotel " + data.nom_hotel;
                            estado = true;
                            using (var stream = System.IO.File.Create(filePathImg1))
                            {
                                await fileContentImg1.CopyToAsync(stream);
                            }
                            using (var stream = System.IO.File.Create(filePathImg2))
                            {
                                await fileContentImg2.CopyToAsync(stream);
                            }

                            Response.StatusCode = (int)HttpStatusCode.Created;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }

                        else
                        {
                            mensaje = "No Se pudo actualizar el hotel " + data.nom_hotel;
                            estado = false;
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }
                    }
                    else
                    {
                        mensaje = "No Se pudo actualizar el Hotel el tamaño de la imagen supera los 2MB" + data.nom_hotel;
                        estado = false;
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(new { Estado = estado, Mensaje = mensaje });
                    }
                }
                else if (data.img1 != null && data.img2 == null && data.img3 != null)
                {

                    sqlstring = @"UPDATE Hotel 
                                SET nom_hotel ='" + data.nom_hotel + "'," +
                                        "categoria =" + data.categoria + "," +
                                        "precio =" + data.precio + "," +
                                        "img1='" + data.img1.FileName + "'," +
                                        "img3='" + data.img3.FileName + "' " +
                                    "WHERE id_hotel =" + data.id_hotel;

                    var requeststatus = Request;
                    string path = @"wwwroot/Content/img/Hoteles/";
                    var filenameImg1 = requeststatus.Form.Files[0].FileName;
                    var filenameImg2 = requeststatus.Form.Files[1].FileName;

                    var fileContentImg1 = Request.Form.Files[0];
                    var fileContentImg2 = Request.Form.Files[1];


                    string SavePathImg1 = path + filenameImg1;
                    var filePathImg1 = Path.Combine(path, requeststatus.Form.Files[0].FileName);

                    string SavePathImg2 = path + filenameImg2;
                    var filePathImg2 = Path.Combine(path, requeststatus.Form.Files[1].FileName);




                    if (filenameImg1.Length < 2097152 || filenameImg2.Length < 2097152)
                    {
                        if (Update(sqlstring))
                        {
                            mensaje = "Se ha actualizado el Hotel " + data.nom_hotel;
                            estado = true;
                            using (var stream = System.IO.File.Create(filePathImg1))
                            {
                                await fileContentImg1.CopyToAsync(stream);
                            }
                            using (var stream = System.IO.File.Create(filePathImg2))
                            {
                                await fileContentImg2.CopyToAsync(stream);
                            }

                            Response.StatusCode = (int)HttpStatusCode.Created;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }

                        else
                        {
                            mensaje = "No Se pudo actualizar el hotel " + data.nom_hotel;
                            estado = false;
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }
                    }
                    else
                    {
                        mensaje = "No Se pudo actualizar el Hotel el tamaño de la imagen supera los 2MB" + data.nom_hotel;
                        estado = false;
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(new { Estado = estado, Mensaje = mensaje });
                    }
                }
                else if (data.img1 == null && data.img2 != null && data.img3 != null)
                {

                    sqlstring = @"UPDATE Hotel 
                                SET nom_hotel ='" + data.nom_hotel + "'," +
                                        "categoria =" + data.categoria + "," +
                                        "precio =" + data.precio + "," +
                                        "img2='" + data.img2.FileName + "'," +
                                        "img3='" + data.img3.FileName + "' " +
                                    "WHERE id_hotel =" + data.id_hotel;

                    var requeststatus = Request;
                    string path = @"wwwroot/Content/img/Hoteles/";
                    var filenameImg1 = requeststatus.Form.Files[0].FileName;
                    var filenameImg2 = requeststatus.Form.Files[1].FileName;

                    var fileContentImg1 = Request.Form.Files[0];
                    var fileContentImg2 = Request.Form.Files[1];


                    string SavePathImg1 = path + filenameImg1;
                    var filePathImg1 = Path.Combine(path, requeststatus.Form.Files[0].FileName);

                    string SavePathImg2 = path + filenameImg2;
                    var filePathImg2 = Path.Combine(path, requeststatus.Form.Files[1].FileName);




                    if (filenameImg1.Length < 2097152 || filenameImg2.Length < 2097152)
                    {
                        if (Update(sqlstring))
                        {
                            mensaje = "Se ha actualizado el Hotel " + data.nom_hotel;
                            estado = true;
                            using (var stream = System.IO.File.Create(filePathImg1))
                            {
                                await fileContentImg1.CopyToAsync(stream);
                            }
                            using (var stream = System.IO.File.Create(filePathImg2))
                            {
                                await fileContentImg2.CopyToAsync(stream);
                            }

                            Response.StatusCode = (int)HttpStatusCode.Created;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }

                        else
                        {
                            mensaje = "No Se pudo actualizar el hotel " + data.nom_hotel;
                            estado = false;
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return Json(new { Estado = estado, Mensaje = mensaje });
                        }
                    }
                    else
                    {
                        mensaje = "No Se pudo actualizar el Hotel el tamaño de la imagen supera los 2MB" + data.nom_hotel;
                        estado = false;
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(new { Estado = estado, Mensaje = mensaje });
                    }
                }

                else {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { Estado = false, Mensaje = "No se envio nada para actualizar" });
                }
                
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Estado = false, Data = "No se pudo crear el hotel" });

            }

        }

        [HttpDelete]
        public JsonResult DeleterHoteles(int id)
        {
            try
            {
                sqlstring = @"DELETE Hotel WHERE id_hotel="+id;
                bool estado;
                string mensaje;
                if (Update(sqlstring))
                {
                    sqlstring = @"DELETE Comentario WHERE id_hotel=" + id;
                    if (Update(sqlstring))
                    {
                        mensaje = "Se ha borrado el hotel";
                        estado = true;

                    }
                    else {

                        mensaje = "No Se pudo borrar el comentario ";
                        estado = false;
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(new { Estado = estado, Mensaje = mensaje });

                    }
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(new { Estado = estado, Mensaje = mensaje });
                }

                else
                {
                    mensaje = "No Se pudo borrar el hotel " ;
                    estado = false;
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { Estado = estado, Mensaje = mensaje });
                }

    
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Estado = false, Data = "No se pudo consultar los datos" });

            }

        }
        #endregion


    }
}
