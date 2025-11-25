using Canguru.Business;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input; // Necessário para bloquear teclas e Mouse
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

        // --- MÁSCARA 1: BLOQUEIA ESPAÇOS ENQUANTO DIGITA ---
        private void txtEmail_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true; // Ignora a tecla espaço
            }
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
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar imagem: " + ex.Message);
                }
            }
        }

        private void BtnSalvarCadastro_Click(object sender, RoutedEventArgs e)
        {
            // 1. PEGAR VALORES
            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();
            string ra = txtRA.Text.Trim();
            string senha = txtSenhaCadastro.Password;
            string confSenha = txtConfirmarSenha.Password;

            // 2. VERIFICA CAMPOS VAZIOS
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(ra) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // --- NOVA VALIDAÇÃO DE EMAIL (MÁSCARA LÓGICA) ---
            // Verifica se tem '@' E se contém '.com' (independente de maiúscula/minúscula)
            if (!email.Contains("@") || !email.ToLower().Contains(".com"))
            {
                MessageBox.Show("E-mail inválido!\n\nO e-mail deve conter '@' e o domínio '.com'.\nExemplo: usuario@gmail.com",
                                "Erro no Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus(); // Coloca o cursor lá para a pessoa arrumar
                return;
            }
            // --------------------------------------------------

            // 3. VALIDAÇÃO DO RA
            if (!ra.StartsWith("1") && !ra.StartsWith("2"))
            {
                MessageBox.Show("RA Inválido!\nDeve começar com 1 (Aluno) ou 2 (Professor).", "Erro de RA", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 4. VALIDAÇÃO DE SENHA
            if (senha != confSenha)
            {
                MessageBox.Show("As senhas não conferem.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 5. SALVAR FOTO
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

            // 6. FINALIZAR CADASTRO NA MEMÓRIA
            bool sucesso = GerenciadorDeUsuarios.CadastrarUsuario(nome, email, senha, ra, nomeArquivoFoto);

            if (sucesso)
            {
                string tipo = ra.StartsWith("1") ? "Aluno" : "Professor";
                MessageBox.Show($"{tipo} cadastrado com sucesso!", "Bem-vindo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Erro: Este Login/Email já está cadastrado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Método vazio necessário para o XAML não dar erro
        }
    }
}