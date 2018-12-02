using System.Reflection;
using Autofac;
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
        }
    }
}