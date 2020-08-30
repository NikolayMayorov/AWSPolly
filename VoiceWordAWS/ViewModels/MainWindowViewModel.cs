using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using CsvHelper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VoiceWordAWS.Model;
using VoiceWordAWS.Sevices;

using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.FolderBrowser;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using IOPath = System.IO.Path;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon.Polly;
using VoiceWordAWS.Sevices.Abstract;

namespace VoiceWordAWS.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;

        private readonly ISettingsService _settingsService;

        private readonly TextService _textService;

        private readonly string _fileSettings = "settings.json";

        private string _pathFolderAudio;

        private Settings _settings;

        private string _url = "https://catchenglish.ru/teksty/teksty-dlya-6-go-klassa/astronomy.html";

        private ObservableCollection<Word> _words = new ObservableCollection<Word>();

        private PollyModel _polly;

        private string _accessKey = "AKIAJQIYI4VDPPE43BEA";//String.Empty;

        private string _secretKey = "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";//String.Empty;

        private VoiceId _voice;

        private List<VoicesLang> _voiceLang = new List<VoicesLang>();

        private string _currentVoice;

        private string _currentLang;

        private VoicesLang _currentVoicesLang;


        private ObservableCollection<string> _availableVoices= new ObservableCollection<string>();

        private ObservableCollection<string> _availableLanguages= new ObservableCollection<string>();

        #region default settings

        private readonly VoiceId __VOICE = VoiceId.Kendra;

        private readonly LanguageCode __LANGUAGE = LanguageCode.EnUS;

        private readonly string __URL = "https://lim-english.com/posts/prostie-teksti-na-angliiskom-dlya-nachinaushih/";

        private readonly string __FILE_SETTINGS = "settings.json";

        private readonly string __PATH_FOLDER_AUDIO = "Audio/";

        #endregion


        public MainWindowViewModel(TextService textService, IDialogService dialogService, ISettingsService settingsService)
        {
           
            _textService = textService;
            _settingsService = settingsService;
            _settings = _settingsService.GetSettings();
            if (_settings == null)
            {
                _settings = new Settings()
                {
                    Language = __LANGUAGE.Value,
                    Voice = __VOICE.Value,
                    PathFolderAudio = __PATH_FOLDER_AUDIO

                };
         
                _settingsService.SaveSettings(_settings);
            }
            _pathFolderAudio = _settings.PathFolderAudio;
            _currentLang = _settings.Language;
            _currentVoice = _settings.Voice;

            GetText = new RelayCommand(execute: OnGetText, canExecute: CanOnGetText);
            GetVoice = new RelayCommand(execute: OnGetVoice, canExecute: CanGetVoice);
            SelectAll = new RelayCommand(execute: OnSelectAll, canExecute: CanSelectAll);
            DeselectAll = new RelayCommand(execute: OnDeselectAll, canExecute: CanDeselectAll);
            OpenSaveFolder = new RelayCommand(execute: OnOpenSaveFolder);
            ConnectAWSPolly = new RelayCommand(execute: OnConnectAWSPolly, CanConnectAWSPolly);


            this.dialogService = dialogService;
        }


        private void OnOpenSaveFolder()
        {
        
           var settingsFolder = new FolderBrowserDialogSettings()
           {
               Description = "Куда сохранять аудио файлы?"
           };
           bool? success = dialogService.ShowFolderBrowserDialog(this, settingsFolder);
           if (success == true)
           {
               PathFolderAudio = settingsFolder.SelectedPath;
           }


        }


        public RelayCommand GetText { get; set; }

        public RelayCommand GetVoice { get; set; }

        public RelayCommand SelectAll { get; set; }

        public RelayCommand DeselectAll { get; set; }

        public RelayCommand OpenSaveFolder { get; set; }

        public RelayCommand ConnectAWSPolly { get; set; }

        public string PathFolderAudio
        {
            get => _pathFolderAudio;
            private set
            {
                if (Set(ref _pathFolderAudio, value))
                {
                    var settings = _settingsService.GetSettings();
                    settings.PathFolderAudio = _pathFolderAudio;
                    _settingsService.SaveSettings(settings);
                }
            }
        }


        public string Title { get; } = "Voice word";

        public string URL
        {
            get => _url;

            set
            {
                if (Set(field: ref _url, newValue: value))
                    GetText.RaiseCanExecuteChanged();
            }
        }

        public string AccessKey
        {
            get => _accessKey;

            set
            {
                Set(field: ref _accessKey, newValue: value);
                GetVoice.RaiseCanExecuteChanged();
            }
        }

        public string SecretKey
        {
            get => _secretKey;

            set
            {
                Set(field: ref _secretKey, newValue: value);
                GetVoice.RaiseCanExecuteChanged();
            }
        }

        public VoiceId Voice
        {
            get => _voice;
            set
            {
                Set(ref _voice, value);
            }
        }

        public List<VoicesLang> VoiceLang
        {
            get => _voiceLang;
            set => Set(field: ref _voiceLang, newValue: value);
        }

        public ObservableCollection<Word> Words
        {
            get => _words;
            set => Set(field: ref _words, newValue: value);
        }

        public string CurrentVoice
        {
            get => _currentVoice;
            set
            {
                Set(field: ref _currentVoice, newValue: value);
                Settings settings = _settingsService.GetSettings();
                _settingsService.SaveSettings(new Settings()
                {
                    Voice = CurrentVoice,
                    PathFolderAudio = settings.PathFolderAudio,
                    Language = settings.Language
                });
            }
        }

        public string CurrentLang
        {
            get => _currentLang;
            set
            {
                Set(field: ref _currentLang, newValue: value);
                Settings settings = _settingsService.GetSettings();
                _settingsService.SaveSettings(new Settings()
                {
                    Voice = settings.Voice,
                    PathFolderAudio = settings.PathFolderAudio,
                    Language = _currentLang
                });
            }
        }

        //public VoicesLang CurrentVoicesLang
        //{
        //    get => _currentVoicesLang;
        //    set
        //    {
        //        Set(field: ref _currentVoicesLang, newValue: value);
        //    }
        //}

        public ObservableCollection<string> AvailableVoices
        {
            get => _availableVoices;
            set
            {
                Set(field: ref _availableVoices, newValue: value);
            }
        }

        public ObservableCollection<string> AvailableLanguages
        {
            get => _availableLanguages;
            set
            {
                Set(field: ref _availableLanguages, newValue: value);
            }
        }

       
        #region Commands

        private bool CanOnGetText()
        {
            return !string.IsNullOrWhiteSpace(value: _url);
        }

        private void OnGetText()
        {
            //var url = "https://lim-english.com/posts/prostie-teksti-na-angliiskom-dlya-nachinaushih/";
            //  var url = "https://catchenglish.ru/teksty/teksty-dlya-6-go-klassa/astronomy.html";
            var resHtml = string.Empty;
            try
            {
                resHtml = _textService.GetHtmlFromWeb(url: _url);
            }
            catch
            {
                //сделать вывод сообщения об ошибке
            }

            var resSimple = _textService.GetTextFromHtml(htmlText: resHtml);

            var resEng = _textService.GetEngText(text: resSimple);

            var arr = _textService.GetWords(text: resEng);
            _words.Clear();
            foreach (var w in arr)
                _words.Add(new Word
                {
                    Value = w,
                    IsChecked = false
                });
        }

        private bool CanDeselectAll()
        {
            return true;
        }

        private void OnDeselectAll()
        {
            foreach (var item in Words) item.IsChecked = false;
  
            Words = new ObservableCollection<Word>(_words);
        }

        private bool CanSelectAll()
        {
            return true;
        }

        private void OnSelectAll()
        {
            foreach (var item in Words) item.IsChecked = true;

            Words = new ObservableCollection<Word>(_words);
        }

        private bool CanGetVoice()
        {
            if (!string.IsNullOrWhiteSpace(value: _accessKey) || !string.IsNullOrWhiteSpace(value: _secretKey))
                return true;
            else
            {
                return false;
            }
        }

        private void OnGetVoice()
        {
            _polly = new PollyModel(_accessKey, _secretKey);
            foreach (var item in _words)
            {
                if (item.IsChecked)
                    _polly.VoiсeWord(item.Value, _pathFolderAudio, _currentVoice, _currentLang);
            }
            
        }

        private bool CanConnectAWSPolly()
        {
            return true;
        }

        private void OnConnectAWSPolly()
        {
            _polly = new PollyModel(_accessKey, _secretKey);
            VoiceLang = _polly.GetVoices();
            foreach (var item in _voiceLang)
            { 
                AvailableVoices.Add(item.Voice);
                AvailableLanguages.Add(item.Lang);
            }

            CurrentVoice = _settingsService.GetSettings().Voice;
            _currentLang = _settingsService.GetSettings().Language;
            _currentVoicesLang = new VoicesLang()
            {
                Lang = _settingsService.GetSettings().Language,
                Voice = _settingsService.GetSettings().Voice
            };
        }


        #endregion
    }
}