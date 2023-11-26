using System.Runtime.CompilerServices;

namespace Greggs.Products.Api.Tests;

public class Module
{
    [ModuleInitializer]
    public static void Initialize() => VerifyHttp.Initialize();
}