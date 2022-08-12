using HotelListing.Data;
using HotelListing.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(DatabaseContext context)
        {
            this.context = context;
        }
        private readonly DatabaseContext context;
        private IGenericRepository<Country> countries;
        private IGenericRepository<Hotel> hotels;
        public IGenericRepository<Country> Countries => countries ??= new GenericRepository<Country>(context);
        public IGenericRepository<Hotel> Hotels => hotels ??= new GenericRepository<Hotel>(context);

        public void Dispose()
        {
            context.Dispose();//kill any memory db connection
            GC.SuppressFinalize(this);//garbage colector
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
