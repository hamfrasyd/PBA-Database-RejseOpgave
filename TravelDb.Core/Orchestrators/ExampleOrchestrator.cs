using TravelDb.Core.Handlers;

namespace TravelDb.Core.Orchestrators
{
    public class ExampleOrchestrator
    {
        ExampleHandler _someHandler;

        public ExampleOrchestrator(ExampleHandler someHandler)
        {
            _someHandler = someHandler;
        }

        public void SomeOrchestratorMethod()
        {
            // Use the handler to perform some business logic
            _someHandler.SomeMethod();
            // _anotherHandler.AnotherMethod();
            // _AthirdHandler.AthirdMethod();
        }

        // Also 1 public method

        // Orchestrators use other handlers to perform business logic.

        // Usually no Repository calls in the Orchestrator, but it can be done if needed.
    }
}
