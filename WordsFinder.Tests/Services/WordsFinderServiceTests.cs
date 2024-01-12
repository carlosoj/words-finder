using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsFinder.Core;
using WordsFinder.Services;
using WordsFinder.Utils;
using Xunit;

namespace WordsFinder.Tests.Services
{
    public class WordsFinderServiceTests
    {
        private readonly IWordsFinderService wordsFinderService;

        public WordsFinderServiceTests()
        {
            wordsFinderService = new WordsFinderService();
        }

        [Fact]
        public async Task Find_WordExistsInRow_ReturnsTrue()
        {
            var matrix = await WordsFinderUtils.GetMatrixFromFileAsync(@"Assets\Sample.txt");
            var word = "chill";
            var wordStrem = new List<string>() { word };
            var result = wordsFinderService.Find(matrix, wordStrem);

            Assert.True(result.Any());
            Assert.Contains(word, result);
        }

        [Fact]
        public async Task Find_WordExistsInColumn_ReturnsWOrd()
        {
            var matrix = await WordsFinderUtils.GetMatrixFromFileAsync(@"Assets\Sample.txt");
            var word = "wind";
            var wordStrem = new List<string>() { word };
            var result = wordsFinderService.Find(matrix, wordStrem);

            Assert.True(result.Any());
            Assert.Contains(word, result);
        }

        [Fact]
        public async Task Find_MultipleWordsExistsInColumn_ReturnsExistingWords()
        {
            var matrix = await WordsFinderUtils.GetMatrixFromFileAsync(@"Assets\Sample.txt");
            var words = new List<string>() { "wind", "cold" };
            var result = wordsFinderService.Find(matrix, words);
            Assert.Equal(2, result.Count());
            Assert.All(result, (word) => words.Contains(word));
        }

        [Fact]
        public async Task Find_MultipleWordsInMatrix_ReturnsAllWords()
        {
            var matrix = await WordsFinderUtils.GetMatrixFromFileAsync(@"Assets\Sample.txt");
            var words = new List<string>() { "cold", "wind", "snow", "chill" };
            var result = wordsFinderService.Find(matrix, words);

            Assert.Equal(3, result.Count());
            Assert.All(result, (word) => words.Contains(word));
        }

        [Fact]
        public async Task Find_WordsThatDoNotExists_ReturnEmptyList()
        {
            var matrix = await WordsFinderUtils.GetMatrixFromFileAsync(@"Assets\Sample.txt");
            var words = new List<string>() { "mac", "medicine", "nulaxy", "brand" };
            var result = wordsFinderService.Find(matrix, words);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Find_LargeMatrix_FindsWords()
        {
            var matrix = await WordsFinderUtils.GetMatrixFromFileAsync(@"Assets\64x64.txt");
            var words = new List<string>()
            { "cartoon", "apple", "experiment", "dont", "blacklist",
                "max", "text", "orange", "pencil",  "laptop", "bird", "battery", "hello" };
            var result = wordsFinderService.Find(matrix, words);

            Assert.Equal(10, result.Count());
            Assert.All(result, (word) => words.Contains(word));
            Assert.Equal("text", result.ElementAt(0));
            Assert.Equal("max", result.ElementAt(1));
            Assert.Equal("pencil", result.ElementAt(2));
            Assert.Equal("laptop", result.ElementAt(3));
            Assert.Equal("orange", result.ElementAt(4));
            Assert.Equal("bird", result.ElementAt(5));
            Assert.Equal("experiment", result.ElementAt(6));
            Assert.Equal("hello", result.ElementAt(7));
            Assert.Equal("cartoon", result.ElementAt(8));
            Assert.Equal("apple", result.ElementAt(9));
        }

        [Fact]
        public async Task Find_WordsStreamContainsNull_IgnoreNulls()
        {
            var matrix = await WordsFinderUtils.GetMatrixFromFileAsync(@"Assets\Sample.txt");
            var words = new List<string>() { "mac", "medicine", "nulaxy", "brand", null, null };
            var result = wordsFinderService.Find(matrix, words);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Find_WordsStreamIsNull_IgnoreNulls()
        {
            Assert.Throws<ArgumentNullException>(() => wordsFinderService.Find(new List<string>(), null));
            Assert.Throws<ArgumentNullException>(() => wordsFinderService.Find(null, new List<string>() { }));
            Assert.Throws<ArgumentNullException>(() => wordsFinderService.Find(null, null));
        }
    }
}
