# FluentMVCTesting.Extensions

[![NuGet version](https://badge.fury.io/nu/FluentMvcTesting.Extensions.svg)](https://badge.fury.io/nu/FluentMvcTesting.Extensions)

This library provides XML extensions for fluent unit testing of ASP.NET MVC Controllers.

## How to use it

```csharp
[Test]
public void ShouldRespondWithXml()
{
    var controller = new CallController();
    controller
        .WithCallTo(c => c.Connect("555-5555"))
        .ShouldReturnXmlResult(data =>
        {
            StringAssert.Contains(
                "Thanks for contacting us", data.XPathSelectElement("Response/Say").Value);
            Assert.That(data.XPathSelectElement("Response/Dial").Value, Is.EqualTo("555-5555"));
        });
}
```
