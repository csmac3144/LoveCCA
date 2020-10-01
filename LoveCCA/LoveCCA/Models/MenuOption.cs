using Plugin.CloudFirestore.Attributes;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace LoveCCA.Models
{
    public class MenuOption : INotifyPropertyChanged
    {
        [Ignored]
        public Command TappedCommand { get; set; }
        private Day ParentDay { get; }
        public MenuOption(Day day) : this()
        {
            ParentDay = day;
            Glyph = "⚪";
        }

        public MenuOption()
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
            await ParentDay.SelectOption(obj as MenuOption);
        }

        public void Notify()
        {
            OnPropertyChanged(nameof(Glyph));
        }
        public string Id { get; set; }
        [Ignored]
        public string Glyph { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Ignored]
        public string PriceLabel => Price.ToString("C");
    }


}
