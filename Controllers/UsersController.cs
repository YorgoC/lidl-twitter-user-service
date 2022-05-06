using System;
using AutoMapper;
using lidl_twitter_user_service.Data;
using Microsoft.AspNetCore.Mvc;
using lidl_twitter_user_service.DTOs;
using System.Collections.Generic;
using lidl_twitter_user_service.Models;

namespace lidl_twitter_user_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _repository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<read_user>> GetUsers()
        {
            Console.WriteLine("--> Getting Users.... ");
            var userItem = _repository.getAllUsers();

            return Ok(_mapper.Map<IEnumerable<read_user>>(userItem));
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public ActionResult<read_user> GetUserById(int id)
        {
            var userItem = _repository.GetUserById(id);
            if(userItem != null)
            {
                return Ok(_mapper.Map<read_user>(userItem));
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<read_user> CreateUser(create_user create_User)
        {
            var userModel = _mapper.Map<User>(create_User);
            _repository.CreateUser(userModel);
            _repository.SaveChanges();

            var userReadDto = _mapper.Map<read_user>(userModel);

            return CreatedAtRoute(nameof(GetUserById), new { Id = userReadDto.Id }, userReadDto);
        }

    }
}
