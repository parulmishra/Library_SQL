using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System;
// using  System.Web.Mvc;

namespace Library.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet("/allBooks")]
    public ActionResult Books()
    {
      // List<Book> allBooks = new List<Book>{new Book("Be like a man act like a woman")};
      List<Book> allBooks = Book.GetAll();
      return View(allBooks);
    }
    [HttpGet("/allAuthors")]
    public ActionResult Authors()
    {
      List<Author> allAuthors = Author.GetAll();
      return View(allAuthors);
    }
    [HttpGet("/allPatrons")]
    public ActionResult Patrons()
    {
      List<Patron> allPatrons = Patron.GetAll();
      return View(allPatrons);
    }
    [HttpGet("/allCopies")]
    public ActionResult Copies()
    {
      List<Copy> allCopies = Copy.GetAll();
      return View(allCopies);
    }
    [HttpGet("/allCheckouts")]
    public ActionResult Checkouts()
    {
      List<Checkout> checkouts = Checkout.GetAll();
      return View(checkouts);
    }
    [HttpGet("/bookform")]
    public ActionResult BookForm()
    {
      return View();
    }
    [HttpPost("/bookform/add")]
    public ActionResult AddBook()
    {
      string ids = Request.Form["authorIds"];
      int numberOfCopies = int.Parse(Request.Form["copies"]);
      string [] idArr = ids.Split(',');
      Book newBook = new Book(Request.Form["title"]);
      newBook.Save();
      for(int i =0; i<numberOfCopies; i++)
      {
        var copy = new Copy(newBook.GetId());
        copy.Save();
      }
      foreach(var id in idArr)
      {
        newBook.AddAuthor(Author.Find(int.Parse(id)));
      }
      return RedirectToAction("Books");
    }
    [HttpGet("/authorform")]
    public ActionResult AuthorForm()
    {
      return View();
    }
    [HttpPost("/authorform/add")]
    public ActionResult AddAuthor()
    {
      string name = Request.Form["author"];
      Author newAuthor = new Author(name);
      newAuthor.Save();
      return RedirectToAction("Authors");
    }

    [HttpGet("/patronform")]
    public ActionResult PatronForm()
    {
      return View();
    }
    [HttpPost("/patronform/add")]
    public ActionResult AddPatron()
    {
      string name = Request.Form["patron"];
      Patron newPatron = new Patron(name);
      newPatron.Save();
      return RedirectToAction("Patrons");
    }
    [HttpGet("/bookDetails/{id}")]
    public ActionResult BookDetails(int id)
    {
      Dictionary<string,object> model = new Dictionary<string,object>();
      Book selectedBook = Book.Find(id);
      model["book"] = selectedBook;
      model["authors"] = selectedBook.GetAuthors();
      model["copies"] = selectedBook.GetCopies();
      return View(model);
    }

    [HttpGet("/authorDetails/{id}")]
    public ActionResult AuthorDetails(int id)
    {
      Dictionary<string,object> model = new Dictionary<string,object>();
      Author author = Author.Find(id);
      List<Book> books = author.GetBooks();

      model["author"] = author;
      model["books"] = books;
      return View(model);
    }

    [HttpGet("/patronDetails/{id}")]
    public ActionResult PatronDetails(int id)
    {
      Dictionary<string,object> model = new Dictionary<string,object>();
      Patron patron = Patron.Find(id);
      List<Book> patronBooks = patron.GetBooks();

      model["patron"] = patron;
      model["patronBooks"] = patronBooks;
      model["allBooks"] = Book.GetAll();
      return View(model);
    }
    [HttpPost("/patronDetails/checkoutBook/{patronId}/{bookId}")]
    public ActionResult CheckoutBook(int patronId, int bookId)
    {
      Patron patron = Patron.Find(patronId);
      Book book = Book.Find(bookId);
      List<Copy> copiesForThisBook = book.GetCopies();
      for(int i=0; i<copiesForThisBook.Count;i++)
      {
        if(copiesForThisBook[i].GetAvailable())
        {
          patron.AddToCheckout(copiesForThisBook[i]);
          copiesForThisBook[i].SetAvailable(false);
          break;
        }
      }
      return RedirectToAction("PatronDetails");
    }

    // [HttpGet("/books/new")]
    // public ActionResult CourseForm()
    // {
    //   List<Patron> departments = Department.GetAll();
    //   return View(departments);
    // }
    // [HttpPost("/books/new")]
    // public ActionResult CourseCreate()
    // {
    //   Course newCourse = new Course(Request.Form["course-name"]);
    //   newCourse.Save();
    //   List<Course> allCourses = Course.GetAll();
    //   return RedirectToAction("Courses");
    // }
    // [HttpGet("/students")]
    // public ActionResult Students()
    // {
    //   List<Student> allStudents = Student.GetAll();
    //   return View(allStudents);
    // }
    //

    //
    // [HttpGet("/departments/new")]
    // public ActionResult DepartmentForm()
    // {
    //   List<Department> departments = Department.GetAll();
    //   return View(departments);
    // }
    // [HttpPost("/departments/new")]
    // public ActionResult DepartmentCreate()
    // {
    //   string departmentName = Request.Form["department-name"];
    //   Department department = new Department(departmentName);
    //   department.Save();
    //   return RedirectToAction("Departments");
    // }
    // [HttpGet("/courses/new")]
    // public ActionResult CourseForm()
    // {
    //   List<Department> departments = Department.GetAll();
    //   return View(departments);
    // }
    // [HttpPost("/courses/new")]
    // public ActionResult CourseCreate()
    // {
    //   Course newCourse = new Course(Request.Form["course-name"]);
    //   newCourse.Save();
    //   List<Course> allCourses = Course.GetAll();
    //   return RedirectToAction("Courses");
    // }
