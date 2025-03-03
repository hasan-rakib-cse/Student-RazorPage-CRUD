using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentRazorCRUD.Data;
using StudentRazorCRUD.Models;

namespace StudentRazorCRUD.Pages.Students
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DeleteModel(ApplicationDbContext context)
        {
            _db = context;
        }

        public Student Std { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
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
            Std = student;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if(id == null)
            {
                return Page();
            }
            var studentData = await _db.Students.FindAsync(id);
            if(studentData != null)
            {
                _db.Students.Remove(studentData);
            }
            await _db.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
