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
    public class ServicesLogCategory : IServicesRepositoryLog<LogCategory>
    {
        private readonly BookyDbContext _dbContext;

        public ServicesLogCategory(BookyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool DeleteLog(Guid Id)
        {
            try
            {
                var logCategory = FindById(Id);
                if (logCategory == null)
                    return false;

                _dbContext.LogCategories.Remove(logCategory);
                commit();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public LogCategory? FindById(Guid? Id)
        {
            try
            {
                return _dbContext.LogCategories.Include(lc => lc.Category).FirstOrDefault(c => c.Id.Equals(Id));
            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<LogCategory>? GetAll()
        {
            try
            {
                return _dbContext.LogCategories.Include(lc => lc.Category).OrderByDescending(lc => lc.Date).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool Save(Guid? Id, Guid? UserId)
        {
            try
            {
                return LogAction(Id, UserId, Helper.Save);
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Update(Guid? Id, Guid? UserId)
        {
            try
            {
                return LogAction(Id, UserId, Helper.Update);
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Delete(Guid? Id, Guid? UserId)
        {
            try
            {
                return LogAction(Id, UserId, Helper.Delete);
            }
            catch (Exception)
            {

                return false;
            }
        }

        private bool LogAction(Guid? Id, Guid? UserId, string action)
        {
            var logCategory = new LogCategory
            {
                Id = Guid.NewGuid(),
                CategoryId = Id,
                UserId = UserId,
                Action = action,
                Date = DateTime.Now,
            };
            _dbContext.LogCategories.Add(logCategory);
            commit();
            return true;
        }

        private void commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
