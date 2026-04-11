using System;
using System.Collections.Generic;
using System.Text;
using Journey_of_faith.Domain.exceptions;

namespace Journey_of_faith.Domain.entities.quiz
{
    public class QuestionType
    {
        public int Id { get; set; }
        public string Name { get; private set; } = string.Empty;
        private QuestionType() { }
        public QuestionType(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new DomainException($"{name}is not required");
            }
            Name = name;
        }
    }
}
