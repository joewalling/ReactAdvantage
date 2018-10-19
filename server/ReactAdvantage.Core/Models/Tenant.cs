namespace ReactAdvantage.Domain.Models
{
    public class Tenant
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public void UpdateValuesFrom(Tenant other)
        {
            //only update the editable fields
            Name = other.Name;
        }
    }
}
