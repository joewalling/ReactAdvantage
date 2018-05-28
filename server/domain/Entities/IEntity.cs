namespace ReactAdvantage.Domain.Entities
{
    public interface IEntity<TPrimaryKey>
    {
        /// <summary>Unique identifier for this entity.</summary>
        TPrimaryKey Id { get; set; }

        /// <summary>
        /// Checks if this entity is transient (not persisted to database and it has not an <see cref="P:Abp.Domain.Entities.IEntity`1.Id" />).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        bool IsTransient();
    }
}
