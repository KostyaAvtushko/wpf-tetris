using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Xps.Serialization;

namespace Tetris_game.Models
{
    public class Player
    {
        [JsonIgnore]
        public int Score { get; set; }
        public int Rank { get; set; }

        private string _name = String.Empty;
        private string _password = String.Empty;

        public Player(string name, string password, int score)
        {
            _name = name;
            _password = password;
            Score = score;
        }

        public string Name { get => _name; set => SetField(ref _name, value); }
        public string Password { get => _password; set => SetField(ref _password, value); }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void Save()
        {
            try
            {
                var player = new Player(Name, Password, 0);
                string json = JsonSerializer.Serialize(player);
                File.AppendAllLines("C:\\db\\db.txt", new[] { json });
            }
            catch { }
        }

        public static string GetFromDB(string name, string password)
        {
            try
            {
                StreamReader streamReader = new StreamReader("C:\\db\\db.txt");
                string data = streamReader.ReadLine();
                while (data != null)
                {
                    var p = JsonSerializer.Deserialize<Player>(data);
                    //string nameJson = (string)JsonSerializer.Deserialize<Dictionary<string, object>> (data)["Name"];
                    //string passJson = (string)JsonSerializer.Deserialize<Dictionary<string, object>>(data)["Password"];

                    if (
                        p.Name == name &&
                        p.Password == password
                        )
                    {
                        return p.Name;
                    }
                    data = streamReader.ReadLine();
                }
            }
            catch { }


            return null;
        }

    }
}
