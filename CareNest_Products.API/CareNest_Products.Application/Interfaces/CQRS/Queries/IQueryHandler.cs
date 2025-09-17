namespace CareNest_Products.Application.Interfaces.CQRS.Queries
{
    /// <summary>
    /// Query handler interface
    /// </summary>
    /// <typeparam name="TQuery">Query type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        Task<TResponse> HandleAsync(TQuery query);
    }
}
