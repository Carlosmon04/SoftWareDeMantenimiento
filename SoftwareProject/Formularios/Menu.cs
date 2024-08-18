using SoftwareProject.Formularios;
using SoftwareProject.Formularios.Formularios_de_DELETE;
using SoftwareProject.Formularios.CRUD_Clientes;
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


namespace SoftwareProject
{
    public partial class Menu : Form
    {
        private SqlConnection cnx;
        private int userID;
        public Menu(SqlConnection conexion, int usuario)
        {
            InitializeComponent();
            Design();
            cnx= conexion;
            userID = usuario;
        }

        #region Metodos

        private void Design()
        {

            subMenuClie.Visible = false;
            subMenuEmpleado.Visible = false;
            subMenuInventario.Visible = false;
            subMenuS.Visible = false;
            subMenuP.Visible = false;
            subMenuFinanza.Visible = false;
        }

        private void Ocultar()
        { 
            if (subMenuClie.Visible == true)
            {
                subMenuClie.Visible = false;
            }

            if (subMenuEmpleado.Visible == true)
            {
                subMenuEmpleado.Visible = false;
            }

            if (subMenuInventario.Visible == true)
            {
                subMenuInventario.Visible = false;
            }

            if (subMenuS.Visible == true)
            {
                subMenuS.Visible = false;
            }

            if(subMenuP.Visible == true)
            {
                subMenuP.Visible = false;
            }

            if (subMenuFinanza.Visible == true)
            {
                subMenuFinanza.Visible = false;
            }


        }


        private void Mostrar(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                Ocultar();
                subMenu.Visible = true;
            }
            else 
                subMenu.Visible = false; 
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            Mostrar(subMenuEmpleado);
        }

        private void btnAggUser_Click(object sender, EventArgs e)
        {
            OpenChildForm(new RegistroEmpleados(cnx, userID));
            Ocultar();
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            OpenChildForm(new DeleteEmpleados(cnx));
            Ocultar();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            Mostrar(subMenuClie);
        }

        private void btnAggClie_Click(object sender, EventArgs e)
        {
            //probando Git
            OpenChildForm(new ConsultarClientes(cnx));
            Ocultar();
        }

      

        private Form activeForm = null;
        public void OpenChildForm (Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelForms.Controls.Add(childForm);
            panelForms.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private int TipoAutorizado(SqlConnection conexion, int usuario)
        { 
        int quien = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("spJefes", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", usuario);
                cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.Read()) { 
                if (reader.IsDBNull(0))
                {
                    quien = 1;

                }
                if (!reader.IsDBNull(0))
                {
                    quien = 2;
                }
                if (!reader.HasRows)
                {
                    quien = 0;
                }
            }
                reader.Close();
                return quien;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un Error " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return quien;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (TipoAutorizado(cnx, userID) != 1)
            { 
               btnEmpleados.Visible = false;
                
            }

        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            Mostrar(subMenuInventario);
        }

        private void btnCompraInv_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Compra(cnx, userID));
            Ocultar();
        }

        private void btnInfoInv_Click(object sender, EventArgs e)
        {
            OpenChildForm(new InformacionInv(cnx));
            Ocultar();
        }

        #endregion Metodos


        private void button1_Click(object sender, EventArgs e)
        {
            Mostrar(subMenuS);
        }

        private void btnAgregarS_Click(object sender, EventArgs e)
        {
            //OpenChildForm(new El formulario que van a llamar por ejemplo infoPaquetes(cnx));
            Ocultar();
        }

        private void btnInfoS_Click(object sender, EventArgs e)
        {
            //OpenChildForm(new El formulario que van a llamar por ejemplo infoPaquetes(cnx));
            Ocultar();
        }

        private void btnPaquetes_Click(object sender, EventArgs e)
        {
            Mostrar(subMenuP);
        }

        private void btnAgregarP_Click(object sender, EventArgs e)
        {
           
            Ocultar();
        }

        private void btnInfoP_Click(object sender, EventArgs e)
        {
            OpenChildForm(new VerPaquetes(cnx));
            Ocultar();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            SolicitarServicio s = new SolicitarServicio(cnx, userID);
            s.Visible = true;

        }

        private void btnFinanzas_Click(object sender, EventArgs e)
        {
            Mostrar(subMenuFinanza);
        }

        private void btnCuentas_Click(object sender, EventArgs e)
        {
            Ocultar();
        }

        private void btnActividad_Click(object sender, EventArgs e)
        {
            Ocultar();
        }

        private void btnVerSolicitudes_Click(object sender, EventArgs e)
        {
            VerSolicitudes vs = new VerSolicitudes(cnx, userID);
            vs.Visible = true;
            Ocultar();
        }

        private void btnComprarA_Click(object sender, EventArgs e)
        {
            Ocultar();
        }
    }
}
