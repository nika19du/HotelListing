using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Jamaica",
                    ShortName = "JM"
                },
                new Country
                {
                    Id = 2,
                    Name = "Bulgaria",
                    ShortName = "BG"
                },
                new Country
                {
                    Id = 3,
                    Name = "Russia",
                    ShortName = "RU"
                },
                new Country
                {
                    Id = 4,
                    Name = "Bahamas",
                    ShortName = "BS"
                },
                new Country
                {
                    Id = 5,
                    Name = "Cayman Island",
                    ShortName = "CI"
                }
            );

            builder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Sandals Resort and  Spa",
                    Address = "Negril",
                    CountryId = 1,
                    Rating = 4.5
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Oazis",
                    Address = "Plovdiv",
                    CountryId = 2,
                    Rating = 4
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Grand Paldium",
                    Address = "Negril",
                    CountryId = 4,
                    Rating = 4
                },
                new Hotel
                {
                    Id = 4,
                    Name = "Comfort Suites",
                    Address = "George Town",
                    CountryId = 5,
                    Rating = 4.3
                }
              );
        }
    }
}
