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
using SoftwareProject.Formularios.Inventario;

namespace SoftwareProject.Formularios.Inventario
{
    public partial class EditarInv : Form
    {
        SqlConnection cnx;
        String Articulo, Descripcion, Medida, Existencia, Proveedor, Rentabilidad, Estado;
<<<<<<< HEAD
        private int UserId;

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
=======
>>>>>>> ee1d9478137acdd7a8e8f785df6cf51d7f1f35be

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();

            Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

            if (form1 != null)
            {
<<<<<<< HEAD
                form1.OpenChildForm(new InformacionInv(cnx,UserId));
=======
                form1.OpenChildForm(new InformacionInv(cnx));
>>>>>>> ee1d9478137acdd7a8e8f785df6cf51d7f1f35be
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                int ID = ArticuloID;
                SqlCommand cmd = new SqlCommand("spEditarInv", cnx);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ArticuloID", ID);
                cmd.Parameters.AddWithValue("@NombreArt", txtArticulo.Text);
                cmd.Parameters.AddWithValue("@Desc", txtDescripcion.Text);
                cmd.Parameters.AddWithValue("@Prov", txtProveedor.Text);
                cmd.Parameters.AddWithValue("@Estado", cbxEstado.SelectedItem.ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                MessageBox.Show("Los cambios se realizaron con exito", "Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

                if (form1 != null)
                {
<<<<<<< HEAD
                    form1.OpenChildForm(new InformacionInv(cnx,UserId));
=======
                    form1.OpenChildForm(new InformacionInv(cnx));
>>>>>>> ee1d9478137acdd7a8e8f785df6cf51d7f1f35be
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        int ArticuloID;

<<<<<<< HEAD
        public EditarInv(SqlConnection conexion, int articuloID, String articulo, string descripcion, String medida, String existencia, string proveedor, string rentabilidad, string estado, int usuario)
=======
        public EditarInv(SqlConnection conexion, int articuloID, String articulo, string descripcion, String medida, String existencia, string proveedor, string rentabilidad, string estado)
>>>>>>> ee1d9478137acdd7a8e8f785df6cf51d7f1f35be
        {
            InitializeComponent();
            cnx = conexion;
            ArticuloID = articuloID;
            Articulo = articulo;
            Descripcion = descripcion;
            Medida = medida;
            Existencia = existencia;
            Proveedor = proveedor;
            Rentabilidad = rentabilidad;
            Estado = estado;
<<<<<<< HEAD
            UserId = usuario;
=======
>>>>>>> ee1d9478137acdd7a8e8f785df6cf51d7f1f35be
        }

        private void EditarInv_Load(object sender, EventArgs e)
        {
            txtArticulo.Text = Articulo;
            txtDescripcion.Text = Descripcion;
            txtMedida.Text = Medida;
            txtExistencia.Text = Existencia;
            txtProveedor.Text = Proveedor;
            txtRentabilidad.Text = Rentabilidad;
            cbxEstado.Text = Estado;
        }
    }
}
