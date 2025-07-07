using AuraPodcastAvaloniaUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibVLCSharp.Shared;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AuraPodcastAvaloniaUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public LibVLC LibVLC { get; }

    [ObservableProperty]
    private MediaPlayer? _mediaPlayer;

    [ObservableProperty]
    private string _rssUrl = "https://feeds.simplecast.com/gvtxUiIf"; // For testing

    [ObservableProperty]
    private PodcastEpisode? _selectedEpisode;

    public ObservableCollection<PodcastEpisode> Episodes { get; } = new();

    public MainWindowViewModel()
    {
        // Init LibVLC
        Core.Initialize();
        LibVLC = new LibVLC();
    }

    public ObservableCollection<WeatherForecast> Forecasts { get; } = new();

    [RelayCommand]
    private async Task LoadFeedAsync()
    {
        if (string.IsNullOrWhiteSpace(RssUrl)) return;

        Episodes.Clear();
        SelectedEpisode = null; 
        MediaPlayer?.Stop();    
        MediaPlayer?.Dispose(); 
        MediaPlayer = null;

        try
        {
            var apiUrl = $"https://localhost:7094/api/podcast?url={Uri.EscapeDataString(RssUrl)}";
            var episodes = await _httpClient.GetFromJsonAsync<PodcastEpisode[]>(apiUrl);

            if (episodes != null)
            {
                foreach (var episode in episodes)
                {
                    Episodes.Add(episode);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading feed: {ex.Message}");
        }
    }

    [RelayCommand]
    private void PlayEpisode(PodcastEpisode episode)
    {
        if (string.IsNullOrWhiteSpace(episode?.AudioUrl)) return;

        SelectedEpisode = episode;

        MediaPlayer?.Stop();
        MediaPlayer?.Dispose();

        MediaPlayer = new MediaPlayer(LibVLC)
        {
            Media = new Media(LibVLC, new Uri(episode.AudioUrl))
        };

        MediaPlayer.Play();
    }
}