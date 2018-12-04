using System.Reflection;
using Autofac;
using Thomfre.AdventOfCode2018.Tools;
using Module = Autofac.Module;

namespace Thomfre.AdventOfCode2018
{
    public class SolverModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .Where(t => t.Name.EndsWith("Solver") && !t.IsAbstract)
                   .AsImplementedInterfaces();

            builder.RegisterType<SolutionPresenter>()
                   .As<ISolutionPresenter>()
                   .As<IStartable>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<InputLoader>()
                   .As<IInputLoader>()
                   .AsSelf();
        }
    }
}