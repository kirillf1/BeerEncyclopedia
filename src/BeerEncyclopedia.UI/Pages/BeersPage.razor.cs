using BeerEncyclopedia.Application.BeersServices.SearchServices;
using BeerEncyclopedia.Application.Contracts.Beers;
using Microsoft.AspNetCore.Components;

namespace BeerEncyclopedia.UI.Pages
{
    public partial class BeersPage
    {
        [Inject]
        private IBeerSearchByQueryService BeerSearchService { get; set; } = default!;
        public List<BeerLabel> Beers { get; set; } = new List<BeerLabel>();
        private BeersQuery BeersQuery { get; set; } = new BeersQuery();
        private int PageCount { get; set; }
        private bool IsLoading { get; set; } = false;
        private bool isDrawerOpened;
        protected override async Task OnInitializedAsync()
        {
            await LoadBeers(1);
        }
        private Task RefreshQuery()
        {
            BeersQuery = new BeersQuery();
            return Task.CompletedTask;
        }

        private async Task LoadBeers(int page)
        {
            try
            {
                IsLoading = true;
                BeersQuery.PageIndex = page - 1;
                var apiResult = await BeerSearchService.SearchBeerLabels(BeersQuery, default);
                if (apiResult.IsSuccess)
                {
                    PageCount = apiResult.Value.TotalPages;
                    Beers.Clear();
                    Beers.AddRange(apiResult.Value.Data);
                }
            }
            finally
            {
                IsLoading = false;
                isDrawerOpened = false;
                StateHasChanged();
            }
        }
    }
}
