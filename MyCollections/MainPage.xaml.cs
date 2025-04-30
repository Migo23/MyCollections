using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace MyCollections
{
    public partial class MainPage : ContentPage
    {
        private List<Collection> _collections;

        public MainPage()
        {
            InitializeComponent();
            FileService.GetDebugPath();
            _collections = FileService.LoadCollections();
            CollectionsView.ItemsSource = _collections;
            CollectionsView.SelectionChanged += OnCollectionSelected;
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            var name = await DisplayPromptAsync("Nowa kolekcja", "Podaj nazwę kolekcji:");
            if (string.IsNullOrWhiteSpace(name))
                return;

            var col = new Collection { Name = name };
            _collections.Add(col);
            FileService.SaveCollection(col);
            CollectionsView.ItemsSource = null;
            CollectionsView.ItemsSource = _collections;
        }

        private async void OnCollectionSelected(object s, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;
            var col = (Collection)e.CurrentSelection[0];
            await Navigation.PushAsync(new CollectionPage(col));
            CollectionsView.SelectedItem = null;
        }
    }
}
