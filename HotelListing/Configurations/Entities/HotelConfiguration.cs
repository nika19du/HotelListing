using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Configurations.Entities
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
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
