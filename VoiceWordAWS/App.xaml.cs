using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;

using MvvmDialogs;
using VoiceWordAWS.Sevices;
using VoiceWordAWS.Sevices.Abstract;

namespace VoiceWordAWS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SimpleIoc.Default.Register<IDialogService>(() => new DialogService());
            SimpleIoc.Default.Register<ISettingsService>(() => new SettingsService());

        }


    }
}
