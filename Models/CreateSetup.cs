using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inquizition.Models
{
    public class CreateSetup
    {
        public string InquizitionName { get; set; }

        public string SelectedAssessment { get; set; }

        public bool IsPrivate { get; set; }
    }
}
