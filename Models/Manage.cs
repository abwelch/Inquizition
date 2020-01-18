using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inquizition.Models
{
    public class ManageIndex
    {
        public List<string> FlashCardInquizitorNames { get; set; }

        public List<string> TwoColumnInquizitorNames { get; set; }

        public List<string> QuizInquizitorNames { get; set; }

        [Required]
        public string Inquizitor { get; set; }

        [Required]
        public string Operation { get; set; }
    }

    public class ManageDisplay
    {
        public List<FlashCardEntry> FlashInquizitor { get; set; }

        public string Color { get; set; }
    }
}
