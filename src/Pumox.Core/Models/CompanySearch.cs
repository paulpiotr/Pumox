using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetAppCommon.Validation;
using Newtonsoft.Json;

namespace Pumox.Core.Models
{
    #region public partial class CompanySearch
    /// <summary>
    /// Model danych wyszukiwania firm
    /// Company search data model
    /// </summary>
    [NotMapped]
    public partial class CompanySearch
    {
        #region private string _keyword; public string Keyword
        private string _keyword;

        [JsonProperty(nameof(Keyword))]
        [Display(Name = "Słowa kluczowe", Prompt = "Wpisz słowa kluczowe", Description = "Słowa kluczowe")]
        //[Required]
        [MinLength(3)]
        [MaxLength(64)]
        public string Keyword
        {
            get => _keyword;
            set
            {
                if (value != _keyword)
                {
                    _keyword = value;
                }
            }
        }
        #endregion

        #region private DateTime? _employeeDateOfBirthFrom; public DateTime? EmployeeDateOfBirthFrom

        private DateTime? _employeeDateOfBirthFrom;

        [JsonProperty(nameof(EmployeeDateOfBirthFrom))]
        [Display(Name = "Data urodzenia pracownika od", Prompt = "Wpisz lub wybierz datę urodzenia pracownika od", Description = "Data urodzenia pracownika od")]
        [DataType(DataType.DateTime)]
        [DateYearsRange(-120, -18)]
        public DateTime? EmployeeDateOfBirthFrom
        {
            get => _employeeDateOfBirthFrom;
            set
            {
                if (value != _employeeDateOfBirthFrom)
                {
                    _employeeDateOfBirthFrom = value;
                }
            }
        }
        #endregion

        #region private DateTime? _employeeDateOfBirthTo; public DateTime? EmployeeDateOfBirthTo

        private DateTime? _employeeDateOfBirthTo;

        [JsonProperty(nameof(EmployeeDateOfBirthTo))]
        [Display(Name = "Data urodzenia pracownika do", Prompt = "Wpisz lub wybierz datę urodzenia pracownika do", Description = "Data urodzenia pracownika do")]
        [DataType(DataType.DateTime)]
        [DateYearsRange(-120, -18)]
        public DateTime? EmployeeDateOfBirthTo
        {
            get => _employeeDateOfBirthTo;
            set
            {
                if (value != _employeeDateOfBirthTo)
                {
                    _employeeDateOfBirthTo = value;
                }
            }
        }
        #endregion

        #region private IEnumerable<Employee.JobTitles> _employeeJobTitles; public IEnumerable<Employee.JobTitles> EmployeeJobTitles

        private IEnumerable<Employee.JobTitles> _employeeJobTitles;

        [JsonProperty(nameof(EmployeeJobTitles))]
        [Display(Name = "Role pracownika", Prompt = "Wpisz lub wybierz role pracownika", Description = "Role pracownika")]
        public IEnumerable<Employee.JobTitles> EmployeeJobTitles
        {
            get => _employeeJobTitles;
            set
            {
                if (value != _employeeJobTitles)
                {
                    _employeeJobTitles = value;
                }
            }
        }
        #endregion
    }
    #endregion
}
