using MyWebApplication.Domain.Enums;
using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Entities.Flyweight;

namespace MyWebApplication.Domain.Entities
{
    public class Course  : ICourse
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Language Language { get; set; } = null!; //flyweight

        public LanguageLevel Level { get; set; }

        public decimal Price { get; set; }

        public CourseType Type { get; set; }


        private List<ICourse> _items = new List<ICourse>(); //composite pattern

        public void Add(ICourse component)
        {
            _items.Add(component);
        }

        public void Remove(ICourse component)
        {
            _items.Remove(component);
        }

        public virtual string GetDescription() //decorator
        {
            return Title;
        }

        // public void Display()
        //   {
        //   Console.WriteLine($"Course: {Title}");

        //  foreach (var item in _items)
        // {
        //     item.Display();
        //  }
        //  }           

        public Course Clone()
        {
            return new Course
            {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description,
                Language = this.Language,
                Level = this.Level,
                Price = this.Price,
                Type = this.Type
            };
        }
    }
}