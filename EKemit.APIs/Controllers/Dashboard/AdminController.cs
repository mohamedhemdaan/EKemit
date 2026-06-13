using EKemit.APIs.DTOs;
using EKemit.APIs.DTOs.Dashbord_DTOs;
using EKemit.APIs.Errors;
using EKemit.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EKemit.APIs.Controllers.Dashboard
{
    public class AdminController : APIBaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;


        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost("AddRole")]
        public async Task<ActionResult> AddRole(string roleName)
        {
            // Check if role already exists
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest($"Role '{roleName}' already exists.");
            }

            // Create new role
            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return Ok($"Role '{roleName}' created successfully.");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }




        [HttpPost("AddUserToRole")]
        public async Task<ActionResult> AddUserToRole(string userEmail, string roleName)
        {
            // Get user and role
            var user = await _userManager.FindByEmailAsync(userEmail);
            var role = await _roleManager.FindByNameAsync(roleName);

            if (user == null)
            {
                return BadRequest($"User '{userEmail}' not found.");
            }

            if (role == null)
            {
                return BadRequest($"Role '{roleName}' not found.");
            }

            // Check if user is already in role
            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                return BadRequest($"User '{userEmail}' is already in role '{roleName}'.");
            }

            // Add user to role
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return Ok($"User '{userEmail}' added to role '{roleName}' successfully.");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }


        [HttpPut("EditRole/{id}")]
        public async Task<ActionResult> EditRole(string id, RoleDto model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role == null)
                {
                    return NotFound(new ApiResponse(404, "Role not found."));
                }

                // Check if the new role name already exists and is different from the current role name
                if (await _roleManager.RoleExistsAsync(model.Name) && role.Name != model.Name)
                {
                    return BadRequest(new ApiResponse(400, "Role already exists."));
                }

                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return Ok(new ApiResponse(200, "Role updated successfully."));
                }
                return BadRequest(new ApiResponse(400, result.Errors.First().Description));
            }
            return BadRequest(new ApiResponse(400, "Invalid data."));
        }



        [HttpPost("DeleteUser/{email}")]
        public async Task<ActionResult> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        //[HttpGet("AllUsers")]
        //public async Task<IActionResult> AllUsers()
        //{
        //    var Users = await _userManager.Users.Select(U => new UserDashboardDto
        //    {
        //        Id = U.Id,
        //        UserName = U.UserName,
        //        PhoneNumber = U.PhoneNumber,
        //        Email = U.Email,
        //        DisplayName = U.DisplayName,
        //        Roles = _userManager.GetRolesAsync(U).Result
        //    }).ToListAsync();

        //    return Ok(Users);
        //}



        [HttpGet("AllUsers")]
        public async Task<IActionResult> AllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserDashboardDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = new UserDashboardDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Roles = roles
                };
                userDtos.Add(userDto);
            }

            return Ok(userDtos);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUser([FromBody] UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null || string.IsNullOrEmpty(updateUserDto.Id))
            {
                return BadRequest(new ApiResponse(400, "Invalid user data."));
            }

            var user = await _userManager.FindByIdAsync(updateUserDto.Id);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found."));
            }

            // Update user properties
            user.UserName = updateUserDto.UserName;
            user.Email = updateUserDto.Email;
            user.DisplayName = updateUserDto.DisplayName;
            user.PhoneNumber = updateUserDto.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse(400, "Failed to update user."));
            }

            // Update user roles with role checking
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in updateUserDto.Roles)
            {
                if (await _roleManager.RoleExistsAsync(role.Name))
                {
                    if (userRoles.Any(r => r == role.Name) && !role.IsSelected)
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else if (!userRoles.Any(r => r == role.Name) && role.IsSelected)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                }
                else
                {
                    return BadRequest(new ApiResponse(400, $"Role '{role.Name}' does not exist."));
                }
            }

            return Ok(new ApiResponse(200, "User updated successfully."));
        }



        //[Authorize]
        [HttpPut("UpdateSelf")]
        public async Task<IActionResult> UpdateSelf([FromBody] UpdateSelfUserDto updateSelfUserDto)
        {
            if (updateSelfUserDto == null)
            {
                return BadRequest("Invalid user data.");
            }

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update user properties
            user.UserName = updateSelfUserDto.UserName;
            user.Email = updateSelfUserDto.Email;
            user.DisplayName = updateSelfUserDto.DisplayName;
            user.PhoneNumber = updateSelfUserDto.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok("User updated successfully.");
            }

            return BadRequest("Failed to update user.");
        }

        //[Authorize]
        //[HttpGet("CurrentUser")]
        //public async Task<ActionResult<updateSelfUserDto>> GetCurrentUser()
        //{
        //    // Get the current user's email from the claims
        //    var userEmail = User.FindFirstValue(ClaimTypes.Email);

        //    if (string.IsNullOrEmpty(userEmail))
        //    {
        //        return Unauthorized(new ApiResponse(401, "User not logged in."));
        //    }

        //    // Find the user by email
        //    var user = await _userManager.FindByEmailAsync(userEmail);

        //    if (user == null)
        //    {
        //        return NotFound(new ApiResponse(404, "User not found."));
        //    }

        //    // Get user roles
        //    //var roles = await _userManager.GetRolesAsync(user);

        //    // Map user details to DTO
        //    var currentUserDto = new updateSelfUserDto
        //    {
        //        Id = user.Id,
        //        UserName = user.UserName,
        //        Email = user.Email,
        //        DisplayName = user.DisplayName,
        //        PhoneNumber = user.PhoneNumber,
        //        //Roles = roles
        //    };

        //    return Ok(currentUserDto);
        //}

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UpdateSelfUserDto>> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(new ApiResponse(401, "User not logged in."));
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found."));
            }

            var currentUserDto = new UpdateSelfUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                DisplayName = user.DisplayName,
                PhoneNumber = user.PhoneNumber
            };

            return Ok(currentUserDto);
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("AllRoles")]
        public async Task<ActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }
    }
}



