﻿using System;
using AutoMapper;
using lidl_twitter_user_service.Data;
using Microsoft.AspNetCore.Mvc;
using lidl_twitter_user_service.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using lidl_twitter_user_service.Models;
using lidl_twitter_user_service.SyncDataServices.Http;

namespace lidl_twitter_user_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _repository;
        private readonly IMapper _mapper;
        private readonly ITweetDataClient _tweetDataClient;

        public UsersController(
            IUserRepo repository,
            IMapper mapper,
            ITweetDataClient tweetDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _tweetDataClient = tweetDataClient;
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

        [HttpPost]
        public async Task<ActionResult<ReadUser>> CreateUser(CreateUser createUser)
        {
            var userModel = _mapper.Map<User>(createUser);
            _repository.CreateUser(userModel);
            _repository.SaveChanges();

            var userReadDto = _mapper.Map<ReadUser>(userModel);

            try
            {
                await _tweetDataClient.SendUserToTweet(userReadDto);
            }
            catch(Exception e)
            {
                Console.WriteLine($"--> Could not send synchronously: {e.Message}");
            }

            return CreatedAtRoute(nameof(GetUserById), new { Id = userReadDto.Id }, userReadDto);
        }

    }
}