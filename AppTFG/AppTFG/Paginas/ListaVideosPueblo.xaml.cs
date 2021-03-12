﻿using Acr.UserDialogs;
using AppTFG.FormsVideoLibrary;
using AppTFG.Helpers;
using AppTFG.Modelos;
using AppTFG.Servicios;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaVideosPueblo : ContentPage
    {
        public Video Video;
        public ListaVideosPueblo(Pueblo pueblo)
        {
            InitializeComponent();
            Title = "Videos de " + pueblo.Nombre;
            BindingContext = pueblo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            var pueblo = (Pueblo)BindingContext;
            if (pueblo != null)
            {
                lsvVideosPueblo.ItemsSource = null;
                lsvVideosPueblo.ItemsSource = await FirebaseHelper.ObtenerTodosVideosPueblo(pueblo.Id);
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

        public async void LsvVideosPueblo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var dato = (Video)e.SelectedItem;
                await Navigation.PushAsync(new SubirVideo(dato));
                lsvVideosPueblo.SelectedItem = null;
            }
            catch (Exception)
            {

            }
        }

        public async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            Label nombreUsuario = new Label();
            nombreUsuario.SetBinding(Label.TextProperty, new Binding("Nombre", source: AppShell.inicio));
            string nombre = nombreUsuario.Text;
            Usuario user = await FirebaseHelper.ObtenerUsuario(nombre);
            Pueblo puebloUser = await FirebaseHelper.ObtenerPueblo(user.UsuarioId);
            //var pueblo = (Pueblo)BindingContext;
            await Navigation.PushAsync(new SubirVideo(new Video() { IdPueblo = puebloUser.Id }));
        }
    }
}