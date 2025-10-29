using Microsoft.Win32;     
using System.Windows.Media.Imaging;
using System;
using System.Windows;
using Canguru.Core;
using Canguru.Business;
using System;
namespace Canguru.WPF
{

    public partial class CadastroWindow : Window
    {
        private string caminhoImagemPerfil = null;
        public CadastroWindow()
        {
            InitializeComponent();
        }
        private void BtnEscolherFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Arquivos de Imagem (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|Todos os Arquivos (*.*)|*.*";

            // 3. Define o título da janela
            openFileDialog.Title = "Selecione uma Foto de Perfil";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                  
                    string filePath = openFileDialog.FileName;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();         
                    bitmap.UriSource = new Uri(filePath); 
                    bitmap.EndInit();           


                    imgPerfil.Source = bitmap;

                    caminhoImagemPerfil = filePath;
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Erro ao carregar a imagem: {ex.Message}", "Erro de Imagem", MessageBoxButton.OK, MessageBoxImage.Error);
                    caminhoImagemPerfil = null; 
                    imgPerfil.Source = null;    
                }
            }
        }
        private void BtnSalvarCadastro_Click(object sender, RoutedEventArgs e)
        {
            // ... Sua lógica de salvar cadastro ...
            // Ao salvar, você pode usar a variável 'caminhoImagemPerfil' 
            // para saber qual foto o usuário escolheu e salvá-la 
            // (copiando o arquivo, salvando no banco, etc.)
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow TelaLogin = new LoginWindow();
            this.Close();
            TelaLogin.ShowDialog();
        }
    }
}
