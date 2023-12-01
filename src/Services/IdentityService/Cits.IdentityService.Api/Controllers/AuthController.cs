using Cits.IdentityService.Application.Contracts.Auth;
using Cits.IdentityService.Application.Contracts.Auth.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Cits.IdentityService.Api.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginInput input, CancellationToken cancellationToken)
    {
        return null;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserRegisterInput input, CancellationToken cancellationToken)
    {
        return null;
    }
}