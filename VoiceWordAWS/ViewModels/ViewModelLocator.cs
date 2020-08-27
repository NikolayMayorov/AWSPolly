using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using VoiceWordAWS.Sevices;

namespace VoiceWordAWS.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(()=>SimpleIoc.Default);
            var services = SimpleIoc.Default;

            services.Register<MainWindowViewModel>();
            services.Register<TextService>();
        }

        public MainWindowViewModel MainWindowModel => ServiceLocator.Current.GetInstance<MainWindowViewModel>();
    }
}
