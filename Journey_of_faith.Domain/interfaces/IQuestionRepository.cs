using Journey_of_faith.Domain.entities.quiz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Journey_of_faith.Domain.interfaces
{
    public interface IQuestionRepository 
    {
        Task<bool> NameExistsAsync(string name, string table);
        Task<bool> CreateQuizLevel(QuizLevel quiz);
        Task<bool> CreateQuestionType(QuestionType questionType);
        Task<bool> CreateQuestionCategory(QuestionCategory questionCategory);


        Task<bool> CreateQuestionAsync(Question question);
        //Task<Question?> GetDetailsQuestion(int id);
        Task<int> GetCountQuestion();
        //Task<Question?> GetDetailsQuestion(int id);
        Task<bool> CheckUniqueName(string name);

        Task<bool> DeleteQuestion(int Id);

        Task<IEnumerable<QuizLevel>> GetLevelsAsync();
        Task<QuizLevel?> GetDetailQuizLevel(int Id);
        Task<int> GetCountQuestionByLevel(string name);

        Task<IEnumerable<QuestionType>> GetAllTypeQuestion();
        Task<QuestionType?> GetDetailsQuestionType(int Id);
        Task<IEnumerable<QuestionCategory>> GetAllCategoryQuestion();
        Task<QuestionCategory?> GetDetailsQuestionCategory(int Id);

        Task<bool> CheckValidId(int id, string table);
        

    }
}
