using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inquizition.Data;

namespace Inquizition.Models
{
    public interface IColorThemeManager
    {
        public void AddColorTheme(string color, string name, string creator);

        public void ClearUnathenticatedTheme(string flagUsername);

        public string RetrieveCardColor(string inquziName);
    }

    public class ColorThemeManager : IColorThemeManager
    {
        private readonly InquizitionContext _dbContext;

        public ColorThemeManager(InquizitionContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string RetrieveCardColor(string inquziName)
        {
            var entry = _dbContext.ColorTheme.FirstOrDefault(c => c.InquizitorName == inquziName);
            if (entry == null)
                return string.Empty;
            return entry.Color;
        }

        public void AddColorTheme(string color, string name, string creator)
        {
            ColorTheme newColorEntry = new ColorTheme
            {
                Color = color,
                InquizitorName = name,
                Creator = creator
            };
            _dbContext.ColorTheme.Add(newColorEntry);
            _dbContext.SaveChanges();
        }

        public void ClearUnathenticatedTheme(string flagUsername)
        {
            var toDeleteThemes = _dbContext.ColorTheme.Where(c => c.Creator == flagUsername);
            foreach (ColorTheme c in toDeleteThemes)
            {
                _dbContext.ColorTheme.Remove(c);
            }
            _dbContext.SaveChanges();
        }
    }

    // Database model
    public class ColorTheme
    {
        public int ID { get; set; }

        public string Creator { get; set; }

        public string Color { get; set; }

        public string InquizitorName { get; set; }
    }
}
