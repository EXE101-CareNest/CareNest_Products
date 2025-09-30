using MediatR;
using CareNest_Products.Application.Interfaces.CQRS;
using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Application.Interfaces.CQRS.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace CareNest_Products.Application.UseCases
{
    /// <summary>
    /// Use case dispatcher implementation using MediatR
    /// </summary>
    public class UseCaseDispatcher : IUseCaseDispatcher
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;

        public UseCaseDispatcher(IMediator mediator, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> DispatchAsync<TCommand, TResponse>(TCommand command) 
            where TCommand : ICommand<TResponse>
        {
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();
            return await handler.HandleAsync(command);
        }

        public async Task DispatchAsync<TCommand>(TCommand command) 
            where TCommand : ICommand
        {
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            await handler.HandleAsync(command);
        }

        public async Task<TResponse> DispatchQueryAsync<TQuery, TResponse>(TQuery query) 
            where TQuery : IRequest<TResponse>
        {
            return await _mediator.Send(query);
        }
    }
}
