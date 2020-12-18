using CW.BlazorDemo.Client.CQRS;
using CW.Contracts.CQRS;
using LightInject;

namespace CW.BlazorDemo.Client
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.RegisterFrom<CW.Core.CompositionRoot>();

            serviceRegistry
                .Register<IQueryExecutor, QueryExecutor>()
                .Register<ICommandExecutor, CommandExecutor>()
                ;
        }
    }
}