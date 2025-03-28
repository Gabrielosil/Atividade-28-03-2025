using System;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GerarIdAleatorio(); // Gera um ID ao iniciar o formulário
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            bool sucesso = SalvarUsuario();

            if (sucesso)
            {
                MessageBox.Show("Usuário salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GerarIdAleatorio(); // Gera um novo ID para o próximo usuário
                txtNome.Clear();
                txtTelefone.Clear();
            }
            else
            {
                MessageBox.Show("Erro ao salvar o usuário!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GerarIdAleatorio()
        {
            Random random = new Random();
            txtId.Text = random.Next(1000, 9999).ToString(); // Gera um ID entre 1000 e 9999
            txtId.ReadOnly = true; // Impede edição do ID
        }

        public bool SalvarUsuario()
        {
            try
            {
                // Verifica se o telefone tem 11 dígitos
                if (txtTelefone.Text.Length != 11)
                {
                    MessageBox.Show("O telefone deve ter exatamente 11 dígitos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Criando um novo usuário com os dados do formulário
                Usuario novoUsuario = new Usuario
                {
                    Id = Convert.ToInt32(txtId.Text), // O ID já foi gerado automaticamente
                    Nome = txtNome.Text.Trim(),
                    Telefone = txtTelefone.Text.Trim()
                };

                // Chamada do método para salvar no banco de dados
                return Database.SalvarUsuario(novoUsuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void txtNome_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas letras, tecla Backspace e espaço
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTelefone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite apenas números e tecla Backspace
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTelefone_TextChanged(object sender, EventArgs e)
        {
            // Limita a 11 caracteres no telefone
            if (txtTelefone.Text.Length > 11)
            {
                txtTelefone.Text = txtTelefone.Text.Substring(0, 11);
                txtTelefone.SelectionStart = txtTelefone.Text.Length; // Mantém o cursor no final
            }
        }
    }
}
