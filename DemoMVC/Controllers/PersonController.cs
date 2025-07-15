using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoMVC.Data;
using DemoMVC.Models.Process;
using DemoMVC.Models;
using OfficeOpenXml;


namespace DemoMVC.Controllers
{
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ExcelProcess _excellProcess = new ExcelProcess();
        private object _excelProcess;

        public PersonController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Person.ToListAsync());
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (Person == null)
            {
                return NotFound();
            }

            return View(Person);
        }

        public IActionResult Create()
        {
            AutoGenerateId autoGenerateId = new AutoGenerateId();
            var person = _context.Person.OrderByDescending(s => s.PersonId).FirstOrDefault();
            var PersonId = person == null ? "St000" : person.PersonId;
            var newPersonId = autoGenerateId.GenerateId(PersonId);

            var newPerson = new Person
            {
                PersonId = newPersonId,
                FullName = string.Empty
            };

            return View(newPerson);
        }
        public async Task<IActionResult> Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    ModelState.AddModelError("", "Please choose excel file to upload!");
                }
                else
                {
                    var fileName = DateTime.Now.ToShortTimeString() + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels", fileName);
                    var fileLocation = new FileInfo(filePath).ToString();
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);

                        var dt = _excellProcess.ExcelToDataTable(fileLocation);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var ps = new Person();

                            ps.PersonId = dt.Rows[i][0].ToString();
                            ps.FullName = dt.Rows[i][1].ToString();
                            ps.Address = dt.Rows[i][2].ToString();

                            _context.Add(ps);
                        }

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Address")] Person person)
        {
            if (ModelState.IsValid)
            {
                person.PersonId = AutoGenerateId.Generate(_context);
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Person = await _context.Person.FindAsync(id);
            if (Person == null)
            {
                return NotFound();
            }
            return View(Person);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PersonId,FullName,Address")] Person Person)
        {
            if (id != Person.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(Person.PersonId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Person);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (Person == null)
            {
                return NotFound();
            }

            return View(Person);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var Person = await _context.Person.FindAsync(id);
            if (Person != null)
            {
                _context.Person.Remove(Person);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(string id)
        {
            return _context.Person.Any(e => e.PersonId == id);
        }
        public IActionResult Download()
        {
            var fileName = "YourFileName" + ".xlsx";
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A1"].Value = "PersonID";
                worksheet.Cells["B1"].Value = "FullName";
                worksheet.Cells["C1"].Value = "Address";

                var personList = _context.Person.ToList();

                worksheet.Cells["A2"].LoadFromCollection(personList);

                var stream = new MemoryStream(excelPackage.GetAsByteArray());

                return File(stream, 
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                            fileName);
            }
        }

       
    }
}