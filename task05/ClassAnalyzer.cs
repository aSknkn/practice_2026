
using System;
using System.Reflection;
using System.Collections.Generic;

namespace task05;

public class ClassAnalyzer
{
    private readonly Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }

    public IEnumerable<string> GetPublicMethods()
    {
        return _type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Select(method => method.Name);
    }

public IEnumerable<string> GetMethodParams(string methodname)
{
    var method = _type.GetMethod(methodname);
    var paramNames = method.GetParameters().Select(param => param.Name);
    List<string> resultList = paramNames.ToList();
    string returnTypeName = method.ReturnType.Name;
    resultList.Add(returnTypeName);

    return resultList;
}

    public IEnumerable<string> GetAllFields()
    {
        return _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Select(field => field.Name);
    }
    public IEnumerable<string> GetProperties()
    {
        return _type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(param => param.Name);
    }

    public bool HasAttribute<T>() where T : Attribute
    {
        return _type.IsDefined(typeof(T));
    }
}
