namespace MVVM.MVVM.System.Base.Command.Base
{
/// <summary>
/// Represents a command that can be executed with a parameter of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the parameter passed to the command.</typeparam>
public interface ICommand<in T>
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