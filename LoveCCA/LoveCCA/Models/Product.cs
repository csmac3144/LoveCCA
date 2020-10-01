using Plugin.CloudFirestore.Attributes;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace LoveCCA.Models
{
    public class Product : INotifyPropertyChanged
    {
        [Ignored]
        public Command TappedCommand { get; set; }
        [Ignored]
        private Day ParentDay { get; }
        public Product(Day day) : this()
        {
            ParentDay = day;
            SelectionGlyph = "⚪";
        }

        public Product()
        {
            TappedCommand = new Command(OnTapped);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async void OnTapped(object obj)
        {
            await ParentDay.SelectOption(obj as Product);
        }

        public void Notify()
        {
            OnPropertyChanged(nameof(SelectionGlyph));
        }
        [Id]
        public string Id { get; set; }
        public int MenuIndex { get; set; }
        public string Name { get; set; }
        [Ignored]
        public string SelectionGlyph { get; set; }
        public string Glyph { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
        [Ignored]
        public string PriceLabel => Price.ToString("C");
    }


}
