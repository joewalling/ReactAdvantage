using System.ComponentModel.DataAnnotations;

namespace ReactAdvantage.Domain.ViewModels.Dto
{
    public class PagedAndFilteredInputDto
    {
        [Range(1, AppConsts.MaxPageSize)]
        public int MaxResultCount { get; set; }

        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        public string Filter { get; set; }

        public PagedAndFilteredInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}