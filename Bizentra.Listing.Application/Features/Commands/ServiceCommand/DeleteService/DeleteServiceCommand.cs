using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Domain.Entities;
using MediatR;
using NLog;

namespace Bizentra.Listing.Application.Features.Commands.ServiceCommand.DeleteService
{
    public class DeleteServiceCommand
    {
        public class Command : IRequest<Result>
        {
            public Guid ServiceId { get; set; }
        }

        public class Result
        {
            public bool IsSuccessful { get; set; }
            public string Message { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IBaseRepository<Service> _serviceRepository;
            private readonly IUnitofWork _unitofWork;
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

            public Handler(IBaseRepository<Service> serviceRepository, IUnitofWork unitofWork)
            {
                _serviceRepository = serviceRepository;
                _unitofWork = unitofWork;
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var result = new Result
                {
                    IsSuccessful = false,
                    Message = ""
                };

                if (command.ServiceId == Guid.Empty)
                {
                    Logger.Error("Service Id cannot be empty");
                    return result;
                }

                var serviceToDelete = await _serviceRepository.GetByIdAsync(command.ServiceId);
                if (serviceToDelete == null)
                {
                    Logger.Error($"Service with Id : {command.ServiceId} was not found");
                    return result;
                }

                serviceToDelete.IsDeleted = true;

                if (await _unitofWork.SubmitChangesAsync())
                {
                    result.IsSuccessful = true;
                    result.Message = "Service deleted successfully";
                }

                return result;
            }
        }
    }
}
