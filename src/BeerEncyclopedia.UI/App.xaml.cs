namespace BeerEncyclopedia.UI
{
    public partial class App : IApplication
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
    }
}