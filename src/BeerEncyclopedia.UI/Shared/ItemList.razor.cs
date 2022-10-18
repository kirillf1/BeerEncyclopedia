using Microsoft.AspNetCore.Components;

namespace BeerEncyclopedia.UI.Shared
{
    public partial class ItemList<TItem>
    {

        [Parameter]
        public RenderFragment<TItem> RowTemplate { get; set; } = default!;
        [Parameter]
        public IEnumerable<TItem> Items { get; set; } = default!;
        [Parameter]
        public bool IsLoading { get; set; }
    }
}
