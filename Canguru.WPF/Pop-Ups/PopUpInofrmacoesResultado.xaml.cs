using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Canguru.WPF.Pop_Ups
{
    public partial class PopUpInofrmacoesResultado : Window
    {
        public PopUpInofrmacoesResultado()
        {
            InitializeComponent();
        }
        public PopUpInofrmacoesResultado(string titulo, string mensagem, string caminhoImagem = null)
            : this()
        {
            txtTitulo.Text = titulo ?? string.Empty;
            txtConteudo.Text = mensagem ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(caminhoImagem))
                CarregarImagem(caminhoImagem);
        }

        private void CarregarImagem(string caminhoImagem)
        {
            try
            {
              
                string caminhoPossivel = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, caminhoImagem);

                BitmapImage bmp = new BitmapImage();

                if (File.Exists(caminhoPossivel))
                {
                    bmp.BeginInit();
                    bmp.UriSource = new Uri(caminhoPossivel, UriKind.Absolute);
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.EndInit();
                    imgPopup.Source = bmp;
                    return;
                }
                try
                {
                    var uriPack = new Uri($"pack://application:,,,/{caminhoImagem}", UriKind.Absolute);
                    bmp = new BitmapImage(uriPack);
                    imgPopup.Source = bmp;
                    return;
                }
                catch
                {
                    imgPopup.Source = null;
                }
            }
            catch
            {
                imgPopup.Source = null;
            }
        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
