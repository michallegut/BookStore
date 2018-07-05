using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace BookStore
{
    [ServiceContract]
    public interface IBookStoreService
    {
        //XML
        [OperationContract]
        [WebGet(UriTemplate = "/xml/books", ResponseFormat = WebMessageFormat.Xml)]
        List<Book> getAllXml();

        [OperationContract]
        [WebGet(UriTemplate = "/xml/books/id/{id}", ResponseFormat = WebMessageFormat.Xml)]
        Book getByIdXml(string Id);

        [OperationContract]
        [WebGet(UriTemplate = "/xml/books/title/{title}", ResponseFormat = WebMessageFormat.Xml)]
        List<Book> getByTitleXml(string Title);

        [OperationContract]
        [WebInvoke(UriTemplate = "/xml/books", Method = "POST", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        string addToInventoryXml(Book element);

        [OperationContract]
        [WebInvoke(UriTemplate = "/xml/books/{id}", Method = "DELETE", ResponseFormat = WebMessageFormat.Xml)]
        string sellXml(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "/xml/books", Method = "PUT", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        string changePriceXml(Book element);

        //JSON
        [OperationContract]
        [WebGet(UriTemplate = "/json/books", ResponseFormat = WebMessageFormat.Json)]
        List<Book> getAllJson();

        [OperationContract]
        [WebGet(UriTemplate = "/json/books/id/{id}", ResponseFormat = WebMessageFormat.Json)]
        Book getByIdJson(string Id);

        [OperationContract]
        [WebGet(UriTemplate = "/json/books/title/{title}", ResponseFormat = WebMessageFormat.Json)]
        List<Book> getByTitleJson(string Title);

        [OperationContract]
        [WebInvoke(UriTemplate = "/json/books", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string addToInventoryJson(Book element);

        [OperationContract]
        [WebInvoke(UriTemplate = "/json/books/{id}", Method = "DELETE", ResponseFormat = WebMessageFormat.Json)]
        string sellJson(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "/json/books", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string changePriceJson(Book element);
    }

    [DataContract]
    public class Book
    {
        int id;
        string title;
        double price;

        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        [DataMember]
        public double Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}
