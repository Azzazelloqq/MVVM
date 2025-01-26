namespace MVVM.MVVM.System.Base.Command.Base
{
/// <summary>
/// Represents a command that can be executed and disposed.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Determines whether the command can be executed.
    /// </summary>
    /// <returns><c>true</c> if the command can execute; otherwise, <c>false</c>.</returns>
    public bool CanExecute();

    /// <summary>
    /// Releases all resources used by the object.
    /// </summary>
    public void Dispose();
}
}