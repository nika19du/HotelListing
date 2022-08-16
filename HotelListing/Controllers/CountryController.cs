using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{id:int}", Name = "GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [HttpPost] 
     //   [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            if (this.User.IsInRole("User"))
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                logger.LogError($"Invalid Post attempt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = mapper.Map<Country>(countryDTO);
                await unitOfWork.Countries.Insert(country);
                await unitOfWork.Save();

                return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateCountry)}");
                return StatusCode(500, "Internal Server Error.Please Try Again");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {
            //if (!this.User.IsInRole("User") || !this.User.IsInRole("Administrator"))
            //{
            //    return Unauthorized();
            //}
            if (!ModelState.IsValid || id < 1)
            {
                logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = await unitOfWork.Countries.Get(q => q.Id == id);
                if (country == null)
                {
                    logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                    return BadRequest("Submitted data is invalid");
                }

                mapper.Map(countryDTO, country);
                unitOfWork.Countries.Update(country);
                await unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdateCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try Again");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                return BadRequest();
            }
            try
            {
                var country = await unitOfWork.Countries.Get(q => q.Id == id);
                if (country == null)
                {
                    logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                    return BadRequest("Submitted data is invalid");
                }

                await unitOfWork.Countries.Delete(id);
                await unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something Went Wrong in {nameof(DeleteCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
