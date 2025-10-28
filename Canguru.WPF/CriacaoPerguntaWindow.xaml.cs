using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Canguru.Telas.TelaCriacaoPergunta
{
    public partial class MainWindow : Window
    {
        private List<Pergunta> perguntas;
        private string caminhoArquivo = "perguntas.txt";
        private int proximoId = 1;

        public MainWindow()
        {
            InitializeComponent();
            perguntas = new List<Pergunta>();
            ConfigurarComboBox();
            CarregarPerguntasExistentes();
        }

        private void ConfigurarComboBox()
        {
            cmbResposta.Items.Clear();
            cmbResposta.Items.Add("A");
            cmbResposta.Items.Add("B");
            cmbResposta.Items.Add("C");
            cmbResposta.Items.Add("D");
            cmbResposta.SelectedIndex = 0;
        }

        private void CarregarPerguntasExistentes()
        {
            try
            {
                if (File.Exists(caminhoArquivo))
                {
                    var linhas = File.ReadAllLines(caminhoArquivo);
                    foreach (var linha in linhas)
                    {
                        if (!string.IsNullOrEmpty(linha) && linha.StartsWith("ID:"))
                        {
                            proximoId++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar perguntas: {ex.Message}", "Erro",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos())
            {
                SalvarPergunta();
            }
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparCampos();
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtPergunta.Text))
            {
                MessageBox.Show("Digite o enunciado da pergunta!", "Atenção",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPergunta.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAlternativaA.Text) ||
                string.IsNullOrWhiteSpace(txtAlternativaB.Text) ||
                string.IsNullOrWhiteSpace(txtAlternativaC.Text) ||
                string.IsNullOrWhiteSpace(txtAlternativaD.Text))
            {
                MessageBox.Show("Preencha todas as alternativas!", "Atenção",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void SalvarPergunta()
        {
            try
            {
                // Criar nova pergunta
                var novaPergunta = new Pergunta
                {
                    Id = proximoId++,
                    Enunciado = txtPergunta.Text.Trim(),
                    Alternativas = new List<string>
                    {
                        txtAlternativaA.Text.Trim(),
                        txtAlternativaB.Text.Trim(),
                        txtAlternativaC.Text.Trim(),
                        txtAlternativaD.Text.Trim()
                    },
                    IndiceRespostaCorreta = cmbResposta.SelectedIndex
                };

                
                SalvarNoArquivo(novaPergunta);

                
                perguntas.Add(novaPergunta);

                // Limpar campos
                LimparCampos();

                MessageBox.Show("Pergunta salva com sucesso!", "Sucesso",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar pergunta: {ex.Message}", "Erro",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SalvarNoArquivo(Pergunta pergunta)
        {
            using (StreamWriter sw = new StreamWriter(caminhoArquivo, true, Encoding.UTF8))
            {
                sw.WriteLine($"ID: {pergunta.Id}");
                sw.WriteLine($"Pergunta: {pergunta.Enunciado}");
                sw.WriteLine($"A: {pergunta.Alternativas[0]}");
                sw.WriteLine($"B: {pergunta.Alternativas[1]}");
                sw.WriteLine($"C: {pergunta.Alternativas[2]}");
                sw.WriteLine($"D: {pergunta.Alternativas[3]}");
                sw.WriteLine($"Resposta: {(char)('A' + pergunta.IndiceRespostaCorreta)}");
                sw.WriteLine("---"); 
            }
        }

        private void LimparCampos()
        {
            txtPergunta.Clear();
            txtAlternativaA.Clear();
            txtAlternativaB.Clear();
            txtAlternativaC.Clear();
            txtAlternativaD.Clear();
            cmbResposta.SelectedIndex = 0;
            txtPergunta.Focus();
        }

        // Classe Pergunta
        public class Pergunta
        {
            public int Id { get; set; }
            public string Enunciado { get; set; }
            public List<string> Alternativas { get; set; }
            public int IndiceRespostaCorreta { get; set; }

            public Pergunta()
            {
                Alternativas = new List<string>();
            }

            public bool VerificarResposta(int indiceResposta)
            {
                return indiceResposta == IndiceRespostaCorreta;
            }
        }
    }
}