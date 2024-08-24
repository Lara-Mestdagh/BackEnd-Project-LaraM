using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(JwtTokenGenerator jwtTokenGenerator, ILogger<AuthController> logger)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            // Hardcode for testing purposes
            if (request.Email == "laramestdagh2002@gmail.com" && request.Password == "password_dnd_co")
            {
                // Generate a custom Firebase token for a test user
                var customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync("testUserId");

                // Exchange the custom token for an ID token using the Firebase client SDK or REST API
                var idToken = await ExchangeCustomTokenForIdTokenAsync(customToken);

                // Verify the Firebase ID token
                var decodedToken = await _jwtTokenGenerator.VerifyFirebaseTokenAsync(idToken);

                // Generate JWT and Refresh token
                var jwtToken = _jwtTokenGenerator.GenerateTokenAsync(decodedToken.Uid);
                var refreshToken = _jwtTokenGenerator.GenerateRefreshTokenAsync();

                // Store refresh token securely

                return Ok(new
                {
                    Token = jwtToken,
                    RefreshToken = refreshToken
                });
            }

            return Unauthorized();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while logging in.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.RefreshToken))
        {
            _logger.LogWarning("Invalid refresh token request.");
            return BadRequest("Invalid refresh token request.");
        }
        try
        {
            // Validate refresh token here...
            // Implement your logic to validate refresh token

            // If valid, generate new JWT asynchronously
            var newJwtToken = await _jwtTokenGenerator.GenerateTokenAsync(request.UserId);

            return Ok(new
            {
                Token = newJwtToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while refreshing token.");
            return StatusCode(500, "Internal server error.");
        }
    }

    // Method to simulate exchange of custom token for ID token using Firebase REST API
    private async Task<string> ExchangeCustomTokenForIdTokenAsync(string customToken)
    {
        using var client = new HttpClient();
        var requestUri = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithCustomToken?key={_jwtTokenGenerator.FirebaseApiKey}";

        var payload = new
        {
            token = customToken,
            returnSecureToken = true
        };

        var response = await client.PostAsJsonAsync(requestUri, payload);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<FirebaseTokenResponse>();
        return result!.IdToken;
    }
}

public class LoginRequest
{
    [Required(ErrorMessage = "Email is required.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; set; }
}

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "Refresh token is required.")]
    public string? RefreshToken { get; set; }

    [Required(ErrorMessage = "User ID is required.")]
    public string? UserId { get; set; }
}

// Model to parse Firebase REST API response
public class FirebaseTokenResponse
{
    public string? IdToken { get; set; }
    public string? RefreshToken { get; set; }
    public string? ExpiresIn { get; set; }
}