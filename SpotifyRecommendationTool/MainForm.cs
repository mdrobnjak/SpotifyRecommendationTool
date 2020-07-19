using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;

namespace SpotifyRecommendationTool
{
    public partial class MainForm : Form
    {
        const int RecommendationsPerRequest = 100;

        SpotifyWebAPI api;
        AuthorizationCodeAuth auth;

        public MainForm()
        {
            InitializeComponent();

            InitControls();

            InitDgvForTrackData(ref dgvSeeds);
            InitDgvForTrackData(ref dgvRecos);
            InitDgvForTrackData(ref dgvTuneable);
        }

        #region Spotify API

        /// <summary>
        /// Prompts the user to authorize the application to access data.
        /// </summary>
        public void UserAuthorize()
        {
            auth = new AuthorizationCodeAuth(
              "b0e7bb6bcf43430181cad104ada6477c",
              "164fcb9dd29f4d588141668ba7683f3a",
              "http://localhost:8888/callback",
              "http://localhost:8888/callback",
              Scope.PlaylistModifyPublic);

            SimpleHTTPServer.RequestReceived += async (sender, payload) =>
            {
                auth.Stop();
                var code = System.Web.HttpUtility.ParseQueryString(payload.Context.Request.RawUrl).GetValues("/callback?code")[0];
                Token token = await auth.ExchangeCode(code);
                api = new SpotifyWebAPI()
                {
                    TokenType = token.TokenType,
                    AccessToken = token.AccessToken
                };
                // Do requests with API client
                Console.WriteLine("Authentication executed...");
            };

            auth.Start(); // Starts an internal HTTP Server
            Console.WriteLine("Started internal HTTP server...");
            auth.OpenBrowser();
            Console.WriteLine("Opened browser...");
        }

        /// <summary>
        /// Returns after user authentication and API initialization.
        /// </summary>
        /// <returns></returns>
        private async Task WaitForAuthentication()
        {
            while (api == null)
            {
                await Task.Delay(1000);
            }
        }

        #endregion

        private void InitControls()
        {
            btnShowRecos.BackColor = Color.LightGreen;
        }

