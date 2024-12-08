using BlazorApp1.Models;

namespace BlazorApp1.Service
{
    public class EmailService
    {
        private readonly HttpClient _httpClient;
        private const string EMAIL_API_URL = "/api/EmailSender";
        private string otp;

        public EmailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GenerateOTPAsync(int length = 6)
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task SendOTPByEmailAsync(string email)
        {
            otp = await GenerateOTPAsync();
            var emailData = new
            {
                To = email,
                Subject = "OTP Verification",
                Body = $"Your OTP for verification is: {otp}"
            };

            var response = await _httpClient.PostAsJsonAsync(EMAIL_API_URL, emailData);
            response.EnsureSuccessStatusCode();
        }

        public async Task<VerifyResult> VerifyOTPAsync(string enteredOTP)
        {
            if (!string.IsNullOrEmpty(otp) && otp == enteredOTP)
            {
                otp = "";
                return new VerifyResult { IsValid = true, Message = "OTP verified successfully." };
            }
            else
            {
                return new VerifyResult { IsValid = false, Message = "Invalid OTP. Please try again." };
            }
        }
    }
}
