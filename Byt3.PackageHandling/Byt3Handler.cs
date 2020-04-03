using System;
using System.Collections.Generic;

namespace Byt3.PackageHandling
{
    public class Byt3Handler
    {
        private readonly Dictionary<Type, AHandler> handlers = new Dictionary<Type, AHandler>();
        private readonly Dictionary<Type, AHandler> implicitHandlerMap = new Dictionary<Type, AHandler>();
        private readonly AHandler FallbackHandler;
        public HandlerLookupType LookupType = HandlerLookupType.TraverseUp;
        private bool ExactOnly => LookupType == HandlerLookupType.None;
        private bool UseFallback => (LookupType & HandlerLookupType.UseFallback) != 0;
        private bool TraverseUp => (LookupType & HandlerLookupType.TraverseUp) != 0;

        public Byt3Handler(HandlerLookupType lookupType = HandlerLookupType.TraverseUp, AHandler fallback = null)
        {
            LookupType = lookupType;
            FallbackHandler = fallback ?? new DefaultHandler();
        }

        public void Handle(object objectToHandle, object context)
        {
            Type t = objectToHandle.GetType();
            if (!TryGetHandler(t, out AHandler handler))
            {
                throw new Exception("Has no Handler for type: " + t.AssemblyQualifiedName);
            }

            handler.Handle(objectToHandle, context);
        }

        public void AddHandler<T>(AHandler<T> handler)
        {
            AddHandler(typeof(T), handler);
        }

        public void AddHandler(Type t, AHandler handler)
        {
            if (handlers.ContainsKey(t)) return;

            if (implicitHandlerMap.ContainsKey(t)) implicitHandlerMap.Remove(t); //Remove From cache and add to handlers

            handlers.Add(t, handler);
        }

        private bool HasHandler(Type t)
        {
            if (handlers.ContainsKey(t)) return true; //Exact
            if (ExactOnly) return false;
            if (implicitHandlerMap.ContainsKey(t)) return true; //Implicit Hit
            if (TraverseUp) return TryAddImplicitHandler(t, out AHandler _); //Traverse Up(only fails if UseFallback == false)
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

            if (UseFallback)
            {
                implicitHandlerMap.Add(t, FallbackHandler);
                implicitHandler = FallbackHandler;
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
                if (implicitHandlerMap.ContainsKey(t))//Cache Hit
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
                handler = FallbackHandler;
                return true;
            }

            handler = null;
            return false;
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