using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.quiz
{
    public class QuestionCategory
    {
        public int Id { get; set; }
        public string Name { get; private set; } = string.Empty;

        private QuestionCategory() { }
        public QuestionCategory(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException($"{name} is required.");
            }

            Name = name;
        }
    }
}
