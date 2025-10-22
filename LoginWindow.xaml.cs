using System.Windows; // <-- Mude o using
using Canguru.Core;
using Canguru.Business;
using System.Windows.Input; // <-- Adicione para o KeyDown

namespace Canguru.WPF
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            // Carregue suas imagens (exemplo)
            // (Certifique-se que as imagens estão no seu projeto WPF)
            // imgFundo.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/bgPrincipal.png"));
            // imgLogo.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/logo_canguruu.png"));
        }

        // --- LÓGICA COPIADA E ADAPTADA ---

        private void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            // MUDANÇA: 'PasswordBox' usa .Password, não .Text
            string login = txtLogin.Text;
            string senha = txtSenha.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Preencha todos os campos!"); // MessageBox funciona igual
                return;
            }


            Usuario usuarioLogado = GerenciadorDeUsuarios.ValidarLogin(login, senha);

            if (usuarioLogado != null)
            {
                MessageBox.Show($"Bem-vindo, {usuarioLogado.Nome}!");

                // Crie sua PrincipalWindow (você fará esta tela depois)
                // PrincipalWindow principal = new PrincipalWindow(usuarioLogado);
                // principal.Show();

                this.Close(); // MUDANÇA: this.Hide() vira this.Close()
            }
            else
            {
                MessageBox.Show("Login ou senha incorretos.");
            }
        }

        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtSenha_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}