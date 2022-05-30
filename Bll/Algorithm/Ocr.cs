using IronOcr;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Path = System.IO.Path;

namespace Bll.Algorithm
{
    public class Ocr
    {

        public List<List<string>> getTextFromPath(string path)
        {
            var Ocr = new IronTesseract();
            Ocr.Language = OcrLanguage.HebrewAlphabetFast;
            Ocr.AddSecondaryLanguage(OcrLanguage.English);
            string text = "";
            if (path.EndsWith(".jpg"))
            {
                using (var Input = new OcrInput(path))
                {
                    var Result = Ocr.Read(Input);
                    text = Result.Text;
                }

            }
            else
            {
                using (var input = new OcrInput())
                {
                    input.AddPdf(path, "");
                    var Result = Ocr.Read(input);
                    text = Result.Text;
                }
            }

            List<string> lines = text.Split(new[] { Environment.NewLine, "\n" },
            StringSplitOptions.None).ToList();

            List<List<string>> allWords = new List<List<string>>();

            foreach (var line in lines)
            {
                if (textToWordes(line) != null && textToWordes(line).Count > 0)
                    allWords.Add(textToWordes(line));
            }

            return allWords;

        }
     
        public List<string> textToWordes(string line)
        {
            if (line == "") return null;
            char[] splites = new char[] { '\t', '\n', '\r', ' ','"' };
            List<string> wordes = line.Split(splites).ToList();
            List<string> listWordes = new List<string>();
            foreach (var w in wordes)
            {   //??
                var word = Regex.Replace(w, "", string.Empty);
                new List<string> { "@", ",", ";", "+", "=", "'", "[", "]", "(", ")", "\"", "!", " .", "|" ,}.ForEach(e => word = word.Replace(e, ""));
                if (word.Length > 1)
                    listWordes.Add(word);
            }
            return listWordes;
        }

    }
}

