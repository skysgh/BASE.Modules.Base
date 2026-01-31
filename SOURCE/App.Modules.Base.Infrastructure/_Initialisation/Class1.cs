using App.Host.Web.Services;
using App.Host.Web.Services.Implementations;
using App.Modules.Base.Infrastructure.Services;
using App.Modules.Base.Infrastructure.Services.Implementations;
using Lamar;

namespace App.Host.Web.Startup
{
    public class ModuleServiceRegistry : ServiceRegistry
    {
        public ModuleServiceRegistry()
        {


            For<IFooService>().Use<FooService>();
            //slightly less fluent for Generics:
            For(typeof(IBarService<>)).Use(typeof(BarService<>));

            // TODO: Not working. Moving on for now.
            // Gives a Builder error. No tasks
            //Scan(x =>
            //{
            //    x.AddAllTypesOf<IBazService>().NameBy(x =>
            //    {
            //        //var tmp = x.Name;
            //        //var pos = x.Name.IndexOf("BazService");
            //        //var r = x.Name.Substring(0, pos);
            //        //return r;
            //    }
            //    );
            //});
        }
    }
}