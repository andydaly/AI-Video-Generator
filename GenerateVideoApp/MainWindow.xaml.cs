using Microsoft.Extensions.Configuration;
using System.Windows;


namespace GenerateVideoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AIVideoGenerator AIVideoGenerator;

        public MainWindow()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            string endpoint = configuration["ApiSettings:Endpoint"];
            string apiKey = configuration["ApiSettings:ApiKey"];
            AIVideoGenerator = new AIVideoGenerator(
                endpoint,
                apiKey,
                "preview");

            InitializeComponent();
            
        }

        private async void GenerateVideo_Click(object sender, RoutedEventArgs e)
        {
            
            string loadingIconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "media", "loadingicon.mp4");
            VideoPlayer.Source = new Uri(loadingIconPath, UriKind.Absolute);
            VideoPlayer.Play();

            string prompt = PromptInput.Text;
            if (string.IsNullOrWhiteSpace(prompt))
            {
                MessageBox.Show("Please enter a prompt to generate the video.");
                return;
            }

            string VideoFilePath = await AIVideoGenerator.GenerateVideoAsync(prompt, 10);


            VideoPlayer.Source = new Uri(VideoFilePath, UriKind.Absolute);
            VideoPlayer.Play();
            
        }

        private void VideoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Position = TimeSpan.Zero;
            VideoPlayer.Play();
        }
    }
}