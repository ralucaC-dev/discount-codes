using DiscountCodes.SignalRServer.Infrastructure;

namespace DiscountCodes.SignalRServer.Services;

public class DiscountCodeService : IDiscountCodeService
{
    private readonly IDatabaseContext _dbContext;

    public DiscountCodeService(IDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> GenerateCodesAsync(int number, byte length)
    {
        if (number < 0 || number > 2000)
        {
            return false;
        }

        if (length < 7 || length > 8)
        {
            return false;
        }

        return await _dbContext.GenerateCodesAsync(number, length);
    }

    public async Task<byte> ApplyCodeAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return 0;
        }

        if (code.Trim().Length < 7 || code.Trim().Length > 8)
        {
            return 0;
        }
        
        return await _dbContext.ApplyCodeAsync(code);
    }
}