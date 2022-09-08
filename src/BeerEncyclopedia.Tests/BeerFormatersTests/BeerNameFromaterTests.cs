using BeerFormaters.BeerNameFormater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerEncyclopedia.Tests.BeerFormatersTests
{
    public class BeerNameFormaterTests
    {
        [Theory]
        [InlineData("Пиво Schlitz Вайсбир светлое пастеризованное нефильтрованное 5.2%", "Schlitz Вайсбир")]
        [InlineData("Пиво светлое ЖИГУЛЕВСКОЕ Оригинальное Высшая проба фильтрованное 4%, Россия,", "ЖИГУЛЕВСКОЕ Оригинальное Высшая проба")]
        [InlineData("нефильтрованное Пиво темное GUINNESS стаут железная банка 10%", "GUINNESS стаут")]
        [InlineData("Пиво Bud светлое пастеризованное 5%, 440мл", "Bud")]
        [InlineData("Пиво Sint Gummarus Dubbel темное фильтрованное непастеризованное 0,75 л", "Sint Gummarus Dubbel")]
        [InlineData("Пиво Leffe Ruby красное 5%, 0,33л", "Leffe Ruby")]
        [InlineData("Пиво SWEETWATER Ipa стекло, 0,355л", "SWEETWATER Ipa")]
        [InlineData("Пиво Балтика №3 Классическое светлое 4.8%, 450мл", "Балтика №3 Классическое")]
        public void FormatBeerName_UnfomatedName_ShouldRemainOnlyName(string unfomated,string formated)
        {
            IBeerNameFormater formater = new BeerNameFormater();
            
            var result = formater.Format(unfomated);

            Assert.Equal(result.Name, formated);
        }
    }
}
