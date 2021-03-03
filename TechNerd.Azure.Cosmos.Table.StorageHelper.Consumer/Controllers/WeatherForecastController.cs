using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Consumer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IUserRepository _userRepository;
        public WeatherForecastController(IUserRepository repository, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _userRepository = repository;
        }


        [HttpGet]
        public async Task<IEnumerable<UserEntity>> Get()
        {
            //var userDetail = await _userRepository.GetUserDetailsAsync(new UserEntity() { Id = "sampleEntity" });
            var userDetail = new UserEntity()
            {
                Id = "mySampleEntity",
                Name = "Dhivya",
                Email = "dhivyaEmail"
            };
            var result = await _userRepository.CreateUserAsync(userDetail);
            userDetail.Email = "updatedEmail";
            var update = await _userRepository.UpdateUserAsync(userDetail);
            var delete = await _userRepository.DeleteUserAsync(userDetail);
            var collection = await _userRepository.GetAllUserDetailsAsync("");
            foreach (var item in collection)
            {
                await _userRepository.DeleteUserAsync(item);
            }
            return collection.ToArray();
        }
    }
}
