using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.questions.commands
{
    public class CreateQuizLevelHandler(IQuestionRepository questionRepository)
        : IRequestHandler<CreateQuizLevelCommand, bool> 
    {
        private readonly IQuestionRepository _questionRepository = questionRepository;
        public async Task<bool> Handle(CreateQuizLevelCommand command, CancellationToken token)
        {
            if(await _questionRepository.NameExistsAsync(command.Name, "QuizLevel"))
            {
                throw new ConfictException("Tên Cấp độ đã tồn tại không thể thực hiện tạo thêm");
            }
            var quiz = new QuizLevel(command.Name);
            await _questionRepository.CreateQuizLevel(quiz);
            Console.WriteLine("Tạo Cấp độ thành công.");
            return quiz.Id > 0;
        }
    }

    public class CreateQuestionTypeHandler(IQuestionRepository question) : IRequestHandler<CreateQuestionTypeCommand, bool> 
    {
        private readonly IQuestionRepository _question = question;
        public async Task<bool> Handle(CreateQuestionTypeCommand command, CancellationToken token)
        {

            if (await _question.NameExistsAsync(command.Name, "QuestionType"))
            {
                throw new ConfictException("Tên Kiêu câu hỏi đã tồn tại không thể thực hiện tạo thêm");
            }
            var questionType = new QuestionType(command.Name);
            await _question.CreateQuestionType(questionType);
            Console.WriteLine("Tạo Kiểu câu hỏi thành công");
            return questionType.Id > 0;
        }
    }


    public class CreateQuestionCategoryHandler(IQuestionRepository question) : IRequestHandler<CreateQuestionCategoryCommand, bool>
    {
        private readonly IQuestionRepository _questions = question;
        public async Task<bool> Handle(CreateQuestionCategoryCommand command, CancellationToken token)
        {
            if (await _questions.NameExistsAsync(command.Name, "QuestionCategory"))
            {
                throw new ConfictException("Tên Chủ đề đã tồn tại không thể thực hiện tạo thêm");
            }
            var questionType = new QuestionCategory(command.Name);
            await _questions.CreateQuestionCategory(questionType);
            Console.WriteLine("Tạo Kiểu câu hỏi thành công");
            return questionType.Id > 0;
        }
    }

    public class CreateQuestionHandler(IQuestionRepository question, IDbConnectionFactory connection) 
        : IRequestHandler<CreateQuestionCommand, bool>
    {
        private readonly IQuestionRepository _question = question;
        private readonly IDbConnectionFactory _conneciton = connection;

        public async Task<bool> Handle(CreateQuestionCommand command, CancellationToken token)
        {
            if(!await _question.CheckValidId(command.LevelId, "QuizLevel"))
            {
                throw new NotFoundException($"Level {command.LevelId} không tồn tại fen oie");
            }

            if (!await _question.CheckValidId(command.TypeId, "QuestionType"))
            {
                throw new NotFoundException($"Type {command.TypeId} không tồn tại fen oie");
            }

            if (!await _question.CheckValidId(command.CategoryId, "QuestionCategory"))
            {
                throw new NotFoundException($"Category {command.CategoryId} không tồn tại fen oie");
            }

            if(!command.Items.Any(e => e.IsCorrect))
            {
                throw new BadRequestException("Phải có ít nhất một đáp án đúng");
            }
            if(command.Items.Count < 3)
            {
                throw new BadRequestException("Phải có ít nhất từ 3 đáp án");
            }


            var question = Question.Create(levelId: command.LevelId, questionContent: command.QuestionContent, 
                typeId: command.TypeId, categoryId: command.CategoryId, imageUrl: command.ImageUrl);

                
            foreach(var item in command.Items)
            {
                question.AddAnswer(questionId: item.QuestionId, content: item.Content,isCorrect:item.IsCorrect,
                   imageUrl: item.ImageUrl, explance: item.Explanation);
            }
            await _question.CreateQuestionAsync(question);

            return true;
        }
    }

}
