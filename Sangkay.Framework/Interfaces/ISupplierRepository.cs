using Sangkay.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sangkay.Framework.Interfaces
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        // Domain-level repository methods only — no UI/ViewModel types here.
        Task<IEnumerable<Supplier>> SearchByNameAsync(string query);
    }
}