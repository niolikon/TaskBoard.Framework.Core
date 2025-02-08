using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Framework.Core.Dtos;
using TaskBoard.Framework.Core.Exceptions.Rest;
using TaskBoard.Framework.Core.Security.Authentication;
using TaskBoard.Framework.Core.Services;

namespace TaskBoard.Framework.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseSecuredCrudController<TId, TInputDto, TOutputDto> : ControllerBase
    where TInputDto : class
    where TOutputDto : BaseOutputDto<TId>
{
    protected ISecuredCrudService<TId, TInputDto, TOutputDto> _service;
    protected IAuthenticatedUserService _userService;

    public BaseSecuredCrudController(
        ISecuredCrudService<TId, TInputDto, TOutputDto> service,
        IAuthenticatedUserService userService)
    {
        _service = service;
        _userService = userService;
    }

    [HttpPost]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public virtual async Task<ActionResult<TOutputDto>> Create([FromBody] TInputDto inputDto)
    {
        AuthenticatedUser user = _userService.User;

        if (!ModelState.IsValid)
        {
            throw new BadRequestRestException("Invalid input data");
        }

        TOutputDto result = await _service.CreateAsync(inputDto, user);
        string actionName = nameof(ReadAsync).Replace("Async", "");
        var routeValues = new { id = result.Id };

        return CreatedAtAction(
            actionName,
            routeValues, 
            result);
    }

    [HttpGet]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public virtual async Task<ActionResult<IEnumerable<TOutputDto>>> ReadAllAsync()
    {
        AuthenticatedUser user = _userService.User;
        IEnumerable<TOutputDto> result = await _service.ReadAllAsync(user);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<ActionResult<TOutputDto>> ReadAsync(TId id)
    {
        AuthenticatedUser user = _userService.User;
        TOutputDto result = await _service.ReadAsync(id, user);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public virtual async Task<ActionResult<TOutputDto>> UpdateAsync(TId id, [FromBody] TInputDto inputDto)
    {
        AuthenticatedUser user = _userService.User;

        if (!ModelState.IsValid)
        {
            throw new BadRequestRestException("Invalid input data");
        }

        TOutputDto result = await _service.UpdateAsync(id, inputDto, user);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public virtual async Task<ActionResult> DeleteAsync(TId id)
    {
        AuthenticatedUser user = _userService.User;
        await _service.DeleteAsync(id, user);
        return NoContent();
    }
}
