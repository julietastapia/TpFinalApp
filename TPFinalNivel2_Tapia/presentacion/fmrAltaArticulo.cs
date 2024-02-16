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
    public partial class frmAltaArticulo : Form
    {
        private Articulos articulo = null;
        public frmAltaArticulo()
        {
            InitializeComponent();
        }
        public frmAltaArticulo(Articulos articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }

        private void btnCancelarNuevo_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptarNuevo_Click(object sender, EventArgs e)
        {
           
            ArticulosNegocio negocio = new ArticulosNegocio();

            try
            {
                if (articulo == null)
                    articulo = new Articulos();

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.ImagenUrl = txtUrlImagen.Text;
                articulo.Precio = int.Parse(txtPrecio.Text);
                articulo.Marcas =(Marca)cboMarca.SelectedItem;
                articulo.Categoria =(Categorias)cboCategoria.SelectedItem;

                if(articulo.Id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");

                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");

                }

                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }




          
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            try
            {
                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "IdCategoria";
                cboCategoria.DisplayMember = "Descripcion";

                if(articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtUrlImagen.Text = articulo.ImagenUrl;
                    txtPrecio.Text = articulo.Precio.ToString();
                    cargarImagen(articulo.ImagenUrl);
                    cboMarca.SelectedValue = articulo.Marcas.Id;
                    cboCategoria.SelectedValue = articulo.Categoria.IdCategoria;
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            
            
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
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
    }
}
