using MediatR;
using CareNest_Products.Application.Interfaces.CQRS;
using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Application.Interfaces.CQRS.Queries;

namespace CareNest_Products.Application.UseCases
{
    /// <summary>
    /// Use case dispatcher implementation using MediatR
    /// </summary>
    public class UseCaseDispatcher : IUseCaseDispatcher
    {
        private readonly IMediator _mediator;

        public UseCaseDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TResponse> DispatchAsync<TCommand, TResponse>(TCommand command) 
            where TCommand : ICommand<TResponse>
        {
            return (TResponse)await _mediator.Send(command);
        }

        public async Task DispatchAsync<TCommand>(TCommand command) 
            where TCommand : ICommand
        {
            await _mediator.Send(command);
        }

        public async Task<TResponse> DispatchQueryAsync<TQuery, TResponse>(TQuery query) 
            where TQuery : IRequest<TResponse>
        {
            return await _mediator.Send(query);
        }
    }
}
