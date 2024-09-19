using Currency;
using Newtonsoft.Json;
using Utilities;

namespace Saving.Proxies
{
    public class CurrencySaveProxy : ASaveProxy
    {
        private const string CurrencyVariableName = "_amount";
        
        public override void Read(string json) => CurrencyVariableName.SetValue(FindObjectOfType<CurrencyHolder>(), JsonConvert.DeserializeObject<int>(json));

        public override string Write() => JsonConvert.SerializeObject(CurrencyVariableName.GetValue<CurrencyHolder, int>(FindObjectOfType<CurrencyHolder>()));
    }
}