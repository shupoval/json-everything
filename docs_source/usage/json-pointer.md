# JsonPointer<nsp>.Net

[JSON Pointer](https://tools.ietf.org/html/rfc6901) is a mechanism that allows you to isolate a single element within a JSON document by navigating down a series of object properties and array indices.

## The syntax

The syntax is really simple:

```
/objects/and/3/arrays
```

This pointer has four segments.  Each segment specifies either an object property or, if the segment is a number, an array index.  Interestingly, the `3` above could be either an object property *or* an array index.  There's nothing about the pointer that specifies a distinction.  It will resolve for both of these documents:

```json
{
  "objects": {
    "and": [
      "item zero",
      null,
      2,
      {
        "arrays": "found me"
      }
    ]
  }
}

{
  "objects": {
    "and": {
      "3": {
        "arrays": "found me"
      }
    }
  }
}
```

If a property contains a `/`, it must be escaped by replacing it with `~1`.  Additionally, the escape character `~` must be escaped by replacing it with `~0`.

It also supports a URL format, which is essentially the same thing, except that it starts with a `#`, then followed by the standard pointer.  This format also will `%`-encode any URL-reserved characters, like `=` and `?`.

## In code

The `JsonPointer` struct is the model for JSON Pointer.

The easiest way to create one is to parse it with either `Parse()` or `TryParse()`.  This will give you an instance of the model that can be used to evaluate instances.

Evaluating the above example might look something like this:

```c#
var element = JsonDocument.Parse("{\"objects\":{\"and\":[\"item zero\",null,2,{\"arrays\":\"found me\"}]}}");
var pointer = JsonPointer.Parse("/objects/and/3/arrays");

var result = pointer.Evaluate(element); // contains a JsonElement with a "found me" value.
```

You can also create pointers from objects using lambda expressions:

```C#
var pointer = JsonPointer.Create(x => x.objects.and[3].arrays);
```

This yields the same pointe as the example above.

## Relative JSON Pointers

[JSON Hyperschema](https://datatracker.ietf.org/doc/draft-handrews-json-schema-hyperschema/) relies on a variation of JSON Pointers called [Relative JSON Pointers](https://tools.ietf.org/id/draft-handrews-relative-json-pointer-00.html) that also includes the number of parent navigations.  This allows the system to start at an internal node in the JSON document and navigate to another node potentially on another subtree.

Relative JSON Pointers are implemented with the `RelativeJsonPointer` struct.  Interactions with this struct are very similar to `JsonPointer`.

Unfortunately, since evaluation of these pointers require parent navigation, a feature which is [currently unsupported by `System.Text.Json`](https://github.com/dotnet/runtime/issues/40452), only the model is functional at this time.