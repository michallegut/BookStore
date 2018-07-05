using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace TextBasedClient
{
    class Program
    {
        private static string format = "xml";

        static void Main(string[] args)
        {
            printCommands();
            Console.WriteLine();
            string command = null;
            Console.Write("->");
            while (!(command = Console.ReadLine()).Equals("exit"))
            {
                Console.WriteLine();
                string[] commandParts = command.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (commandParts.Length > 0)
                {
                    switch (commandParts[0])
                    {
                        case "exit":
                            break;
                        case "help":
                            printCommands();
                            break;
                        case "format":
                            handleFormat(commandParts);
                            break;
                        case "getall":
                            getAll();
                            break;
                        case "getbyid":
                            getById(commandParts);
                            break;
                        case "getbytitle":
                            getByTitle(commandParts);
                            break;
                        case "add":
                            add(commandParts);
                            break;
                        case "sell":
                            sell(commandParts);
                            break;
                        case "changeprice":
                            changeprice(commandParts);
                            break;
                        default:
                            Console.WriteLine("Error: Invalid command.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Error: Invalid command.");
                }
                Console.WriteLine();
                Console.Write("->");
            }
        }

        private static void changeprice(string[] commandParts)
        {
            if (commandParts.Length >= 3)
            {
                if (format.Equals("xml"))
                {
                    string responseString = sendRequest("/books", "PUT", "<Book xmlns=\"http://schemas.datacontract.org/2004/07/BookStore\"><Id>" + commandParts[1] + "</Id><Price>" + commandParts[2].Substring(0, commandParts[2].IndexOf('.') + 3) + "</Price><Title></Title></Book>");
                    if (responseString != null)
                    {
                        parseXml(responseString);
                    }
                }
                else
                {
                    string responseString = sendRequest("/books", "PUT", "{\"Id\":" + commandParts[1] + ",\"Price\":" + commandParts[2].Substring(0, commandParts[2].IndexOf('.') + 3) + ",\"Title\":\"\"}");
                    if (responseString != null)
                    {
                        parseJson(responseString);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Invalid syntax.");
            }
        }

        private static void sell(string[] commandParts)
        {
            if (commandParts.Length >= 2)
            {
                string responseString = sendRequest("/books/" + commandParts[1], "DELETE", null);
                if (responseString != null)
                {
                    if (responseString.Length > 0)
                    {
                        if (format.Equals("xml"))
                        {
                            parseXml(responseString);
                        }
                        else
                        {
                            parseJson(responseString);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid ID.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Invalid syntax.");
            }
        }

        private static void add(string[] commandParts)
        {
            if (commandParts.Length >= 4)
            {
                if (format.Equals("xml"))
                {
                    string responseString = sendRequest("/books", "POST", "<Book xmlns=\"http://schemas.datacontract.org/2004/07/BookStore\"><Id>" + commandParts[1] + "</Id><Price>" + commandParts[3].Substring(0, commandParts[3].IndexOf('.') + 3) + "</Price><Title>" + commandParts[2] + "</Title></Book>");
                    if (responseString != null)
                    {
                        parseXml(responseString);
                    }
                }
                else
                {
                    string responseString = sendRequest("/books", "POST", "{\"Id\":" + commandParts[1] + ",\"Price\":" + commandParts[3].Substring(0, commandParts[3].IndexOf('.') + 3) + ",\"Title\":\"" + commandParts[2] + "\"}");
                    if (responseString != null)
                    {
                        parseJson(responseString);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Invalid syntax.");
            }
        }

        private static void getByTitle(string[] commandParts)
        {
            if (commandParts.Length >= 2)
            {
                string responseString = sendRequest("/books/title/" + commandParts[1], "GET", null);
                if (responseString != null)
                {
                    if (responseString.Length > 0)
                    {
                        if (format.Equals("xml"))
                        {
                            parseXml(responseString);
                        }
                        else
                        {
                            parseJson(responseString);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid title.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Invalid syntax.");
            }
        }

        private static void getAll()
        {
            string responseString = sendRequest("/books", "GET", null);
            if (responseString != null)
            {
                if (format.Equals("xml"))
                {
                    parseXml(responseString);
                }
                else
                {
                    parseJson(responseString);
                }
            }
        }

        private static void parseJson(string responseString)
        {
            dynamic books = JsonConvert.DeserializeObject(responseString);
            if (books.GetType().Name.Equals("JArray") && books.Count > 0)
            {
                Console.WriteLine("Result:");
                Console.WriteLine();
                foreach (dynamic book in books)
                {
                    Console.WriteLine("Id: " + book["Id"].Value);
                    Console.WriteLine("Title: " + book["Title"].Value);
                    Console.WriteLine("Price: " + book["Price"].Value + "$");
                    Console.WriteLine();
                }
            }
            else if (books.GetType().Name.Equals("JObject"))
            {
                Console.WriteLine("Result:");
                Console.WriteLine();
                Console.WriteLine("Id: " + books["Id"].Value);
                Console.WriteLine("Title: " + books["Title"].Value);
                Console.WriteLine("Price: " + books["Price"].Value + "$");
                Console.WriteLine();
            }
            else if (books.GetType().Name.Equals("String"))
            {
                Console.WriteLine(responseString.Substring(1, responseString.Length - 2));
            }
            else
            {
                Console.WriteLine("Error: No results.");
            }
        }

        private static void parseXml(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            if (xmlDoc.FirstChild.Name.Equals("string"))
            {
                Console.WriteLine(xmlDoc.FirstChild.InnerText);
            }
            else
            {
                XmlNodeList books = xmlDoc.GetElementsByTagName("Book");
                if (books.Count > 0)
                {
                    Console.WriteLine("Result:");
                    Console.WriteLine();
                    foreach (XmlNode book in books)
                    {
                        XmlNodeList attributes = book.ChildNodes;
                        Console.WriteLine("Id: " + attributes.Item(0).InnerText);
                        Console.WriteLine("Title: " + attributes.Item(2).InnerText);
                        Console.WriteLine("Price: " + attributes.Item(1).InnerText + "$");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Error: No results.");
                }
            }
        }

        private static void getById(string[] commandParts)
        {
            if (commandParts.Length >= 2)
            {
                string responseString = sendRequest("/books/id/" + commandParts[1], "GET", null);
                if (responseString != null)
                {
                    if (responseString.Length > 0)
                    {
                        if (format.Equals("xml"))
                        {
                            parseXml(responseString);
                        }
                        else
                        {
                            parseJson(responseString);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid ID.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Invalid syntax.");
            }
        }

        private static string sendRequest(string endpoint, string method, string data)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create("http://localhost:60451/BookStoreService.svc/" + format + "/" + endpoint) as HttpWebRequest;
                request.KeepAlive = false;
                request.Method = method;
                if (format.Equals("xml"))
                {
                    request.ContentType = "text/xml";
                }
                else
                {
                    request.ContentType = "application/json";
                }
                if (data != null)
                {
                    byte[] bufor = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = bufor.Length;
                    Stream postData = request.GetRequestStream();
                    postData.Write(bufor, 0, bufor.Length);
                    postData.Close();
                }
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Encoding encoding = System.Text.Encoding.GetEncoding(1252);
                StreamReader responseStream = new StreamReader(response.GetResponseStream(), encoding);
                string responseString = responseStream.ReadToEnd();
                responseStream.Close();
                response.Close();
                return responseString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return null;
            }
        }

        private static void handleFormat(string[] commandParts)
        {
            if (commandParts.Length >= 2)
            {
                if (commandParts[1].Equals("xml") || commandParts[1].Equals("json"))
                {
                    format = commandParts[1];
                    Console.WriteLine("Format changed successfuly.");
                }
                else
                {
                    Console.WriteLine("Error: Invalid format.");
                }
            }
            else
            {
                Console.WriteLine("Currently used format is " + format.ToUpper() + ".");
            }
        }

        private static void printCommands()
        {
            Console.WriteLine("Avaliable commands:");
            Console.WriteLine();
            Console.WriteLine("exit - close client application");
            Console.WriteLine("help - list of avaliable commands");
            Console.WriteLine("format - display currently used format");
            Console.WriteLine("format {xml, json} - change currently used format");
            Console.WriteLine("getall - list of all books");
            Console.WriteLine("getbyid <id> - book with given id");
            Console.WriteLine("getbytitle <title> - list of books with title containing given string");
            Console.WriteLine("add <id> <title> <price> - add a book to inventory");
            Console.WriteLine("sell <id> - sell a book removing it from inventory");
            Console.WriteLine("changeprice <id> <price> - change price of the book with given id");
        }
    }
}
