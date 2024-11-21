using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManejoUsuarios.Models;
using Microsoft.Azure.Cosmos;

namespace ManejoUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ManejoUsuariosContext _context;
        //Conexion
        private static string connectionString = "AccountEndpoint=https://cosmosbdmu.documents.azure.com:443/;AccountKey=AjKdzRRMDnNffUW8k8oKBsfz8qb0zlTzF6KBkAwBXFsYvGMTU7WetdtvYfEmvEvhHjVNaLWwLRy7ACDbyPEY3Q==;";
        //cliente
        private static CosmosClient client = new CosmosClient(connectionString);
        //BD y Contenedor
        private static Container container = client.GetContainer("ManejoUsuarios", "Usuarios");

        public UsuarioController(ManejoUsuariosContext context)
        {
            _context = context;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();
            // Crear una consulta
            var query = new QueryDefinition("SELECT * FROM Usuarios c ");
            
            // Ejecutar la consulta
            using FeedIterator<dynamic> feedIterator = container.GetItemQueryIterator<dynamic>(query);
            
            //procesar los resultados
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<dynamic> response = await feedIterator.ReadNextAsync();

                foreach (dynamic item in response)
                {
                    Usuario usuario = new Usuario();
                    usuario.id = item.id;
                    usuario.Nombre = item.Nombre;
                    usuario.Apellido = item.Apellido;
                    usuario.Edad = item.Edad;
                    
                    usuarios.Add(usuario);
                }
            }
            
            return usuarios;
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario(string id)
        {
            List<Usuario> usuarios = new List<Usuario>();
            // Crear una consulta
            var query = new QueryDefinition("SELECT * FROM Usuarios c where c.id = @id")
                .WithParameter("@id", id);
            
            // Ejecutar la consulta
            using FeedIterator<dynamic> feedIterator = container.GetItemQueryIterator<dynamic>(query);
            
            //procesar los resultados
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<dynamic> response = await feedIterator.ReadNextAsync();

                foreach (dynamic item in response)
                {
                    Usuario usuario = new Usuario();
                    usuario.id = item.id;
                    usuario.Nombre = item.Nombre;
                    usuario.Apellido = item.Apellido;
                    usuario.Edad = item.Edad;
                    
                    usuarios.Add(usuario);
                }
            }
            
            return usuarios;
        }

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(string id, Usuario usuario)
        {
            ItemResponse<Usuario> response = await container.ReplaceItemAsync<Usuario>(usuario,id);
            
            return CreatedAtAction("GetUsuario", new { id = usuario.id }, usuario);
        }

        // POST: api/Usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            Usuario usr = new Usuario
            {
                id = usuario.id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Edad = usuario.Edad
                
            };
           ItemResponse<Usuario> response = await container.CreateItemAsync<Usuario>(usr);

            return CreatedAtAction("GetUsuario", new { id = usuario.id }, usuario);
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteUsuario(string id)
        {
            try
            {
                await container.DeleteItemAsync<Usuario>(id, new PartitionKey(id));
                return "Deleted";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
