using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerFlow.Domain.DTOs;
using PartnerFlow.Domain.Interfaces.Services;

namespace PartnerFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidoController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpPost]
    public async Task<IActionResult> CriarPedido([FromBody] PedidoDto pedido)
    {
        await _pedidoService.CriarPedidoAsync(pedido);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPedido(Guid id)
    {
        var pedido = await _pedidoService.ObterPedidoAsync(id);
        if (pedido == null) return NotFound();
        return Ok(pedido);
    }
}