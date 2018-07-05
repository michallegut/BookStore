using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace GraphicalClient
{
    public partial class MainWindow : Window
    {
        private string format = "xml";

        public MainWindow()
        {
            InitializeComponent();
            Json.Checked += Json_Checked;
            Xml.Checked += Xml_Checked;
            TabControl.SelectionChanged += TabControl_SelectionChanged;
            GetByIdButton.Click += GetByIdButton_Click;
            GetByTitleButton.Click += GetByTitleButton_Click;
            AddButton.Click += AddButton_Click;
            SellButton.Click += SellButton_Click;
            ChangePriceButton.Click += ChangePriceButton_Click;
        }

        private void ChangePriceButton_Click(object sender, RoutedEventArgs e)
        {
            Status.Content = "";
            string result = changeprice(ChangePriceId.Text, ChangePricePrice.Text);
            if (result.Length > 0)
            {
                if (!result.StartsWith("Error"))
                {
                    Status.Foreground = Brushes.Green;
                }
                else
                {
                    Status.Foreground = Brushes.Red;
                }
                Status.Content = result;
            }
        }

        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            Status.Content = "";
            string result = sell(SellId.Text);
            if (result.Length > 0)
            {
                if (!result.StartsWith("Error"))
                {
                    Status.Foreground = Brushes.Green;
                }
                else
                {
                    Status.Foreground = Brushes.Red;
                }
                Status.Content = result;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Status.Content = "";
            string result = add(AddId.Text, AddTitle.Text, AddPrice.Text);
            if (result.Length > 0)
            {
                if (!result.StartsWith("Error"))
                {
                    Status.Foreground = Brushes.Green;
                }
                else
                {
                    Status.Foreground = Brushes.Red;
                }
                Status.Content = result;
            }
        }

        private void GetByTitleButton_Click(object sender, RoutedEventArgs e)
        {
            Status.Content = "";
            GetByTitleTextBlock.Text = getByTitle(GetByTitleTitle.Text);
        }

        private void GetByIdButton_Click(object sender, RoutedEventArgs e)
        {
            Status.Content = "";
            GetByIdTextBlock.Text = getById(GetByIdId.Text);
        }

        private void Xml_Checked(object sender, RoutedEventArgs e)
        {
            Json.IsChecked = false;
            format = "xml";
        }

        private void Json_Checked(object sender, RoutedEventArgs e)
        {
            Xml.IsChecked = false;
            format = "json";
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Status.Content = "";
            if (GetAll.IsSelected)
            {
                GetAllTextBlock.Text = getAll();
            }
            if (GetById.IsSelected)
            {
                GetByIdTextBlock.Text = "";
            }
            if (GetByTitle.IsSelected)
            {
                GetByTitleTextBlock.Text = "";
            }
        }

        private string getAll()
        {
            string responseString = sendRequest("/books", "GET", null);
            if (responseString != null)
            {
                if (format.Equals("xml"))
                {
                    return parseXml(responseString);
                }
                else
                {
                    return parseJson(responseString);
                }
            }
            else
            {
                return "";
            }
        }

        private string getById(string id)
        {
            string responseString = sendRequest("/books/id/" + id, "GET", null);
            if (responseString != null)
            {
                if (responseString.Length > 0)
                {
                    if (format.Equals("xml"))
                    {
                        return parseXml(responseString);
                    }
                    else
                    {
                        return parseJson(responseString);
                    }
                }
                else
                {
                    Status.Foreground = Brushes.Red;
                    Status.Content = "Error: Invalid ID.";
                    return "";
                }
            }
            else return "";
        }

        private string getByTitle(string title)
        {
            string responseString = sendRequest("/books/title/" + title, "GET", null);
            if (responseString != null)
            {
                if (responseString.Length > 0)
                {
                    if (format.Equals("xml"))
                    {
                        return parseXml(responseString);
                    }
                    else
                    {
                        return parseJson(responseString);
                    }
                }
                else
                {
                    Status.Foreground = Brushes.Red;
                    Status.Content = "Error: Invalid title.";
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        private string add(string id, string title, string price)
        {
            if (format.Equals("xml"))
            {
                string responseString = sendRequest("/books", "POST", "<Book xmlns=\"http://schemas.datacontract.org/2004/07/BookStore\"><Id>" + id + "</Id><Price>" + (price.IndexOf('.') != -1 ? price.Substring(0, price.IndexOf('.') + 3) : price) + "</Price><Title>" + title + "</Title></Book>");
                if (responseString != null)
                {
                    return parseXml(responseString);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                string responseString = sendRequest("/books", "POST", "{\"Id\":" + id + ",\"Price\":" + (price.IndexOf('.') != -1 ? price.Substring(0, price.IndexOf('.') + 3) : price) + ",\"Title\":\"" + title + "\"}");
                if (responseString != null)
                {
                    return parseJson(responseString);
                }
                else
                {
                    return "";
                }
            }
        }

        private string sell(string id)
        {
            string responseString = sendRequest("/books/" + id, "DELETE", null);
            if (responseString != null)
            {
                if (responseString.Length > 0)
                {
                    if (format.Equals("xml"))
                    {
                        return parseXml(responseString);
                    }
                    else
                    {
                        return parseJson(responseString);
                    }
                }
                else
                {
                    Status.Foreground = Brushes.Red;
                    Status.Content = "Error: Invalid ID.";
                    return "";
                }
            }
            else return "";
        }

        private string changeprice(string id, string price)
        {
            if (format.Equals("xml"))
            {
                string responseString = sendRequest("/books", "PUT", "<Book xmlns=\"http://schemas.datacontract.org/2004/07/BookStore\"><Id>" + id + "</Id><Price>" + (price.IndexOf('.') != -1 ? price.Substring(0, price.IndexOf('.') + 3) : price) + "</Price><Title></Title></Book>");
                if (responseString != null)
                {
                    return parseXml(responseString);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                string responseString = sendRequest("/books", "PUT", "{\"Id\":" + id + ",\"Price\":" + (price.IndexOf('.') != -1 ? price.Substring(0, price.IndexOf('.') + 3) : price) + ",\"Title\":\"\"}");
                if (responseString != null)
                {
                    return parseJson(responseString);
                }
                else
                {
                    return "";
                }
            }
        }

        private string sendRequest(string endpoint, string method, string data)
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
                Status.Foreground = Brushes.Red;
                Status.Content = e.Message.ToString();
                return null;
            }
        }

        private string parseXml(string xml)
        {
            StringBuilder result = new StringBuilder();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            if (xmlDoc.FirstChild.Name.Equals("string"))
            {
                result.AppendLine(xmlDoc.FirstChild.InnerText);
            }
            else
            {
                XmlNodeList books = xmlDoc.GetElementsByTagName("Book");
                if (books.Count > 0)
                {
                    result.AppendLine("Result:");
                    result.AppendLine();
                    foreach (XmlNode book in books)
                    {
                        XmlNodeList attributes = book.ChildNodes;
                        result.AppendLine("Id: " + attributes.Item(0).InnerText);
                        result.AppendLine("Title: " + attributes.Item(2).InnerText);
                        result.AppendLine("Price: " + attributes.Item(1).InnerText + "$");
                        result.AppendLine();
                    }
                }
                else
                {
                    result.AppendLine("No results.");
                }
            }
            return result.ToString();
        }

        private string parseJson(string responseString)
        {
            StringBuilder result = new StringBuilder();
            dynamic books = JsonConvert.DeserializeObject(responseString);
            if (books.GetType().Name.Equals("JArray") && books.Count > 0)
            {
                result.AppendLine("Result:");
                result.AppendLine();
                foreach (dynamic book in books)
                {
                    result.AppendLine("Id: " + book["Id"].Value);
                    result.AppendLine("Title: " + book["Title"].Value);
                    result.AppendLine("Price: " + book["Price"].Value + "$");
                    result.AppendLine();
                }
            }
            else if (books.GetType().Name.Equals("JObject"))
            {
                result.AppendLine("Result:");
                result.AppendLine();
                result.AppendLine("Id: " + books["Id"].Value);
                result.AppendLine("Title: " + books["Title"].Value);
                result.AppendLine("Price: " + books["Price"].Value + "$");
                result.AppendLine();
            }
            else if (books.GetType().Name.Equals("String"))
            {
                result.AppendLine(responseString.Substring(1, responseString.Length - 2));
            }
            else
            {
                result.AppendLine("No results.");
            }
            return result.ToString();
        }
    }
}
