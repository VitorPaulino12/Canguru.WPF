using Canguru.Business;
using Canguru.Core;
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
            if (FindName("LoadingOverlay") is FrameworkElement loading)
            {
                loading.Visibility = Visibility.Collapsed;
            }
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
                MessageBox.Show("Preencha login e senha.", "Campos Vazios", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Usuario usuarioLogado = GerenciadorDeUsuarios.ValidarLogin(login, senha);

            if (usuarioLogado != null)
            {
                string tipo = usuarioLogado is Adm ? "Administrador" : (usuarioLogado is Professor ? "Professor" : "Aluno");
                MessageBox.Show($"Bem-vindo, {tipo} {usuarioLogado.Nome}!", "Login Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                TelaHome novatela = new TelaHome(usuarioLogado);
                novatela.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Login ou senha incorretos.", "Erro de Login", MessageBoxButton.OK, MessageBoxImage.Error);
                txtSenha.Clear();
                txtSenha.Focus();
            }
        }

        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            CadastroWindow TelaCadastro = new CadastroWindow();
            TelaCadastro.ShowDialog();
        }

        private void BtnEsqueceuSenha_Click(object sender, RoutedEventArgs e)
        {
            string email = txtLogin.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Digite seu Login/Email no campo de usuário para recuperar a senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtLogin.Focus();
                return;
            }

            // Busca usuário
            Usuario usuario = GerenciadorDeUsuarios.BuscarPorEmail(email);

            if (usuario == null)
            {
                MessageBox.Show("Usuário não encontrado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Gera senha aleatória
            string novaSenha = System.IO.Path.GetRandomFileName().Replace(".", "").Substring(0, 6);

            // Atualiza na memória
            GerenciadorDeUsuarios.AtualizarSenha(usuario.Id, novaSenha);

            // Tenta enviar ou simular
            string resultado = EmailService.EnviarOuSimular(email, novaSenha);

            if (resultado == "OK")
            {
                MessageBox.Show("Email de recuperação enviado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // MODO SIMULAÇÃO: Mostra a senha na tela porque estamos num protótipo
                MessageBox.Show($"[SIMULAÇÃO DE EMAIL]\n\n" +
                                $"Para: {email}\n" +
                                $"Nova Senha Gerada: {resultado}\n\n" +
                                $"(Copie esta senha para logar)",
                                "Email Simulado", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void txtLogin_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { }
    }
}