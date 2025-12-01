using Canguru.Business;
using Canguru.WPF.Pop_Ups;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Canguru.WPF
{
    public partial class CadastroWindow : Window
    {
        private string caminhoImagemPerfil = null;

        public CadastroWindow()
        {
            InitializeComponent();
        }

        private void txtEmail_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private void BtnEscolherFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imagens|*.png;*.jpg;*.jpeg;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(filePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    FotoBrush.ImageSource = bitmap;
                    caminhoImagemPerfil = filePath;
                }
                catch (Exception)
                {
                    var PopUp = new PopUpsGerais(7);
                    PopUp.ShowDialog();
                }
            }
        }

        private void BtnSalvarCadastro_Click(object sender, RoutedEventArgs e)
        {
            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();
            string ra = txtRA.Text.Trim();
            string senha = txtSenhaCadastro.Password;
            string confSenha = txtConfirmarSenha.Password;

            // Validações básicas
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(ra) || string.IsNullOrWhiteSpace(senha))
            {
                var PopUp = new PopUpsGerais(8);
                PopUp.ShowDialog();
                return;
            }

            if (!email.Contains("@") || !email.ToLower().Contains(".com"))
            {
                var PopUp = new PopUpsGerais(9);
                PopUp.ShowDialog();
                txtEmail.Focus();
                return;
            }

            // VALIDAÇÃO DO RA (1 ou 2)
            if (!ra.StartsWith("1") && !ra.StartsWith("2"))
            {
                var PopUp = new PopUpsGerais(10); // RA Inválido
                PopUp.ShowDialog();
                return;
            }

            if (senha != confSenha)
            {
                var PopUp = new PopUpsGerais(11);
                PopUp.ShowDialog();
                return;
            }

            // Salva a foto fisicamente
            string nomeArquivoFoto = null;
            if (caminhoImagemPerfil != null)
            {
                try
                {
                    string pastaDestino = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil");
                    Directory.CreateDirectory(pastaDestino);
                    nomeArquivoFoto = Guid.NewGuid().ToString() + Path.GetExtension(caminhoImagemPerfil);
                    File.Copy(caminhoImagemPerfil, Path.Combine(pastaDestino, nomeArquivoFoto));
                }
                catch { }
            }

            bool sucesso = GerenciadorDeUsuarios.CadastrarUsuario(nome, email, senha, ra, nomeArquivoFoto);

            if (sucesso)
            {
                if (ra.StartsWith("1"))
                {
                    var PopUp = new PopUpsGerais(12); 
                    PopUp.ShowDialog();
                }
                else if (ra.StartsWith("2"))
                {
                    var PopUp = new PopUpsGerais(13); 
                    PopUp.ShowDialog();
                }

                this.DialogResult = true;
                this.Close();
            }
            else
            {
                var PopUp = new PopUpsGerais(14); // Erro: Usuário já existe
                PopUp.ShowDialog();
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e) { }
    }
}