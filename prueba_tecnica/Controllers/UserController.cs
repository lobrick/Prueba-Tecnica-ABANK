using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prueba_tecnica.Modelo;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Proteger el controlador con autorización
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Método privado para encontrar un usuario por ID
    private async Task<ActionResult<Seguridad>> FindUserById(int? id)
    {
        var user = await _context.Seguridad.FindAsync(id);
        if (user == null)
        {
            return NotFound(new { message = "Usuario no encontrado." });
        }
        return user;
    }

    // Endpoint para crear un nuevo usuario
    [HttpPost("createuser")]
    public async Task<IActionResult> CreateUser([FromBody] Seguridad newUser)
    {
        if (newUser == null)
        {
            return BadRequest("Usuario inválido.");
        }

        // Verifica si el número de teléfono ya está registrado
        var existingUser = await _context.Seguridad.FirstOrDefaultAsync(u => u.telefono == newUser.telefono);
        if (existingUser != null)
        {
            return BadRequest("El número de teléfono ya está en uso. Por favor, ingrese un número distinto.");
        }

        // Aquí podrías hacer el hash de la contraseña antes de almacenarla
        newUser.password = BCrypt.Net.BCrypt.HashPassword(newUser.password);

        // Agrega el nuevo usuario a la base de datos
        _context.Seguridad.Add(newUser);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllUsers), newUser);
    }

    // Endpoint para obtener todos los usuarios
    [HttpGet("getallusers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Seguridad.ToListAsync();
        return Ok(users);
    }

    // Endpoint para obtener un usuario por ID
    [HttpGet("getuser")]
    public async Task<IActionResult> GetUserById(int? id)
    {
        var userResult = await FindUserById(id);
        if (userResult.Result is NotFoundResult) return userResult.Result;

        return Ok(userResult.Value);
    }

    // Endpoint para actualizar un usuario
    [HttpPut("updateuser")]
    public async Task<IActionResult> UpdateUser([FromBody] Seguridad updatedUser)
    {
        var existingUserResult = await FindUserById(updatedUser.id);
        if (existingUserResult.Result is NotFoundResult) return existingUserResult.Result;

        // Aquí podrías hacer el hash de la nueva contraseña si se cambia
        if (!string.IsNullOrEmpty(updatedUser.password))
        {
            updatedUser.password = BCrypt.Net.BCrypt.HashPassword(updatedUser.password);
        }

        // Actualiza los valores de la entidad existente
        var existingUser = existingUserResult.Value;
        existingUser.nombres = updatedUser.nombres;
        existingUser.apellidos = updatedUser.apellidos;
        existingUser.fechanacimiento = updatedUser.fechanacimiento;
        existingUser.direccion = updatedUser.direccion;
        existingUser.telefono = updatedUser.telefono;
        existingUser.email = updatedUser.email;
        existingUser.fechamodificacion = DateOnly.FromDateTime( DateTime.Now); // Actualiza la fecha de modificación

        _context.Entry(existingUser).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            // Devuelve el listado actualizado de usuarios
            var users = await _context.Seguridad.ToListAsync();
            return Ok(users);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return StatusCode(500, new { message = "Error al actualizar el usuario.", error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error inesperado.", error = ex.Message });
        }
    }

    // Endpoint para eliminar un usuario
    [HttpDelete("deleteuser")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var userResult = await FindUserById(id);
        if (userResult.Result is NotFoundResult) return userResult.Result;

        _context.Seguridad.Remove(userResult.Value);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
