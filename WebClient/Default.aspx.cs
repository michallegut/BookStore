using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;

namespace WebClient
{
    public partial class Default : System.Web.UI.Page
    {
        private string format = "xml";

        protected void Page_Load(object sender, EventArgs e)
        {
            Json.CheckedChanged += Json_CheckedChanged;
            Xml.CheckedChanged += Xml_CheckedChanged;
            TabContainer.ActiveTabChanged += TabContainer_ActiveTabChanged;
            GetByIdButton.Click += GetByIdButton_Click;
            GetByTitleButton.Click += GetByTitleButton_Click;
            AddToInventoryButton.Click += AddToInventoryButton_Click;
            SellButton.Click += SellButton_Click;
            ChangePriceButton.Click += ChangePriceButton_Click;
            if (TabContainer.ActiveTab.TabIndex == 0)
            {
                string result = getAll();
                if (result.Length > 0)
                {
                    if (!result.StartsWith("Error"))
                    {
                        GetAllResult.ForeColor = Color.Green;
                    }
                    else
                    {
                        GetAllResult.ForeColor = Color.Red;
                    }
                    GetAllResult.Text = result;
                }
            }
        }

        private void ChangePriceButton_Click(object sender, EventArgs e)
        {
            string result = changeprice(ChangePriceId.Value, ChangePricePrice.Value);
            if (result.Length > 0)
            {
                if (!result.StartsWith("Error"))
                {
                    ChangePriceResult.ForeColor = Color.Green;
                }
                else
                {
                    ChangePriceResult.ForeColor = Color.Red;
                }
                ChangePriceResult.Text = result;
            }
        }

        private void SellButton_Click(object sender, EventArgs e)
        {
            string result = sell(SellId.Value);
            if (result.Length > 0)
            {
                if (!result.StartsWith("Error"))
                {
                    SellResult.ForeColor = Color.Green;
                }
                else
                {
                    SellResult.ForeColor = Color.Red;
                }
                SellResult.Text = result;
            }
        }

        private void AddToInventoryButton_Click(object sender, EventArgs e)
        {
            string result = add(AddToInventoryId.Value, AddToInventoryTitle.Value, AddToInventoryPrice.Value);
            if (result.Length > 0)
            {
                if (!result.StartsWith("Error"))
                {
                    AddToInventoryResult.ForeColor = Color.Green;
                }
                else
                {
                    AddToInventoryResult.ForeColor = Color.Red;
                }
                AddToInventoryResult.Text = result;
            }
        }

        private void GetByTitleButton_Click(object sender, EventArgs e)
        {
            string result = getByTitle(GetByTitleTitle.Value);
            if (result.Length > 0)
            {
                if (!result.StartsWith("Error"))
                {
                    GetByTitleResult.ForeColor = Color.Green;
                }
                else
                {
                    GetByTitleResult.ForeColor = Color.Red;
                }
                GetByTitleResult.Text = result;
            }
        }

        private void GetByIdButton_Click(object sender, EventArgs e)
        {
            string result = getById(GetByIdId.Value);
            if (result.Length > 0)
            {
                if (!result.StartsWith("Error"))
                {
                    GetByIdResult.ForeColor = Color.Green;
                }
                else
                {
                    GetByIdResult.ForeColor = Color.Red;
                }
                GetByIdResult.Text = result;
            }
        }

        private void TabContainer_ActiveTabChanged(object sender, EventArgs e)
        {
            switch (TabContainer.ActiveTab.TabIndex)
            {
                case 0:
                    string result = getAll();
                    if (result.Length > 0)
                    {
                        if (!result.StartsWith("Error"))
                        {
                            GetAllResult.ForeColor = Color.Green;
                        }
                        else
                        {
                            GetAllResult.ForeColor = Color.Red;
                        }
                        GetAllResult.Text = result;
                    }
                    break;
                case 1:
                    GetByIdResult.Text = "";
                    break;
                case 2:
                    GetByTitleResult.Text = "";
                    break;
                default:
                    break;
            }
        }

        private string getAll()
        {
            string responseString = sendRequest("/books", "GET", null, GetAllResult);
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

        private string sendRequest(string endpoint, string method, string data, Label label)
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
                label.ForeColor = Color.Red;
                label.Text = e.Message.ToString();
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
            return result.ToString().Replace(Environment.NewLine, "<br />");
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
            return result.ToString().Replace(Environment.NewLine, "<br />");
        }

        private string getById(string id)
        {
            string responseString = sendRequest("/books/id/" + id, "GET", null, GetAllResult);
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
                    return "Error: Invalid ID.";
                }
            }
            else return "";
        }

        private string getByTitle(string title)
        {
            string responseString = sendRequest("/books/title/" + title, "GET", null, GetByTitleResult);
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
                    return "Error: Invalid title.";
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
                string responseString = sendRequest("/books", "POST", "<Book xmlns=\"http://schemas.datacontract.org/2004/07/BookStore\"><Id>" + id + "</Id><Price>" + (price.IndexOf('.') != -1 ? price.Substring(0, price.IndexOf('.') + 3) : price) + "</Price><Title>" + title + "</Title></Book>", AddToInventoryResult);
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
                string responseString = sendRequest("/books", "POST", "{\"Id\":" + id + ",\"Price\":" + (price.IndexOf('.') != -1 ? price.Substring(0, price.IndexOf('.') + 3) : price) + ",\"Title\":\"" + title + "\"}", AddToInventoryResult);
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
            string responseString = sendRequest("/books/" + id, "DELETE", null, SellResult);
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
                    return "Error: Invalid ID.";
                }
            }
            else return "";
        }

        private string changeprice(string id, string price)
        {
            if (format.Equals("xml"))
            {
                string responseString = sendRequest("/books", "PUT", "<Book xmlns=\"http://schemas.datacontract.org/2004/07/BookStore\"><Id>" + id + "</Id><Price>" + (price.IndexOf('.') != -1 ? price.Substring(0, price.IndexOf('.') + 3) : price) + "</Price><Title></Title></Book>", ChangePriceResult);
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
                string responseString = sendRequest("/books", "PUT", "{\"Id\":" + id + ",\"Price\":" + (price.IndexOf('.') != -1 ? price.Substring(0, price.IndexOf('.') + 3) : price) + ",\"Title\":\"\"}", ChangePriceResult);
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

        private void Xml_CheckedChanged(object sender, EventArgs e)
        {
            Json.Checked = false;
            format = "xml";
        }

        private void Json_CheckedChanged(object sender, EventArgs e)
        {
            Xml.Checked = false;
            format = "json";
        }
    }
}