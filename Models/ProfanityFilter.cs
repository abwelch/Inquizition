using System.Collections.Generic;

namespace Inquizition.Models
{
    public static class ProfanityFilter
    {
        static private List<string> ProfanityCollection { get; }

        static public bool ContainsProfanity(string userInput)
        {
            foreach (string word in ProfanityCollection)
            {
                if (userInput.ToLower().Contains(word))
                {
                    return true;
                }
            }
            return false;
        }

        static ProfanityFilter()
        {
            // For the greater good
            ProfanityCollection = new List<string>
            {
                "fuck",
                "shit",
                "asshole",
                "cunt",
                "slut",
                "whore",
                "bitch",
                "kike",
                "queer",
                "chink",
                "nigger",
                "nigga",
                "bastard",
                "anus",
                "anal",
                "penis",
                "vagina",
                "clit",
                "cock",
                "dildo",
                "vibrator",
                "milf",
                "69",
                "420",
                "marijuana",
                "cocaine",
                "diddler",
                "orgasm",
                "nigr",
                "jackoff",
                "jizz",
                "cumshot",
                "creampie",
                "skank",
                "orgy",
                "milf",
                "towelhead",
                "erection",
                "boner",
                "spooge",
                "splooge",
                "molest",
                "wanker",
                "tittie",
                "tits",
                "boob",
                "blowjob",
                "bukkake",
                "stiffy",
                "scrotum",
                "handjob",
                "gloryhole",
                "turdburglar",
                "lesbian",
                "lesbo",
                "squirter",
                "masturbate",
                "jerkoff",
                "ballsack",
                "ballsac",
                "heroin",
                "gangbang",
                "muff",
                "munch",
                "poontang",
                "rape",
                "gentile",
                "hitler",
                "nazi",
                "isis",
                "sucker",
                "beaner",
                "fuk",
                "fag",
                "niger",
            };
        }
    }
}
