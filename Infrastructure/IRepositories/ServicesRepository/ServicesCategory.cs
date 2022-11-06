using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IRepositories.ServicesRepository
{
    public class ServicesCategory : IServicesRepository<Category>
    {
        private readonly BookyDbContext _dbContext;

        public ServicesCategory(BookyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool Delete(Guid Id)
        {
            try
            {
                var category = FindById(Id);

                if (category == null)
                    return false;

                category.CurrentState = (int) Helper.eCurrentState.Delete;
                _dbContext.Categories.Update(category);
                commit();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        public Category? FindById(Guid? Id)
        {
            try
            {
                return _dbContext.Categories.FirstOrDefault(c => c.Id.Equals(Id) && c.CurrentState != (int)Helper.eCurrentState.Delete);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public Category? FindByName(string Name)
        {
            try
            {
                return _dbContext.Categories.FirstOrDefault(c => c.Name.Equals(Name.Trim()) && c.CurrentState != (int)Helper.eCurrentState.Delete);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<Category>? GetAll()
        {
            try
            {
                return _dbContext.Categories.OrderBy(c => c.Name).Where(c => c.CurrentState != (int)Helper.eCurrentState.Delete).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool Save(Category Model)
        {
            try
            {
                var category = FindById(Model.Id);

                if(category == null)
                {
                    Model.Id = Guid.NewGuid();
                    Model.CurrentState =(int) Helper.eCurrentState.Active;
                    _dbContext.Categories.Add(Model);
                }
                else
                {
                    category.Name = Model.Name;
                    category.Description = Model.Description;
                    category.CurrentState =(int) Helper.eCurrentState.Active;
                    _dbContext.Categories.Update(category);
                }
                commit();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        private void commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
