using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Application.RepositoryInterfaces;
using PhotoAlbum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PhotoAlbum.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;

        public CommentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            try
            {
                return await _context.Comments.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving comments", ex);
            }
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            try
            {
                return await _context.Comments.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving comment with ID {id}", ex);
            }
        }

        public async Task AddCommentAsync(Comment comment)
        {
            try
            {
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding comment", ex);
            }
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            try
            {
                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while updating comment with ID {comment.Id}", ex);
            }
        }

        public async Task DeleteCommentAsync(int id)
        {
            try
            {
                var comment = await _context.Comments.FindAsync(id);
                if (comment != null)
                {
                    _context.Comments.Remove(comment);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while deleting comment with ID {id}", ex);
            }
        }
    }
}
