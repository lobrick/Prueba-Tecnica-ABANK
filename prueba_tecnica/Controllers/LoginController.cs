using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;  
using System.Security.Claims;
using System.Text;
using prueba_tecnica.Modelo;

namespace prueba_tecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public LoginController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            try
            {
                // Busca en la base de datos el usuario con el teléfono proporcionado
                var user = _context.Seguridad.FirstOrDefault(u => u.telefono == login.Telefono);

                if (user == null)
                {
                    return Unauthorized(new { message = "Usuario no encontrado" });
                }

                // Verifica la contraseña
                if (login.Password != user.password)
                {
                    return Unauthorized(new { message = "Contraseña incorrecta" });
                }

                // Si las credenciales son válidas, genera el token
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }
            catch (SocketException ex)
            {
                // Manejamos errores de red y socket
                return StatusCode(500, new { message = "Error de red o conexión: " + ex.Message });
            }
            catch (Exception ex)
            {
                // Capturamos cualquier otro tipo de excepción
                return StatusCode(500, new { message = "Error inesperado: " + ex.Message });
            }
        }

        private string GenerateJwtToken(Seguridad user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.email), // Usa el email como claim
                    new Claim("id", user.id.ToString()), // Almacenar el ID en los claims
                    new Claim("telefono", user.telefono),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // Manejamos errores durante la generación del token
                throw new Exception("Error al generar el token: " + ex.Message);
            }
        }
    }
}
