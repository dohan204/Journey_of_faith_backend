//using Journey_of_faith.Application.exceptions;
//using Journey_of_faith.Domain.interfaces;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Journey_of_faith.Application.usecases.questions.commands
//{
//    public class UpdateQuestionHandler : IRequestHandler<UpdateQuestionCommand, Unit> 
//    {
//        private readonly IQuestionRepository _repo;
//        public UpdateQuestionHandler(IQuestionRepository repo)
//        {
//            _repo = repo; 
//        }

//        public async Task<Unit> Handle(UpdateQuestionCommand command, CancellationToken token)
//        {
//            var id = _repo.GetDetailsQuestion();
//            if (id == null)
//            {
//                throw new NotFoundException("Không tìm thấy câu hỏi");
//            }


//        }
//    }
//}
