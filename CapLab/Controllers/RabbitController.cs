using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CapLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitController : ControllerBase
    {

        private readonly ICapPublisher _publisher;
        private readonly ILogger<RabbitController> _logger;
        public RabbitController(ICapPublisher publisher, ILogger<RabbitController> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        [HttpGet, Route("send-message")]
        public async Task<IActionResult> SendMessageAsync()
        {
            var userId = Guid.NewGuid();
            await _publisher.PublishAsync("ryan.exchange.direct", new { UserId = userId });
            return Ok(userId);
        }

        [NonAction]
        [CapSubscribe("ryan.exchange.direct", Group = "ryan.hello")]
        public void MessageSubscriber(object payload)
        {
            var result = JsonConvert.SerializeObject(payload);
            _logger.Log(LogLevel.Information, result);
        }
    }
}