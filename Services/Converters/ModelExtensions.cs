using System;
using Data.Entities;
using Services.Models;

namespace Services.Converters
{
    public static class ModelExtensions
    {
        public static Group ToEntity (this GroupViewModel viewModel)
        {
            return new Group()
            {
                Id = viewModel.Id ?? 0,
                CreatedAt = DateTime.Now,
                Description = viewModel.Description,
                Title = viewModel.Title
            };
        }

        public static GroupViewModel ToViewModel(this Group entity)
        {
            return new GroupViewModel()
            {
                Id = entity.Id,
                Description = entity.Description,
                Title = entity.Title
            };
        }
    }
}