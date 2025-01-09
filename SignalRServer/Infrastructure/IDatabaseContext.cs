namespace DiscountCodes.SignalRServer.Infrastructure;

public interface IDatabaseContext
{
    Task<bool> GenerateCodesAsync(int number, byte length);
    Task<byte> ApplyCodeAsync(string code);
}