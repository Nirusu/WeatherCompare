using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Timers;
using WeatherCompare.Properties;

namespace WeatherCompare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NetatmoSession session = new NetatmoSession();
        private Boolean isRefreshRunning = false;
        private Timer refreshTimer = new Timer();
        private int refreshTimerTicks = 0;
        public MainWindow()
        {
            InitializeComponent();
            refreshTimer.Elapsed += new ElapsedEventHandler(OnRefreshTimedEvent);
            refreshTimer.Interval = 1000;
            Refresh();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private async void Refresh()
        {
            if (!isRefreshRunning)
            {
                isRefreshRunning = true;
                APIResponse[] apiResponse = { null, null };
                DateTimeOffset dto = new DateTimeOffset();
                dto = DateTime.UtcNow;
                if (Settings.Default.refresh_token.Equals(""))
                {
                    windowMain.lblStatus.Content = "Rufe Token vom Server ab...";
                    //TODO: replace with SecureString and/or Windows Credential Manager
                    AccessData accessdata = await session.GetAccessTokenFromPassword();
                    Settings.Default.access_token = accessdata.AccessToken;
                    Settings.Default.refresh_token = accessdata.RefreshToken;
                    Settings.Default.tokenExpiresIn = dto.ToUnixTimeSeconds() + accessdata.ExpiresIn;
                    Settings.Default.Save();
                }
                else if (Settings.Default.tokenExpiresIn <= dto.ToUnixTimeSeconds() + 600) // refresh token 10 minutes earlier than it runs out due to possible differences from local time and server time
                {
                    windowMain.lblStatus.Content = "Erneuere Token...";
                    AccessData accessdata = await session.RefreshToken();
                    Settings.Default.access_token = accessdata.AccessToken;
                    Settings.Default.refresh_token = accessdata.RefreshToken;
                    Settings.Default.tokenExpiresIn = dto.ToUnixTimeSeconds() + accessdata.ExpiresIn;
                    Settings.Default.Save();
                }
                windowMain.lblStatus.Content = "Rufe Wetterdaten ab...";
                windowMain.lblStationName1.Content = "London"; // HARDCODED
                windowMain.lblRealStationName1.Content = "Westminster"; // HARDCODED
                windowMain.lblStationName2.Content = "Los Angeles"; // HARDCODED
                windowMain.lblRealStationName2.Content = "W 1st St"; // HARDCODED
                try
                {
                    apiResponse[0] = await session.RequestPublicWeatherStationData(Settings.Default.access_token, 51.50350694, -0.12597799, 51.4909232, -0.14267206); // Westminster - HARDCODED
                    apiResponse[1] = await session.RequestPublicWeatherStationData(Settings.Default.access_token, 34.05422389, -118.22258949, 34.03800893, -118.27237129); // Los Angeles - HARDCODED
                }
                catch
                {
                    windowMain.lblStatus.Content = "Beim Abrufen der Daten ist ein Fehler aufgetreten!";
                    isRefreshRunning = false;
                    return;
                }
                try
                {
                    windowMain.lblTemperature1.Content = ProcessWeatherData(apiResponse[0], "70:ee:50:01:f7:00") + "°C"; // HARDCODED - demo station
                }
                catch (CorrectModuleNotFoundException)
                {
                    windowMain.lblRealStationName1.Content = "Temperatur konnte nicht ausgelesen werden";
                }
                catch (StationNotFoundException)
                {
                    windowMain.lblRealStationName1.Content = "Station konnte nicht gefunden werden.";
                }
                try
                {
                    windowMain.lblTemperature2.Content = ProcessWeatherData(apiResponse[1], "70:ee:50:20:bf:36") + "°C"; // HARDCODED - demo station
                }
                catch (CorrectModuleNotFoundException)
                {
                    windowMain.lblRealStationName2.Content = "Temperatur konnte nicht ausgelesen werden";
                }
                catch (StationNotFoundException)
                {
                    windowMain.lblRealStationName2.Content = "Station konnte nicht gefunden werden.";
                }
                isRefreshRunning = false;
                windowMain.lblStatus.Content = "Fertig!";
            }
            windowMain.btnRefresh.Content = "Aktualisieren (5)";
            refreshTimer.Enabled = true;
        }

        private String ProcessWeatherData(APIResponse apiResponse, String requestedMacAddress)
        {
            Boolean foundCorrectModule = false;
            foreach (Body b in apiResponse.Body)
            {
                if (b.StationID.Equals(requestedMacAddress)) // check Station ID
                {
                    foreach (KeyValuePair<string, SingleModule> kvp in b.Measures) // check measures of station
                    {
                        if (kvp.Value.TypeOfModule != null && kvp.Value.TypeOfModule.Any("temperature".Contains)) // since there are multiple measure data streams, check if the current selected stream contains temperature
                        {
                            foreach (KeyValuePair<string, double[]> kvpMeasuredData in kvp.Value.MeasuredData) // read temperature out of temperature & humidity data
                            {
                                foundCorrectModule = true;
                                return kvpMeasuredData.Value[0].ToString();
                            }
                        }
                    }
                    if (!foundCorrectModule)
                    {
                        throw new CorrectModuleNotFoundException("The requested station was found, however temperature data from this station wasn't found.");
                    }

                }
            }
            throw new StationNotFoundException("The requested station couldn't be found.");
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            btnRefresh.IsEnabled = false;
            Refresh();
        }
        private void OnRefreshTimedEvent(object source, ElapsedEventArgs e)
        {
            if (refreshTimerTicks < 4)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    refreshTimerTicks++;
                    btnRefresh.Content = "Aktualisieren (" + (5 - refreshTimerTicks) + ")";
                }));

            }
            else
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    btnRefresh.Content = "Aktualisieren";
                    btnRefresh.IsEnabled = true;
                    refreshTimerTicks = 0;
                    refreshTimer.Enabled = false;
                }));

            }
        }
    }
    public class StationNotFoundException : Exception
    {
        public StationNotFoundException(string Message) : base(Message)
        {

        }
    }
    public class CorrectModuleNotFoundException : Exception
    {
        public CorrectModuleNotFoundException(string Message) : base(Message)
        {

        }
    }
}

