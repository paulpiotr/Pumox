using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pumox.Core.Database.Data;
using Pumox.Core.Models;
using Pumox.Core.ViewModels;

namespace Pumox.Core.WebApplication.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class CompanyController : ControllerBase
    {
        #region private readonly log4net.ILog log4net
        /// <summary>
        /// log4net
        /// </summary>
        private readonly log4net.ILog _log4net = Log4netLogger.Log4netLogger.GetLog4netInstance(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region private readonly AppSettings _appSettings
        /// <summary>
        /// Vies.Core.Database.Models.AppSettings
        /// </summary>
        private readonly AppSettings _appSettings = new AppSettings();
        #endregion

        #region private readonly IServiceScopeFactory _serviceScopeFactory
        /// <summary>
        /// private readonly IServiceScopeFactory _serviceScopeFactory
        /// </summary>
        private readonly IServiceScopeFactory _serviceScopeFactory;
        #endregion

        #region private readonly PumoxCoreDatabaseContext _context;
        /// <summary>
        /// private readonly PumoxCoreDatabaseContext _context
        /// </summary>
        private readonly PumoxCoreDatabaseContext _context;
        #endregion

        #region public CompanyController(IServiceScopeFactory serviceScopeFactory)
        public CompanyController(IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                _serviceScopeFactory = serviceScopeFactory;
                _context = serviceScopeFactory.CreateScope().ServiceProvider.GetService<PumoxCoreDatabaseContext>();
            }
            catch (Exception e)
            {
                _log4net.Error(string.Format("\n{0}\n{1}\n{2}\n{3}\n", e.GetType(), e.InnerException?.GetType(), e.Message, e.StackTrace), e);
            }
        }
        #endregion

        #region public async Task<IActionResult> PutCompanyAsync(long id, Company company)
        // PUT: api/Company/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        [HttpPut("/[controller]/Update/{id}")]
        public async Task<IActionResult> PutCompanyAsync(long id, Company company)
        {
            if (ModelState.IsValid)
            {
                if (null != _context && null != company)
                {
                    if (id != company?.Id)
                    {
                        return BadRequest($"Pole Id [{ company?.Id }] w przekazanym obiekcie Company musi być oznaczone i jednoznaczne z polem Id [{ id }] przekazanym parametrze URL!");
                    }
                    var employeeList = company?.Employees?.ToList();
                    if (null != employeeList)
                    {
                        foreach (Employee employee in employeeList)
                        {
                            if (employee.CompanyId > 0 && employee?.CompanyId != company?.Id)
                            {
                                return BadRequest($"Pole CompanyId [{ employee?.CompanyId }] w przekazanej liście Employees musi być jednoznaczne z polem Id [{ id }] przekazanym parametrze URL lub ustawione na NULL!");
                            }
                            if (employee.Id > 0 && await _context.Employee.Where(w => w.Id == employee.Id).Select(s => s.CompanyId).FirstOrDefaultAsync() != company.Id)
                            {
                                return BadRequest($"Pracownik [{ employee?.Id }] w przekazanej liście Employees nie należy do firmy o Id [{ id }] przekazanym parametrze URL!");
                            }
                        }
                    }
                    _context.Entry(company).State = EntityState.Modified;
                    if (company.Employees?.Count > 0)
                    {
                        company.Employees.ToList().ForEach(employe =>
                        {
                            _context.Entry(employe).State = employe.Id > 0 ? EntityState.Modified : EntityState.Added;
                        });
                    }
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CompanyExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return NoContent();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
            return NotFound();
        }
        #endregion

        #region public async Task<ActionResult<CompanyResultViewModel>> PostCompanyAsync(Company company)
        // POST: [controller]/Create
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/[controller]/Create")]
        public async Task<ActionResult<CompanyResultViewModel>> PostCompanyAsync(Company company)
        {
            if (ModelState.IsValid)
            {
                if (null != _context && null != company)
                {
                    _context.Company.Add(company);
                    await _context.SaveChangesAsync();
                    return Created(string.Empty, new CompanyResultViewModel { Id = (long)(company?.Id) });
                    //return CreatedAtAction(nameof(GetCompanyAsync), new { company.Id, isOnlyId = true }, new { id = company.Id });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
            return NotFound();
        }
        #endregion

        #region public async Task<ActionResult<List<Company>>> PostSearchCompanyAsync(CompanySearch companySearch = null)
        // POST: [controller]/Search
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/[controller]/Search")]
        //public async Task<ActionResult<List<Company>>> PostSearchCompanyAsync(CompanySearch companySearch)
        public async Task<ActionResult<CompanySearchResultsViewModel>> PostSearchCompanyAsync(CompanySearch companySearch = null)
        {
            if (ModelState.IsValid)
            {
                if (null != _context)
                {
                    /// Keyword
                    var keyword = new SqlParameter("keyword", System.Data.SqlDbType.NVarChar)
                    {
                        Value = string.Format("{0}{1}{2}", "%", companySearch?.Keyword, "%")
                    };

                    /// EmployeeDateOfBirthFrom
                    var employeeDateOfBirthFrom = new SqlParameter("employeeDateOfBirthFrom", System.Data.SqlDbType.DateTime)
                    {
                        Value = null != companySearch?.EmployeeDateOfBirthFrom ? companySearch?.EmployeeDateOfBirthFrom : DateTime.Now.AddYears(-120)
                    };

                    /// EmployeeDateOfBirthTo
                    var employeeDateOfBirthTo = new SqlParameter("employeeDateOfBirthTo", System.Data.SqlDbType.DateTime)
                    {
                        Value = null != companySearch?.EmployeeDateOfBirthTo ? companySearch?.EmployeeDateOfBirthTo : DateTime.Now.AddYears(-18)
                    };

                    /// EmployeeJobTitles
                    List<sbyte> employeeJobTitlesList = companySearch?.EmployeeJobTitles?.Cast<sbyte>().ToList() ?? Enum.GetValues(typeof(Employee.JobTitles)).Cast<sbyte>().ToList();

                    /// EmployeeJobTitles
                    var employeeJobTitles = new SqlParameter("employeeJobTitles", System.Data.SqlDbType.VarChar)
                    {
                        Value = string.Join(",", employeeJobTitlesList.ConvertAll(x => x.ToString()))
                    };

                    /// Build SQL QUERY
                    var sqlStringBuilder = new System.Text.StringBuilder();
                    if (null != companySearch?.Keyword)
                    {
                        sqlStringBuilder.Append(" e.FirstName LIKE @keyword ");
                        sqlStringBuilder.Append(" OR ");
                        sqlStringBuilder.Append(" e.LastName LIKE  @keyword ");
                    }
                    if(null != companySearch?.EmployeeDateOfBirthFrom && companySearch?.EmployeeDateOfBirthFrom != DateTime.MinValue && null != companySearch?.EmployeeDateOfBirthTo && companySearch?.EmployeeDateOfBirthTo != DateTime.MinValue)
                    {
                        if (sqlStringBuilder.ToString().Length > 0)
                        {
                            sqlStringBuilder.Append(" OR ");
                        }
                        sqlStringBuilder.Append(" (e.DateOfBirth BETWEEN @employeeDateOfBirthFrom AND @employeeDateOfBirthTo) ");
                    }
                    if(null != companySearch?.EmployeeJobTitles)
                    {
                        if (sqlStringBuilder.ToString().Length > 0)
                        {
                            sqlStringBuilder.Append(" OR ");
                        }
                        sqlStringBuilder.Append(" e.JobTitle IN (SELECT CAST(value AS tinyint) FROM STRING_SPLIT(@employeeJobTitles, ',')) ");
                    }
                    if (null != companySearch?.Keyword)
                    {
                        sqlStringBuilder.Append(" UNION SELECT c.Id FROM [pcd].[Company] c WHERE c.Name LIKE @keyword ");
                    }

                    sqlStringBuilder.Insert(0, sqlStringBuilder.ToString().Length > 0 ? " SELECT e.CompanyId FROM [pcd].[Employee] e WHERE 1=1 AND " : " SELECT e.CompanyId FROM [pcd].[Employee] e WHERE 1=1 ");
#if DEBUG
                    Console.WriteLine(Regex.Replace(sqlStringBuilder.ToString(), @"\s{2,}", " ").Trim());
#endif
                    /// Create query and get find ID
                    List<long> employeeList = await _context.Employee
                        .FromSqlRaw(Regex.Replace(sqlStringBuilder.ToString(), @"\s{2,}", " ").Trim(), keyword, employeeDateOfBirthFrom, employeeDateOfBirthTo, employeeJobTitles)
                        .Select(s => s.CompanyId)
                        .ToListAsync();

                    /// Create query and get find ID
                    //List<long> employeeList = await _context.Employee
                    //    .FromSqlRaw(Regex.Replace("" +
                    //        //"SELECT e.CompanyId FROM [pcd].[Employee] e WHERE 1=1 AND " +
                    //        //" e.FirstName LIKE @keyword OR " +
                    //        //" e.LastName LIKE  @keyword OR " +
                    //        //" (e.DateOfBirth BETWEEN @employeeDateOfBirthFrom AND @employeeDateOfBirthTo) OR " +
                    //        //" e.JobTitle IN (SELECT CAST(value AS tinyint) FROM STRING_SPLIT(@employeeJobTitles, ',')) " +
                    //        //" UNION SELECT c.Id FROM [pcd].[Company] c WHERE c.Name LIKE @keyword " +
                    //    "", @"\s{2,}", string.Empty).Trim(), keyword, employeeDateOfBirthFrom, employeeDateOfBirthTo, employeeJobTitles)
                    //    .Select(s => s.CompanyId)
                    //    .ToListAsync();

                    return new CompanySearchResultsViewModel { Results = await _context.Company.Where(w => (null != employeeList && employeeList.Count > 0 && employeeList.Contains(w.Id))).Include(i => i.Employees).ToListAsync() };
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
            return NotFound();
        }
        #endregion

        #region public async Task<IActionResult> DeleteCompanyAsync(long id)
        // DELETE: [controller]/Delete/{id}
        [HttpDelete("/[controller]/Delete/{id}")]
        public async Task<IActionResult> DeleteCompanyAsync(long id)
        {
            Company company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            _context.Company.Remove(company);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        private bool CompanyExists(long id)
        {
            return _context.Company.Any(e => e.Id == id);
        }
    }
}
