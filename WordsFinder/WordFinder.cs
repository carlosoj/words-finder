using WordsFinder.Core;
using WordsFinder.Services;
using WordsFinder.Utils;

namespace WordsFinder
{
    public class WordFinder
    {
        private readonly IWordsFinderService _wordsFinderService;
        private readonly IEnumerable<string> _matrix;

        public WordFinder(IEnumerable<string> matrix)
        {
            _wordsFinderService = new WordsFinderService();
        }

        public WordFinder(IWordsFinderService wordsFinderService, IEnumerable<string> matrix)
        {
            _wordsFinderService = wordsFinderService;
            _matrix = matrix;
        }

        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            if (_matrix is null)
                throw new ArgumentNullException("The matrix has not been set");

            if(!WordsFinderUtils.IsValid(_matrix))
            {
                throw new InvalidOperationException();
            }

            return _wordsFinderService.Find(_matrix, wordstream);
        }
    }
}
