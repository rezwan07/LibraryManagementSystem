using AutoMapper;
using LibraryManagementSystem.Core.Entities;
using LibraryManagementSystem.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Book mappings
            CreateMap<Book, BookDTO>();
            CreateMap<CreateBookDTO, Book>();
            CreateMap<UpdateBookDTO, Book>();

            //User mappings
            // PasswordHash auto ignore
            CreateMap<User, UserDTO>();
        }
    }
}
