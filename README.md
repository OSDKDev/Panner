<!-- <AspNetCore> -->
<p align=center><kbd>
    <b>ASP.NET Core WebAPI?</b> You should start with <a href="https://github.com/OSDKDev/Panner.AspNetCore"><b>Panner.AspNetCore</b></a>!
</kbd></p>
<!-- </AspNetCore> -->

# Panner [![Nuget](https://img.shields.io/nuget/v/Panner?label=NuGet&color=success)](https://www.nuget.org/packages/Panner) [![Coverage Status](https://img.shields.io/coveralls/github/OSDKDev/Panner)](https://coveralls.io/github/OSDKDev/Panner?branch=master)
**Clean**, **customizable** & **extensible** framework for the configuration, parsing and application of sorts and filters from a CSV.

---

### Basic Usage - Sample

#### a. Configuration

Setting up your context to define your entities and rules.
```csharp
var builder = new PContext();

// Entity wide configuration (Option 1)
builder.Entity<Post>()
    .AllPropertiesAreSortableByName();
    .AllPropertiesAreFilterableByName();

// A more granular approach (Option 2)
builder.Entity<Post>()
    .Property(x => x.Id, o => o
        .IsSortableByName()
        .IsFilterableByName()
    ).Property(x => x.Title, o => o
        .IsSortableByName()
        .IsFilterableByName()
    ).Property(x => x.CreatedOn, o => o
        .IsSortableAs("Creation")
        .IsFilterableAs("Creation")
    );


IPContext context = builder.Build(); 

// Ta-daah! Now we have a context!
```

#### b. Parsing
Feeding Panner's context our CSV inputs and obtaining sort/filter particles

##### Sorts
```csharp
var csvSort = "-Title, Id"; // Example

if (!context.TryParseCsv(
    input: csvSort,
    out IEnumerable<ISortParticle<TEntity>> sortParticles
)){
    // Parsing failed
    return;
}

// Ta-daah! Now we have sort particles!
```

##### Filters
```csharp
var csvFilter = "Id<666, Id>13"; // Example

if (!context.TryParseCsv(
    input: csvFilter,
    out IEnumerable<IFilterParticle<TEntity>> filterParticles
)){
    // Parsing failed
    return;
}

// Ta-daah! Now we have filter particles!
```

#### c. Application
Using Panner's extension methods to apply our particles to any `IEnumerable`, `IQueryable`, `DbSet`, etc..
```csharp
var pannedQueryable = posts
    .Apply(filterParticles)
    .Apply(sortParticles);

// Ta-daah! Now we have a filtered and sorted IQueryable!
```
---

### Advanced Usage - Samples
<details>
    <summary>
        <b>Custom sort for a specific entity</b><br/>
        Ability to sort our fake entity <code>Post</code> by "popularity", an arbritary concept and not a property of the entity.<br/>
        Leveraging <code>ISortParticle</code>, <code>ISortParticleGenerator</code> and extending <code>PEntityBuilder</code>.
    </summary>
    <p>
        
##### Particle - SortPostByPopularityParticle.cs
```csharp
public class SortPostByPopularityParticle : ISortParticle<Post> // Panner's interface
{
    readonly bool Descending;

    public SortPostByPopularityParticle(bool descending)
    {
        this.Descending = descending;
    }

    public IOrderedQueryable<Post> ApplyTo(IOrderedQueryable<Post> source)
    {
        // Here's how the sorting is done when the particle is applied.
        if (this.Descending)
            return source
                .ThenByDescending(x => x.AmtLikes)
                .ThenByDescending(x => x.AmtComments);
        else
            return source
                .ThenBy(x => x.AmtLikes)
                .ThenBy(x => x.AmtComments);
    }
}
```

##### Particle Generator - SortPostsByPopularityParticleGenerator.cs
```csharp
public class SortPostsByPopularityParticleGenerator : ISortParticleGenerator<Post> // Panner's interface
{
    public bool TryGenerate(IPContext context, string input, out ISortParticle<Post> particle)
    {
        var descending = input.StartsWith('-');
        var remaining = descending ? input.Substring(1) : input;

        if (!remaining.Trim().Equals("Popularity", System.StringComparison.OrdinalIgnoreCase))
        {
            // Not the input we're interested in.
            particle = null;
            return false;
        }

        particle = new SortPostByPopularityParticle(descending);
        return true;
    }
}
```

##### PEntityBuilder Extension - PEntityBuilder.Post.IsSortableByPopularity.cs
```csharp
public static partial class PEntityBuilderExtensions
{
    /// <summary>Marks the entity as sortable by popularity.</summary>
    public static PEntityBuilder<Post> IsSortableByPopularity(this PEntityBuilder<Post> builder)
    {
        builder.GetOrCreateGenerator<ISortParticle<Post>, SortPostsByPopularityParticleGenerator>();
        return builder; // So we can chain calls!
    }
}
```

##### Setting it all up in the context builder
```csharp
builder.Entity<Post>()
    .IsSortableByPopularity();
```
---

</p></details>








