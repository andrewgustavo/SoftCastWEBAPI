using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftCast.Models
{
    [PrimaryKey(nameof(PlaylistID), nameof(ConteudoID))]
    public class ItemPlaylist
    {
        public int PlaylistID { get; set; }
        public int ConteudoID { get; set; }

        // Propriedades de navegação, se aplicável
        public Playlist Playlist { get; set; }
        public Conteudo Conteudo { get; set; }
    }
}
