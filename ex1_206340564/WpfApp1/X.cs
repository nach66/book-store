using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using eclass;

namespace tar1
{
    class Xslm
    {
        public string Filename { get; set; }
        public Xslm(string file)
        {
            Filename = file;
        }

        public ObservableCollection<Book> Read()
        {
            ObservableCollection<Book> books = new ObservableCollection<Book>();
            XmlTextReader reader = new XmlTextReader(Filename);
            reader.WhitespaceHandling = WhitespaceHandling.None;

            while (reader.Name != "BOOK")
            {
                reader.Read();
            }

            while (reader.Name == "BOOK")
            {
                reader.ReadStartElement("BOOK");

                string firName = reader.ReadElementString("AutorfName");
                string lasName = reader.ReadElementString("AutorlName");
                int booknum = Convert.ToInt32(reader.ReadElementString("Autor_number_of_books"));
                string isb = reader.ReadElementString("isbn");
                string boName = reader.ReadElementString("bookName");
                decimal pric = Convert.ToDecimal(reader.ReadElementString("Price"));
                int copy = Convert.ToInt32(reader.ReadElementString("Copys"));


                Author a = new Author(firName, lasName, booknum);
                Book b = new Book(a, isb, boName, pric, copy);
                books.Add(b);

                reader.ReadEndElement();
            }
            reader.Close();
            return books;
        }

        public void Write(ObservableCollection<Book> mybooks)
        {
            XmlTextWriter writer = new XmlTextWriter(Filename, Encoding.Unicode);
            writer.Formatting = Formatting.Indented;

            // start the document
            writer.WriteStartDocument();
            writer.WriteStartElement("books");

            // write the data
            foreach (Book item in mybooks)
            {
                writer.WriteStartElement("BOOK");
                writer.WriteElementString("AutorfName",
                   item.getf());
                writer.WriteElementString("AutorlName",
                   item.getl());
                writer.WriteElementString("Autor_number_of_books",
                   item.getb().ToString());
                writer.WriteElementString("isbn",
                   item.Isbn);
                writer.WriteElementString("bookName",
                   item.BookName);
                writer.WriteElementString("Price",
                   item.Price.ToString());
                writer.WriteElementString("Copys",
                   item.Copys.ToString());
                writer.WriteEndElement();
            }

            // end the document
            writer.WriteEndDocument();
            writer.Close();
        }
    }
}