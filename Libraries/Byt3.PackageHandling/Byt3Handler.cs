using System;
using System.Collections.Generic;

namespace Byt3.PackageHandling
{
    public class Byt3Handler
    {
        private readonly Dictionary<Type, AHandler> handlers = new Dictionary<Type, AHandler>();
        private readonly Dictionary<Type, AHandler> implicitHandlerMap = new Dictionary<Type, AHandler>();
        private readonly AHandler fallbackHandler;

        public Byt3HandlerLookupType LookupType { get; set; } = Byt3HandlerLookupType.TraverseUp;
        private bool ExactOnly => LookupType == Byt3HandlerLookupType.None;
        private bool UseFallback => (LookupType & Byt3HandlerLookupType.UseFallback) != 0;
        private bool TraverseUp => (LookupType & Byt3HandlerLookupType.TraverseUp) != 0;
        private bool IncludeInterfaces => (LookupType & Byt3HandlerLookupType.IncludeInterfaces) != 0;

        public Byt3Handler(Byt3HandlerLookupType lookupType = Byt3HandlerLookupType.TraverseUp,
            AHandler fallback = null)
        {
            LookupType = lookupType;
            fallbackHandler = fallback ?? new DefaultHandler();
        }

        public void Handle(object objectToHandle, object context)
        {
            Type t = objectToHandle.GetType();
            if (!TryGetHandler(t, out AHandler handler))
            {
                throw new HandlerNotFoundException("Has no Handler for type: " + t.AssemblyQualifiedName);
            }

            handler.Handle(objectToHandle, context);
        }

        public void AddHandler<T>(AHandler<T> handler)
        {
            AddHandler(typeof(T), handler);
        }

        public void AddHandler(Type t, AHandler handler)
        {
            if (handlers.ContainsKey(t))
            {
                return;
            }

            if (implicitHandlerMap.ContainsKey(t))
            {
                implicitHandlerMap.Remove(t); //Remove From cache and add to handlers
            }

            handlers.Add(t, handler);
        }

        public bool HasHandler(Type t)
        {
            if (handlers.ContainsKey(t))
            {
                return true; //Exact
            }

            if (ExactOnly)
            {
                return false;
            }

            if (implicitHandlerMap.ContainsKey(t))
            {
                return true; //Implicit Hit
            }

            if (TraverseUp)
            {
                return TryAddImplicitHandler(t, out AHandler _); //Traverse Up(only fails if UseFallback == false)
            }

            return false;
        }

        private bool TryAddImplicitHandler(Type t, out AHandler implicitHandler)
        {
            Type baseT = FindCompatibleBaseType(t);
            if (baseT != null)
            {
                implicitHandlerMap.Add(t, handlers[baseT]); //Connecting the Types
                implicitHandler = handlers[baseT];
                return true;
            }

            if (IncludeInterfaces)
            {
                Type[] interfaces = FindInterfaces(t);
                if (interfaces.Length != 0)
                {
                    implicitHandlerMap.Add(t, handlers[interfaces[0]]); //Connecting the Types
                    implicitHandler = handlers[interfaces[0]];
                    return true;
                }
            }

            if (UseFallback)
            {
                implicitHandlerMap.Add(t, fallbackHandler);
                implicitHandler = fallbackHandler;
                return true;
            }

            implicitHandler = null;
            return false;
        }

        private bool TryGetHandler(Type t, out AHandler handler)
        {
            if (handlers.ContainsKey(t))
            {
                //Exact Type
                handler = handlers[t];
                return true;
            }

            if (TraverseUp) //Traverse Up
            {
                if (implicitHandlerMap.ContainsKey(t)) //Cache Hit
                {
                    handler = implicitHandlerMap[t];
                    return true;
                }

                if (TryAddImplicitHandler(t, out handler)) //Try Add To Cache
                {
                    return true;
                }
            }

            if (UseFallback)
            {
                handler = fallbackHandler;
                return true;
            }

            handler = null;
            return false;
        }

        private Type[] FindInterfaces(Type t)
        {
            return t.FindInterfaces((type, criteria) => handlers.ContainsKey(type), null);
        }

        private Type FindCompatibleBaseType(Type t)
        {
            Type current = t;
            do
            {
                current = current.BaseType;
                if (handlers.ContainsKey(current))
                {
                    return current;
                }
            } while (current != typeof(object));


            return null;
        }
    }
}