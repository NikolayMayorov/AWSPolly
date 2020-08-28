using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VoiceWordAWS.Model;
using VoiceWordAWS.Sevices;

namespace VoiceWordAWS.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly TextService _textService;

        private string _url = "https://catchenglish.ru/teksty/teksty-dlya-6-go-klassa/astronomy.html";

        private ObservableCollection<Word> _words = new ObservableCollection<Word>();

        private PollyModel _polly;

        private string _accessKey = String.Empty;

        private string _secretKey = String.Empty;



        public MainWindowViewModel(TextService textService)
        {
            _textService = textService;
            GetText = new RelayCommand(execute: OnGetText, canExecute: CanOnGetText);
            GetVoice = new RelayCommand(execute: OnGetVoice, canExecute: CanGetVoice);
            SelectAll = new RelayCommand(execute: OnSelectAll, canExecute: CanSelectAll);
            DeselectAll = new RelayCommand(execute: OnDeselectAll, canExecute: CanDeselectAll);
            
        }


        public RelayCommand GetText { get; set; }

        public RelayCommand GetVoice { get; set; }

        public RelayCommand SelectAll { get; set; }

        public RelayCommand DeselectAll { get; set; }

        public string Title { get; } = "Voice word";

        public string URL
        {
            get => _url;

            set
            {
                Set(field: ref _url, newValue: value);
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

        public ObservableCollection<Word> Words
        {
            get => _words;
            set => Set(field: ref _words, newValue: value);
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
                //сделать вывод сообщения об ишибке
            }

            var resSimple = _textService.GetTextFromHtml(htmlText: resHtml);

            var resEng = _textService.GetEngText(text: resSimple);

            var arr = _textService.GetWords(text: resEng);

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
                    _polly.VoiсeWord(item.Value, "Audio");
            }
            
        }

        #endregion
    }
}