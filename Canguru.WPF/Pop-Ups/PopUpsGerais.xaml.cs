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

namespace Canguru.WPF.Pop_Ups
{
    /// <summary>
    /// Interaction logic for PopUpsGerais.xaml
    /// </summary>
    public partial class PopUpsGerais : Window
    {
        private int _identificadorNotificacao;
        private Action _acaoConfirmada;
        public PopUpsGerais(int identificadorNotificacao, Action acaoConfirmada = null)
        {
            InitializeComponent();
            _identificadorNotificacao = identificadorNotificacao;
            _acaoConfirmada = acaoConfirmada;

            PreencherTela(_identificadorNotificacao);
        }

        private void BtnNao_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnSim_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnVerde_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            if (_acaoConfirmada != null)
                _acaoConfirmada.Invoke();

            this.Close();

        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        public void PreencherTela(int identificadorNotificacao)
        {
            //esse método serve para pegar o identificador de notificação e comparar com um dos casos
            //quando identificado um numero especifico passado por parâmetro na hora de instânciar a tela
            //ele compara com uma das op´ções e molda a dela diacordo com as seus respectivos requisitos
            //dês de esconder e reposicionar botões até mudar o titulo e conteudo da notificação
            //Segue uma cola dos itens que podem ser mudados para não precisar ficar abrindo a tela toda hora
            //x:Name="txtConteudo" -> (Texto presente no corpo da notificação),x:Name="btnVermelho" -> (botão de cor vermelha)
            //x:Name="btnVerde" (botão de cor verde), x:Name="txtTituloNotificacao" -> (texto do titulo da noficicação)
            //
            switch (identificadorNotificacao)
            {
                //PopUps da tela de login

                case 1:
                    txtTituloNotificacao.Text = "Aviso";
                    txtConteudo.Text = "Preenchar Login e Senha para prosseguir!";
                    btnVermelho.Content = "sem texto";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Visible;
                    btnVerde.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    txtTituloNotificacao.Text = "ERRO";
                    txtConteudo.Text = "Login ou senha incorretos.";
                    btnVermelho.Content = "sem texto";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;//significa que tem que aparecer
                    btnVerde.Visibility = Visibility.Visible; // Significa que tem que esconde
                    break;
                case 3:
                    txtTituloNotificacao.Text = "Atenção";
                    txtConteudo.Text = "Digite seu Login no campo acima para recuperar a senha.";
                    btnVermelho.Content = "sem texto";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;//significa que tem que aparecer
                    btnVerde.Visibility = Visibility.Visible; // Significa que tem que esconde
                    break;
                case 4:
                    txtTituloNotificacao.Text = "ERRO";
                    txtConteudo.Text = "Usuário não encontrado.";
                    btnVermelho.Content = "sem texto";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;//significa que tem que aparecer
                    btnVerde.Visibility = Visibility.Visible; // Significa que tem que esconde
                    break;
                case 5:
                    txtTituloNotificacao.Text = "Sucesso";
                    txtConteudo.Text = "Nova senha enviada para seu E-mail cadastrado";
                    btnVermelho.Content = "sem texto";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;//significa que tem que aparecer
                    btnVerde.Visibility = Visibility.Visible; // Significa que tem que esconde
                    break;
                case 6:
                    txtTituloNotificacao.Text = "Sucesso";
                    txtConteudo.Text = "Bom vindo ao sistema!";
                    btnVermelho.Content = "sem texto";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;//significa que tem que aparecer
                    btnVerde.Visibility = Visibility.Visible; // Significa que tem que esconde
                    break;
                //TELA DE CADASTRO DE USUÁRIO
                case 7:
                    txtTituloNotificacao.Text = "Erro";
                    txtConteudo.Text = "Erro ao carregar imagem";
                    btnVermelho.Content = "sem texto";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;//significa que tem que aparecer
                    btnVerde.Visibility = Visibility.Visible; // Significa que tem que esconde
                    break;
                case 8:
                    txtTituloNotificacao.Text = "Atenção";
                    txtConteudo.Text = "Preencha todos os campos obrigatórios.";
                    btnVermelho.Content = "sem texto";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;//significa que tem que aparecer
                    btnVerde.Visibility = Visibility.Visible; // Significa que tem que esconde
                    break;
                case 9:
                    txtTituloNotificacao.Text = "Atenção";
                    txtConteudo.Text = "E-mail inválido!";
                    btnVermelho.Content = "sem texto";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;//significa que tem que aparecer
                    btnVerde.Visibility = Visibility.Visible; // Significa que tem que esconde
                    break;
                case 10:
                    txtTituloNotificacao.Text = "Atenção";
                    txtConteudo.Text = "RA Inválido!\nDeve começar com 1 (Aluno) ou 2 (Professor).";
                    btnVermelho.Content = "sem texto";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;//significa que tem que aparecer
                    btnVerde.Visibility = Visibility.Visible; // Significa que tem que esconde
                    break;
                case 11:
                    txtTituloNotificacao.Text = "Erro";
                    txtConteudo.Text = "As senhas não conferem.";
                    btnVermelho.Content = "sem texto";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;//significa que tem que aparecer
                    btnVerde.Visibility = Visibility.Visible; // Significa que tem que esconde
                    break;
                case 12:
                    txtTituloNotificacao.Text = "cadastrado com sucesso!";
                    txtConteudo.Text = "Bem-vindo";
                    btnVermelho.Content = "sem texto";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;//significa que tem que aparecer
                    btnVerde.Visibility = Visibility.Visible; // Significa que tem que esconde
                    break;
                case 13:
                    txtTituloNotificacao.Text = "cadastrado com sucesso!";
                    txtConteudo.Text = "Bem-vindo Professor!";
                    btnVermelho.Content = "sem texto";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;//significa que tem que aparecer
                    btnVerde.Visibility = Visibility.Visible; // Significa que tem que esconde
                    break;
                case 14:
                    txtTituloNotificacao.Text = "ERRO";
                    txtConteudo.Text = "Este Login/Email já está cadastrado.";
                    btnVermelho.Content = "sem texto";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;//significa que tem que aparecer
                    btnVerde.Visibility = Visibility.Visible; // Significa que tem que esconde
                    break;
                //TelaHome
                case 15:
                    txtTituloNotificacao.Text = "Boas Vindas!";
                    txtConteudo.Text = "Seja benvindo ao sistema";
                    btnVerde.Content = "Ok";
                    btnVerde.Visibility = Visibility.Visible;
                    btnVermelho.Visibility = Visibility.Collapsed;
                    break;
                case 16:
                    txtTituloNotificacao.Text = "Atenção!";
                    txtConteudo.Text = "Digite algo antes de postar!";
                    btnVerde.Content = "Ok";
                    btnVerde.Visibility = Visibility.Visible;
                    btnVermelho.Visibility = Visibility.Collapsed;
                    break;
                case 17: // CONFIRMAR SAÍDA
                    txtTituloNotificacao.Text = "Confirmar saída";
                    txtConteudo.Text = "Deseja realmente sair da sua conta?";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "Sair";
                    btnVermelho.Visibility = Visibility.Visible;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                //Tela degenciar Sala
                case 18: //Excluir um usuário da lista
                    txtTituloNotificacao.Text = "Excluir Registro";
                    txtConteudo.Text = "Tem certeza que deseja excluir este usuário?";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "Excluir";
                    btnVermelho.Visibility = Visibility.Visible;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                case 19: //Erro ao excluir um usuario da lista
                    txtTituloNotificacao.Text = "ERRO";
                    txtConteudo.Text = "Erro ao excluir usuário.";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                case 20: //Erro ao excluir um usuario da lista
                    txtTituloNotificacao.Text = "ERRO";
                    txtConteudo.Text = "Erro ao carregar histórico";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                case 21:
                    txtTituloNotificacao.Text = "ERRO";
                    txtConteudo.Text = "Erro ao carregar interações";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                //tela de gerenciar sessão
                case 22:
                    txtTituloNotificacao.Text = "ERRO";
                    txtConteudo.Text = "não foi possível localizar a pergunta";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                case 23: //Erro ao excluir um usuario da lista
                    txtTituloNotificacao.Text = "ERRO";
                    txtConteudo.Text = "Por favor, selecione uma sessão \n antes de criar uma pergunta.";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                case 24: //Erro ao excluir um usuario da lista
                    txtTituloNotificacao.Text = "Atenção";
                    txtConteudo.Text = "Por favor, insira um número válido \n para a alternativa correta (0-3).";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                case 25: //Erro ao excluir um usuario da lista
                    txtTituloNotificacao.Text = "Atenção, Enunciado Vazio!";
                    txtConteudo.Text = "Por favor, insira o enunciado da pergunta.";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                case 26:
                    txtTituloNotificacao.Text = "Alternativas Incompletas!";
                    txtConteudo.Text = "Por favor, preencha todas as alternativas.";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                case 27: 
                    txtTituloNotificacao.Text = "Alternativa Correta Fora do Range!";
                    txtConteudo.Text = "A alternativa correta deve \n ser um número entre 0 e 3.";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                case 28:
                    txtTituloNotificacao.Text = "ERRO!";
                    txtConteudo.Text = "Erro ao salvar pergunta.";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                case 29:
                    txtTituloNotificacao.Text = "Pergunta Não Selecionada";
                    txtConteudo.Text = "Por favor, selecione uma pergunta para excluir.";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                case 30:
                    txtTituloNotificacao.Text = "Sessão Não Selecionada";
                    txtConteudo.Text = "Por favor, selecione uma sessão para excluir.";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                case 31:
                    txtTituloNotificacao.Text = "ERRO!";
                    txtConteudo.Text = "Sessão não encontrada.";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                case 32:
                    txtTituloNotificacao.Text = "Atenção!";
                    txtConteudo.Text = "Selecione uma pergunta antes de atualizar.";
                    btnVermelho.Content = "Cancelar";
                    btnVerde.Content = "OK";
                    btnVermelho.Visibility = Visibility.Collapsed;
                    btnVerde.Visibility = Visibility.Visible;
                    break;
                case 33:
                    txtTituloNotificacao.Text = "Sucesso!";
                    txtConteudo.Text = "Pergunta atualizada com sucesso!";
                    btnVerde.Content = "OK";
                    btnVerde.Visibility = Visibility.Visible;
                    btnVermelho.Visibility = Visibility.Collapsed;
                    break;
                case 34:
                    txtTituloNotificacao.Text = "ERRO!";
                    txtConteudo.Text = "Erro ao atualizar pergunta";
                    btnVerde.Content = "OK";
                    btnVerde.Visibility = Visibility.Visible;
                    btnVermelho.Visibility = Visibility.Collapsed;
                    break;
                case 35:
                    txtTituloNotificacao.Text = "ERRO!";
                    txtConteudo.Text = "Por favor, insira o nome da sessão.";
                    btnVerde.Content = "OK";
                    btnVerde.Visibility = Visibility.Visible;
                    btnVermelho.Visibility = Visibility.Collapsed;
                    break;
                case 36:
                    txtTituloNotificacao.Text = "Sucesso!";
                    txtConteudo.Text = "Sessão criada com sucesso!";
                    btnVerde.Content = "OK";
                    btnVerde.Visibility = Visibility.Visible;
                    btnVermelho.Visibility = Visibility.Collapsed;
                    break;
                case 37:
                    txtTituloNotificacao.Text = "ERRO!";
                    txtConteudo.Text = "Erro ao criar sessão";
                    btnVerde.Content = "OK";
                    btnVerde.Visibility = Visibility.Visible;
                    btnVermelho.Visibility = Visibility.Collapsed;
                    break;
                case 38:
                    txtTituloNotificacao.Text = "Seucesso!";
                    txtConteudo.Text = "Sessão atualizada com sucesso!";
                    btnVerde.Content = "OK";
                    btnVerde.Visibility = Visibility.Visible;
                    btnVermelho.Visibility = Visibility.Collapsed;
                    break;
                default:
                    txtTituloNotificacao.Text = "Aviso";
                    txtConteudo.Text = "Notificação não reconhecida pelo sistema.";
                    btnVerde.Content = "OK";
                    btnVerde.Visibility = Visibility.Visible;
                    btnVermelho.Visibility = Visibility.Collapsed;
                    break;
                

                    // ✅ POPUP DE CONFIRMAÇÃO PARA EXCLUIR item da lista
                    /*
                    case 1:
                        

                    // ✅ POPUP DE CONFIRMAÇÃO DE SAÍDA
                    case 2:
                        txtTituloNotificacao.Text = "Sair do Sistema";
                        txtConteudo.Text = "Deseja realmente sair?";
                        btnVermelho.Content = "Não";
                        btnVerde.Content = "Sim";
                        btnVermelho.Visibility = Visibility.Visible;
                        btnVerde.Visibility = Visibility.Visible;
                        break;

                    // ✅ POPUP DE AVISO (SÓ UM BOTÃO)
                    case 3:
                        txtTituloNotificacao.Text = "Aviso";
                        txtConteudo.Text = "Operação realizada com sucesso!";
                        btnVerde.Content = "OK";
                        btnVerde.Visibility = Visibility.Visible;
                        btnVermelho.Visibility = Visibility.Collapsed;
                        break;

                    // ✅ POPUP DE ERRO
                    case 4:
                        txtTituloNotificacao.Text = "Erro";
                        txtConteudo.Text = "Algo deu errado ao processar a ação.";
                        btnVerde.Content = "Fechar";
                        btnVerde.Visibility = Visibility.Visible;
                        btnVermelho.Visibility = Visibility.Collapsed;
                        break;
                    //Boas vindas ao sistema
                    

                    // ✅ CASO PADRÃO (SEGURANÇA)
                    default:
                        txtTituloNotificacao.Text = "Notificação";
                        txtConteudo.Text = "Mensagem não identificada.";
                        btnVerde.Content = "OK";
                        btnVerde.Visibility = Visibility.Visible;
                        btnVermelho.Visibility = Visibility.Collapsed;
                        break;*/
            }

        }

    }
}
