﻿# What is this repository?
This repository is made for tracking my progress on reading **CodeMaze book** - Ultimate ASP.NET Core Web API, from zero to six-figure backend developer. [Link to store](https://code-maze.com/ultimate-aspnetcore-webapi-second-edition/?source=nav). Which I bought for improving my knowledge on web APIs inside .NET and building cleaner applications in my future career.

## More about book

Book have covered some important topics like:

 - Middlewares
 - Logging
 - Architecture implementation
 - Data models, DTOs and EF core migrations
 - Handling requests
 - Error handling
 - Content negotiation
 - Validations
 - Action filters
 - Paging
 - API searching,sorting and filtering
 - HATEOAS (Hypermedia as  the engine of application state)
 -  API versioning
 - Caching and rate limiting
 - Authentication and authorization with JWT
 - Option pattern and loading configuration
 - Deployment of application

## Book also included some additional topics

 - CQRS pattern
 - MediatR library

# My goal is to...
My goal is to explain what I have learned, what I think should be better approach and to explain some problems that I struggled with while reading and following the tutorial. I will not be able to cover whole book, but I will give my best to catch most important parts.

 ## Intro and project configuration
 On start of this book I havent catch some new and interesting topics for myself, but I would like to emphasize following parts... First of all extension methods, which was not completely new for me, but I liked the way that throught whole book author will put all features inside extension methods and in the end when I observed created project, I actually liked how `Program.cs` file stayed clean and readable. 
Second thing that I would like to emphasize is how author explained in details environment based settings and environment variables which is crucial part to separate `Production` and `Development` envs.

## Middlewares
This chapter learned me a lot. What are middlewares, how to use them in right way, important order of middlewares in application and much more. Book also contains couple of diagrams which clearly show how middleware behaive and how/when I need to use `.Run()` or `.Use()` functions. This topic is not complicated and hard to understand, but it error prome if not implemented and documented correctly. This is where book and their authors did great job on explaining all parts in details. 
Till now I never used custom middlewares in my projects, just because I took another approach. But I need to consider using them much more in future for to improve writting cleaner code.
I found out that methods like `.Map()` and `MapWhen()` would definitely help me on mine previous project, where I could have used `.Map()` middleware only for specific endpoints or/and `.MapWhen()` middleware for HttpContext which contains specific headers for example.

## Logging
> [!WARNING]
> In next chapter I will explain my personal opinion and not facts.

In this chapter I did some changes in my project and I avoided some libraries that book used. I did not use [NLog](https://nlog-project.org/) for logging, but I used [Serilog](https://serilog.net/). Reason for that is just because `Serilog` feel much more updated and upgraded logging system, then `Nlog`.  In some project that I worked on, Serilog showed much more options and extensions for better structural logging and this is my main reason for using it. 
But I followed other steps that is inside the book. I wrapped my logging into singleton and did some basic abstraction with `ILoggerManager` interface. Application logs were displayed inside log file and inside app console. 

## Onion architecture 
Architecture design of application is something that is complicated, even if its look simple on beginning. At least for me... In this book, author made decision to use `Onion architecture` and I will not deep dive in how and why its good or not, but explain in few sentences what I learned in this chapter.

1. Rule is that this **architecture split application in next layers**:

 - Domain layer - Which hold our domain models, contracts, exceptions.
 - Service/Application layer - Which hold our services and bussines logic.
 - Infrastructure layer - Which hold our DbContext and other I/O operations.
 - Presentation layer - Which hold (in this example) our controllers for API actions.
 
 2. Rule is that **flow of dependencies is toward the core** of the architecture.
3. Layers **interact strictly throught the interfaces**

There is much more to talk about written architecture, but it would take me a lot of time, but I want to point to one sentence from the book: 

> Testability is very high with the Onion architecture because everything depends on abstractions.

And this is where this architecture shine, because it would be easy to mock testing with Moq library.

## Repository pattern
Like I said I will skip some parts of book, because I cant fit everything. I want to stop on repository pattern part, just because I used to implement it in some of my previous project, but must admit that, I failed to implement it in correct way. This is where book showed me pretty good example and explained step by step how to use it. I will post interface from the book only to explain crucial parts of abstraction.

```csharp
public interface IRepositoryBase<T>
{
	IQueryable<T> FindAll(bool trackChanges);
	IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression, bool trackChanges);
	void Create(T entity);
	void Update(T entity);
	void Delete(T entity); 
}
```
This simple but very strong interface will be used on rest of the project and I want to point how it helped me to minimize errors with EF Core and tracking entities. 
Also I want to point `Expression<Func<T,bool>> expression` where Expression is used for optimized querying and its recommended to use it like this.

## Handling all types of requests
In book author used separated project for controllers and used `.Assembly` to load them all which is something also new for me. I used to have everything in main project, but this approach is handy and I may consider using it in future projects.
For action methods I will just cover part that I found it first time in book so for example, `PATCH, OPTIONS, HEAD` I never worked with them before and with book I found interesting how good `PATCH` method can be useful and optimized instead of using only `PUT`. So basicly `PATCH` using something called `JsonPatchDocument` and to use it we should put in our request header media type `application/json-patch+json`

| OPERATION | EXPLANATION |
|--|--|
| Add | Assigns a new value to a required property |
| Remove | Sets a default value to a required property |
| Replace | Replaces a value of a required property to a new value |
| Copy | Copies the value from a property in other `path` property |
| Move | Moves the value from property in the `from` to path `property` |
| Test | Tests if a property has a specified value. |

Example of one request body for `Add method`:
```json
{
	"op": "add",
	"path": "/name",
	"value": "new value"
}
```
## Global error handling
Book explained couple of ways how to handle errors globally. First book used built in middleware for exceptions, which was okey, but in the end book used `result pattern` which I personally like and prefer more. But, in the end all of examples gave good error handling.
For example I liked how I created custom Exceptions that inherit also from custom exception. 
Flow example:

    CompanyNotFoundException > NotFoundException > Exception
    
and after pairing it with middleware, exception response is clear and easy to read, but also abstracted for client to avoid leaking some unneeded and dangerous informations.

## Custom formatter
After reading chapter about custom formatters, I was not even aware that its possible to change media types to client so easy. For example from `JSON` to `CSV`.  Book used `TextOutputFormatter` base class to create custom return media type. To be precised, I needed to create CSV output formatter. With only one class, couple of created functions and at end adding it to controller config with: `config.OutputFormatters.Add(CustomFormatter)`


## Object validation
Validating object is also something that I used a lot in other projects and I know how crucial is to have good validators. In book I first used Built-in attributes from `System.ComponentModel.DataAnnotations` namespace to validate properties and learned how to implement my own custom validator and how to make object validatable with `IValidatableObject` interface. But at the end of the book we switched to `FluentValidation` library which for me is much better approach, but not for simple projects.

Some of built-in validators that I have used:

 - [EmailAddress]
 - [Range]
 - [Required]

## Asynchronous programming 
After pointing couple of time how I will avoid deep diving into some parts of the book and this topic is definitely number one most complex part in general. I will just emphasize couple of things that I learned even before this book:

 - Every `I/O` operation should be async to avoid blocking program flow.
 - Function that uses `async/await` should be named with suffix `Async`.
 - Creating thread or using thread pool instead of using tasks may lead to performance issues.
 - Every function marked as `async` need to return `Task<T>` or `Task` object.
- Avoid using `.Result` which may create strange behavior in program.
- Use cancellation token for cancelling tasks.

And I will copy sentence from book:

> It is very important to understand that the Task represents an execution of the asynchronous method and not the result.

This is rules are something I followed on all my previous projects and book on similar way simplifed asynchronous programming, because we can all agree how this topic can be complicated.

## Action filters 
In chapter about action filters, I didnt know anything, so I took it carefully and I will definitely used it in next projects because it offer a great way to hook into MVC action invocation pipeline. 

There is different types of filters:

 - Authorization filters
 - Resource filters
 - Action filters
 - Exception filters
 - Result filters

Author of the book used example with `action filter` where we created custom filter to validate our data. And to create our filter we need to implement `IActionFilter`and with two functions `OnActionExecuting` `OnActionExecuted` we can create all the logic we need.
We can scope our filters on different ways: 

 - Global
 - Action
 - Controller

Where if we want to add it globally we need to put it in config action of `AddControllers` function. But if we want to use filter in scope of action or controller we need to add it as scoped service. After that we can attach our action just like we do with attributes.
Also I found also important to follow flow of custom actions and this is the flow:

 1. OnActionExecuting ( Global )
 2. OnActionExecuting ( Controller )
 3. OnActionExecuting (Action method )
 4. Action method is executing ...
 5. OnActionExecuted ( Action method )
 6. OnActionExecuted ( Controller )
 7. OnActionExecuted ( Global )

## Paging, sorting, filtering, data shaping
Book cover this topic very well and there is good reason why. Having good paging, sorting, filtering and data shaping improves our customers better experience, improves our performance and speed of searching data. 
Paging was implented simply by adding another parameter at `GET` action with decorator `[FromQuery]` where we take that metadata and `.Skip` or/and `.Take` data from EF Core query. Also we added new header to response called `X-Pagination` with `.WithExposedHeaders("X-Pagination")` method inside our CORS policy which in the end give us metadata about current page, next page, previous page and much more...

Filtering also works same as paging by extending metadata from endpoint query params. Precisely author of book added only few type of filtering: `MinAge` and `MagAxe` filters.
But on my opinion this feature could be much better written to avoid this type of limited filtering. 

Search feature works by adding string property called `SearchTerm` and we modified our extension method to check, trim and set .`ToLower()` and call `.Search(SearchTerm)` which is used by ef core.

But sorting and data shaping was hardest part or lets say most complicated part. For sorting we added another string property `OrderBy` and we extended it with extension method `Sort` which do next:

 - Check if `OrderBy` is empty or `null`
 - Trim and split charachter `,`
 - Use reflection with binding flags `Public` and `Instance`
 - Create string builder in which we will store all properties that we want to sort.
 - Add direction `descending` or `ascending` if param end with ` desc`
 - Last check if created string from string builder is empty or `null`
 - return employees with `.OrderBy(orderQuery)`

Where this would not be possible without another system nuget package called:

    System.Linq.Dynamic.Core

Data shaping could be topic for itselfs, but I will shrinke informations. In our request parameters we added again another property called `Fields` and then created `IDataShaper<T> where T : class`. Used reflection again to check all properties from type T and shaped the data.  Next sentence is copied from book:

> The GetRequiredProperties method does the magic. It parses the input string and returns just the properties we need to return to the controller.

 And on the end our `DataShaper` was added as scoped service to our app.
I could write few pages on DataShaper on how does it work. I can only add at the end that I had couple of issues and which was not well documented inside the book and I needed to use source code to fix the problem.
But in the end, request link was looking like this:

    https://localhost:5001/api/companies/C9D4C053-49B6-410C-BC78-2D54A9991870/employees?pageNumber=1&pageSize=4&minAge=26&maxAge=32&searchTerm=A&orderBy=name

## HATEOAS
HATEOAS stands for hypermedia as the engine of application state. Which I never saw before and it was completely new topic to me. Book says how this topic is important because REST cannot be consider as RESTful because we would miss benefits from this architecture.

> REST architecture allows us to generate hypermedia links in our responses dynamically and thus make navigation much easier.

So some benefits from HATEOAS are:

 - API becomes self-discoverable and explorable.
 - A client can use links to implement the logic.
 - The server drives the application state and URL structure and not vice versa.
 - The link relations can be used to point the developer's documentation.
 - Versioning throught hyperlinks becomes easier.
 - API is evolvable without breaking all the clients.
 
 What are downsides? Only that it's not easy to implement and takes to much time so we should consider doing it before start working on it. 

## Versioning APIs

Versioning is very important topic for me, because I had couple of situations where my app broke due to changes on back end part. Requierements change over time and without good versioning system, there is big chance that we will have huge amount of problems in future. I learned that in hard way... In this book I didn't used libraries they gave, because it's market as obsolete `Microsoft.AspNetCore.Mvc.Versioning`. Instead I used `WHAT I USED?`. So some parts of this chapter was completely useless for me. 

First of all I modified my controllers and added one more controller called `CompaniesV2Controller`. 
I changed Route attribute to: `[Route("api/v{version:apiVersion}/companies")]`
and added new attribute to controller: `[ApiExplorerSettings(GroupName = "v1")]`.
Also there is a way where you can set individual actions to different API version just by adding new attribute above given action: `[MapToApiVersion("1.0")]`.
With implemented version system in every response we get new header `X-Api-Version` which indicate what api version was used for action.

## Caching
With caching we can improve our performance, but when problem occure, first of all we should take a look at caching, because cached data may be the only logical problem. This is also something I learned hard way... 

To use caching in our action method we should add attribute:
`[ResponseCache(Duration = 60)]`
But that would make our app a horror story if someone ask us to change all durations from all our controllers in app. This is where we can also create caching profiles inside `.AddControllers` by adding next:
```csharp
config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
```
And used it just like this:
```csharp
[ResponseCache(CacheProfileName = "120SecondsDuration")]
```
As I mentioned before, caching is error prone and there is something called `Validation model` which is used for validate and freshness of the response. It will check if response is cached and still usable. As book said without this, some type of problem may occure:

> If someone updates that company after five minutes, without validation the client would receive the wrong response for another 25 minutes

To support validation I used library `Marvin.Cache.Headers` as book said. This library support cache headers, expires, ETag and Last-Modified by adding just next line of code:

```csharp
services.AddHttpCacheHeaders();
```
But at the end of this chapter book said how ResponseCaching library is not so good for validating, it is much better for just expiration and author gave us couple of alternatives for use:

>  1. Varnish 
>  2. Apache Traffic Server
>  3. Squid
>  CDN ( Content Delivery Network) (not free)


## Rate limiting and throttling 

I will not deep dive on this topic, because it's easy to implement and by the title you can make conclusion what does rate limiter and throttling actually do. By following book I used library called `AspNetCoreRateLimit` and by adding simple configuration I configured for what endpoint, what is limit for request in which period. Also I needed to add request pipeline:
```csharp
app.UseIpRateLimiting();
```

## JWT, Identity and Refresh token

By far this chapter, at least for me, was most useful part of the book. Why? With this book, for first time I implemented completely functional `Authentication` and `Authorization` and by replacing `DbContext` to `IdentityDbContext` using a `Microsoft.AspNetCore.Identity.EntityFrameworkCore` library from Microsoft.

```csharp
builder.Services.AddAuthentication(); 
// This is my extension method for adding identity.
builder.Services.ConfigureIdentity();

// Adding middlewares
app.UseAuthentication();
app.UseAuthorization();
```
Just couple of short sentences for **JWT bearer token**. 
Full name is JSON Web Token which is used for storing user identity and claims.
Token is splitted in 3 parts:

 - Head
 - Payload
 - Signature

Where in **head** we have all informations for JWT itself, **payload** (or body) we store claims and data. And with **signature** we are aware of which cryptographic was used for encryption/decryption. 
> WARNING
> JWT uses symmtrical type of encryption.

To be able to use JWT in safe way I needed to add secret key / password and valid audience who is able to create encrypted data and load them to `.AddJwtBearer()`. Of course before adding `.AddJwtBearer()`, we added authentication like this:
```csharp
	services.AddAuthentication(opt =>
	{
		opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
```
 ...where we configured how this authentication will authenticate user and what is default challenge schema.

To make our app follow SOLID principles, we created interface to make some abstraction and interface segregation:

```csharp
public interface IAuthenticationService
{
	Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
	Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
	Task<string> CreateToken();
}
```
Which will be used in our application for all authentication processes like `Login` or `Register` or to receive `RefreshToken`. To authenticate and to be authorized at the end we created endpoints `/login` `/register` endpoints and attribute `[authorize]` to protect our action methods in controller from unauthorized users. 

Book finished this topic with implementing the `RefreshToken` to help our user to avoid logging every time when token expires. Main question for me was, why we dont use just token and set greater expiration date? And book explain it well. 
Just by increasing the token expiration, there is better chance for hacker to take our token and use it. By providing token with shorter expiration date and refresh token, malicious user cant use token for too long.

To add refresh token in identity I needed to add next:

 - Add 2 new properties in our `IdentityUser`
 - Add new function to our interface `CreateToken(bool populateExp)`
 - Modify our `AuthenticationService`
 - Create function that will create our refresh token
 - Create a function that will get principal from expired token
 - Modify our already made function `CreateToken()` to handle and return refreshToken.
 - Modify `/login` endpoint and create new controller for creating token with endpoint `/refresh` for refreshing our token.

## Binding options and option pattern
In my previous project I used option pattern a lot. Just because it's simple and give us much more possibilities at runtime and development. Only new part that I learned is difference between next interfaces:

 - `IOptions<T>` is used for loading configuration only once. Not support reload.
 - `IOptionsSnapshot<T>` Can reload/refresh configuration, registered as scoped service. Cant be loaded to **singleton service**.
 - `IOptionsMonitor<T>` Same as snapshot but not as scoped but as singleton. Can be injected in **any service lifetime**.

I dont have anything special to add to this topic. Option pattern is something crucial to use in almost every project for our good. By finishing this book chapter, I moved all configuration properties inside `appsettings.json` and `appsettings.development.json` file.

## Documenting API with Swagger
 
 Swagger is so powerful but yet so simple tool, that I used in every project. But by reading the book I also learned something new which may help me in future. I learned even before how swagger can help me and my colleagues in work flow and improving testing and readability of created API. 

With book I learned that swagger can have multiple endpoints for different versions of API and that controller can be discovered with swagger by adding next attribute above controller class:
```csharp
[ApiExplorerSettings(GroupName = "v1")]
```

Something that we needed to implement and it's not implemented by default is `authorization` support. So by adding:

```csharp
s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
{ 
	In = ParameterLocation.Header, 
	Description = "Place to add JWT with Bearer", 
	Name = "Authorization", 
	Type = SecuritySchemeType.ApiKey, Scheme = "Bearer" 
});
``` 

... And by adding:

```csharp
s.AddSecurityRequirement(new OpenApiSecurityRequirement() 
{ 
	{ 
		new OpenApiSecurityScheme 
		{ 
			Reference = new OpenApiReference 
			{ 
				Type = ReferenceType.SecurityScheme, 
				Id = "Bearer",
			},
			Name = "Bearer",
		},
		new List()<string>()
	}
});
		
```
By adding this we are able to see on every endpoint icon if authorization is required.
One more thing that I didnt know and I will copy sentence from book is:

> Adding triple-slash comments to the action method enhances the Swagger UI by adding a description to the section header.

## Deployment on IIS

Reason why I took this book is to improve my technology stack. This is why almost everything that I wrote here is something that I already worked on.

IIS is also something that can be packed in separed book because of possibilites...
But in the end I learned again something new regardless of my previous knowledge of deployment.

**Configuring enviroment file** is something I used first time thanks to this book because of JWT stored in environment file. Yes okey, I have put secret in appsettings, which is not safe and it's very bad practice, but I followed tutorial and I understand the point of author on this topic.
But after I finished app and moved to this chapter, I felt really bad by not trying it by myself. 


## Response and performance improvements

This bonus chapter I also finished and it's implemented in my example app. Can't say anything more then I really prefer this type of response type ex. response pattern, just because it is much more cleaner and easire to extend like how author say:

    We have a solution that is easy to implement, fast, and extendable.

## CQRS and MediatR

CQRS pattern and MediatR library is by far the greatest thing I tried and I really enjoyed this chapter. CQRS which stands for `Command Query Responsibility Segregation` is something I have not yet used, because of scale of my projects, and even book or other developers recommend sometimes to avoid it because of complexity. Can't say it's really complicated, but yeah maybe sometimes not needed to add it, because we may fail `KISS` principle which say `Keep it simple and stupid`. 

With MediatR library which in the end encapsulate CQRS pattern and give us good decouple of components and layers, we accomplished one strong, clean and readable code. Good thing of this pattern is code separation where our read-only part can be much faster, while command part stay the same, easy test code writting and mocking and in the end easy feature upgrades.

Also one good reason of why should someone use MediatR is `IPipelineBehavior` which stands after the request and before behaivor and with that we can modify/validate or what ever we need with data. Just like ASP.NET use Middleware concept.

Implementing for me was not hard, but only because I touched MassTransit which in the end is same principle, only difference is `Broker` like `RabbitMQ` or `Kafka`

## End of story

This is biggest `readme.md` I ever wrote, but point of this is to explain what I have learned, what topics I already known, proof of work, practice in writting good documentation and for myself colective sum of learned topics. 