//
//     [HttpGet("/students/new")]
//     public ActionResult StudentForm()
//     {
//         return View();
//     }
//
//     [HttpPost("/students/new")]
//     public ActionResult StudentCreate()
//     {
//
//         Student newStudent = new Student(Request.Form["student-name"], DateTime.Parse(Request.Form["enrollmentDate"]));
//
//         newStudent.Save();
//         List<Student> allStudents = Student.GetAll();
//         return View("Students", allStudents);
//     }
//
//     [HttpGet("/students/{id}")]
//     public ActionResult StudentDetail(int id)
//     {
//         Dictionary<string, object> model = new Dictionary<string, object>();
//         Student selectedStudent= Student.FindStudentById(id);
//         List<Course> studentCourses = selectedStudent.GetCourses();
//         List<Course> allCourses = Course.GetAll();
//         model.Add("student", selectedStudent);
//         model.Add("studentCourses", studentCourses);
//         model.Add("allCourses", allCourses);
//         return View( model);
//
//     }
//
//     [HttpGet("/courses/{id}")]
//     public ActionResult CourseDetail(int id)
//     {
//         Dictionary<string, object> model = new Dictionary<string, object>();
//         Course selectedCourse = Course.Find(id);
//         List<Student> courseStudents = selectedCourse.GetStudents();
//         List<Student> allStudents = Student.GetAll();
//         model.Add("selectedCourse", selectedCourse);
//         model.Add("courseStudents", courseStudents);
//         model.Add("allStudents", allStudents);
//         return View(model);
//     }
//
// //AD CATEGORY TO TASK
//     [HttpPost("/students/add_course")]
//     public ActionResult StudentAddCourse()
//     {
//         Student student = Student.FindStudentById(Int32.Parse(Request.Form["student-id"]));
//
//         Course course = Course.Find(Int32.Parse(Request.Form["course-id"]));
//         student.AddCourse(course);
//
//         Dictionary<string, object> model = new Dictionary<string, object>();
//         Student selectedStudent = Student.FindStudentById(student.GetId());
//         List<Course> studentCourses = selectedStudent.GetCourses();
//         List<Course> allCourses = Course.GetAll();
//         model.Add("student", selectedStudent);
//         model.Add("studentCourses", studentCourses);
//         model.Add("allCourses", allCourses);
//         return View("StudentDetail", model);
//     }
//
// //ADD TASK TO CATEGORY
//     [HttpPost("courses/add_student")]
//     public ActionResult CourseAddStudent()
//     {
//       Course course = Course.Find(Int32.Parse(Request.Form["course-id"]));
//       Student student = Student.FindStudentById(Int32.Parse(Request.Form["student-id"]));
//
//       course.AddStudent(student);
//
//       Dictionary<string, object> model = new Dictionary<string, object>();
//       Course selectedCourse = Course.Find(course.GetId());
//       List<Student> courseStudents = selectedCourse.GetStudents();
//       List<Student> allStudents = Student.GetAll();
//       model.Add("selectedCourse", selectedCourse);
//       model.Add("courseStudents", courseStudents);
//       model.Add("allStudents", allStudents);
//       return View("CourseDetail", model);
//     }
//     [HttpGet("/courses/delete/{id}")]
//     public ActionResult DeleteCourse(int id)
//     {
//       Course course = Course.Find(id);
//       course.Delete();
//
//       return RedirectToAction("Courses");
//     }
//
//     [HttpGet("/students/delete/{id}")]
//     public ActionResult DeleteStudent(int id)
//     {
//       Student student = Student.FindStudentById(id);
//       student.Delete();
//
//       return RedirectToAction("Students");
//     }

  }
}
