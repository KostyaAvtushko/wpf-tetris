using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Tetris_game.Models;

namespace Tetris_game.ViewModels
{
    public class AuthViewModel
    {
        public ICommand SwapCommand { get; set; }
        public ICommand AuthCommand { get; set; }

        public AuthViewModel()
        {
            SwapCommand = new RelayCommand(Swap, CanSwap);
            AuthCommand = new RelayCommand(Auth, CanAuth);
        }

        private bool CanAuth(object obj)
        {
            return true;
        }


        private void Auth(object obj)
        {
            var values = (object[])obj;
            if (values[5].Equals(Visibility.Visible))
            {
                if (
                    values[0] is string logInName &&
                     values[1] is string logInPassword
                    )
                 {
                string player = Player.GetFromDB(logInName, logInPassword);
                if (player != null)
                {
                    MainWindow game = new MainWindow();
                    game.Show();
                } 
                else
                {

                }

                }
            } else
            {
                if (
                    values[2] is string signUpName &&
                    values[3] is string signUpPassword &&
                    values[4] is string signUpPassoword
                    )
                {
                    Player user = new Player(signUpName, signUpPassword, 0);
                    user.Save();
                }
            }
        }

        private bool CanSwap(object obj)
        {
            return true;
        }

        private void Swap(object obj)
        {
            var values = (object[])obj;
            if (
                values[0] is FrameworkElement elementToHide &&
                values[1] is FrameworkElement elementToShow
                )
            {
                elementToHide.Visibility = Visibility.Hidden;
                elementToShow.Visibility = Visibility.Visible;
            }
        }
    }
}
