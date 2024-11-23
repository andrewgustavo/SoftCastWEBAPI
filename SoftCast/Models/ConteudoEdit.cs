using System.Text.Json.Serialization;

namespace SoftCast.Models
{
    public class ConteudoEdit
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public string ClassificacaoIndicativa { get; set; }
        public string VideoPath { get; set; }
        public int CriadorID { get; set; }
    }
}