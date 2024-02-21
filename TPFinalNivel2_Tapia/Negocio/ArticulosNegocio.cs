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
                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, ImagenUrl, Precio, IdMarca, IdCategoria) VALUES (@Codigo, @Nombre, @Descripcion, @ImagenUrl, @Precio, @IdMarca, @IdCategoria)");
                datos.setearParametro("@Codigo", nuevo.Codigo);
                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Descripcion", nuevo.Descripcion);
                datos.setearParametro("@ImagenUrl", nuevo.ImagenUrl);
                datos.setearParametro("@Precio", nuevo.Precio);
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
                datos.setearConsulta("UPDATE ARTICULOS SET Codigo = @codigo, Nombre = @Nombre, Descripcion = @Descripcion, ImagenUrl = @ImagenUrl, Precio = @Precio, IdCategoria = @IdCategoria, IdMarca = @IdMarca WHERE id = @Id");
                datos.setearParametro("@Codigo", articulo.Codigo);
                datos.setearParametro("@Nombre", articulo.Nombre);
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

        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from ARTICULOS where id = @id");
                datos.setearParametro("@id",id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulos> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulos> lista = new List<Articulos>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT Codigo, Nombre, A.Descripcion, ImagenUrl, Precio, C.Descripcion as Categoria, M.Descripcion as Marca, A.IdMarca, A.IdCategoria, A.Id From ARTICULOS A, CATEGORIAS C, MARCAS M Where C.Id = A.IdCategoria AND M.Id = A.IdMarca AND ";
                
                if(campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Precio < " + filtro;
                            break;
                        default:
                            consulta += "Precio = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                        switch(criterio)
                        {   
                            case "Comienza con":
                                consulta += "Nombre like '" + filtro + "%'";
                                break;
                            case "Termina con":
                                consulta += "Nombre like '%" + filtro + "'";
                                 break;
                            default:
                                consulta += "Nombre like '%" + filtro + "%'";
                                break;
                         }

                }
                else if (campo == "Categoria")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "C.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "C.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "C.Descripcion like '%" + filtro + "%'";
                            break;
                    }

                }
                else
                {
                    switch(criterio)
                    {
                        case "Comienza con":
                            consulta += "M.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "M.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "M.Descripcion like '%" + filtro + "%'";
                            break;
                        }
                    }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
               

                while (datos.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = datos.Lector.GetSqlMoney(4);

                    aux.Marcas = new Marca();
                    aux.Marcas.Id = (int)datos.Lector["IdMarca"];
                    aux.Marcas.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new Categorias();
                    aux.Categoria.IdCategoria = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    lista.Add(aux);
                }
                return lista;

            }
            catch (Exception ex )
            {

                throw ex;
            }
        }
    }
}









