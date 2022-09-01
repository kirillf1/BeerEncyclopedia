using System.Globalization;
using System.Text.RegularExpressions;

namespace BeerFormaters.BeerNameFormater
{
    public static class NameRusFormaterHelper
    {
        private const string NumberPattern = @"\d+([.,][0-9]{1,3})?";
        private const string ColorsPattern = @"\b(светлое|темное|коричневое|янтарное|красное)\b";
        private const string PopularCountriesPattern = @"\b(россия|германия|чехия|сша|бельгия|англия)\b";
        public static bool TryReplaceVolume(out double volume, ref string name)
        {
            volume = 0;
            var matches = Regex.Matches(name, NumberPattern + @"\s*(мл|л|млр|l|ml|mlr)", RegexOptions.IgnoreCase);
            if (!matches.Any())
                return false;
            foreach (var match in matches.Select(c => c.Value))
            {
                name = name.Replace(match, "");
            }
            return double.TryParse(Regex.Match(matches.First().Value, NumberPattern).Value,
                    NumberStyles.Any, CultureInfo.InvariantCulture, out volume);
        }
        public static bool TryRetriveColor(out string? color, string name)
        {
            color = null;
            var match = Regex.Match(name, ColorsPattern, RegexOptions.IgnoreCase);
            if (!match.Success)
                return false;
            color = match.Value;
            return true;
        }
        public static bool TryRetrivePasteurization(out string? pasteurization, string name)
        {
            pasteurization = null;
            var match = Regex.Match(name, "(не)?пастеризованн(ое|ая)", RegexOptions.IgnoreCase);
            if (match.Success)
                pasteurization = match.Value;
            return match.Success;
        }
        public static bool TryRetriveFiltration(out string? filtration, string name)
        {
            filtration = null;
            var match = Regex.Match(name, "(не)?фильтрованн(ое|ая)", RegexOptions.IgnoreCase);
            if (match.Success)
                filtration = match.Value;
            return match.Success;
        }
        public static bool TryRetriveCountry(out string? country, string name)
        {
            country = null;
            var match = Regex.Match(name, PopularCountriesPattern, RegexOptions.IgnoreCase);
            if (match.Success)
                country = match.Value;
            return match.Success;
        }
        public static bool TryReplaceStrenght(out double? strenght, ref string name)
        {
            strenght = null;
            var matches = Regex.Matches(name, NumberPattern + @"\s*%", RegexOptions.IgnoreCase);
            if (!matches.Any())
                return false;
            foreach (var match in matches.Select(c => c.Value))
            {
                name = name.Replace(match, "");
            }
            if( double.TryParse(Regex.Match(matches.First().Value, NumberPattern).Value,
                    NumberStyles.Any, CultureInfo.InvariantCulture, out var newStrenght))
            {
                strenght = newStrenght;
                return true;
            }
            return false;      
        }
        public static string ReplaceExtraText(string name)
        {
            var nameParts = name.Split(' ');
            for (int i = 0; i < nameParts.Length; i++)
            {
                if (nameParts[i].Length < 3)
                    continue;
                if (Regex.IsMatch(nameParts[i], @"\b(пиво|пивной|(напиток|нап)|in|can|в|жестяной|железн(ой|ая)|банк(е|а)|" +
                    @"ж/б|безалкогольное|стекло|неосветленное)\b", RegexOptions.IgnoreCase))
                    nameParts[i] = "";
            }
            return string.Join(' ', nameParts.Where(c=> !string.IsNullOrWhiteSpace(c)));
        }
    }
}

