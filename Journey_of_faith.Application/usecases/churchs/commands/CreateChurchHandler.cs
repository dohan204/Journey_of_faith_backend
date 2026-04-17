using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.churchs.commands
{
    public class CreateChurchHandler : IRequestHandler<CreateChurchCommand, int>
    {
        private readonly IChurchRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        public CreateChurchHandler(IChurchRepository repo, ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateChurchCommand command, CancellationToken token)
        {
            if (!Guid.TryParse(_currentUserService.UserId, out var userId))
            {
                throw new UnauthorizationException("Người dùng không hợp lệ");
            }
            if (!await _repo.GetDioceseExistsAsync(command.DioceseId)) {
                throw new NotFoundException("Không có giáo phận mà nhà nhờ đăng ký.");
            }
            var church = new Church(command.Name, command.Thumbnail, command.Website, command.Address, command.DioceseId, 
                                    command.Latitude, command.Longitude, userId, userId);
            await _repo.CreateAsync(church);
            return church.Id;
        }
    }
}
