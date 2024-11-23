using System.Text.Json.Serialization;

namespace SoftCast.Models
{
    public class PlaylistEdit
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int UsuarioID { get; set; }
    }
}
