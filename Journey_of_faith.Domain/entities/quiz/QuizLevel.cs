using Journey_of_faith.Domain.exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.quiz
{
    public class QuizLevel
    {
        public int Id { get; set; }
        public string Name { get; private set; } = string.Empty;
        public QuizLevel(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new DomainException($"{name} is required");
            }
            Name = name;
        }
    }
}
