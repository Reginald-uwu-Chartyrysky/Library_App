using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryClasses;
namespace Library_App.Classes
{
    class Utility_Book
    {

        Cooler_LibraryDBEntities db = new Cooler_LibraryDBEntities();
        public int ID { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Publisher { get; set; }


        public string YearOfPublishing { get; set; }
        public Nullable<int> Number { get; set; }
        public string Archive { get; set; }

        public string Author { get; set; }

        public List<Authors> authors { get; set; }

        
        public void FindAuthors()
        {
           
            authors = new List<Authors>();
            authors.AddRange ((
                 from auth in db.Authors
                 join conn in db.AuthorsBooks on auth.ID equals conn.AuthorID
                 where conn.BookID.Value.Equals(ID)


                 select auth
               

                      ).ToList());
                
        }

        public void FillAuthorNames()
        {
            Author = "";
            foreach (Authors auth in authors)
            {
                if (Author != "")
                { Author += ", "; }
                Author += auth.Surname + " " + auth.Name;
            }
        }

    }
}
