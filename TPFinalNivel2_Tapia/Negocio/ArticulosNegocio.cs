using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;
using System.Security.Cryptography.X509Certificates;


namespace Negocio
{
    public class ArticulosNegocio
    {
       public List<Articulos> listar()
        {
            List<Articulos> lista = new List<Articulos>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true"; // conexion a la base de datos
                comando.CommandType = System.Data.CommandType.Text; // realiza la lectura
                comando.CommandText = "SELECT Codigo, Nombre, A.Descripcion, ImagenUrl, Precio, C.Descripcion as Categoria, M.Descripcion as Marca, A.IdMarca, A.IdCategoria, A.Id From ARTICULOS A, CATEGORIAS C, MARCAS M Where C.Id = A.IdCategoria AND M.Id = A.IdMarca"; // consulta en la base de datos
                comando.Connection = conexion; // ejecuta la conexion

                conexion.Open(); // abre la conexion
                lector = comando.ExecuteReader(); // realiza la lectura

                while (lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.Id = (int)lector["Id"];
                    aux.Codigo = (string)lector["Codigo"];
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];
                    aux.ImagenUrl = (string)lector["ImagenUrl"];
                    aux.Precio = lector.GetSqlMoney(4);

                    aux.Marcas = new Marca();
                    aux.Marcas.Id = (int)lector["IdMarca"];
                    aux.Marcas.Descripcion = (string)lector["Marca"];
                    aux.Categoria = new Categorias();
                    aux.Categoria.IdCategoria = (int)lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)lector["Categoria"];
                   

                    lista.Add(aux);
                }

                conexion.Close();
                return lista;
            }
             catch (Exception ex)
            {
                throw ex;
            }

        }

            public void agregar(Articulos nuevo)
            {

            AccesoDatos datos = new AccesoDatos();

                try
                {
                datos.setearConsulta("Insert into ARTICULOS (Codigo, Nombre, Descripcion, ImagenUrl, Precio, IdMarca, IdCategoria) values ('" + nuevo.Codigo + "','" + nuevo.Nombre + "',' " + nuevo.Descripcion + "','" + nuevo.ImagenUrl + "'," + nuevo.Precio + ", @IdMarca, @IdCategoria)");
                datos.setearParametro("@IdMarca", nuevo.Marcas.Id);
                datos.setearParametro("@IdCategoria", nuevo.Categoria.IdCategoria);
                datos.ejecutarAccion();


                }
                catch (Exception ex)
                {

                    throw ex;
                 }
                finally
                {
                    datos.cerrarConexion();
                }

            }

            public void modificar(Articulos articulo)
            {
                AccesoDatos datos = new AccesoDatos();
                try
                {
                    datos.setearConsulta("update ARTICULOS set Codigo = @codigo, Descripcion = @Descripcion, ImagenUrl = @ImagenUrl, Precio = @Precio, IdCategoria = @Categoria, IdMarca = @Marca where id = @Id");
                    datos.setearParametro("@Codigo", articulo.Codigo);
                    datos.setearParametro("@Descripcion", articulo.Descripcion); 
                    datos.setearParametro("@ImagenUrl", articulo.ImagenUrl); 
                    datos.setearParametro("@Precio", articulo.Precio);
                    datos.setearParametro("@IdCategoria", articulo.Categoria.IdCategoria);
                    datos.setearParametro("@IdMarca", articulo.Marcas.Id);
                    datos.setearParametro("@Id", articulo.Id);

                    datos.ejecutarAccion();

                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    datos.cerrarConexion();
                }
            }

    }
}









