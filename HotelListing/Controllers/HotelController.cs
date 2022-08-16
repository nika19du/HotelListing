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
    public class HotelController : ControllerBase
    {
        public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.mapper = mapper;
        }
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<HotelController> logger;
        private readonly IMapper mapper;

        [HttpGet]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotel = await unitOfWork.Hotels.GetAll();
                var results = mapper.Map<IList<HotelDTO>>(hotel); 
                return Ok(results);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something Went Wrong in the {nameof(GetHotels)}");
                return StatusCode(500, "Internal Server Error.Please Try Again");//status code 500 is server issues
            }
        }
         
        [HttpGet("{id:int}",Name ="GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await unitOfWork.Hotels.Get(q => q.Id == id, new List<string> { "Country" });
                var results = mapper.Map<HotelDTO>(hotel);
                return Ok(results);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something Went Wrong in the {nameof(GetHotel)}");
                return StatusCode(500, "Internal Server Error.Please Try Again");//status code 500 is server issues
            }
        }

       // [Authorize(Roles ="Administrator")] 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
        {
            if (this.User.IsInRole("User"))
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                logger.LogError($"Invalid PUT attempt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = mapper.Map<Hotel>(hotelDTO);
                await unitOfWork.Hotels.Insert(hotel);
                await unitOfWork.Save();

                return CreatedAtRoute("GetHotel", new { id=hotel.Id },hotel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateHotel)}");
                return StatusCode(500, "Internal Server Error.Please Try Again");
            }
        }

     //   [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id,[FromBody] UpdateHotelDTO hotelDTO)
        {
            //if (!this.User.IsInRole("User") || !this.User.IsInRole("Administrator"))
            //{
            //    return Unauthorized();
            //}
            if (!ModelState.IsValid || id<1)
            {
                logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = await unitOfWork.Hotels.Get(q => q.Id == id);
                if (hotel==null)
                {
                    logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
                    return BadRequest("Submitted data is invalid");
                }

                mapper.Map(hotelDTO, hotel);
                unitOfWork.Hotels.Update(hotel);
                await unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdateHotel)}");
                return StatusCode(500, "Internal Server Error. Please Try Again");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id<1)
            {
                logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                return BadRequest();
            }
            try
            {
                var hotel = await unitOfWork.Hotels.Get(q => q.Id == id);
                if (hotel==null)
                {
                    logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                    return BadRequest("Submitted data is invalid");
                }

                await unitOfWork.Hotels.Delete(id);
                await unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something Went Wrong in {nameof(DeleteHotel)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
