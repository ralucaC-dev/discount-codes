namespace DiscountCodes.SignalRServer.Services;

public interface IDiscountCodeService
{
    Task<bool> GenerateCodesAsync(int number, byte length);
    Task<byte> ApplyCodeAsync(string code);
}