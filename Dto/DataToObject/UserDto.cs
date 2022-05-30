using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;
namespace Dto
{
    public class UserDto
    {
        public UserDto()
        {
                
        }
        public UserDto(string p,string n,string b,string e)
        {
            Phone = p;
            userName = n;
            businessName = b;
            emailAddress = e;

        }
        public string Phone { get; set; }
        public string userName { get; set; }
        public string businessName { get; set; }
        public string emailAddress { get; set; }

        public static UserDto DalToDto(User_tbl user)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<User_tbl, UserDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<UserDto>(user);

        }
        public User_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<UserDto, User_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<User_tbl>(this);
        }
    }
}
