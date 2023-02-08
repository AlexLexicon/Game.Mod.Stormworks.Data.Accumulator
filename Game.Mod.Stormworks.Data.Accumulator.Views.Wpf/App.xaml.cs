using Game.Mod.Stormworks.Data.Accumulator.ViewModels;
using Lexicon.Common.Wpf.DependencyInjection;
using Lexicon.Common.Wpf.DependencyInjection.Mvvm.Extensions;

namespace Game.Mod.Stormworks.Data.Accumulator.Views.Wpf;
public partial class App : System.Windows.Application
{
    public App()
    {
        WpfApplicationBuilder builder = WpfApplication.CreateBuilder(this);

        builder.Services.AddDataContext<MainWindowViewModel>().WithHostElement<MainWindowView>();

        WpfApplication app = builder.Build();

        app.CreateAndShow<MainWindowViewModel>();
    }
}
