using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;

namespace What2Gift.Application.Users.Helpers;

public static class TopUpCodeGenerator
{
    public static async Task<int> GenerateUniqueTopUpCodeAsync(IDbContext context, CancellationToken cancellationToken = default)
    {
        var random = new Random();
        var maxAttempts = 10000; // Maximum attempts to find unique code
        
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            var code = random.Next(0, 10000); // 0 to 9999
            
            // Check if code is already used
            var exists = await context.Users
                .AnyAsync(u => u.TopUpCode == code, cancellationToken);
            
            if (!exists)
            {
                return code;
            }
        }
        
        // If all codes are taken (very unlikely), throw exception
        throw new InvalidOperationException("Unable to generate unique TopUpCode. All codes from 0-9999 are taken.");
    }
}

