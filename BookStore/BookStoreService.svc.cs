using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace BookStore
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class BookStoreService : IBookStoreService
    {
        private static List<Book> books;

        public BookStoreService()
        {
            books = new List<Book>() {
                new Book {Id = 1, Title = "Python Crash Course: A Hands-On, Project-Based Introduction to Programming", Price = 22.60},
                new Book {Id = 2, Title = "The Self-Taught Programmer: The Definitive Guide to Programming Professionally", Price = 15.08},
                new Book {Id = 3, Title = "Automate the Boring Stuff with Python: Practical Programming for Total Beginners", Price = 19.16}
            };
        }

        //XML
        public List<Book> getAllXml()
        {
            return books;
        }

        public Book getByIdXml(string Id)
        {
            return getById(Id);
        }

        public List<Book> getByTitleXml(string Title)
        {
            return getByTitle(Title);
        }

        public string addToInventoryXml(Book element)
        {
            return addToInventory(element);
        }

        public string sellXml(string Id)
        {
            return sell(Id);
        }

        public string changePriceXml(Book element)
        {
            return changePrice(element);
        }

        //JSON
        public List<Book> getAllJson()
        {
            return books;
        }

        public Book getByIdJson(string Id)
        {
            return getById(Id);
        }

        public List<Book> getByTitleJson(string Title)
        {
            return getByTitle(Title);
        }

        public string addToInventoryJson(Book element)
        {
            return addToInventory(element);
        }

        public string sellJson(string Id)
        {
            return sell(Id);
        }

        public string changePriceJson(Book element)
        {
            return changePrice(element);
        }

        //Private
        private Book getById(string Id)
        {
            int intId;
            try
            {
                intId = int.Parse(Id);
            }
            catch (Exception e)
            {
                return null;
            }
            return getById(intId);
        }

        private Book getById(int Id)
        {
            return books.Find(b => b.Id == Id);
        }

        private List<Book> getByTitle(string Title)
        {
            return books.FindAll(b => b.Title.ToLower().Contains(Title.ToLower()));
        }

        private string addToInventory(Book element)
        {
            if (getById(element.Id) == null)
            {
                if (element.Price >= 0)
                {
                    books.Add(element);
                    return "Book has been added successfuly.";
                }
                else
                {
                    return "Error: Invalid price.";
                }
            }
            else
            {
                return "Error: Invalid ID.";
            }
        }

        private string sell(string Id)
        {
            Book book = getById(Id);
            if (book != null)
            {
                books.Remove(book);
                return "Book has been sold successfuly.";
            }
            else
            {
                return "Error: Invalid ID.";
            }
        }

        private string changePrice(Book element)
        {
            Book book = getById(element.Id);
            if (book != null)
            {
                if (element.Price >= 0)
                {
                    book.Price = element.Price;
                    return "Price has been changed successfuly.";
                }
                else
                {
                    return "Error: Invalid price.";
                }
            }
            else
            {
                return "Error: Invalid ID.";
            }
        }
    }
}