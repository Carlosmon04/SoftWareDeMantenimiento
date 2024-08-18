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
using System.Drawing.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SoftwareProject.Formularios
{
    public partial class Login : Form
    {
        private SqlConnection cnx;
        private int usuarioID;
        private bool conectado;


        public Login()
        {
            InitializeComponent();
           
        }


        public SqlConnection CNX
        {
            get { return cnx; }
        }

        public bool Conectado
        {
            get { return conectado; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try {

                String url = "Server= 3.128.144.165;" +
                             "DataBase= DB20222000953;" +
                             "User ID= david.rodriguez;" +
                             "password= DR20222000953;";
                conectado = false;
                cnx= new SqlConnection(url);
                cnx.Open();
                conectado= true;


                if (Credenciales(cnx) == true)
                {
                    SqlCommand cmd = new SqlCommand("spValidarUsuario", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                   object reader = cmd.ExecuteScalar();
                    

                    

                 
                         usuarioID = Convert.ToInt32(reader);
                        Console.WriteLine(usuarioID);
                    cmd.Dispose();
                    
                  
                    
                    Menu frmInicio = new Menu(cnx,usuarioID);
                    frmInicio.Show(); 
                    
                }
                else {
                    MessageBox.Show("Las credenciales son invalidas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsuario.Clear();
                    txtPassword.Clear();
                }
            }
            catch (SqlException ex)
            {
                //inicio del for
             for (int i = 0; i < ex.Errors.Count; i++)
                {
                    if (ex.Errors[i].Number == 53)
                    {
                        MessageBox.Show("Error de comunicacion con el servidor ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //termina if
                    else {
                        MessageBox.Show(ex.Errors[i].Message, ex.Errors[i].Number.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private bool Credenciales(SqlConnection cnx)
        {
            bool acceso = false;
            try
            {
                SqlCommand cmd = new SqlCommand("spUsuarios", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", txtUsuario.Text);
                cmd.Parameters.AddWithValue("@Pass", txtPassword.Text);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) { 
                    if (reader.HasRows && reader["Estado"].ToString() != "I")
                    {
                        acceso = true;
                        reader.Close();
                        cmd.Dispose();
                        return acceso;
                    }
            }
            else { acceso = false; reader.Close();cmd.Dispose(); }
            return acceso;
        }catch(Exception ex) {MessageBox.Show("No existen las credenciales o " + ex.Message); }
            return acceso;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            RegistroUsuarios r= new RegistroUsuarios();
           
            r.Show();
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "Usuario")
            {
                txtUsuario.Text = "";
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                txtUsuario.Text = "Usuario";
            }

        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Password")
            {
                txtPassword.Text = "";
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Password";
            }
        }
    }
}