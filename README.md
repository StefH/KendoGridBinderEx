# KendoGridBinder

[![Build status](https://ci.appveyor.com/api/projects/status/6ynbga07r315xhb8?svg=true)](https://ci.appveyor.com/project/StefH/kendogridbinderex)


## Versions

| NuGet | Frameworks | NuGet |
| - | - | - |
| KendoGridBinder | .NET 4.5 | [![NuGet Badge](https://buildstats.info/nuget/KendoGridBinder)](https://www.nuget.org/packages/KendoGridBinder)
| KendoGridBinder.AspNetCore | .NET 4.6.1<br>NETStandard 1.6<br>NETStandard 2.0 | [![NuGet Badge](https://buildstats.info/nuget/KendoGridBinder.AspNetCore)](https://www.nuget.org/packages/KendoGridBinder.AspNetCore)

## Demo
A live demo can be found [here](https://kendogridbinderex.azurewebsites.net).

### Action Method
```csharp
[HttpPost]
public JsonResult Grid(KendoGridMvcRequest request)
{
    var employees = new List<Employee>
    {
        new Employee { EmployeeId = 1, FirstName = "Bill", LastName = "Jones", Email = "bill@email.com" },
        new Employee { EmployeeId = 2, FirstName = "Rob", LastName = "Johnson", Email = "rob@email.com" },
        new Employee { EmployeeId = 3, FirstName = "Jane", LastName = "Smith", Email = "jane@email.com" }
    };

    var grid = new KendoGridEx<Employee, EmployeeVM>(request, employees.AsQueryable());
    return Json(grid);
}
```

### HTML
```html
<div id="grid"></div>
```


### Script
```javascript
<script>
    var url = '@Url.Action("Grid")';

    var dataSource = {
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        pageSize: 20,
        transport: {
            read: {
                type: 'post',
                dataType: 'json',
                url: url
            }
        },
        schema: {
            data: 'Data',
            total: 'Total',
            model: {
                id: 'Id',
                fields: {
                    FirstName: { type: 'string' },
                    LastName: { type: 'string' },
                    Email: { type: 'string' }
                }
            }
        }
    };

    $('#grid').kendoGrid({
        dataSource: dataSource,
        height: 400,
        columns: [
            { field: 'FirstName', title: 'First Name' },
            { field: 'LastName', title: 'Last Name' },
            { field: 'Email' }
        ],        
        filterable: true,
        sortable: true,
        pageable: true
    });
</script>
```