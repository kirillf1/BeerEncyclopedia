using BeerEncyclopedia.Application.Contracts.Manufacturers;
using BeerEncyclopedia.Application.ManufacturerServices;
using Microsoft.AspNetCore.Components;

namespace BeerEncyclopedia.UI.Pages
{
    public partial class ManufacturersPage
    {
        [Inject]
        private IManufacturerSearchService manufacturerSearchService { get; set; } = default!;
        private bool IsLoading { get; set; }
        private bool isDrawerOpened;
        private ManufacturerQuery ManufacturerQuery { get; set; } = new();
        private int PageCount;
        private List<ManufacturerLabel> Manufacturers { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await LoadManufacturers(1);
        }
        private Task RefreshQuery()
        {
            ManufacturerQuery = new();
            return Task.CompletedTask;
        }
        private async Task LoadManufacturers(int page)
        {
            try
            {
                IsLoading = true;
                ManufacturerQuery.PageIndex = page - 1;
                var apiResult = await manufacturerSearchService.SearchManufacturerLabel(ManufacturerQuery, default);
                if (apiResult.IsSuccess)
                {
                    PageCount = apiResult.Value.TotalPages;
                    Manufacturers.Clear();
                    Manufacturers.AddRange(apiResult.Value.Data);
                }
            }
            finally
            {
                isDrawerOpened = false;
                IsLoading = false;
                StateHasChanged();
            }
        }
    }
}
