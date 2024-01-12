namespace WordsFinder.Utils
{
    public class WordsFinderUtils
    {
        public static async Task<IEnumerable<string>> GetMatrixFromFileAsync(string pathToFile)
        {
            var matrix = new List<string>();
            var range = File.ReadLinesAsync(pathToFile);
            await foreach (var line in range)
            {
                matrix.Add(line.Trim());
            }

            return matrix;
        }

        public static bool IsValid(IEnumerable<string> matrix)
        {
            const int MAX_SIZE = 64;
            if (!matrix.Any())
                return false;

            var firstRow = matrix.First();
            int columnsCount = firstRow.Length;

            if(firstRow.Length > MAX_SIZE || columnsCount > MAX_SIZE)
                return false;

            if (columnsCount != matrix.Count())
                return false;

            foreach (var line in matrix)
            {
                if (line.Length != columnsCount)
                    return false;
            }

            return true;
        }
    }
}
