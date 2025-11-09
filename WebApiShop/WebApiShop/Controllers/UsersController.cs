using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static WebApiShop.Controllers.UsersController;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        string _filePath = "C:\\Users\\User\\Desktop\\לימודים\\יד\\web-api\\lesson4\\data.txt";

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int userId)
        {
            using (StreamReader reader = System.IO.File.OpenText(_filePath))
            {
                string? _currentUserInFile;
                while ((_currentUserInFile = reader.ReadLine()) != null)
                {
                    User? _user = JsonSerializer.Deserialize<User>(_currentUserInFile);
                    if (_user != null &&  _user.id == userId)
                        return Ok(_user);
                }
            }
                   return NoContent();
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            int _numberOfUsers = System.IO.File.ReadLines(_filePath).Count();
            user.id = _numberOfUsers + 1;
            string _userJson = JsonSerializer.Serialize(user);
            System.IO.File.AppendAllText(_filePath, _userJson + Environment.NewLine);
            return CreatedAtAction(nameof(Get), new { id = user.id }, user);
        }


        [HttpPost("Login")]
        public ActionResult<User> Login([FromBody] User info)
        {
            using (StreamReader reader = System.IO.File.OpenText(_filePath))
            {
                string? _currentUserInFile;
                while ((_currentUserInFile = reader.ReadLine()) != null)
                {
                    User? _user = JsonSerializer.Deserialize<User>(_currentUserInFile);
                    if (_user!= null && _user.userName == info.userName && _user.passWord == info.passWord)
                        return Ok(_user);
                   }
            }
            return NoContent() ;

        }
        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User info)
        {
            string _textToReplace = string.Empty;
            using (StreamReader reader = System.IO.File.OpenText(_filePath))
            {
                string? _currentUserInFile;
                while ((_currentUserInFile = reader.ReadLine()) != null)
                {
                    User? _user = JsonSerializer.Deserialize< User>(_currentUserInFile);
                    if (_user != null && _user.id == id)
                        _textToReplace = _currentUserInFile;
                }
            }

            if (_textToReplace != string.Empty)
            {
                string _text = System.IO.File.ReadAllText(_filePath);
                _text = _text.Replace(_textToReplace, JsonSerializer.Serialize(info));
                System.IO.File.WriteAllText(_filePath, _text);
            }
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
