using System;

namespace RandomCssMaui
{
    public partial class App : Application
    {
        public int LuckyNumber { get; }

        public App()
        {
            InitializeComponent();

            var rnd = new Random();
            LuckyNumber = rnd.Next(1, 31);

            MainPage = new AppShell();
        }
    }
}