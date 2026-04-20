using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Application.exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Application.common.interfaces;

namespace Journey_of_faith.Application.usecases.churchs.commands
{
    public class CreateDioceseHandler : IRequestHandler<CreateDioceseCommand, int>
    {
        private readonly IChurchRepository _churchRepository;
        private readonly ICurrentUserService _currentUserService;
        public CreateDioceseHandler(IChurchRepository churchRepository, ICurrentUserService currentUserService)
        {
            _churchRepository = churchRepository;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateDioceseCommand command, CancellationToken token)
        {
            if (await _churchRepository.UniqueNameDiocese(command.Name))
            {
                throw new UnprocessableEntityException("Tên Giáo xữ đã tòn tại");
            }
            if (!Guid.TryParse(_currentUserService.UserId, out var userId))
            {
                throw new UnauthorizationException("Người dùng không hợp lệ");
            }
            var diocese = new Diocese(command.Name, command.Website, command.Address ?? string.Empty, command.Thumbnail ?? string.Empty, userId);
            var dioceseId = await _churchRepository.CreateAsync(diocese);
            return dioceseId;
        }
    }
}
