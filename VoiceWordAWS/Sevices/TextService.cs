using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace VoiceWordAWS.Sevices
{
    public  class TextService
    {
        private  char[] split = { '\t', '\r', '\n', ' ' };

        /// <summary>
        /// Получение html документа
        /// </summary>
        public  string GetHtmlFromWeb(string url)
        {
            var webGet = new HtmlWeb();
            var doc = webGet.Load(url: url);
            var htmlText = doc.ParsedText;
            return htmlText;
        }

        /// <summary>
        /// Парсинг HTML документа. Получение только текста 
        /// </summary>
        public  string GetTextFromHtml(string htmlText)
        {
            var patternTags = @"<[\w\W]*?>";

            var patternScript = @"<script[\w\W]*?/script>";

            var patternStyle = @"<style[\w\W]*?/style>";

          //  var patternHead = @"<head[\w\W]*?/head>";

            var patternBody = @"<body[\w\W]*?/body>";

            var patternSpecialChar = @"&[\w\W]*?;";

            var patternHref = @"<a.*?/a>";

            var patternComments = @"<!--[\w\W]*?-->";

            var text = string.Empty;

            var result = string.Empty;

            var regex = new Regex(pattern: patternBody, options: RegexOptions.IgnoreCase);
            var match = regex.Match(input: htmlText);
            if (match.Success)
            {
                foreach (var s in match.Captures) text += s;
                regex = new Regex(pattern: patternComments, options: RegexOptions.IgnoreCase);
                text = regex.Replace(input: text, "");

                regex = new Regex(pattern: patternScript, options: RegexOptions.IgnoreCase);
                text = regex.Replace(input: text, "");

                regex = new Regex(pattern: patternStyle, options: RegexOptions.IgnoreCase);
                text = regex.Replace(input: text, "");

                regex = new Regex(pattern: patternHref, options: RegexOptions.IgnoreCase);
                text = regex.Replace(input: text, "");

                regex = new Regex(pattern: patternSpecialChar, options: RegexOptions.IgnoreCase);
                text = regex.Replace(input: text, "");

                regex = new Regex(pattern: patternTags, options: RegexOptions.IgnoreCase);
                text = regex.Replace(input: text, "");
            }
            else
            {
                regex = new Regex(pattern: patternComments, options: RegexOptions.IgnoreCase);
                text = regex.Replace(input: text, "");

                regex = new Regex(pattern: patternScript, options: RegexOptions.IgnoreCase);
                text = regex.Replace(input: text, "");

                regex = new Regex(pattern: patternStyle, options: RegexOptions.IgnoreCase);
                text = regex.Replace(input: text, "");

                regex = new Regex(pattern: patternHref, options: RegexOptions.IgnoreCase);
                text = regex.Replace(input: text, "");

                regex = new Regex(pattern: patternSpecialChar, options: RegexOptions.IgnoreCase);
                text = regex.Replace(input: text, "");

                regex = new Regex(pattern: patternTags, options: RegexOptions.IgnoreCase);
                text = regex.Replace(input: text, "");
            }



            return text;
        }

        /// <summary>
        /// Получение только английского текста
        /// </summary>
        public  string GetEngText(string text)
        {
            var result = string.Empty;

            foreach (var ch in text)
            {
                if (char.IsLetter(ch))
                {
                    if ((ch >= 0x61 && ch <= 0x7A) || (ch >= 0x41 && ch <= 0x5A))
                        result += ch;
                }
                else if (char.IsPunctuation(ch) || char.IsSeparator(ch))
                {
                    result += " ";
                }
            }

            return result;
        }


        /// <summary>
        /// Получение из текста не повторяющихся слов
        /// </summary>
        public  IEnumerable<string> GetWords(string text)
        {
            
            return text.ToLower().Split(separator: split, options: StringSplitOptions.RemoveEmptyEntries).Distinct();
        }
    }
}

