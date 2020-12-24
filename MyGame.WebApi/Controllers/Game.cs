using System;
using System.Collections.Generic;

namespace MyGame.WebApi.Controllers
{
    public class Game
    {
        public Guid Id { get; set; }
        public List<string> Players { get; set; }
    }
}