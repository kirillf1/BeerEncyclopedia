using BeerEncyclopedia.Application.Contracts.Countries;
using BeerEncyclopedia.Application.Contracts.Manufacturers;
using BeerEncyclopedia.Application.ManufacturerServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BeerEncyclopedia.UI.Shared.Manufacturers
{
    public partial class ManufacturerSelect
    {
        static readonly Dictionary<string, string> CountryImages = new (StringComparer.OrdinalIgnoreCase);
        static ManufacturerSelect()
        {
            CountryImages["Россия"] = "https://upload.wikimedia.org/wikipedia/en/thumb/f/f3/Flag_of_Russia.svg/250px-Flag_of_Russia.svg.png";
            CountryImages["Unknown"] = "https://static.wikia.nocookie.net/kongregate/images/9/96/Unknown_flag.png/revision/latest?cb=20100825093317";
        }
        [Parameter]
        public Action<ManufacturerLabel>? ManufacturerSelected { get; set; }
        [Parameter]
        public Action<IEnumerable<ManufacturerLabel>>? ManufacturersSelected { get; set; }
        [Parameter]
        public bool EnableMultiSelect { get; set; } = true;
        [Parameter]
        public List<Guid> ManufacturerIds
        {
            get => _manufacturerIds;
            set
            {
                if (MudSelect is not null && _manufacturerIds != value)
                {
                    MudSelect.SelectedValues = Manufacturers.Where(m=> value.Contains(m.Id));
                    _manufacturerIds = value;
                }
            }
        }
        private List<Guid> _manufacturerIds = new();
        [Inject]
        private IManufacturerSearchService ManufacturerSearchService { get; set; } = default!;
        private List<ManufacturerLabel> Manufacturers { get; set; } = new();
        private MudSelect<ManufacturerLabel>? MudSelect { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            var result = await ManufacturerSearchService.SearchManufacturerLabel(
                new ManufacturerQuery { PageSize = int.MaxValue }, default);
            if (result.IsSuccess)
            {
                Manufacturers.AddRange(result.Value.Data);
                MudSelect.SelectedValues = Manufacturers.Where(m => ManufacturerIds.Contains(m.Id));
            }

        }
        private static string GetCountryImage(CountryDto countryDto)
        {
            if(CountryImages.TryGetValue(countryDto.Name,out var image))
            {
                return image;
            }
            return CountryImages["Unknown"];
        }
        private void SelectedItems(IEnumerable<ManufacturerLabel> selectItems)
        {
            ManufacturerIds.Clear();
            ManufacturerIds.AddRange(selectItems.Select(c => c.Id));
            ManufacturersSelected?.Invoke(selectItems);
        }
        private void SelectedItem(ManufacturerLabel selectItem)
        {
            ManufacturerIds.Clear();
            ManufacturerIds.Add(selectItem.Id);
            ManufacturerSelected?.Invoke(selectItem);
        }
    }
}
