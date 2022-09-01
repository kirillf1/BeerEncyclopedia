using BeerFormaters.BeerNameFormater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerEncyclopedia.Tests.BeerFormatersTests
{
    public class BeerFormaterHelperTests
    {
        [Theory]
        [InlineData("Пиво Schlitz Вайсбир светлое пастеризованное нефильтрованное 5.2%, 430мл")]
        [InlineData("Пиво светлое ЖИГУЛЕВСКОЕ Оригинальное Высшая проба пастеризованное 4%, 1.2л, Россия, 1.2 L")]
        [InlineData("Пиво темное GUINNESS стаут железная банка, 0,44л")]
        public void TryReplaceVolume_WithVolume_ReturnTrueAndReplaceName(string beerName)
        {
            string beerNameBefore = beerName;

            var result = NameRusFormaterHelper.TryReplaceVolume(out var volume, ref beerName);

            Assert.True(result);
            Assert.True(volume > 0);
            Assert.True(beerNameBefore.Length > beerName.Length);
        }
        [Theory]
        [InlineData("Пиво Schlitz Вайсбир светлое пастеризованное нефильтрованное 5.2%")]
        [InlineData("Пиво светлое ЖИГУЛЕВСКОЕ Оригинальное Высшая проба пастеризованное 4%, Россия,")]
        [InlineData("Пиво темное GUINNESS стаут железная банка")]
        public void TryReplaceVolume_WithoutVolume_ReturnFalse(string beerName)
        {
            string beerNameBefore = beerName;

            var result = NameRusFormaterHelper.TryReplaceVolume(out var volume, ref beerName);

            Assert.False(result);
            Assert.True(volume == 0);
            Assert.True(beerNameBefore.Length == beerName.Length);
        }
        [Theory]
        [InlineData("Пиво Schlitz Вайсбир светлое пастеризованное нефильтрованное 5.2%")]
        [InlineData("Пиво светлое ЖИГУЛЕВСКОЕ Оригинальное Высшая проба пастеризованное 4%, Россия,")]
        [InlineData("Пиво темное GUINNESS стаут железная банка")]
        public void TryRetriveColor_WithColor_ReturnTrue(string beerName)
        {
            var result = NameRusFormaterHelper.TryRetriveColor(out var color,  beerName);

            Assert.True(result);
            Assert.True(color != null);
        }
        [Theory]
        [InlineData("Пиво Schlitz Вайсбир светлое пастеризованное нефильтрованное 5.2%")]
        [InlineData("Пиво светлое ЖИГУЛЕВСКОЕ Оригинальное Высшая проба непастеризованное 4%, Россия,")]
        [InlineData("непастеризованное Пиво темное GUINNESS стаут железная банка")]
        public void TryRetrivePasteurization_ContainsPasteurization_ReturnTrue(string beerName)
        {
            var result = NameRusFormaterHelper.TryRetrivePasteurization(out var pasteurization, beerName);

            Assert.True(result);
            Assert.True(pasteurization!= null);
        }
        [Theory]
        [InlineData("Пиво Schlitz Вайсбир светлое пастеризованное нефильтрованное 5.2%")]
        [InlineData("Пиво светлое ЖИГУЛЕВСКОЕ Оригинальное Высшая проба фильтрованное 4%, Россия,")]
        [InlineData("нефильтрованное Пиво темное GUINNESS стаут железная банка")]
        public void TryRetriveFiltration_ContainsFiltration_ReturnTrue(string beerName)
        {
            var result = NameRusFormaterHelper.TryRetriveFiltration(out var pasteurization, beerName);

            Assert.True(result);
            Assert.True(pasteurization != null);
        }
        [Theory]
        [InlineData("Пиво Schlitz Вайсбир светлое пастеризованное нефильтрованное 5.2%")]
        [InlineData("Пиво светлое ЖИГУЛЕВСКОЕ Оригинальное Высшая проба фильтрованное 4%, Россия,")]
        [InlineData("нефильтрованное Пиво темное GUINNESS стаут железная банка 10%")]
        public void TryReplaceStrenght_CountainsStrenght_ReturnTrueAndReplace(string beerName)
        {
            string beerNameBefore = beerName;

            var result = NameRusFormaterHelper.TryReplaceStrenght(out var strenght, ref beerName);

            Assert.True(result);
            Assert.True(strenght > 0);
            Assert.True(beerNameBefore.Length > beerName.Length);
        }
        [Fact]
        public void TryReplaceStrenght_StrenghtNotExists_ReturnFalse()
        {
            var beerName = "Пиво неизвестно%";
            
            var result = NameRusFormaterHelper.TryReplaceStrenght(out var strenght, ref beerName);

            Assert.False(result);
            Assert.Null(strenght);
        }
    }
}
