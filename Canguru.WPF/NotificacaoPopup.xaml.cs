using Canguru.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Canguru.Core;
using Canguru.WPF;
using QuizTeste;


namespace Canguru.WPF
{
    public partial class NotificacaoPopup : Window
    {
        private Usuario usuarioLogado;

        public NotificacaoPopup(Usuario usuario)
        {
            InitializeComponent();
            usuarioLogado = usuario;
        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnNao_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSim_Click(object sender, RoutedEventArgs e)
        {
            // Exemplo: abre tela de quiz
            TelaPerguntas telaHome = new TelaPerguntas(usuarioLogado);
            telaHome.Show();
            this.Close();
        }
    }
}
