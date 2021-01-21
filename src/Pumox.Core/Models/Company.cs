using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Pumox.Core.Models
{
    #region public partial class Company
    /// <summary>
    /// Mdel danych firma
    /// Model danych firma
    /// </summary>
    [Table("Company", Schema = "pcd")]
    public partial class Company
    {
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

        #region private string _name; public string Name

        private string _name;

        [JsonProperty(nameof(Name))]
        [Column(nameof(Name), TypeName = "varchar(64)")]
        [Display(Name = "Nazwa firmy", Prompt = "Wpisz nazwę firmy", Description = "Nazwa firmy")]
        [Required]
        [MinLength(8)]
        [MaxLength(64)]
        [StringLength(64)]
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    //OnPropertyChanged(nameof(Name));
                }
            }
        }
        #endregion

        #region private int _establishmentYear;
        private int _establishmentYear;

        [JsonProperty(nameof(EstablishmentYear))]
        [Column(nameof(EstablishmentYear), TypeName = "tinyint")]
        [Display(Name = "Rok założenia firmy", Prompt = "Wpisz rok założenia firmy", Description = "Rok założenia firmy")]
        [Required]
        [Range(uint.MinValue, uint.MaxValue)]
        public int EstablishmentYear
        {
            get => _establishmentYear;
            set
            {
                if (value != _establishmentYear)
                {
                    _establishmentYear = value;
                }
            }
        }
        #endregion

        #region public virtual ICollection<Employee> Employees { get; set; }
        /// <summary>
        /// Kolekcje referencji pracowników przypisanych do firmy
        /// Collections of references of employees assigned to the company
        /// </summary>
        [Display(Name = "Pracownicy", Prompt = "Wpisz lub wybierz pracowników", Description = "Pracownicy przypisani do firmy")]
        [InverseProperty("Company")]
        public virtual ICollection<Employee> Employees { get; set; }
        #endregion
    }
    #endregion
}
