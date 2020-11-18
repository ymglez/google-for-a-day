using System;
using System.Linq;
using System.Threading.Tasks;
using GoogleForADay.Core.Abstractions.Indexer;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model.Indexer;
using GoogleForADay.Core.Model.Store;
using GoogleForADay.Services.Business.Validators;

namespace GoogleForADay.Services.Business.Controllers
{
    public class SearchEngineController
    {
        private IndexerManagerBase Indexer { get; }
        public IKeyValueRepository<Keyword> Repository { get; }

        public SearchEngineController(IndexerManagerBase indexer, IKeyValueRepository<Keyword> repository)
        {
            Indexer = indexer;
            Repository = repository;
        }

        public async Task<IndexResponse> Index(string url, int depth = 2)
        {
            try
            {
                if(!InputValidator.ValidUrl(url)) 
                    throw new ArgumentException($"input '{url}' is not valid url");

                var response = await Indexer.Index(url, depth);
                // transform response according business need
                response.IndexedWordsCount= response.IndexedWords.Count;
                response.IndexedWords = null;
                return response;
            }
            catch (Exception e)
            {
                System.IO.File.AppendAllText("errors.log", $"{e.Message}\n");
                throw;
            }
        }


        public Keyword Search(string word)
        {
            try
            {
                if (!InputValidator.ValidWord(word))
                    throw new ArgumentException($"input '{word}' is not valid word"); 

                var response = Repository.Get(word.ToLowerInvariant());

                if (response == null)
                    return null;

                // transform response according business need
                response.References = response.References.OrderByDescending(r => r.Occurrences).ToList();
                return response;
            }
            catch (Exception e)
            {
                System.IO.File.AppendAllText("errors.log", $"{e.Message}\n");
                throw;
            }
        }



    }
}
