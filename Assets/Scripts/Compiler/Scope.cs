using System;
using System.Collections.Generic;
using Interpeter;

public class Scope
    {
        public Dictionary<string, string> Vars { get; } = new Dictionary<string, string>();
        public Dictionary<string,object> Values{get;} = new Dictionary<string, object>();
        //Introduces "card" y recibes acceso a todos los posibles parametros de card
        public Dictionary<string, TypeInfo> typeInfo = new Dictionary<string, TypeInfo>();
        public Scope Parent {get;}

        public Scope(Scope parent = null)
        {
            Parent = parent;
        }

        public void Declare(string name, string type)
        {
            if (ContainsVar(name) && Vars[name] != type)
                throw new Exception("El tipo de la variable no coincide con el ya existente");
            
            Vars[name] = type;
        }

        public string Resolve(string name)
        {
            if (Vars.TryGetValue(name, out var type))
            {
                return type;
            }
            return Parent?.Resolve(name);
        }
        public object GetValue(string name)
        {
            if (Values.TryGetValue(name, out var value))
            {
                return value;
            }
            return Parent?.Resolve(name);
        }
        public void AssignVariable(string name, object val)
        {
            if(this.Parent is null)
            {
                Values[name] = val;
                return;
            }
            if (Values.ContainsKey(name))
            {
                Values[name] = val;
                return;
            }
            AssignVariable(name,val);
        }
        
        public bool ContainsVar(string name)
        {
            if(Vars.ContainsKey(name))
                return true;
            if(Parent == null)
                return false;
            
            return Parent.ContainsVar(name);         
        }
        public bool ContainsTypeInfo(string name)
        {
            if(typeInfo.ContainsKey(name))
                return true;
            if(Parent == null)
                return false;
            
            return Parent.ContainsTypeInfo(name);     
        }
          public TypeInfo ResolveTypeInfo(string name)
        {
            if (typeInfo.TryGetValue(name, out var type))
            {
                return type;
            }
            return Parent?.ResolveTypeInfo(name);
        }
    }
