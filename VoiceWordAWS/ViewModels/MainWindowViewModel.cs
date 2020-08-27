using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VoiceWordAWS.Sevices;


namespace VoiceWordAWS.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _title = "Voice word";

        private string _url = String.Empty;

        private readonly TextService _textService;

        public RelayCommand GetText { get; set; }

   

        public MainWindowViewModel(TextService textService)
        {
            _textService = textService;
            GetText = new RelayCommand(OnGetText, CanOnGetText);
           
           
        }

        private bool CanOnGetText()
        {
            return !string.IsNullOrWhiteSpace(URL);

        }

        private void OnGetText()
        {
            var url = "https://lim-english.com/posts/prostie-teksti-na-angliiskom-dlya-nachinaushih/";
            //  var url = "https://catchenglish.ru/teksty/teksty-dlya-6-go-klassa/astronomy.html";
            string resHtml = _textService.GetHtmlFromWeb(_url);

            var resSimple = _textService.GetTextFromHtml(resHtml);

            var resEng = _textService.GetEngText(resSimple);

            var arr = _textService.GetWords(resEng).ToArray();
        }


        public string Title
        {
            get => _title;
        }

        public string URL
        {
            
            get => _url;

            set
            {
                GetText.RaiseCanExecuteChanged();
                Set(ref _url, value);
            }
        }
    }
}
