namespace Pinelabs_Example;

public class Program
{
    public static async Task Main(string[] args)
    {
        PinelabsExample example = new PinelabsExample();
        await example.InitPinelabs(args);
    }
}