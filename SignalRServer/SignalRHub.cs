using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using DiscountCodes.SignalRServer.Services;

namespace DiscountCodes.SignalRServer;

public class SignalRHub : Hub
{
    private readonly IDiscountCodeService _service;

    public SignalRHub(IDiscountCodeService service)
    {
        _service = service;
    }

    public async Task<bool> GenerateCodes(int number, byte length)
    {
        Console.WriteLine($"GenerateCodes called with parameters: number={number}, length={length}");

        if (number < 0 || number > 2000)
        {
            Console.WriteLine($"Number must be between 0 and 2000");
            return false;
        }

        if (length < 7 || length > 8)
        {
            Console.WriteLine($"Length must be between 7 and 8");
            return false;
        }

        bool result = await _service.GenerateCodesAsync(number, length);

        return result;
    }

    public async Task<byte> ApplyCode(string code)
    {
        Console.WriteLine($"Apply code: code={code}");

        if (string.IsNullOrWhiteSpace(code))
        {
            Console.WriteLine($"Code must not be null or emtpy.");
            return 0;
        }

        if (code.Trim().Length < 7 || code.Trim().Length > 8)
        {
            Console.WriteLine($"Code must be between 7 and 8 characters long.");
            return 0;
        }

        byte result = await _service.ApplyCodeAsync(code);

        return result;
    }
}