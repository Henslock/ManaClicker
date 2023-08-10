public interface IConsoleCommand
{
    string commandWord { get; }
    string result { get; }
    bool Processs(string[] args);
    string GetProcessResult();
}