using APEX.Server.Authentication;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APEX.Data.Authentication;
using APEX.Data;
using Microsoft.AspNetCore.Authorization;

namespace APEX.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApexUser> _userManager;
        private readonly RoleManager<ApexRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApexDbContext _ctx;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<ApexUser> userManager, RoleManager<ApexRole> roleManager, IConfiguration configuration, ApexDbContext context, ILogger<AuthController> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _ctx = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return NotFound("User not found");

            var pwdCheck = await _userManager.CheckPasswordAsync(user, model.Password);
            if (pwdCheck)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var curUser = await _userManager.FindByNameAsync(model.Username);
            if (curUser != null)
                return StatusCode(StatusCodes.Status409Conflict, "User already exists!");

            ApexUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, "User creation failed! Please check user details and try again.");

            user = await _userManager.FindByNameAsync(model.Username);
            using var trans = await _ctx.Database.BeginTransactionAsync();

            try
            {
                var userRootEntry = new FileEntry
                {
                    Name = $"{user.Id}_root",
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    CreatedBy = user,
                    ModifiedBy = user,
                    IsFolder = true,
                    ParentFile = null
                };

                _ctx.Files.Add(userRootEntry);
                await _ctx.SaveChangesAsync();

                var defaultContainer = new FileContainer
                {
                    Name = "Default Container",
                    RootFolder = userRootEntry,
                };
                _ctx.FileContainers.Add(defaultContainer);
                await _ctx.SaveChangesAsync();

                await trans.CommitAsync();
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                _logger.LogError($"Failed to create FileEntries for new user because {ex}");

                await _userManager.DeleteAsync(user);

                return StatusCode(StatusCodes.Status500InternalServerError, "User creation failed! Unable to create default FileContainer.");
            }

            return Ok("User created successfully!");
        }

        [HttpDelete]
        [Authorize(Roles = UserRoles.ADMIN)]
        [Route("delete-user")]
        public async Task<IActionResult> DeleteUserAsync([FromBody] DeleteUserRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                return StatusCode(StatusCodes.Status404NotFound, "User doesn't exists!");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, $"User deletion failed because:\r\n{string.Join("\r\n", result.Errors.Select(x => x.Description))}");

            // TODO: delete content

            return Ok($"User {request.Username} deleted");
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequest model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status409Conflict, "User already exists!");

            ApexUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, "User creation failed! Please check user details and try again.");

            if (!await _roleManager.RoleExistsAsync(UserRoles.ADMIN))
                await _roleManager.CreateAsync(new ApexRole(UserRoles.ADMIN));

            if (!await _roleManager.RoleExistsAsync(UserRoles.USER))
                await _roleManager.CreateAsync(new ApexRole(UserRoles.USER));

            if (await _roleManager.RoleExistsAsync(UserRoles.ADMIN))
                await _userManager.AddToRoleAsync(user, UserRoles.ADMIN);

            if (await _roleManager.RoleExistsAsync(UserRoles.ADMIN))
                await _userManager.AddToRoleAsync(user, UserRoles.USER);

            return Ok("User created successfully!");
        }

        [HttpGet]
        [Authorize]
        [Route("getCurrentUser")]
        public IActionResult GetCurrentUser()
        {
            return Ok(User);
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
