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
    public partial class SolicitarServicio : Form
    {
        private SqlConnection cnx;
        private int userID;
        private int ServicioID;
        DataTable TabServicios;
        DateTime Visita;
        private int year;
        private int month;
        private int day;
        private int hour;
        private String Hora;
        public SolicitarServicio(SqlConnection conexion,int usuario)
        {
            InitializeComponent();
            cnx = conexion;
            userID = usuario;
        }

        private void SolicitarServicio_Load(object sender, EventArgs e)
        {




            try
            {
                TabServicios = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("spServiciosVer", cnx);
                adapter.Fill(TabServicios);
                dataGridView1.DataSource = TabServicios;
                dataGridView1.ReadOnly = true;
                dataGridView1.Columns["SerciciosId"].Visible = false;
                dataGridView1.Columns["Estado"].Visible = false;
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
            try {
                Visita = new DateTime();
                if (dataGridView1.Rows.Count > 0)
                {
                    ServicioID = (int)TabServicios.DefaultView[dataGridView1.CurrentRow.Index]["SerciciosId"];
                    if (MessageBox.Show("Seguro que quieres solicitar este servicio? ", "???", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Console.WriteLine(ServicioID);

                        MessageBox.Show("Eliga Cuando desea la Visita", "Elige Con Gusto", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    
                    }


                }
            
            }catch(SqlException ex)
            {
                MessageBox.Show("Ocurrio un ERROR" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }



        }

         

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnConfirmarFecha_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Estas seguro de tu fecha y hora de visita? ", "???", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                try
                {
                  

                    SqlCommand cmd = new SqlCommand("spSolicitudCliente", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario", userID);
                    cmd.Parameters.AddWithValue("@fechahora", FechaQuerida());
                    cmd.Parameters.AddWithValue("@fechasalida", FechaQuerida().AddHours(1));
                    cmd.Parameters.AddWithValue("@ServicioID", ServicioID);
                   
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Implementar logica de las horas (Pendiente)
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ocurrio un ERROR" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

            }
        }

        private void Calendar_DropDown(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

          
        }

        private DateTime FechaQuerida()
        {
            if (cmxHoras.SelectedItem.ToString() == "8:00") Hora = "8";
            if (cmxHoras.SelectedItem.ToString() == "9:00") Hora = "9";
            if (cmxHoras.SelectedItem.ToString() == "10:00") Hora = "10";
            if (cmxHoras.SelectedItem.ToString() == "11:00") Hora = "11";
            if (cmxHoras.SelectedItem.ToString() == "12:00") Hora = "12";
            if (cmxHoras.SelectedItem.ToString() == "13:00") Hora = "13";
            if (cmxHoras.SelectedItem.ToString() == "14:00") Hora = "14";
            if (cmxHoras.SelectedItem.ToString() == "15:00") Hora = "15";
            if (cmxHoras.SelectedItem.ToString() == "16:00") Hora = "16";
            if (cmxHoras.SelectedItem.ToString() == "17:00") Hora = "17";
            if (cmxHoras.SelectedItem.ToString() == "18:00") Hora = "18";
            DateTime r = Calendar1.SelectionEnd;
            year = r.Year;
            month = r.Month;
            day = r.Day;
            hour = Convert.ToInt32(Hora);

            Visita = new DateTime(year, month, day, hour, 0, 0);
            return Visita;
        }

        private void cmxHoras_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime max = DateTime.Now;
            Console.WriteLine(max);
            Console.WriteLine(FechaQuerida());

            if (max > FechaQuerida()) {
                int x = max.Hour;

                String y = cmxHoras.SelectedIndex.ToString();
            int z = Convert.ToInt32(y);

            if (z < x)
            {
                MessageBox.Show("Esta hora ya no esta disponible por Hoy ", "Hora Fuera Rango", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        }
    }
}
