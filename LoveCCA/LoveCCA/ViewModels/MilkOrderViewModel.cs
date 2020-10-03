using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using LoveCCA.Models;
using LoveCCA.Views;
using System.Collections.Generic;
using LoveCCA.Services;
using System.Net.Http.Headers;
using System.Data;
using LoveCCA.Services.MealService;

namespace LoveCCA.ViewModels
{
    public class MilkOrderViewModel : BaseViewModel
    {
        private Item _selectedItem;

        public ObservableCollection<Day> Items { get; private set; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command DoneCommand { get; }
        public Command<Item> ItemTapped { get; }
        private MealCalendarService _mealCalendarService;
        private IOrderHistoryService _orderHistoryService;

        public MilkOrderViewModel()
        {
            Title = "Milk Order";
            Items = new ObservableCollection<Day>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Item>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);

            DoneCommand = new Command(async () => await Done());

            _mealCalendarService = new MealCalendarService();
            _orderHistoryService = new OrderHistoryService();

            Kids = new List<Student>();
            if (UserProfileService.Instance.CurrentUserProfile.Kids != null &&
                UserProfileService.Instance.CurrentUserProfile.Kids.Count > 0)
            {
                Kids = UserProfileService.Instance.CurrentUserProfile.Kids;
                _selectedKid = Kids[0];
            }
            OnPropertyChanged("Kids");
        }

        public async Task Done()
        {
            await Shell.Current.GoToAsync($"..");
        }

        public List<Student> Kids { get; private set; }

        private Student _selectedKid;
        public Student SelectedKid { 
            get {
                return _selectedKid;
            } 
            set
            {
                if (_selectedKid != null && value.Id != _selectedKid.Id)
                {
                    _selectedKid = value;
                    IsBusy = true;
                } 
                else
                {
                    _selectedKid = value;
                }
            }
        }

        public void OnDisappearing()
        {
            Items?.Clear();
            Items = null;
            _orderHistoryService = null;
            _mealCalendarService = null;
            SelectedItem = null;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                await _mealCalendarService.Initialize(DateTime.Now,_selectedKid,"Milk");
                foreach (var item in _mealCalendarService.WeekDays)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }

        internal async Task UpdateOrder(Day day, bool value)
        {
            string id = await _orderHistoryService.SaveMilkOrder(day, value);
            day.OrderId = id;
        }
    }
}