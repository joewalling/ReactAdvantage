namespace ReactAdvantage.Domain.ViewModels.Dto
{
    public class SelectListDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class SelectListDto<T> : SelectListDto where T : class
    {
        public T Item { get; set; }
    }
}
