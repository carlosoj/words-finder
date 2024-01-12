using WordsFinder.Core;

namespace WordsFinder.Services
{
    public class WordsFinderService : IWordsFinderService
    {
        public IEnumerable<string> Find(IEnumerable<string> matrix, IEnumerable<string> wordstream)
        {
            ArgumentNullException.ThrowIfNull(matrix);
            ArgumentNullException.ThrowIfNull(wordstream);

            if (wordstream == null)
                throw new ArgumentNullException(nameof(wordstream));

            var foundWords = new Dictionary<string, int>();
            var wordKeys = new Dictionary<char, IList<string>>();
            var wordsCount = new Dictionary<string, int>();

            //create a dictionary of words
            foreach (var word in wordstream)
            {
                if (string.IsNullOrWhiteSpace(word) || wordsCount.ContainsKey(word))//skips duplicated or invalid words from the steream
                    continue;
                
                wordsCount.Add(word, 0);

                var firstLetter = word[0];
                if (!wordKeys.TryGetValue(firstLetter, out IList<string>? value))//grouping words that start with the same character
                {
                    value = new List<string>();
                    wordKeys[firstLetter] = value;
                }

                value.Add(word);
            }

            //searching
            int rowIndex = 0;
            foreach (var row in matrix)//starts iterating the matrix starting by the rows
            {
                for (var columnIndex = 0; columnIndex < row.Length; columnIndex++)
                {
                    var currentLetter = row[columnIndex];
                    if (wordKeys.ContainsKey(currentLetter))//checks if any of words to search start with this character
                    {
                        foreach (var word in wordKeys[currentLetter])//iterates through the words that start with the current character
                        {
                            if (ExistsInRow(word, row, columnIndex))//checks if the word exists in the row
                            {
                                wordsCount[word] = wordsCount[word] + 1;
                            }

                            if (ExistsInColumn(word, matrix, columnIndex, rowIndex))//checks if the word exists in the column
                            {
                                wordsCount[word] = wordsCount[word] + 1;
                            }
                        }
                    }
                }
                rowIndex++;//moves to the next row
            }

            return wordsCount
                .OrderByDescending(k => k.Value)
                .ThenBy(k => k.Key)
                .Where(k => k.Value > 0)
                .Take(10)
                .Select(k => k.Key);//gets the top 10 words by descending order
        }

        private static bool ExistsInRow(string word, string row, int startingIndex)
        {
            var availableCharacters = (row.Length - startingIndex);
            if (availableCharacters < word.Length) //check that word is not larger that the available columns
                return false;

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] != row[startingIndex+i])
                    return false;
            }

            return true;
        }

        private static bool ExistsInColumn(string word, IEnumerable<string> matrix, int columnIndex, int startingRowIndex)
        {
            int matrixRowCount = matrix.Count();
            var availableCharacters = (matrixRowCount - startingRowIndex);
            if (word.Length > availableCharacters)
                return false;

            for (int i =0; i<word.Length; i++)
            {
                var matrixVal = matrix.ElementAt(startingRowIndex+i)[columnIndex];
                if (matrixVal != word[i])
                    return false;
            }

            return true;
        }
    }
}
