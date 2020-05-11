using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MySqlCRUD
{
    public partial class frmPrincipal : Form
    {
        string connectionString = @"Server=localhost;Database=bikcraftdb;Uid=root;Pwd=admin;";
        int idCliente = 0;
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("ClienteAddOuEdit", mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_idCliente", idCliente);
                mySqlCmd.Parameters.AddWithValue("_NomeCli", txtNome.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_EnderecoCli", txtEndereco.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_CidadeCli", txtCidade.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_TelCli", txtTelefone.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_EmailCli", txtEmail.Text.Trim());
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Salvo com sucesso.");
                Clear();
                GridFill();
            }
        }
        void GridFill()
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("ClienteViewAll", mysqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtbCliente = new DataTable();
                sqlDa.Fill(dtbCliente);
                dgvCliente.DataSource = dtbCliente;
                dgvCliente.Columns[0].Visible = false;
            }

        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Clear();
            GridFill();
        }

        void Clear()
        {
            txtNome.Text = txtEndereco.Text = txtCidade.Text = txtTelefone.Text = txtEmail.Text = "";
            idCliente = 0;
            btnSalvar.Text = "Salvo";
            btnDeletar.Enabled = false;
        }

        private void dgvCliente_DoubleClick(object sender, EventArgs e)
        {
            if(dgvCliente.CurrentRow.Index != -1)
            {
                txtNome.Text = dgvCliente.CurrentRow.Cells[1].Value.ToString();
                txtEndereco.Text = dgvCliente.CurrentRow.Cells[2].Value.ToString();
                txtCidade.Text = dgvCliente.CurrentRow.Cells[3].Value.ToString();
                txtTelefone.Text = dgvCliente.CurrentRow.Cells[4].Value.ToString();
                txtEmail.Text = dgvCliente.CurrentRow.Cells[5].Value.ToString();
                idCliente = Convert.ToInt32(dgvCliente.CurrentRow.Cells[0].Value.ToString());
                btnSalvar.Text = "Atualizar";
                btnDeletar.Enabled = true;
            }
        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("ClienteSearchByValue", mysqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("_SearchValue", txtProcurar.Text);
                DataTable dtbCliente = new DataTable();
                sqlDa.Fill(dtbCliente);
                dgvCliente.DataSource = dtbCliente;
                dgvCliente.Columns[0].Visible = false;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("ClienteDeleteByID", mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_idCliente", idCliente);
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Deletado com sucesso.");
                Clear();
                GridFill();
            }
        }
    }
}
