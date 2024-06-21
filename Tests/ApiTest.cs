using System.Net;
using PracticalTaskSelenium.Business.Models;
using PracticalTaskSelenium.Core.Utilities;
using RestSharp;

namespace PracticalTaskSelenium.Tests.Api;

[TestFixture]
// [Parallelizable(ParallelScope.All)]
[Category("API")]
public class ApiTests : ApiBaseTest
{
    [Test]
    public async Task ValidateUsersListReceived()
    {
        var request = new RequestBuilder()
            .CreateRequest("users")
            .AddMethod(Method.Get)
            .Build();

        var response = await ApiHelper.ExecuteAsyncRequest<List<User>>(_client.Client, request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "The status code was not successfull");
        Assert.That(response.ErrorMessage, Is.Null.Or.Empty, "There were error message(s)");
        Assert.That(response.Data, Is.Not.Null, "The response was equal to null");
        Assert.That(response.Data.Count, Is.GreaterThan(0), "The were no objects in the response");

        foreach (var user in response.Data)
        {
            Assert.That(user.Id, Is.Not.Null, "ID didn't exist");
            Assert.That(user.Name, Is.Not.Null, "Name didn't exist");
            Assert.That(user.Username, Is.Not.Null, "Username didn't exist");
            Assert.That(user.Email, Is.Not.Null, "Email didn't exist");
            Assert.That(user.Address, Is.Not.Null, "Address didn't exist");
            Assert.That(user.Phone, Is.Not.Null, "Phone didn't exist");
            Assert.That(user.Website, Is.Not.Null, "Website didn't exist");
            Assert.That(user.Company, Is.Not.Null, "Company didn't exist");
        }
    }

    [Test]
    public async Task ValidateResponseHeaderForUsersList()
    {
        var request = new RequestBuilder()
            .CreateRequest("users")
            .AddMethod(Method.Get)
            .Build();

        var response = await ApiHelper.ExecuteAsyncRequest<List<User>>(_client.Client, request);

        Assert.That(response.ContentHeaders, Is.Not.Null, "Content Headers were null");

        var contentType = response.ContentHeaders.FirstOrDefault(h => h.Name == "Content-Type").Value;

        Assert.That(contentType, Is.EqualTo("application/json; charset=utf-8"), "Content-Type was not equal to expected");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "The status code was not successfull");
        Assert.That(response.ErrorMessage, Is.Null.Or.Empty, "There were error message(s)");
    }

    [Test]
    public async Task ValidateResponseBodyForUsersList()
    {
        var request = new RequestBuilder()
            .CreateRequest("users")
            .AddMethod(Method.Get)
            .Build();

        var response = await ApiHelper.ExecuteAsyncRequest<List<User>>(_client.Client, request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "The status code was not successfull");
        Assert.That(response.ErrorMessage, Is.Null.Or.Empty, "There were error message(s)");
        Assert.That(response.Data.Count, Is.EqualTo(10), "The number of objects was not as expected");

        int distinctIdsCount = response.Data.Select(user => user.Id).Distinct().Count();

        Assert.That(response.Data.Count, Is.EqualTo(distinctIdsCount), "Some of IDs were not unique");

        foreach (var user in response.Data)
        {
            Assert.That(user.Name, Is.Not.Empty, "Name is empty");
            Assert.That(user.Username, Is.Not.Empty, "Username is empty");
            Assert.That(user.Company, Is.Not.Null, "Company is empty");
            Assert.That(user.Company.Name, Is.Not.Empty, "Company name is empty");
        }
    }

    [Test]
    public async Task ValidateThatUserCanBeCreated()
    {
        var request = new RequestBuilder()
            .CreateRequest("users")
            .AddMethod(Method.Post)
            .AddJsonBody(new { Name = "New User", UserName = "newuser" })
            .Build();

        var response = await ApiHelper.ExecuteAsyncRequest<User>(_client.Client, request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "The status code was not successfull");
        Assert.That(response.ErrorMessage, Is.Null.Or.Empty, "There were error message(s)");
        Assert.That(response.Data, Is.Not.Null, "The response was equal to null");
        Assert.That(response.Data.Id, Is.Not.Null, "The ID was not created");
    }

    [Test]
    public async Task ValidateUserNotifiedIfResourceNotExists()
    {
        var request = new RequestBuilder()
            .CreateRequest("invalidendpoint")
            .AddMethod(Method.Get)
            .Build();

        var response = await ApiHelper.ExecuteAsyncRequest(_client.Client, request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Unexpected HTTP status code.");
        Assert.That(response.ErrorMessage, Is.Null.Or.Empty, "There were error message(s)");
    }
}