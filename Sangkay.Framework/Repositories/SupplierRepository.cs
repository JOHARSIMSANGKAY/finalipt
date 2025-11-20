// Basser.Framework/Repositories/SupplierRepository.cs
using Sangkay.Domain.Entities;
using Sangkay.Framework.Data;
using Sangkay.Framework.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sangkay.Framework.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly SangkayDbContext _db;
        public SupplierRepository(SangkayDbContext db) => _db = db;

        public async Task AddAsync(Supplier entity)
        {
            await _db.Suppliers.AddAsync(entity);
        }

        public async Task DeleteAsync(Supplier entity)
        {
            _db.Suppliers.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _db.Suppliers.AsNoTracking().OrderBy(s => s.Name).ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _db.Suppliers.FindAsync(id);
        }

        public async Task UpdateAsync(Supplier entity)
        {
            _db.Suppliers.Update(entity);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Supplier>> SearchByNameAsync(string query)
        {
            return await _db.Suppliers
                .Where(s => s.Name.Contains(query))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
