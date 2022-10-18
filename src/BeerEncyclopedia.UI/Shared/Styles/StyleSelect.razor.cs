using BeerEncyclopedia.Application.Contracts.Styles;
using BeerEncyclopedia.Application.StyleServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq;

namespace BeerEncyclopedia.UI.Shared.Styles
{
    public partial class StyleSelect
    {
        [Parameter]
        public Action<StyleLabel>? StyleSelected { get; set; }
        [Parameter]
        public Action<IEnumerable<StyleLabel>>? StylesSelected { get; set; }
        [Parameter]
        public bool EnableMultiSelect { get; set; } = true;
        [Parameter]
        public List<Guid>? StyleIds
        {
            get => stylesIds;
            set
            {
                if (MudSelect is not null && value !=null && stylesIds != value)
                {
                    MudSelect.SelectedValues = Styles.Where(s => value.Contains(s.Id));
                    stylesIds = value;
                }
            }
        }
        private List<Guid>? stylesIds = new();
        [Inject]
        private IStyleSearchService StyleSearchService { get; set; } = default!;
        private List<StyleLabel> Styles { get; set; } = new();
        private MudSelect<StyleLabel>? MudSelect { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var result = await StyleSearchService.SearchStyleLabel(
                new StyleQuery { PageSize = int.MaxValue }, default);
            if (result.IsSuccess)
            {
                
                Styles.AddRange(result.Value.Data);
                if (StyleIds != null)
                    MudSelect.SelectedValues = Styles.Where(s => StyleIds.Contains(s.Id));
            }
        }
        private void SelectedItems(IEnumerable<StyleLabel> selectItems)
        {
            StyleIds?.Clear();
            StylesSelected?.Invoke(selectItems);
            StyleIds?.AddRange(selectItems.Select(c => c.Id));
        }
        private void SelectedItem(StyleLabel selectItem)
        {
            StyleIds?.Clear();
            StyleIds?.Add(selectItem.Id);
            StyleSelected?.Invoke(selectItem);
        }
    }
}
