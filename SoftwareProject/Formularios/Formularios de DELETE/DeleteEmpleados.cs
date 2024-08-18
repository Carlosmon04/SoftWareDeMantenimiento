using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SoftwareProject.Formularios.Formularios_de_DELETE
{
    public partial class DeleteEmpleados : Form
    {
        SqlConnection cnx;
        DataTable TabEmpleados;

        public DeleteEmpleados(SqlConnection conexion)
        {
            InitializeComponent();
            cnx= conexion;
        }

        private void DeleteEmpleados_Load(object sender, EventArgs e)
        {
            
            try
            {
                TabEmpleados = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("select e.EmpleadoId, e.Nombre, e.DNI, e.E_mail, e.Telefono, E.Direccion, E.Especializacion, e.Sueldo, e.jefeId, E.UsuarioId, u.Estado\r\n" +
                    "from Empleado as e inner join Usuarios as u on e.UsuarioId = u.UsuarioId where u.estado = 'A' ", cnx);
                adapter.Fill(TabEmpleados);
                dataGridView1.DataSource = TabEmpleados;
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


        private void CargarInactivos()
        {
            TabEmpleados = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("select e.EmpleadoId, e.Nombre, e.DNI, e.E_mail, e.Telefono, E.Direccion, E.Especializacion, e.Sueldo, e.jefeId, E.UsuarioId, u.Estado\r\n" +
                "from Empleado as e inner join Usuarios as u on e.UsuarioId = u.UsuarioId where u.estado = 'I'", cnx);
            adapter.Fill(TabEmpleados);
            dataGridView1.DataSource = TabEmpleados;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ScrollBars = ScrollBars.Both;
            Estado();
            dataGridView1.Refresh();
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

        private SqlCommand DeleteEmpleadosProc(SqlConnection conexion, int usuarioID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spDeleteEmpleados", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user", usuarioID);
                DialogResult r = MessageBox.Show("Estas Seguro que quieres Eliminar al Usuario" + usuarioID + "?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    cmd.ExecuteNonQuery();
                    TabEmpleados = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter("select e.EmpleadoId, e.Nombre, e.DNI, e.E_mail, e.Telefono, E.Direccion, E.Especializacion, e.Sueldo, e.jefeId, E.UsuarioId, u.Estado\r\n" +
                    "from Empleado as e inner join Usuarios as u on e.UsuarioId = u.UsuarioId where u.estado = 'A'", cnx);
                    adapter.Fill(TabEmpleados);
                    dataGridView1.DataSource = TabEmpleados;
                    Estado();
                }
                cmd.Dispose();
                return cmd;
            }
            catch (SqlException ex)
            {
               MessageBox.Show("Ocurrio un Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return null;
        }

        private void EditarEmpleados(SqlConnection cnx, int rowActual)
        {
            int EmpleadoID,Jefe;
            String nombre, DNI, Correo, Telefono, Direccion, Esp, salario; 
            bool Estado;

            try
            {
                EmpleadoID = (int)TabEmpleados.DefaultView[dataGridView1.CurrentRow.Index]["EmpleadoID"];
                nombre = TabEmpleados.DefaultView[dataGridView1.CurrentRow.Index]["nombre"].ToString();
                DNI = TabEmpleados.DefaultView[dataGridView1.CurrentRow.Index]["DNI"].ToString();
                Correo = TabEmpleados.DefaultView[dataGridView1.CurrentRow.Index]["E_mail"].ToString();
                Telefono = TabEmpleados.DefaultView[dataGridView1.CurrentRow.Index]["Telefono"].ToString();
                Direccion = TabEmpleados.DefaultView[dataGridView1.CurrentRow.Index]["direccion"].ToString();
                Esp = TabEmpleados.DefaultView[dataGridView1.CurrentRow.Index]["Especializacion"].ToString();
                salario = TabEmpleados.DefaultView[dataGridView1.CurrentRow.Index]["sueldo"].ToString();
                string estadoString = TabEmpleados.DefaultView[dataGridView1.CurrentRow.Index]["Estado"].ToString();

                Jefe = TabEmpleados.DefaultView[dataGridView1.CurrentRow.Index]["JefeID"] 
                       != DBNull.Value ? (int)TabEmpleados.DefaultView[dataGridView1.CurrentRow.Index]["JefeID"] : 0; //Si se recupera un jefeID nulo se enviara como 0
                Estado = estadoString == "True";

                Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

                if (form1 != null)
                {
                    form1.OpenChildForm(new EditarEmpleados(cnx, nombre, DNI, Correo, Telefono, Direccion, Esp, salario, Estado, EmpleadoID,Jefe));
                }

            } catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }                
        }


        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            string busqueda = txtBusqueda.Text;

            string filtro = string.Format("Convert(DNI, 'System.String') like '%{0}%'", busqueda);

            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filtro;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int EmpleadoUserId = (int)TabEmpleados.DefaultView[dataGridView1.CurrentRow.Index]["UsuarioId"];
            DeleteEmpleadosProc(cnx, EmpleadoUserId);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                CargarInactivos();
            }
            else
            {
                TabEmpleados = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("select e.EmpleadoId, e.Nombre, e.DNI, e.E_mail, e.Telefono, E.Direccion, E.Especializacion, e.Sueldo, e.jefeId, E.UsuarioId, u.Estado\r\n" +
                    "from Empleado as e inner join Usuarios as u on e.UsuarioId = u.UsuarioId where u.estado = 'A' ", cnx);
                adapter.Fill(TabEmpleados);
                dataGridView1.DataSource = TabEmpleados;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.ScrollBars = ScrollBars.Both;
                Estado();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int EmpleadoUserId = (int) TabEmpleados.DefaultView[dataGridView1.CurrentRow.Index]["EmpleadoId"];

            if (dataGridView1.SelectedRows.Count > 0)
            {
                EditarEmpleados(cnx, EmpleadoUserId);
            }
            else 
            {
                MessageBox.Show("Por favor seleccione una fila para editar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
