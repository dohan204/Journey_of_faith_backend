namespace Journey_of_faith.Application.usecases.songs;

public interface ISongQueries
{
    Task<bool> SongCategoryExistsAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ArtistExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ArtistNameExistsAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> AlbumExistsAsync(int id, CancellationToken cancellationToken = default);
}
