using FluentValidation;
using Journey_of_faith.Application.usecases.songs;
using MediatR;

namespace Journey_of_faith.Application.usecases.songs.commands;


public class CreateSongCategoryCommand : IRequest<int>
{
    public required string Name {get; set;}
}

public class CreateSongCategoryCommandValidator : AbstractValidator<CreateSongCategoryCommand>
{
    private readonly ISongQueries songQueries;
    public CreateSongCategoryCommandValidator(ISongQueries songQueries)
    {
        this.songQueries = songQueries;

        RuleFor(e => e.Name).MustAsync(async (name, cancellationToken) =>
        {
            bool exists = await songQueries.SongCategoryExistsAsync(name, cancellationToken);
            return !exists;
        }).WithMessage("SongCategory already taken.");
    }
}
