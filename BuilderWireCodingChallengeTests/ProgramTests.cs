using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace BuilderWireCodingChallenge.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        string articleFilePath = @"C:\BuilderWire\Input\ArticleTest.txt";
        string wordsFilePath = @"C:\BuilderWire\Input\WordsTest.txt";
        string outputFilePath = @"C:\BuilderWire\Output\OutputTest.txt";

        [TestMethod()]
        public void ProcessInputFiles_WhenAllFilesProvided_OutputFileCreated()
        {
            CreateBuilderWireDirectory();
            CreateArticleTestFile();
            CreateWordsTestFile();

            Program.outputFilePath = outputFilePath;
            Program.ProcessInputFiles();

            Assert.IsTrue(File.Exists(outputFilePath));

            DeleteAllTestFiles();
        }

        [TestMethod()]
        public void AllInputFilesReady_WhenFilesNotProvided_ReturnFalse()
        {
            Program.wordsFilePath = wordsFilePath;
            Program.articleFilePath = articleFilePath;
            CreateBuilderWireDirectory();
            DeleteAllTestFiles();

            var result = Program.AllInputFilesReady(out string message);

            Assert.IsFalse(result);
            Assert.IsTrue(message != "");
        }

        [TestMethod()]
        public void AllInputFilesReady_WhenArticleFileDoesNotHaveText_ReturnFalse()
        {
            CreateBuilderWireDirectory();
            Program.articleFilePath = articleFilePath;
            if (File.Exists(articleFilePath))
            {
                File.Delete(articleFilePath);
            }
            using (FileStream fs = File.Create(articleFilePath))
            {
                var sentence = new UTF8Encoding(true).GetBytes("");
                fs.Write(sentence, 0, sentence.Length);
            }
            CreateWordsTestFile();

            Program.outputFilePath = outputFilePath;
            var result = Program.AllInputFilesReady(out string message);

            Assert.IsFalse(result);
            Assert.IsTrue(message != "");

            DeleteAllTestFiles();
        }

        [TestMethod()]
        public void AllInputFilesReady_WhenWordFileDoesNotHaveText_ReturnFalse()
        {
            CreateBuilderWireDirectory();
            Program.wordsFilePath = wordsFilePath;
            if (File.Exists(wordsFilePath))
            {
                File.Delete(wordsFilePath);
            }
            using (FileStream fs = File.Create(wordsFilePath))
            {
                var sentence = new UTF8Encoding(true).GetBytes("");
                fs.Write(sentence, 0, sentence.Length);
            }
            CreateArticleTestFile();

            Program.outputFilePath = outputFilePath;
            var result = Program.AllInputFilesReady(out string message);

            Assert.IsFalse(result);
            Assert.IsTrue(message != "");

            DeleteAllTestFiles();
        }

        [TestMethod()]
        public void ValidateParagraphInArticleFile_WhenParagraphDoesNotEndWithPeriod_ReturnFalse()
        {
            CreateBuilderWireDirectory();
            Program.articleFilePath = articleFilePath;
            if (File.Exists(articleFilePath))
            {
                File.Delete(articleFilePath);
            }
            using (FileStream fs = File.Create(articleFilePath))
            {
                var sentence = new UTF8Encoding(true).GetBytes("The quick brown fox jumps over the lazy dog\nLorem ipsum dolor sit amet");
                fs.Write(sentence, 0, sentence.Length);
            }

            var result = Program.ValidateParagraphInArticleFile(out string message);

            Assert.IsFalse(result);
            Assert.IsTrue(message != "");

            DeleteAllTestFiles();
        }

        private void CreateBuilderWireDirectory()
        {
            Program.outputFilePath = outputFilePath;
            Program.CreateBuilderWireDirectory();
        }

        private void CreateArticleTestFile()
        {
            Program.articleFilePath = articleFilePath;
            if (File.Exists(articleFilePath))
            {
                File.Delete(articleFilePath);
            }

            using (FileStream fs = File.Create(articleFilePath))
            {
                var sentence = new UTF8Encoding(true).GetBytes("This is a sentence number 1. This is a sentence number 2.");
                fs.Write(sentence, 0, sentence.Length);
            }
        }

        private void CreateWordsTestFile()
        {
            Program.wordsFilePath = wordsFilePath;
            if (File.Exists(wordsFilePath))
            {
                File.Delete(wordsFilePath);
            }

            using (FileStream fs = File.Create(wordsFilePath))
            {
                var sentence = new UTF8Encoding(true).GetBytes("This\nis\na\nsentence\nnumber\n1\n2");
                fs.Write(sentence, 0, sentence.Length);
            }
        }

        private void DeleteAllTestFiles()
        {
            if (File.Exists(articleFilePath))
            {
                File.Delete(articleFilePath);
            }
            if (File.Exists(wordsFilePath))
            {
                File.Delete(wordsFilePath);
            }
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
        }
    }
}