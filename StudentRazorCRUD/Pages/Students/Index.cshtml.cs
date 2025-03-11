using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using StudentRazorCRUD.Data;
using StudentRazorCRUD.Models;
using System.Reflection;
using System.Text;

namespace StudentRazorCRUD.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public IndexModel(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _db = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IList<Student> StudentList { get; set; } = default!;
        public async Task OnGetAsync()
        {
            //StudentList = await _db.Students.ToListAsync();
            StudentList = await _db.Students.FromSqlRaw("EXEC GetStudentList").ToListAsync();
        }

        // Using EPPLUS package
        public async Task<IActionResult> OnPostExport()
        {
            StudentList = await _db.Students.FromSqlRaw("EXEC GetStudentList").ToListAsync();

            var stream = new MemoryStream();
            // Using EPPlus to generate Excel file
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set EPPlus license context
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
                workSheet.Cells.LoadFromCollection(StudentList, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = "StudentList.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        // Using DotNetCore.NPOI Package
        public async Task<IActionResult> OnPostExport2()
        {
            // Fetch data from database
            var studentList = await _db.Students.FromSqlRaw("EXEC GetStudentList").ToListAsync();

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = "Students.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();

            using (var fs = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Sheet1");

                // Creating header row
                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Id");
                row.CreateCell(1).SetCellValue("Name");
                row.CreateCell(2).SetCellValue("Email");
                row.CreateCell(3).SetCellValue("Phone");
                row.CreateCell(4).SetCellValue("Subscribed");

                // Adding data from database
                int rowIndex = 1;
                foreach (var student in studentList)
                {
                    row = excelSheet.CreateRow(rowIndex++);
                    row.CreateCell(0).SetCellValue(student.Id);
                    row.CreateCell(1).SetCellValue(student.Name);
                    row.CreateCell(2).SetCellValue(student.Email);
                    row.CreateCell(3).SetCellValue(student.Phone);
                    row.CreateCell(4).SetCellValue(student.Subscribed);
                }
                workbook.Write(fs);
            }

            // Read the file and return as a download
            using (var stream = new FileStream(file.FullName, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        // Using DotNetCore.NPOI Package
        public async Task<IActionResult> OnPostExport3()
        {
            var studentList = await _db.Students.FromSqlRaw("EXEC GetStudentList").ToListAsync();

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = "StudentData.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();

            using (var fs = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Sheet1");

                // Get properties of the Student class dynamically
                PropertyInfo[] properties = typeof(Student).GetProperties();

                // Create header row automatically
                IRow headerRow = excelSheet.CreateRow(0);
                int colIndex = 0;
                foreach (var prop in properties)
                {
                    headerRow.CreateCell(colIndex++).SetCellValue(prop.Name);
                }

                // Add data rows
                int rowIndex = 1;
                foreach (var student in studentList)
                {
                    IRow row = excelSheet.CreateRow(rowIndex++);
                    foreach (var prop in properties)
                    {
                        var value = prop.GetValue(student, null);
                        row.CreateCell(colIndex++).SetCellValue(value?.ToString() ?? string.Empty);
                    }
                }
                workbook.Write(fs);
            }

            using (var stream = new FileStream(file.FullName, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public async Task<IActionResult> OnPostExportText()
        {
            var studentList = await _db.Students.FromSqlRaw("EXEC GetStudentList").ToListAsync();

            // Define file path
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = "Students.txt";
            string filePath = Path.Combine(sWebRootFolder, sFileName);

            // Use StringBuilder to build the text content
            StringBuilder sb = new StringBuilder();

            // Get properties of Student class dynamically
            PropertyInfo[] properties = typeof(Student).GetProperties();

            // Generate header dynamically
            sb.AppendLine(string.Join(", ", properties.Select(p => p.Name)));

            // Generate data rows dynamically
            foreach (var student in studentList)
            {
                var values = properties.Select(p => p.GetValue(student, null)?.ToString() ?? string.Empty);
                sb.AppendLine(string.Join(", ", values));
            }

            // Write data to the text file
            await System.IO.File.WriteAllTextAsync(filePath, sb.ToString(), Encoding.UTF8);

            // Read the file and return as a download
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "text/plain", sFileName);
        }

        public async Task<IActionResult> OnPostExportText2()
        {
            var studentList = await _db.Students.FromSqlRaw("EXEC GetStudentList").ToListAsync();

            // Define file path
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = "SavedLists.txt";
            string filePath = Path.Combine(sWebRootFolder, sFileName);

            Directory.CreateDirectory(sWebRootFolder);

            // Open StreamWriter
            using (TextWriter tw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                foreach (var student in studentList)
                {
                    tw.WriteLine($"{student.Id}, {student.Name}, {student.Email}, {student.Phone}, {student.Subscribed}");
                }
            }

            // Read the file and return as download
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "text/plain", sFileName);
        }
    }
}

