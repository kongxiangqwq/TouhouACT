using System;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySys {
    public class Entity : UnityEngine.Object
    {
        protected readonly GameObject gameObject;
        protected readonly Transform transform;
        protected readonly Rigidbody2D rigidbody;
        
        private Dictionary<Attribute,AttributeInstance> attributes;
        public Entity(GameObject gameObject)
        {
            if (gameObject == null) { throw new GameObjectNULLException(); }
            this.gameObject = gameObject;

            transform = gameObject.transform;

            rigidbody = GetOrAddComponent<Rigidbody2D>();
        }

        public AttributeInstance GetAttribute(Attribute attribute) { return attributes.GetValueOrDefault(attribute, null); }

        public bool HasAttribute(Attribute attribute) { return attributes.ContainsKey(attribute); }

        public bool AddAttribute(Attribute attribute) { return attributes.TryAdd(attribute, new AttributeInstance(attribute)); }
        public bool AddAttribute(Attribute attribute, float baseValue) { return attributes.TryAdd(attribute, new AttributeInstance(attribute, baseValue)); }
        public bool AddAttribute(Attribute attribute, float minValue, float maxValue, float baseValue) { return attributes.TryAdd(attribute, new AttributeInstance(attribute, minValue, maxValue, baseValue)); }

        public T GetComponent<T>() where T : Component
        {
            return gameObject.GetComponent<T>();
        }

        public bool TryGetComponent<T>(out T result) where T : Component
        {
            return gameObject.TryGetComponent<T>(out result);
        }

        public T AddComponent<T>() where T : Component
        {  
            return gameObject.AddComponent<T>(); 
        }

        public T AddComponent<T>(Action<T> init) where T : Component
        {
            var component = AddComponent<T>();
            init(component);
            return component;
        }

        public T GetOrAddComponent<T>() where T : Component
        {
            if (TryGetComponent<T>(out T component))
                return component;
            else
                return  AddComponent<T>();
        }
        
        public T GetOrAddComponent<T>(Action<T> init) where T : Component
        {
            if (TryGetComponent<T>(out T component))
                return component;
            else
                return AddComponent<T>(init);
        }

        public bool HasComponent<T>() where T : Component
        {
            return TryGetComponent<T>(out T component);
        }
    }
}
