using MediatR;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Abstraction.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;