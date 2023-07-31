using Data.DataPort;
using Data.Models.DTO;

namespace Specs.Drivers
{
    public interface IClientDriver
    {
        /// <summary>
        /// Setup client driver
        /// </summary>
        /// <param name="dataPort">Data Port</param>
        void SetUp(IToDoItemDataPort dataPort);

        /// <summary>
        /// Request ToDoItem list
        /// </summary>
        void RequestToDoItemList();

        /// <summary>
        /// Get result of the requested ToDoItem list
        /// </summary>
        /// <returns>List of ToDoItem</returns>
        IEnumerable<ToDoItemDto> GetRequestedToDoItemList();

        /// <summary>
        /// Teardown client driver
        /// </summary>
        void TearDown();
    }
}
