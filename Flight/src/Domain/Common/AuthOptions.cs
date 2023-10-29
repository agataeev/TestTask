using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Common;

public class AuthOptions
{
    const string KEY = "airastana the most secret key in the world";
    public static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromHours(1);

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}