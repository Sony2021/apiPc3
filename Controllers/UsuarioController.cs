using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using apiPc3.Integration;
using apiPc3.Integration.dto;

namespace apiPc3.Controllers
{
 
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly ListarUsuariosApiIntegration _listUsers;
        private readonly CrearUsuarioApiIntegration _createUser;
    

    public UsuarioController(ILogger<UsuarioController> logger,
        ListarUsuariosApiIntegration listUsers,
        CrearUsuarioApiIntegration createUser)
        {
            _logger = logger;
            _listUsers = listUsers;
            _createUser = createUser;
        }
    [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Usuario> users = await _listUsers.GetAllUser();
            return View(users);
        }

    public IActionResult Create()
        {
            return View();
        }
        
     [HttpPost]
     public async Task<IActionResult> Create(string name, string job)
        {
            try
            {
                // Llamar al método CreateUser de tu integración para crear un nuevo usuario
                var response = await _createUser.CreateUser(name, job);
                
                // Verificar si la creación del usuario fue exitosa
                if (response != null)
                {
                    // Mostrar mensaje de confirmación
                    TempData["SuccessMessage"] = "Usuario "+response.Name+" creado correctamente, con el trabajo "+ response.Job+" y Id "+response.Id+" a las "+response.CreatedAt;
                }
                else
                {
                    // Manejar el caso en que la creación del usuario no fue exitosa
                    ModelState.AddModelError("", "Error al crear el usuario");
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante la creación del usuario
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                ModelState.AddModelError("", "Error al crear el usuario");
            }
            
            // Redireccionar a la acción Index
            return View();
        }   

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

    }    
}