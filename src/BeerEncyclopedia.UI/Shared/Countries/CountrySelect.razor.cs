using BeerEncyclopedia.Application.Contracts.Countries;
using BeerEncyclopedia.Application.CountryServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BeerEncyclopedia.UI.Shared.Countries
{
    public partial class CountrySelect
    {
        [Parameter]
        public bool EnableMultiSelect { get; set; }
        [Parameter]
        public List<Guid> CountryIds
        {
            get => countryIds;
            set
            {
                if (MudSelect is not null && countryIds != value)
                {
                    MudSelect.SelectedValues = Countries.Where(c=> value.Contains(c.Id));
                    countryIds = value;
                }
            }
        }
        [Parameter]
        public Action<CountryDto>? CountrySelected { get; set; }
        [Parameter]
        public Action<IEnumerable<CountryDto>>? CountriesSelected { get; set; }
        private List<CountryDto> Countries { get; set; } = new();
        private List<Guid> countryIds = new();
        [Inject]
        private ICountrySearchService CountrySearchService { get; set; } = default!;
        private MudSelect<CountryDto>? MudSelect { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var result = await CountrySearchService.GetAllCountries();
            if (result.IsSuccess)
            {
                Countries.AddRange(result.Value);
                if(CountryIds.Any())
                    MudSelect.SelectedValues = Countries.Where(c => CountryIds.Contains(c.Id));
            }
        }
        private void SelectedItems(IEnumerable<CountryDto> selectItems)
        {
            CountryIds?.Clear();
            CountryIds?.AddRange(selectItems.Select(c => c.Id));
            CountriesSelected?.Invoke(selectItems);
        }
        private void SelectedItem(CountryDto selectItem)
        {
            CountryIds?.Clear();
            CountryIds?.Add(selectItem.Id);
            CountrySelected?.Invoke(selectItem);
        }
    }
}
