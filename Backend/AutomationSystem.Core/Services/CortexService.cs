using AutomationSystem.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace AutomationSystem.Core.Services;

public class CortexService : ICortexService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CortexService> _logger;
    private readonly IConfiguration _configuration;

    public CortexService(HttpClient httpClient, ILogger<CortexService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;

        var baseUrl = _configuration["Cortex:BaseUrl"] ?? "https://cortex.amazon.com/api";
        var apiKey = _configuration["Cortex:ApiKey"] ?? "";

        _httpClient.BaseAddress = new Uri(baseUrl);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    public async Task<List<TourData>> GetToursAsync(DateTime date)
    {
        try
        {
            var dateStr = date.ToString("yyyy-MM-dd");
            var response = await _httpClient.GetAsync($"/tours?date={dateStr}");
            
            response.EnsureSuccessStatusCode();

            var tours = await response.Content.ReadFromJsonAsync<List<TourData>>() ?? new List<TourData>();

            _logger.LogInformation($"Retrieved {tours.Count} tours for date {dateStr}");

            return tours;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving tours for date {date:yyyy-MM-dd}");
            throw;
        }
    }

    public async Task<TourData?> GetTourByIdAsync(string tourId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/tours/{tourId}");
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Tour {tourId} not found");
                return null;
            }

            var tour = await response.Content.ReadFromJsonAsync<TourData>();

            _logger.LogInformation($"Retrieved tour {tourId}");

            return tour;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving tour {tourId}");
            throw;
        }
    }

    public async Task<bool> UpdateTourStatusAsync(string tourId, string status)
    {
        try
        {
            var payload = new { status };
            var response = await _httpClient.PutAsJsonAsync($"/tours/{tourId}/status", payload);
            
            response.EnsureSuccessStatusCode();

            _logger.LogInformation($"Updated tour {tourId} status to {status}");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating tour {tourId} status");
            return false;
        }
    }
}

