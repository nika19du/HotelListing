using AutoMapper;
using HotelListing.Data;
using HotelListing.Models;
using HotelListing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class AccountController : ControllerBase
    {
        public AccountController(UserManager<ApiUser> userManager,  
            ILogger<AccountController> logger,
            IMapper mapper, IAuthManager authManager)
        {
            this.userManager = userManager; 
            this.logger = logger;
            this.mapper = mapper;
            this.authManager = authManager;
        }

        private readonly UserManager<ApiUser> userManager; 
        private readonly ILogger<AccountController> logger;
        private readonly IMapper mapper;
        private readonly IAuthManager authManager;

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            logger.LogInformation($"Registration Attempt for {userDTO.Email}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = mapper.Map<ApiUser>(userDTO);
                user.UserName = userDTO.Email;
                var results = await userManager.CreateAsync(user,userDTO.Password);//it automatically take the password hash it,do all the work

                if (!results.Succeeded)
                {
                    foreach (var err in results.Errors)
                    {
                        ModelState.AddModelError(err.Code,err.Description);
                    }
                    return BadRequest(ModelState);
                }
                await userManager.AddToRolesAsync(user, userDTO.Roles);
                return Accepted(); 
            }
            catch (Exception ex)
            {
                logger.LogError(ex,$"Something Went Wrong in the {nameof(Register)}");
                return Problem($"Something Went Wrong in the {nameof(Register)}",statusCode:500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            logger.LogInformation($"Login Attempt for {userDTO.Email}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {  
                if (!await authManager.ValidateUser(userDTO))
                {
                    return Unauthorized();
                }

                return Accepted(new { Token = await authManager.CreateToken() });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something Went Wrong in the {nameof(Login)}");
                return Problem($"Something Went Wrong in the {nameof(Login)}", statusCode: 500);
            }
        }
    }
}
