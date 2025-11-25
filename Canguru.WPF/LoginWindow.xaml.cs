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

        // Deixei o método aqui (vazio) para não dar erro no seu XAML, 
        // mas agora ele NÃO bloqueia mais nada. Pode digitar espaço à vontade.
        private void txtLogin_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // LIVRE: Nenhuma tecla é bloqueada.
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

            // Validação simples: Só impede login vazio
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Preencha login e senha.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Busca direto na memória (sem verificar se é email ou RA antes)
            Usuario usuarioLogado = GerenciadorDeUsuarios.ValidarLogin(login, senha);

            if (usuarioLogado != null)
            {
                string tipo = usuarioLogado is Adm ? "Administrador" : (usuarioLogado is Professor ? "Professor" : "Aluno");
                MessageBox.Show($"Bem-vindo, {tipo} {usuarioLogado.Nome}!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                TelaHome novatela = new TelaHome(usuarioLogado);
                novatela.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Login ou senha incorretos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Digite seu Login no campo acima para recuperar a senha.", "Ajuda", MessageBoxButton.OK, MessageBoxImage.Information);
                txtLogin.Focus();
                return;
            }

            Usuario usuario = GerenciadorDeUsuarios.BuscarPorEmail(email);

            if (usuario == null)
            {
                MessageBox.Show("Usuário não encontrado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string novaSenha = System.IO.Path.GetRandomFileName().Replace(".", "").Substring(0, 6);
            GerenciadorDeUsuarios.AtualizarSenha(usuario.Id, novaSenha);

            string resultado = EmailService.EnviarOuSimular(email, novaSenha);

            if (resultado == "OK")
            {
                MessageBox.Show($"Nova senha enviada para: {email}", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"[SIMULAÇÃO]\nUsuário: {usuario.Nome}\nNova Senha: {resultado}", "Recuperação", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnSair_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtLogin_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { }
    }
}