using FluentValidation;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.songs.commands;


public class CreateSongCategoryCommand : IRequest<int>
{
    public required string Name {get; set;}
}

public class CreateSongCategoryCommandValidator : AbstractValidator<CreateSongCategoryCommand>
{
    private readonly ISongRepository songRepository;
    public CreateSongCategoryCommandValidator(ISongRepository songRepository)
    {
        this.songRepository = songRepository;

        RuleFor(e => e.Name).MustAsync(async (name, cancellationToken) =>
        {
            bool exits = await songRepository.ExitsCategorySongAsync(name);
            return exits;
        }).WithMessage("SongCategory already taken.");
    }
}
