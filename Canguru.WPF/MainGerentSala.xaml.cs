using Canguru.Business;
using Canguru.Core;
using System;
using System.Collections.Generic;
using System.IO;
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
                    string caminhoFoto = EncontrarArquivo(Path.Combine("FotosPerfil", usuarioLogado.CaminhoFotoPerfil));
                    if (caminhoFoto != null)
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

        // Métodos auxiliares
        private void dgHistorico_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void dgQuizSelecionado_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void NomeProf_TextChanged(object sender, TextChangedEventArgs e) { }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Usuario usuario)
            {
                _usuarioSelecionadoId = usuario.Id;
                MessageBox.Show($"[DEBUG] Editar usuário: {usuario.Nome} (ID: {_usuarioSelecionadoId})");
            }
        }

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
                    bool sucesso = GerenciadorDeUsuarios.ExcluirUsuario(_usuarioSelecionadoId);

                    if (sucesso)
                    {
                        MessageBox.Show("Usuário excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        AtualizarUsuarios();
                        dgHistorico.ItemsSource = null;
                        dgQuizSelecionado.ItemsSource = null;
                    }
                    else
                    {
                        MessageBox.Show("Erro ao excluir usuário.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void BtnGerarRelatorio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var turma = new TurmaResultados();

                var alunosFakes = GeradorAlunos.Gerar(20);
                alunosFakes.ForEach(turma.Adicionar);

                string nomeArquivo = "ModeloResultados.xlsx";
                string caminhoModelo = null;

                caminhoModelo = EncontrarArquivo(Path.Combine("Resources", nomeArquivo));

                if (caminhoModelo == null)
                {
                    caminhoModelo = EncontrarArquivo(nomeArquivo);
                }

                if (caminhoModelo == null)
                {
                    MessageBox.Show(
                        $"Não consegui encontrar o arquivo '{nomeArquivo}'.\n\nDICA: Mova o arquivo Excel para dentro da pasta 'Canguru.WPF/Resources'.",
                        "Arquivo perdido",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }

                string pastaDestino = Path.GetDirectoryName(caminhoModelo);
                string caminhoFinal = Path.Combine(pastaDestino, "ResultadosGerados.xlsx");

                ExportadorExcel.Exportar(
                    caminhoModelo,
                    caminhoFinal,
                    turma.Resultados
                );

                var abrir = MessageBox.Show(
                    $"Sucesso! Arquivo gerado em:\n{caminhoFinal}\n\nDeseja abrir a pasta?",
                    "Relatório Gerado",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information
                );

                if (abrir == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{caminhoFinal}\"");
                }
            }
            catch (IOException)
            {
                MessageBox.Show("O arquivo Excel parece estar aberto. Feche-o e tente novamente.", "Arquivo em uso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string EncontrarArquivo(string caminhoRelativo)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            string tentativa1 = Path.Combine(baseDir, caminhoRelativo);
            if (File.Exists(tentativa1)) return tentativa1;

            DirectoryInfo dirAtual = new DirectoryInfo(baseDir);

            for (int i = 0; i < 6; i++)
            {
                if (dirAtual == null || !dirAtual.Exists) break;

                string tentativa = Path.Combine(dirAtual.FullName, caminhoRelativo);
                if (File.Exists(tentativa)) return tentativa;

                dirAtual = dirAtual.Parent;
            }

            return null; 
        }
    }
}