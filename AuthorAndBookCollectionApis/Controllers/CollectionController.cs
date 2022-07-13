using AuthorAndBookCollectionApis.Entities;
using AuthorAndBookCollectionApis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorAndBookCollectionApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly ICollectionService _service;

        public CollectionController(ICollectionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Author>>> GetCollectionByAuthorIds([FromQuery]List<string> authorIds)
            => await _service.GetAuthorsByIds(authorIds);
    }
}
