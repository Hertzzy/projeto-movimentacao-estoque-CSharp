using AutoMapper;
using movimentacao_de_projeto.Models;
using movimentacao_de_projeto.Dto;

//Mapper
namespace movimentacao_de_projeto.Profiles
{
    public class ProfileAutoMapper : Profile
    {
        public ProfileAutoMapper()
        {
            CreateMap<Produto, ProdutoListarDto>();
        }
    }
}