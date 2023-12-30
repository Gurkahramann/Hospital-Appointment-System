using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AspWebProgram.Models;
using AspWebProgramming.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[Route("api/[controller]")]
[ApiController]
public class DoktorApiController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly DataContext db;

    public DoktorApiController(DataContext context, IConfiguration configuration)
    {
        db = context;
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("generateToken")]
    public IActionResult GenerateToken([FromBody] LoginModel model)
    {
        var adminUser = db.Adminler.FirstOrDefault(u => u.KullaniciAdi == model.Username && u.Sifre == model.Password);

        if (adminUser != null)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],  // İssuer değerini güncelleyin
                audience: _configuration["Jwt:Audience"],  // Audience değerini güncelleyin
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        return Unauthorized();
    }
    [HttpPut("edit/{id}")]
    public IActionResult Edit(int id, [FromBody] Doktor doktor)
    {
        if (id != doktor.DoktorId)
        {
            return BadRequest();
        }

        var existingDoktor = db.Doktorlar.Find(id);
        if (existingDoktor == null)
        {
            return NotFound();
        }
        existingDoktor.DoktorSoyad=doktor.DoktorSoyad;
        existingDoktor.DoktorTc=doktor.DoktorTc;
        existingDoktor.DoktorAd = doktor.DoktorAd;
        // Diğer alanlar da güncellenebilir...

        db.Update(existingDoktor);
        db.SaveChanges();

        return NoContent(); // Başarılı bir güncelleme için NoContent döner
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete(int id)
    {
        var doktor = db.Doktorlar.Find(id);
        if (doktor == null)
        {
            return NotFound();
        }

        db.Doktorlar.Remove(doktor);
        db.SaveChanges();

        return NoContent(); // Başarılı bir silme işlemi için NoContent döner
    }
}
