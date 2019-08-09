namespace Expertia.Estructura.Utils.Behavior
{
    public interface IFileIO
    {
        string FullName { get; }
        void WriteContent(string content);
        void WriteContent(string[] contents);
        string ReadContent();
    }
}
