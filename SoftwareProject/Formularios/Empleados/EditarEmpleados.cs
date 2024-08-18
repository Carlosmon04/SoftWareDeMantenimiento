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
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using SoftwareProject.Formularios.Formularios_de_DELETE;

namespace SoftwareProject.Formularios
{
    public partial class EditarEmpleados : Form
    {
        SqlConnection cnx;
        int EmpleadoID, jefeID;
        String Nombre, DNI, E_mail, Telefono, Direccion, Especializacion, Sueldo,EstadoF;
        bool Estado;

        private void chkJefe_CheckedChanged(object sender, EventArgs e)
        {

            if (chkJefe.Checked)
            {
                cbxJefe.Visible = true;
                llenarJefes();
            }
            else
                cbxJefe.Visible = false;

        }

        private int IDJefe()
        {
            try
            {

                int IDJefe = 0;
                SqlCommand cmd = new SqlCommand("Select EmpleadoID from Empleado where nombre = @nombre", cnx);
                cmd.Parameters.AddWithValue("@nombre", cbxJefe.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                cbxJefe.Items.Clear();

                while (reader.Read())
                {
                    IDJefe = reader.GetInt32(0);
                }

                reader.Close();
                return IDJefe;
                 
            }
            catch (SqlException ex)
            {

                MessageBox.Show("Ocurrio un Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return 0;
        }

        private void llenarJefes()
        {

            try
            {

                SqlCommand cmd = new SqlCommand("Select nombre from Empleado where JefeID is null",cnx);
                SqlDataReader reader = cmd.ExecuteReader();
                cbxJefe.Items.Clear();

                while (reader.Read())
                {
                    string nombreJefe = reader.GetString(0);
                    cbxJefe.Items.Add(nombreJefe);
                }

                reader.Close(); 

            }
            catch (SqlException ex)
            {

                MessageBox.Show("Ocurrio un Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private bool Validaciones()
        {
            // Limpiar errores previos
            error.Clear();

            bool esValido = true;

            
            if (txtNombre.Text.Length < 3)
            {
                error.SetError(txtNombre, "El nombre no puede tener menos de 3 letras");
                esValido = false;
            }

            if (txtDNI.Text.Length != 13 && txtDNI.Text.Length != 15 )
            {
                error.SetError(txtDNI, "La identidad debe tener 13 o 15 caracteres.");
                esValido = false;
            }

           
            if (!txtCorreo.Text.Contains("@") || !txtCorreo.Text.Contains("."))
            {
                error.SetError(txtCorreo, "El correo no es válido.");
                esValido = false;
            }

            if (txtTelefono.Text.Length < 6)
            {
                error.SetError(txtTelefono, "El número de teléfono no es válido.");
                esValido = false;
            }

            if (txtDireccion.Text.Length == 0)
            {
                error.SetError(txtDireccion, "La dirección no puede estar vacía.");
                esValido = false;
            }

            
            if (cbxEsp.SelectedItem == null || cbxEsp.SelectedItem.ToString() == "")
            {
                error.SetError(cbxEsp, "Debe seleccionar una especialización para el usuario.");
                esValido = false;
            }

            if (!int.TryParse(txtSueldo.Text, out int sueldo))
            {
                error.SetError(txtSueldo, "El sueldo debe ser un número válido.");
                esValido = false;
            }
            else if (sueldo < 0)
            {
                error.SetError(txtSueldo, "El sueldo no puede ser negativo.");
                esValido = false;
            }

            if (chkJefe.Checked)
            {
                if (cbxJefe.SelectedItem == null || cbxJefe.SelectedItem.ToString() == "")
                {
                    error.SetError(cbxJefe, "Debe seleccionar un Jefe.");
                    esValido = false;
                }
            }

            return esValido;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {

            if (Validaciones())
            {
                try
                {
                    int Jefeid = jefeID;
                    SqlCommand cmd = new SqlCommand("spEditarEmpleados", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (chkEstado.Checked)
                    {
                        EstadoF = "A";
                    }
                    else
                    {
                        EstadoF = "N";
                    }

                    if (chkJefe.Checked)
                    {
                        Jefeid = IDJefe();
                    }

                    cmd.Parameters.AddWithValue("@Name", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@DNI", txtDNI.Text);
                    cmd.Parameters.AddWithValue("@Email", txtCorreo.Text);
                    cmd.Parameters.AddWithValue("@Cel", txtTelefono.Text);
                    cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text);
                    cmd.Parameters.AddWithValue("@Area", cbxEsp.SelectedItem);
                    cmd.Parameters.AddWithValue("@Sueldo", txtSueldo.Text);
                    cmd.Parameters.AddWithValue("@Estado", EstadoF);
                    cmd.Parameters.AddWithValue("@EmpleadoID", EmpleadoID);
                    if (Jefeid != 0)
                    {
                        cmd.Parameters.AddWithValue("@Jefe", Jefeid);
                    }
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    MessageBox.Show("Los cambios se realizaron con exito", "Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

                    if (form1 != null)
                    {
                        form1.OpenChildForm(new DeleteEmpleados(cnx));
                    }

                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ocurrio un Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();

            Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

            if (form1 != null)
            {
                form1.OpenChildForm(new DeleteEmpleados(cnx));
            }
        }

       
        public EditarEmpleados(SqlConnection conexion, String nombre,String dni,String Correo,String Tlf,String Drc,String Esp,String salario, bool Etd, int usuario, int jefe)
        {
            InitializeComponent();
            cnx = conexion;
            Nombre = nombre;
            DNI = dni;
            E_mail = Correo;
            Telefono = Tlf;
            Direccion = Drc;
            Especializacion = Esp;
            Sueldo = salario;
            Estado = Etd;
            EmpleadoID = usuario;
            jefeID = jefe;
        }

        private void EditarEmpleados_Load(object sender, EventArgs e)
        {
            String[] Areas = { "Administracion Gerencial", "Tecnico Redes" };
            cbxEsp.Items.AddRange(Areas);
            txtNombre.Text = Nombre;
            txtDNI.Text = DNI;
            txtCorreo.Text = E_mail;
            txtTelefono.Text = Telefono;
            txtDireccion.Text = Direccion;
            cbxEsp.Text = Especializacion;
            txtSueldo.Text = Sueldo;
            cbxJefe.Visible = false;
        
            if (Estado)
            {
                chkEstado.Checked = true;
            }
            else 
            {
                chkEstado.Checked = false;
            }
        }

    }
}
