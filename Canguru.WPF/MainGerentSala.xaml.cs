using Canguru.Business;
using Canguru.Core;
using Canguru.WPF.Pop_Ups;
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
                //MessageBox.Show($"[DEBUG] Editar usuário: {usuario.Nome} (ID: {_usuarioSelecionadoId})");
            }
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Usuario usuario)
            {
                _usuarioSelecionadoId = usuario.Id;

                // Abre popup de CONFIRMAÇÃO (case 18)
                var popupConfirmacao = new PopUpsGerais(18, ExecutarExclusao);
                popupConfirmacao.ShowDialog();
            }
        }
        private void ExecutarExclusao()
        {
            bool sucesso = GerenciadorDeUsuarios.ExcluirUsuario(_usuarioSelecionadoId);

            if (sucesso)
            {
                AtualizarUsuarios();
                dgHistorico.ItemsSource = null;
                dgQuizSelecionado.ItemsSource = null;
            }
            else
            {
                // Se der erro → abre popup de erro (case 19)
                var popupErro = new PopUpsGerais(19);
                popupErro.ShowDialog();
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
                //MessageBox.Show($"Erro ao carregar histórico: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                var pOPuP = new PopUpsGerais(20);
                pOPuP.ShowDialog();
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
                //MessageBox.Show($"Erro ao carregar interações: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                var PopUp = new PopUpsGerais(21);
                PopUp.ShowDialog();
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
                string caminhoModelo = EncontrarArquivo(Path.Combine("Resources", nomeArquivo))
                                       ?? EncontrarArquivo(nomeArquivo);

                if (caminhoModelo == null)
                {
                    new Pop_Ups.PopUpInfoemações(1).ShowDialog();
                    return;
                }

                string pastaDestino = Path.GetDirectoryName(caminhoModelo);
                string caminhoFinal = Path.Combine(pastaDestino, "ResultadosGerados.xlsx");

                ExportadorExcel.Exportar(
                    caminhoModelo,
                    caminhoFinal,
                    turma.Resultados
                );

                var popup = new Pop_Ups.PopUpInfoemações(2, () =>
                {
                    System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{caminhoFinal}\"");
                });

                popup.ShowDialog();
            }
            catch (IOException)
            {
                new Pop_Ups.PopUpInfoemações(3).ShowDialog();
            }
            catch (Exception ex)
            {
                new Pop_Ups.PopUpInfoemações(1).ShowDialog();
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