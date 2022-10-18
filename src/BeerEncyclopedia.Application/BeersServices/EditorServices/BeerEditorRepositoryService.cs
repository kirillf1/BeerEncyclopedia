using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Application.Specifications.Beers;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.BeersServices.EditorServices
{
    public class BeerEditorRepositoryService : IBeerEditorService
    {
        private readonly IRepository<Beer> beerRepository;
        private readonly IRepository<Country> countryRepository;
        private readonly IRepository<Color> colorRepository;
        private readonly IRepository<Manufacturer> manufacturerRepository;
        private readonly IRepository<Style> stylesRepository;

        public BeerEditorRepositoryService(IRepository<Beer> beerRepository,
            IRepository<Country> countryRepository, IRepository<Color> colorRepository,
            IRepository<Manufacturer> manufacturerRepository, IRepository<Style> stylesRepository)
        {
            this.beerRepository = beerRepository;
            this.countryRepository = countryRepository;
            this.colorRepository = colorRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.stylesRepository = stylesRepository;
        }

        public async Task<Result> AddBeer(BeerDetails beerDetails, CancellationToken cancellationToken = default)
        {
            try
            {
                var newBeer = new Beer(beerDetails.Id, beerDetails.Name, beerDetails.MainImageUrl,
                    new ChemicalIndicators(default, default, default, default),
                    new OrganolepticIndicators(new Color(default,"unknown"), "", default))
                { Country = new Country(default,"unknonw","uw")};
                await InsertValues(newBeer, beerDetails);
                await beerRepository.AddAsync(newBeer, cancellationToken);
                await beerRepository.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result> RemoveBeer(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var beer = await beerRepository.GetByIdAsync(id, cancellationToken);
                if (beer is null)
                    return Result.NotFound();
                await beerRepository.DeleteAsync(beer, cancellationToken);
                await beerRepository.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result> UpdateBeer(BeerDetails beerDetails, CancellationToken cancellationToken = default)
        {
            try
            {
                var spec = new BeerByIdSpec(beerDetails.Id);
                var beer = await beerRepository.FirstOrDefaultAsync(spec, cancellationToken);
                if (beer is null)
                    return Result.NotFound();
                await InsertValues(beer, beerDetails);
                await beerRepository.UpdateAsync(beer);
                await beerRepository.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        private async Task InsertValues(Beer beer, BeerDetails beerDetails)
        {
            beer.Name = beerDetails.Name;
            beer.Description = beerDetails.Description;
            if (DateOnly.TryParse(beerDetails.CreationTime, out var creationTime))
                beer.CreationTime = creationTime;
            beer.AltName = beerDetails.AltName;
            beer.BeerImages.MainImageUrl = beerDetails.MainImageUrl;
            beer.BeerImages.ImageUrls = beerDetails.AdditionalImages;
            beer.Rating = beerDetails.Rating;
            beer.OrganolepticIndicators.Bitterness = beerDetails.Bitterness;
            if (beerDetails.Color.Id != default && beer.OrganolepticIndicators.Color.Id != beerDetails.Color.Id)
                beer.OrganolepticIndicators.Color = await colorRepository.GetByIdAsync(beerDetails.Color.Id)
                    ?? throw new ArgumentNullException("Color is not exist in repository");
            beer.OrganolepticIndicators.Taste = beerDetails.Taste ?? "unknown";
            beer.ChemicalIndicators.InitialWort = beerDetails.InitialWort;
            beer.ChemicalIndicators.Pasteurization = beerDetails.Pasteurization;
            beer.ChemicalIndicators.Filtration = beerDetails.Filtration;
            if (beerDetails.Country.Id != default && beer.Country.Id != beerDetails.Country.Id)
                beer.Country = await countryRepository.GetByIdAsync(beerDetails.Country.Id)
                     ?? throw new ArgumentNullException("Country is not exist in repository");
            await UpdateManufacturers(beer.Manufacturers, beerDetails.Manufacturers.Select(c => c.Id));
            await UpdateStyles(beer.Styles, beerDetails.Styles.Select(c => c.Id));
        }
        private async Task UpdateManufacturers(List<Manufacturer> manufacturers, IEnumerable<Guid> ids)
        {
            manufacturers.RemoveAll(m => !ids.Contains(m.Id));
            foreach (var id in ids)
            {
                if (!manufacturers.Any(c => c.Id == id))
                    manufacturers.Add(await manufacturerRepository.GetByIdAsync(id)
                        ?? throw new ArgumentNullException("Manufacturer is not exist in repository"));
            }
        }
        private async Task UpdateStyles(List<Style> styles, IEnumerable<Guid> ids)
        {
            styles.RemoveAll(m => !ids.Contains(m.Id));
            foreach (var id in ids)
            {
                if (!styles.Any(c => c.Id == id))
                    styles.Add(await stylesRepository.GetByIdAsync(id)
                        ?? throw new ArgumentNullException("Styles is not exist in repository"));
            }
        }
    }
}
