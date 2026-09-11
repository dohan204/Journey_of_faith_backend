using Journey_of_faith.Domain.entities.quiz;
using MediatR;

namespace Journey_of_faith.Application.usecases.quizs.queries;


public class GetAllTopicQuery : IRequest<IEnumerable<Topic>>
{
    
}