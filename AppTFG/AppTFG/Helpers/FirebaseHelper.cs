﻿using AppTFG.Modelos;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppTFG.Helpers
{
    public class FirebaseHelper
    {

        //Conexión Base de datos en tiempo real
        //public static FirebaseClient firebase = new FirebaseClient("https://apptfg-2e2e6-default-rtdb.europe-west1.firebasedatabase.app/");

        public static FirebaseClient firebase = new FirebaseClient("https://apptfg-2e2e6-default-rtdb.europe-west1.firebasedatabase.app/",
                new FirebaseOptions { OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s) });

        //Conexión Almacenaimento contenido multimedia
        public static FirebaseStorage firebaseStorage = new FirebaseStorage("apptfg-2e2e6.appspot.com");


        //MÉTODOS CRUD USUARIO
        public static async Task<List<Usuario>> ObtenerTodosUsuarios()
        {
            try
            {
                var listaUsuarios = (await firebase
                .Child("Usuarios")
                .OnceAsync<Usuario>()).Select(item =>
                new Usuario
                {
                    Nombre = item.Object.Nombre,
                    Password = item.Object.Password,
                    UsuarioId = item.Object.UsuarioId
                }).ToList();
                return listaUsuarios;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Leer 
        public static async Task<Usuario> ObtenerUsuario(string nombre)
        {
            try
            {
                var todosUsuarios = await ObtenerTodosUsuarios();
                await firebase
                .Child("Usuarios")
                .OnceAsync<Usuario>();
                return todosUsuarios.Where(a => a.Nombre == nombre).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Insertar
        public static async Task<bool> InsertarUsuario(string nombre, string password, int id)
        {
            try
            {
                await firebase
                .Child("Usuarios")
                .PostAsync(new Usuario() { Nombre = nombre, Password = password, UsuarioId = id});
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Actualizar
        public static async Task<bool> ActualizarUsuario(string nombre, string password, int id)
        {
            try
            {
                var actualizarUsuario = (await firebase
                .Child("Usuarios")
                .OnceAsync<Usuario>()).Where(a => a.Object.Nombre == nombre).FirstOrDefault();
                await firebase
                .Child("Usuarios")
                .Child(actualizarUsuario.Key)
                .PutAsync(new Usuario() { Nombre = nombre, Password = password, UsuarioId = id });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Eliminar
        public static async Task<bool> EliminarUsuario(string nombre)
        {
            try
            {
                var eliminarUsuario = (await firebase
                .Child("Usuarios")
                .OnceAsync<Usuario>()).Where(a => a.Object.Nombre == nombre).FirstOrDefault();
                await firebase.Child("Usuarios").Child(eliminarUsuario.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //MÉTODOS CRUD PUEBLO

        public static async Task<List<Pueblo>> ObtenerTodosPueblos()
        {
            try
            {
                var listaPueblos = (await firebase
                .Child("Pueblos")
                .OnceAsync<Pueblo>()).Select(item =>
                new Pueblo
                {
                    Id = item.Object.Id,
                    Nombre = item.Object.Nombre,
                    Descripcion = item.Object.Descripcion,
                    ImagenPrincipal = item.Object.ImagenPrincipal,
                    Fotos = item.Object.Fotos,
                    Videos = item.Object.Videos,
                    Rutas = item.Object.Rutas,
                    Actividades = item.Object.Actividades
                }).ToList();
                return listaPueblos;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //public static async Task<Pueblo> ObtenerTodosPueblosUsuario(string nombreUsuario)
        //{
        //    try
        //    {
        //        var todosPueblos = await ObtenerTodosPueblos();
        //        await firebase.Child("Pueblos").OnceAsync<Pueblo>();
        //        return (Pueblo)todosPueblos.Where(a => a.Usuario.Nombre.Equals(nombreUsuario));
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine($"Error:{e}");
        //        return null;
        //    }
        //}

        //Leer 
        public static async Task<Pueblo> ObtenerPueblo(int id)
        {
            try
            {
                var todosPueblos = await ObtenerTodosPueblos();
                await firebase
                .Child("Pueblos")
                .OnceAsync<Pueblo>();
                return todosPueblos.Where(a => a.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Insertar
        public static async Task<bool> InsertarPueblo(int id, string nombre, string descrpcion, string imagen) /*, int idUsuario*/
        {
            try
            {
                await firebase
                .Child("Pueblos")
                .PostAsync(new Pueblo() { Id = id, Nombre = nombre, Descripcion = descrpcion, ImagenPrincipal = imagen }); /*, IdUsuario = idUsuario*/
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Actualizar 
        public static async Task<bool> ActualizarPueblo(int id, string nombre, string descrpcion, string imagen)
        {
            try
            {
                var actualizarPueblo = (await firebase
                .Child("Pueblos")
                .OnceAsync<Pueblo>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase
                .Child("Pueblos")
                .Child(actualizarPueblo.Key)
                .PutAsync(new Pueblo() { Id = id, Nombre = nombre, Descripcion = descrpcion, ImagenPrincipal = imagen });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Eliminar
        public static async Task<bool> EliminarPueblo(int id)
        {
            try
            {
                var eliminarPueblo = (await firebase
                .Child("Pueblos")
                .OnceAsync<Pueblo>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase.Child("Pueblos").Child(eliminarPueblo.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //MÉTODOS CRUD RUTA

        public static async Task<List<Ruta>> ObtenerTodasRutas()
        {
            try
            {
                var listaRutas = (await firebase
                .Child("Rutas")
                .OnceAsync<Ruta>()).Select(item =>
                new Ruta
                {
                    Id = item.Object.Id,
                    Nombre = item.Object.Nombre,
                    Descripcion = item.Object.Descripcion,
                    ImagenPrincipal = item.Object.ImagenPrincipal,
                    VideoUrl = item.Object.VideoUrl,
                    IdPueblo = item.Object.IdPueblo,
                    Camino = item.Object.Camino,
                    Ubicaciones = item.Object.Ubicaciones
                }).ToList();
                return listaRutas;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        public static async Task<List<Ruta>> ObtenerTodasRutasPueblo(int idPueblo)
        {
            try
            {
                var todasRutas = await ObtenerTodasRutas();
                await firebase.Child("Rutas").OnceAsync<Ruta>();
                return todasRutas.Where(a => a.IdPueblo.Equals(idPueblo)).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Read 
        public static async Task<Ruta> ObtenerRuta(int id)
        {
            try
            {
                var todasRutas = await ObtenerTodasRutas();
                await firebase
                .Child("Rutas")
                .OnceAsync<Ruta>();
                return todasRutas.Where(a => a.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Insertar ruta
        public static async Task<bool> InsertarRuta(int id, string nombre, string descrpcion, string imagen, Video video, int idPueblo, List<Posicion> camino, List<Ubicacion> ubicaciones)
        {
            try
            {
                await firebase
                .Child("Rutas")
                .PostAsync(new Ruta() { Id = id, Nombre = nombre, Descripcion = descrpcion, ImagenPrincipal = imagen, VideoUrl = video, IdPueblo = idPueblo, Camino = camino, Ubicaciones = ubicaciones });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Actualizar ruta
        public static async Task<bool> ActualizarRuta(int id, string nombre, string descrpcion, string imagen, Video video, int idPueblo, List<Posicion> camino, List<Ubicacion> ubicaciones)
        {
            try
            {
                var actualizarRuta = (await firebase
                .Child("Rutas")
                .OnceAsync<Ruta>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase
                .Child("Rutas")
                .Child(actualizarRuta.Key)
                .PutAsync(new Ruta() { Id = id, Nombre = nombre, Descripcion = descrpcion, ImagenPrincipal = imagen, VideoUrl = video, IdPueblo = idPueblo, Camino = camino, Ubicaciones = ubicaciones });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Borrar ruta
        public static async Task<bool> EliminarRuta(int id)
        {
            try
            {
                var eliminarRuta = (await firebase
                .Child("Rutas")
                .OnceAsync<Ruta>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase.Child("Rutas").Child(eliminarRuta.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //MÉTODOS CRUD UBICACION

        public static async Task<List<Ubicacion>> ObtenerTodasUbicaciones()
        {
            try
            {
                var listaUbicaciones = (await firebase
                .Child("Ubicaciones")
                .OnceAsync<Ubicacion>()).Select(item =>
                new Ubicacion
                {
                    Id = item.Object.Id,
                    Nombre = item.Object.Nombre,
                    Latitud = item.Object.Latitud,
                    Longitud = item.Object.Longitud
                }).ToList();
                return listaUbicaciones;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        public static async Task<List<Ubicacion>> ObtenerTodasUbicacionesRuta(int idRuta)
        {
            try
            {
                var todasUbicaciones = await ObtenerTodasUbicaciones();
                await firebase.Child("Ubicaciones").OnceAsync<Ubicacion>();
                return todasUbicaciones.Where(a => a.Id.Equals(idRuta)).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Read ubicacion
        public static async Task<Ubicacion> ObtenerUbicacion(int id)
        {
            try
            {
                var todasUbicaciones = await ObtenerTodasUbicaciones();
                await firebase
                .Child("Ubicaciones")
                .OnceAsync<Ubicacion>();
                return todasUbicaciones.Where(a => a.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Insertar ubicacion
        public static async Task<bool> InsertarUbicacion(int id, string nombre, double latitud, double longitud)
        {
            try
            {
                await firebase
                .Child("Ubicaciones")
                .PostAsync(new Ubicacion() { Id = id, Nombre = nombre, Latitud = latitud, Longitud = longitud });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Actualizar ubicacion
        public static async Task<bool> ActualizarUbicacion(int id, string nombre, double latitud, double longitud)
        {
            try
            {
                var actualizarUbicacion = (await firebase
                .Child("Ubicaciones")
                .OnceAsync<Ubicacion>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase
                .Child("Ubicaciones")
                .Child(actualizarUbicacion.Key)
                .PutAsync(new Ubicacion() { Id = id, Nombre = nombre, Latitud = latitud, Longitud = longitud });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Borrar ubicacion
        public static async Task<bool> EliminarUbicacion(int id)
        {
            try
            {
                var eliminarUbicacion = (await firebase
                .Child("Ubicaciones")
                .OnceAsync<Ubicacion>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase.Child("Ubicaciones").Child(eliminarUbicacion.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //MÉTODOS CRUD CAMINO

        public static async Task<List<Posicion>> ObtenerTodasPosiciones()
        {
            try
            {
                var listaPosiciones = (await firebase
                .Child("Posiciones")
                .OnceAsync<Posicion>()).Select(item =>
                new Posicion
                {
                    Id = item.Object.Id,
                    X = item.Object.X,
                    Y = item.Object.Y
                }).ToList();
                return listaPosiciones;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        public static async Task<List<Posicion>> ObtenerTodasPosicionesRuta(int idRuta)
        {
            try
            {
                var todasPosiciones = await ObtenerTodasPosiciones();
                await firebase.Child("Posiciones").OnceAsync<Posicion>();
                return todasPosiciones.Where(a => a.Id.Equals(idRuta)).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Read ubicacion
        public static async Task<Posicion> ObtenerPosicion(int id)
        {
            try
            {
                var todasPosiciones = await ObtenerTodasPosiciones();
                await firebase
                .Child("Posiciones")
                .OnceAsync<Posicion>();
                return todasPosiciones.Where(a => a.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Insertar ubicacion
        public static async Task<bool> InsertarPosicion(int id, double x, double y)
        {
            try
            {
                await firebase
                .Child("Posiciones")
                .PostAsync(new Posicion() { Id = id, X = x, Y = y });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Actualizar ubicacion
        public static async Task<bool> ActualizarPosicion(int id, double x, double y)
        {
            try
            {
                var actualizarPosicion = (await firebase
                .Child("Posiciones")
                .OnceAsync<Posicion>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase
                .Child("Posiciones")
                .Child(actualizarPosicion.Key)
                .PutAsync(new Posicion() { Id = id, X = x, Y = y });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Borrar ubicacion
        public static async Task<bool> EliminarPosicion(int id)
        {
            try
            {
                var eliminarPosicion = (await firebase
                .Child("Posiciones")
                .OnceAsync<Posicion>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase.Child("Posiciones").Child(eliminarPosicion.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //MÉTODOS CRUD ACTIVIDAD

        public static async Task<List<Actividad>> ObtenerTodasActividades()
        {
            try
            {
                var listaActividades = (await firebase
                .Child("Actividades")
                .OnceAsync<Actividad>()).Select(item =>
                new Actividad
                {
                    Id = item.Object.Id,
                    Nombre = item.Object.Nombre,
                    Descripcion = item.Object.Descripcion,
                    ImagenPrincipal = item.Object.ImagenPrincipal,
                    VideoUrl = item.Object.VideoUrl,
                    IdPueblo = item.Object.IdPueblo
                }).ToList();
                return listaActividades;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        public static async Task<List<Actividad>> ObtenerTodasActividadesPueblo(int idPueblo)
        {
            try
            {
                var todasActividades = await ObtenerTodasActividades();
                await firebase.Child("Actividades").OnceAsync<Actividad>();
                return todasActividades.Where(a => a.IdPueblo.Equals(idPueblo)).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Leer 
        public static async Task<Actividad> ObtenerActividad(int id)
        {
            try
            {
                var todasActividades = await ObtenerTodasActividades();
                await firebase
                .Child("Actividades")
                .OnceAsync<Actividad>();
                return todasActividades.Where(a => a.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Insertar
        public static async Task<bool> InsertarActividad(int id, string nombre, string descrpcion, string imagen, Video video, int idPueblo)
        {
            try
            {
                await firebase
                .Child("Actividades")
                .PostAsync(new Actividad() { Id = id, Nombre = nombre, Descripcion = descrpcion, ImagenPrincipal = imagen, VideoUrl = video, IdPueblo = idPueblo });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Actualizar
        public static async Task<bool> ActualizarActividad(int id, string nombre, string descrpcion, string imagen, Video video)
        {
            try
            {
                var actualizarActividad = (await firebase
                .Child("Actividades")
                .OnceAsync<Actividad>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase
                .Child("Actividades")
                .Child(actualizarActividad.Key)
                .PutAsync(new Actividad() { Id = id, Nombre = nombre, Descripcion = descrpcion, ImagenPrincipal = imagen, VideoUrl = video });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Eliminar
        public static async Task<bool> EliminarActividad(int id)
        {
            try
            {
                var eliminarActividad = (await firebase
                .Child("Actividades")
                .OnceAsync<Actividad>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase.Child("Actividades").Child(eliminarActividad.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //MÉTODOS CRUD FOTOS

        public static async Task<List<Foto>> ObtenerTodasFotos()
        {
            try
            {
                var listaFotos = (await firebase
                .Child("Fotos")
                .OnceAsync<Foto>()).Select(item =>
                new Foto
                {
                    Id = item.Object.Id,
                    Nombre = item.Object.Nombre,
                    Imagen = item.Object.Imagen,
                    IdPueblo = item.Object.IdPueblo
                }).ToList();
                return listaFotos;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        public static async Task<List<Foto>> ObtenerTodasFotosPueblo(int idPueblo)
        {
            try
            {
                var todasFotos = await ObtenerTodasFotos();
                await firebase.Child("Fotos").OnceAsync<Foto>();
                return todasFotos.Where(a => a.IdPueblo.Equals(idPueblo)).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Leer 
        public static async Task<Foto> ObtenerFoto(int id)
        {
            try
            {
                var todasFotos = await ObtenerTodasFotos();
                await firebase
                .Child("Fotos")
                .OnceAsync<Foto>();
                return todasFotos.Where(a => a.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Insertar foto
        public static async Task<bool> InsertarFoto(int id, string nombre, string imagen, int idPueblo)
        {
            try
            {
                await firebase
                .Child("Fotos")
                .PostAsync(new Foto() { Id = id, Nombre = nombre, Imagen = imagen, IdPueblo = idPueblo });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Actualizar
        public static async Task<bool> ActualizarFoto(int id, string nombre, string imagen)
        {
            try
            {
                var actualizarFoto = (await firebase
                .Child("Fotos")
                .OnceAsync<Foto>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase
                .Child("Fotos")
                .Child(actualizarFoto.Key)
                .PutAsync(new Foto() { Id = id, Nombre = nombre, Imagen = imagen});
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Eliminar
        public static async Task<bool> EliminarFoto(int id)
        {
            try
            {
                var eliminarFoto = (await firebase
                .Child("Fotos")
                .OnceAsync<Foto>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase.Child("Fotos").Child(eliminarFoto.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        public static async Task<string> SubirFoto(Stream fileStream, string fileName)
        {
            var imageUrl = await firebaseStorage
                .Child("Fotos")
                .Child(fileName)
                .PutAsync(fileStream);
            return imageUrl;
        }

        public static async Task<string> CargarFoto(string fileName)
        {
            return await firebaseStorage
                .Child("Fotos")
                .Child(fileName)
                .GetDownloadUrlAsync();
        }

        public static async Task BorrarFoto(string fileName)
        {
            await firebaseStorage
                 .Child("Fotos")
                 .Child(fileName)
                 .DeleteAsync();
        }

        //MÉTODOS CRUD VIDEOS

        public static async Task<List<Video>> ObtenerTodosVideos()
        {
            try
            {
                var listaVideos = (await firebase
                .Child("Videos")
                .OnceAsync<Video>()).Select(item =>
                new Video
                {
                    Id = item.Object.Id,
                    Nombre = item.Object.Nombre,
                    Videoclip = item.Object.Videoclip,
                    IdPueblo = item.Object.IdPueblo
                }).ToList();
                return listaVideos;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        public static async Task<List<Video>> ObtenerTodosVideosPueblo(int idPueblo)
        {
            try
            {
                var todasVideos = await ObtenerTodosVideos();
                await firebase.Child("Videos").OnceAsync<Video>();
                return todasVideos.Where(a => a.IdPueblo.Equals(idPueblo)).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Leer 
        public static async Task<Video> ObtenerVideo(int id)
        {
            try
            {
                var todosVideos = await ObtenerTodosVideos();
                await firebase
                .Child("Videos")
                .OnceAsync<Video>();
                return todosVideos.Where(a => a.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Insertar video
        public static async Task<bool> InsertarVideo(int id, string nombre, string videoclip, int idPueblo)
        {
            try
            {
                await firebase
                .Child("Videos")
                .PostAsync(new Video() { Id = id, Nombre = nombre, Videoclip = videoclip, IdPueblo = idPueblo });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Actualizar
        public static async Task<bool> ActualizarVideo(int id, string nombre, string videoclip)
        {
            try
            {
                var actualizarVideo = (await firebase
                .Child("Videos")
                .OnceAsync<Video>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase
                .Child("Videos")
                .Child(actualizarVideo.Key)
                .PutAsync(new Video() { Id = id, Nombre = nombre, Videoclip = videoclip});
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        //Eliminar
        public static async Task<bool> EliminarVideo(int id)
        {
            try
            {
                var eliminarVideo = (await firebase
                .Child("Videos")
                .OnceAsync<Video>()).Where(a => a.Object.Id == id).FirstOrDefault();
                await firebase.Child("Videos").Child(eliminarVideo.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }

        public static async Task<string> SubirVideo(Stream fileStream, string fileName)
        {
            var imageUrl = await firebaseStorage
                .Child("Videos")
                .Child(fileName)
                .PutAsync(fileStream);
            return imageUrl;
        }

        public static async Task<string> CargarVideo(string fileName)
        {
            return await firebaseStorage
                .Child("Videos")
                .Child(fileName)
                .GetDownloadUrlAsync();
        }

        public static async Task BorrarVideo(string fileName)
        {
            await firebaseStorage
                 .Child("Videos")
                 .Child(fileName)
                 .DeleteAsync();
        }
    }
}