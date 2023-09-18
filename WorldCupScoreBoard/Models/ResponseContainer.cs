using SportRadar.WorldCupScoreBoard.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportRadar.WorldCupScoreBoard.Models
{
    public class ResponseContainer
    {
        public ResponseTypes ResponseType;
        public string Message = "Default Error Message";
    }
}
