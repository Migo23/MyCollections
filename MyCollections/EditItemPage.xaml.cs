using MyCollections;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MyCollections
{
    public partial class EditItemPage : ContentPage
    {
        private Collection _collection;
        private CollectionItem _item;

        public EditItemPage(Collection collection, CollectionItem item)
        {
            InitializeComponent();
            _collection = collection;
            _item = item;

            BackgroundColor = Color.FromArgb("#1e1e2f");
            Resources = new ResourceDictionary
            {
                {"PrimaryColor", Color.FromArgb("#ff6b6b")},
                {"AccentColor", Color.FromArgb("#4ecdc4")},
                {"TextColor", Colors.White},
                {"EntryBackground", Color.FromArgb("#2c2c3a")}
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            StatusPicker.ItemsSource = new List<string>
            {
                "nowy", "u¿yty", "na sprzeda¿", "sprzedany", "chcê kupiæ"
            };

            NameEntry.Text = _item.Name;
            NameEntry.TextColor = (Color)Resources["TextColor"];
            NameEntry.BackgroundColor = (Color)Resources["EntryBackground"];

            PriceEntry.Text = _item.Price.ToString();
            PriceEntry.TextColor = (Color)Resources["TextColor"];
            PriceEntry.BackgroundColor = (Color)Resources["EntryBackground"];

            StatusPicker.SelectedItem = _item.Status;
            StatusPicker.TextColor = (Color)Resources["TextColor"];
            StatusPicker.BackgroundColor = (Color)Resources["EntryBackground"];

            RatingSlider.Value = _item.Rating;
            RatingSlider.MinimumTrackColor = (Color)Resources["PrimaryColor"];
            RatingSlider.MaximumTrackColor = (Color)Resources["AccentColor"];
            RatingLabel.Text = _item.Rating.ToString();
            RatingLabel.TextColor = (Color)Resources["AccentColor"];

            CommentEditor.Text = _item.Comment;
            CommentEditor.TextColor = (Color)Resources["TextColor"];
            CommentEditor.BackgroundColor = (Color)Resources["EntryBackground"];

            RatingSlider.ValueChanged += (s, e) =>
            {
                RatingLabel.Text = ((int)e.NewValue).ToString();
            };

            SaveBtn.BackgroundColor = (Color)Resources["PrimaryColor"];
            SaveBtn.TextColor = Colors.White;

            DeleteBtn.BackgroundColor = Colors.Gray;
            DeleteBtn.TextColor = Colors.White;
        }

        private async void OnSave(object sender, EventArgs e)
        {
            string name = NameEntry.Text?.Trim() ?? "";
            decimal price = decimal.TryParse(PriceEntry.Text, out var p) ? p : 0;
            string status = StatusPicker.SelectedItem?.ToString() ?? "";
            int rating = (int)RatingSlider.Value;
            string comment = CommentEditor.Text ?? "";

            bool isDuplicate = !_collection.Items.Contains(_item) &&
                _collection.Items.Any(i =>
                    i.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                    i.Price == price &&
                    i.Status == status &&
                    i.Rating == rating &&
                    i.Comment == comment);

            if (isDuplicate)
            {
                await DisplayAlert("B³¹d", "Taki przedmiot ju¿ istnieje w kolekcji.", "OK");
                return;
            }

            _item.Name = name;
            _item.Price = price;
            _item.Status = status;
            _item.Rating = rating;
            _item.Comment = comment;

            if (!_collection.Items.Contains(_item))
                _collection.Items.Add(_item);

            FileService.SaveCollection(_collection);
            await Navigation.PopAsync();
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            _collection.Items.Remove(_item);
            FileService.SaveCollection(_collection);
            await Navigation.PopAsync();
        }
    }
}
