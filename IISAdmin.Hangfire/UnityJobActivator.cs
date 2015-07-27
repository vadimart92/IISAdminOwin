using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Practices.Unity;

namespace IISAdmin.Hangfire
{
    public class UnityJobActivator : JobActivator
    {
        private readonly IUnityContainer _container;

        public UnityJobActivator(IUnityContainer container)
        {
            _container = container;
        }

        public override object ActivateJob(Type type)
        {
            return _container.Resolve(type);
        }
    }
}
