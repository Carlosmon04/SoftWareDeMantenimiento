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
    public partial class CompraArtExistente : Form
    {
        private SqlConnection cnx;
        private int userID;

        String Articulo,Cantidad, Descripcion, Medida,Costo, Proveedor;
        public CompraArtExistente()
        {
            InitializeComponent();
        }
        public CompraArtExistente(SqlConnection conexion, int usuario,string articulo, String medida,string proveedor,string descripcion)
        {
            InitializeComponent();
            cnx = conexion;
            Articulo = articulo;
            Medida = medida;
            Proveedor = proveedor;
            Descripcion = descripcion;
            userID = usuario;
        }

        private void CompraArtExistente_Load(object sender, EventArgs e)
        {
            txtArticulo.ReadOnly = true;
            txtMedida.ReadOnly = true;
            txtDescripcion.ReadOnly = true;

            txtArticulo.Text = Articulo;
            txtMedida.Text = Medida;
            txtProveedor.Text = Proveedor;
            txtDescripcion.Text = Descripcion;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd2 = new SqlCommand("spCompraSinart", cnx);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@NombreArt", txtArticulo.Text);
            cmd2.Parameters.AddWithValue("@Cantidad", txtCantidad.Text);
            cmd2.Parameters.AddWithValue("@Pro", txtProveedor.Text);
            cmd2.Parameters.AddWithValue("@Costo", txtCosto.Text);
            cmd2.ExecuteNonQuery();

            txtCantidad.Clear(); txtCosto.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
