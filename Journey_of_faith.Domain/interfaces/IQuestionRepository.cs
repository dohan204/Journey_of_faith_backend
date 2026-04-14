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
        Task<QuestionView?> GetDetailsQuestion(int id);
        Task<int> GetCountQuestion();
        //Task<Question?> GetDetailsQuestion(int id);
        Task<bool> CheckUniqueName(string name);
        Task<bool> UpdateQuestion(Question question);
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


    public class QuestionView
    {
        public int Id { get; set; }
        public int LevelId { get; set; }
        public string QuestionContent { get; set; } = string.Empty;
        public int TypeId { get; set; }
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public List<AnswerView> Answers { get; set; } = new();
    }
    public class AnswerView
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
    }
}
