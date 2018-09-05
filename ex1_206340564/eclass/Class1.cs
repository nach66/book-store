using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eclass
{
     public class Author
     {
        public string fname, lname;
        public int books;

        public Author(string fname, string lname, int b)
        {
            Fname = fname;
            Lname = lname;
            Books = b;
        }

        public Author(Author b)
        {
            Fname = b.Fname;
            Lname = b.Lname;
            Books = b.Books;
        }

        public string Fname
        {
            get
            {
                return fname;
            }
            set
            {
                fname = value;
            }
        }

        public string Lname
        {
            get
            {
                return lname;
            }
            set
            {
                lname = value;
            }
        }

        public int Books
        {
            get
            {
                return books;
            }
            set
            {
                books = value;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Author d = obj as Author;
            if (d != null)
            {
                return d.Fname == Fname &&
                    d.Lname == Lname;
            }
            return false;
        }

        public Author Clone ()
        {
            return this;
        }

     }   

     public class Book : Author, IComparable<Book>
     {
            public string isbn;
            public string bookName;
            public decimal Price { get; set; }
            public int Copys { get; set; }

        public Book(Author a, string i, string b, decimal p, int c) :base (a)
            {
                Isbn = i;
                BookName = b;
                Price = p;
                Copys = c;
            }

            public Author geta()
            {
                return base.Clone();
            }
            public string getf()
            {
                return base.Fname;
            }

            public string getl()
            {
                return base.Lname;
            }
            public int getb()
            {
                return base.Books;
            }
            public void setb(int b)
            {
            base.Books = b;
            }

            public string Isbn
            {
                    get
                    {
                        return isbn;
                    }
                    set
                    {
                        isbn = value;
                    }
            }
          
            public string BookName
            {
                get
                {
                    return bookName;
                }
                set
                {
                    bookName = value;
                }
            }          

        public override string ToString()
        {
            return BookName + " " + Isbn;
        }

        public override bool Equals(object obj)
            {
                Book other = obj as Book;
                if (other != null)
                {
                return this.BookName.Equals(other.BookName) && this.Isbn.Equals(other.Isbn);

//                return Isbn == st.Isbn;
                }
                return false;
            }        

        public int CompareTo(Book other)
        {
            if (this.BookName == other.BookName)
                return 0;
            return BookName.CompareTo(other.BookName);
        }
    }
}