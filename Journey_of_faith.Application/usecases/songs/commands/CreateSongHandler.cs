using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.entities.musics;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.songs.commands;


public class CreateSongHandler : IRequestHandler<CreateSongCommand, int>
{
    private readonly ISongRepository songRepository;
    public CreateSongHandler(ISongRepository songRepository)
    {
        this.songRepository = songRepository;
    }

    public async Task<int> Handle(CreateSongCommand command, CancellationToken cancellationToken)
    {
        if(!await songRepository.ExitsArtistAsync(command.ArtistId))
        {
           throw new NotFoundException("Nghệ sĩ không tồn tại"); 
        }

        if(!await songRepository.ExitsAlbumAsync(command.AlbumId))
        {
            throw new NotFoundException("Album không tồn tại.");
        }

        var song = new Song(
            command.Title,
            command.ArtistId, 
            command.AlbumId,
            command.Duration,
            command.AudioUrl, 
            command.CoverImageUrl, 
            command.Lyric,
            command.PlayCount,
            true
        );

        return await songRepository.CreateSongAsync(song, command.CategorySongId, cancellationToken);
    }
}