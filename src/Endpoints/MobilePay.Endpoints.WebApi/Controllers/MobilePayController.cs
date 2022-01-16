using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobilePay.Core.ApplicationService.Commands.TransactionAggregate.CreateTransactionRequest;
using MobilePay.Core.Domain.Commons;
using MobilePay.Endpoints.WebApi.Models;
using MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetMerchantFee;
using MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetTransactionDetail;
using MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetTransactions;

namespace MobilePay.Endpoints.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MobilePayController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public MobilePayController(IMediator mediator, IConfiguration configuration, IMapper mapper)
        {
            _mediator = mediator;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateTransactionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(AddTransactionModel model) =>
            CreatedAtAction(nameof(Get), new { id = model.Id },
                await _mediator.Send(_mapper.Map<CreateTransactionRequestCommand>(model)));

        [HttpPost("list")]
        [ProducesResponseType(typeof(CreateTransactionRequestCommand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<CreateTransactionRequestCommand> Post(CreateTransactionRequestCommand models) =>
            await _mediator.Send(models);

        [HttpGet("calculatefee")]
        [ProducesResponseType(typeof(GetTransactionDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<GetMerchantFeeDto> GetMerchantFee([FromQuery] MerchantModel model) =>
            await _mediator.Send(_mapper.Map<GetMerchantFeeQuery>(model));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetTransactionDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTransactionDetailDto>> Get(Guid id)
        {
            var result = await _mediator.Send(new GetTransactionDetailQuery(id));

            if (result == null) return NotFound();

            return result;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<GetTransactionsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<List<GetTransactionsDto>> GetAll([FromQuery] GetTransactionsModel model) =>
            await _mediator.Send(new GetTransactionsQuery
            {
                MerchantName = model.MerchantName,
                Paging = new() { PageIndex = model.PageIndex, PageSize = model.PageSize }
            });
    }
}
