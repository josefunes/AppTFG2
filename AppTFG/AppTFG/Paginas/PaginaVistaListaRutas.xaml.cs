﻿using Acr.UserDialogs;
using AppTFG.Helpers;
using AppTFG.Modelos;
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
    public partial class PaginaVistaListaRutas : ContentPage
    {
        public PaginaVistaListaRutas(Pueblo pueblo)
        {
            InitializeComponent();
            Title = "Rutas de " + pueblo.Nombre;
            BindingContext = pueblo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            //var bd = new ServicioBaseDatos<Ruta>();
            var pueblo = (Pueblo)BindingContext;
            if (pueblo != null)
            {
                lsvRutasPueblo.ItemsSource = null;
                lsvRutasPueblo.ItemsSource = await FirebaseHelper.ObtenerTodasRutasPueblo(pueblo.Id);
            }
            Loading(false);
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

        private async void LsvRutasPueblo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Ruta)e.SelectedItem;
                await Navigation.PushAsync(new PaginaVistaRuta(dato));
                lsvRutasPueblo.SelectedItem = null;
            }
            catch (Exception)
            {
            }
        }
    }
}