using Canguru.Business;
using Canguru.Core;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Canguru.WPF
{
    public partial class MainGerentClasse : Window
    {
        private Usuario usuarioLogado;
        private int _usuarioSelecionadoId;

        public MainGerentClasse(Usuario usuario)
        {
            InitializeComponent();
            usuarioLogado = usuario;
            Loaded += GerenciarUsuarios_Loaded;

            ConfigurarPerfil();
        }

        private void ConfigurarPerfil()
        {
            NomeProf.Text = usuarioLogado.Nome;

            if (!string.IsNullOrEmpty(usuarioLogado.CaminhoFotoPerfil))
            {
                try
                {
                    string caminhoFoto = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil", usuarioLogado.CaminhoFotoPerfil);
                    if (System.IO.File.Exists(caminhoFoto))
                    {
                        FotoProfBrush.ImageSource = new BitmapImage(new Uri(caminhoFoto));
                    }
                }
                catch { /* Ignora erro de imagem */ }
            }
        }

        private void GerenciarUsuarios_Loaded(object sender, RoutedEventArgs e)
        {
            AtualizarUsuarios();
        }

        private void AtualizarUsuarios()
        {
            var usuarios = GerenciadorDeUsuarios.GetTodosUsuarios();

            // MENSAGEM DE DEBUG (Como você pediu)
            MessageBox.Show($"[DEBUG] Total de usuários carregados: {usuarios.Count}");

            dgUsuarios.ItemsSource = null;
            dgUsuarios.ItemsSource = usuarios;
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            TelaHome telaHome = new TelaHome(usuarioLogado);
            telaHome.Show();
            this.Close();
        }

        private void dgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUsuarios.SelectedItem is Usuario usuarioSelecionado)
            {
                _usuarioSelecionadoId = usuarioSelecionado.Id;
                if (lblHistoricoTitulo != null)
                    lblHistoricoTitulo.Text = $"Histórico de {usuarioSelecionado.Nome}";

                AtualizarHistorico();
                AtualizarInteracoes();
            }
        }

        private void dgHistorico_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void dgQuizSelecionado_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void NomeProf_TextChanged(object sender, TextChangedEventArgs e) { }

        // --- BOTÃO EDITAR (DEBUG) ---
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Usuario usuario)
            {
                _usuarioSelecionadoId = usuario.Id;
                MessageBox.Show($"[DEBUG] Editar usuário: {usuario.Nome} (ID: {_usuarioSelecionadoId})");
            }
        }

        // --- BOTÃO EXCLUIR (CORRIGIDO) ---
        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Usuario usuario)
            {
                _usuarioSelecionadoId = usuario.Id;

                var result = MessageBox.Show(
                    $"Tem certeza que deseja excluir \"{usuario.Nome}\"?",
                    "Confirmar exclusão",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    // Usa o método corrigido que retorna BOOL
                    bool sucesso = GerenciadorDeUsuarios.ExcluirUsuario(_usuarioSelecionadoId);

                    if (sucesso)
                    {
                        MessageBox.Show("Usuário excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        AtualizarUsuarios();

                        // Limpa as tabelas de detalhes
                        dgHistorico.ItemsSource = null;
                        dgQuizSelecionado.ItemsSource = null;
                    }
                    else
                    {
                        MessageBox.Show("Erro ao excluir usuário (ID não encontrado).", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void AtualizarHistorico()
        {
            try
            {
                if (_usuarioSelecionadoId == 0) return;
                var resultados = GerenciadorResultFinal.GetResultados()
                    .Where(r => r.idAluno == _usuarioSelecionadoId)
                    .ToList();

                dgHistorico.ItemsSource = null;
                if (resultados.Any()) dgHistorico.ItemsSource = resultados;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar histórico: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AtualizarInteracoes()
        {
            try
            {
                if (_usuarioSelecionadoId == 0) return;
                var interacoes = GerenciadorInteracao.GetInteracoes()
                    .Where(i => i.idAluno == _usuarioSelecionadoId)
                    .OrderByDescending(i => i.DataInteracao)
                    .ToList();

                dgQuizSelecionado.ItemsSource = null;
                if (interacoes.Any())
                {
                    var dadosParaGrid = interacoes.Select(i => new
                    {
                        Pergunta = i.idPerguntaInteracao,
                        Sessao = "Interação " + i.idInteracao,
                        Acerto = i.resultadoInteração == 1 ? "✔️" : "",
                        Erro = i.resultadoInteração == 0 ? "❌" : ""
                    }).ToList();

                    dgQuizSelecionado.ItemsSource = dadosParaGrid;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar interações: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
