using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pumox.Core.Models;

namespace Pumox.Core.ViewModels
{
    public partial class CompanySearchResultsViewModel
    {
        private List<Company> _results;

        [JsonProperty(nameof(Results))]
        [Display(Name = "Lista rezultatów wyszukiwania firm", Prompt = "Uzupełnij listę rezultatów wyszukiwania firm", Description = "Lista rezultatów wyszukiwania firm")]
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
