#region using

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using NetAppCommon.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#endregion

namespace Pumox.Core.Models
{
    #region public partial class Employee

    /// <summary>
    ///     Model danych pracownik
    ///     Employee data model
    /// </summary>
    [Table("Employee", Schema = "pcd")]
    public class Employee
    {
        #region public enum JobTitles : sbyte

        public enum JobTitles : sbyte
        {
            [EnumMember(Value = "Administrator")] Administrator,
            [EnumMember(Value = "Developer")] Developer,
            [EnumMember(Value = "Architect")] Architect,
            [EnumMember(Value = "Manager")] Manager
        }

        #endregion

        #region public virtual Company Company { get; set; }

        /// <summary>
        ///     Referencja do obiektu modelu firma jako Company
        ///     The company model object is referenced as Company
        /// </summary>
        [JsonProperty(nameof(Company))]
        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }

        #endregion

        #region private long _id; public long Id

        private long _id;

        [Key]
        [JsonProperty(nameof(Id))]
        /// Tylko do migracji
        /// [Column(nameof(Id), TypeName = "bigint identity(1, 1)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id
        {
            get => _id;
            private set
            {
                if (value != _id)
                {
                    _id = value;
                }
            }
        }

        #endregion

        #region private long _companyId; public long CompanyId

        private long _companyId;

        /// <summary>
        ///     Identyfikator firmy (klucz obcy)
        ///     Company ID (foreign key)
        /// </summary>
        [JsonProperty(nameof(CompanyId))]
        [Column(nameof(CompanyId), TypeName = "bigint")]
        [Display(Name = "Identyfikator firmy", Prompt = "Wybierz lub wpisz identyfikator firmy",
            Description = "Identyfikator firmy (klucz obcy)")]
        public long CompanyId
        {
            get => _companyId;
            set
            {
                if (value != _companyId)
                {
                    _companyId = value;
                }
            }
        }

        #endregion

        #region private string _firstName; public string FirstName

        private string _firstName;

        [JsonProperty(nameof(FirstName))]
        [Column(nameof(FirstName), TypeName = "varchar(64)")]
        [Display(Name = "Imię", Prompt = "Wpisz imię", Description = "Imię")]
        [Required]
        [MinLength(3)]
        [MaxLength(64)]
        [StringLength(64)]
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (value != _firstName)
                {
                    _firstName = value;
                    //OnPropertyChanged(nameof(Name));
                }
            }
        }

        #endregion

        #region private string _lastName; public string LastName

        private string _lastName;

        [JsonProperty(nameof(LastName))]
        [Column(nameof(LastName), TypeName = "varchar(64)")]
        [Display(Name = "Nazwisko", Prompt = "Wpisz nazwisko", Description = "Nazwisko")]
        [Required]
        [MinLength(3)]
        [MaxLength(64)]
        [StringLength(64)]
        public string LastName
        {
            get => _lastName;
            set
            {
                if (value != _lastName)
                {
                    _lastName = value;
                    //OnPropertyChanged(nameof(Name));
                }
            }
        }

        #endregion

        #region private DateTime _dateOfBirth; public DateTime DateOfBirth

        private DateTime _dateOfBirth;

        [JsonProperty(nameof(DateOfBirth))]
        //[Column(nameof(LastName), TypeName = "")]
        [Display(Name = "Data urodzenia", Prompt = "Wpisz lub wybierz datę urodzenia", Description = "Data urodzenia")]
        [Required]
        [DataType(DataType.Date)]
        [DateYearsRange(-120, -18)]
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (value != _dateOfBirth)
                {
                    _dateOfBirth = value;
                }
            }
        }

        #endregion

        #region private JobTitles _jobTitle; public JobTitles JobTitle

        private JobTitles _jobTitle;

        [JsonProperty(nameof(JobTitle))]
        [JsonConverter(typeof(StringEnumConverter))]
        [Column("JobTitle", TypeName = "tinyint")]
        [Display(Name = "Stanowisko", Prompt = "Wpisz lub wybierz stanowisko", Description = "Stanowisko")]
        [Required]
        [Range(0, 3)]
        public JobTitles JobTitle
        {
            get => _jobTitle;
            private set
            {
                if (value != _jobTitle)
                {
                    _jobTitle = value;
                }
            }
        }

        #endregion
    }

    #endregion
}
