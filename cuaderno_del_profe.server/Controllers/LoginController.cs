using cuaderno_del_profe.server.Entities;
using cuaderno_del_profe.server.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cuaderno_del_profe.server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Cuaderno_del_ProfeContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(Cuaderno_del_ProfeContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(Models.LoginRequest request)
        {
            var usuario = _context.Usuarios
                .Include(u => u.UsuarioRoles).ThenInclude(ur => ur.Rol)
                .FirstOrDefault(u => u.NombreUsuario == request.Usuario);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Contrasena, usuario.ContrasenaHash))
                return Unauthorized(new OperationResult(false, "Usuario o contraseña no validos"));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario)
            };

            // Agrega roles
            foreach (var rol in usuario.UsuarioRoles.Select(ur => ur.Rol.Nombre))
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var jwtKey = _configuration["Jwt:Key"];
            var sessionDuration = _configuration["Jwt:sessionDuration"];
            int defaultSessionDuration = 1440;
            int.TryParse(sessionDuration, out defaultSessionDuration);
            if (string.IsNullOrEmpty(jwtKey)) throw new Exception("No se encontro la propiedad \"Jwt.Key\" el el archivo appsettings.json");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(defaultSessionDuration),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            usuario.UsuarioRoles = null;
            return Ok(new OperationResult(true, "Éxito al iniciar sesión", usuario, tokenString));
        }
        [HttpPost()]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] CambiarContrasenaModel request)
        {
            if (request.NuevaContrasena != request.ConfirmNuevaContrasena) return BadRequest(new OperationResult(Field: nameof(request.ConfirmNuevaContrasena), "Confirmacion nueva contraseña no coincide"));

            // Obtener ID del usuario autenticado
            var idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            if (usuario == null)
                return Unauthorized(new OperationResult(false, "Usuario no encontrado o no está en línea"));

            // Verificar contraseña actual
            if (!BCrypt.Net.BCrypt.Verify(request.ContrasenaActual, usuario.ContrasenaHash))
                return BadRequest(new OperationResult(Field: nameof(request.ContrasenaActual), "La contraseña actual es incorrecta"));

            // Encriptar la nueva contraseña
            var nuevaHash = BCrypt.Net.BCrypt.HashPassword(request.NuevaContrasena);

            usuario.ContrasenaHash = nuevaHash;
            await _context.SaveChangesAsync();

            return Ok(new OperationResult(true, "Contraseña actualizada exitosamente."));
        }
    }

}
