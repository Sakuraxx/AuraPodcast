using AuraPodcastAvaloniaUI.Models;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AuraPodcastAvaloniaUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public ObservableCollection<WeatherForecast> Forecasts { get; } = new();

    [RelayCommand]
    private async Task LoadForecastsAsync()
    {
        Forecasts.Clear();

        try
        {
            var apiUrl = "https://localhost:7094/WeatherForecast";

            var forecasts = await _httpClient.GetFromJsonAsync<WeatherForecast[]>(apiUrl);

            if (forecasts != null)
            {
                foreach (var forecast in forecasts)
                {
                    Forecasts.Add(forecast);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching data: {ex.Message}");
        }
    }
}