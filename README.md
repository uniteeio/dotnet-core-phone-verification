# A .NET Core 2 service for phone number verification

Nuget package can be found here : https://www.nuget.org/packages/Unitee.PhoneVerification

## Install package

dotnet add package Unitee.PhoneVerification

## Twilio verify API

This lib use Twilio verify API, you need a Twilio API key in order to use this lib. Please follow this link for more informations : https://www.twilio.com/verify/api 

### Configuration

Add Twilio app API Key in the root of your appsettings.json :

<pre>
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  <b>"PhoneVerification": {
    "TwilioVerifyApiKey": "a5ze5rty123qs3df3gh"
  }</b>
}
</pre>

Add service in your Startup.cs file :
<pre>
// This method gets called by the runtime. Use this method to add services to the container.
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    <b>services.AddPhoneVerification(Configuration); </b>
}
</pre>
        
### Controller
####Inject service in your class (controller, ...) : 
<pre>
private readonly IPhoneVerification _phoneVerification;

public ValuesController(IPhoneVerification _phoneVerification)
{
    _phoneVerification = phoneVerification;
}
</pre>

####Send validation code : 
```
[HttpGet]
public async Task<ActionResult> SendCode()
{
    SendCodeResponse sendCodeResponse = await _client.SendCode("+33781643710", 33);
    
    return Ok();
}
```

This method will send the code via SMS to the defined phone number.

####Code validation :
```
[HttpGet]
public async Task<ActionResult> VerifyCode()
{
    VerifyCodeResponse verifyCodeResponse =  await _client.VerifyCode("+33781643710", "6133", 33);
    
    return Ok();
}
```

This method will verified the provided code regarding the provided phone number.