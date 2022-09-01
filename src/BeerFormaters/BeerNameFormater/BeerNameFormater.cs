using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BeerFormaters.BeerNameFormater
{
    public class BeerNameFormater : IBeerNameFormater
    {
        public FormatNameResult Format(string rawName)
        {
            bool? filtration = null;
            if(NameRusFormaterHelper.TryRetriveFiltration(out var filtrationString, rawName))
            {
                rawName = rawName.Replace(filtrationString!,"");
                filtration = !filtrationString!.StartsWith("не",StringComparison.InvariantCultureIgnoreCase);
            }
            NameRusFormaterHelper.TryReplaceStrenght(out var strenght, ref rawName);
            NameRusFormaterHelper.TryReplaceVolume(out var volume, ref rawName);
            if(NameRusFormaterHelper.TryRetriveColor(out var color, rawName))
                rawName = rawName.Replace(color!, "");
            if(NameRusFormaterHelper.TryRetriveCountry(out var country, rawName))
                rawName = rawName.Replace(country!, "");
            bool? pasteurization = null;
            if (NameRusFormaterHelper.TryRetrivePasteurization(out var pasterString, rawName))
            {
                rawName = rawName.Replace(pasterString!, "");
                pasteurization = !pasterString!.StartsWith("не", StringComparison.InvariantCultureIgnoreCase);
            }
            rawName = NameRusFormaterHelper.ReplaceExtraText(Regex.Replace(rawName, "[\",.]", ""));
            return new FormatNameResult(rawName)
            {
                Color = color,
                Filtration = filtration,
                Pasteurization = pasteurization,
                Volume = volume,
                Strenght = strenght,
                Country = country                
            };
        }
    }
}
