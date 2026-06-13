using AutoMapper;
using EKemit.APIs.DTOs;
using EKemit.APIs.Errors;
using EKemit.APIs.Extensions;
using EKemit.APIs.Helpers.Email;
using EKemit.Core.Entities.Identity;
using EKemit.Core.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace EKemit.APIs.Controllers
{
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly IEmailSettings _emailSettings;
        private readonly IConfiguration configuration;

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager
                                 , IAuthService authService,IMapper mapper,IEmailSettings emailSettings,
                                   IConfiguration configuration        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
            _emailSettings = emailSettings;
            this.configuration = configuration;
        }

        ////Register 
        ////BaseUrl/Api/Account/Register
        //[HttpPost("Register")]
        //public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        //{
        //    if (CheckEmailExists(model.Email).Result.Value)
        //        return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "this Email Already existed" } });

        //    var user = new AppUser()
        //    {
        //        DisplayName = model.DisplayName,
        //        Email = model.Email,                   // OmarAhmed@gmail.com
        //        UserName = model.Email.Split('@')[0], //OmarAhmed
        //        PhoneNumber = model.PhoneNumber
        //    };
        //    var Result = await _userManager.CreateAsync(user, model.Password);
        //    if (!Result.Succeeded) return BadRequest(new ApiResponse(400));

        //    var ReturnedUser = new UserDto()
        //    {
        //        DisplayName = user.DisplayName,
        //        Email = user.Email,
        //        Token = await _authService.CreateTokenAsync(user, _userManager)
        //    };
        //    return Ok(ReturnedUser);
        //}


        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            // Check if email already exists
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This email is already registered" } });

            // Create the user
            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0], // Extract username from email
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            // Assign "Buyer" role to the newly created user
            var roleResult = await _userManager.AddToRoleAsync(user, "Buyer");
            if (!roleResult.Succeeded) return BadRequest(new ApiResponse(400, "Failed to assign Buyer role"));

            // Create the UserDto to return
            var returnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            };

            return Ok(returnedUser);
        }


        //Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            var token = await _authService.CreateTokenAsync(user, _userManager);


            Response.Cookies.Append("token",token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Domain = "localhost",
                Expires = DateTime.Now.AddDays(double.Parse(configuration["Jwt:DurationDays"]))
            });
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });

        }

        [Authorize]
        [HttpGet]// Get :  BaseUrl/Api/Account
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            //email
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.DisplayName,
            });
        }


        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {

            var user = await _userManager.FindUserWithAddressAsync(User);
            
            return Ok(_mapper.Map<AddressDto>(user.Address));

        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto UpdatedAddress)
        {
            var user = await _userManager.FindUserWithAddressAsync(User); //User by Token

            var address = _mapper.Map<AddressDto,Address>(UpdatedAddress); //updated address and id =0


            //address.Id = user.Address.Id; //updated user with id
            user.Address = address; // updated address of user

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(address);

        }



        [HttpGet("TestSendEmail")]
        public ActionResult TestSendEmail()
        {
            var email = new Emaill()
            {
                To = "mohamedhemdan9654@gmail.com",
                Subject = "Test",
                Body = "Reset Password"
            };

            _emailSettings.SendEmail(email);

            return StatusCode(StatusCodes.Status200OK,new ApiResponse(200, "Email is Sent Successfully"));
        }




        //Send ForgetPasswordlink to Email
        [HttpPost("ForgetPassword")] //Post : BaseUrl/api/Account/ForgetPassword
        public async Task<ActionResult> ForgotPassword(ForgetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user is not null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                //https://localhost/Account/ResetPassword?email=mohamedhemdan743@gmail.com?Token=udgugutfyegiuhdjzzzjzjzzjz
                var ForgetPasswordlink = Url.Action("ResetPassword", "Accounts", new { Email = user.Email, Token = token }, Request.Scheme);
                ForgetPasswordlink = ForgetPasswordlink.Replace("https://localhost:7294", "http://localhost:3000");
                ForgetPasswordlink = ForgetPasswordlink.Replace("api/Accounts/", "");
                var email = new Emaill()
                {
                    To = user.Email,
                    Subject = "Reset Password",
                    Body = ForgetPasswordlink
                };
                _emailSettings.SendEmail(email);
                return StatusCode(StatusCodes.Status200OK,
                    new ApiResponse(200, $"A link to change your password has been sent on {user.Email} , Please check Your Email"));

            }
            return StatusCode(StatusCodes.Status400BadRequest,new ApiResponse(400,"This Email Not Found"));   
        }

        [HttpGet("resetpassword")]
        public ActionResult ResetPassword(string email , string token)
        {
            var model = new ResetPasswordDto()
            {
                Email = email,
                Token = token
            };

            return Ok(model);   
        }

        [HttpPost("resetpassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user is not null)
            {
               var result =  await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return BadRequest(ModelState);
                }

                return StatusCode(StatusCodes.Status200OK, new ApiResponse(200, "The Password has been changed Successfully"));

            }

            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse(400, "Couldn't Send Link to email , try again "));

        }


        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto changePassword)
        {
            // Get the user 
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            // ChangePasswordAsync Method changes the user password
            var result = await _userManager.ChangePasswordAsync(user,changePassword.oldPassword,changePassword.NewPassword);

            // The new password did not meet the complexity rules or the current password is incorrect.
            // Add these errors to the ModelState 
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return BadRequest(ModelState);
            }

            // Add these errors to the ModelState and rerender ChangePassword view
            await _signInManager.RefreshSignInAsync(user);

            return Ok("Your password is successfully changed.");
        }


        [HttpGet("emailexists")] // api/Account/emailexists?email=mohamedhemdan@gmail.com
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null; //true if exist
        }

    }


    
}
