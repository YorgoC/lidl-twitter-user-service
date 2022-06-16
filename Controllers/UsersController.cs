using System;
using AutoMapper;
using lidl_twitter_user_service.Data;
using Microsoft.AspNetCore.Mvc;
using lidl_twitter_user_service.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using lidl_twitter_user_service.AsyncDataServices;
using lidl_twitter_user_service.Models;
//using lidl_twitter_user_service.SyncDataServices.Http;

namespace lidl_twitter_user_service.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _repository;
        private readonly IMapper _mapper;
     //   private readonly ITweetDataClient _tweetDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public UsersController(
            IUserRepo repository,
            IMapper mapper,
    //        ITweetDataClient tweetDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
     //       _tweetDataClient = tweetDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReadUser>> GetUsers()
        {
            Console.WriteLine("--> Getting Users.... ");
            var userItem = _repository.getAllUsers();
            return Ok(_mapper.Map<IEnumerable<ReadUser>>(userItem));
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public ActionResult<ReadUser> GetUserById(int id)
        {
            var userItem = _repository.GetUserById(id);
            if(userItem != null)
            {
                return Ok(_mapper.Map<ReadUser>(userItem));
            }

            return NotFound();
        }

        [HttpPost("auth0",Name = "CheckIfUserIsRegisteredByAuth0Id")]
        public ActionResult<Boolean> CheckIfUserIsRegisteredByAuth0Id(CreateUser createUser)
        {
            
            Console.WriteLine("--> Checking auth0Id");
            var userItem = _repository.GetUserByAuth0Id(createUser.Auth0Id);
            if (userItem != null)
            {
                return true;
            }

            return false;
        }
        
        [HttpPost("getuser",Name = "GetUserByAuth0Id")]
        public ActionResult<ReadUser> GetUserByAuth0Id(CreateUser createUser)
        {
            
            Console.WriteLine("--> Checking auth0Id");
            var userItem = _repository.GetUserByAuth0Id(createUser.Auth0Id);
            if (userItem != null)
            {
                return Ok(_mapper.Map<ReadUser>(userItem));
            }
            
            return NotFound();
            
        }

        [HttpPost]
        public async Task<ActionResult<ReadUser>> CreateUser(CreateUser createUser)
        {
            var userModel = _mapper.Map<User>(createUser);
            _repository.CreateUser(userModel);
            _repository.SaveChanges();

            var userReadDto = _mapper.Map<ReadUser>(userModel);

            // //Send Sync message
            // try
            // {
            //     await _tweetDataClient.SendUserToTweet(userReadDto);
            // }
            // catch(Exception e)
            // {
            //     Console.WriteLine($"--> Could not send synchronously: {e.Message}");
            // }
            
            //Send Async message
            try
            {
                var publishedUserDto = _mapper.Map<PublishedUser>(userReadDto);
                publishedUserDto.Event = "User_Published";
                _messageBusClient.PublishNewUser(publishedUserDto);
            }
            catch(Exception e)
            {
                Console.WriteLine($"--> Could not send Asynchronously: {e.Message}");
            }

            return CreatedAtRoute(nameof(GetUserById), new { Id = userReadDto.Id }, userReadDto);
        }

    }
}
