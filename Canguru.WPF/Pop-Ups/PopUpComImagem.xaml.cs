using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Canguru.WPF.Pop_Ups
{
    public partial class PopUpComImagem : Window
    {
        public bool Resultado { get; private set; }

        public PopUpComImagem(string titulo, string mensagem, string caminhoImagem)
        {
            InitializeComponent();

            txtTitulo.Text = titulo;
            txtConteudo.Text = mensagem;
            CarregarImagem(caminhoImagem);
        }

        private void CarregarImagem(string caminho)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(caminho, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                imgNotificacao.Source = bitmap;
            }
            catch
            {
                imgNotificacao.Source = null;
            }
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            Resultado = true;
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Resultado = false;
            Close();
        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e)
        {
            Resultado = false;
            Close();
        }
    }
}
