// using System; REMOVIDO (estava duplicado)
using Canguru.Business;
using Canguru.Core;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Canguru.WPF
{
    public partial class CadastroWindow : Window
    {
        // CORREÇÃO 1: O nome da variável é 'caminhoImagemPerfil'
        private string caminhoImagemPerfil = null;

        public CadastroWindow()
        {
            InitializeComponent();
        }

        private void BtnEscolherFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Arquivos de Imagem (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|Todos os Arquivos (*.*)|*.*";
            openFileDialog.Title = "Selecione uma Foto de Perfil";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    BitmapImage bitmap = new BitmapImage();

                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(filePath);

                    // -----------------------------------------------------------------
                    // CORREÇÃO 2: ADICIONAR ESTA LINHA!
                    // Isso força o WPF a carregar a imagem e soltar o arquivo original.
                    // Sem isso, o File.Copy() no botão Salvar falhará.
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    // -----------------------------------------------------------------

                    bitmap.EndInit();

                    imgPerfil.Source = bitmap;
                    caminhoImagemPerfil = filePath; // Nome da variável corrigido
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
            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();
            string senha = txtSenhaCadastro.Password;
            string confSenha = txtConfirmarSenha.Password;
            

            // --- Validações (Seu código aqui já estava bom) ---
            if (string.IsNullOrWhiteSpace(nome) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Por Favor, preencha Nome, Email e Senha.", "Campos Obrigatórios", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (senha != confSenha)
            {
                MessageBox.Show("As duas senhas não coincidem.", "Verifique-as!", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSenhaCadastro.Focus();
                return;
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Formato de Email Inválido.", "Email Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return;
            }
            if (senha.Length < 6)
            {
                MessageBox.Show("A senha deve ter no mínimo 6 caracteres.", "Senha insuficiente.", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSenhaCadastro.Focus();
                return;
            }

            // -----------------------------------------------------------------
            // CORREÇÃO 3: LÓGICA DE DECISÃO (ALUNO ou PROFESSOR)
            // -----------------------------------------------------------------

            Usuario novoUsuario;
            if (chkProfessor.IsChecked == true) 
            {
                novoUsuario = new Professor
                {
                    Nome = nome,
                    Login = email,
                    Senha = senha
                };
            }
            else
            {
                novoUsuario = new Aluno
                {
                    Nome = nome,
                    Login = email,
                    Senha = senha
                };
            }

            string nomeArquivoFoto = null;
            if (caminhoImagemPerfil != null)
            {
                try
                {
                    string pastaDestino = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil");
                    Directory.CreateDirectory(pastaDestino);

                    nomeArquivoFoto = Guid.NewGuid().ToString() + Path.GetExtension(caminhoImagemPerfil);
                    string caminhoDestino = Path.Combine(pastaDestino, nomeArquivoFoto);

                    File.Copy(caminhoImagemPerfil, caminhoDestino);

                    novoUsuario.CaminhoFotoPerfil = nomeArquivoFoto;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao salvar a foto de perfil: {ex.Message}\nO cadastro continuará sem foto.", "Erro Foto", MessageBoxButton.OK, MessageBoxImage.Warning);
                    nomeArquivoFoto = null;
                }
            }


            try
            {               
                bool cadastroOk = GerenciadorDeUsuarios.CadastrarUsuario(novoUsuario);

                if (cadastroOk)
                {
                    MessageBox.Show($"Usuário '{novoUsuario.Nome}' cadastrado com sucesso!", "Cadastro Concluído", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Não foi possível realizar o cadastro. O email/login informado já pode estar em uso.", "Falha no Cadastro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    if (nomeArquivoFoto != null)
                    {
                        string caminhoParaDeletar = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil", nomeArquivoFoto);
                        if (File.Exists(caminhoParaDeletar)) File.Delete(caminhoParaDeletar);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro inesperado ao salvar o cadastro: {ex.Message}", "Erro Crítico", MessageBoxButton.OK, MessageBoxImage.Error);
                if (nomeArquivoFoto != null)
                {
                    string caminhoParaDeletar = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil", nomeArquivoFoto);
                    if (File.Exists(caminhoParaDeletar)) File.Delete(caminhoParaDeletar);
                }

            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; 
            this.Close();
        }
    }
}