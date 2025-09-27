using CareNest_Products.Application.Interfaces.CQRS.Commands;
using MediatR;

namespace CareNest_Products.Application.Interfaces.CQRS
{
    /// <summary>
    /// Use case dispatcher interface
    /// </summary>
    public interface IUseCaseDispatcher
    {
        /// <summary>
        /// Dispatch command with response
        /// </summary>
        Task<TResponse> DispatchAsync<TCommand, TResponse>(TCommand command) 
            where TCommand : ICommand<TResponse>;

        /// <summary>
        /// Dispatch command without response
        /// </summary>
        Task DispatchAsync<TCommand>(TCommand command) 
            where TCommand : ICommand;

        /// <summary>
        /// Dispatch query
        /// </summary>
        Task<TResponse> DispatchQueryAsync<TQuery, TResponse>(TQuery query) 
            where TQuery : IRequest<TResponse>;
    }
}
