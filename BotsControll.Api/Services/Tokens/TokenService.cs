using BotsControll.Core.Dtos.Login;
using BotsControll.Core.Models.Identity;
using BotsControll.Core.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BotsControll.Api.Services.Tokens;

public class TokenService
{
    private readonly ITokensRepository _tokenRepository;
    private readonly ILogger<TokenService> _logger;

    public TokenService(ITokensRepository tokenRepository, ILogger<TokenService> logger)
    {
        _tokenRepository = tokenRepository;
        _logger = logger;
    }

    public TokenPair CreateTokenPair(IEnumerable<Claim> claims)
    {
        return new TokenPair(
            AccessToken: CreateAccessToken(claims),
            RefreshToken: CreateRefreshToken(claims)
        );
    }

    private static string CreateAccessToken(IEnumerable<Claim> claims)
    {
        var jwt = new JwtSecurityToken(
            issuer: TokenConfig.Issuer,
            audience: TokenConfig.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TokenConfig.LifeTime),
            signingCredentials: new SigningCredentials(TokenConfig.GetSymmetricSecurityAccessKey(), SecurityAlgorithms.HmacSha256)
        );
        //Todo Выести в сервис JwtSecurityTokenHandler
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private static string CreateRefreshToken(IEnumerable<Claim> claims)
    {
        var jwt = new JwtSecurityToken(
            issuer: TokenConfig.Issuer,
            audience: TokenConfig.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TokenConfig.RefreshLifeTime),
            signingCredentials: new SigningCredentials(TokenConfig.GetSymmetricSecurityRefreshKey(), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public bool ValidateAccessToken(string token, out ClaimsPrincipal? userClaimsPrincipal)
    {
        try
        {
            userClaimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidIssuer = TokenConfig.Issuer,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = TokenConfig.Audience,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,
                    IssuerSigningKey = TokenConfig.GetSymmetricSecurityAccessKey(),
                },
                out SecurityToken? result
            );

            return true;
        }
        catch (SecurityTokenException)
        {
            userClaimsPrincipal = null;
            return false;
        }

        _logger.LogError("Unexpected behaviour in token validation");
        throw new Exception("Unexpected behaviour in token validation");
    }

    public bool ValidateRefreshToken(string token, out ClaimsPrincipal? userClaimsPrincipal)
    {
        try
        {
            userClaimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidIssuer = TokenConfig.Issuer,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = TokenConfig.Audience,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,
                    IssuerSigningKey = TokenConfig.GetSymmetricSecurityRefreshKey(),
                },
                out SecurityToken? result
            );

            return true;
        }
        catch (SecurityTokenException)
        {
            userClaimsPrincipal = null;
            return false;
        }

        _logger.LogError("Unexpected behaviour in token validation");
        throw new Exception("Unexpected behaviour in token validation");
    }

    public async Task<Token?> FindToken(string refreshToken)
    {
        return await _tokenRepository.TryGetToken(refreshToken);
    }

    public async Task<Token> SaveToken(int userId, string refreshToken)
    {
        var token = await _tokenRepository.TryGetByUserId(userId);
        if (token != null)
            token.RefreshToken = refreshToken;

        token = new Token
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            RefreshToken = refreshToken
        };
        await _tokenRepository.Add(token);

        return token;
    }

    public async Task<Token> RemoveToken(string refreshToken)
    {
        var token = await _tokenRepository.GetToken(refreshToken);

        await _tokenRepository.Delete(token);

        return token;
    }
}