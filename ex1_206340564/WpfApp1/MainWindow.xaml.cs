using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using eclass;

namespace tar1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
            listBoxbooks.ItemsSource = mybooks;
            r.Document = myFlowDoc;
        }

        string path = @"..\..\books.xml";
        FlowDocument myFlowDoc = new FlowDocument();
        RichTextBox rich = new RichTextBox();
        ObservableCollection<Book> mybooks = new ObservableCollection<Book>();
        ObservableCollection<Author> myaus = new ObservableCollection<Author>();

        public void ButtonAdd_Click(object s, RoutedEventArgs e) //Add a book to the list
        {
            if (!AllTextBoxesFilled())
            {
                MessageBox.Show("You must fill all the data",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int m = 0;
            Book newb;
            try
            {
                Author mark = new Author(finame.Text, laname.Text, int.Parse(co.Text));
                if (!this.myaus.Contains(mark)) //if the autor not in the list, add
                {
                    this.myaus.Add(mark);
                    newb = new Book(mark, iss.Text, bo.Text, decimal.Parse(pr.Text), int.Parse(co.Text));
                }
                else //else add a book to his copys
                {
                    for (int i = 0; i < mybooks.Count; i++)
                    {
                        if (mybooks[i].getf() == mark.Fname && mybooks[i].getl() == mark.Lname)
                        {
                            int was = mybooks[i].getb();                        
                            mybooks[i].setb(was+mark.Books);
                            m = mybooks[i].getb();
                        }
                    }
                    mark.Books = m;
                    newb = new Book(mark, iss.Text, bo.Text, decimal.Parse(pr.Text), int.Parse(co.Text));
                }

                if (!this.mybooks.Contains(newb))
                {
                    mybooks.Add(newb);
                    mybooks = new ObservableCollection<Book>(from i in mybooks orderby i.BookName select i);
                    listBoxbooks.ItemsSource = mybooks;
                }
                else
                {
                    MessageBox.Show("book alraeddy ecxist",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Run myRun = new Run("added: " + newb.BookName);
                Paragraph myParagraph = new Paragraph();
                myParagraph.Inlines.Add(myRun);
                myParagraph.Foreground = Brushes.Green;
                myFlowDoc.Blocks.Add(myParagraph);
            }
            catch (FormatException)
            {
                MessageBox.Show("מחרוזת קלט לא היתה בתבנית הנכונה",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("null pointer");
            }
        }

        public bool AllTextBoxesFilled() //make sure that all the Text Boxes Filled
        {
            if (string.IsNullOrEmpty(iss.Text) ||
                    string.IsNullOrEmpty(bo.Text) ||
                    string.IsNullOrEmpty(co.Text) ||
                    string.IsNullOrEmpty(pr.Text) ||
                    string.IsNullOrEmpty(finame.Text) ||
                    string.IsNullOrEmpty(laname.Text))
            {
                return false;
            }
            
            return true;
        }

        public bool IsbnTextBoxesFilled()
        {
            if (string.IsNullOrEmpty(iss.Text))
            {
                return false;
            }           
            return true;
        }

        private void ListBoxbooks_SelectionChanged(object sender, SelectionChangedEventArgs e) 
            //defines data showing abaut the selected book
        {
            if (null != listBoxbooks.SelectedItem)
            {
                Book im = (Book)listBoxbooks.SelectedItem;

                D_is.Text = im.Isbn;
                D_bname.Text = im.BookName;
                D_aname.Text = im.getf() +" "+ im.getl();
                D_aubook.Text = im.getb().ToString();
                D_price.Text = im.Price.ToString();
                D_copy.Text = im.Copys.ToString();
            }
        }

        private void Buttona_Click(object sender, RoutedEventArgs e) //defines what happen when amount update selected
        {
            if (!IsbnTextBoxesFilled())
            {
                MessageBox.Show("You must fill all the isbn and number of copys",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Boolean exists = false;
            for (int j = 0; j < mybooks.Count; j++)
            {
                if (mybooks[j].Isbn == iss.Text)
                {
                    exists = true;
                }
            }
            if (!exists)
            {
                MessageBox.Show("book not exists!",
                   "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int was = 0;
            int Missormore = 0;
            for (int i = 0; i < mybooks.Count; i++)
            {
                if (mybooks[i].Isbn == iss.Text)
                {
                    was = mybooks[i].Copys;
                    Missormore = int.Parse(co.Text) - was;

                    mybooks[i].Copys = int.Parse(co.Text);

                    Run myRun = new Run("change the amount of "+mybooks[i].BookName+
                        " from " +was+" to "+ int.Parse(co.Text));
                    Paragraph myParagraph = new Paragraph();
                    myParagraph.Inlines.Add(myRun);
                    myParagraph.Foreground = Brushes.Blue;
                    myFlowDoc.Blocks.Add(myParagraph);
                }

                for (int j = 0; j < mybooks.Count; j++)
                {
                    if (mybooks[j].getf() == mybooks[i].getf() && mybooks[j].getl() == mybooks[i].getl())
                    {
                        was = mybooks[j].getb();
                        mybooks[j].setb(was+Missormore);
                    }
                }
            }
        }

        private void Buttonp_Click(object sender, RoutedEventArgs e) //defines what happen when price update selected
        {
            if(!IsbnTextBoxesFilled())
            {
                MessageBox.Show("You must fill all the isbn and the new price",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Boolean exists = false;
            for (int j = 0; j < mybooks.Count; j++)
            {
                if (mybooks[j].Isbn == iss.Text)
                {
                    exists = true;
                }
            }
            if (!exists)
            {
                MessageBox.Show("book not exists!",
                   "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            for (int i = 0; i < mybooks.Count; i++)
            {
                if (mybooks[i].Isbn == iss.Text)
                {
                    decimal was = mybooks[i].Price;
                    if ((decimal.Parse(pr.Text)) > was)
                    {                    
                        MessageBox.Show("You can only low the price!",
                         "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        mybooks[i].Price = decimal.Parse(pr.Text);

                        Run myRun = new Run("change the price of " +
                            mybooks[i].BookName + " from " + was + " to " + decimal.Parse(pr.Text));
                        Paragraph myParagraph = new Paragraph();
                        myParagraph.Inlines.Add(myRun);
                        myParagraph.Foreground = Brushes.Blue;
                        myFlowDoc.Blocks.Add(myParagraph);
                    }
                }                
            }
        }

        private void Buttond_Click(object sender, RoutedEventArgs e) //delete a book from the list
        {
            if (!IsbnTextBoxesFilled())
            {
                MessageBox.Show("You must fill all the isbn",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int ind = 0;
            Boolean exists = false;
            for (int j = 0; j < mybooks.Count; j++)
            {
                if (mybooks[j].Isbn == iss.Text)
                {
                    ind = j;
                    exists = true;
                }
            }
            if (!exists)
            {
                MessageBox.Show("book not exists!",
                   "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete?",
                "delete", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                string n = mybooks[ind].BookName;
                mybooks.Remove(mybooks[ind]);

                Run myRun = new Run("Deleted " + n);
                Paragraph myParagraph = new Paragraph();
                myParagraph.Inlines.Add(myRun);
                myParagraph.Foreground = Brushes.Red;
                myFlowDoc.Blocks.Add(myParagraph);
            }
        }
        
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        //When you close the program, the details are saved to the XML file
        {
            if (mybooks.Count > 0)
            {
                Xslm xmlConnection = new Xslm(path);
                xmlConnection.Write(mybooks);
            }
            else
            {
                File.Delete(path);
            }           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) 
            //At the beginning of the program the details will be read from the XML file
        {
            if (File.Exists(path))
            {
                Xslm xmlConnection = new Xslm(path);
                mybooks = xmlConnection.Read();
                listBoxbooks.ItemsSource = mybooks;            
            }
        }
    }
}