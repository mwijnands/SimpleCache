# SimpleCache #

- Allows you to check if some value exists in cache and if not, acquire the value, using 1 method call.
- Takes care of locking while the value is being acquired when it does not yet exist in cache.
- Generates cachekeys including the type name being cached so you can use the same key (ie FindAll) for different types.

## Examples ##

Get a value from cache and if it does not exist in cache yet, acquire it and put it in cache:

```csharp
var person = MemoryCache.Default.Get("Find", () => _personRepository.Find());
```

Same as above but the value will stay in the cache for only 15 minutes:

```csharp
var person = MemoryCache.Default.Get("Find", TimeSpan.FromMinutes(15), () => _personRepository.Find());
```

There are methods available to invalidate cache at an absolute expirationdate or when a certain file has changed as well. For all these methods there are async versions available in case the acquire method is async:

```csharp
var person = await MemoryCache.Default.GetAsync("Find", () => _personRepository.FindAsync());
```
