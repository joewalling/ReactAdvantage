namespace ReactAdvantage.Domain.ViewModels.Dto
{
    public class PagedAndSortedInputDto : PagedInputDto
    {
        public string Sorting { get; set; }

        public PagedAndSortedInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}