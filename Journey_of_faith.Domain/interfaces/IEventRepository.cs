using Journey_of_faith.Domain.entities.events;

namespace Journey_of_faith.Domain.interfaces
{
    public class EventListFilter
    {
        public string? Keyword { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? StartFrom { get; set; }
        public DateTime? StartTo { get; set; }
        public bool OnlyUpcoming { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class EventCategoryView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Events { get; set; } = string.Empty;

        public List<Event> EventsList { get; set; }
    }

    public class EventImageView
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class EventListItemView
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsFollowed { get; set; }
        public int FollowerCount { get; set; }
    }

    public class EventDetailsView : EventListItemView
    {
        public int ParticipantCount { get; set; }
        public List<EventCategoryView> Categories { get; set; } = [];
        public List<EventImageView> Images { get; set; } = [];
    }

    public class EventPagedResult
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<EventListItemView> Items { get; set; } = [];
    }

    public class CreateEventPayload
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ImageUrl { get; set; }
        public Guid CreatorUserId { get; set; }
        public List<int> CategoryIds { get; set; } = [];
        public List<string> ImageUrls { get; set; } = [];
    }

    public class UpdateEventPayload
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ImageUrl { get; set; }
        public Guid LastModifierUserId { get; set; }
        public List<int>? CategoryIds { get; set; }
        public List<string>? ImageUrls { get; set; }
    }

    public interface IEventRepository
    {
        Task<bool> EventExistsAsync(int eventId);
        Task<bool> CategoryExistsAsync(int categoryId);
        Task<bool> CategoryNameExistsAsync(string categoryName);

        Task<int> CreateCategoryAsync(string categoryName);
        Task<IEnumerable<EventCategoryView>> GetCategoriesAsync();

        Task<int> CreateEventAsync(CreateEventPayload payload);
        Task<bool> UpdateEventAsync(UpdateEventPayload payload);
        Task<bool> DeleteEventAsync(int eventId, Guid deleterUserId);

        Task<EventDetailsView?> GetEventDetailsAsync(int eventId, Guid? userId);
        Task<EventPagedResult> GetEventsAsync(EventListFilter filter, Guid? userId);

        Task<bool> IsFollowingEventAsync(Guid userId, int eventId);
        Task<bool> FollowEventAsync(Guid userId, int eventId);
        Task<bool> UnfollowEventAsync(Guid userId, int eventId);
        Task<IEnumerable<EventListItemView>> GetFollowedEventsAsync(Guid userId, DateTime? startFrom, DateTime? startTo);
    }
}
