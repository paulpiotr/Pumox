#region using

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Pumox.Core.Models;

#endregion

namespace Pumox.Core.ViewModels
{
    public class CompanySearchResultsViewModel
    {
        private List<Company> _results;

        [JsonProperty(nameof(Results))]
        [Display(Name = "Lista rezultatów wyszukiwania firm", Prompt = "Uzupełnij listę rezultatów wyszukiwania firm",
            Description = "Lista rezultatów wyszukiwania firm")]
        public List<Company> Results
        {
            get => _results;
            set
            {
                if (value != _results)
                {
                    _results = value;
                }
            }
        }
    }
}
