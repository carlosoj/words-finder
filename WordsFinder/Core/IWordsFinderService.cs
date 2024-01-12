namespace WordsFinder.Core
{
    public interface IWordsFinderService
    {
        public IEnumerable<string> Find(IEnumerable<string> matrix, IEnumerable<string> wordstream);

    }
}
