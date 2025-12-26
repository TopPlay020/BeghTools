using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace BeghTools.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject, ITransientable
    {
#pragma warning disable CS8618
        [ObservableProperty]
        private object mainView;
        [ObservableProperty]
        private bool isAnimated;
        [ObservableProperty]
        private ObservableCollection<MenuItemModel> mainMenuItems;
        [ObservableProperty]
        private ObservableCollection<MenuItemModel> footerMenuItems;
        [ObservableProperty]
        private MenuItemModel mainSelectedItem;
        [ObservableProperty]
        private MenuItemModel footerSelectedItem;
        public MainWindowViewModel()
        {
            var MainMenuItemsUnOrdered = new ObservableCollection<MenuItemModel>();
            FooterMenuItems = new();
            foreach (var type in GetTypesImplementing<IPageMenu>())
            {
                var attr = type.GetCustomAttribute<PageInfoAttribute>()!;
                var PageType = GetTypeByName(type.Name.Substring(0, type.Name.Length - "ViewModel".Length))!;

                if (attr.IsFooterPage)
                    FooterMenuItems.Add(new MenuItemModel { Title = attr.PageTitle, Icon = attr.PageIcon, PageType = PageType, Order = attr.Order });
                else
                    MainMenuItemsUnOrdered.Add(new MenuItemModel { Title = attr.PageTitle, Icon = attr.PageIcon, PageType = PageType, Order = attr.Order });
            }

            MainMenuItems = new(
                MainMenuItemsUnOrdered.OrderBy(x => x.Order)
            );

            MainSelectedItem = mainMenuItems.Last();
        }

        void ChangeView(MenuItemModel? value, bool isMain)
        {
            if (value == null) return;
            if (isMain) FooterSelectedItem = null;
            else MainSelectedItem = null;
            IsAnimated = false;
            MainView = GetRequiredService(value.PageType);
            IsAnimated = true;
        }
        partial void OnMainSelectedItemChanged(MenuItemModel value) => ChangeView(value, true);
        partial void OnFooterSelectedItemChanged(MenuItemModel value) => ChangeView(value, false);

    }
}
