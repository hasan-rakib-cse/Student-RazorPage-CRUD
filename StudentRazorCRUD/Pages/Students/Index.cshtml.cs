using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentRazorCRUD.Data;
using StudentRazorCRUD.Models;

namespace StudentRazorCRUD.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext context)
        {
            _db = context;
        }

        public IList<Student> StudentList { get; set; } = default!;
        public async Task OnGetAsync()
        {
            StudentList = await _db.Students.ToListAsync();
        }
    }
}
