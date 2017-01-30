# WhetStone
The C# companion Library

Whetstone seeks to do what the .NET standard library and the C# language don't. It has a bunch of features that together create an optimized, versatile toolset for any C# programmer, made to help write cleaner, better code.

Whetstone focuses on optimization, versatility, and usability, all while keeping LINQ-style deferred execution in mind.

The API can be found [here](TBD)

Let's dive right in to what Whetstone provides:
## I: Locked Structures and List and Collection Specialization
To understand the motivation behind Locked Structures, we need to understand the problem this module tackles:
```csharp
var list = new List<int>(new int[]{0,1,2,3,4,5,6,7,8,9,10});
list.Count(); //O(1) operation
list.Select(x=>x*x).Count(); //O(n) operation, even though the solution is trivial
```

LINQ's `Count()` method, as well as other methods (like `Contains()`and `ElementAt()`), have special checks in case the IEnumerable is an IList or IContainer. If the method LINQ is trying to execute can be executed by the Interface method, then it is executed instead. This is why `list.Count()` is O(1) operation, LINQ's `Count()` sees that list is an IList, and instead returns the IList's `Count` property.

The Issue is that even though `list.Select(x=>x*x).Count()` **can** be an O(1) operation, it isn't, because `Select()` always returns a pure IEnumerable. The solution is simple, if the `Select()` method sees an ICollection, it will return an ICollection, disguised as an IEnumerable. This solution has two chief problems, both to be addressed:

### \#1- The Immutable Structures
The concept of an IList or ICollections that cannot be edited is scary in the c# world. Convention is that ILists can be mutated. It was decided that creating a read-only IList was justified on three grounds:

