using Domain.Entities;
using Repository.Interfaces;
using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Classes
{
    public class TestService :ITestService
    {
        ITestRepository _testRepository = null;

        public TestService(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }


        public void Add(TestViewModel model)
        {
            if (String.IsNullOrEmpty(model.Message)) return;

            _testRepository.Add(new Test() { Message = model.Message, Date = DateTime.Now });
        }

        public ICollection<Models.TestViewModel> GetAllMessages()
        {
            var entities = _testRepository.Get().ToList();
            ICollection<TestViewModel> models = new List<TestViewModel>();

            foreach (var entity in entities)
            {
                models.Add(new TestViewModel() { Date = entity.Date, Message = entity.Message });
            }
            return models;
        }

        public void Dispose()
        {
            _testRepository.Dispose();
        }
    }
}
