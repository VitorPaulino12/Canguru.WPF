using System;

namespace Canguru.WPF
{
    public class FeedPost
    {
        public string Titulo { get; set; }
        public bool FoiConcluido { get; set; }
        public DateTime DataPostagem { get; set; }
        public string DataFormatada => DataPostagem.ToString("dd/MM/yyyy");
    }
}