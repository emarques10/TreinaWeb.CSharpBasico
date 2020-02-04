using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgendaContatos
{
    public partial class frmAgendaContatos : Form
    {
        private OperacaoEnum acao;
        public frmAgendaContatos()
        {
            InitializeComponent();
        }

        private void frmAgendaContatos_Shown(object sender, EventArgs e)
        {
            AlterarBotoesSalvarECancelar(false);
            AlterarBotoesIncluirAlterarExcluir(true);
            CarregarListaContatos();
            AlterarEstadoCampos(false);
        }

        private void AlterarBotoesSalvarECancelar(bool estado)
        {
            btnSalvar.Enabled = estado;
            btnCancelar.Enabled = estado;
        }

        private void AlterarBotoesIncluirAlterarExcluir(bool estado)
        {
            btnIncluir.Enabled = estado;
            btnAlterar.Enabled = estado;
            btnExcluir.Enabled = estado;
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            AlterarBotoesSalvarECancelar(true);
            AlterarBotoesIncluirAlterarExcluir(false);
            AlterarEstadoCampos(true);
            acao = OperacaoEnum.INCLUIR;
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            AlterarBotoesSalvarECancelar(true);
            AlterarBotoesIncluirAlterarExcluir(false);
            AlterarEstadoCampos(true);
            acao = OperacaoEnum.ALTERAR;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Tem certeza?", "Pergunta", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int indiceExcluido = lxbContatos.SelectedIndex;
                lxbContatos.SelectedIndex = 0;
                lxbContatos.Items.RemoveAt(indiceExcluido);
                List<Contato> contatosList = new List<Contato>();
                foreach(Contato contato in lxbContatos.Items)
                {
                    contatosList.Add(contato);
                }
                ManipuladorArquivos.EscreverArquivo(contatosList);
                CarregarListaContatos();
                LimparCampos();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            AlterarBotoesSalvarECancelar(false);
            AlterarBotoesIncluirAlterarExcluir(true);
            AlterarEstadoCampos(false);

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Contato contato = new Contato
            {
                Nome = txbNome.Text,
                Email = txbEmail.Text,
                NumeroTelefone = txbNumeroTelefone.Text
            };
            List<Contato> contatosList = new List<Contato>();
            foreach (Contato contatoDaLista in lxbContatos.Items)
            {
                contatosList.Add(contatoDaLista);
            }
            if(acao == OperacaoEnum.INCLUIR)
            {
                contatosList.Add(contato);
            }
            else
            {
                int indice = lxbContatos.SelectedIndex;
                contatosList.RemoveAt(indice);
                contatosList.Insert(indice, contato);
            }

            ManipuladorArquivos.EscreverArquivo(contatosList);
            CarregarListaContatos();
            AlterarBotoesSalvarECancelar(false);
            AlterarBotoesIncluirAlterarExcluir(true);
            LimparCampos();
            AlterarEstadoCampos(false);
        }

        private void CarregarListaContatos()
        {
            lxbContatos.Items.Clear();
            lxbContatos.Items.AddRange(ManipuladorArquivos.LerArquivo().ToArray());
        }

        private void LimparCampos()
        {
            txbNome.Text = "";
            txbEmail.Text = "";
            txbNumeroTelefone.Text = "";
        }

        private void AlterarEstadoCampos(bool estado)
        {
            txbNome.Enabled = estado;
            txbEmail.Enabled = estado;
            txbNumeroTelefone.Enabled = estado;
        }

        private void lxbContatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Contato contato = (Contato)lxbContatos.Items[lxbContatos.SelectedIndex];
            txbNome.Text = contato.Nome;
            txbEmail.Text = contato.Email;
            txbNumeroTelefone.Text = contato.NumeroTelefone;
        }
    }
}
