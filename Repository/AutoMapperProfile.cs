using AutoMapper;
using Entities.Models;
using Models.DbModels;

namespace Repository
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserView, UserDisplayV2>();
        }
    }
}