# Views Practice

An ASP.NET Core MVC implementation of the Razor views assignment.

## Run

From this directory:

```powershell
dotnet run
```

Open the local address printed by the .NET CLI. The home page links to the GitHub assignment repository; the **Sprint Tasks** bar opens the page index.

## Pages

- `/Home/Index` — home page and Sprint Tasks navigation.
- `/Home/SprintTasks` — links to each exercise view.
- `/Home/Greetings` — date-aware morning/evening greeting.
- `/Home/ProductInfo` — products created and rendered in Razor.
- `/Home/SuperMarkets` — supermarket list supplied with `ViewBag`.
- `/Home/ShoppingList` — dictionary model and injected `_TimeToBuy` partial.
- `/Home/ShoppingCart` — HTML-helper order form with GET and POST actions.

The shopping time service is registered in `Program.cs` as `ITimeService` to `SimpleTimeService`.
