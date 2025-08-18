using AutoMapper;
using PartnerFlow.Domain.DTOs;
using PartnerFlow.Domain.Entities;

namespace PartnerFlow.Domain.Mapping;

public class PedidoProfile : Profile
{
    public PedidoProfile()
    {
        CreateMap<PedidoDto, Pedido>();

        CreateMap<ItemPedidoDto, ItemPedido>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.PedidoId, opt => opt.Ignore());
    }
}
