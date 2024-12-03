using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Timers;
using Flappy_Pony;


namespace Flappy_Pony
{
    public partial class MainPage : ContentPage
    {
        private double _gravity = 0.5;
        private double _obstacleSpeed = 5;
        private List<BoxView> _obstacles = new List<BoxView>();
        private Random _random = new Random();
        private double _jumpForce = 10;
        private double _velocity = 0;

        public MainPage()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnScreenTapped;
            PonyImage.GestureRecognizers.Add(tapGesture);

            var timer = new Timer(20);
            timer.Elapsed += OnGameTick;
            timer.Start();

            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                CreateObstacle();
                return true;
            });
        }

        private void OnScreenTapped(object sender, EventArgs e)
        {
            if (PonyImage.TranslationY <= 0)
            {
                _velocity = _jumpForce;
            }
        }

        private void OnGameTick(object sender, ElapsedEventArgs e)
        {
            _velocity -= _gravity;
            PonyImage.TranslationY -= _velocity;

            if (PonyImage.TranslationY < 0)
            {
                PonyImage.TranslationY = 0;
                _velocity = 0;
            }

            UpdateObstacles();
            CheckCollisions();
        }

        private void UpdateObstacles()
        {
            for (int i = _obstacles.Count - 1; i >= 0; i--)
            {
                var obstacle = _obstacles[i];
                obstacle.TranslationX -= _obstacleSpeed;

                if (obstacle.TranslationX < -50)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        GameLayout.Children.Remove(obstacle);
                        _obstacles.RemoveAt(i);
                    });
                }
            }
        }

        private void CheckCollisions()
        {
            foreach (var obstacle in _obstacles)
            {
                var obstacleBounds = AbsoluteLayout.GetLayoutBounds(obstacle);
                var ponyBounds = AbsoluteLayout.GetLayoutBounds(PonyImage);

                if (obstacleBounds.IntersectsWith(ponyBounds))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("Game Over", "El pony chocó con un obstáculo", "Reiniciar")
                            .ContinueWith(t => ResetGame());
                    });
                    break;
                }
            }
        }

        private void CreateObstacle()
        {
            var obstacle = new BoxView
            {
                WidthRequest = 50,
                HeightRequest = _random.Next(100, 300),
                Color = Color.Green,
                TranslationX = GameLayout.Width,
                TranslationY = GameLayout.Height - _random.Next(100, 300)
            };

            GameLayout.Children.Add(obstacle);
            _obstacles.Add(obstacle);
        }

        private void ResetGame()
        {
            // Reiniciar la posición del pony y limpiar obstáculos
            PonyImage.TranslationY = 0;
            _velocity = 0;

            foreach (var obstacle in _obstacles)
            {
                Device.BeginInvokeOnMainThread(() => GameLayout.Children.Remove(obstacle));
            }

            _obstacles.Clear();
        }
    }
}