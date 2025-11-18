using Canguru.Business;
using Canguru.Core;
using System.Runtime.Intrinsics.Arm;
using System.Windows;
using System.Windows.Input;

namespace Canguru.WPF
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingOverlay.Visibility = Visibility.Collapsed;
        }

        private void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            FazerLogin();
        }

        private void txtSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                FazerLogin();
        }

        private void FazerLogin()
        {
           
            string login = txtLogin.Text.Trim();
            string senha = txtSenha.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Preencha login e senha.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 1. Valida o login. O método retorna um objeto do tipo "Usuario"
            Usuario usuarioLogado = GerenciadorDeUsuarios.ValidarLogin(login, senha);

            if (usuarioLogado != null)
            {
                // 2. AQUI ESTÁ A MÁGICA: Verificamos o tipo REAL do usuário que retornou
                if (usuarioLogado is Aluno)
                {
                    // O usuário é um Aluno!
                    MessageBox.Show($"Bem-vindo, Aluno {usuarioLogado.Nome}!", "Login Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Aqui você abre a tela principal do aluno
                    // var telaAluno = new AlunoMainWindow(usuarioLogado);
                    // telaAluno.Show();
                }
                else if (usuarioLogado is Professor)
                {
                    MessageBox.Show($"Bem-vindo, Professor {usuarioLogado.Nome}!", "Login Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (usuarioLogado is Adm)
                {
                    MessageBox.Show($"Bem-vindo, Administrador!", "Login Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                TelaHome novatela = new TelaHome(usuarioLogado);
                novatela.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Login ou senha incorretos.", "Falha na Autenticação", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
            //CÓDIGO ANTERIOR PARA FICAR SALVO CASO NECESSÁRIO
            /*
            LoadingOverlay.Visibility = Visibility.Visible;

            string login = txtLogin.Text.Trim();
            string senha = txtSenha.Password.Trim();

            if (login == "" || senha == "")
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
                MessageBox.Show("Preencha todos os campos.");
                return;
            }

            var usuario = GerenciadorDeUsuarios.ValidarLogin(login, senha);

            LoadingOverlay.Visibility = Visibility.Collapsed;

            if (usuario == null)
            {
                MessageBox.Show("Login ou senha incorretos.");
                return;
            }

            MessageBox.Show($"Bem-vindo, {usuario.Nome}!");
            TelaHome novatela = new TelaHome(usuario);
            novatela.Show();
            this.Close();

            // Aqui você abriria a janela principal
            // new MainWindow().Show();
            // Close();
            */
        }

        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            CadastroWindow TelaCadastro = new CadastroWindow();
            TelaCadastro.ShowDialog();
        }

        private async void BtnEsqueceuSenha_Click(object sender, RoutedEventArgs e)
        {
            string email = txtLogin.Text.Trim(); // ← AQUI! usando txtLogin, já que txtUsuario não existe

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Digite seu e-mail para redefinir a senha.",
                    "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                await SupabaseService.ResetarSenha(email);

                MessageBox.Show(
                    "Enviamos um link de redefinição de senha para o seu e-mail!\n" +
                    "Abra sua caixa de entrada e siga as instruções.",
                    "Verifique seu e-mail",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao enviar o e-mail de redefinição: {ex.Message}",
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }
}

