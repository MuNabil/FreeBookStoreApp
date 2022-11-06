using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IRepositories
{
    public interface IServicesRepository<Entity>
    {
        List<Entity>? GetAll();

        Entity? FindById(Guid? Id);

        Entity? FindByName(string Name);

        bool Save(Entity Model);

        bool Delete(Guid Id);
    }
}
