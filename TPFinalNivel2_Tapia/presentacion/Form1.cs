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

        private void frmArticulos_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Categoria");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Precio");
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
        }

        private void cargar()
        {
            ArticulosNegocio negocio = new ArticulosNegocio();

            try
            {
                listaArticulos = negocio.listar();
                dgvArticulos.DataSource = listaArticulos;
                cargarImagen(listaArticulos[0].ImagenUrl);
                ocultarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pboxArticulos.Load(imagen);
            }
            catch (Exception ex)
            {
                pboxArticulos.ImageLocation = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png";
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

        private void btnEliminar_Click_(object sender, EventArgs e)
        {
           ArticulosNegocio negocio = new ArticulosNegocio();
           Articulos seleccionado;
           seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;

            if (MessageBox.Show("¿Está seguro de eliminar el artículo " + seleccionado.Nombre + "?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    negocio.eliminar(seleccionado.Id);
                    cargar();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void tbFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulos> listaFiltrada;
            string filtro = tbFiltro.Text;

            if(filtro != "")
            {
                listaFiltrada = listaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Marcas.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()));

            }
            else
            {
                listaFiltrada = listaArticulos;
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Empieza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }
        private bool validarFiltro()
        {
            if(cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar");
                return true;
            }
            if (cboCriterio.SelectedIndex<0)
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar");
                return true;
            }
            if(cboCampo.SelectedItem.ToString() == "Precio")
            {
                if(string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                 {
                    MessageBox.Show("Debes cargar el Filtro para buscar Precio");
                    return true;
                }
                if(!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Sólo números para filtrar");
                        return true;
                }
            }
            return false;
        }
        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            try
            {
                if (validarFiltro())
                    return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }
    }
}




















            





