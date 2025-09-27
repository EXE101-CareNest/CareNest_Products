namespace CareNest_Products.Application.Interfaces.CQRS.Commands
{
    /// <summary>
    /// Command interface
    /// </summary>
    /// <typeparam name="TResponse">Response type</typeparam>
    public interface ICommand<out TResponse>
    {
    }

    /// <summary>
    /// Command interface without response
    /// </summary>
    public interface ICommand
    {
    }
}
