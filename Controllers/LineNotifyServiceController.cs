using LineNotifyService.API.Repositories;
using LineNotifyService.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LineNotifyService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LineNotifyServiceController : ControllerBase
    {
        private readonly LineNotifyServiceRepository lineNotifyServiceRepository;
        public LineNotifyServiceController(LineNotifyServiceRepository lineNotifyServiceRepository)
        {
            this.lineNotifyServiceRepository = lineNotifyServiceRepository;
        }

        [HttpGet]
        public IActionResult OAuthLineNotifyUrl([FromQuery] GetOAuthParam param)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(lineNotifyServiceRepository.OAuthLineNotifyUrl(param));
        }

        [HttpPost]
        public async Task<IActionResult> OAuthExchangeCode([FromBody] GetTokenParam param)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(await lineNotifyServiceRepository.OAuthExchangeCode(param));
        }

        [HttpPost]
        public async Task<IActionResult> SendNotify([FromBody] NotifyParam param)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(await lineNotifyServiceRepository.SendNotify(param));
        }

        [HttpGet]
        public async Task<IActionResult> CheckStatus([FromQuery] CheckStatusParam param)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(await lineNotifyServiceRepository.CheckStatus(param));
        }

        [HttpPost]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenParam param)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(await lineNotifyServiceRepository.RevokeToken(param));
        }

    }
}
