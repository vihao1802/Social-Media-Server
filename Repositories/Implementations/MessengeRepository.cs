using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;

namespace SocialMediaServer.Repositories.Implementations
{
    public class MessengeRepository: IMessengeRepository
    {
        private readonly ApplicationDBContext _context;

        public MessengeRepository(ApplicationDBContext context){
            _context = context;
        }

        public async Task<List<Messenge>> GetMessagesByRelationshipIdAsync(int relationshipId){
            var messenges = await _context.Messenges
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Include(m => m.MediaContents) 
            .Where(m => m.RelationshipId == relationshipId)
            .OrderBy(m => m.Sent_at)
            .ToListAsync();

            return messenges;
        }

        public async Task<Messenge> GetLatestMessageByRelationshipIdAsync(int relationshipId){
            var message = await _context.Messenges
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => m.RelationshipId == relationshipId)
            .OrderByDescending(m => m.Sent_at)
            .FirstOrDefaultAsync();

            return message;
        }

        public async Task<Messenge> SendMessengeAsync(Messenge newMessenge){
            
            var messenge = await _context.Messenges.AddAsync(newMessenge);

            await _context.SaveChangesAsync();
        
            return messenge.Entity;
        }

        public async Task DeleteMessengeAsync(Messenge message){
            _context.Messenges.Remove(message);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task<Messenge> GetMessengeAsyncById(int messengeId){
            var messenge = await _context.Messenges.FirstOrDefaultAsync(x => x.id == messengeId);
            if(messenge == null){
                return null;
            }
            return messenge;
        }
    }
}