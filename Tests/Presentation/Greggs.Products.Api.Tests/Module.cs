using System.Runtime.CompilerServices;

namespace Greggs.Products.Api.Tests;

public class Module
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyHttp.Initialize();
        
        // The traceId changes from request to request
        // and as such cannot be verified. 
        VerifierSettings.IgnoreMember("traceId");
    }
}