        /// <summary>
        /// Initialize the DataGridView to display track data. The ValueType of each column is stored in the Tag property.
        /// </summary>
        /// <param name="dgv"></param>
        private void InitDgvForTrackData(ref DataGridView dgv)
        {
            dgv.Font = new Font("Courier New", 12, FontStyle.Bold);
            dgv.AllowUserToResizeColumns = true;
            dgv.AllowUserToOrderColumns = true;

            dgv.ColumnHeadersVisible = true;

            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = dgv.Font;
            dgv.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Title", Tag = "".GetType() });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Artist", Tag = "".GetType() });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Genre", Tag = "".GetType() });

            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Tempo", Tag = new float().GetType() });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Key", Tag = "".GetType() });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Duration", Tag = "".GetType() });

            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Popularity", Tag = new int().GetType() });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Release", Tag = "".GetType() });

            //https://developer.spotify.com/documentation/web-api/reference/tracks/get-audio-features/
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Acousticness", Tag = new float().GetType() });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Danceability", Tag = new float().GetType() });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Energy", Tag = new float().GetType() });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Instrumentalness", Tag = new float().GetType() });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Liveness", Tag = new float().GetType() });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Loudness", Tag = new float().GetType() });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Speechiness", Tag = new float().GetType() });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Valence", Tag = new float().GetType() });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Analysis_URL", Tag = "".GetType() });

            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "IsDataFromSpotify", Tag = new bool().GetType(), Visible = false });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "TrackId", Tag = "".GetType(), Visible = false });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "ArtistId", Tag = "".GetType(), Visible = false });
        }
        
        private async void btnShowRecos_Click(object sender, EventArgs e)
        {
            btnShowRecos.Enabled = false;

            if (auth == null)
            {
                SimpleHTTPServer.Start();
                UserAuthorize();
                await WaitForAuthentication();
            }

            GetTrackAttributes(dgvSeeds);
            GetAudioFeatures(dgvSeeds);
            dgvSeeds.Refresh();

            dgvRecos.Rows.Clear();
            GetRecommendations(dgvSeeds, dgvRecos);
            GetAudioFeatures(dgvRecos);

            btnShowRecos.Enabled = true;
        }

        private void GetTrackAttributes(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) { continue; }

                Dictionary<string, dynamic> d = row.ToDict();

                if (d["IsDataFromSpotify"] == false)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        var columnType = (Type)cell.DataGridView.Columns[cell.ColumnIndex].Tag;

                        cell.Value = columnType.Name == "String" ? "" : Activator.CreateInstance(columnType);
                    }

                    if (!String.IsNullOrWhiteSpace(d["Title"]))
                    {
                        SearchItem searchItem = api.SearchItems(d["Title"] + " " + d["Artist"], SearchType.Track);
                        if (searchItem.Tracks.Items.Count > 0)
                        {
                            FullTrack topTrack = searchItem.Tracks.Items[0];
                            row.Cells["Title"].Value = topTrack.Name;
                            row.Cells["Artist"].Value = topTrack.Artists[0].Name;
                            row.Cells["TrackId"].Value = topTrack.Id;
                            row.Cells["Popularity"].Value = topTrack.Popularity;
                            row.Cells["Release"].Value = topTrack.Album.ReleaseDate;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (!String.IsNullOrWhiteSpace(d["Artist"]))
                    {
                        FullArtist topArtist = api.SearchItems(d["Artist"], SearchType.Artist).Artists.Items[0];
                        row.Cells["Artist"].Value = topArtist.Name;
                        row.Cells["ArtistId"].Value = topArtist.Id;
                    }
                    else if (!String.IsNullOrWhiteSpace(d["Genre"]))
                    {
                        string formattedGenre = d["Genre"].Replace(" ", "-").ToLower();
                        row.Cells["Genre"].Value = api.GetRecommendationSeedsGenres().Genres.Contains(formattedGenre) ? formattedGenre : "";
                    }

                    row.Cells["IsDataFromSpotify"].Value = true;
                }
                else if (d["IsDataFromSpotify"] == true)
                {
                    continue;
                }
            }
        }
        
        private async void GetAudioFeatures(DataGridView dgv)
        {
            List<string> ids = dgv.Columns["TrackId"].GetUniqueValues();

            if (ids.Count == 0)
            {
                return;
            }

            SeveralAudioFeatures severalAudioFeatures = await api.GetSeveralAudioFeaturesAsync(ids);

            foreach (AudioFeatures audioFeatures in severalAudioFeatures.AudioFeatures)
            {
                DataGridViewRow row = dgv.Rows
                                        .Cast<DataGridViewRow>()
                                        .Where(r => r.Cells["TrackId"].FormattedValue.ToString().Equals(audioFeatures.Id))
                                        .First();

                row.Cells["Tempo"].Value = audioFeatures.Tempo;
                row.Cells["Key"].Value = Key.ConvertToString(audioFeatures.Key, audioFeatures.Mode);

                TimeSpan t = TimeSpan.FromMilliseconds(audioFeatures.DurationMs);
                row.Cells["Duration"].Value = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);

                row.Cells["Acousticness"].Value = audioFeatures.Acousticness;
                row.Cells["Danceability"].Value = audioFeatures.Danceability;
                row.Cells["Energy"].Value = audioFeatures.Energy;
                row.Cells["Instrumentalness"].Value = audioFeatures.Instrumentalness;
                row.Cells["Liveness"].Value = audioFeatures.Liveness;
                row.Cells["Loudness"].Value = audioFeatures.Loudness;
                row.Cells["Speechiness"].Value = audioFeatures.Speechiness;
                row.Cells["Valence"].Value = audioFeatures.Valence;
                row.Cells["Analysis_URL"].Value = audioFeatures.AnalysisUrl;
            }
        }
        
        public void GetRecommendations(DataGridView dgvSeeds, DataGridView dgvRecos)
        {
            List<string> artistSeeds = dgvSeeds.Columns["ArtistId"].GetUniqueValues();
            List<string> genreSeeds = dgvSeeds.Columns["Genre"].GetUniqueValues();
            List<string> trackSeeds = dgvSeeds.Columns["TrackId"].GetUniqueValues();

            if (artistSeeds.Count + genreSeeds.Count + trackSeeds.Count == 0)
            {
                return;
            }

            TuneableTrack target = null;

            if (dgvTuneable.Rows.Count > 0)
            {
                Dictionary<string,dynamic> d = dgvTuneable.Rows[0].ToDict();
                
                target = new TuneableTrack()
                {
                    //Acousticness = d["Acousticness"],
                    //Danceability = d["Danceability"],
                    //Energy = d["Energy"],
                    //Instrumentalness = d["Instrumentalness"],
                    Key = String.IsNullOrWhiteSpace(d["Key"]) ? null : Key.ConvertToInt(d["Key"]).Item1,
                    //Liveness = d["Liveness"],
                    //Loudness = d["Loudness"],
                    Mode = String.IsNullOrWhiteSpace(d["Key"]) ? null : Key.ConvertToInt(d["Key"]).Item2,
                    //Popularity = d["Popularity"],
                    //Speechiness = d["Speechiness"],
                    Tempo = d["Tempo"],
                    //Valence = d["Valence"]
                };
            }

            Recommendations recos = api.GetRecommendations(
                artistSeed: artistSeeds,
                genreSeed: genreSeeds,
                trackSeed: trackSeeds,
                target: target,
                min: null,
                max: null,
                limit: RecommendationsPerRequest);

            foreach (var track in recos.Tracks)
            {
                dgvRecos.Rows.Add(track.Name);

                DataGridViewRow newRow = dgvRecos.Rows
                                            .Cast<DataGridViewRow>()
                                            .Where(r => r.Cells["Title"].Value.ToString() == track.Name)
                                            .First();

                foreach (DataGridViewColumn column in dgvRecos.Columns)
                {
                    newRow.Cells[column.Name].ValueType = (Type)column.Tag;
                }

                FullTrack fullTrack = api.GetTrack(track.Id);

                newRow.Cells["Title"].Value = track.Name;
                newRow.Cells["Artist"].Value = track.Artists[0].Name;
                newRow.Cells["TrackId"].Value = track.Id;
                newRow.Cells["Popularity"].Value = fullTrack.Popularity;
                newRow.Cells["Release"].Value = fullTrack.Album.ReleaseDate;
            }
        }
    }
}
