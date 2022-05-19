using _20220516.Models;
using _20220516.Service;
using Microsoft.AspNetCore.Mvc;

namespace _20220516.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public IActionResult Index(int nowPage = 1)
        {
            // var list = _studentService.GetStudents();

            int count = 10; // 取幾筆
            int offset = (nowPage - 1) * count; // 從第幾筆開始
            var (total, list) = _studentService.GetStudents(offset, count); // 取得全部筆數及資料

            ViewData["Total"] = total; // 全部筆數
            ViewData["nowPage"] = nowPage; // 目前在第幾頁

            return View(list);
        }

        [HttpPost]
        public IActionResult Index(Dictionary<string, string> queryDic, int nowPage = 1)
        {

            int count = 10; // 取幾筆
            int offset = (nowPage - 1) * count; // 從第幾筆開始
            var (total, list) = _studentService.GetStudents(offset, count, queryDic);

            ViewData["Total"] = total; // 全部筆數
            ViewData["nowPage"] = nowPage; // 目前在第幾頁

            ViewData["query_studentName"] = queryDic["studentName"];
            ViewData["query_studentNo"] = queryDic["studentNo"];
            ViewData["query_githubLink"] = queryDic["githubLink"];

            return View(list);
        }

        public IActionResult Update(string studentNo)
        {
            var student = _studentService.GetStudentByStudentNo(studentNo);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Student student)
        {
            if (ModelState.IsValid) // 是否通過驗證
            {
                _studentService.UpdateStudent(student);
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public IActionResult Delete(string studentNo)
        {
            var student = _studentService.GetStudentByStudentNo(studentNo);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(string studentNo)
        {
            _studentService.DeleteStudent(studentNo);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            _studentService.CreateStudent(student);
            return RedirectToAction("Index");
        }

        public IActionResult Detail(string studentNo)
        {
            var student = _studentService.GetStudentByStudentNo(studentNo);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
    }
}
