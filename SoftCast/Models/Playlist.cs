﻿using System.Text.Json.Serialization;

namespace SoftCast.Models
{
    public class Playlist
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int UsuarioID { get; set; }

        [JsonIgnore]
        public Usuario Usuario { get; set; }

        [JsonIgnore]
        public List<Conteudo> Conteudos { get; set; }
    }
}