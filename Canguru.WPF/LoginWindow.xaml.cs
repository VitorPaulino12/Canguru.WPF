using System.Windows;
using Canguru.Core;
using Canguru.Business;
using System.Windows.Input; 

namespace Canguru.WPF
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string senha = txtSenha.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Preencha todos os campos!");
                return;
            }


            Usuario usuarioLogado = GerenciadorDeUsuarios.ValidarLogin(login, senha);

            if (usuarioLogado != null)
            {
                MessageBox.Show($"Bem-vindo, {usuarioLogado.Nome}!");


                this.Close();
                
            }
            else
            {
                MessageBox.Show("Login ou senha incorretos.");
            }
        }

        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            CadastroWindow TelaCadastro = new CadastroWindow();
            this.Close();
            TelaCadastro.ShowDialog();
        }

        private void txtSenha_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}