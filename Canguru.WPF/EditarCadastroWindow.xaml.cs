using System.Windows;

namespace Canguru.WPF
{
    public partial class EditarCadastroWindow : Window
    {
        public string NovoNome { get; private set; }
        public string NovoEmail { get; private set; }

        public EditarCadastroWindow(string nomeAtual, string emailAtual)
        {
            InitializeComponent();

            txtNome.Text = nomeAtual;
            txtEmail.Text = emailAtual.Replace("E-mail: ", "").Trim();
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            NovoNome = txtNome.Text;
            NovoEmail = txtEmail.Text;
            DialogResult = true; 
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; 
            Close();
        }
    }
}