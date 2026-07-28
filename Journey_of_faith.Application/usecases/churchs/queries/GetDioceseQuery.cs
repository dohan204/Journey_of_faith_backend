using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.churchs.queries;


public class GetDioceseQuery : IRequest<IEnumerable<Diocese>> {

}



public class GetDioceseHandler : IRequestHandler< GetDioceseQuery,IEnumerable<Diocese>> {
    private readonly IChurchRepository repository;
    public GetDioceseHandler(IChurchRepository repository) {
        this.repository = repository;
    }


    public async Task<IEnumerable<Diocese>> Handle(GetDioceseQuery query, CancellationToken token) {
        return await this.repository.GetAllDiocesesAsync();
    }
}