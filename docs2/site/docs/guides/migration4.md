# Migrating from v3.x to v4.x

## New Features

## Breaking Changes

* `NameConverter` and `SchemaFilter` have been removed from `ExecutionOptions` and are now properties on the `Schema`.

### Enumeration Graph Types

Version 4.0 compares enumeration values with the values returned from fields resolvers
by their value; the name is ignored. This will break applications where the enumeration
value does not equal the set value in the `EnumerationGraphType`. This is typically seen
with custom enumeration graph types as shown in the following example:

```csharp
public enum IsSet
{
    NotSet,     // defaults to value 0
    Set         // defaults to value 1
}

public class AssignmentStateEnumType  : EnumerationGraphType {
    public AssignmentStateEnumType()
    {
        // In Version 3.x the value is ignored and this would work properly because it is matched by name
        //AddValue(name: "NotSet", description: "There is no assignment", value: 10);
        //AddValue(name: "Set", description: "There is an assignment", value: 20);

        // Version 4.x requires you to use the correct value or enumeration member

        // Works, but not recommended
        AddValue(name: "NotSet", description: "There is no assignment", value: 0);
        AddValue(name: "Set", description: "There is an assignment", value: 1);

        // Recommended
        AddValue(name: "NotSet", description: "There is no assignment", value: IsSet.NotSet);
        AddValue(name: "Set", description: "There is an assignment", value: IsSet.Set);
    }
}

class MyGraphType : ObjectGraphType
{
    public MyGraphType()
    {
        Field<AssignmentStateEnumType>("Test", resolve: context => IsSet.Set);
    }
}
```

If you previously used strings to return enumeration values, you may need to make
a change as shown in the following sample:

```csharp
public class AssignmentStateEnumType  : EnumerationGraphType {
    public AssignmentStateEnumType()
    {
        // In Version 3.x the value is ignored and this would work properly because it is matched by name
        //AddValue(name: "NotSet", description: "There is no assignment", value: 0);
        //AddValue(name: "Set", description: "There is an assignment", value: 1);

        // Version 4.x requires you to use the correct value or enumeration member
        AddValue(name: "NotSet", description: "There is no assignment", value: "NotSet");
        AddValue(name: "Set", description: "There is an assignment", value: "Set");
    }
}

class MyGraphType : ObjectGraphType
{
    public MyGraphType()
    {
        Field<AssignmentStateEnumType>("Test", resolve: context => "Set");
    }
}
```
