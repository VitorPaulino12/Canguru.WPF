using System.Windows;
using Canguru.Core;       // Para usar Usuario, Aluno, Professor, etc.
using Canguru.Business;  // Para usar GerenciadorDeUsuarios
using Microsoft.Win32;      // Para a janela "Abrir Arquivo" (OpenFileDialog)
using System.Windows.Media.Imaging; // Para carregar imagens (BitmapImage)
using System;                 // Para a classe Uri e Exception
using System.ComponentModel;  // Para a verificação do Designer (opcional)
using System.IO;              // Para trabalhar com arquivos (copiar a foto)

// REMOVIDO: using System; duplicado

namespace Canguru.WPF
{
    public partial class CadastroWindow : Window
    {
        // Variável no nível da classe para guardar o caminho da foto
        private string caminhoImagemPerfilSelecionada = null;

        public CadastroWindow()
        {
            InitializeComponent();

            // Opcional: Proteção para o designer (se ele der erro ao abrir)
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
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
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; // Carrega imagem imediatamente
                    bitmap.EndInit();
                    imgPerfil.Source = bitmap;
                    caminhoImagemPerfilSelecionada = filePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao carregar a imagem: {ex.Message}", "Erro de Imagem", MessageBoxButton.OK, MessageBoxImage.Error);
                    caminhoImagemPerfilSelecionada = null;
                    imgPerfil.Source = null;
                }
            }
        }

        private void BtnSalvarCadastro_Click(object sender, RoutedEventArgs e)
        {
            // CORREÇÃO: Usar minúsculas para variáveis locais (convenção C#)
            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();
            string senha = txtSenhaCadastro.Password;
            string confSenha = txtConfirmarSenha.Password; // Renomeado para clareza

            // --- Validações ---
            if (string.IsNullOrWhiteSpace(nome) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Por favor, preencha Nome, Email e Senha.", "Campos Obrigatórios", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (senha != confSenha)
            {
                MessageBox.Show("As Senhas Não Coincidem, Verifique!", "Senhas Incompatíveis.", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSenhaCadastro.Focus();
                return;
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Formato de email inválido.", "Email Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return;
            }
            if (senha.Length < 6)
            {
                MessageBox.Show("A senha deve ter pelo menos 6 caracteres.", "Senha Curta", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSenhaCadastro.Focus();
                return;
            }
            Usuario novoUsuario = new Usuario
            {
                Nome = nome,
                Login = email,
                Senha = senha
            };

            // 2. Processar a Imagem (Copiar para pasta e guardar nome/caminho)
            string nomeArquivoFoto = null;
            if (caminhoImagemPerfilSelecionada != null)
            {
                try
                {
                    // Define uma pasta para salvar as fotos (ex: dentro da pasta do executável)
                    string pastaDestino = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil");
                    Directory.CreateDirectory(pastaDestino); // Cria a pasta se não existir

                    // Cria um nome único para o arquivo para evitar colisões
                    nomeArquivoFoto = Guid.NewGuid().ToString() + Path.GetExtension(caminhoImagemPerfilSelecionada);
                    string caminhoDestino = Path.Combine(pastaDestino, nomeArquivoFoto);

                    // Copia o arquivo selecionado para a pasta de destino
                    File.Copy(caminhoImagemPerfilSelecionada, caminhoDestino);

                    // Guarda apenas o nome do arquivo (ou caminho relativo) no objeto Usuario
                    novoUsuario.CaminhoFotoPerfil = nomeArquivoFoto; // (Assumindo que sua classe Usuario tem essa propriedade)
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao salvar a foto de perfil: {ex.Message}\nO cadastro continuará sem foto.", "Erro Foto", MessageBoxButton.OK, MessageBoxImage.Warning);
                    nomeArquivoFoto = null; // Garante que não salve referência inválida
                }
            }

            // 3. Chamar o Gerenciador de Negócios
            try
            {
                bool cadastroOk = GerenciadorDeUsuarios.CadastrarUsuario(novoUsuario);

                if (cadastroOk)
                {
                    MessageBox.Show($"Usuário '{novoUsuario.Nome}' cadastrado com sucesso!", "Cadastro Concluído", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true; // Sinaliza sucesso para quem chamou (LoginWindow)
                    this.Close(); // Fecha a janela de cadastro
                }
                else
                {
                    MessageBox.Show("Não foi possível realizar o cadastro. O email/login informado já pode estar em uso.", "Falha no Cadastro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    // Se falhou, talvez queira deletar a imagem que foi copiada
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
                // Se falhou, talvez queira deletar a imagem que foi copiada
                if (nomeArquivoFoto != null)
                {
                    string caminhoParaDeletar = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil", nomeArquivoFoto);
                    if (File.Exists(caminhoParaDeletar)) File.Delete(caminhoParaDeletar);
                }
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            // CORREÇÃO: Apenas feche a janela. Quem a chamou (LoginWindow com ShowDialog)
            // continuará de onde parou. Não precisa criar uma nova LoginWindow.
            this.DialogResult = false; // Sinaliza cancelamento
            this.Close();
        }
    }
}