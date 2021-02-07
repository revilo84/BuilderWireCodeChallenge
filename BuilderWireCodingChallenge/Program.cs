using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BuilderWireCodingChallenge
{
    public class Program
    {
        public static List<string> Words = new List<string>();
        public static List<string> Paragraph = new List<string>();
        public static string builderWireInputDirectory = @"C:\BuilderWire\Input\";
        public static string builderWireOutputDirectory = @"C:\BuilderWire\Output\";
        public static string wordsFilePath = @"C:\BuilderWire\Input\Words.txt";
        public static string articleFilePath = @"C:\BuilderWire\Input\Article.txt";
        public static string outputFilePath = @"C:\BuilderWire\Output\Output.txt";

        public static void Main(string[] args)
        {
            Console.WriteLine("This program counts every word from the Article.txt file and determine what sentence it is being used.");
            Console.WriteLine("Please make sure that the Article.txt and Words.txt are present in C:\\BilderWire\\Input folder before continuing.\n");
            Console.WriteLine("Note: For the Article.txt, please make sure that each paragraph are sparated with new line and each sentences are separated with period and spaces.");
            Console.WriteLine("Press any key to continue.\n\n");
            Console.ReadKey();

            CreateBuilderWireDirectory();
            string message = "";
            if (!AllInputFilesReady(out message))
            {
                Console.WriteLine(message);
                Console.ReadKey();
                return;
            }

            if (!ValidateParagraphInArticleFile(out message))
            {
                Console.WriteLine(message);
                Console.ReadKey();
                return;
            }

            foreach (var line in ProcessInputFiles())
            {
                Console.WriteLine(line);
            }
            
            Console.WriteLine("\n\nWord processing complete! Thank you.\nPress any key to exit the program.");
            Console.ReadKey();
            return;
        }

        public static List<string> ProcessInputFiles()
        {
            var newLines = new List<string>();
            try
            {
                int index = 0;
                int repeat = 1;
                var alphabetStr = "";
                var alphabets = GetAlphabets();
                var paragraphSplitWords = new List<string>();

                Words = File.ReadAllLines(wordsFilePath).ToList();
                Paragraph = File.ReadAllLines(articleFilePath).ToList();

                foreach (var line in Paragraph)
                {
                    paragraphSplitWords.AddRange(line.Split(' '));
                }
                var sentences = SentenceBuilder(paragraphSplitWords);

                foreach (string word in Words)
                {
                    for (int i = 0; i < repeat; i++)
                    {
                        alphabetStr = alphabetStr + alphabets[index];
                    }

                    string wordMatch = RemoveSpecialCharacters(word);
                    newLines.Add($"{alphabetStr}. {word} {{{WordPlacement(sentences, wordMatch.ToLower())}}}".ToLower());
                    File.WriteAllLines(outputFilePath, newLines);

                    if (index == (alphabets.Count() - 1))
                    {
                        index = 0;
                        repeat++;
                    }
                    else
                    {
                        index++;
                    }
                    alphabetStr = "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error: {ex}");
            }
            return newLines;
        }

        private static List<char> GetAlphabets()
        {
            var alphabets = new List<char>();
            for(int i = 65; i <= 90; i++)
            {
                alphabets.Add((char)i);
            }
            return alphabets;
        }

        private static List<char> GetSpecialCharacters()
        {
            var spclCharacters = new List<char>();
            for (int i = 33; i <= 47; i++)
            {
                spclCharacters.Add((char)i);
            }
            for (int i = 58; i <= 64; i++)
            {
                spclCharacters.Add((char)i);
            }
            for (int i = 91; i <= 96; i++)
            {
                spclCharacters.Add((char)i);
            }
            for (int i = 123; i <= 126; i++)
            {
                spclCharacters.Add((char)i);
            }
            return spclCharacters;
        }

        private static string RemoveSpecialCharacters(string word)
        {
            var spclCharacters = GetSpecialCharacters();
            foreach(char spclChar in spclCharacters)
            {
                if(word.Contains('.'))
                {
                    if (word.IndexOf('.') < word.Length - 1)
                    {
                        continue;
                    }
                }                
                word = word.Replace(spclChar, ' ').Trim();
            }
            return word;
        }

        private static string WordPlacement(List<string> sentences, string wordMatch)
        {
            int wordCount = 0;
            int sentenceNo = 1;
            string placement = "";
            foreach (string sentence in sentences)
            {
                var sentenceWords = new List<string>(sentence.ToLower().Split(' '));
                var count = sentenceWords.Where(word => RemoveSpecialCharacters(word) == wordMatch).Count();
                if (count > 0)
                {
                    for (var i = 1; i <= count; i++)
                    {
                        placement = placement + ',' + sentenceNo.ToString();
                    }
                    wordCount = wordCount + count;
                }
                sentenceNo++;
            }
            return $"{wordCount}:{placement.Substring(1, placement.Length - 1)}";
        }

        private static List<string> SentenceBuilder(List<string> paragraphSplitWords)
        {
            var sentences = new List<string>();
            var wordsWithDot = Words.Where(i => i.Contains('.')).ToList();
            string word = "";
            foreach(string paragraphWord in paragraphSplitWords)
            {
                if (paragraphWord.Contains('.'))
                {
                    if (!wordsWithDot.Contains(paragraphWord.ToLower()))
                    {
                        word = word + ' ' + paragraphWord;
                        sentences.Add(RemoveSpecialCharacters(word.Trim()));
                        word = "";
                        continue;
                    }
                    word = word + ' ' + paragraphWord;
                    continue;
                }
                word = word + ' ' + paragraphWord;
            }
            return sentences;
        }

        public static void CreateBuilderWireDirectory()
        {
            if (!Directory.Exists(builderWireInputDirectory))
            {
                Directory.CreateDirectory(builderWireInputDirectory);
            }

            if (!Directory.Exists(builderWireOutputDirectory))
            {
                Directory.CreateDirectory(builderWireOutputDirectory);
            }

            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
        }

        public static bool AllInputFilesReady(out string message)
        {
            if (!File.Exists(articleFilePath))
            {
                message = "The Article.txt file does not exist.\n" +
                    "Please make sure that the Article.txt is present in C:\\BilderWire\\Input folder before continuing.\n" +
                    "Press any key to exit the program.";                
                return false;
            }

            if (!File.Exists(wordsFilePath))
            {
                message = "The Words.txt file does not exist.\n" +
                    "Please make sure that the Words.txt is present in C:\\BilderWire\\Input folder before continuing.\n" +
                    "Press any key to exit the program.";
                return false;
            }

            if (File.ReadAllLines(articleFilePath).Count() == 0 || File.ReadAllText(articleFilePath) == "")
            {
                message = "Please make sure that the Article.txt has some text in it.\n" +
                    "Press any key to exit the program.";
                return false;
            }

            if (File.ReadAllLines(wordsFilePath).Count() == 0 || File.ReadAllText(wordsFilePath) == "")
            {
                message = "Please make sure that the Words.txt contains all the distict words in the Article.txt.\n" +
                    "Press any key to exit the program.";
                return false;
            }
            message = "";
            return true;
        }

        public static bool ValidateParagraphInArticleFile(out string message)
        {
            var paragraphSplitWords = new List<string>();
            Paragraph = File.ReadAllLines(articleFilePath).ToList();

            var index = 1;
            foreach (var line in Paragraph)
            {
                paragraphSplitWords.AddRange(line.Split(' '));

                if(!paragraphSplitWords[paragraphSplitWords.Count - 1].Contains('.'))
                {
                    message = $"The paragraph {index} does not end with period.\nPlease make sure every paragraph ends with a period.";
                    return false;
                }
                index++;
            }

            message = "";
            return true;
        }
    }
}
