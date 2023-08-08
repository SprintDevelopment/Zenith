using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.Values.Dtos
{
    public class OperationResultDto
    {
        public OperationResultTypes OperationResultType { get; set; }
        public string ResultTitle { get; set; }
        public string ResultDescription { get; set; }
        public string UsefulParameter { get; set; }
    }
}
