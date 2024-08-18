using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SoftwareProject.Formularios.CRUD_Clientes
{
    public partial class ConsultarClientes : Form
    {
        private SqlConnection cnx;
        private DataTable TabClientes;
        private DataTable TabClientesInactivos;
        
        public ConsultarClientes(SqlConnection conexion)
        {
            InitializeComponent();
            cnx = conexion;
            //probando git
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ConsultarClientes_Load(object sender, EventArgs e)
        {
            String consulta = "Select * from vConsultarClientes";
            try
            {
                
                    TabClientes = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(consulta, cnx);
                    adapter.Fill(TabClientes);
                    dataGridView1.DataSource = TabClientes;
                    dataGridView1.ReadOnly = true;
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridView1.ScrollBars = ScrollBars.Both;
                    Estado();
           
                

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private void Estado()
        {
            if (dataGridView1.Columns.Contains("Estado"))
            {
                dataGridView1.Columns.Remove("Estado");

                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
                {
                    Name = "Estado",
                    HeaderText = "Estado",
                    Width = 50,
                    FillWeight = 50,
                    DataPropertyName = "Estado"
                };

                dataGridView1.Columns.Add(checkBoxColumn);

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["Estado"] != null)
                    {
                        string check = row.Cells["Estado"].Value.ToString();

                        row.Cells["Estado"].Value = check == "A";
                    }
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkInactivos.Checked == false)
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        int EmpleadoUserId = (int)TabClientes.DefaultView[dataGridView1.CurrentRow.Index]["Clienteid"];

                        EditarCliente r = new EditarCliente(cnx, EmpleadoUserId);
                        r.Visible = true;
                    }
                }

                if (chkInactivos.Checked == true)
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        int Inactivos = (int)TabClientesInactivos.DefaultView[dataGridView1.CurrentRow.Index]["Clienteid"];

                        EditarCliente r = new EditarCliente(cnx, Inactivos);
                        r.Visible = true;
                    }
                }

            }
            catch (SqlException ex)
            { MessageBox.Show("" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
        }




        private void btnRegresarC_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

        }

        private void chkInactivos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dataTable = new DataTable();
                string consulta = chkInactivos.Checked ? "Select * from vClientesInactivos" : "Select * from vConsultarClientes";

                using (SqlDataAdapter adapter = new SqlDataAdapter(consulta, cnx))
                {
                    adapter.Fill(dataTable);
                }

                dataGridView1.DataSource = dataTable;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.ScrollBars = ScrollBars.Both;

                if (chkInactivos.Checked)
                {
                    TabClientesInactivos = dataTable;
                }
                else
                {
                    TabClientes = dataTable;
                }

                Estado();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            string busqueda = txtBusqueda.Text;

            string filtro = string.Format("Convert(DNI, 'System.String') like '%{0}%'", busqueda);

            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filtro;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Si el checkbox NO está marcado, trabaja con TabClientes
                if (!chkInactivos.Checked)
                {
                    int EmpleadoUserId = (int)TabClientes.DefaultView[dataGridView1.CurrentRow.Index]["UsuarioId"];

                    SqlCommand cmd = new SqlCommand("spDeleteClientes", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@user", EmpleadoUserId);

                    if (MessageBox.Show("¿Estás seguro que quieres borrar este registro?", "CUIDADO", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                    {
                        cmd.ExecuteNonQuery();

                        // Refresca el DataGridView
                        String consulta = "Select * from vConsultarClientes";
                        TabClientes = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(consulta, cnx);
                        adapter.Fill(TabClientes);
                        dataGridView1.DataSource = TabClientes;
                        dataGridView1.ReadOnly = true;
                        dataGridView1.AllowUserToAddRows = false;
                        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridView1.ScrollBars = ScrollBars.Both;
                        Estado();

                        MessageBox.Show("Se ha eliminado exitosamente", "LISTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmd.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("El proceso de eliminación fue cancelado.", "CANCELADO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else // Si el checkbox ESTÁ marcado, trabaja con TabClientesInactivos
                {
                    int EmpleadoUser = (int)TabClientesInactivos.DefaultView[dataGridView1.CurrentRow.Index]["UsuarioId"];

                    SqlCommand cmd2 = new SqlCommand("spDeleteClientesInactivos", cnx);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@user", EmpleadoUser);

                    if (MessageBox.Show("¿Estás seguro que quieres borrar este registro?", "???", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cmd2.ExecuteNonQuery();

                        // Refresca el DataGridView
                        TabClientesInactivos = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter("Select * from vClientesInactivos", cnx);
                        adapter.Fill(TabClientesInactivos);
                        dataGridView1.DataSource = TabClientesInactivos;
                        dataGridView1.ReadOnly = true;
                        dataGridView1.AllowUserToAddRows = false;
                        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridView1.ScrollBars = ScrollBars.Both;
                        Estado();

                        MessageBox.Show("Se ha eliminado exitosamente", "LISTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmd2.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("El proceso de eliminación fue cancelado.", "CANCELADO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
