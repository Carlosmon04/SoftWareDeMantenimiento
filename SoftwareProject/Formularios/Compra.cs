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

namespace SoftwareProject.Formularios
{
    public partial class Compra : Form
    {
        private SqlConnection cnx;
        private int userID;

<<<<<<< HEAD
        SqlCommand cmd3;
        SqlDataReader data;
=======
        SqlCommand cmd;
>>>>>>> ee1d9478137acdd7a8e8f785df6cf51d7f1f35be
        public Compra()
        {
            InitializeComponent();
        }
        public Compra(SqlConnection conexion, int usuario)
        {
            InitializeComponent();
            cnx = conexion;
            userID = usuario;
        }
        private void Compra_Load(object sender, EventArgs e)
        {
<<<<<<< HEAD
            
            cmbMedida.SelectedIndex.ToString();
=======

>>>>>>> ee1d9478137acdd7a8e8f785df6cf51d7f1f35be
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verificar si la tecla presionada es un número o una tecla de control (como Backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten números en este campo.", "Entrada no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
<<<<<<< HEAD
                        SqlCommand cmd3 = new SqlCommand("spCompra", cnx);
                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.Parameters.AddWithValue("@NombreArt", txtArticulo.Text);
                        cmd3.Parameters.AddWithValue("@Cantidad", txtCantidad.Text);
                        cmd3.Parameters.AddWithValue("@TipoMedida", cmbMedida.SelectedIndex.ToString());
                        cmd3.Parameters.AddWithValue("@Pro", txtProveedor.Text);
                        cmd3.Parameters.AddWithValue("@Costo", txtCosto.Text);
                        cmd3.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text);
                        cmd3.ExecuteNonQuery();

                txtArticulo.Clear(); txtCantidad.Clear(); txtProveedor.Clear(); txtCosto.Clear(); txtDescripcion.Clear();
=======
                SqlCommand cmd = new SqlCommand("spCompra", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreArt",txtArticulo.Text);
                cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();
>>>>>>> ee1d9478137acdd7a8e8f785df6cf51d7f1f35be


            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un Error " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
<<<<<<< HEAD

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }
=======
>>>>>>> ee1d9478137acdd7a8e8f785df6cf51d7f1f35be
    }
}
