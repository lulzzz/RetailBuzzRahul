using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using System.Web.Routing;

namespace MvcRetailApp.IoC
{
    public class UnityControllerFactory :DefaultControllerFactory
    {
        IUnityContainer container;
        public UnityControllerFactory(IUnityContainer container)
        {
            this.container = container;
        }
        protected override IController GetControllerInstance(RequestContext reqContext, Type controllerType)
        {
            IController controller;
            if (controllerType == null)
                throw new HttpException(
                        404, String.Format(
                            "The controller for '{0}' could not be found" + "or it does not implement IController.",
                        reqContext.HttpContext.Request.Path));

            if (!typeof(IController).IsAssignableFrom(controllerType))
                throw new ArgumentException(
                        string.Format(
                            "Requested type is not a controller: {0}",
                            controllerType.Name),
                            "controllerType");
            try
            {
                controller = container.Resolve(controllerType) as IController;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(String.Format(
                                        "Error resolving the controller {0}",
                                        controllerType.Name), ex);
            }
            return controller;
        }

    }
    public class HttpContextLifetimeManager<T> : LifetimeManager, IDisposable
    {
        public override object GetValue()
        {
            var assemblyQualifiedName = typeof(T).AssemblyQualifiedName;
            if (assemblyQualifiedName != null)
                return HttpContext.Current.Items[assemblyQualifiedName];
            return null;
        }

        public override void RemoveValue()
        {
            var assemblyQualifiedName = typeof(T).AssemblyQualifiedName;
            if (assemblyQualifiedName != null)
                HttpContext.Current.Items.Remove(assemblyQualifiedName);
        }

        public override void SetValue(object newValue)
        {
            var assemblyQualifiedName = typeof(T).AssemblyQualifiedName;
            if (assemblyQualifiedName != null)
                HttpContext.Current.Items[assemblyQualifiedName] = newValue;
        }

        public void Dispose()
        {
            RemoveValue();
        }
    }

}