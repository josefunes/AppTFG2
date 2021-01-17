﻿using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaActividad : ContentPage
    {
        ServicioBaseDatos<Actividad> bd;
        Actividad Actividad;
        public PaginaActividad(Actividad actividad)
        {
            InitializeComponent();
            Title = "Nueva Actividad";
            Actividad = actividad;
            this.BindingContext = actividad;
            bd = new ServicioBaseDatos<Actividad>();

            if (actividad.Id == 0)
                this.ToolbarItems.RemoveAt(1);
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        private async void BtnImagen_Clicked(object sender, EventArgs e)
        {
            var imagen = await ServicioMultimedia.SeleccionarImagen();
            Actividad.ImagenPrincipal = imagen.Path;
            imgActividad.Source = ImageSource.FromFile(imagen.Path);
        }

        async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var actividad = (Actividad)this.BindingContext;
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                await DisplayAlert("Advertencia", Constantes.TitleActividadRequired, "OK");
                return;
            }
            if (imgActividad.Equals(null))
            {
                await DisplayAlert("Advertencia", Constantes.InsertImageRequired, "OK");
                return;
            }
            if (actividad.Id > 0)
                await bd.Actualizar(actividad);
            else
                await bd.Agregar(actividad);

            Loading(false);
            await DisplayAlert("Correcto", "Registro realizado correctamente", "OK");
            await Navigation.PopAsync();
        }

        async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Advertencia", "¿Deseas eliminar este registro?", "Si", "No"))
            {
                Loading(true);
                await bd.Eliminar(((Actividad)this.BindingContext).Id);
                Loading(false);
                await DisplayAlert("Correcto", "Registro eliminado correctamente", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}