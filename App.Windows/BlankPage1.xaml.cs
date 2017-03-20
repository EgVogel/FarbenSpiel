﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        String inhalt = "0";

        public BlankPage1()
        {
            this.InitializeComponent();
        }

        public async void DatenHohlen()
        {
            StorageFolder Ordner = ApplicationData.Current.LocalFolder;     
            StorageFile score = await Ordner.CreateFileAsync("Highscore.txt", CreationCollisionOption.OpenIfExists);
            inhalt = await FileIO.ReadTextAsync(score);
            if (inhalt == "")
            {
                await FileIO.WriteTextAsync(score, "0");
            }
            Highscore.Text = "Persönlicher Highscore : " + inhalt;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            drehdich.Begin();
            DatenHohlen();
            Highscore.Text = "Persönlicher Highscore : " + inhalt;
        }

        private void Neues_Spiel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Exit();
        }
    }
}