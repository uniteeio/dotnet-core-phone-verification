using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using PhoneVerification.Exceptions;
using PhoneVerification.Models;
using Xunit;

namespace PhoneVerification.Tests
{
    public class PhoneVerificationClientTests
    {
        private readonly PhoneVerificationClient _client;
        private readonly FakeMessageHandler _messageHandler;

        public PhoneVerificationClientTests()
        {
            _messageHandler = new FakeMessageHandler();
            var httpClient = new HttpClient(_messageHandler);
            
            _client = new PhoneVerificationClient(
                new PhoneVerificationOptions {TwilioVerifyApiKey = "azerty "},
                httpClient
            );
        }

        [Fact]
        public async Task SendCodeShouldReturnSendCodeResponse()
        {
            _messageHandler.Response = GetResponseFromFile("ValidSendCodeResponse.json", HttpStatusCode.OK);
            var result = await _client.SendCode("+33781643710", 33);
            
            Assert.IsType<SendCodeResponse>(result);
        }
        
        [Fact]
        public async Task SendCodeShouldThrowExceptionCauseInternalError()
        {
            _messageHandler.Response = GetResponseFromFile("InternalError.json", HttpStatusCode.InternalServerError);
            await Assert.ThrowsAsync<SendCodeException>(() => _client.SendCode("+33781643710", 33));
        }
        
        [Fact]
        public async Task VerifyCodeShouldReturnVerifyCodeResponse()
        {
            _messageHandler.Response = GetResponseFromFile("ValidVerifyCodeResponse.json", HttpStatusCode.OK);
            var result = await _client.VerifyCode("+33781643710", "6133", 33);
            
            Assert.IsType<VerifyCodeResponse>(result);
        }
        
        [Fact]
        public async Task VerifyCodeShouldThrowExceptionCauseNoPendingVerification()
        {
            _messageHandler.Response = GetResponseFromFile("NoPendingVerificationCodeResponse.json", HttpStatusCode.NotFound);
            await Assert.ThrowsAsync<NoPendingVerificationException>(() => _client.VerifyCode("+33781643710", "6133", 33));
        }
        
        [Fact]
        public async Task VerifyCodeShouldThrowExceptionCauseCodeIsIncorrect()
        {
            _messageHandler.Response = GetResponseFromFile("IncorrectVerificationCodeResponse.json", HttpStatusCode.Unauthorized);
            await Assert.ThrowsAsync<IncorrectVerificationCodeException>(() => _client.VerifyCode("+33781643710", "6133", 33));
        }
        
        [Fact]
        public async Task VerifyCodeShouldThrowExceptionCauseInternalError()
        {
            _messageHandler.Response = GetResponseFromFile("InternalError.json", HttpStatusCode.InternalServerError);
            await Assert.ThrowsAsync<VerificationCodeException>(() => _client.VerifyCode("+33781643710", "6133", 33));
        }
        
        private HttpResponseMessage GetResponseFromFile(string file, HttpStatusCode statusCode)
        {
            var assembly = GetType().GetTypeInfo().Assembly;
            var response = new HttpResponseMessage();
            var resources = assembly.GetManifestResourceNames();
            var resourceName = resources.FirstOrDefault(f => f.Equals($"PhoneVerification.Tests.JsonFiles.{file}" , StringComparison.OrdinalIgnoreCase));
            var json = "";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }

            response.StatusCode = statusCode;
            response.Content = new StringContent(json);
            
            return response;
        }
    }
}