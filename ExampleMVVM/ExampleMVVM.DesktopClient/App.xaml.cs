using ExampleMVVM.DesktopClient.Repositories;
using ExampleMVVM.DesktopClient.ViewModels;
using ExampleMVVM.DesktopClient.Views;
using System.Windows;

namespace ExampleMVVM.DesktopClient
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainView mainView = new MainView();
            CustomerRepository customerRepository = new CustomerRepository();
            MainViewModel mainViewModel = new MainViewModel(customerRepository);

            mainViewModel.RequestClose += (sender, args) => mainView.Close();

            mainView.DataContext = mainViewModel;
            mainView.Show();
        }
    }
}
