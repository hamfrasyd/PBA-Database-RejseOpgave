using TravelDb.Core.Interfaces;

namespace TravelDb.Core.Handlers
{
    public class ExampleHandler
    {
        IRepository _someRepository;

        public ExampleHandler(IRepository someRepository)
        {
            _someRepository = someRepository;
        }
        public void SomeMethod()
        {
            // Implementation of the method goes here

            // Kalder typsik Repositoryies
            _someRepository.SomeRepositoryMethod();

        }
        // 1 Public metode

        // Any number of private methods

        // Handlers er en mindre Service klasse der udføre noget business logic

        // Normalt entry point til backend logik, men kan også kaldes fra Orchestrators.
    }
}
