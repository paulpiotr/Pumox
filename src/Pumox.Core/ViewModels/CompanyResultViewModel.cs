using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetAppCommon.Validation;
using Newtonsoft.Json;

namespace Pumox.Core.ViewModels
{
    [NotMapped]
    public partial class CompanyResultViewModel
    {
        #region private long _id;
        private long _id;

        [JsonProperty(nameof(Id))]
        [Display(Name = "Identyfikator Id zwracany w rezultacie tworzenia noego rekordu", Prompt = "Wpisz identyfikator Id zwracany w rezultacie tworzenia noego rekordu", Description = "Identyfikator Id zwracany w rezultacie tworzenia noego rekordu")]
        public long Id
        {
            get => _id;
            set
            {
                if(value != _id)
                {
                    _id = value;
                }
            }
        }
        #endregion
    }
}
