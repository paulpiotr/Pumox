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

        // GET: api/Company
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanyAsync()
        {
            return await _context.Company.ToListAsync();
        }

        // GET: api/Company/5
        [HttpGet("{id}/{isOnlyId?}")]
        public async Task<ActionResult<object>> GetCompanyAsync(long id, bool isOnlyId = true)
        {
            Company company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return isOnlyId ? (new { id = company.Id }) : (ActionResult<object>)company;
        }

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
                    if (id != company.Id)
                    {
                        return BadRequest();
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

        // POST: [controller]/Create
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/[controller]/Create")]
        public async Task<ActionResult<Company>> PostCompanyAsync(Company company)
        {
            if (ModelState.IsValid)
            {
                if (null != _context && null != company)
                {
                    _context.Company.Add(company);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetCompanyAsync), new { company.Id, isOnlyId = true }, new { id = company.Id });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
            return NotFound();
        }

        // POST: [controller]/Create
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/[controller]/Search")]
        //public async Task<ActionResult<List<Company>>> PostSearchCompanyAsync(CompanySearch companySearch)
        public async Task<ActionResult<List<Company>>> PostSearchCompanyAsync(CompanySearch companySearch)
        {
            if (ModelState.IsValid)
            {
                if (null != _context)
                {
                    /// Keyword
                    var keyword = new SqlParameter("keyword", System.Data.SqlDbType.NVarChar)
                    {
                        Value = string.Format("{0}{1}{2}", "%", companySearch.Keyword, "%")
                    };

                    /// EmployeeDateOfBirthFrom
                    var employeeDateOfBirthFrom = new SqlParameter("employeeDateOfBirthFrom", System.Data.SqlDbType.DateTime)
                    {
                        Value = null != companySearch.EmployeeDateOfBirthFrom ? companySearch.EmployeeDateOfBirthFrom : DateTime.Now.AddYears(-120)
                    };

                    /// EmployeeDateOfBirthTo
                    var employeeDateOfBirthTo = new SqlParameter("employeeDateOfBirthTo", System.Data.SqlDbType.DateTime)
                    {
                        Value = null != companySearch.EmployeeDateOfBirthTo ? companySearch.EmployeeDateOfBirthTo : DateTime.Now.AddYears(-18)
                    };

                    /// EmployeeJobTitles
                    List<sbyte> employeeJobTitlesList = companySearch.EmployeeJobTitles.Cast<sbyte>().ToList() ?? Enum.GetValues(typeof(Employee.JobTitles)).Cast<sbyte>().ToList();

                    /// EmployeeJobTitles
                    var employeeJobTitles = new SqlParameter("employeeJobTitles", System.Data.SqlDbType.VarChar)
                    {
                        Value = string.Join(",", employeeJobTitlesList.ConvertAll(x => x.ToString()))
                    };

                    List<long> employeeList = await _context.Employee
                        .FromSqlRaw(Regex.Replace("" +
                        "SELECT e.CompanyId FROM [pcd].[Employee] e WHERE " +
                            " e.FirstName LIKE @keyword OR " +
                            " e.LastName LIKE  @keyword OR " +
                            " (e.DateOfBirth BETWEEN @employeeDateOfBirthFrom AND @employeeDateOfBirthTo) OR " +
                            " e.JobTitle IN (SELECT CAST(value AS tinyint) FROM STRING_SPLIT(@employeeJobTitles, ',')) " +
                        " UNION " +
                        "SELECT c.Id FROM [pcd].[Company] c WHERE c.Name LIKE @keyword ", @"\s+", " ").Trim(), keyword, employeeDateOfBirthFrom, employeeDateOfBirthTo, employeeJobTitles)
                        .Select(s => s.CompanyId)
                        .ToListAsync();

                    return await _context.Company.Where(w => (null != employeeList && employeeList.Count > 0 && employeeList.Contains(w.Id))).Include(i => i.Employees).ToListAsync();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
            return NotFound();
        }

        // DELETE: api/Company/5
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

        private bool CompanyExists(long id)
        {
            return _context.Company.Any(e => e.Id == id);
        }
    }
}
