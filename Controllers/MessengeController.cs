using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.Services.Implementations;
using SocialMediaServer.Services.Interfaces;
using SocialMediaServer.DTOs;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Mappers;
using Microsoft.AspNetCore.Authorization;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using SocialMediaServer.Configuration;


namespace SocialMediaServer.Controllers
{
    [Route("/api/messenge")]
    [ApiController]
    [Authorize]
    public class MessengeController: ControllerBase
    {

        private readonly IWebHostEnvironment _environment;
        private readonly IMessengeService _messengeService;
        private readonly IUserService _userService;
        private readonly IMessengeFileService _messengeFileService;
        public MessengeController(IMessengeService messengeService, IUserService userService, IWebHostEnvironment environment, IMessengeFileService messengeFileService){
            _messengeService = messengeService;
            _userService = userService;
            _environment = environment;
            _messengeFileService = messengeFileService;
        }
        
        [HttpGet("{relationShipId}")]
        public async Task<IActionResult> GetMessengeByRelationShipId([FromRoute] int relationShipId){
            var list_messenge = await _messengeService.GetMessagesByRelationshipIdAsync(relationShipId);
            return Ok(list_messenge);   
        }

        [HttpPost("{relationShipId}")]
        public async Task<IActionResult> SendMessengeAsync([FromRoute] int relationShipId, MessengeDTO messengeDTO){

            var senderClaims = await _userService.GetCurrentUser(User);

            var fileUpload = new List<string>();

            if (senderClaims == null)
                return Unauthorized("User is not logged in.");
            
            try {

                if (messengeDTO.files != null || messengeDTO.files?.Count > 0)
                {
                    var validExtensions = new List<string>() { ".jpg", ".jpeg", ".png", ".gif", ".xlsx", ".docx", ".pdf" };
                    foreach (var file in messengeDTO.files)
                    {
                        if (!validExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                        {
                            return BadRequest($"Invalid file extension: {file.FileName}");
                        }

                        if (file.Length > 1024 * 1024 * 5) 
                        {
                            return BadRequest($"File size exceeds limit: {file.FileName}");
                        }

                    }   

                    var secureUrls = await _messengeFileService.UploadFileMessengerAsync(messengeDTO.files, "UploadFiles");

                    foreach(var file in secureUrls){
                        fileUpload.Add(file);
                    }
             
                    await _messengeService.SendMessengeAsync(senderClaims.Id.ToString(), relationShipId, messengeDTO.ReplyToId , messengeDTO.Content, fileUpload);
                } else {
                    await _messengeService.SendMessengeAsync(senderClaims.Id.ToString(), relationShipId, messengeDTO.ReplyToId , messengeDTO.Content, []);
                }

                

            } catch(Exception ex) { 
                return StatusCode(500, ex.Message);
            }

            return Ok("Send Message Ok");
           
        }

        [HttpDelete("{messengeId}")]
        public async Task<IActionResult> DeleteMessageAsync([FromRoute] int messengeId){
            var senderClaims = await _userService.GetCurrentUser(User);
            
            if (senderClaims == null)
                return Unauthorized("User is not logged in.");

            await _messengeService.DeleteMessengeAsync(senderClaims.Id.ToString(), messengeId);

            return NoContent();    
        }

    }
}