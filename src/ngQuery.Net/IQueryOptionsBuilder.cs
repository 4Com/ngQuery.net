namespace ngQuery.Net
{
    public interface IQueryOptionsBuilder
    {
        string Build<T>() where T : class;
    }
}
