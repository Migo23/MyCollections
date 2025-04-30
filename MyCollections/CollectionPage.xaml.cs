using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace MyCollections
{
    public partial class CollectionPage : ContentPage
    {
        private Collection _col;
        private List<CollectionItem> _sortedItems;

        public CollectionPage(Collection col)
        {
            InitializeComponent();
            _col = col;
            BindingContext = _col;
            RefreshList();
            ItemsView.SelectionChanged += OnItemSelected;
        }

        private void RefreshList()
        {
            _sortedItems = _col.Items
                .OrderBy(i => i.Status == "sprzedany" ? 1 : 0)
                .ThenBy(i => i.Name)
                .ToList();
            ItemsView.ItemsSource = _sortedItems;
        }

        private async void OnAddItem(object sender, EventArgs e)
        {
            var newItem = new CollectionItem();
            await Navigation.PushAsync(new EditItemPage(_col, newItem));
        }

        private async void OnItemSelected(object s, SelectionChangedEventArgs e)
        {
            if (!e.CurrentSelection.Any()) return;
            var item = (CollectionItem)e.CurrentSelection.First();
            await Navigation.PushAsync(new EditItemPage(_col, item));
            ItemsView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshList();
            FileService.SaveCollection(_col);
        }
    }
}
