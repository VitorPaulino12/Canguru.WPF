using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Canguru.Telas.TelaCriacaoPergunta
{
    public partial class MainWindow : Window
    {
        private ArrayList perguntas;
        private int proximoId = 1;

        public MainWindow()
        {
            InitializeComponent();
            perguntas = new ArrayList();
            ConfigurarComboBox();
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
                    Alternativas = new ArrayList
                    {
                        txtAlternativaA.Text.Trim(),
                        txtAlternativaB.Text.Trim(),
                        txtAlternativaC.Text.Trim(),
                        txtAlternativaD.Text.Trim()
                    },
                    IndiceRespostaCorreta = cmbResposta.SelectedIndex
                };

                // Adicionar no ArrayList
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
            public ArrayList Alternativas { get; set; }
            public int IndiceRespostaCorreta { get; set; }

            public Pergunta()
            {
                Alternativas = new ArrayList();
            }

            public bool VerificarResposta(int indiceResposta)
            {
                return indiceResposta == IndiceRespostaCorreta;
            }
        }
    }
}