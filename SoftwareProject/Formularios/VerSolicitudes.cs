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

namespace SoftwareProject.Formularios
{
    public partial class VerSolicitudes : Form
    {
        private SqlConnection cnx;
        private int UserID;
        DataTable TabSolicitudes;
        
        

        public VerSolicitudes(SqlConnection conexion, int usuario)
        {
            InitializeComponent();
            cnx= conexion;
            UserID = usuario;
            
        }

        private void VerSolicitudes_Load(object sender, EventArgs e)
        {

            TiposSolicitudes(cnx, "P");
        
        }

        private void TiposSolicitudes(SqlConnection conexion,String estado)
        {

            try
            {
                TabSolicitudes = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("spSoliPendiente", conexion);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.AddWithValue("@Estado", estado);

                adapter.Fill(TabSolicitudes);
                dataGridView1.DataSource = TabSolicitudes;
                dataGridView1.ReadOnly = true;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un ERROR" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }

        }

        private void cmxSoli_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmxSoli.SelectedItem.ToString() == "Pendientes") TiposSolicitudes(cnx, "P" );
            if (cmxSoli.SelectedItem.ToString() == "Activas") TiposSolicitudes(cnx, "A");
            if (cmxSoli.SelectedItem.ToString() == "Terminadas") TiposSolicitudes(cnx, "T" );



        }

        private void btnAsignar_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (dataGridView1.Rows.Count > 0)
                {

                    if ((String)TabSolicitudes.DefaultView[dataGridView1.CurrentRow.Index]["Estado"] == "P")
                    {
                        int ClienteID = (int)TabSolicitudes.DefaultView[dataGridView1.CurrentRow.Index]["ClienteID"];
                        int Solicitud = (int)TabSolicitudes.DefaultView[dataGridView1.CurrentRow.Index]["SolicitudId"];

                        
                        if (ClienteID !=0 && Solicitud != 0)
                        {
                            Console.WriteLine(ClienteID);
                            Console.WriteLine(Solicitud);
                            AsignarEmpleado r = new AsignarEmpleado(cnx, ClienteID, Solicitud);
                       r.Visible = true;
                    }
                        
                    }
                    else { MessageBox.Show("Solo puedes Asignar a Solicitudes Pendientes", "Denegado", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop); }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un ERROR" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar si se ha seleccionado un ítem en el ComboBox
                if (cmxSoli.SelectedIndex == -1)
                {
                    MessageBox.Show("Seleccione qué solicitudes ver", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return; // Salir del método si no hay selección
                }

                // Inicializar la variable x
                String x = String.Empty;

                // Determinar el valor de x según la selección del ComboBox
                switch (cmxSoli.SelectedItem.ToString())
                {
                    case "Pendientes":
                        x = "P";
                        break;
                    case "Activas":
                        x = "A";
                        break;
                    case "Terminadas":
                        x = "T";
                        break;
                    default:
                        MessageBox.Show("Selección no válida", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return; // Salir del método si no es una selección válida
                }

                // Llamar al método TiposSolicitudes con el valor de x
                TiposSolicitudes(cnx, x);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrió un ERROR: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
