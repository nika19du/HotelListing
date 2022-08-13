using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Configurations.Entities
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
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
        }
    }
}
