using AutoMapper;
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

        [Authorize]
        [HttpGet("{id:int}")]
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
    }
}
