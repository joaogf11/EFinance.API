using EFinnance.API.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static EFinnance.API.ViewModels.Auth.AuthViewModel;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<UserViewModel> _userManager;
    private readonly SignInManager<UserViewModel> _signInManager;

    public AuthController(UserManager<UserViewModel> userManager, SignInManager<UserViewModel> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new UserViewModel { UserName = model.Username, Email = model.Email, PhoneNumber = model.Phone };
        if (_userManager.FindByEmailAsync(user.Email) != null)
        {
            return BadRequest(new { erros = "Email já cadastrado!" });
        }
        if(model.Phone.Length > 11)
        {
            return BadRequest(new { erros = "Telefone Inválido! Somente números! (1199999999)" });
        }

        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (result.Succeeded)
        {
            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

        var errosTraduzidos = TraduzirErros(result.Errors);
        return BadRequest(new { erros = errosTraduzidos });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
        {
            return Unauthorized("Usuário ou senha inválidos!");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized("Usuário ou senha inválidos!");
        }

        var token = GenerateJwtToken(user);
        return Ok(new { token, userId = user.Id });
    }

    private string GenerateJwtToken(UserViewModel user)
    {
        var key = Encoding.UTF8.GetBytes("X7z!r9G@t2L#pD5mV8c*KqYs&fN4wZJb");

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
    };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            Issuer = "EFinnanceAPI",
            Audience = "EFinnanceClient"
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private List<string> TraduzirErros(IEnumerable<IdentityError> erros)
    {
        var lista = new List<string>();

        foreach (var erro in erros)
        {
            var descricao = erro.Description;

            if (descricao.Contains("is already taken"))
                lista.Add("Este nome de usuário já está em uso.");
            else if (descricao.Contains("Passwords must have at least one non alphanumeric character"))
                lista.Add("A senha deve conter ao menos um caractere especial.");
            else if (descricao.Contains("Passwords must have at least one uppercase"))
                lista.Add("A senha deve conter ao menos uma letra maiúscula.");
            else if (descricao.Contains("Passwords must have at least one lowercase"))
                lista.Add("A senha deve conter ao menos uma letra minúscula.");
            else if (descricao.Contains("Passwords must have at least one digit"))
                lista.Add("A senha deve conter ao menos um número.");
            else if (descricao.Contains("Passwords must be at least"))
                lista.Add("A senha é muito curta.");
            else if (descricao.Contains("is invalid"))
                lista.Add("Campo inválido.");
            else if (descricao.Contains("is already associated with an account"))
                lista.Add("Este e-mail já está associado a uma conta.");
            else
                lista.Add(descricao); 
        }

        return lista;
    }

}
