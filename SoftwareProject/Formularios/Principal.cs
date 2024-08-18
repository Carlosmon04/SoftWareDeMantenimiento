using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareProject
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
            Design();
        }

        #region Metodos

        private void Design()
        {

            subMenuClie.Visible = false;
            subMenuUsers.Visible = false;
        }

        private void Ocultar()
        { 
            if (subMenuClie.Visible == true)
            {
                subMenuClie.Visible = false;
            }

            if (subMenuUsers.Visible == true)
            {
                subMenuUsers.Visible = false;
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
            Mostrar(subMenuUsers);
        }

        private void btnAggUser_Click(object sender, EventArgs e)
        {
            Ocultar();
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Prueba());
            Ocultar();
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            Ocultar();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            Mostrar(subMenuClie);
        }

        private void btnAggClie_Click(object sender, EventArgs e)
        {
            Ocultar();
        }

        private void btnEditClie_Click(object sender, EventArgs e)
        {
            Ocultar();
        }

        private void btnDeleteClie_Click(object sender, EventArgs e)
        {
            Ocultar();
        }

        private Form activeForm = null;
        private void OpenChildForm (Form childForm)
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

        #endregion Metodos
    }
}
