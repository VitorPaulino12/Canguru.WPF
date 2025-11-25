using Canguru.WPF;
using System.Linq;
using System.Windows;

public static class FecharJanelas
{
    public static void VoltarParaLogin()
    {
        // Cria uma nova tela de Login
        var login = new LoginWindow();
        login.Show();

        // Fecha todas as outras janelas
        foreach (Window janela in Application.Current.Windows.Cast<Window>().ToList())
        {
            if (janela is not LoginWindow)
                janela.Close();
        }
    }
}
