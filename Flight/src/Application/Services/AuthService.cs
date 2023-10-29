using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using Domain;
using Domain.Common;
using Domain.DTOs;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;


    public AuthService(IPasswordHasher<User> passwordHasher, IUnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Tuple<string, List<string>>> AuthenticateAsync(long userId, string password)
    {
        Log.Information("Try login: {login}", userId);
        var identity = await GetIdentityAsync(userId, password);
        if (identity == null)
        {
            Log.Warning("Login failed for {login}", userId);
            return null;
        }

        var accessToken = GenerateToken(identity, AuthOptions.AccessTokenLifetime);
        var permissions = accessToken.Claims.Where(u => u.Type == ClaimsIdentity.DefaultRoleClaimType)
            .Select(u => u.Value).ToList();
        return new Tuple<string, List<string>>(EncodeTokenToString(accessToken), permissions);
    }

    public async Task<JwtResponseDto> AuthenticateWithRefreshAsync(string login, string password)
    {
        
        var user = await _unitOfWork.UserRepository.GetByUsernameAsync(new List<Expression<Func<User, bool>>>
        {
            user => user.Username == login
        });
        
        if (user == null)
        {
            throw new Exception(@"Пользователь с таким Логин \ Пароль не найден");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        if (result != PasswordVerificationResult.Success)
        {
            throw new Exception("Неверный пароль");
        }

        var jwt = await AuthenticateAsync(user.Id, password);
        if (jwt == null)
        {
            throw new Exception(@"Пользователь с таким Логин \ Пароль не найден");
        }

        if (string.IsNullOrEmpty(jwt.Item1))
        {
            return null;
        }

        Log.Information("Login succeeded for {login}", login);
        var refreshIdentity = GenerateRefreshTokenIdentity(login);
        var refreshToken = EncodeTokenToString(GenerateToken(refreshIdentity, TimeSpan.FromDays(1)));
        return new JwtResponseDto
        {
            Token = jwt.Item1,
            AccessTokenExpiration = DateTime.Now.Add(AuthOptions.AccessTokenLifetime), RefreshToken = refreshToken,
            Permission = jwt.Item2
        };
    }

    public async Task<JwtResponseDto> RefreshAccessToken(string refreshToken)
    {
        JwtSecurityToken token;
        try
        {
            token = ReadToken(refreshToken);
        }
        catch (Exception e)
        {
            Log.Warning("ReadToken NULL value: {0}", e.Message);
            return null;
        }

        DateTime dNow = DateTime.Now;
        DateTime vFrom = TimeZoneInfo.ConvertTimeFromUtc(token.ValidFrom, TimeZoneInfo.Local);
        DateTime vTo = TimeZoneInfo.ConvertTimeFromUtc(token.ValidTo, TimeZoneInfo.Local);

        if (vFrom >= dNow || vTo <= dNow)
        {
            throw new Exception("Срок действия токена истек. Пройдите авторизацию");
        }

        var user = await _unitOfWork.UserRepository.GetByIdAsync(long.Parse(token.Issuer));
        if (user == null)
        {
            throw new Exception("Пользователь не найден");
        }

        var accessTokenIdentity = GenerateAccessTokenIdentity(user);
        var accessToken = GenerateToken(accessTokenIdentity, AuthOptions.AccessTokenLifetime);
        var newRefreshTokenIdentity = GenerateRefreshTokenIdentity(user.Username);
        var newRefreshToken = GenerateToken(newRefreshTokenIdentity, TimeSpan.FromDays(1));

        return new JwtResponseDto
        {
            Token = EncodeTokenToString(accessToken),
            RefreshToken = EncodeTokenToString(newRefreshToken),
            AccessTokenExpiration = DateTime.Now.Add(AuthOptions.AccessTokenLifetime)
        };
    }

    private JwtSecurityToken ReadToken(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            handler.ValidateToken(jwt, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
            }, out _);
        }
        catch (SecurityTokenException e)
        {
            throw new Exception("Принятый токен невалиден: " + e.Message);
        }

        return handler.ReadJwtToken(jwt);
    }

    private async Task<ClaimsIdentity> GetIdentityAsync(long userId, string password)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

        var verification = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        if (verification != PasswordVerificationResult.Success)
        {
            return null;
        }

        return GenerateAccessTokenIdentity(user);
    }


    private ClaimsIdentity GenerateAccessTokenIdentity(User user)
    {
        var claims = new List<Claim> {new(ClaimsIdentity.DefaultNameClaimType, user.Username)};

        claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleId.ToString()));


        var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }

    private ClaimsIdentity GenerateRefreshTokenIdentity(string userName)
    {
        var claims = new List<Claim> {new(ClaimsIdentity.DefaultNameClaimType, userName)};
        var claimsIdentity = new ClaimsIdentity(claims, "RefreshToken", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }

    private JwtSecurityToken GenerateToken(ClaimsIdentity identity, TimeSpan duration)
    {
        var now = DateTime.Now;
        var jwt = new JwtSecurityToken(
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(duration),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        return jwt;
    }

    private string EncodeTokenToString(JwtSecurityToken jwt)
    {
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }


    /*public async Task<bool> PasswordsMatched(User user, string newPassword)
    {
        var verification = _passwordHasher.VerifyHashedPassword(user, user.Password, newPassword);
        return verification == PasswordVerificationResult.Success;
    }*/

    /*public async Task EditPasswordAsync(UserAccountRequest request)
    {
        using (var userRepository = new Repository<User>(_provider))
        {
            var user = await userRepository.Get(x => x.UserName.ToLower() == request.Email.ToLower()).SingleAsync();
            user.Password = _passwordHasher.HashPassword(user, request.Password);
            userRepository.Update(user);
            userRepository.Commit();
        }
    }*/
}