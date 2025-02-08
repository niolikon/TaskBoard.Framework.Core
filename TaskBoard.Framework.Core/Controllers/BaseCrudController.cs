using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Framework.Core.Dtos;
using TaskBoard.Framework.Core.Services;
using TaskBoard.Framework.Core.Exceptions.Rest;

namespace TaskBoard.Framework.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseCrudController<TId, TInputDto, TOutputDto> : ControllerBase
    where TInputDto : class
    where TOutputDto : BaseOutputDto<TId>
{
    protected ICrudService<TId, TInputDto, TOutputDto> _service;

    public BaseCrudController(ICrudService<TId, TInputDto, TOutputDto> service)
    {
        _service = service;
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<ActionResult<TOutputDto>> CreateAsync([FromBody] TInputDto inputDto)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestRestException("Invalid input data");
        }

        TOutputDto result = await _service.CreateAsync(inputDto);
        string actionName = nameof(ReadAsync).Replace("Async", "");
        var routeValues = new { id = result.Id };

        return CreatedAtAction(
            actionName, 
            routeValues, 
            result);
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<ActionResult<TOutputDto>> ReadAsync(TId id)
    {
        TOutputDto result = await _service.ReadAsync(id);
        return Ok(result);
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<IEnumerable<TOutputDto>>> ReadAllAsync()
    {
        IEnumerable<TOutputDto> result = await _service.ReadAllAsync();
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public virtual async Task<ActionResult<TOutputDto>> UpdateAsync(TId id, [FromBody] TInputDto inputDto)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestRestException("Invalid input data");
        }

        TOutputDto result = await _service.UpdateAsync(id, inputDto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public virtual async Task<ActionResult> DeleteAsync(TId id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
