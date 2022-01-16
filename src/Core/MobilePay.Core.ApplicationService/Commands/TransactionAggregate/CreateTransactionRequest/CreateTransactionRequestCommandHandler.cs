using AutoMapper;
using MediatR;
using MobilePay.Core.Domain.Commons;
using MobilePay.Core.Domain.TransactionAggregate.Contracts;
using static MobilePay.Core.Domain.TransactionAggregate.TransactionCommand;

namespace MobilePay.Core.ApplicationService.Commands.TransactionAggregate.CreateTransactionRequest;

public class CreateTransactionRequestCommandHandler : IRequestHandler<CreateTransactionRequestCommand, CreateTransactionRequestCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionRequestCommandHandler(ITransactionRepository transactionRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateTransactionRequestCommand> Handle(CreateTransactionRequestCommand request, CancellationToken cancellationToken)
    {
        var dataModel = _mapper.Map<CreateTransactionCommand>(request);
        await _transactionRepository.CreateListAsync(dataModel, cancellationToken);

        await _unitOfWork.SaveAsync(cancellationToken);

        return request;
    }
}
