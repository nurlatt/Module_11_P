using System;
using System.Collections.Generic;

public enum AvailabilityStatus
{
    Available,
    CheckedOut
}

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public string ISBN { get; set; }
    public AvailabilityStatus AvailabilityStatus { get; set; }

    public Book(string title, string author, string genre, string isbn)
    {
        Title = title;
        Author = author;
        Genre = genre;
        ISBN = isbn;
        AvailabilityStatus = AvailabilityStatus.Available;
    }

    public void ChangeAvailabilityStatus(AvailabilityStatus status)
    {
        AvailabilityStatus = status;
    }

    public void GetBookInfo()
    {
        Console.WriteLine($"Title: {Title}, Author: {Author}, Genre: {Genre}, ISBN: {ISBN}, Status: {AvailabilityStatus}");
    }
}

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public virtual void Register() { }
    public virtual void Login() { }
}

public class Reader : User
{
    public Reader(string id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public void BorrowBook(Book book)
    {
        if (book.AvailabilityStatus == AvailabilityStatus.Available)
        {
            book.ChangeAvailabilityStatus(AvailabilityStatus.CheckedOut);
            Console.WriteLine($"{Name} borrowed the book: {book.Title}");
        }
        else
        {
            Console.WriteLine($"Book {book.Title} is not available.");
        }
    }

    public void ReturnBook(Book book)
    {
        book.ChangeAvailabilityStatus(AvailabilityStatus.Available);
        Console.WriteLine($"{Name} returned the book: {book.Title}");
    }
}

public class Librarian : User
{
    public Librarian(string id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public void IssueBook(Book book, Reader reader)
    {
        reader.BorrowBook(book);
    }

    public void ReturnBook(Book book, Reader reader)
    {
        reader.ReturnBook(book);
    }
}

public class Loan
{
    public Book Book { get; set; }
    public Reader Reader { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public Loan(Book book, Reader reader)
    {
        Book = book;
        Reader = reader;
        LoanDate = DateTime.Now;
    }

    public void IssueLoan()
    {
        Console.WriteLine($"Loan issued for book: {Book.Title} to reader: {Reader.Name}");
    }

    public void ReturnLoan()
    {
        ReturnDate = DateTime.Now;
        Console.WriteLine($"Loan returned for book: {Book.Title} by reader: {Reader.Name}");
    }
}

public class Library
{
    public List<Book> Books { get; set; }
    public List<User> Users { get; set; }
    public List<Loan> Loans { get; set; }

    public Library()
    {
        Books = new List<Book>();
        Users = new List<User>();
        Loans = new List<Loan>();
    }

    public void AddBook(Book book)
    {
        Books.Add(book);
        Console.WriteLine($"Book {book.Title} added to library.");
    }

    public void RemoveBook(Book book)
    {
        Books.Remove(book);
        Console.WriteLine($"Book {book.Title} removed from library.");
    }

    public Book SearchBook(string query)
    {
        foreach (var book in Books)
        {
            if (book.Title.Contains(query) || book.Author.Contains(query))
            {
                return book;
            }
        }
        return null;
    }

    public void GenerateReport()
    {
        Console.WriteLine("Generating library report...");
        // Реализовать генерацию отчетов
    }
}
public interface IBookOperations
{
    void ChangeAvailabilityStatus(AvailabilityStatus status);
    void GetBookInfo();
}

public interface IUserOperations
{
    void Register();
    void Login();
}

public interface ILoanOperations
{
    void IssueLoan();
    void ReturnLoan();
}

public interface ILibraryOperations
{
    void AddBook(Book book);
    void RemoveBook(Book book);
    Book SearchBook(string query);
    void GenerateReport();
}

class Program
{
    static void Main(string[] args)
    {
        // Создаем библиотеку
        Library library = new Library();

        // Добавляем книги в библиотеку
        Book book1 = new Book("C# Programming", "John Doe", "Programming", "1234567890");
        Book book2 = new Book("Learn Design Patterns", "Jane Smith", "Programming", "0987654321");
        library.AddBook(book1);
        library.AddBook(book2);

        // Создаем пользователей
        Reader reader = new Reader("1", "Alice", "alice@example.com");
        Librarian librarian = new Librarian("2", "Bob", "bob@example.com");

        // Выдача книги
        librarian.IssueBook(book1, reader);
        reader.ReturnBook(book1);

        // Поиск книги
        var foundBook = library.SearchBook("Design Patterns");
        if (foundBook != null)
        {
            foundBook.GetBookInfo();
        }

        // Генерация отчетов
        library.GenerateReport();

        // Вывод диаграммы компонентов
        PrintComponentDiagram();

        static void PrintComponentDiagram()
        {
            Console.WriteLine(@"
+-------------------+             +-------------------+
|      Library      |<----------->|       User        |
|-------------------|             |-------------------|
| + AddBook()       |             | + Register()      |
| + RemoveBook()    |             | + Login()         |
| + SearchBook()    |             |-------------------|
| + GenerateReport()|             | [Reader]          |
|                   |             |  + BorrowBook()   |
|                   |             |  + ReturnBook()   |
|                   |             | [Librarian]       |
|                   |             |  + IssueBook()    |
|                   |             |  + ReturnBook()   |
+-------------------+             +-------------------+

           |                             ^
           |                             |
           v                             |
+-------------------+                   [Loan]
|      Catalog      |<------------------+----------------------+
|-------------------|                   |  + IssueLoan()       |
| + SearchBooks()   |                   |  + ReturnLoan()      |
| + FilterBooks()   |                   |  + ManageTransactions|
+-------------------+                   +----------------------+
           |
           |
           v
+-------------------+   
|       Book        |
|-------------------|
| + ChangeStatus()  |
| + GetInfo()       |
+-------------------+

+-------------------+   
|      Report       |<----------------------------------------+
|-------------------|                                         |
| + GenerateReport()|                                         |
| + DisplayStats()  |                                         |
+-------------------+                                         |
                                                              |
                                                              v
                                                      +-------------------+
                                                      |  AvailabilityStatus|
                                                      |-------------------|
                                                      | {Available/CheckedOut}|
                                                      +-------------------+
");

        }
    }
}
