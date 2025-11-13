using Canguru.Business;
using Canguru.Core;
using Canguru.WPF;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Canguru.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainGerentClasse : Window
    {
        private Usuario usuarioLogado;
        private int _usuarioSelecionadoId;
        public MainGerentClasse(Usuario usuario)
        {
            InitializeComponent();
            Loaded += GerenciarUsuarios_Loaded;
            NomeProf.Text = usuario.Nome;
            usuarioLogado = usuario;
            if (!string.IsNullOrEmpty(usuarioLogado.CaminhoFotoPerfil))
            {
                try
                {
                    string caminhoFoto = System.IO.Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "FotosPerfil",
                        usuarioLogado.CaminhoFotoPerfil);

                    if (System.IO.File.Exists(caminhoFoto))
                    {
                        var imagem = new BitmapImage(new Uri(caminhoFoto));
                        FotoProfBrush.ImageSource = imagem;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao carregar foto de perfil: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }   
        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {

            TelaHome telaHome = new TelaHome(usuarioLogado);
                telaHome.Show();
                this.Close();
        }
        private void AtualizarHistorico()
        {
            try
            {
                if (_usuarioSelecionadoId == 0)
                {
                    dgHistorico.ItemsSource = null;
                    return; 
                }

                var resultados = GerenciadorResultFinal.GetResultados()
                    .Where(r => r.idAluno == _usuarioSelecionadoId)
                    .ToList();

                if (resultados.Count > 0)
                {
                    dgHistorico.ItemsSource = null;
                    dgHistorico.ItemsSource = resultados;
                }
                else
                {
                    dgHistorico.ItemsSource = null;
                    MessageBox.Show("Nenhum resultado registrado para este usuário.",
                        "Histórico vazio", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar histórico: {ex.Message}",
                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AtualizarInteracoes()
        {
            try
            {
                if (_usuarioSelecionadoId == 0)
                {
                    dgQuizSelecionado.ItemsSource = null;
                    return;
                }
                var interacoes = GerenciadorInteracao.GetInteracoes().Where(i => i.idAluno == _usuarioSelecionadoId).OrderByDescending(i => i.DataInteracao).ToList();

                if (interacoes.Count > 0)
                {
                    var dadosParaGrid = interacoes.Select(i => new
                    {
                        Pergunta = i.idPerguntaInteracao,Sessao = "Sessão " + i.idInteracao, Acerto = i.resultadoInteração == 1 ? "✔️" : "", Erro = i.resultadoInteração == 0 ? "❌" : ""}).ToList();

                    dgQuizSelecionado.ItemsSource = null;
                    dgQuizSelecionado.ItemsSource = dadosParaGrid;
                }
                else
                {
                    dgQuizSelecionado.ItemsSource = null;
                    MessageBox.Show("Nenhuma interação registrada para este usuário.","Sem dados", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                //debug para mostrar se tem interações desse usuário e quantas são é so pra checar, temq ue tirar isso depois (as vezes se vc clicar muito rapido no final de ul quiz
                //ele considera 2 sessões 'terminadas' então ao inves de ter 10 respostas ele passa a ter 11 ou 20... isso é pra checar)
                MessageBox.Show($"Total de interações no sistema: {GerenciadorInteracao.GetInteracoes().Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar interações: {ex.Message}","Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void GerenciarUsuarios_Loaded(object sender, RoutedEventArgs e)
        {
            AtualizarUsuarios();
           
        }
        private void NomeProf_TextChanged(object sender, TextChangedEventArgs e)
        {
            
                //NomeProf.Text = usuarioLogado.Nome;
        }

        private void dgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //é pra ele só carregar os dados do usuário selecionado!, se não ele carrega de todo mundo
            // e talvez o professor só queria ver de um determinado usuário da classe
            if (dgUsuarios.SelectedItem is Usuario usuarioSelecionado)
            {
                _usuarioSelecionadoId = usuarioSelecionado.Id;
                lblHistoricoTitulo.Text = $"Histórico de {usuarioSelecionado.Nome}";
                AtualizarHistorico();    
                AtualizarInteracoes();    
            }
        }
        private void dgHistorico_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUsuarios.SelectedItem is Usuario usuarioSelecionado)
            {
                _usuarioSelecionadoId = usuarioSelecionado.Id;
                //Segiura o id do usuario selecionado
            }

        }
        private void AtualizarUsuarios()
        {
            var usuarios = GerenciadorDeUsuarios.GetTodosUsuarios();
            MessageBox.Show($"Usuários carregados: {usuarios.Count}");
            dgUsuarios.ItemsSource = null;
            dgUsuarios.ItemsSource = usuarios;
        }
       /* private void GerenciarUsuarios_Loaded(object sender, RoutedEventArgs e)
        {
            AtualizarUsuarios();
        }*/
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Usuario usuario)
            {
                _usuarioSelecionadoId = usuario.Id;
                MessageBox.Show($"Editar usuário: {usuario.Nome} (ID: {_usuarioSelecionadoId})");
            }
        }


        // Clique no botão Excluir
        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Usuario usuario)
            {
                _usuarioSelecionadoId = usuario.Id;
                MessageBoxResult result = MessageBox.Show(
                    $"Tem certeza que deseja excluir o usuário \"{usuario.Nome}\"?",
                    "Confirmar exclusão",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    bool sucesso = GerenciadorDeUsuarios.ExcluirUsuario(_usuarioSelecionadoId);

                    if (sucesso)
                    {
                        MessageBox.Show("Usuário excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        AtualizarUsuarios();
                    }
                    else
                    {
                        MessageBox.Show("Erro: usuário não encontrad.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        
    }
}
