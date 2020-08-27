using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VoiceWordAWS.Sevices;


namespace VoiceWordAWS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
      
            var url = "https://lim-english.com/posts/prostie-teksti-na-angliiskom-dlya-nachinaushih/";
          //  var url = "https://catchenglish.ru/teksty/teksty-dlya-6-go-klassa/astronomy.html";
            string resHtml = TextService.GetHtmlFromWeb(url);

            var resSimple = TextService.GetTextFromHtml(resHtml);

            var resEng = TextService.GetEngText(resSimple);

            var arr = TextService.GetWords(resEng).ToArray();

            

        }
    }
}
