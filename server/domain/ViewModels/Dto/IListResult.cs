using System.Collections.Generic;

namespace ReactAdvantage.Domain.ViewModels.Dto
{
    /// <summary>
    /// This interface is defined to standardize to return a list of items to clients.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="P:ReactAdvantage.Domain.ViewModels.Dto.IListResult`1.Items" /> list</typeparam>
    public interface IListResult<T>
    {
        /// <summary>List of items.</summary>
        IReadOnlyList<T> Items { get; set; }
    }
}
