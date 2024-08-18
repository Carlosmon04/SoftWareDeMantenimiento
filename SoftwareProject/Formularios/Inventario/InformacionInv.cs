using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareProject.Formularios.Inventario
{
    public partial class InformacionInv : Form
    {
        SqlConnection cnx;
        DataTable TabInventario;
        public InformacionInv(SqlConnection conexion)
        {
            InitializeComponent();
            cnx = conexion;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InformacionInv_Load(object sender, EventArgs e)
        {
            try
            {
                TabInventario = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("select * from vInventario", cnx);
                adapter.Fill(TabInventario);
                dataGridView1.DataSource = TabInventario;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.ScrollBars = ScrollBars.Both;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDescontinuados()
        {
            TabInventario = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from vInventarioD", cnx);
            adapter.Fill(TabInventario);
            dataGridView1.DataSource = TabInventario;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.Refresh();
        }

        private void EditarInventario(SqlConnection cnx)
        {
            String Articulo, Descripcion, Medida, Existencia, Proveedor, Rentabilidad, Estado;
            int ArticuloID;

            try
            {
                ArticuloID = (int)TabInventario.DefaultView[dataGridView1.CurrentRow.Index]["ArticuloID"];
                Articulo = TabInventario.DefaultView[dataGridView1.CurrentRow.Index]["Articulo"].ToString();
                Descripcion = TabInventario.DefaultView[dataGridView1.CurrentRow.Index]["Descripcion"].ToString();
                Medida = TabInventario.DefaultView[dataGridView1.CurrentRow.Index]["Medida"].ToString();
                Existencia = TabInventario.DefaultView[dataGridView1.CurrentRow.Index]["Existencia"].ToString();
                Proveedor = TabInventario.DefaultView[dataGridView1.CurrentRow.Index]["Proveedor"].ToString();
                Rentabilidad = TabInventario.DefaultView[dataGridView1.CurrentRow.Index]["Rentabilidad"].ToString();
                Estado = TabInventario.DefaultView[dataGridView1.CurrentRow.Index]["Estado"].ToString();

                Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

                if (form1 != null)
                {
                    form1.OpenChildForm(new EditarInv(cnx, ArticuloID, Articulo, Descripcion, Medida, Existencia, Proveedor, Rentabilidad, Estado));
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private SqlCommand DeleteInventario(SqlConnection conexion, int ArticuloID, String nombre, String Estado)
        {
            if (Estado == "V")
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spEliminarInv", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ArticuloID", ArticuloID);
                    DialogResult r = MessageBox.Show("Estas Seguro que quieres eliminar al Articulo: " + nombre + "?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (r == DialogResult.Yes)
                    {
                        cmd.ExecuteNonQuery();
                        TabInventario = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter("select * from vInventario", cnx);
                        adapter.Fill(TabInventario);
                        dataGridView1.DataSource = TabInventario;
                    }
                    cmd.Dispose();
                    return cmd;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ocurrio un Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spEliminarInvI", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Articulo", ArticuloID);
                    DialogResult r = MessageBox.Show("Estas Seguro que quieres eliminar al Articulo: " + nombre + "?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (r == DialogResult.Yes)
                    {
                        cmd.ExecuteNonQuery();
                        TabInventario = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter("select * from vInventario", cnx);
                        adapter.Fill(TabInventario);
                        dataGridView1.DataSource = TabInventario;
                    }
                    cmd.Dispose();
                    return cmd;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ocurrio un Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            return null;
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            string busqueda = txtBusqueda.Text;

            string filtro = string.Format("Convert(Articulo, 'System.String') like '%{0}%'", busqueda);

            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filtro;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int ArticuloId = (int)TabInventario.DefaultView[dataGridView1.CurrentRow.Index]["ArticuloID"];
            String Nombre = TabInventario.DefaultView[dataGridView1.CurrentRow.Index]["Articulo"].ToString();
            String Estado = TabInventario.DefaultView[dataGridView1.CurrentRow.Index]["Estado"].ToString();
            DeleteInventario(cnx, ArticuloId, Nombre, Estado);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                CargarDescontinuados();
            }
            else
            {
                TabInventario = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("select * from vInventario", cnx);
                adapter.Fill(TabInventario);
                dataGridView1.DataSource = TabInventario;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.ScrollBars = ScrollBars.Both;
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                EditarInventario(cnx);
            }
            else
            {
                MessageBox.Show("Por favor seleccione una fila para editar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
