using LanguageExt;

namespace Api;

// not even making things async, as they should, just to keep the example simple

public class SomeThingRepository
{
    public Option<SomeThing> Load(SomeId id) => throw new NotImplementedException();
    
    public void Save(SomeThing thing) => throw new NotImplementedException();
}

public class SomeOtherThingRepository
{
    public Option<SomeOtherThing> Load(SomeOtherId id) => throw new NotImplementedException();
    
    public void Save(SomeOtherThing otherThing) => throw new NotImplementedException();
}