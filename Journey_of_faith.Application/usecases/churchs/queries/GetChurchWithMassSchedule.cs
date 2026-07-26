using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.churchs.queries
{
    public class GetChurchWithMassScheduleQueries : IRequest<IEnumerable<Church>>, ICacheableQuery
    {
        public string Orderby { get; set; } = string.Empty;

        public string CacheKey => $"churches-{(!string.IsNullOrEmpty(Orderby) ? Orderby.ToLower().Trim() : "default")}";
        public bool BypassCache => false;
    }


    public class GetChurchWithMassScheduleHandler : IRequestHandler<GetChurchWithMassScheduleQueries, IEnumerable<Church>>
    {
        private readonly IChurchRepository churchRepository;
        public GetChurchWithMassScheduleHandler(IChurchRepository churchRepository)
        {
            this.churchRepository = churchRepository;
        }

        public async Task<IEnumerable<Church>> Handle(GetChurchWithMassScheduleQueries queries, CancellationToken cancellationToken)
        {
            return await churchRepository.GetAllAsync(queries.Orderby, cancellationToken);
        }
    }
}
