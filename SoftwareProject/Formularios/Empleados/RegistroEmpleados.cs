using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareProject.Formularios
{
    public partial class RegistroEmpleados : Form
    {
        String url = "Server= 3.128.144.165;" +
                           "DataBase= DB20222000953;" +
                           "User ID= david.rodriguez;" +
                           "password= DR20222000953;";
        SqlConnection conexion;
        private SqlConnection cnx;
        private int userID;
        public RegistroEmpleados(SqlConnection conexion, int usuario)
        {
            InitializeComponent();
            cnx= conexion;
            userID = usuario;
            conexion = new SqlConnection(url);
        }

 
        private void RegistroEmpleados_Load(object sender, EventArgs e)
        {
            String[] Areas = { "Administracion Gerencial", "Tecnico Redes" };
            cmbAreas.Items.AddRange(Areas);
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool Validaciones()
        {
            // Limpiar errores previos
            error.Clear();

            bool esValido = true;


            if (txtNombre.Text.Length < 3 || txtNombre.Text == "Nombre")
            {
                error.SetError(txtNombre, "El nombre no es valido");
                esValido = false;
            }

            if (txtDNI.Text.Length != 13 && txtDNI.Text.Length != 15 || txtDNI.Text == "Identidad")
            {
                error.SetError(txtDNI, "La identidad no es valida");
                esValido = false;
            }


            if (!txtMail.Text.Contains("@") || !txtMail.Text.Contains(".") || txtMail.Text == "Correo")
            {
                error.SetError(txtMail, "El correo no es válido.");
                esValido = false;
            }

            if (txtTelefono.Text.Length < 6 || txtTelefono.Text == "Telefono")
            {
                error.SetError(txtTelefono, "El número de teléfono no es válido.");
                esValido = false;
            }

            if (txtDireccion.Text.Length == 0 || txtDireccion.Text == "Direccion")
            {
                error.SetError(txtDireccion, "La dirección no es valida.");
                esValido = false;
            }

            if (!int.TryParse(txtSueldo.Text, out int sueldo) || txtSueldo.Text == "Sueldo LPS.")
            {
                error.SetError(txtSueldo, "El sueldo debe ser un número válido.");
                esValido = false;
            }
            else if (sueldo < 0)
            {
                error.SetError(txtSueldo, "El sueldo no puede ser negativo.");
                esValido = false;
            }

            if(txtPass.Text.Length < 3 || txtPass.Text == "Contraseña")
            {
                error.SetError(txtPass, "La contraseña no es valida");
                esValido = false;
            }

            if(cmbAreas.SelectedItem == null || cmbAreas.SelectedItem.ToString() == "")
            {
                error.SetError(cmbAreas, "Seleccione una especialidad");
                esValido = false;
            }

            if (txtUsername.Text == "Username")
            {
                error.SetError(txtUsername, "El username no es valido");
                esValido = false;
            }

            return esValido;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (UsuarioExiste(txtUsername.Text))
            {
                error.SetError(txtUsername, "El usuario ya existe");
            }
            else if(Validaciones())
            {

                try
                {
                    Autorizado(cnx, userID);
                    Commando(cnx, userID);
                    MessageBox.Show("Se ha Registrado Exitosamente", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {

                    MessageBox.Show("Ocurrio un Error " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }

        }

        private SqlCommand Commando(SqlConnection conexion, int user) {

            try
            {
                SqlCommand cmd = new SqlCommand("spRegistrarEmpleadosJefes", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                String consulta = "Select * from Empleado where UsuarioId = @user";
                SqlCommand leerID = new SqlCommand(consulta, conexion);
                leerID.Parameters.AddWithValue("@user", user);
                SqlDataReader reader = leerID.ExecuteReader();
                int id = 0;

                if (reader.Read())
                {
                    id = Convert.ToInt32(reader["EmpleadoId"]);
                }
                reader.Close();
                leerID.Dispose();

                cmd.Parameters.AddWithValue("@Name", txtNombre.Text);
                cmd.Parameters.AddWithValue("@DNI", txtDNI.Text);
                cmd.Parameters.AddWithValue("@Email", txtMail.Text);
                cmd.Parameters.AddWithValue("@Cel", txtTelefono.Text);
                cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text);
                cmd.Parameters.AddWithValue("@Area", cmbAreas.SelectedItem);
                cmd.Parameters.AddWithValue("@Sueldo", txtSueldo.Text);
                cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                bool x = CHKBox();
                if (x == true)
                {
                    cmd.Parameters.AddWithValue("@Jefe", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Jefe", id);
                }

                cmd.Parameters.AddWithValue("Pass", txtPass.Text);

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return cmd;
            }
            catch (SqlException ex)
            {
              MessageBox.Show("Ocurrio un Error "+ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
           return null;
        }

        private bool CHKBox() 
        {
            bool ConfirmarF = false;

            if (chkJefe.Checked)
            {
                ConfirmarF = true;
            }
            else
            {
                ConfirmarF = false;
            }
            return ConfirmarF;
        }
        private bool Autorizado(SqlConnection conexion, int user)
        {
            bool acceso = false;
            try
            {

                SqlCommand cmd = new SqlCommand("spJefes", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", user);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read()) {
                if (reader.IsDBNull(0))
                {
                    acceso = true;
                    Console.WriteLine("SI FUnciona");
                }
            }
                else
                {
                    Console.WriteLine("Nuuuu");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un Error " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return acceso;
        }
        private bool UsuarioExiste(string nombreUsuario)
        {
            bool existe = false;
            try
            {
                SqlConnection cnx = new SqlConnection(url);

                cnx.Open();
                SqlCommand cmd = new SqlCommand("SELECT dbo.fnUsuarioExiste(@NombreUsuario)", cnx);

                cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                existe = (bool)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar si el usuario existe: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return existe;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Enter(object sender, EventArgs e)
        {
            if (txtNombre.Text == "Nombre")
            {
                txtNombre.Text = "";
            }
        }

        private void txtNombre_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                txtNombre.Text = "Nombre";
            }
        }

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Username")
            {
                txtUsername.Text = "";
            }
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Username";
            }
        }

        private void txtDNI_Enter(object sender, EventArgs e)
        {
            if (txtDNI.Text == "Identidad")
            {
                txtDNI.Text = "";
            }
        }

        private void txtDNI_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                txtDNI.Text = "Identidad";
            }
        }

        private void txtPass_Enter(object sender, EventArgs e)
        {
            if (txtPass.Text == "Contraseña")
            {
                txtPass.Text = "";
            }
        }

        private void txtPass_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPass.Text))
            {
                txtPass.Text = "Contraseña";
            }
        }

        private void txtMail_Enter(object sender, EventArgs e)
        {
            if (txtMail.Text == "Correo")
            {
                txtMail.Text = "";
            }
        }

        private void txtMail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMail.Text))
            {
                txtMail.Text = "Correo";
            }
        }

        private void txtTelefono_Enter(object sender, EventArgs e)
        {
            if (txtTelefono.Text == "Telefono")
            {
                txtTelefono.Text = "";
            }
        }

        private void txtTelefono_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                txtTelefono.Text = "Telefono";
            }
        }

        private void txtDireccion_Enter(object sender, EventArgs e)
        {
            if (txtDireccion.Text == "Direccion")
            {
                txtDireccion.Text = "";
            }
        }

        private void txtDireccion_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                txtDireccion.Text = "Direccion";
            }
        }

        private void txtSueldo_Enter(object sender, EventArgs e)
        {
            if (txtSueldo.Text == "Sueldo LPS.")
            {
                txtSueldo.Text = "";
            }
        }

        private void txtSueldo_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSueldo.Text))
            {
                txtSueldo.Text = "Sueldo LPS.";
            }
        }
    }
}
