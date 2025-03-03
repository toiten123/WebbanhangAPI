using System;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuanLyBanHang.Services
{
    public class TelegramBot
    {
        private readonly string _botToken;
        private readonly string _chatId;
        private readonly HttpClient _httpClient;

        public TelegramBot(string botToken, string chatId)
        {
            _botToken = botToken;
            _chatId = chatId;
            _httpClient = new HttpClient();
        }

        public async Task SendMessageAsync(string message)
        {
            var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
            var payload = new { chat_id = _chatId, text = message };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Lỗi Gửi Tin Nhắn => {response.ReasonPhrase}");
            }
        }

        public async Task SendPhotoAsync(string photoUrl, string caption = "")
        {
            var url = $"https://api.telegram.org/bot{_botToken}/sendPhoto";
            var payload = new { chat_id = _chatId, photo = photoUrl, caption = caption };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Lỗi Gửi Hình Ảnh => {response.ReasonPhrase}");
            }
        }

        public async Task SendDocumentAsync(string documentUrl, string caption = "")
        {
            var url = $"https://api.telegram.org/bot{_botToken}/sendDocument";
            var payload = new { chat_id = _chatId, document = documentUrl, caption = caption };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Lỗi Gửi Tài Liệu => {response.ReasonPhrase}");
            }
        }

        public async Task SendAudioAsync(string audioUrl, string caption = "")
        {
            var url = $"https://api.telegram.org/bot{_botToken}/sendAudio";
            var payload = new { chat_id = _chatId, audio = audioUrl, caption = caption };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Lỗi Gửi Âm Thanh => {response.ReasonPhrase}");
            }
        }

        public async Task SendVideoAsync(string videoUrl, string caption = "")
        {
            var url = $"https://api.telegram.org/bot{_botToken}/sendVideo";
            var payload = new { chat_id = _chatId, video = videoUrl, caption = caption };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Lỗi Gửi Video => {response.ReasonPhrase}");
            }
        }
    }
}
