using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Globalization;
using Library_App.Classes;
using LibraryClasses;
namespace Library_App
{
    /// <summary>
    /// Логика взаимодействия для Library.xaml
    /// </summary>
    public partial class Library : Window
    {
        
        
        DB_Finder find = new DB_Finder();
        DB_Worker create = new DB_Worker();
      
        private List<Utility_Book> TranslateBooks(List<Utility_Book> receiver, List<Books> giver) //Перевод списка книг по типу БД в лист книг программного класса
        {

            foreach (Books book in giver.Distinct().ToList())
            {
                Utility_Book copy_book = new Utility_Book();
                copy_book.ID = book.ID;
                copy_book.Name = book.Name;
                copy_book.Department = book.Department;
                copy_book.Publisher = book.Publisher;
                copy_book.YearOfPublishing = book.YearOfPublishing.Value.ToShortDateString();
                copy_book.Number = book.Number;
                copy_book.Archive = book.Archive;

                copy_book.FindAuthors();
                copy_book.FillAuthorNames();

                receiver.Add(copy_book);
            }

            return receiver;
        }

       
        public void Update(ListBox list, IEnumerable<dynamic> items) //Обновление ItemsSource ListBox'а
        {
            list.ItemsSource = items;
        }



        

        public Library()
        {
            InitializeComponent();



            List<Utility_Book> books = new List<Utility_Book>();  
            
            TranslateBooks(books,find.FindAllBooks());
       
            var orders = find.FindAllOrders();

            var readers = find.FindAllReaders();

            Update(BooksList, books);

           
            Update(OrdersList, orders);

            Update(ReadersList, readers);
        }

        private void Button_FindBooks(object sender, RoutedEventArgs e)
        {



            bool IsSearchBarEmpty = true;



            if (BookName.Text != "" ||

                AuthorSurname.Text != "" ||

                AuthorName.Text != "" ||

                Publisher.Text != "" ||

                Year.Text != "" ||

                Archive.Text != "" ||

                Department.Text != "")

                IsSearchBarEmpty = false;


            List<Utility_Book> foundBooks = new List<Utility_Book>();
            

            if (!IsSearchBarEmpty)
            {
                TranslateBooks(foundBooks, find.FindSpecBooks(BookName.Text, AuthorSurname.Text, AuthorName.Text, Publisher.Text, Year.Text, Archive.Text, Department.Text));
              
            }
            else
            {
                TranslateBooks(foundBooks, find.FindAllBooks());
            }

            Update(BooksList, foundBooks);
        }

        private void Button_MakeOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                create.CreateOrder(Convert.ToInt32(BookIDForOrder.Text), Convert.ToInt32(ReaderID.Text), DateTime.Today, OrderReturnDate.SelectedDate.Value);
                var upd_orders = find.FindAllOrders();
                Update(OrdersList, upd_orders);
            }
            catch
            {
                MessageBox.Show("Проверьте правильность введенных данных", "Ошибка");
            }
        }

        private void Button_FindOrders(object sender, RoutedEventArgs e)
        {

            
               var foundOrders = find.FindSpecOrders(OrderDate.Text, OrderReturnDateFind.Text, Returned.IsChecked.Value.ToString(), ActualReturnDate.Text, ReaderIDFind.Text, BookID.Text, BookNameOrderFind.Text, ReaderName.Text, ReaderPhone.Text);
            
            
            Update(OrdersList, foundOrders);


        }

        private void Button_MakeReader(object sender, RoutedEventArgs e)
        {
           
            create.CreateReader(CreateReaderSurname.Text , CreateReaderName.Text, CreateReaderPatronym.Text, CreateReaderAdress.Text, CreateReaderPhone.Text);
            var upd_readers = find.FindAllReaders();

            Update(ReadersList, upd_readers);
        }

        private void Button_FindReaders(object sender, RoutedEventArgs e)
        {
            bool IsSearchBarEmpty = true;



            if (FindReaderID.Text != "" ||

                FindReaderSurname.Text != "" ||

                FindReaderName.Text != "" ||

                FindReaderPatronym.Text != "" ||

                FindReaderAdress.Text != "" ||

                FindReaderPhone.Text != "" )

                IsSearchBarEmpty = false;


            var foundReaders = find.FindAllReaders();

            if (!IsSearchBarEmpty)
            { foundReaders = find.FindSpecReaders(FindReaderID.Text, FindReaderSurname.Text, FindReaderName.Text, FindReaderPatronym.Text, FindReaderAdress.Text, FindReaderPhone.Text); }

            Update(ReadersList, foundReaders);
        }
    }
}
