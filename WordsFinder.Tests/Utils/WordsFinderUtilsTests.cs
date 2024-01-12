using System.Threading.Tasks;
using WordsFinder.Utils;
using Xunit;

namespace WordsFinder.Tests.Utils
{
    public class WordsFinderUtilsTests
    {
        [Fact]
        public async Task IsValid_WhenMatrixIsValid_ReturnsTrue()
        {
            var matrix = await WordsFinderUtils.GetMatrixFromFileAsync(@"Assets\Sample.txt");
            var result = WordsFinderUtils.IsValid(matrix);
            Assert.True(result);
        }

        [Fact]
        public async Task IsValid_WhenMatrixRowsHaveDifferentLength_ReturnsFalse()
        {
            var matrix = await WordsFinderUtils.GetMatrixFromFileAsync(@"Assets\InvalidMatrix_DiffRowLengths.txt");
            var result = WordsFinderUtils.IsValid(matrix);
            Assert.False(result);
        }

        [Fact]
        public async Task IsValid_WhenMatrixIsNotSquare_ReturnsFalse()
        {
            var matrix = await WordsFinderUtils.GetMatrixFromFileAsync(@"Assets\InvalidMatrix_NotSquare.txt");
            var result = WordsFinderUtils.IsValid(matrix);
            Assert.False(result);
        }
    }
}
