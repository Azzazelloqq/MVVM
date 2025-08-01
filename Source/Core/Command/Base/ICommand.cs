using System;

namespace Azzazelloqq.MVVM.Core
{
/// <summary>
/// Represents a command that can be executed and disposed.
/// </summary>
public interface ICommand : IDisposable
{
    /// <summary>
    /// Determines whether the command can be executed.
    /// </summary>
    /// <returns><c>true</c> if the command can execute; otherwise, <c>false</c>.</returns>
    public bool CanExecute();
}
}