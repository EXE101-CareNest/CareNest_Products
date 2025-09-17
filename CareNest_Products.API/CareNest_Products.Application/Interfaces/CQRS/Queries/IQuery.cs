namespace CareNest_Products.Application.Interfaces.CQRS.Queries
{
    /// <summary>
    /// Query interface
    /// </summary>
    /// <typeparam name="TResponse">Response type</typeparam>
    public interface IQuery<out TResponse>
    {
    }
}
