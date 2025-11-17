using System.Windows;
using System.Windows.Input;
using Canguru.Business;

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

            // Aqui você abriria a janela principal
            // new MainWindow().Show();
            // Close();
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

