﻿using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubirFoto : ContentPage
    {
        Foto Foto;
        public SubirFoto(Foto foto)
        {
            InitializeComponent();
            Foto = foto;
            BindingContext = foto;

            if (foto.Id == 0)
            {
                this.ToolbarItems.RemoveAt(1);
                Title = "Nueva foto";
            }
            else
            {
                Title = Foto.Nombre;
            }
        }

        void Loading(bool mostrar)
        {
            if (mostrar)
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
            }
            else
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void BtnImagen_Clicked(object sender, EventArgs e)
        {
            var imagen = await ServicioMultimedia.SeleccionarImagen();
            Foto.Imagen = imagen.Path;
            Foto.Stream = imagen.GetStream();
            imgFoto.Source = ImageSource.FromFile(imagen.Path);
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var foto = (Foto)BindingContext;
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                UserDialogs.Instance.Alert("Advertencia", Constantes.TitleImagenRequired, "OK");
                return;
            }
            if (foto.Id > 0)
            {
                await FirebaseHelper.ActualizarFoto(foto.Id, foto.Nombre, foto.Imagen);
            } 
            else
            {
                await FirebaseHelper.InsertarFoto(foto.Id = Constantes.GenerarId(), foto.Nombre, foto.Imagen = await FirebaseHelper.SubirFoto(foto.Stream, foto.Nombre), foto.IdPueblo);
            }
            Loading(false);
            UserDialogs.Instance.Alert("Correcto", "Registro realizado correctamente", "OK");
            await Navigation.PopAsync();
        }

        async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Advertencia", "¿Deseas eliminar este registro?", "Si", "No"))
            {
                Loading(true);
                await FirebaseHelper.EliminarFoto(Foto.Id);
                await FirebaseHelper.BorrarFoto(Foto.Nombre);
                Loading(false);
                UserDialogs.Instance.Alert("Correcto", "Registro eliminado correctamente", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}