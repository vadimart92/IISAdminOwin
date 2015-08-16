using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace IISAdmin.Interfaces.Common
{
    [Serializable]
    public class SerializeDelegate : ISerializable
    {
        internal SerializeDelegate(Delegate delegate_)
        {
            this.delegate_ = delegate_;
        }

        internal SerializeDelegate(SerializationInfo info, StreamingContext context)
        {
            Type delType = (Type)info.GetValue("delegateType", typeof(Type));

            //If it's a "simple" delegate we just read it straight off
            if (info.GetBoolean("isSerializable"))
                this.delegate_ = (Delegate)info.GetValue("delegate", delType);

            //otherwise, we need to read its anonymous class
            else
            {
                MethodInfo method = (MethodInfo)info.GetValue("method", typeof(MethodInfo));

                AnonymousClassWrapper w =
                    (AnonymousClassWrapper)info.GetValue
                ("class", typeof(AnonymousClassWrapper));

                delegate_ = Delegate.CreateDelegate(delType, w.obj, method);
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("delegateType", delegate_.GetType());

            //If it's an "simple" delegate we can serialize it directly
            if ((delegate_.Target == null ||
                delegate_.Method.DeclaringType
                    .GetCustomAttributes(typeof(SerializableAttribute), false).Length > 0) &&
                delegate_ != null)
            {
                info.AddValue("isSerializable", true);
                info.AddValue("delegate", delegate_);
            }

            //otherwise, serialize anonymous class
            else
            {
                info.AddValue("isSerializable", false);
                info.AddValue("method", delegate_.Method);
                info.AddValue("class",
                    new AnonymousClassWrapper
            (delegate_.Method.DeclaringType, delegate_.Target));
            }
        }

        public Delegate Delegate { get { return delegate_; } }

        Delegate delegate_;

        [Serializable]
        class AnonymousClassWrapper : ISerializable
        {
            internal AnonymousClassWrapper(Type bclass, object bobject)
            {
                this.type = bclass;
                this.obj = bobject;
            }

            internal AnonymousClassWrapper(SerializationInfo info, StreamingContext context)
            {
                Type classType = (Type)info.GetValue("classType", typeof(Type));
                obj = Activator.CreateInstance(classType);

                foreach (FieldInfo field in classType.GetFields())
                {
                    //If the field is a delegate
                    if (typeof(Delegate).IsAssignableFrom(field.FieldType))
                        field.SetValue(obj,
                            ((SerializeDelegate)info.GetValue
                    (field.Name, typeof(SerializeDelegate)))
                                .Delegate);
                    //If the field is an anonymous class
                    else if (!field.FieldType.IsSerializable)
                        field.SetValue(obj,
                            ((AnonymousClassWrapper)info.GetValue
                    (field.Name, typeof(AnonymousClassWrapper)))
                                .obj);
                    //otherwise
                    else
                        field.SetValue(obj, info.GetValue(field.Name, field.FieldType));
                }
            }

            void ISerializable.GetObjectData
            (SerializationInfo info, StreamingContext context)
            {
                info.AddValue("classType", type);

                foreach (FieldInfo field in type.GetFields())
                {
                    //See corresponding comments above
                    if (typeof(Delegate).IsAssignableFrom(field.FieldType))
                        info.AddValue(field.Name, new SerializeDelegate
                        ((Delegate)field.GetValue(obj)));
                    else if (!field.FieldType.IsSerializable)
                        info.AddValue(field.Name, new AnonymousClassWrapper
                    (field.FieldType, field.GetValue(obj)));
                    else
                        info.AddValue(field.Name, field.GetValue(obj));
                }
            }

            public Type type;
            public object obj;
        }
    }
}