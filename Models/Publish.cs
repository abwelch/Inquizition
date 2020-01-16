using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inquizition.Data;

namespace Inquizition.Models
{
    public class Publish
    {
        public int ID { get; set; }



        public string InquizitorName { get; set; }

        public bool Published { get; set; }
    }

    public interface IPublishManager
    {
        public void RemoveUnpublished(string name);
    }

    public class PublishManager : IPublishManager
    {
        private readonly InquizitionContext _dbContext;

        public PublishManager(InquizitionContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void RemoveUnpublished(string inquizName)
        {



            /*
            var inquizToDelete = _dbContext.Publish.Where(p => p.Published == false);
            foreach (Publish p in inquizToDelete)
            {
                var entryToDelete = _dbContext.FlashCards.Where(f => f.InquizitorName == p.InquizitorName);
                foreach (FlashCardEntry f in entryToDelete)
                {

                }
            } */
        }

    }
}
