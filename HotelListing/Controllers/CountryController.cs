using AutoMapper;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.mapper = mapper;
        }
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CountryController> logger;
        private readonly IMapper mapper;

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries =await unitOfWork.Countries.GetAll();
                var results = mapper.Map<IList<CountryDTO>>(countries);//Converting in CountryDTO
                return Ok(results);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,$"Something Went Wrong in the {nameof(GetCountries)}");
                return StatusCode(500,"Internal Server Error.Please Try Again");//status code 500 is server issues
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await unitOfWork.Countries.Get(q=>q.Id==id,new List<string> { "Hotels"});
                var results = mapper.Map<CountryDTO>(country);//Converting in CountryDTO
                return Ok(country);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something Went Wrong in the {nameof(GetCountry)}");
                return StatusCode(500, "Internal Server Error.Please Try Again");//status code 500 is server issues
            }
        }
    }
}
