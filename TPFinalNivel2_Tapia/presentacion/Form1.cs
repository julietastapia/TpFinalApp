using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace presentacion
{
    public partial class frmArticulos : Form
    {
        private List<Articulos> listaArticulos;
        public frmArticulos()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Saliendo de la aplicación...");
            Application.Exit();

        }

        private void frmArticulos_Load(object sender, EventArgs e)
        {
            cargar();
        }


        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            Articulos seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.ImagenUrl);
        }


        private void cargar()
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            try
            {
                listaArticulos = negocio.listar();
                dgvArticulos.DataSource = listaArticulos;
                dgvArticulos.Columns["ImagenUrl"].Visible = false;
                dgvArticulos.Columns["Id"].Visible = false;
                cargarImagen(listaArticulos[0].ImagenUrl);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pboxArticulos.Load(imagen);

            }
            catch (Exception ex)
            {

                pboxArticulos.Load("https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png");
            }
        }

        private void bntAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

            Articulos seleccionado;
            seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;

            frmAltaArticulo modificar = new frmAltaArticulo(seleccionado);
            modificar.ShowDialog();
            cargar();
        }
    }
}





