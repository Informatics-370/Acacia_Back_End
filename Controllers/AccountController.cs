using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.Models.Identities;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Extensions;
using Acacia_Back_End.Helpers;
using Acacia_Back_End.Infrastructure.Data;
using Acacia_Back_End.Infrastructure.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using Stripe;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Acacia_Back_End.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailservice;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IUserRepository userRepo, ITokenService tokenService, IMapper mapper, IEmailService emailservice) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userRepo = userRepo;
            _tokenService = tokenService;
            _mapper = mapper;
            _emailservice = emailservice;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserVM>> GetCurrentUser()
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddress(User);

            var myuser = _mapper.Map<AppUser, UserVM>(user);
            myuser.Token = _tokenService.CreateToken(user);
            myuser.Roles = await _userManager.GetRolesAsync(user);

            return Ok(myuser);
        }

        [HttpGet("Users")]
        public async Task<ActionResult<Pagination<UserVM>>> GetUsers([FromQuery]UserSpecParams userParams)
        {
            var users = _userRepo.GetUsersAsync(userParams).Result.ToList();

            return Ok(new Pagination<UserVM>(userParams.PageIndex, userParams.PageSize, users.Count, users.Skip((userParams.PageIndex - 1) * userParams.PageSize).Take(userParams.PageSize).ToList()));
        }

        [HttpGet("User")]
        public async Task<ActionResult<UserVM>> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null) return NotFound(new ApiResponse(404));

            var myuser = _mapper.Map<AppUser, UserVM>(user);
            myuser.Roles = await _userManager.GetRolesAsync(user);

            return Ok(myuser);

        }

        // Need to do this differently in Iteration 5
        [HttpGet("roles")]
        public async Task<ActionResult<IReadOnlyList<IdentityRole>>> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return Ok(roles);
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<User_AddressVM>> GetUserAddress()
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddress(HttpContext.User);

            return _mapper.Map<User_Address, User_AddressVM>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<User_AddressVM>> UpdateUserAddress(User_AddressVM address)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddress(HttpContext.User);

            user.Address = _mapper.Map<User_AddressVM, User_Address>(address);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<User_Address, User_AddressVM>(user.Address));

            return BadRequest("There was a problem updating the user");
        }



        [HttpPost("register"), DisableRequestSizeLimit]
        public async Task<ActionResult<UserVM>> Register([FromForm] RegisterVM registerVM)
        {
            if (CheckEmailExistsAsync(registerVM.Email).Result.Value) return BadRequest(new ApiResponse(400, "The email submitted has been detected on the system."));

            var formCollection = await Request.ReadFormAsync();
            var imageUrl = "";
            if (formCollection.Files.Count() > 0)
            {
                var file = formCollection.Files.First();
                imageUrl = SaveProfilePictureImage(file).Result;
            }
            if (imageUrl == "")
            {
                imageUrl = "images/users/default.jpg";
            }

            var user = new AppUser
            {
                DisplayName = registerVM.DisplayName,
                Email = registerVM.Email,
                UserName = registerVM.Email,
                ProfilePicture = imageUrl
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            // Roles Added
            if (registerVM.Roles != "Externalcustomer")
            {
                await _userManager.AddToRoleAsync(user, "Externalcustomer");

                //Customer email
                EmailVM request_customer = new EmailVM
                {
                    To = registerVM.Email,
                    Subject = "Awaiting approval",
                    Body = "You have applied for a role that requires approval from the manager. If requirments are met, your role will be updated within 5 working days."
                };
                _emailservice.SendEmail(request_customer);
                // Manager email
                EmailVM request_manager = new EmailVM
                {
                    To = "mzamotembe7@gmail.com",
                    Subject = "Role request",
                    Body = "We have recieved a request from " + registerVM.Email + " to register as a " + registerVM.Roles + ". This role change can be made on the following link: http://localhost:4200/dashboard/user-details/" + registerVM.Email
                };
                _emailservice.SendEmail(request_manager);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, registerVM.Roles);
            }

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            var myuser = _mapper.Map<AppUser, UserVM>(user);
            myuser.Token = _tokenService.CreateToken(user);
            myuser.Roles = await _userManager.GetRolesAsync(user);

            return Ok(myuser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserVM>> Login(LoginVM loginvm)
        {

            var user = await _userManager.FindByEmailAsync(loginvm.Email);

            if (user == null) return Unauthorized(new ApiResponse(401));

            var result2 = await _signInManager.CheckPasswordSignInAsync(user, loginvm.Password, false);

            if (!result2.Succeeded) return Unauthorized(new ApiResponse(401));

            var myuser = _mapper.Map<AppUser, UserVM>(user);
            myuser.Token = _tokenService.CreateToken(user);
            myuser.Roles = await _userManager.GetRolesAsync(user);

            return Ok(myuser);
        }


        [Authorize]
        [HttpPut("update-user"), DisableRequestSizeLimit]
        public async Task<ActionResult<UserVM>> UpdateUserDetails([FromForm] UserVM userDetails)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddress(HttpContext.User);

            var formCollection = await Request.ReadFormAsync();
            if (formCollection.Files.Count() > 0)
            {
                var file = formCollection.Files.First();
                user.ProfilePicture = UpdateUserImage(file, user.ProfilePicture).Result;
            }
            user.DisplayName = userDetails.DisplayName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok();
            return BadRequest("There was a problem updating the user");
        }

        [HttpPut("update-user-role")]
        public async Task<ActionResult<UserVM>> UpdateUserRole(UserVM userDetails)
        {
            var user = await _userManager.FindByEmailAsync(userDetails.Email);

            if (user == null) return NotFound(new ApiResponse(404));

            var roles = await _userManager.GetRolesAsync(user);

            if (roles == null) return NotFound(new ApiResponse(404));

            var result1 = await _userManager.RemoveFromRolesAsync(user, roles);

            if (result1.Succeeded)
            {
                var result2 = await _userManager.AddToRoleAsync(user, userDetails.Roles.FirstOrDefault());
                if (result2.Succeeded)
                {
                    EmailVM request = new EmailVM
                    {
                        To = userDetails.Email,
                        Subject = "User Role Updated",
                        Body = "Your user role on the acacia system has been update to " + userDetails.Roles.FirstOrDefault()
                    };
                    _emailservice.SendEmail(request);
                    return Ok(userDetails);
                }

                return BadRequest(new ApiResponse(400));
            }

            return BadRequest(new ApiResponse(400));
        }

        [Authorize]
        [HttpDelete("delete-user")]
        public async Task<ActionResult<UserVM>> DeleteUser()
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddress(HttpContext.User);

            var result = await _userRepo.RemoveUser(user);

            if (result == true) return Ok();

            return BadRequest(new ApiResponse(400, "There was a problem adding a deleting the user. Please check for any associations before deleting."));
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpPut("reset-password")]
        public async Task<ActionResult<ResetPasswordVM>> ResetPassword(ResetPasswordVM password)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddress(HttpContext.User);

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password.OldPassword);


            if (isPasswordValid == false)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Please check current password" } });
            }

            var result = await _userManager.ChangePasswordAsync(user, password.OldPassword, password.NewPassword);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok();
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> sendForgotPasswordEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound(new ApiResponse(404));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var twoFactorToken = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 1);

            if (twoFactorToken == null) return BadRequest(new ApiResponse(400));

            EmailVM forgotpassword = new EmailVM
            {
                To = email,
                Subject = "Two-Factor Authentication - Password Reset",
                Body = "Hello, your password reset link is " + "http://localhost:4200/account/forgot-password?id=" + user.Id + "&token=" + token + " And the token is " + twoFactorToken.FirstOrDefault()
            };

            _emailservice.SendEmail(forgotpassword);

            return Ok();
        }

        [HttpPost("resend-forgot-password-OTP")]
        public async Task<ActionResult> resendForgotPasswordOTP(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound(new ApiResponse(404));

            var twoFactorToken = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 1);

            if (twoFactorToken == null) return BadRequest(new ApiResponse(400));

            EmailVM forgotpassword = new EmailVM
            {
                To = user.Email,
                Subject = "Two-Factor Authentication - Password Reset",
                Body = "Hello, your OTP is " + twoFactorToken.FirstOrDefault()
            };
            _emailservice.SendEmail(forgotpassword);

            return Ok();
        }

        [HttpPost("reset-forgot-password")]
        public async Task<ActionResult> resetForgottenPassword(ForgotPasswordVM resetVM)
        {
            var user = await _userManager.FindByIdAsync(resetVM.userid);

            if (user == null) return NotFound(new ApiResponse(404, "User not found"));

            var result1 = await _userManager.RedeemTwoFactorRecoveryCodeAsync(user, resetVM.TwoFactorCode);

            if (!result1.Succeeded) return BadRequest(new ApiResponse(400, "Please check your OTP"));

            resetVM.token = resetVM.token.Replace(' ', '+');

            var result = await _userManager.ResetPasswordAsync(user, resetVM.token, resetVM.NewPassword);

            if (result.Succeeded) return Ok(new { message = "Password updated successfully" });

            return BadRequest(new ApiResponse(400, "There was a problem reseting your password."));
        }

        private async Task<string> SaveProfilePictureImage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);

                string filePath = Path.Combine("wwwroot/images/users/", uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return "images/users/" + uniqueFileName;
            }
            return null;
        }

        private async Task<string> UpdateUserImage(IFormFile file, string imageurl)
        {
            if (file != null && file.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);

                string filePath = Path.Combine("wwwroot/images/users/", uniqueFileName);

                // Remove the existing image if it exists
                string existingFilePath = imageurl;
                if (System.IO.File.Exists("wwwroot/" + existingFilePath))
                {
                    System.IO.File.Delete("wwwroot/" + existingFilePath);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return "images/users/" + uniqueFileName;
            }
            return null;
        }
    }
}
