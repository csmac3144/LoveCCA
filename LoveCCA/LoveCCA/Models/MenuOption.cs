using Plugin.CloudFirestore.Attributes;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace LoveCCA.Models
{
    public class MenuOption : INotifyPropertyChanged
    {
        public Command TappedCommand { get; set; }
        private Day ParentDay { get; }
        public MenuOption(Day day) : this()
        {
            ParentDay = day;
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

        public void OnTapped(object obj)
        {
            ParentDay.SelectOption(obj as MenuOption);
        }

        public void Notify()
        {
            OnPropertyChanged(nameof(Glyph));
        }

        public string Glyph { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Id]
        public string PriceLabel => Price.ToString("C");
    }


}
