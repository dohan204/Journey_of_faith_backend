using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.entities.musics;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.songs.commands;


public class CreateSongHandler : IRequestHandler<CreateSongCommand, int>
{
    private readonly ISongRepository songRepository;
    private readonly ISongQueries songQueries;
    public CreateSongHandler(ISongRepository songRepository, ISongQueries songQueries)
    {
        this.songRepository = songRepository;
        this.songQueries = songQueries;
    }

    public async Task<int> Handle(CreateSongCommand command, CancellationToken cancellationToken)
    {
        if(!await songQueries.ArtistExistsAsync(command.ArtistId, cancellationToken))
        {
           throw new NotFoundException("Nghệ sĩ không tồn tại"); 
        }

        if(!await songQueries.AlbumExistsAsync(command.AlbumId, cancellationToken))
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
