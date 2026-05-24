using FluentValidation;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.songs.commands;


public class CreateSongCommand : IRequest<int>
{
    public string Title {get; set;} = string.Empty;
    public int ArtistId {get; set;}
    public int AlbumId {get; set;}
    public int Duration {get; set;}
    public string AudioUrl {get; set;} = string.Empty;
    public string CoverImageUrl {get; set;} = string.Empty;
    public string Lyric {get; set;} = string.Empty;
    public int PlayCount {get; set;} 
    public bool IsActive {get; set;}

    public int CategorySongId {get; set;}
}

public class CreateSongValidator : AbstractValidator<CreateSongCommand>
{
    private readonly ISongRepository songRepository;
    public CreateSongValidator(ISongRepository songRepository)
    {
        this.songRepository = songRepository;

        RuleFor(e => e.Title)
            .NotNull().WithMessage("Title is not null")
            .NotEmpty().WithMessage("Title is not empty.");

        RuleFor(e => e.ArtistId)
            .GreaterThan(0).WithMessage("ArtistId must greater than 0");

        RuleFor(e => e.AlbumId)
            .GreaterThan(0).WithMessage("AlbumId is greater than 0");

        RuleFor(e => e.Duration)
            .NotNull().NotEmpty().WithMessage("Duration is not null and empty.")
            .GreaterThan(0).WithMessage("Duration is not less than 0");

        RuleFor(e => e.AudioUrl)
            .NotEmpty().WithMessage("Audio URL is required.");
        
    }
}

