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
    
    public partial class AsignarEmpleado : Form
    {
        private SqlConnection cnx;
        private int solicitudID;
        private int ClienteID;
        DataTable TabED;
        public AsignarEmpleado(SqlConnection coneixion,int cliente,int solicitud)
        {
            InitializeComponent();
            cnx = coneixion;
            ClienteID = cliente;
            solicitudID = solicitud;
        }

        private void AsignarEmpleado_Load(object sender, EventArgs e)
        {
            try
            {
                String consulta = "select * from dbo.fEmpleadosDisponibles()";
                SqlDataAdapter adapter = new SqlDataAdapter(consulta,cnx);
                TabED=new DataTable();
                adapter.Fill(TabED);
                dataGridView1.DataSource = TabED;
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

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.Rows.Count > 0)
                {

                    int empleadoId = (int)TabED.DefaultView[dataGridView1.CurrentRow.Index]["EmpleadoID"];
                    SqlCommand cmd = new SqlCommand("spEstadoActivosSoli", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@empleado", empleadoId);
                    cmd.Parameters.AddWithValue("@cliente", ClienteID);
                    cmd.Parameters.AddWithValue("@SoliID", solicitudID);

                    if (MessageBox.Show("Seguro que quieres asignar a empleado " + empleadoId, "Seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                    cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        MessageBox.Show("Asignacion Realizada ", "Exito al Emigrar", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        this.Close();
                    }

                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un ERROR" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
