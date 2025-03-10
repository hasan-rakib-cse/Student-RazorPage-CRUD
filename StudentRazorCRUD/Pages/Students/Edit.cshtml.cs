using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentRazorCRUD.Data;
using StudentRazorCRUD.Models;

namespace StudentRazorCRUD.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public EditModel(ApplicationDbContext context)
        {
            _db = context;
        }

        //public IActionResult OnGet()
        //{
        //    return Page();
        //}

        [BindProperty]
        public Student Std { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var student = await _db.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            Std = student;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                _db.Students.Update(Std);
                await _db.SaveChangesAsync();
            }
            catch(Exception err)
            {
                Console.WriteLine(err.Message);
                ModelState.AddModelError("", "An error occurred while updating the student record.");
                return Page();
            }
            //catch(DbUpdateConcurrencyException)
            //{
            //    if(!StudentExist(Std.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}            
            return RedirectToPage("./Index");
        }

        //private bool StudentExist(int id)
        //{
        //    return _db.Students.Any(s => s.Id == id);
        //}
    }
}
