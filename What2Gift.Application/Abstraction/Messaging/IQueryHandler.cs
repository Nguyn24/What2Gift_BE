using MediatR;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Abstraction.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;