* The .NET IList and ICollection Interfaces have [IsReadOnly](https://msdn.microsoft.com/en-us/library/system.collections.ilist.isreadonly(v=vs.110).aspx) properties that we can use to fulfill this purpose.
* C# arrays are partially immutable, throwing exception when one tries to insert items into them.
* IReadOnlyList or IReadOnlyCollection inputs don't trigger LINQ's special checks.

This means that **whenever a list or collection is to be edited, the user must first be sure it isn't read-only** (although, in most most cases, it can known beforehand, see Mutability Passing Functions, below).

### \#2- Compile-Time Specilization
It was decided that the specialized LINQ methods (the ones introduced in this module). Will both return and recieve specilized IEnumerables **explicitly**. This means Two things:
* The function's arguments must be recognized as ILists or ICollections **at compile time**. If they are not (for example in case the argument is `(IEnumerable<int>)new int[]{0,1,2,3}`), LINQ's regular function will be executed instead of the Whetstone specialized function. This is to avoid ambiguity as to which method is called (Whetstone's or Linq's), as well as to allow for explicit specialized IEnumerable return:
* When you use Whetstone's functions, you can be certain you are getting back an ICollection or IList, it's the return value's type. This avoid a lot of cumbersome casting and unnecessary converting. It also allows function chaining like this:
```csharp
var list = new int[]{2,3,5,7}.Select(x=>x*x) //{4,9,25,49}
            .SelectMany(x=>new int[]{x,x*2}) //{4,8,9,18,25,50,49,98}
            .Zip(new int[]{0,1,2,3,4,5,6,7}) //{(4,0),(8,1),(9,2),(18,3),(25,4),(50,5),(49,6),(98,7)}
            .Skip(2)                         //{(9,2),(18,3),(25,4),(50,5),(49,6),(98,7)}
            .Select(a=>a.Item1-a.Item2);     //{7,15,21,45,43,91}
            //Still a List!
```
### \#3-Clarifications and Repercussions
Everything returned by Whetstone's LINQ-like functions is still lazily evaluated. It uses no extra memory and is sometimes more efficient than pure LINQ's IEnumerable.

The abundance of read-only Collections and Lists brings new opportunities to the C# toolbox, like:
```csharp
var dotNetRange = Enumerable.Range(0,10); //Why is this not a range?
var whetstoneRange = range.Range(10); //Oh, yeah! Constant-time, Constant-memory random access!
```
Wherever possible, and without sacrificing complexity or memory, Whetstone strives to return ILists and ICollections instead of IEnumerables for improved usability.
## II: Mutability Passing Functions
As stated above, it's usually pretty easy to tell in advance when a returned IList or ICollection is read-only or not. But sometimes it's not that simple.
```csharp
var originalList = new List<int>(new int[]{2,3,7,11});
var slice = originalList.Skip(2); //Still a list!
slice.Insert(0,5);//wait...
```
Should `originalList` now be {2,3,5,7,11} and `slice` be {5,7,11}? Should `originalList` stay how it is, ans `slice` copy itself into a new List with the added item? Should an exception be thrown, making this theoretical usage impossible? The decisions are:
* Whetstone's specialized LINQ functions are made to be contant-memory, and they will always stay that way.
* IList and ICollection mutations will be allowed, and **they will mutate the original IEnumerable**.
* The returned value will be read-only if the input is read-only.

Code like the one above get confusing very quickly, and is discouraged in cases where reference to the original input is kept somewhere.
## III: Fields
Fields is a crutch. It seeks to prevent a lot of messy code out there. Take a look at [.NET's `Abs()` function](https://msdn.microsoft.com/en-us/library/system.math.abs(v=vs.110).aspx). 7 overloads for functions that all basically do the same thing:
```csharp
TYPE Abs(TYPE value){
    if (value < 0)
        return -value;
    return value;
}
```
The reason is clear: **C# generics don't allow static functions and, by extension, operators**. Whetstone's solution is to use **fields**.
Fields are like referencable static classes. The same way that an IComparer provides a way to compare types, Field can add, subtract, negate, and a whole lot of other things to any type.
### \#1- Instant Lookup
There is a central dictionary of fields, one for each type (much like `Comparer<T>.Default`). Meaning you don't have to provide the same integer field every time you want to use field operations.
You can also transform your object into a Field-Wrapped object, letting you use operators and the like with generic lookups instead of a dictionary lookup.
```csharp
public static T Abs<T>(T value)
{
    Field<T> field = Fields.getField<T>();//get the default field
    if (field.isNegative(value))
        return field.Negate(value);//not pretty but gets the job done
    return value;
}
```
Or:
```csharp
public static T Abs<T>(T value)
{
    var wrapped = value.ToFieldWrapper();//no extra memory
    if (wrapped < 0)
        return -wrapped;//elegent, efficient, generic
    return value;
}
```
### \#2- Support for Custom Types
Another strong tool is the support for user-created types. A user can make his type Field supported in one of two ways:
1. Create a Field object, and add it to the central dictionary (Check the API as to how).
2. Simply add the relevent operators and castings into your custom type! (If a relevent field cannot be found, a dynamic field will be returned, that uses dynamic dispatch).

### \#3- Overhead Avoidance
Obviously, all these lookups and calls have some overhead. Which is why, in most cases where fields are involved, you can bring your own, precompiled values.
```csharp
var list = new int[]{2,3,5,7,11};
//instead of this:
list.GetProduct();
//this is better:
list.GetProduct(1);
//yes, this is even better:
list.GetProduct(1,(a,b)=>a*b);
```
For this reason, many common functions like `Range` have specialized, non-generic cases for common types.
### \#4- Repercussions
If you're willing to accept the runtime overhead, fields can have many usages you never thought of before:
```csharp
var aTOz = range.IRange('a','z');//Contant-memory list of all lowercase letters
var concatedNumbers = range.Range(10).Select(a=>a.ToString()).GetSum();//"0123456789"
```
## IV: Many, Many Extension Methods
This library features a lot of extention methods that you never knew you needed. Designed to make the code more readable, usable, and efficient. Here are some examples:
```csharp
var lists = new []{range.Range(10),range.Range(12,22),range.Range(0,100,10)};
lists.Select(a=>a.Count).AllEqual(); //checks that all members of an enumerable are equal

var bigRange = range.Range(50);
bigRange.Chunk(10); //{0..10,10..20,20..30,30..40,40..50}

public static IEnumerable<bool> Lucky(Random r, double odds)
{
    while (r.NextDouble() < odds)
    {
        yield return true;
    }
}
var lucky1 = Lucky(new Random(),0.5);
var lucky2 = Lucky(new Random(),0.99);//this guy could take a while...
lucky1.CompareCount(lucky2); //but now we don't have to wait for it!

new int[]{2,3,5,7}.Trail(2);//{{2,3},{3,5},{5,7}}

"this is a very long string".LongestCommonPrefix("This is an even longer string"); "This is a "
```

And much, much more.

I hope you'll give Whetstone a try. It's sure to be one of your go-to C# libraries.
