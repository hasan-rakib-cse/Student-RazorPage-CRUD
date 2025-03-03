using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentRazorCRUD.Data;
using StudentRazorCRUD.Models;

namespace StudentRazorCRUD.Pages.Students
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DetailsModel(ApplicationDbContext context)
        {
            _db = context;
        }

        public Student Std { get; set; } = default!;

        public async Task <IActionResult> OnGetAsync(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var student = await _db.Students.FirstOrDefaultAsync(s => s.Id == id);
            if(student == null)
            {
                return NotFound();
            }
            else
            {
                Std = student;
            }
            return Page();
        }
    }
}
