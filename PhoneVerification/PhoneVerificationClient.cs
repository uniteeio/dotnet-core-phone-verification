using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PhoneVerification.Exceptions;
using PhoneVerification.Models;

namespace PhoneVerification
{
    public class PhoneVerificationClient : IPhoneVerificationClient
    {
        private readonly PhoneVerificationOptions _options;
        private readonly HttpClient _httpClient;

        private const string BaseUri = "https://api.authy.com/protected/json/phones/verification";

        public PhoneVerificationClient(PhoneVerificationOptions options, HttpClient httpClient)
        {
            _options = options;
            _httpClient = httpClient;
        }


        public async Task<SendCodeResponse> SendCode(string phoneNumber, int countryCode)
        {
            try
            {
                var postContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string ,string>("via", "sms"),
                    new KeyValuePair<string ,string>("phone_number", phoneNumber),
                    new KeyValuePair<string ,string>("country_code", countryCode.ToString()),
                });

                var response = await _httpClient.PostAsync(BaseUri + "/start?api_key=" + _options.TwilioVerifyApiKey, postContent);
                
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<SendCodeResponse>(responseContent);

                if (!result.Success)
                {
                    throw new SendCodeException(result.Message);
                }

                return result;
            }
            catch (HttpRequestException e)
            {
                throw new PhoneVerificationException(e.Message);
            }
        }
        
        public async Task<VerifyCodeResponse> VerifyCode(string phoneNumber, string verificationCode, int countryCode)
        {
            try
            {
                
                var uriBuilder = new UriBuilder(BaseUri + "/check") {
                    Query = $"phone_number={phoneNumber}&country_code={countryCode}&verification_code={verificationCode}"
                };
                
                _httpClient.DefaultRequestHeaders.Add("X-Authy-API-Key", _options.TwilioVerifyApiKey);

                var response = await _httpClient.GetAsync(uriBuilder.ToString());

                

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<VerifyCodeResponse>(responseContent);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new NoPendingVerificationException(result.Message);
                }
                
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new IncorrectVerificationCodeException(result.Message);
                }
                
                if (!result.Success)
                {
                    throw new VerificationCodeException(result.Message);
                }

                return result;
            }
            catch (HttpRequestException e)
            {
                throw new PhoneVerificationException(e.Message);
            }
        }      
    }
}