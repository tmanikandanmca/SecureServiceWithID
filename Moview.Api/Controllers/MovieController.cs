using Microsoft.AspNetCore.Mvc;
using Moview.Api.DTO.Response;
using Moview.Api.Model;

namespace Moview.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{

    public Task<MovieResponseDTO> GetMovieById(int id)
    {
        
        return null;
    }
    
}